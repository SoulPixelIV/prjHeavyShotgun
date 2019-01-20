using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftMovement : MonoBehaviour {

    public float travelHeight;
    public float travelSpeed;
    public bool active;

    int dir;

    void Start()
    {
        if (active)
        {
            InvokeRepeating("CallLift", travelHeight, travelHeight);
        }
    }

    void FixedUpdate ()
    {  
        if (active)
        {
            if (dir == 0)
            {
                gameObject.GetComponent<Rigidbody>().MovePosition((transform.position + transform.up * Time.deltaTime));
            }
            else
            {
                gameObject.GetComponent<Rigidbody>().MovePosition((transform.position + -transform.up * Time.deltaTime));
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
}
