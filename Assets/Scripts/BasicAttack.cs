using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDamage : MonoBehaviour
{
    public int damage;

    bool dealtDamage;
    float attackCooldown = 2f;
	
	void Update () {
        attackCooldown -= 1 * Time.deltaTime;

        if (attackCooldown <= 0)
        {
            dealtDamage = false;
            attackCooldown = 2f;
        }
	}

    private void OnTriggerStay(Collider other)
    {
        if (!dealtDamage && other.GetComponent<HealthSystem>() != null)
        {
            //Play Hitmarker
            Camera.main.gameObject.GetComponent<AudioSource>().Play();

            other.GetComponent<HealthSystem>().HealthLoss(damage);
            dealtDamage = true;
        }
    }
}
