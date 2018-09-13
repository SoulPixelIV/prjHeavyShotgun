using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    public int speed;
    public float despawnTime;

    [HideInInspector] public GameObject player;
    [HideInInspector] public Vector3 destination;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        destination = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        despawnTime -= 1 * Time.deltaTime;

        if (despawnTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
