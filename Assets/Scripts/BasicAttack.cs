using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour {

    public bool enemy = true;
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
        if (enemy)
        {
            if (other.tag == "Player")
            {
                if (!dealtDamage)
                {
                    PlayerDamage();
                }
            }
        }
        else
        {
            if (other.tag == "Enemy")
            {
                if (!dealtDamage)
                {
                    //Play Hitmarker
                    Camera.main.gameObject.GetComponent<AudioSource>().Play();

                    other.GetComponent<HealthSystem>().HealthLoss(damage);
                    dealtDamage = true;
                }
            }
        }
    }

    void PlayerDamage()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>().HealthLoss(damage);
        dealtDamage = true;
        gameObject.SetActive(false);
    }
}
