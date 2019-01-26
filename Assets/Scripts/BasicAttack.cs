using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    public int damage;
    public bool damageEnemies;

    bool dealtDamage;
    float attackCooldown = 0.02f;

    void Update()
    {
        attackCooldown -= 1 * Time.deltaTime;

        if (attackCooldown <= 0)
        {
            dealtDamage = false;
            attackCooldown = 0.02f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<HealthSystem>() != null)
        {
            if (!dealtDamage)
            {
                if (other.tag == "Enemy")
                {
                    if (damageEnemies)
                    {
                        other.GetComponent<HealthSystem>().HealthLoss(damage / 3);
                        //Play Hitmarker
                        Camera.main.gameObject.GetComponent<AudioSource>().Play();
                    }
                }
                else
                {
                    other.GetComponent<HealthSystem>().HealthLoss(damage);
                    //Play Hitmarker
                    Camera.main.gameObject.GetComponent<AudioSource>().Play();
                }
                dealtDamage = true;
            }
        }
    }
}
