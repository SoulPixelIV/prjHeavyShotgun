﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonMovement : MonoBehaviour
{
    public bool active;
    public float riseLevel;

    Vector3 startPos;
    float riseLevelSave;
    GameObject[] poisonPipes;
    GameObject[] alarmLights;
    bool activated;

    void Start()
    {
        startPos = transform.position;
        riseLevelSave = riseLevel;
        poisonPipes = GameObject.FindGameObjectsWithTag("PoisonPipe");
        alarmLights = GameObject.FindGameObjectsWithTag("AlarmLightRotation");
    }

    void Update()
    {
        if (riseLevel > 0 && active && !activated)
        {
            riseLevel -= Time.deltaTime;
            transform.Translate(Vector3.up * Time.deltaTime);
            for (int i = 0; i < poisonPipes.Length; i++)
            {
                poisonPipes[i].GetComponent<PoisonPipeSpawning>().dropPoison();
            }
            for (int i = 0; i < alarmLights.Length; i++)
            {
                alarmLights[i].GetComponent<AlarmLightRotation>().active = true;
            }
            activated = true;
        }
        if (riseLevel < 0)
        {
            riseLevel = riseLevelSave;
            active = false;
            activated = false;
        }
    }

    public void ResetPos()
    {
        transform.position = startPos;
        active = false;
        riseLevel = riseLevelSave;
    }
}
