using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoisonRunnerAI : MonoBehaviour
{
    public bool friendly;
    public float attackCooldown;
    public float hitboxLifetime;
    public float walkSpeed;
    public float ClosestDistanceToPlayer = 4;
    public float explodeDelay;
    public GameObject InterestPoint;
    [Header("[0,1] -> Non-Attack Animations|[3] -> Explosion Animation")]
    public string[] animations;
    public float[] hitboxDelays;

    float attackCooldownSave;
    float hitboxLifetimeSave;
    [Header("Keep same number of arguments as Hitbox Delays")]
    public float[] hitboxDelaysSave;
    int randAttack;
    bool dontAttack;
    bool hitboxActive;
    float explodeDelaySave;
    [HideInInspector]
    public Vector3 startPos;
    public Quaternion startRot;
    public bool reachedInterestPoint;

    public void Start()
    {
        attackCooldownSave = attackCooldown;
        hitboxLifetimeSave = hitboxLifetime;
        startPos = transform.position;
        startRot = transform.rotation;
        explodeDelaySave = explodeDelay;
        randAttack = Random.Range(2, animations.Length);
        for (int i = 0; i < hitboxDelays.Length; i++)
        {
            hitboxDelaysSave[i] = hitboxDelays[i];
        }
        Animator anim = GameObject.Find("Model").GetComponent<Animator>();
        GetComponent<NavMeshAgent>().speed = 0; //Reset speed
        anim.Play(animations[0]); //Play Idle animation
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AlarmLight")
        {
            if (gameObject.GetComponentInChildren<SightChecking>().aggro)
            {
                other.transform.parent.GetComponent<LightFlickering>().enabled = true;
            }
        }
    }

    void Update ()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Animator anim = GameObject.Find("Model").GetComponent<Animator>();

        if (gameObject.GetComponentInChildren<SightChecking>().aggro) //Player in Sight
        {
            //Set speed and destination
            if (gameObject.GetComponent<NavMeshAgent>().enabled == true)
            {
                if (InterestPoint == null || reachedInterestPoint)
                {
                    GetComponent<NavMeshAgent>().speed = walkSpeed;
                    gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;
                }
            }
            attackCooldown -= 1 * Time.deltaTime; //Attack Cooldown going down
        }
        else
        {
            //GetComponent<NavMeshAgent>().speed = 0; //Reset speed
            //anim.Play(animations[0]); //Play Idle animation
        }

        //Stop near player
        if (Vector3.Distance(transform.position, player.transform.position) <= ClosestDistanceToPlayer)
        {
            GetComponent<NavMeshAgent>().speed = 0; //Reset speed
            anim.Play(animations[2]);
            if (Vector3.Distance(transform.position, player.transform.position) >= 5f)
            {
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, transform.position.z));
            }
        }
        else if (gameObject.GetComponentInChildren<SightChecking>().aggro)
        {
            anim.Play(animations[1]);
            explodeDelay = explodeDelaySave;
        }

        //Interest Point
        if (gameObject.GetComponentInChildren<SightChecking>().aggro) //Player in Sight
        {
            if (InterestPoint != null && !reachedInterestPoint && gameObject.GetComponent<NavMeshAgent>().enabled == true)
            {
                if (Vector3.Distance(transform.position, InterestPoint.transform.position) > 2)
                {
                    GetComponent<NavMeshAgent>().speed = walkSpeed;
                    gameObject.GetComponent<NavMeshAgent>().destination = InterestPoint.transform.position;
                }
                else
                {
                    if (!InterestPoint.transform.Find("LeverObject").GetComponent<PressInteract>().activated)
                    {
                        InterestPoint.transform.Find("LeverObject").GetComponent<PressInteract>().Activate();
                    }
                    reachedInterestPoint = true;
                }
            }
        }

        //Explosion
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("poisonRunnerExploding"))
        {
            explodeDelay -= Time.deltaTime;
        }
        if (explodeDelay < 0)
        {
            GameObject.Find("PoisonBomb").GetComponent<Destructible>().Destroy();
        }

        /*
        //Attack Animation
        if (attackCooldown < 0 && !dontAttack && gameObject.GetComponentInChildren<SightChecking>().aggro && !friendly)
        {
            Animation();
            randAttack = Random.Range(2, animations.Length);
            dontAttack = true;
        }
        if (attackCooldown < 0)
        {
            if (hitboxDelays.Length != 0)
            {
                hitboxDelays[randAttack] -= 1 * Time.deltaTime;
            }
        }
        //Attack Hitbox
        if (hitboxDelays.Length != 0)
        {
            if (hitboxDelays[randAttack] < 0)
            {
                hitboxLifetime -= 1 * Time.deltaTime;
            }
            if (hitboxDelays[randAttack] < 0 && !hitboxActive && !friendly)
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
        }

        /*
        //Reset
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<FPCharacterController>().dead)
        {
            transform.position = startPos;
            transform.rotation = startRot;
            GetComponentInChildren<SightChecking>().aggro = false;
        }
        */
	}

    void Animation()
    {
        Animator anim = GameObject.Find("Model").GetComponent<Animator>();
        anim.Play(animations[randAttack]);
    }

    void Attack ()
    {
        transform.Find("AttackHitbox").gameObject.SetActive(true);
    }
}
