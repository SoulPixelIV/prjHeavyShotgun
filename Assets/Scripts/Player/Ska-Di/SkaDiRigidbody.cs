using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkaDiRigidbody : MonoBehaviour
{
    public float explosionRadius;

    bool exploding;
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

    public void Update()
    {
        if (exploding && explosion.transform.localScale.x < explosionRadius)
        {
            explosion.transform.localScale += new Vector3(1, 1, 1);
        }
    }

    public void Explosion()
    {
        exploding = true;
        gameObject.GetComponent<Rigidbody>().detectCollisions = true;
        explosion.SetActive(true);
        Destroy(gameObject, 0.2f); 
    }
}
