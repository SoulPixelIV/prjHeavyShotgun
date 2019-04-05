using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class HealthSystem : MonoBehaviour {

    public float health;
    public float healthSave;
    public bool dead;
    public float hitTimer;

    float hitTimerSave;
    bool hit;
    bool flash;
    float flashTime = 0.1f;
    Renderer[] allRenderers;

    ChromaticAberration chrom = null;
    Vignette vignette = null;

    void Start () {
        hitTimerSave = hitTimer;
        healthSave = health;
        if (transform.Find("Model") != null)
        {
            allRenderers = transform.Find("Model").gameObject.GetComponents<Renderer>();
        }

        //Postprocessing
        PostProcessVolume volume = Camera.main.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out chrom);
        volume.profile.TryGetSettings(out vignette);
    }
	
	void Update () {
        if (hit)
        {
            hitTimer -= Time.deltaTime;
        }
        if (hitTimer < 0)
        {
            chrom.intensity.value = 0;
            vignette.intensity.value = 0;
            hit = false;
            hitTimer = hitTimerSave;
        }
        if (flash)
        {
            flashTime -= Time.deltaTime;
        }
        if (flashTime < 0)
        {
            flash = false;
            flashTime = 0.1f;
            foreach (Renderer rend in allRenderers)
            {
                for (int z = 0; z < rend.materials.Length; z++)
                {
                    rend.materials[z].SetColor("_EmissionColor", new Color(0, 0, 0));
                }
            }
        }
    }

    public float HealthGain(float heal)
    {
        health += heal;
        return health;
    }

    public bool HealthLoss (float damage)
    {
        health -= damage;

        //Enemy Flash
        if (gameObject.tag == "Enemy")
        {
            flash = true;
            foreach (Renderer rend in allRenderers)
            {
                for (int z = 0; z < rend.materials.Length; z++)
                {
                    rend.materials[z].SetColor("_EmissionColor", new Color(1, 0.4f, 0.4f));
                }
            }
        }

        if (health <= 0)
        {
            dead = true;
            if (gameObject.tag == "Player")
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<FPCharacterController>().dead = true;
            }
            if (gameObject.tag == "Enemy")
            {
                gameObject.SetActive(false);
                if (GetComponent<EnemyAI>() != null)
                {
                    if (GetComponent<EnemyAI>().bowActive)
                    {
                        GetComponent<EnemyAI>().ToggleBow();
                    }
                }
            }
            /*
            //Deactivate NavMeshAgent
            if (GetComponent<NavMeshAgent>() != null)
            {
                GetComponent<NavMeshAgent>().enabled = false; 
            }
            */
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
