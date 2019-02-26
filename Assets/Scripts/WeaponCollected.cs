using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollected : MonoBehaviour {

    public int weapon;

    Animator animShoMiRü;
    Animator animSkadi;
    Animator animFists;

    void Start () {
        animShoMiRü = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerCamera").transform.Find("GunCamera").transform.Find("ShoMiRü").GetComponent<Animator>();
        animSkadi = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerCamera").transform.Find("GunCamera").transform.Find("Skadi").GetComponent<Animator>();
        animFists = GameObject.FindGameObjectWithTag("Player").transform.Find("PlayerCamera").transform.Find("GunCamera").transform.Find("Hands").GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !animShoMiRü.GetCurrentAnimatorStateInfo(0).IsName("shotgunReload") &&
            !animSkadi.GetCurrentAnimatorStateInfo(0).IsName("skadiAttack") &&
            !animShoMiRü.GetCurrentAnimatorStateInfo(0).IsName("shoot") &&
            !animShoMiRü.GetCurrentAnimatorStateInfo(0).IsName("shotgunMoveBack") &&
            !animFists.GetCurrentAnimatorStateInfo(0).IsName("handsAttack"))
        {
            if (weapon == 1)
            {
                GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<WeaponSwitching>().shotgunUnlocked = true;
                GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<WeaponSwitching>().WeaponSwitch(1);
            }
            if (weapon == 2)
            {
                GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<WeaponSwitching>().skadiUnlocked = true;
                GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<WeaponSwitching>().WeaponSwitch(2);
            }

            Destroy(gameObject);
        }
    }
}
