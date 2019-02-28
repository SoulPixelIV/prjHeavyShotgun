using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbCollection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<FPCharacterController>().orbCount++;
            GameObject.FindGameObjectWithTag("Player").GetComponent<FPCharacterController>().orbCountTxt.text = "Orbs x" + GameObject.FindGameObjectWithTag("Player").GetComponent<FPCharacterController>().orbCount;
            Destroy(gameObject);
        }
    }
}
