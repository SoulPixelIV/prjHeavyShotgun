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
		//Shift Layermask to Ignore Raycast
        layerMask = 1 << 2;
        layerMask = ~layerMask;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < fovAngle * 0.5f)
            {
                Debug.DrawRay(transform.position + new Vector3(0, 1, 0), direction, Color.green);
                RaycastHit hit;

                if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), direction.normalized, out hit, col.radius, layerMask))
                {
                    Debug.Log(hit.collider);
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        aggro = true;
                    }
                }
            }
            else
            {
                Debug.DrawRay(transform.position + new Vector3(0, 1, 0), direction, Color.red);
            }
        }

		if (other.gameObject.tag == "Enemy") 
		{
			Vector3 directionNew = other.transform.position - transform.position - new Vector3(0, 1, 0);
			float angleNew = Vector3.Angle(directionNew, transform.forward);

			RaycastHit hit2;

			if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), directionNew.normalized, out hit2, col.radius))
			{
				if (hit2.collider.gameObject.tag == "Enemy" && hit2.collider.transform.Find("Sight").GetComponent<SightChecking>().aggro)
				{
					aggro = true;
				}
			}
		}
    }
}
