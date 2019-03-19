using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponSwitching>().shotgunUnlocked == true)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponSwitching>().WeaponSwitch(1);
            GameObject.FindGameObjectWithTag("Shotgun").GetComponent<ShoMiRüBehaviour>().magazines += 6;
            gameObject.SetActive(false);
        }
    }
}
