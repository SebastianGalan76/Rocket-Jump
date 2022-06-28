using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSystem : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private Camera bgCamera;

    [SerializeField] private GameObject[] backgrounds;
    [SerializeField] private Color32[] colors;

    private float stageSize = 154f;

    private void Update() {
        bgCamera.transform.localPosition = new Vector3((player.localPosition.x / 5f) + 2.5f, (player.transform.position.y % stageSize) / 25f, -10);
    }

    //Change the camera position after the game starts.
    public void LoadCamera(float x, float y) {
        bgCamera.transform.localPosition = new Vector3(x, y, -10);
    }

    //Change background style //0-Summer, 1-Autumn, 2-Winter
    public void ChangeBackground(int stageNr) {
        for(int i = 0; i < backgrounds.Length; i++) {
            backgrounds[i].SetActive(false);
        }

        backgrounds[stageNr].SetActive(true);
        bgCamera.backgroundColor = colors[stageNr];
    }
}
