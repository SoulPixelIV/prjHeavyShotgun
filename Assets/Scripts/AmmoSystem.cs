using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoSystem : MonoBehaviour {

    public TextMeshProUGUI ammoTxt;
    public int weapon;

    int bullets;
    int magazines;

	void Update () {
        if (weapon == 0)
        {
            bullets = 0;
            magazines = 0;
            ammoTxt.text = ("Ammo: " + bullets + " / " + magazines);
        }
        if (GameObject.FindGameObjectWithTag("Shotgun") != null && weapon == 1)
        {
            bullets = GameObject.FindGameObjectWithTag("Shotgun").GetComponent<ShoMiRüBehaviour>().bullets;
            magazines = GameObject.FindGameObjectWithTag("Shotgun").GetComponent<ShoMiRüBehaviour>().magazines;
            ammoTxt.text = ("Ammo: " + bullets + " / " + magazines);
        }
        if (GameObject.FindGameObjectWithTag("Skadi") != null && weapon == 2)
        {
            bullets = GameObject.FindGameObjectWithTag("Skadi").GetComponent<SkaDiBehaviour>().bullets;
            magazines = GameObject.FindGameObjectWithTag("Skadi").GetComponent<SkaDiBehaviour>().magazines;
            ammoTxt.text = ("Ammo: " + bullets + " / " + magazines);
        }
    }
}
