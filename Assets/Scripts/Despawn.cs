using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour {

    public float despawnTime;
    public bool despawnOnContact;

	void Update () {
        if (!despawnOnContact)
        {
            despawnTime -= 1 * Time.deltaTime;
        }

        if (despawnTime <= 0)
        {
            Destroy(gameObject);
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (despawnOnContact)
        {
            Destroy(gameObject);
        }
    }
}
