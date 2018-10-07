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
        if (other.tag == "Player")
        {
            if (target.gameObject.tag == "ArrowShooter")
            {
                gameObject.GetComponent<AudioSource>().Play();
                target.GetComponent<ArrowShooter>().activated = true;
            }
        }
    }
}
