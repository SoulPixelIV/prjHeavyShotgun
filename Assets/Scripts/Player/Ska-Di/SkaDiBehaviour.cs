using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkaDiBehaviour : MonoBehaviour {

	public GameObject skadi;
	public float throwStrength;
    public float attackCooldown;
    public int bullets;
    public int magazines;

    float attackCooldownSave;
    bool punched;
    int bulletsMax;

    GameObject player;
    GameObject[] skadis;

	void Start () {
        bulletsMax = bullets;
        attackCooldownSave = attackCooldown;
        player = GameObject.FindGameObjectWithTag("Player");
    }

	void Update () {
        Animator anim = GetComponent<Animator>();

        if (attackCooldown > 0)
        {
            attackCooldown -= 1 * Time.deltaTime;
        }
		if (Input.GetKeyDown(KeyCode.Mouse0) && attackCooldown <= 0 && bullets > 0 || Input.GetKey(KeyCode.U) && attackCooldown <= 0 && bullets > 0 )
        {
            Punch();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKey(KeyCode.O))
        {
            skadis = GameObject.FindGameObjectsWithTag("SkadiRB");
            for (int i = 0; i < skadis.Length; i++)
            {
                skadis[i].GetComponent<SkaDiRigidbody>().Explosion();
            }
        }

        //Reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (bullets < bulletsMax && magazines > 0)
            {
                if (bulletsMax - bullets <= magazines)
                {
                    //anim.Play("shotgunReload");
                    magazines -= bulletsMax - bullets;
                    bullets = bulletsMax;
                }
                else
                {
                    //anim.Play("shotgunReload");
                    bullets += magazines;
                    magazines = 0;
                }
            }
        }

        //Moving Animation
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<FPCharacterController>().isMoving &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("skadiAttack") &&
            !anim.GetCurrentAnimatorStateInfo(0).IsName("skadiWalk"))
        {
            anim.Play("skadiWalk");
        }
    }

    void Punch()
    {
        Animator anim = GetComponent<Animator>();
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("skadiAttack"))
        {
            bullets -= 1; //Ammo
            anim.Play("skadiAttack");
            attackCooldown = attackCooldownSave;
            punched = true;
            //Instantiate Skadi
            var skadiThrow = Instantiate(skadi, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);
            skadiThrow.GetComponent<Rigidbody>().AddForce((Camera.main.transform.forward * throwStrength) + new Vector3(0, 0, 0), ForceMode.Impulse);
        }
    }
}
