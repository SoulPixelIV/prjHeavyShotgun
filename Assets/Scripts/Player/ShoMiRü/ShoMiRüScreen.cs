using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoMiRüScreen : MonoBehaviour
{
    public float maxRange;
    public float scale;
    public GameObject enemyDot;

    public GameObject player;
    public GameObject[] enemies;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        InvokeRepeating("ScanForEnemies", 1.0f, 0.4f);
    }

    void Update()
    {
        //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(transform.rotation.x, player.transform.rotation.y, transform.rotation.z), Time.deltaTime);
    }

    void ScanForEnemies()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (Vector3.Distance(player.transform.position, enemies[i].transform.position) < maxRange)
            {
                if (GameObject.Find("" + enemies[i]) == null)
                {
                    GameObject currEnemyDot = Instantiate(enemyDot, transform.position + CalcPos(enemies[i].transform.position), transform.rotation);
                    currEnemyDot.name = "" + enemies[i];
                }
                else
                {
                    GameObject targetDot = GameObject.Find("" + enemies[i]);
                    targetDot.transform.position = transform.position + (CalcPos(enemies[i].transform.position));
                }
            }
            else
            {
                if (GameObject.Find("" + enemies[i]) != null)
                {
                    GameObject targetDot = GameObject.Find("" + enemies[i]);
                    Destroy(targetDot);
                }
            }
        }
    }

    Vector3 CalcPos(Vector3 enemyPos)
    {
        Debug.Log("CALC");
        Vector3 dotPos = new Vector3(player.transform.position.x - enemyPos.x, player.transform.position.y - enemyPos.y, 0);
        return dotPos.normalized * (Vector3.Distance(player.transform.position, enemyPos) * scale);
    }
}
