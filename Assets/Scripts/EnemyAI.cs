﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float attackCooldown;
    public float hitboxLifetime;
    public float walkSpeed;
    public float ClosestDistanceToPlayer = 4;
    [Header("[0,1] -> Non-Attack Animations|[>1] -> Attack Animations")]
    public string[] animations;
    public float[] hitboxDelays;

    float attackCooldownSave;
    float hitboxLifetimeSave;
    [Header("Keep same number of arguments as Hitbox Delays")]
    public float[] hitboxDelaysSave;
    int randAttack;
    bool dontAttack;
    bool hitboxActive;
    Vector3 startPos;
    Quaternion startRot;

    void Start()
    {
        attackCooldownSave = attackCooldown;
        hitboxLifetimeSave = hitboxLifetime;
        startPos = transform.position;
        startRot = transform.rotation;
        randAttack = Random.Range(2, animations.Length);
        for (int i = 0; i < hitboxDelays.Length; i++)
        {
            hitboxDelaysSave[i] = hitboxDelays[i];
        }
    }

    void Update ()
    {
        Animator anim = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (gameObject.GetComponentInChildren<SightChecking>().aggro) //Player in Sight
        {
            //Set speed and destination
            if (gameObject.GetComponent<NavMeshAgent>().enabled == true)
            {
                GetComponent<NavMeshAgent>().speed = walkSpeed;
                gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;
            }
            //Activate Arrow Shooter
            if (GetComponent<ArrowShooter>() != null)
            {
                GetComponent<ArrowShooter>().activated = true;
            }
            attackCooldown -= 1 * Time.deltaTime; //Attack Cooldown going down
        }
        else
        {
            GetComponent<NavMeshAgent>().speed = 0; //Reset speed
            anim.Play("" + animations[0]); //Play Idle animation
        }

        //Stop near player
        if (Vector3.Distance(transform.position, player.transform.position) <= ClosestDistanceToPlayer)
        {
            GetComponent<NavMeshAgent>().speed = 0; //Reset speed
            if (Vector3.Distance(transform.position, player.transform.position) >= 6)
            {
                transform.LookAt(player.transform.position);
            }
        }

        //Attack Animation
        if (attackCooldown < 0 && !dontAttack && gameObject.GetComponentInChildren<SightChecking>().aggro)
        {
            Animation();
            randAttack = Random.Range(2, animations.Length);
            dontAttack = true;
        }
        if (attackCooldown < 0)
        {
            hitboxDelays[randAttack] -= 1 * Time.deltaTime;
        }
        //Attack Hitbox
        if (hitboxDelays[randAttack] < 0)
        {
            hitboxLifetime -= 1 * Time.deltaTime;
        }
        if (hitboxDelays[randAttack] < 0 && !hitboxActive)
        {
            Attack();
            hitboxActive = true;
        }
        if (hitboxLifetime < 0)
        {
            transform.Find("AttackHitbox").gameObject.SetActive(false);
            hitboxLifetime = hitboxLifetimeSave;
            for (int i = 0; i < hitboxDelays.Length; i++)
            {
                hitboxDelays[i] = hitboxDelaysSave[i];
            }
            attackCooldown = attackCooldownSave;
            dontAttack = false;
            hitboxActive = false;
        }

        //Reset
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<FPCharacterController>().dead)
        {
            transform.position = startPos;
            transform.rotation = startRot;
            GetComponentInChildren<SightChecking>().aggro = false;
        }
	}

    void Animation()
    {
        Animator anim = GetComponent<Animator>();
        anim.Play("" + animations[randAttack]);
    }

    void Attack ()
    {
        transform.Find("AttackHitbox").gameObject.SetActive(true);
    }
}
