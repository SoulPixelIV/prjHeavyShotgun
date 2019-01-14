using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickering : MonoBehaviour
{
    public float flickerIntervall;
    public float lightDuration;

    public Material light;
    public Material lightOff;

    float flickerIntervallSave;
    float lightDurationSave;
    bool lightOn;

    void Start()
    {
        flickerIntervallSave = flickerIntervall;
        lightDurationSave = lightDuration;
    }

    void Update()
    {
        flickerIntervall -= 1 * Time.deltaTime;

        if (flickerIntervall < 0)
        {
            GameObject.Find("LampLight").GetComponent<Light>().enabled = true;
            gameObject.GetComponent<MeshRenderer>().material = light;
            lightOn = true;
        }

        if (lightOn)
        {
            lightDuration -= 1 * Time.deltaTime;
        }

        if (lightDuration < 0)
        {
            GameObject.Find("LampLight").gameObject.GetComponent<Light>().enabled = false;
            gameObject.GetComponent<MeshRenderer>().material = lightOff;
            lightDuration = lightDurationSave;
            flickerIntervall = flickerIntervallSave;
            lightOn = false;
        }
    }
}
