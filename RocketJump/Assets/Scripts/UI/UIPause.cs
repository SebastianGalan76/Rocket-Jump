using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPause : MonoBehaviour {
    [SerializeField] private GameObject PPause;
    [HideInInspector] public static bool isPaused;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
        }
    }

    public void PauseGame() {
        isPaused = !isPaused;

        if (isPaused) {
            Time.timeScale = 0f;
            PPause.SetActive(true);
        } else {
            Time.timeScale = 1f;
            PPause.SetActive(false);
        }
    }
}
