using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonMovement : MonoBehaviour
{
    public bool active;
    public float riseLevel;

    float riseLevelSave;

    void Start()
    {
        riseLevelSave = riseLevel;
    }

    void Update()
    {
        if (riseLevel > 0 && active)
        {
            riseLevel -= Time.deltaTime;
            transform.Translate(Vector3.up * Time.deltaTime);
        }
        if (riseLevel < 0)
        {
            riseLevel = riseLevelSave;
            active = false;
        }
    }
}
