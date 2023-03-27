using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameSystem : MonoBehaviour {
    public static bool isPaused = false;

    [SerializeField] private UICircle circle;
    [SerializeField] private LevelSystem level;

    public Transform playerT;

    private new CameraSystem camera;
    public PlayerSkinSystem playerSkinSystem;

    public GameObject[] secretRoom;
    public GameObject[] wiseMovement;

    public SaveLoadSystem saveLoadSystem;

    public TrailRenderer[] trails;

    private void Start() {
        camera = Camera.main.GetComponent<CameraSystem>();

        LoadGame();
    }

    public void LoadGame() {
        saveLoadSystem.LoadGame();
        camera.LoadCamera();
        playerSkinSystem.LoadSkinSystem();

        int currentLevel = level.GetCurrentLevel();

        level.ChangeLevelNr(currentLevel + 1);
        level.ChangeBackground(currentLevel / 14);

        TurnOnTrail();

        for(int i = 0; i < secretRoom.Length; i++) {
            if(int.Parse(FileManager.LoadData("currentGame/secretRoom" + i)) == 0) {
                secretRoom[i].SetActive(false);
            }
        }
        for(int i = 0; i < wiseMovement.Length; i++) {
            if(int.Parse(FileManager.LoadData("currentGame/wiseMovement" + i)) == 0) {
                wiseMovement[i].SetActive(false);
            }
        }

        circle.HideCircle();
    }

    public void SaveGame() {
        saveLoadSystem.SaveGame();

        playerSkinSystem.SaveSkinSystem();
    }

    public void TurnOnTrail() {
        for(int i = 0; i < trails.Length; i++) {
            trails[i].enabled = true;
        }
    }

    private void OnApplicationQuit() {
        SaveGame();
    }
}
