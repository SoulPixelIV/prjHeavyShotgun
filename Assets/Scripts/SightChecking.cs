using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightChecking : MonoBehaviour {

    public bool aggro;
    public float fovAngle = 110;

    SphereCollider col;
    int layerMask;

    void Awake()
    {
        col = GetComponent<SphereCollider>();
    }

    void Update()
    {
        layerMask = 1 << 15;
        layerMask = ~layerMask;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 direction = other.transform.position - transform.position - new Vector3(0, 2, 0);
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < fovAngle * 0.5f)
            {
                Debug.DrawRay(transform.position + new Vector3(0, 2, 0), direction, Color.green);
                RaycastHit hit;

                if (Physics.Raycast(transform.position + new Vector3(0, 2, 0), direction.normalized, out hit, col.radius, layerMask))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        aggro = true;
                    }
                }
            }
            else
            {
                Debug.DrawRay(transform.position + new Vector3(0, 2, 0), direction, Color.red);
            }
        }
    }
}
