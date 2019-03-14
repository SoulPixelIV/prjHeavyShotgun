using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLightRotation : MonoBehaviour
{
    public bool active;
    public float timer;

    float timerSave;
    bool rotating;

    GameObject light1;
    GameObject light2;

    void Start()
    {
        timerSave = timer;
        light1 = transform.Find("RotatingLight1").gameObject;
        light2 = transform.Find("RotatingLight2").gameObject;
    }

    void Update()
    {
        Animator anim = GetComponent<Animator>();

        if (active)
        {
            timer -= Time.deltaTime;
            if (!rotating)
            {
                anim.speed = 1;
                light1.SetActive(true);
                light2.SetActive(true);
                rotating = true;
            }
        }

        if (timer < 0)
        {
            active = false;
            timer = timerSave;
            rotating = false;
            anim.speed = 0;
            light1.SetActive(false);
            light2.SetActive(false);
        }
    }
}
