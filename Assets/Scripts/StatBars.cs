using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBars : MonoBehaviour
{
    void Update()
    {
        gameObject.transform.localScale = new Vector3(GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>().health / 100, 1f);
    }
}
