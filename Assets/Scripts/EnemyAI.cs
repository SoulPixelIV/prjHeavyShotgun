using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    public int sightDistance;
    public GameObject player;
    
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) < sightDistance && Vector3.Distance(gameObject.transform.position, player.transform.position) > 5)
        {
            if (GetComponent<NavMeshAgent>().isStopped == true)
            {
                GetComponent<NavMeshAgent>().isStopped = false;
                gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;
            }
        }
        else
        {
            if (GetComponent<NavMeshAgent>().isStopped == false)
            {
                GetComponent<NavMeshAgent>().isStopped = true;
            }
        }
	}
}
