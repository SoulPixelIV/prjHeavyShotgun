using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    public int damage;
    public bool damageEnemies;
    public bool damagePlayer;
    public float attackCooldown = 0.02f;

    bool dealtDamage;
    float attackCooldownSave;

    void Start()
    {
        attackCooldownSave = attackCooldown;
    }

    void Update()
    {
        attackCooldown -= Time.deltaTime;

        if (attackCooldown <= 0)
        {
            dealtDamage = false;
            attackCooldown = attackCooldownSave;
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
                        other.GetComponent<HealthSystem>().HealthLoss(damage);
                        //Play Hitmarker
                        Camera.main.gameObject.GetComponent<AudioSource>().Play();
                    }
                }
                else
                {
                    if (other.tag == "Player")
                    {
                        if (damagePlayer)
                        {
                            other.GetComponent<HealthSystem>().HealthLoss(damage);
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
                }
                dealtDamage = true;
            }
        }
    }
}
