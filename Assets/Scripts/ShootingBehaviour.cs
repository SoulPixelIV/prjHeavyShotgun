﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBehaviour : MonoBehaviour {

    public Rigidbody bullet;
    public int damage;
    public int power;
	
	// Update is called once per frame
	void Update () {
        Animator anim = GetComponent<Animator>();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("shoot"))
            {
                Shoot();
            }
        }
	}

    void Shoot ()
    {
        Animator anim = GetComponent<Animator>();
        RaycastHit hit;

        var direction = Camera.main.transform.TransformDirection(new Vector3(0, 0, 1));
        
        if (Physics.Raycast (Camera.main.transform.position, direction, out hit, 300))
        {
            GameObject objHit = hit.collider.gameObject;
            Debug.Log("Hit object:" + objHit);

            //Environment Hit
            if (objHit.tag == "Environment")
            {
                //Direction between player and object
                Vector3 dir = (transform.position - objHit.transform.position).normalized;

                if (objHit.GetComponent<Destructible>() != null)
                {
                    objHit.GetComponent<Destructible>().Destroy();
                }

                objHit.GetComponent<Rigidbody>().AddForce(-dir * power, ForceMode.Impulse);
            }
            //Enemy Hit
            if (objHit.tag == "Enemy")
            {
                //Direction between player and enemy
                Vector3 dir = (transform.position - objHit.transform.position).normalized;

                if (objHit.GetComponent<HealthSystem>().HealthLoss(damage) == true)
                {
                    objHit.GetComponent<Rigidbody>().AddForce(-dir * power, ForceMode.Impulse);
                }
                else
                {
                    objHit.GetComponent<Rigidbody>().AddForce(-dir * power / 8, ForceMode.Impulse);
                    objHit.GetComponent<HealthSystem>().HealthLoss(damage);
                }
                //Play Hitmarker
                Camera.main.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        
        //Play animation and sound
        anim.Play("shoot");
        if (!gameObject.GetComponent<AudioSource>().isPlaying)
        {
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
