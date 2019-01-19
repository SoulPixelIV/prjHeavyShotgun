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

    void OnCollisionEnter(Collision other)
    {
        gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().detectCollisions = false;
        transform.SetParent(other.transform);
    }

    public void Explosion()
    {
        gameObject.GetComponent<Rigidbody>().detectCollisions = true;
        explosion.SetActive(true);
        Destroy(gameObject, 0.4f); 
    }
}
