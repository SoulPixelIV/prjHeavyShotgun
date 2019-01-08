using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoSystem : MonoBehaviour {

    public TextMeshProUGUI ammoTxt;

    int bullets;
    int magazines;

	void Update () {
        bullets = GameObject.FindGameObjectWithTag("Shotgun").GetComponent<ShootingBehaviour>().bullets;
        magazines = GameObject.FindGameObjectWithTag("Shotgun").GetComponent<ShootingBehaviour>().magazines;
        ammoTxt.text = ("Ammo: " + bullets + " / " + magazines);
    }
}
