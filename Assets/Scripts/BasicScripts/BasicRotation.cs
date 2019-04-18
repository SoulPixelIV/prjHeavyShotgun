using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRotation : MonoBehaviour
{
    public bool rotX;
    public bool rotY;
    public bool rotZ;

    public float rotSpeed;

    void Update()
    {
        if (rotX)
        {
            transform.Rotate(Time.deltaTime * rotSpeed, 0, 0);
        }
        if (rotY)
        {
            transform.Rotate(0, Time.deltaTime * rotSpeed, 0);
        }
        if (rotZ)
        {
            transform.Rotate(0, 0, Time.deltaTime * rotSpeed);
        }
    }
}
