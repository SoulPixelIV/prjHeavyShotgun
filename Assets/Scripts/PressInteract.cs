using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressInteract : MonoBehaviour {

    public GameObject target;
    public GameObject interactTxt;
    public string openAnimation;
    public bool access;
    public bool showTxt = true;

    public bool activated;
	
	// Update is called once per frame
	void Update () {
		if (access && !activated)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Activate();
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

    public void Activate()
    {
        GetComponent<Animator>().Play(openAnimation);
        showTxt = false;
        if (target.gameObject.tag == "Lift")
        {
            target.GetComponent<LiftMovement>().active = true;
        }
        if (target.gameObject.tag == "Poison")
        {
            target.GetComponent<PoisonMovement>().active = true;
            target.GetComponent<PoisonMovement>().spawnPoisonDropping();
        }
        activated = true;
    }
}
