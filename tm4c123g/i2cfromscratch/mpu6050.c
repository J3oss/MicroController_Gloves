/*
 * Copyright (c) 2014 Thomas Roell.  All rights reserved.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to
 * deal with the Software without restriction, including without limitation the
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
 * sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 *  1. Redistributions of source code must retain the above copyright notice,
 *     this list of conditions and the following disclaimers.
 *  2. Redistributions in binary form must reproduce the above copyright
 *     notice, this list of conditions and the following disclaimers in the
 *     documentation and/or other materials provided with the distribution.
 *  3. Neither the name of Thomas Roell, nor the names of its contributors
 *     may be used to endorse or promote products derived from this Software
 *     without specific prior written permission.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE
 * CONTRIBUTORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * WITH THE SOFTWARE.
 */

#include <kernel.h>
#include "kernel_id.h"

#include "lm4f120_i2c.h"

#include "mpu6050.h"

static uint8_t mpu6050_read_register(uint8_t index)
{
    uint8_t wdata[1], rdata[1];

    wdata[0] = index;
    rdata[0] = 0x00;

    lm4f120_i2c_master_transmit(MPU6050_I2C_ADDRESS, &wdata[0], 1, &rdata[0], 1);
    
    return rdata[0];
}

static uint32_t mpu6050_write_register(uint8_t index, uint8_t data)
{
    uint8_t wdata[2];

    wdata[0] = index;
    wdata[1] = data;

    return lm4f120_i2c_master_transmit(MPU6050_I2C_ADDRESS, &wdata[0], 2, NULL, 0);
}

static uint32_t mpu6050_read_data(uint8_t index, uint8_t *data, uint32_t count)
{
    uint8_t wdata[1];

    wdata[0] = index;

    return lm4f120_i2c_master_transmit(MPU6050_I2C_ADDRESS, &wdata[0], 1, data, count);
}

void mpu6050_init(void)
{
    /* First reset the device, and wait till it comes back from
     * sleep mode.
     */

    mpu6050_write_register(MPU6050_RA_PWR_MGMT_1, (MPU6050_DEVICE_RESET | MPU6050_CLKSEL_PLL_ZGYRO));

    do
    {
        dly_tsk(5);
    }
    while (mpu6050_read_register(MPU6050_RA_PWR_MGMT_1) & MPU6050_DEVICE_RESET);

    dly_tsk(25);


    mpu6050_write_register(MPU6050_RA_USER_CTRL, (MPU6050_FIFO_RESET | MPU6050_I2C_MST_RESET | MPU6050_SIG_COND_RESET));

    do
    {
        dly_tsk(5);
    }
    while (mpu6050_read_register(MPU6050_RA_USER_CTRL) & (MPU6050_FIFO_RESET | MPU6050_I2C_MST_RESET | MPU6050_SIG_COND_RESET));

    dly_tsk(25);


    mpu6050_write_register(MPU6050_RA_PWR_MGMT_1, MPU6050_CLKSEL_PLL_ZGYRO);
    mpu6050_write_register(MPU6050_RA_PWR_MGMT_2, 0);

    /* Get rid of all the I2C slave bits.
     */
    mpu6050_write_register(MPU6050_RA_I2C_MST_CTRL,       0);
    mpu6050_write_register(MPU6050_RA_I2C_SLV0_CTRL,      0);
    mpu6050_write_register(MPU6050_RA_I2C_SLV1_CTRL,      0);
    mpu6050_write_register(MPU6050_RA_I2C_SLV2_CTRL,      0);
    mpu6050_write_register(MPU6050_RA_I2C_SLV3_CTRL,      0);
    mpu6050_write_register(MPU6050_RA_I2C_SLV4_CTRL,      0);
    mpu6050_write_register(MPU6050_RA_I2C_MST_DELAY_CTRL, 0);

    /* Setup for 250Hz sample rate, and a 20Hz low pass filter.
     * This results in a 8.5ms delay for accel, and 8.3ms delay for gyro.
     *
     * Really need to experiment with the ranges. What happens after the rover crashes into something ?
     */

    mpu6050_write_register(MPU6050_RA_CONFIG,             (MPU6050_EXT_SYNC_DISABLED | MPU6050_DLPF_BW_20));
    mpu6050_write_register(MPU6050_RA_SMPLRT_DIV,         3);
    mpu6050_write_register(MPU6050_RA_GYRO_CONFIG,        (MPU6050_GYRO_FS_2000));
    mpu6050_write_register(MPU6050_RA_ACCEL_CONFIG,       (MPU6050_ACCEL_FS_2));
    mpu6050_write_register(MPU6050_RA_INT_PIN_CFG,        (MPU6050_INT_LEVEL | MPU6050_INT_RD_CLEAR | MPU6050_I2C_BYPASS_EN));    
    mpu6050_write_register(MPU6050_RA_INT_ENABLE,         (MPU6050_DATA_RDY_EN));
    mpu6050_write_register(MPU6050_RA_FIFO_EN,            0);
}

void mpu6050_event(event_sensor_primary_t *event_sensor_primary)
{
    uint8_t data[14];

     mpu6050_read_data(MPU6050_RA_ACCEL_XOUT_H, data, 14);

    event_sensor_primary->accel_x = (int16_t)(((uint16_t)data[0]  << 8) | (uint16_t)data[1] );
    event_sensor_primary->accel_y = (int16_t)(((uint16_t)data[2]  << 8) | (uint16_t)data[3] );
    event_sensor_primary->accel_z = (int16_t)(((uint16_t)data[4]  << 8) | (uint16_t)data[5] );
    event_sensor_primary->gyro_x  = (int16_t)(((uint16_t)data[8]  << 8) | (uint16_t)data[9] );
    event_sensor_primary->gyro_y  = (int16_t)(((uint16_t)data[10] << 8) | (uint16_t)data[11]);
    event_sensor_primary->gyro_z  = (int16_t)(((uint16_t)data[12] << 8) | (uint16_t)data[13]);

}
