﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

    public GameObject destroyedVersion;
    public bool marked;
    public bool explosive;
    public bool explosiveOnContact = true;
    public GameObject explosionHitbox;
    public Material standardMat;

    private void Start()
    {
        if (marked)
        {
            GetComponent<Renderer>().material = standardMat;
        }
    }

    //Damage
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hitbox")
        {
            if (gameObject.GetComponent<Destructible>() != null)
            {
                gameObject.GetComponent<Destructible>().Destroy();
            }
        }
    }
    //Falldamage
    void OnCollisionEnter(Collision collision)
    {
        if (GetComponent<Rigidbody>() != null && explosiveOnContact)
        {
            if (GetComponent<Rigidbody>().velocity.x > 3 || GetComponent<Rigidbody>().velocity.y > 3 || GetComponent<Rigidbody>().velocity.z > 3)
            {
                Destroy();
            }
        }
    }

    public void Destroy () {
        if (explosive)
        {
            Instantiate(explosionHitbox, transform.position, Quaternion.identity);
        }
        if (destroyedVersion != null)
        {
            Instantiate(destroyedVersion, transform.position, transform.rotation);
        }
        gameObject.SetActive(false);
	}
}
