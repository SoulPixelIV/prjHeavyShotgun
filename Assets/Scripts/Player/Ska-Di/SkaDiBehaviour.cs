using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkaDiBehaviour : MonoBehaviour {

	public GameObject skadi;
	public float throwStrength;
    public float attackCooldown;

    float attackCooldownSave;
    bool punched;

    GameObject player;

	void Start () {
        attackCooldownSave = attackCooldown;
        player = GameObject.FindGameObjectWithTag("Player");
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
		var skadiThrow = Instantiate(skadi, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);
		skadiThrow.GetComponent<Rigidbody>().AddForce((Camera.main.transform.forward * throwStrength) + new Vector3(0, 0, 0), ForceMode.Impulse);
    }
}
