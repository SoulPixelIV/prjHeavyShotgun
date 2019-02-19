using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressInteract : MonoBehaviour {

    public GameObject target;
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("FOUNDENEMY");
            Activate();
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

    void Activate()
    {
        GetComponent<Animator>().Play("" + openAnimation);
        showTxt = false;
        if (target.gameObject.tag == "Lift")
        {
            target.GetComponent<LiftMovement>().active = true;
        }
    }
}
