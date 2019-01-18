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

    int lastUsed;

	void Start () {
        hands = GameObject.FindGameObjectWithTag("Hands").gameObject;
        shotgun = GameObject.FindGameObjectWithTag("Shotgun").gameObject;
        skadi = GameObject.FindGameObjectWithTag("Skadi").gameObject;

        WeaponSwitch(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponSwitch(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (shotgunUnlocked)
            {
                WeaponSwitch(1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
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
        }
        if (weapon == 1)
        {
            hands.SetActive(false);
            shotgun.SetActive(true);
            skadi.SetActive(false);
            weaponTxt.text = "ShoMiRü";
        }
        if (weapon == 2)
        {
            skadi.SetActive(true);
            shotgun.SetActive(false);
            hands.SetActive(false);
            weaponTxt.text = "Ska-Di";
        }
    }
}
