using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationSpawner : MonoBehaviour
{
	public GameObject bird;
	public int birdSpawnNumber;

	public int maxXSpawn;
	public GameObject YAxisPlane;
	public float YAxisOffset;
	public int maxZSpawn;

	//GameObject birdsParent;
	Vector3 YAxisSpawn;
	Vector3 spawn;
	void Start()
	{
		//GameObject birdsParent = GameObject.FindGameObjectWithTag("birdsParent");
		//Set offset from ground
		YAxisSpawn = new Vector3 (0, YAxisOffset, 0);
		//Set Spawn position
		Vector3 spawn = new Vector3 (Random.Range (0, maxXSpawn), YAxisPlane.transform.position.y + YAxisSpawn.y, Random.Range (0, maxZSpawn));
	}

	void Update()
	{
		//Creat birdSpawnNumber amount of birds
		for (int i = 0; i < birdSpawnNumber; i++) 
		{
			Instantiate (bird, spawn, Quaternion.Euler(new Vector3(270, 0, 0)));
			//Recalculate spawnpoint
			spawn = new Vector3 (Random.Range (0, maxXSpawn), YAxisPlane.transform.position.y + YAxisSpawn.y, Random.Range (0, maxZSpawn));
			birdSpawnNumber--;
		}
	}
}
