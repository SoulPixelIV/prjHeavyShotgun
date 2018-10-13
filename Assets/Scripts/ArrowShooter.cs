using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : MonoBehaviour {

    public bool activated;
    public bool playerFocus;
    public float shotSpeed;
    public float shootTimer;
    public GameObject arrow;
    public GameObject spawnpoint;
    public Vector3 direction;
    public Quaternion rotation;

    float shootTimerSave;

	// Use this for initialization
	void Start () {
        shootTimerSave = shootTimer;
	}
	
	// Update is called once per frame
	void Update () {
		if (activated)
        {
            shootTimer -= 1 * Time.deltaTime;
        }
        if (shootTimer <= 0)
        {
            Shoot();
            shootTimer = shootTimerSave;
        }
	}

    void Shoot()
    {
        if (arrow != null && !playerFocus)
        {
            arrow = Instantiate(arrow, spawnpoint.transform.position, Quaternion.identity);
            arrow.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
            arrow.transform.rotation = rotation;
            gameObject.GetComponent<AudioSource>().Play();
            Destroy(arrow, 3);
        }
        if (arrow != null && playerFocus)
        {
            arrow = Instantiate(arrow, spawnpoint.transform.position, Quaternion.identity);
            arrow.transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, shotSpeed * Time.deltaTime);
            //arrow.transform.rotation = rotation;
            gameObject.GetComponent<AudioSource>().Play();
            Destroy(arrow, 3);
        }
    }
}
