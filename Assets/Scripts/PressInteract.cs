using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressInteract : MonoBehaviour {

    public GameObject interactTxt;
    public string openAnimation;
    public bool access;
    public bool showTxt = true;
	
	// Update is called once per frame
	void Update () {
		if (access)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<Animator>().Play("" + openAnimation);
                showTxt = false;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (showTxt)
            {
                interactTxt.SetActive(true);
            }
            access = true;         
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            interactTxt.SetActive(false);
            access = false;
        }
    }
}
