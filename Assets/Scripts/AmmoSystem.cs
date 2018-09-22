using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoSystem : MonoBehaviour {

    public Text ammoTxt;

    int bullets;
    int magazines;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        bullets = GameObject.FindGameObjectWithTag("Shotgun").GetComponent<ShootingBehaviour>().bullets;
        magazines = GameObject.FindGameObjectWithTag("Shotgun").GetComponent<ShootingBehaviour>().magazines;
        ammoTxt.text = ("Ammo: " + bullets + " / " + magazines);
	}
}
