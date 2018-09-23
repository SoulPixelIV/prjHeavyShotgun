using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponSwitching>().shotgunUnlocked == true)
        {
            GameObject.FindGameObjectWithTag("Shotgun").GetComponent<ShootingBehaviour>().magazines += 6;
            Destroy(gameObject);
        }
    }
}
