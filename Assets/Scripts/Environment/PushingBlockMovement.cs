using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingBlockMovement : MonoBehaviour
{

    public float travelHeight;
    public float travelSpeed;
    public bool active;

    int dir;
    bool activated;

    public void Start()
    {
        if (active)
        {
            WakeUp();
        }
    }

    void FixedUpdate()
    {
        if (active)
        {
            if (!activated)
            {
                WakeUp();
            }
            if (dir == 0)
            {
                gameObject.GetComponent<Rigidbody>().MovePosition((transform.position + transform.right * Time.deltaTime * travelSpeed));
            }
            else
            {
                gameObject.GetComponent<Rigidbody>().MovePosition((transform.position + -transform.right * Time.deltaTime * travelSpeed));
            }
        }
    }

    void CallLift()
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

    void WakeUp()
    {
        InvokeRepeating("CallLift", travelHeight, travelHeight);
        activated = true;
    }
}

