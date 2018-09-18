using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour {

    public int health;
    public bool dead;

    public Text healthTxt;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        healthTxt.text = "Health: " + health;
    }

    public bool HealthLoss (int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            dead = true;

            //Deactivate NavMeshAgent
            if (gameObject.GetComponent<NavMeshAgent>() != null)
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = false; 
            }
        }

        //AI
        if (gameObject.GetComponent<EnemyAI>() != null)
        {
            gameObject.GetComponent<EnemyAI>().aggro = true;
        }
        return dead;
    }
}
