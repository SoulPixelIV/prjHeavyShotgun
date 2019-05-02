using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZWalkerAI : MonoBehaviour
{
    public bool friendly;
    public float attackCooldown;
    public float hitboxLifetime;
    public float walkSpeed;
    public float ClosestDistanceToPlayer = 4;

    int randAttack;
    bool dontAttack;
    bool hitboxActive;
    float attackCooldownSave;
    float hitboxLifetimeSave;
    string ground;
    int stepCount = 0;

    [Header("Destination before normal AI movement")]
    public GameObject InterestPoint;
    
    [Header("[0,1] -> Non-Attack Animations|[>1] -> Attack Animations")]
    public string[] animations;
    public float[] hitboxDelays;
    
    [Header("Keep same number of arguments as Hitbox Delays")]
    public float[] hitboxDelaysSave;
  
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

    void Start()
    {
        Animator anim = GetComponent<Animator>();

        attackCooldownSave = attackCooldown;
        hitboxLifetimeSave = hitboxLifetime;
        startPos = transform.position;
        startRot = transform.rotation;

        GetComponent<NavMeshAgent>().speed = 0; //Reset speed
        anim.Play(animations[0]); //Play Idle animation

        randAttack = Random.Range(2, animations.Length);
        for (int i = 0; i < hitboxDelays.Length; i++)
        {
            hitboxDelaysSave[i] = hitboxDelays[i];
        }        
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
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(animations[0]) || anim.GetCurrentAnimatorStateInfo(0).IsName(animations[1]))
            {
                anim.Play(animations[1]);
            }
        }

        //Stop near player
        if (Vector3.Distance(transform.position, player.transform.position) <= ClosestDistanceToPlayer)
        {
            GetComponent<NavMeshAgent>().speed = 0; //Reset speed
            if (Vector3.Distance(transform.position, player.transform.position) >= 5f)
            {
                transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, transform.position.z));
            }
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

        //Attack Animation
        if (!friendly && gameObject.GetComponentInChildren<SightChecking>().aggro)
        {
            attackCooldown -= Time.deltaTime;
        }
        if (attackCooldown < 0 && !dontAttack && gameObject.GetComponentInChildren<SightChecking>().aggro && !friendly)
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

    void Animation()
    {
        Animator anim = GetComponent<Animator>();
        anim.Play(animations[randAttack]);
    }

    void Attack ()
    {
        transform.Find("AttackHitbox").gameObject.SetActive(true);
    }
}
