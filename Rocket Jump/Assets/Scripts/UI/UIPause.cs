using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPause : MonoBehaviour
{
    [SerializeField] private GameObject PPause;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        GameSystem.isPaused = !GameSystem.isPaused;

        if (GameSystem.isPaused)
        {
            Time.timeScale = 0f;
            PPause.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            PPause.SetActive(false);
        }
    }
}