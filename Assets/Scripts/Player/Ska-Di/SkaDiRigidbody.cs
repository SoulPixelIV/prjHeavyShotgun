using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkaDiRigidbody : MonoBehaviour
{
    void OnCollisionEnter()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}
