using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour {
    [SerializeField]private Transform player;
    [SerializeField]private BackgroundSystem bgSystem;

    private float yPos;

    private int stageNr, lastStageNr;
    private float maxXPos = 18f, minXPos = -1f;

    private float stageSize = 154f;
    private float cameraSize;

    private void Awake() {
        cameraSize = GetComponent<Camera>().orthographicSize;
    }

    private void Update() {
        float x = player.position.x;
        if(x < MinXPos) {
            x = MinXPos;
        } else if(x > MaxXPos) {
            x = MaxXPos;
        }
        yPos = player.transform.position.y % stageSize;

        if(yPos % stageSize > (stageSize - cameraSize)) {
            yPos = stageSize - cameraSize;
        }
        if(yPos < cameraSize) {
            yPos = cameraSize;
        }

        stageNr = (int)(player.position.y / stageSize);
        if(stageNr != lastStageNr) {
            transform.position = new Vector3(x, yPos + (stageNr * stageSize), -10);
        }
        lastStageNr = stageNr;


        transform.position = Vector3.Lerp(transform.position, new Vector3(x, yPos + (stageNr * stageSize), -10), 5f * Time.deltaTime);
    }

    //Load main camera after game starts
    public void LoadCamera() {
        stageNr = (int)(player.position.y / stageSize);
        float x = player.position.x;
        if(x < minXPos) {
            x = minXPos;
        } else if(x > maxXPos) {
            x = maxXPos;
        }
        float y = player.transform.position.y % stageSize;

        if(yPos % stageSize > (stageSize - cameraSize)) {
            yPos = stageSize - cameraSize;
        }
        if(y < cameraSize) {
            y = cameraSize;
        }

        transform.position = new Vector3(x, y + (stageNr * stageSize), -10);
        bgSystem.LoadCamera(x, y + (stageNr * stageSize));
    }

    public float MaxXPos {
        set {
            maxXPos = value;
        }
        get {
            return maxXPos;
        }
    }
    public float MinXPos {
        set {
            minXPos = value;
        }
        get {
            return minXPos;
        }
    }
}
