using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightChecking : MonoBehaviour {

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
            transform.parent.gameObject.GetComponent<EnemyAI>().aggro = true;
        }
    }
}
