using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ShootingBehaviour : MonoBehaviour {

    public Rigidbody bullet;
    public GameObject bullethole;

    public int damage;
    public int power;
    public float shootCooldown;

    public int bullets;
    public int magazines;
    public bool aiming;

    int bulletsMax;
    float shootCooldownSave;
    bool startCooldown;

    DepthOfField dof = null;

    void Start()
    {
        bulletsMax = bullets;
        shootCooldownSave = shootCooldown;
        startCooldown = true;

        //Postprocessing
        PostProcessVolume volume = Camera.main.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out dof);
    }

    // Update is called once per frame
    void Update () {
        Animator anim = GetComponent<Animator>();

        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<FPCharacterController>().dead)
        {
            //Left Mousebutton
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (shootCooldown <= 0 && bullets > 0 && aiming)
                {
                    startCooldown = true;
                    shootCooldown = shootCooldownSave;
                    Shoot();
                }
            }

            //Right Mousebutton
            if (Input.GetKey(KeyCode.Mouse1))
            {
                if (!aiming)
                {
                    anim.Play("shotgunAim");
                }
                dof.active = true;
                aiming = true;
                GameObject.FindGameObjectWithTag("Player").GetComponent<FPCharacterController>().aimingGun = true;
            }
            else
            {
                if (aiming && !anim.GetCurrentAnimatorStateInfo(0).IsName("shoot"))
                {
                    anim.Play("shotgunMoveBack");
                    aiming = false;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<FPCharacterController>().aimingGun = false;
                    dof.active = false;
                }
            }

            //Reload
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (bullets < bulletsMax && magazines > 0)
                {
                    if (bulletsMax - bullets <= magazines)
                    {
                        anim.Play("shotgunReload");
                        magazines -= bulletsMax - bullets;
                        bullets = bulletsMax;
                    }
                    else
                    {
                        anim.Play("shotgunReload");
                        bullets += magazines;
                        magazines = 0;
                    }
                }
            }
        }
        else
        {
            if (bullets < bulletsMax && magazines < bulletsMax)
            {
                bullets = bulletsMax;
                magazines = bulletsMax;
            }
        }

        if (startCooldown)
        {
            shootCooldown -= 1 * Time.deltaTime;
        }
        if (shootCooldown <= 0)
        {
            startCooldown = false;
        }
	}

    void Shoot ()
    {
        Animator anim = GetComponent<Animator>();
        RaycastHit hit;

        //Ammo
        bullets -= 1;

        //Set Sight Layer
        int layerMask = 1 << 10;
        layerMask = ~layerMask;

        var direction = Camera.main.transform.TransformDirection(new Vector3(0, 0, 1));

        if (Physics.Raycast(Camera.main.transform.position, direction, out hit, 300, layerMask))
        {
            GameObject objHit = hit.collider.gameObject;
            Debug.Log("Hit object:" + objHit);

            //Environment Hit
            if (objHit.tag == "Environment")
            {
                //Direction between player and object
                Vector3 dir = (transform.position - objHit.transform.position).normalized;

                if (objHit.GetComponent<Destructible>() != null)
                {
                    objHit.GetComponent<Destructible>().Destroy();
                }

                if (hit.distance < 1.5f)
                {
                    objHit.GetComponent<Rigidbody>().AddForce(-dir * (power * 1.5f), ForceMode.Impulse);
                }
                if (hit.distance > 1.5f && hit.distance < 3)
                {
                    objHit.GetComponent<Rigidbody>().AddForce(-dir * power, ForceMode.Impulse);
                }
                if (hit.distance > 3)
                {
                    objHit.GetComponent<Rigidbody>().AddForce(-dir * (power / 2), ForceMode.Impulse);
                }
            }
            //Enemy Hit
            else if (objHit.tag == "Enemy")
            {
                //Direction between player and enemy
                Vector3 dir = (transform.position - objHit.transform.position).normalized;

                if (hit.distance < 3)
                {
                    if (objHit.GetComponent<HealthSystem>().HealthLoss(damage) == true)
                    {
                        objHit.GetComponent<Rigidbody>().AddForce(-dir * power, ForceMode.Impulse);
                    }
                }
                else
                {
                    if (objHit.GetComponent<HealthSystem>().HealthLoss(damage / 1.5f) == true)
                    {
                        objHit.GetComponent<Rigidbody>().AddForce(-dir * (power / 2.5f) , ForceMode.Impulse);
                    }
                }

                //Play Hitmarker
                Camera.main.gameObject.GetComponent<AudioSource>().Play();
            }
            else if (objHit.tag == "Static")
            {
                //Create Bullethole
                Instantiate(bullethole, hit.point + (hit.normal * 0.01f), Quaternion.LookRotation(hit.normal));
            }
        }
        
        //Play animation and sound
        anim.Play("shoot");
        if (!gameObject.GetComponent<AudioSource>().isPlaying)
        {
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
