using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour {

    public bool enemy = true;
    public int damage;

    bool dealtDamage;
    float attackCooldown = 0.02f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        attackCooldown -= 1 * Time.deltaTime;

        if (attackCooldown <= 0)
        {
            dealtDamage = false;
            attackCooldown = 0.02f;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (enemy)
        {
            if (other.tag == "Player")
            {
                if (!dealtDamage)
                {
                    other.GetComponent<HealthSystem>().HealthLoss(damage);
                    dealtDamage = true;
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
    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            dealtDamage = false;
        }
    }
    */
}
