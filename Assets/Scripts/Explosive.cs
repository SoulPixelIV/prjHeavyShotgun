using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public float explosionRadius;

    public void Start()
    {
        Destroy(gameObject, 0.2f);
    }

    public void Update()
    {
        if (transform.localScale.x < explosionRadius)
        {
            transform.localScale += new Vector3(1, 1, 1);
        }
    }
}
