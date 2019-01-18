using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkaDiRigidbody : MonoBehaviour
{
    public float explosionRadius;

    GameObject explosion;

    void Start()
    {
        explosion = transform.Find("ExplosionHitbox").gameObject;
        explosion.SetActive(false);
    }

    void OnCollisionEnter()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void Explosion()
    {
        explosion.SetActive(true);
        Destroy(gameObject, 0.4f); 
    }
}
