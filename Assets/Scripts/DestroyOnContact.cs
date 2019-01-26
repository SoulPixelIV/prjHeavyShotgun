using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    public float invTime;

    void OnCollisionEnter(Collision collision)
    {
        if (invTime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        invTime -= 1 * Time.deltaTime;
    }
}
