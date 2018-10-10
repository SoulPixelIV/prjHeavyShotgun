using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOpen : MonoBehaviour {

    public bool activated;

    bool open;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (activated && !open)
        {
            GetComponent<Animator>().Play("gateOpen");
            open = true;
        }
	}
}
