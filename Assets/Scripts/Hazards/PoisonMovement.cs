using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonMovement : MonoBehaviour
{
    public bool active;
    public float riseLevel;

    float riseLevelSave;
    GameObject[] poisonPipes;

    void Start()
    {
        riseLevelSave = riseLevel;
        poisonPipes = GameObject.FindGameObjectsWithTag("PoisonPipe");
    }

    void Update()
    {
        if (riseLevel > 0 && active)
        {
            riseLevel -= Time.deltaTime;
            transform.Translate(Vector3.up * Time.deltaTime);
            for (int i = 0; i < poisonPipes.Length; i++)
            {
                poisonPipes[i].GetComponent<PoisonPipeSpawning>().active = true;
            }
        }
        if (riseLevel < 0)
        {
            riseLevel = riseLevelSave;
            active = false;
        }
    }
}
