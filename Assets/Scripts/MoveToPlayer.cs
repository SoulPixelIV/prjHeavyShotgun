using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour {

    public float speed;
    public float despawnTime;

    [HideInInspector] public GameObject player;
    [HideInInspector] public Vector3 destination;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        destination = (player.transform.position - transform.position).normalized;
        Vector3 targetPoint = player.transform.position;
        targetPoint.z = transform.position.z;
        transform.LookAt(targetPoint);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += destination * speed * Time.deltaTime;

        despawnTime -= 1 * Time.deltaTime;

        if (despawnTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
