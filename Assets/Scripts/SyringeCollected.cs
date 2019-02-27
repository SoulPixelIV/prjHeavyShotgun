using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyringeCollected : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<FPCharacterController>().medSyringes += 2;
            other.gameObject.GetComponent<FPCharacterController>().syringeTxt.text = "Med-Syringes x" + other.gameObject.GetComponent<FPCharacterController>().medSyringes;
            Destroy(gameObject);
        }
    }
}
