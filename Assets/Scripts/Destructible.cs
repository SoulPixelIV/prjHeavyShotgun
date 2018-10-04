using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

    public GameObject destroyedVersion;
    public bool marked;
    public Material standardMat;

    private void Start()
    {
        if (marked)
        {
            GetComponent<Renderer>().material = standardMat;
        }
    }

    public void Destroy () {
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
	}
}
