using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour {
    public Transform player;

    public BackgroundSystem bgSystem;
    public GameSystem gameSystem;

    private const float LEVEL_HEIGHT = 154f;
    private const float CAMERA_SIZE = 5.5f;

    int stageNr, lastStageNr;

    private float _maxXPos = 18f, _minXPos = -1f;
    private float posX, posY;

    private void Update() {
        posX = getXPos();
        posY = getYPos();

        stageNr = getStageNr();

        if(stageNr != lastStageNr) {
            transform.position = new Vector3(posX, posY + (stageNr * LEVEL_HEIGHT), -10);
        }
        lastStageNr = stageNr;


        transform.position = Vector3.Lerp(transform.position, new Vector3(posX, posY + (stageNr * LEVEL_HEIGHT), -10), 5f * Time.deltaTime);
    }

    //Load main camera after game starts
    public void LoadCamera() {
        posX = getXPos();
        posY = getYPos();

        stageNr = getStageNr();

        transform.position = new Vector3(posX, posY + (stageNr * LEVEL_HEIGHT), -10);
        bgSystem.LoadCamera(posX, posY + (stageNr * LEVEL_HEIGHT));
    }

    private float getXPos()
    {
        float posX = player.position.x;
        if (posX < _minXPos)
        {
            posX = _minXPos;
        }
        else if (posX > _maxXPos)
        {
            posX = _maxXPos;
        }

        return posX;
    }
    private float getYPos()
    {
        float posY = player.transform.position.y % LEVEL_HEIGHT;

        if (posY % LEVEL_HEIGHT > LEVEL_HEIGHT - CAMERA_SIZE)
        {
            posY = LEVEL_HEIGHT - CAMERA_SIZE;
        }
        if (posY < CAMERA_SIZE)
        {
            posY = CAMERA_SIZE;
        }

        return posY;
    }
    private int getStageNr()
    {
        return (int)player.position.y / (int)LEVEL_HEIGHT;
    }

    public float MaxXPos {
        set {
            _maxXPos = value;
        }
    }
    public float MinXPos {
        set {
            _minXPos = value;
        }
    }
}
