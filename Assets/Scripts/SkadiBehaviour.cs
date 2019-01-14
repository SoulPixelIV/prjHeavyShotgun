using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkadiBehaviour : MonoBehaviour {

    public float attackCooldown;
    public float hitboxCooldown;

    float attackCooldownSave;
    float hitboxCooldownSave;
    bool punched;

	// Use this for initialization
	void Start () {
        attackCooldownSave = attackCooldown;
        hitboxCooldownSave = hitboxCooldown;
    }
	
	// Update is called once per frame
	void Update () {
        if (attackCooldown > 0)
        {
            attackCooldown -= 1 * Time.deltaTime;
        }

        if (punched)
        {
            hitboxCooldown -= 1 * Time.deltaTime;
        }
        if (hitboxCooldown <= 0)
        {
            //transform.Find("HandsAttackHitbox").gameObject.SetActive(false);
            punched = false;
            hitboxCooldown = hitboxCooldownSave;
        }

		if (Input.GetKeyDown(KeyCode.Mouse0) && attackCooldown <= 0)
        {
            Punch();
        }
	}

    void Punch()
    {
        Animator anim = GetComponent<Animator>();
        anim.Play("skadiAttack");

        //transform.Find("HandsAttackHitbox").gameObject.SetActive(true);
        attackCooldown = attackCooldownSave;
        punched = true;
    }
}
