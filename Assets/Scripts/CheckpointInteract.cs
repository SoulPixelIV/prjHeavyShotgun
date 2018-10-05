using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointInteract : MonoBehaviour {

    public GameObject interactTxt;
    public GameObject checkpointTxt;
    public Vector3 spawnOffset;

    public bool activated;
    public bool showTxt = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (activated)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<Animator>().Play("checkpointRotate");
                GameObject.FindGameObjectWithTag("Player").GetComponent<FPCharacterController>().startPos = transform.position + spawnOffset;
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
                checkpointTxt.SetActive(true);
            }
            activated = true;         
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            interactTxt.SetActive(false);
            checkpointTxt.SetActive(false);
            activated = false;
        }
    }
}
