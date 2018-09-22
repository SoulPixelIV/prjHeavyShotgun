using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : MonoBehaviour {

    public GameObject interactTxt;

    public bool access;
    public bool showTxt = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (access)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<Animator>().Play("doorOpen");
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
