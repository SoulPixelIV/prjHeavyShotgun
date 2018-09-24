using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUISystem : MonoBehaviour {

    public Texture2D healthbarEmpty;
    public Texture2D healthbarProgress;
    public float barProgress;

    public float posX;
    public float posY;

    public float sizeX;
    public float sizeY;

    void OnGUI()
    {
        GUI.BeginGroup(new Rect(posX, posY, sizeX, sizeY));
            GUI.Box(new Rect(0, 0, sizeX, sizeY), healthbarEmpty);

            GUI.BeginGroup(new Rect(0, 0, sizeX * barProgress, sizeY));
                GUI.Box(new Rect(0, 0, sizeX, sizeY), healthbarProgress);
            GUI.EndGroup();
        GUI.EndGroup();


    }

    // Update is called once per frame
    void Update () {
        barProgress = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>().health / 100;
	}
}
