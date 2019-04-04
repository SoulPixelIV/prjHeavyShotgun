using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPipeSpawning : MonoBehaviour
{
    public GameObject spawnedObject;

    GameObject poison;

    public void dropPoison()
    {
        poison = Instantiate(spawnedObject, transform.Find("Spawnpoint").transform.position, Quaternion.identity);
        poison.GetComponent<Animator>().Play("poisonDropping");
    }
}
