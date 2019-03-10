using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPipeSpawning : MonoBehaviour
{
    public GameObject spawnedObject;
    public bool active;
    public float timer;

    float timerSave;
    float spawnDelay = 0.2f;

    void Start()
    {
        timerSave = timer;
    }

    void Update()
    {
        if (active)
        {
            timer -= Time.deltaTime;
            spawnDelay -= Time.deltaTime;
        }

        if (spawnDelay < 0)
        {
            Instantiate(spawnedObject, transform.Find("Spawnpoint").transform.position, Quaternion.identity);
            spawnDelay = 0.2f;
        }

        if (timer < 0)
        {
            active = false;
            timer = timerSave;
        }
    }
}
