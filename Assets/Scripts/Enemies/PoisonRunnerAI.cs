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
    [HideInInspector]
    public float explodeDelaySave;
    [HideInInspector]
    public Vector3 startPos;
    public Quaternion startRot;
    public bool reachedInterestPoint;

    [Header("Audio")]
    public AudioClip stepStone1;
    public AudioClip stepStone2;
    public AudioClip stepMetal1;
    public AudioClip stepMetal2;
    private float stepSpeed;
    string ground;
    int stepCount = 0;

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
        Animator anim = GetComponent<Animator>();
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

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Stone")
        {
            ground = "Stone";
        }
        if (other.tag == "Metal")
        {
            ground = "Metal";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Stone")
        {
            ground = null;
        }
        if (other.tag == "Metal")
        {
            ground = null;
        }
    }

    public void RestartAnimation()
    {
        Animator anim = GetComponent<Animator>();
        anim.Play("PoisonRunnerIdle");
    }

    void Update ()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Animator anim = GetComponent<Animator>();

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
            if (GameObject.Find("PoisonBomb") != null)
            {
                GameObject.Find("PoisonBomb").GetComponent<Destructible>().Destroy();
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
        //Audio
        if (!GetComponent<AudioSource>().isPlaying && GetComponent<NavMeshAgent>().speed != 0)
        {
            if (ground == "Stone")
            {
                if (stepCount == 0)
                {
                    GetComponent<AudioSource>().PlayOneShot(stepStone1, 1);
                    stepCount = 1;
                }
                else
                {
                    GetComponent<AudioSource>().PlayOneShot(stepStone2, 1);
                    stepCount = 0;
                }
            }
            else if (ground == "Metal")
            {
                if (stepCount == 0)
                {
                    GetComponent<AudioSource>().PlayOneShot(stepMetal1, 1);
                    stepCount = 1;
                }
                else
                {
                    GetComponent<AudioSource>().PlayOneShot(stepMetal2, 1);
                    stepCount = 0;
                }
            }
            else
            {
                if (stepCount == 0)
                {
                    GetComponent<AudioSource>().PlayOneShot(stepStone1, 1);
                    stepCount = 1;
                }
                else
                {
                    GetComponent<AudioSource>().PlayOneShot(stepStone2, 1);
                    stepCount = 0;
                }
            }
        }
    }
}
