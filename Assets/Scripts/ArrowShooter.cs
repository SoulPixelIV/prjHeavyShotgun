using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : MonoBehaviour {

    public bool activated;
    public bool playerFocus;
    public GameObject player;
    public float shotSpeed;
    public float shootTimer;
    public float arrowLifetime = 3;
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
            if (!playerFocus)
            {
                arrow = Instantiate(arrow, spawnpoint.transform.position, Quaternion.identity);
                arrow.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
                arrow.transform.rotation = rotation;
                gameObject.GetComponent<AudioSource>().Play();
                Destroy(arrow, 3);
            }
            else
            {
                arrow = Instantiate(arrow, spawnpoint.transform.position, Quaternion.identity);

                Vector3 targetPoint = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z) - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(-targetPoint, direction);

                arrow.GetComponent<Rigidbody>().AddForce(targetPoint / 10, ForceMode.Impulse);
                arrow.transform.rotation = targetRotation;
                gameObject.GetComponent<AudioSource>().Play();
                Destroy(arrow, arrowLifetime);
            }
        }
    }
}
