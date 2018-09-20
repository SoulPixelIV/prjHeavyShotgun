using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    public float attackCooldown;
    public GameObject player;

    bool aggro;
    float attackCooldownSave;

    void Start()
    {
        attackCooldownSave = attackCooldown;
    }

    // Update is called once per frame
    void Update () {
        Animator anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");

        if (gameObject.GetComponentInChildren<SightChecking>().aggro == true)
        {
            aggro = true;
        }

        if (aggro)
        {
            attackCooldown -= 1 * Time.deltaTime;

            if (gameObject.GetComponent<NavMeshAgent>().enabled == true)
            {
                GetComponent<NavMeshAgent>().speed = 5.6f;
                gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;
            }
        }
        else
        {
            GetComponent<NavMeshAgent>().speed = 0;
        }

        //Stop near player
        if (Vector3.Distance(transform.position, player.transform.position) <= 4)
        {
            GetComponent<NavMeshAgent>().speed = 0;
        }

        //Attack
        if (attackCooldown <= 0 && aggro)
        {
            Attack();
            attackCooldown = attackCooldownSave;
        }

        if (attackCooldown < 1)
        {
            transform.Find("AttackHitbox").gameObject.SetActive(false);
        }
	}

    void Attack ()
    {
        Animator anim = GetComponent<Animator>();

        anim.Play("zombieAttack");
        transform.Find("AttackHitbox").gameObject.SetActive(true);
    }
}
