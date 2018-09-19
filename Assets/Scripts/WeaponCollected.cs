using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollected : MonoBehaviour {

    public int weapon;

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
            if (weapon == 1)
            {
                GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<WeaponSwitching>().WeaponSwitch(1);
            }

            Destroy(gameObject);
        }
    }
}
