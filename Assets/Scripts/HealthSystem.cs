using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class HealthSystem : MonoBehaviour {

    public float health;
    public bool dead;
    public float hitTimer;

    float hitTimerSave;
    bool hit;

    ChromaticAberration chrom = null;
    Vignette vignette = null;

    // Use this for initialization
    void Start () {
        hitTimerSave = hitTimer;

        //Postprocessing
        PostProcessVolume volume = Camera.main.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out chrom);
        volume.profile.TryGetSettings(out vignette);
    }
	
	void Update () {
        if (hit)
        {
            hitTimer -= 1 * Time.deltaTime;
        }
        if (hitTimer < 0)
        {
            chrom.intensity.value = 0;
            vignette.intensity.value = 0;
            hit = false;
            hitTimer = hitTimerSave;
        }
    }

    public bool HealthLoss (float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            dead = true;
            if (gameObject.tag == "Player")
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<FPCharacterController>().dead = true;
            }

            //Deactivate NavMeshAgent
            if (gameObject.GetComponent<NavMeshAgent>() != null)
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = false; 
            }
        }

        //AI
        if (gameObject.GetComponent<EnemyAI>() != null)
        {
            gameObject.GetComponentInChildren<SightChecking>().aggro = true;
        }

        //Player
        if (gameObject.tag == "Player")
        {
            hit = true;
            chrom.intensity.value = 1;
            vignette.intensity.value = 0.5f;
        }

        return dead;
    }
}
