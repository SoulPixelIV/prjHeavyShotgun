using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponSwitching : MonoBehaviour {

    public GameObject hands;
    public GameObject shotgun;
    public GameObject skadi;
    public TextMeshProUGUI weaponTxt;

    public bool shotgunUnlocked;
    public bool skadiUnlocked;

    Animator animShoMiRü;
    Animator animSkadi;
    Animator animFists;

    Animation shotgunReload;

    int lastUsed;

	void Start () {
        hands = GameObject.FindGameObjectWithTag("Hands").gameObject;
        shotgun = GameObject.FindGameObjectWithTag("Shotgun").gameObject;
        skadi = GameObject.FindGameObjectWithTag("Skadi").gameObject;

        animShoMiRü = transform.Find("PlayerCamera").transform.Find("GunCamera").transform.Find("ShoMiRü").GetComponent<Animator>();
        animSkadi = transform.Find("PlayerCamera").transform.Find("GunCamera").transform.Find("Skadi").GetComponent<Animator>();
        animFists = transform.Find("PlayerCamera").transform.Find("GunCamera").transform.Find("Hands").GetComponent<Animator>();

        WeaponSwitch(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !animShoMiRü.GetCurrentAnimatorStateInfo(0).IsName("shotgunReload") && 
            !animSkadi.GetCurrentAnimatorStateInfo(0).IsName("skadiAttack") && 
            !animShoMiRü.GetCurrentAnimatorStateInfo(0).IsName("shoot") &&
            !animShoMiRü.GetCurrentAnimatorStateInfo(0).IsName("shotgunMoveBack"))
        {
            WeaponSwitch(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !animSkadi.GetCurrentAnimatorStateInfo(0).IsName("skadiAttack") &&
            !animFists.GetCurrentAnimatorStateInfo(0).IsName("handsAttack"))
        {
            if (shotgunUnlocked)
            {
                WeaponSwitch(1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && !animShoMiRü.GetCurrentAnimatorStateInfo(0).IsName("shotgunReload") && 
            !animShoMiRü.GetCurrentAnimatorStateInfo(0).IsName("shoot") &&
            !animShoMiRü.GetCurrentAnimatorStateInfo(0).IsName("shotgunMoveBack") &&
            !animFists.GetCurrentAnimatorStateInfo(0).IsName("handsAttack"))
        {
            if (skadiUnlocked)
            {
                WeaponSwitch(2);
            }
        }
        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (lastUsed == 0)
            {
                WeaponSwitch(0);
            }
            if (lastUsed == 1)
            {
                WeaponSwitch(1);
            }
        }
        */
    }

    public void WeaponSwitch (int weapon) {
        if (weapon == 0)
        {
            hands.SetActive(true);
            shotgun.SetActive(false);
            skadi.SetActive(false);
            weaponTxt.text = "Fists";
            GameObject.FindGameObjectWithTag("Player").GetComponent<AmmoSystem>().weapon = 0;
        }
        if (weapon == 1)
        {
            hands.SetActive(false);
            shotgun.SetActive(true);
            skadi.SetActive(false);
            weaponTxt.text = "ShoMiRü";
            GameObject.FindGameObjectWithTag("Player").GetComponent<AmmoSystem>().weapon = 1;
        }
        if (weapon == 2)
        {
            skadi.SetActive(true);
            shotgun.SetActive(false);
            hands.SetActive(false);
            weaponTxt.text = "Ska-Di";
            GameObject.FindGameObjectWithTag("Player").GetComponent<AmmoSystem>().weapon = 2;
        }
    }
}
