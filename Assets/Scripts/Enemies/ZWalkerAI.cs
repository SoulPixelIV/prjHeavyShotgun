using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZWalkerAI : MonoBehaviour
{
    //public bool bowActive;
    //public float shootTime;
    //public GameObject arrow;
    public bool friendly;
    public float attackCooldown;
    public float hitboxLifetime;
    public float walkSpeed;
    public float ClosestDistanceToPlayer = 4;

    [Header("Destination before normal AI movement")]
    public GameObject InterestPoint;
    
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
        randAttack = Random.Range(2, animations.Length);
        for (int i = 0; i < hitboxDelays.Length; i++)
        {
            hitboxDelaysSave[i] = hitboxDelays[i];
        }
        Animator anim = GetComponent<Animator>();
        GetComponent<NavMeshAgent>().speed = 0; //Reset speed
        anim.Play(animations[0]); //Play Idle animation
        //Bow
        if (bowActive)
        {
            transform.Find("Bow").gameObject.SetActive(true);
            InvokeRepeating("BowShoot", shootTime, shootTime);
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
        //Animator anim = GetComponent<Animator>();
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
            if (!bowActive)
            {
                attackCooldown -= 1 * Time.deltaTime; //Attack Cooldown going down
            }
            anim.Play(animations[1]);
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

        //Bow rotation
        if (bowActive)
        {
            GameObject bow = transform.Find("Bow").gameObject;
            bow.transform.LookAt(player.transform.position);
        }

        //Attack Animation
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

    void Animation()
    {
        Animator anim = GetComponent<Animator>();
        anim.Play(animations[randAttack]);
    }

    void Attack ()
    {
        transform.Find("AttackHitbox").gameObject.SetActive(true);
    }

    void BowShoot()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject bow = transform.Find("Bow").gameObject;
        if (gameObject.GetComponentInChildren<SightChecking>().aggro) //Check if aggro
        {
            //Instantiate Arrow
            var goalRotation = transform.rotation;
            goalRotation *= Quaternion.Euler(-90, -90, 0);
            var arrowShot = Instantiate(arrow, bow.transform.position + bow.transform.forward, bow.transform.rotation * goalRotation);
            arrowShot.GetComponent<Rigidbody>().AddForce((bow.transform.forward * 1.5f), ForceMode.Impulse);
        }    
    }

    public void ToggleBow()
    {
        CancelInvoke("BowShoot");
    }
}
