using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    public float attackCooldown;
    public GameObject player;
    public float speed;

    public string attack;

    bool aggro;
    float attackCooldownSave;
    Vector3 startPos;
    Quaternion startRot;

    void Start()
    {
        attackCooldownSave = attackCooldown;
        startPos = transform.position;
        startRot = transform.rotation;
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
                GetComponent<NavMeshAgent>().speed = speed;
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

        if (attackCooldown < attackCooldownSave - 0.1f)
        {
            transform.Find("AttackHitbox").gameObject.SetActive(false);
        }

        //Reset
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<FPCharacterController>().dead)
        {
            transform.position = startPos;
            transform.rotation = startRot;
            aggro = false;
            GetComponentInChildren<SightChecking>().aggro = false;
        }
	}

    void Attack ()
    {
        Animator anim = GetComponent<Animator>();

        anim.Play("" + attack);
        transform.Find("AttackHitbox").gameObject.SetActive(true);
    }
}
