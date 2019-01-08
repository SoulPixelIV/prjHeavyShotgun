using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftMovement : MonoBehaviour {

    public float travelHeight;
    public float waitTime;
    public bool active;

    float travelHeightSave;
    float waitTimeSave;
    bool waiting;
    int dir;

	void Start () {
        travelHeightSave = travelHeight;
        waitTimeSave = waitTime;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (active)
        {
            travelHeight -= 1 * Time.deltaTime;
        }
        if (travelHeight < 0)
        {
            waiting = true;
        }
        if (waiting)
        {
            waitTime -= 1 * Time.deltaTime;
        }
        if (waitTime < 0)
        {
            waiting = false;
            if (dir == 0)
            {
                dir = 1;
            }
            else
            {
                dir = 0;
            }
            travelHeight = travelHeightSave;
            waitTime = waitTimeSave;
        }
        if (!waiting && active)
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
}
