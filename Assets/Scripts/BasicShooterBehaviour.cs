using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShooterBehaviour : MonoBehaviour {

    public GameObject bullet;
    public float shootTimer;

    float shootTimerSave;

	// Use this for initialization
	void Start () {
        shootTimerSave = shootTimer;
	}
	
	// Update is called once per frame
	void Update () {
		if (shootTimer > 0)
        {
            shootTimer -= 1 * Time.deltaTime;
        }
        else
        {
            Fire();
            shootTimer = shootTimerSave;
        }
	}

    void Fire()
    {
        if (GetComponent<HealthSystem>().HealthLoss(0) == false)
        {
            Instantiate(bullet, transform.position, transform.rotation);
        }
    }
}
