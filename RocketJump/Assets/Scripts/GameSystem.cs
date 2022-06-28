using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameSystem : MonoBehaviour {
    [SerializeField] private Transform playerT;

    [SerializeField] private GameObject[] secretRoom;
    [SerializeField] private GameObject[] wiseMovement;

    [SerializeField] private BackgroundSystem bgSystem;
    [SerializeField] private PlayerSkinSystem playerSkinSystem;
    [SerializeField] private SaveLoadSystem saveLoadSystem;
    [SerializeField] private UISystem ui;

    [SerializeField] private TrailRenderer[] trails;
    [SerializeField] private UICircle circle;

    private new CameraSystem camera;
    private int currentLevel, lastLevel;

    private int levelsInOneStage = 14;
    private float oneLevelHeight = 11;

    private void Awake() {
        camera = Camera.main.GetComponent<CameraSystem>();
    }

    private void Start() {
        LoadGame();
    }

    private void FixedUpdate() {
        currentLevel = (int)(playerT.position.y / oneLevelHeight);

        if(currentLevel != lastLevel) {
            if(currentLevel > lastLevel && currentLevel % levelsInOneStage == 0) {
                bgSystem.ChangeBackground(currentLevel / levelsInOneStage);
            }
            if(currentLevel < lastLevel && lastLevel % levelsInOneStage == 0) {
                bgSystem.ChangeBackground(currentLevel / levelsInOneStage);
            }

            if(currentLevel < lastLevel) {
                SteamAchievements.FallAmount += 1;
                SteamAchievements.FallAmountCurrentGame += 1;
            }

            ui.ChangeLevelNr(currentLevel + 1);
            if(currentLevel >= levelsInOneStage) {
                SteamAchievements.UnlockAchievement("NEW_ACHIEVEMENT_1_0");
            }
            lastLevel = currentLevel;
        }
    }

    public void LoadGame() {
        saveLoadSystem.LoadGame();
        currentLevel = (int)(playerT.position.y / oneLevelHeight);

        lastLevel = currentLevel;

        ui.ChangeLevelNr(currentLevel + 1);

        camera.LoadCamera();

        bgSystem.ChangeBackground(currentLevel / levelsInOneStage);
        playerSkinSystem.LoadSkinSystem();

        TurnOnTrail();
        circle.HideCircle();

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
