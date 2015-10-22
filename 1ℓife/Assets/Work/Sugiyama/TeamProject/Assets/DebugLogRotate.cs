using UnityEngine;
using System.Collections;

public class DebugLogRotate : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float floor_angle = -1 * (transform.localEulerAngles.z);
        if (floor_angle <= -180) floor_angle = (360 + floor_angle);
        Debug.Log(floor_angle);
    }
}
