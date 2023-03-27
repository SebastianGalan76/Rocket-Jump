using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class UISettings : MonoBehaviour
{
    [SerializeField] private Slider soundSlider, musicSlider;
    [SerializeField] private GameObject[] languageFlags;
    [SerializeField] private Text TResolutionValue;
    [SerializeField] private Toggle TFullScreenValue;
    [SerializeField] private CameraSystem cameraSystem;
    [SerializeField] private SoundSystem soundSystem;

    private int resolutionID;
    private string[] resolutions = new string[10] { "1920x1080", "1280x720", "1600x900", "1024x576", "960x720", "1024x768", "1600x1200", "1920x1440", "3840x2160", "4096x2160" };

    private UITextCollection UITextCollection;

    public int languageID;

    private void Awake()
    {
        UITextCollection = GetComponent<UITextCollection>();
    }

    public void LoadSettings()
    {
        ResolutionID = int.Parse(FileManager.LoadData("settings/resolutionID"));
        TResolutionValue.text = resolutions[ResolutionID];

        if (FileManager.LoadData("settings/fullScreen") == "1")
        {
            TFullScreenValue.isOn = true;
        }
        else
        {
            TFullScreenValue.isOn = false;
        }
        ChangeScreenResolution();

        if (PlayerPrefs.GetInt("FirstLoad") == 0)
        {
            languageID = SteamLanguage.GetSteamLanguageID();
            FileManager.SaveData("settings/languageID", languageID);
        }
        else
        {
            languageID = int.Parse(FileManager.LoadData("settings/languageID"));
        }

        MusicBackground.instance.ChangeMusicVolume(float.Parse(FileManager.LoadData("settings/musicVolume")));
        musicSlider.value = float.Parse(FileManager.LoadData("settings/musicVolume"));
        soundSystem.soundVolume = float.Parse(FileManager.LoadData("settings/soundVolume"));
        soundSlider.value = soundSystem.soundVolume;

        ChangeLanguage();
    }
    public void SaveSettings()
    {
        ChangeScreenResolution();
        FileManager.SaveData("settings/resolutionID", ResolutionID);

        if (TFullScreenValue.isOn)
        {
            FileManager.SaveData("settings/fullScreen", 1);
        }
        else
        {
            FileManager.SaveData("settings/fullScreen", 0);
        }

        FileManager.SaveData("settings/soundVolume", soundSystem.soundVolume);

        FileManager.SaveData("settings/musicVolume", MusicBackground.instance.GetVolume());
    }

    //Resolution
    public void ChangeScreenResolution()
    {
        int xIndex = resolutions[ResolutionID].IndexOf("x");
        int.TryParse(resolutions[ResolutionID].Substring(0, xIndex), out int x);
        int.TryParse(resolutions[ResolutionID].Substring(xIndex + 1), out int y);

        Screen.SetResolution(x, y, TFullScreenValue.isOn);

        if (SceneManager.GetActiveScene().name != "Game") { return; }
        if (ResolutionID == 4 || ResolutionID == 5 || ResolutionID == 6)
        {
            cameraSystem.MaxXPos = 20f;
            cameraSystem.MinXPos = -4f;
        }
        else
        {
            cameraSystem.MaxXPos = 18f;
            cameraSystem.MinXPos = -2f;
        }
    }
    public void ChangeResolutionID(bool next)
    {
        if (next)
        {
            ResolutionID++;
        }
        else
        {
            ResolutionID--;
        }
        TResolutionValue.text = resolutions[ResolutionID];
    }

    //Audio
    public void SetAudioVolume(float vol)
    {
        soundSystem.soundVolume = vol;
    }
    public void SetMusicVolume(float vol)
    {
        MusicBackground.instance.ChangeMusicVolume(vol);
    }

    //Language
    public void SetLanguage(int languageID)
    {
        this.languageID = languageID;
        ChangeLanguage();
        FileManager.SaveData("settings/languageID", languageID);
    }
    private void ChangeLanguage()
    {
        for (int i = 0; i < languageFlags.Length; i++)
        {
            languageFlags[i].transform.localScale = Vector3.one;
        }
        languageFlags[languageID].transform.localScale = new Vector3(1.2f, 1.2f, 1f);

        if (languageID == 0)
        {
            TranslateSystem.localeID = "en_GB";
        }
        else if (languageID == 1)
        {
            TranslateSystem.localeID = "pl_PL";
        }
        else if (languageID == 2)
        {
            TranslateSystem.localeID = "de_DE";
        }
        else if (languageID == 3)
        {
            TranslateSystem.localeID = "es_ES";
        }
        else if (languageID == 4)
        {
            TranslateSystem.localeID = "fr_FR";
        }

        UITextCollection.ChangeLanguage();
    }

    public int ResolutionID
    {
        set
        {
            resolutionID = value;
            if (resolutionID >= 10)
            {
                resolutionID = 0;
            }
            if (resolutionID < 0)
            {
                resolutionID = 9;
            }
        }
        get
        {
            return resolutionID;
        }
    }
}