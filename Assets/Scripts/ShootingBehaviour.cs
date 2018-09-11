using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBehaviour : MonoBehaviour {

    public Rigidbody bullet;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Animator anim = GetComponent<Animator>();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("shoot"))
            {
                Shoot();
            }
        }
	}

    void Shoot ()
    {
        Animator anim = GetComponent<Animator>();
        
        Rigidbody spawnedBullet;
        anim.Play("shoot");
        if (!gameObject.GetComponent<AudioSource>().isPlaying)
        {
            gameObject.GetComponent<AudioSource>().Play();
        }
        spawnedBullet = Instantiate(bullet, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z), Quaternion.identity);
        spawnedBullet.velocity = transform.TransformDirection(new Vector3(-1, 0, 0) * 60);
    }
}
