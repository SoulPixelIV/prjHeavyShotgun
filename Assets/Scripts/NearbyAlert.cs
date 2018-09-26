using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearbyAlert : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (other.GetComponentInChildren<SightChecking>() != null)
            {
                if (other.GetComponentInChildren<SightChecking>().aggro)
                {
                    GetComponentInParent<SightChecking>().aggro = true;
                }
            }
        }
    }
}
