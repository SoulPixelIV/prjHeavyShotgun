﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    public int damage;

    bool dealtDamage;
    float attackCooldown = 0.02f;
	
	void Update () {
        attackCooldown -= 1 * Time.deltaTime;

        if (attackCooldown <= 0)
        {
            dealtDamage = false;
            attackCooldown = 0.02f;
        }
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (!dealtDamage)
            {
                //Play Hitmarker
                Camera.main.gameObject.GetComponent<AudioSource>().Play();

                if (tag == "Enemy")
                {
                    other.GetComponent<HealthSystem>().HealthLoss(damage / 3);
                }
                else
                {
                    other.GetComponent<HealthSystem>().HealthLoss(damage);
                }
                dealtDamage = true;
            }
        }
    }
}