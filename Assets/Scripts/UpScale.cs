using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpScale : MonoBehaviour
{
    public float scaleLimit;
    public float scaleSpeed;

    void Update()
    {
        if (transform.localScale.y < scaleLimit)
        {
            transform.localScale += new Vector3(0, Time.deltaTime * scaleSpeed, 0);
        }
    }
}
