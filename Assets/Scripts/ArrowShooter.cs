using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : MonoBehaviour {

    public bool activated;
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
        if (arrow != null)
        {
            arrow = Instantiate(arrow, spawnpoint.transform.position, Quaternion.identity);
            arrow.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
            arrow.transform.rotation = rotation;
            gameObject.GetComponent<AudioSource>().Play();
            Destroy(arrow, 3);
        }
    }
}
