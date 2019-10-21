using UnityEngine;

public class FingerControl : MonoBehaviour
{
    [Range(0f, 1.0f)]
    public float flex = 0f;

    public float top_factor;
    public float mid_factor;
    public float bas_factor;

//    public float top_max_angle;
//    public float mid_max_angle;
//    public float bas_max_angle;
    public Vector3 top_target_angle;
    public Vector3 mid_target_angle;
    public Vector3 bas_target_angle;
//    public Vector3 top_target_position;
//    public Vector3 mid_target_position;
//    public Vector3 bas_target_position;

    public GameObject finger_mid;
    public GameObject finger_top;

    Transform mid_transform;
    Transform top_transform;
    Transform bas_transform;

    Vector3 bas_init_angle;
    Vector3 mid_init_angle;
    Vector3 top_init_angle;
    Vector3 bas_init_position;
    Vector3 mid_init_position;
    Vector3 top_init_position;

    // Start is called before the first frame update
    void Start()
    {
        bas_transform = transform;
        mid_transform = finger_mid.transform;
        top_transform = finger_top.transform;

//        bas_init_position = bas_transform.position;
//        mid_init_position = mid_transform.position;
//        top_init_position = top_transform.position;
        bas_init_angle = bas_transform.eulerAngles;
        mid_init_angle = mid_transform.eulerAngles;
        top_init_angle = top_transform.eulerAngles;

    }

    void Update()
    {
//	    bas_transform.localRotation = Quaternion.Euler( bas_init_angle + (bas_target_angle - bas_init_angle) * flex * bas_factor );
			    // Vector3.Lerp(bas_init_angle, bas_target_angle, flex * bas_factor));
//        bas_transform.localRotation = Quaternion.Euler(Vector3.Lerp(bas_init_angle, bas_init_angle + bas_target_angle, flex * bas_factor));
//        mid_transform.localRotation = Quaternion.Euler(Vector3.Lerp(mid_init_angle, mid_init_angle + mid_target_angle, flex * mid_factor));
//        top_transform.localRotation = Quaternion.Euler(Vector3.Lerp(top_init_angle, top_init_angle + top_target_angle, flex * top_factor));
        bas_transform.eulerAngles = Vector3.Lerp(bas_init_angle, bas_target_angle, flex * bas_factor);
        mid_transform.eulerAngles = Vector3.Lerp(mid_init_angle, mid_target_angle, flex * mid_factor);
        top_transform.eulerAngles = Vector3.Lerp(top_init_angle, top_target_angle, flex * top_factor);
//        bas_transform.position    = Vector3.Lerp(bas_init_position, bas_target_position, flex * bas_factor);
//        mid_transform.position    = Vector3.Lerp(mid_init_position, mid_target_position, flex * mid_factor);
//        top_transform.position    = Vector3.Lerp(top_init_position, top_target_position, flex * top_factor);
    }
}
