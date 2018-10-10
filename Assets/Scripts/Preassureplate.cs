using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preassureplate : MonoBehaviour {

    public GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            gameObject.GetComponent<AudioSource>().Play();
            if (target.gameObject.tag == "ArrowShooter")
            {               
                target.GetComponent<ArrowShooter>().activated = true;
            }
            if (target.gameObject.tag == "Gate")
            {
                target.GetComponent<GateOpen>().activated = true;
            }
        }
    }
}
