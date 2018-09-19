using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour {

    public float despawnTime;

	void Update () {
        despawnTime -= 1 * Time.deltaTime;

        if (despawnTime <= 0)
        {
            Destroy(gameObject);
        }
	}
}
