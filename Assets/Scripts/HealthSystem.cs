using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {

    public int health;
    public bool dead;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Health: " + health);
	}

    public bool HealthLoss (int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            dead = true;
        }
        return dead;
    }
}
