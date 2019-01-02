using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    public float attackCooldown;
    public float hitboxDelay;
    public float hitboxLength;
    public GameObject player;
    public float speed;
    public float distanceToPlayer = 4;
    public bool dontAttack;

    public string attack;

    bool aggro;
    float attackCooldownSave;
    float hitboxDelaySave;
    float hitboxLengthSave;
    Vector3 startPos;
    Quaternion startRot;

    void Start()
    {
        attackCooldownSave = attackCooldown;
        hitboxDelaySave = hitboxDelay;
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
            if (!dontAttack)
            {
                attackCooldown -= 1 * Time.deltaTime; //Attack Cooldown going down
            }
            if (gameObject.GetComponent<NavMeshAgent>().enabled == true)
            {
                GetComponent<NavMeshAgent>().speed = speed;
                gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;
            }

            //Activate Arrow Shooter
            if (GetComponent<ArrowShooter>() != null)
            {
                GetComponent<ArrowShooter>().activated = true;
            }
        }
        else
        {
            GetComponent<NavMeshAgent>().speed = 0;
        }

        //Stop near player
        if (Vector3.Distance(transform.position, player.transform.position) <= distanceToPlayer)
        {
            GetComponent<NavMeshAgent>().speed = 0;
            if (Vector3.Distance(transform.position, player.transform.position) >= 6)
            {
                transform.LookAt(player.transform.position);
            }
        }

        //Attack Animation
        if (attackCooldown <= 0 && aggro && !dontAttack)
        {
            Animation();
            hitboxDelay -= 1 * Time.deltaTime;
            //attackCooldown = attackCooldownSave;
        }
        if (hitboxDelay <= 0)
        {
            Attack();
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

    void Animation ()
    {
        Animator anim = GetComponent<Animator>();
        anim.Play("" + attack);
    }

    void Attack ()
    {
        transform.Find("AttackHitbox").gameObject.SetActive(true);
    }
}
