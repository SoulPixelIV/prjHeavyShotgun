using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour {

    public GameObject hands;
    public GameObject shotgun;

    public bool shotgunUnlocked;

    int lastUsed;

	// Use this for initialization
	void Start () {
        hands = GameObject.FindGameObjectWithTag("Hands").gameObject;
        shotgun = GameObject.FindGameObjectWithTag("Shotgun").gameObject;

        WeaponSwitch(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !GameObject.FindGameObjectWithTag("Shotgun").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("shoot"))
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
