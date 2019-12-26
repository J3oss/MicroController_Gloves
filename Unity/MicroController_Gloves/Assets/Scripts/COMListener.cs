using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class COMListener : MonoBehaviour
{
    static bool _continue;
    static SerialPort _serialPort;

    public Transform gauntlet;

    // Start is called before the first frame update
    void Start()
    {
        string message;
        StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;

        // Create a new SerialPort object with default settings.
        _serialPort = new SerialPort();

        // Allow the user to set the appropriate properties.
        _serialPort.PortName = SetPortName(_serialPort.PortName);
        _serialPort.BaudRate = SetPortBaudRate(_serialPort.BaudRate);
        _serialPort.Parity = SetPortParity(_serialPort.Parity);
        _serialPort.DataBits = SetPortDataBits(_serialPort.DataBits);
        _serialPort.StopBits = SetPortStopBits(_serialPort.StopBits);
        _serialPort.Handshake = SetPortHandshake(_serialPort.Handshake);

        // Set the read/write timeouts
        _serialPort.ReadTimeout = 500;
        _serialPort.WriteTimeout = 500;

        _serialPort.Open();
        _continue = true;


    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            string[] message_data = _serialPort.ReadLine().Split('/');
            float[] data = new float[message_data.Length];
            for(int i = 0; i < message_data.Length; i++)
            {
                data[i] = float.Parse(message_data[i]);
            }
            Debug.Log("Pitch = " + data[0]);
            Debug.Log("Roll  = " + data[1]);
            Debug.Log("Yaw   = " + data[2]);
            gauntlet.localEulerAngles = new Vector3(
                Math.Abs(data[1]) % 255,
                Math.Abs(data[0]) % 255,
                Math.Abs(data[2]) % 255
                );
        }
        catch (TimeoutException) { }
    }

    void OnDestroy()
    {
        _serialPort.Close();
    }


    // Display Port values and prompt user to enter a port.
    public static string SetPortName(string defaultPortName)
    {
        string portName;

        Debug.Log("Available Ports:");
        foreach (string s in SerialPort.GetPortNames())
        {
            Debug.Log(s);
        }

        Debug.LogFormat("Enter COM port value (Default: {0}): ", defaultPortName);
        portName = "COM3";

        if (portName == "" || !(portName.ToLower()).StartsWith("com"))
        {
            portName = defaultPortName;
        }
        return portName;
    }
    // Display BaudRate values and prompt user to enter a value.
    public static int SetPortBaudRate(int defaultPortBaudRate)
    {
        string baudRate;

        Debug.LogFormat("Baud Rate(default:{0}): ", defaultPortBaudRate);
        baudRate = "115200";

        if (baudRate == "")
        {
            baudRate = defaultPortBaudRate.ToString();
        }

        return int.Parse(baudRate);
    }

    // Display PortParity values and prompt user to enter a value.
    public static Parity SetPortParity(Parity defaultPortParity)
    {
        string parity;

        Debug.LogFormat("Available Parity options:");
        foreach (string s in Enum.GetNames(typeof(Parity)))
        {
            Debug.LogFormat("   {0}", s);
        }

        Debug.LogFormat("Enter Parity value (Default: {0}):", defaultPortParity.ToString(), true);
        parity = "";// Console.ReadLine();

        if (parity == "")
        {
            parity = defaultPortParity.ToString();
        }

        return (Parity)Enum.Parse(typeof(Parity), parity, true);
    }
    // Display DataBits values and prompt user to enter a value.
    public static int SetPortDataBits(int defaultPortDataBits)
    {
        string dataBits;

        Debug.LogFormat("Enter DataBits value (Default: {0}): ", defaultPortDataBits);
        dataBits = "8";// Console.ReadLine();

        if (dataBits == "")
        {
            dataBits = defaultPortDataBits.ToString();
        }

        return int.Parse(dataBits.ToUpperInvariant());
    }

    // Display StopBits values and prompt user to enter a value.
    public static StopBits SetPortStopBits(StopBits defaultPortStopBits)
    {
        string stopBits;

        Debug.LogFormat("Available StopBits options:");
        foreach (string s in Enum.GetNames(typeof(StopBits)))
        {
            Debug.LogFormat("   {0}", s);
        }

        Debug.LogFormat("Enter StopBits value (None is not supported and \n" +
         "raises an ArgumentOutOfRangeException. \n (Default: {0}):", defaultPortStopBits.ToString());
        stopBits = "One";//Console.ReadLine();

        if (stopBits == "")
        {
            stopBits = defaultPortStopBits.ToString();
        }

        return (StopBits)Enum.Parse(typeof(StopBits), stopBits, true);
    }
    public static Handshake SetPortHandshake(Handshake defaultPortHandshake)
    {
        string handshake;

        Debug.LogFormat("Available Handshake options:");
        foreach (string s in Enum.GetNames(typeof(Handshake)))
        {
            Debug.LogFormat("   {0}", s);
        }

        Debug.LogFormat("Enter Handshake value (Default: {0}):", defaultPortHandshake.ToString());
        handshake = "None";//Console.ReadLine();

        if (handshake == "")
        {
            handshake = defaultPortHandshake.ToString();
        }

        return (Handshake)Enum.Parse(typeof(Handshake), handshake, true);
    }
}

