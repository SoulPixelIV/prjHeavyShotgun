using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour {

    public GameObject hands;
    public GameObject shotgun;

	// Use this for initialization
	void Start () {
        hands = GameObject.FindGameObjectWithTag("Hands").gameObject;
        shotgun = GameObject.FindGameObjectWithTag("Shotgun").gameObject;

        WeaponSwitch(0);
    }
	
	// Update is called once per frame
	public void WeaponSwitch (int weapon) {
        if (weapon == 0)
        {
            hands.SetActive(true);
            shotgun.SetActive(false);
        }
        if (weapon == 1)
        {
            hands.SetActive(false);
            shotgun.SetActive(true);
        }
    }
}
