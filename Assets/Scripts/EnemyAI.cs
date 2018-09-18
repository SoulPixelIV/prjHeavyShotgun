﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    public float attackCooldown;
    public GameObject player;

    public bool aggro;
    float attackCooldownSave;

    void Start()
    {
        attackCooldownSave = attackCooldown;
    }

    // Update is called once per frame
    void Update () {
        player = GameObject.FindGameObjectWithTag("Player");
        if (aggro)
        {
            attackCooldown -= 1 * Time.deltaTime;

            GetComponent<NavMeshAgent>().speed = 2.5f;
            gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;
        }
        else
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
