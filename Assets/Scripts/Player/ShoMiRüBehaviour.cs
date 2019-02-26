using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ShoMiRüBehaviour : MonoBehaviour {

    public Rigidbody bullet;
    public GameObject bullethole;

    public int damage;
    public int power;
    public float shootCooldown;

    public int bullets;
    public int magazines;
    public bool aiming;
    public bool aimingSneak;

    [Header("Audio")]
    public float reloadDelay;
    public AudioClip shotSound;
    public AudioClip reloadSound;
    bool shotActive;

    int bulletsMax;
    float shootCooldownSave;
    bool startCooldown;
    float reloadDelaySave;

    DepthOfField dof = null;

    void Start()
    {
        bulletsMax = bullets;
        shootCooldownSave = shootCooldown;
        startCooldown = true;
        reloadDelaySave = reloadDelay;

        //Postprocessing
        PostProcessVolume volume = Camera.main.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out dof);
    }

    void Update ()
    {
        Animator anim = GetComponent<Animator>();

        //### Shooting + Aiming ###
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<FPCharacterController>().dead)
        {
            //Shooting
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.U))
            {
                if (shootCooldown <= 0 && bullets > 0 && aiming)
                {
                    startCooldown = true;
                    shootCooldown = shootCooldownSave;
                    Shoot();
                }
            }

            //Aiming
            if (Input.GetKey(KeyCode.Mouse1) && !Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.O) && !Input.GetKey(KeyCode.LeftShift))
            {
                if (!aiming)
                {
                    anim.Play("shotgunAim");
                }
                //dof.active = true; //Depth of field
                aiming = true;
                GameObject.FindGameObjectWithTag("Player").GetComponent<FPCharacterController>().aimingGun = true;
            }
            //Aiming + Sneaking
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Mouse1))
            {
                if (!aimingSneak)
                {
                    anim.Play("shotgunAimSneak");
                    aimingSneak = true;
                    //dof.active = true; //Depth of field
                }
            }

            //Stop aiming
            if (!Input.GetKey(KeyCode.Mouse1) && !Input.GetKey(KeyCode.O))
            {
                if (aiming && !anim.GetCurrentAnimatorStateInfo(0).IsName("shoot"))
                {
                    if (aiming)
                    {
                        anim.Play("shotgunMoveBack");
                        aiming = false;
                        GameObject.FindGameObjectWithTag("Player").GetComponent<FPCharacterController>().aimingGun = false;
                        //dof.active = false; //Depth of field
                    }
                }
            }
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.Mouse1) && !Input.GetKey(KeyCode.O))
            {
                if (aimingSneak && !anim.GetCurrentAnimatorStateInfo(0).IsName("shoot"))
                {
                    if (aimingSneak)
                    {
                        anim.Play("shotgunMoveBackSneak");
                        aimingSneak = false;
                        GameObject.FindGameObjectWithTag("Player").GetComponent<FPCharacterController>().aimingGun = false;
                        //dof.active = false; //Depth of field
                    }
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

        //Audio
        if (shotActive)
        {
            reloadDelay -= Time.deltaTime;
        }
        if (reloadDelay < 0)
        {
            GetComponent<AudioSource>().PlayOneShot(reloadSound, 1);
            shotActive = false;
            reloadDelay = reloadDelaySave;
        }
    }

    void Shoot ()
    {
        Animator anim = GetComponent<Animator>();
        RaycastHit hit;

        //Ammo
        bullets -= 1;

        //Set Sight Layer
        int layerMask = 1 << 2;
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

                if (objHit.GetComponent<Rigidbody>() != null)
                {
                    if (hit.distance < 1.5f)
                    {
                        objHit.GetComponent<Rigidbody>().AddForce(-dir * (power * 1.5f), ForceMode.Impulse);
                    }
                    if (hit.distance > 1.5f && hit.distance < 3)
                    {
                        objHit.GetComponent<Rigidbody>().AddForce(-dir * power, ForceMode.Impulse);
                    }
                    if (hit.distance > 3 && hit.distance < 4)
                    {
                        objHit.GetComponent<Rigidbody>().AddForce(-dir * (power / 2), ForceMode.Impulse);
                    }
                    if (hit.distance > 4 && hit.distance < 6)
                    {
                        objHit.GetComponent<Rigidbody>().AddForce(-dir * (power / 4), ForceMode.Impulse);
                    }
                }
            }
            //Enemy Hit
            else if (objHit.tag == "Enemy")
            {
                //Direction between player and enemy
                Vector3 dir = (transform.position - objHit.transform.position).normalized;

                if (hit.distance < 1.5f)
                {
                    objHit.GetComponent<HealthSystem>().HealthLoss(damage / 1.5f);
                    Camera.main.gameObject.GetComponent<AudioSource>().Play();
                }
                if (hit.distance > 1.5f && hit.distance < 5)
                {
                    objHit.GetComponent<HealthSystem>().HealthLoss(damage);
                    Camera.main.gameObject.GetComponent<AudioSource>().Play();
                }
                if (hit.distance > 5 && hit.distance < 7)
                {
                    objHit.GetComponent<HealthSystem>().HealthLoss(damage / 2);
                    Camera.main.gameObject.GetComponent<AudioSource>().Play();
                }
                if (hit.distance > 7 && hit.distance < 10)
                {
                    objHit.GetComponent<HealthSystem>().HealthLoss(damage / 4);
                    Camera.main.gameObject.GetComponent<AudioSource>().Play();
                }
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
            GetComponent<AudioSource>().PlayOneShot(shotSound, 1);
            shotActive = true;
        }
    }
}
