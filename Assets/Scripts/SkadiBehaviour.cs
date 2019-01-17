using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkadiBehaviour : MonoBehaviour {

	public GameObject skadi;
	public float throwStrength;
    public float attackCooldown;

    float attackCooldownSave;
    bool punched;

	void Start () {
        attackCooldownSave = attackCooldown;
    }

	void Update () {
        if (attackCooldown > 0)
        {
            attackCooldown -= 1 * Time.deltaTime;
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
        attackCooldown = attackCooldownSave;
        punched = true;
		//Instantiate Skadi
		var skadiThrow = Instantiate(skadi, transform.position, Quaternion.identity);
		skadiThrow.GetComponent<Rigidbody>().AddForce((-transform.right * throwStrength) + new Vector3(0, -10, 0), ForceMode.Impulse);
    }
}
