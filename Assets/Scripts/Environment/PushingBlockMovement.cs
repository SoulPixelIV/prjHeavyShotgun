using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingBlockMovement : MonoBehaviour
{
    public float travelSpeed;

    int dir = 1;

    void Update()
    {
        if (dir == 0)
        {
            gameObject.GetComponent<Rigidbody>().MovePosition(transform.position + transform.right * travelSpeed * Time.deltaTime);
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().MovePosition(transform.position + -transform.right * travelSpeed * Time.deltaTime);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PushLimit")
        {
            if (dir == 0)
            {
                dir = 1;
            }
            else
            {
                dir = 0;
            }
        }
    }
}
