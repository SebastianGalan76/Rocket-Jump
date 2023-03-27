using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UISystem : MonoBehaviour
{
    [SerializeField] private Text levelNrText;
    [SerializeField] private GameObject POptions, PCredits;
    [SerializeField] private GameSystem gameSystem;
    [SerializeField] private Animator newGameWarning;

    private UICircle circle;
    private UISettings settings;

    private void Awake()
    {
        circle = GetComponent<UICircle>();
        settings = GetComponent<UISettings>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Game")
        {
            Time.timeScale = 1;
            circle.HideCircle();
        }
        settings.LoadSettings();
    }

    public void StartNewGame(int value)
    {
        switch (value)
        {
            case 0:
                if (FileManager.LoadData("currentGame/time") != "0")
                {
                    newGameWarning.SetBool("Show", true);
                }
                else
                {
                    FileManager.StartNewGame();
                    LoadGame();
                }
                break;
            case 1:
                FileManager.StartNewGame();
                LoadGame();
                break;
            case 2:
                newGameWarning.SetBool("Show", false);
                break;
        }
    }
    public void LoadGame()
    {
        if (FileManager.LoadData("currentGame/levelIsCompleted") == "1")
        {
            StartNewGame(1);
            return;
        }

        circle.ShowCircle();

        StartCoroutine(wait());
        IEnumerator wait()
        {
            yield return new WaitForSecondsRealtime(0.4f);
            SceneManager.LoadScene("Game");
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void ChangeLevelNr(int levelNr)
    {
        levelNrText.text = levelNr.ToString(); ;
    }

    public void ShowOptions(bool value)
    {
        POptions.SetActive(value);
        if (!value)
        {
            settings.SaveSettings();
        }
    }
    public void ShowCredits(bool value)
    {
        PCredits.SetActive(value);
    }

    public void GoToMainMenu()
    {
        gameSystem.SaveGame();
        circle.ShowCircle();

        StartCoroutine(wait());
        IEnumerator wait()
        {
            yield return new WaitForSecondsRealtime(0.8f);
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void SaveGame()
    {
        gameSystem.SaveGame();
    }
}