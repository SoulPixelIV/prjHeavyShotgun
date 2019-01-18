using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkaDiRigidbody : MonoBehaviour
{
    public float explosionRadius;
    public int damage;

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

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HealthSystem>() != null)
        {
            other.GetComponent<HealthSystem>().HealthLoss(damage);
        }
    }

    public void Explosion()
    {
        explosion.SetActive(true);
        Destroy(gameObject, 0.7f); 
    }
}
