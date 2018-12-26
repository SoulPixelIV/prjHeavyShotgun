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
    public GameObject arrowPlayerFocus;
    public GameObject spawnpoint;
    public Vector3 direction;
    public int rotation;

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

        if (GetComponent<EnemyAI>() != null)
        {
            if (!GetComponentInChildren<SightChecking>().aggro)
            {
                activated = false;
            }
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
                gameObject.GetComponent<AudioSource>().Play();
                Destroy(arrow, 3);
                switch (rotation)
                {
                    case 0:
                        arrow.transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case 1:
                        arrow.transform.rotation = Quaternion.Euler(0, 0, 90);
                        break;
                    case 2:
                        arrow.transform.rotation = Quaternion.Euler(0, 90, 0);
                        break;
                    case 3:
                        arrow.transform.rotation = Quaternion.Euler(90, 0, 0);
                        break;
                }
            }
            else
            {
                Instantiate(arrowPlayerFocus, spawnpoint.transform.position, Quaternion.identity);
            }
        }
    }
}
