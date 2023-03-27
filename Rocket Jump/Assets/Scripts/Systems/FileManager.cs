using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Xml;
using System.IO;


public class FileManager : MonoBehaviour {
    static string filePath = "Assets/Resources/GameDataClear.xml";
    static TextAsset gameData;

    static XmlDocument doc;

    public static FileManager instance;

    private void Awake() {
        doc = new XmlDocument();
        LoadDocument();

        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        } else if(instance != this) {
            Destroy(gameObject);
        }

        void LoadDocument() {
            if(Application.platform == RuntimePlatform.WindowsEditor) {
                doc.Load(filePath);
            } else if(Application.platform == RuntimePlatform.WindowsPlayer) {
                if(!File.Exists(Application.persistentDataPath + "/GameData.xml")) {
                    gameData = (TextAsset)Resources.Load("GameData");
                    doc.LoadXml(gameData.text);
                } else {
                    doc.Load(Application.persistentDataPath + "/GameData.xml");
                    if(LoadData("fileVersion") == "NULL") {
                        XmlDocument doc2 = new XmlDocument();
                        gameData = (TextAsset)Resources.Load("GameData");
                        doc2.LoadXml(gameData.text);
                        CopyData();
                        doc2.Save(Application.persistentDataPath + "/GameData.xml");
                        doc.Load(Application.persistentDataPath + "/GameData.xml");

                        void CopyData() {
                            doc2.SelectSingleNode("//game/currentGame/time").InnerText = LoadData("currentGame/time");
                            doc2.SelectSingleNode("//game/currentGame/player/currentSkinID").InnerText = LoadData("currentGame/currentSkinID");
                            doc2.SelectSingleNode("//game/currentGame/currentGameStats/jumpAmount").InnerText = LoadData("currentGame/jumpAmount");
                            doc2.SelectSingleNode("//game/currentGame/currentGameStats/fallAmount").InnerText = LoadData("currentGame/fallAmount");
                            doc2.SelectSingleNode("//game/currentGame/currentGameStats/deathAmount").InnerText = LoadData("currentGame/deathAmount");

                            for(int i = 0; i < 12; i++) {
                                doc2.SelectSingleNode("//game/currentGame/skinObj" + i).InnerText = LoadData("currentGame/skinObj" + i);
                                doc2.SelectSingleNode("//game/currentGame/skin" + i).InnerText = LoadData("currentGame/skin" + i);
                            }
                            for(int i = 0; i < 4; i++) {
                                doc2.SelectSingleNode("//game/currentGame/secretRoom" + i).InnerText = LoadData("currentGame/secretRoom" + i);
                            }
                            for(int i = 0; i < 3; i++) {
                                doc2.SelectSingleNode("//game/currentGame/wiseMovement" + i).InnerText = LoadData("currentGame/wiseMovement" + i);
                            }

                            doc2.SelectSingleNode("//game/stats/jumpAmount").InnerText = LoadData("stats/jumpAmount");
                            doc2.SelectSingleNode("//game/stats/fallAmount").InnerText = LoadData("stats/fallAmount");
                            doc2.SelectSingleNode("//game/stats/deathAmount").InnerText = LoadData("stats/deathAmount");

                            for(int i = 0; i < 12; i++) {
                                doc2.SelectSingleNode("//game/stats/skinObj" + i).InnerText = LoadData("stats/skinObj" + i);
                            }
                            for(int i = 0; i < 4; i++) {
                                doc2.SelectSingleNode("//game/stats/secretRoom" + i).InnerText = LoadData("stats/secretRoom" + i);
                            }
                            for(int i = 0; i < 3; i++) {
                                doc2.SelectSingleNode("//game/stats/wiseMovement" + i).InnerText = LoadData("stats/wiseMovement" + i);
                            }

                            doc2.SelectSingleNode("//game/settings/resolutionID").InnerText = LoadData("settings/resolutionID");
                            doc2.SelectSingleNode("//game/settings/fullScreen").InnerText = LoadData("settings/fullScreen");
                            doc2.SelectSingleNode("//game/settings/languageID").InnerText = LoadData("settings/languageID");
                            doc2.SelectSingleNode("//game/settings/soundVolume").InnerText = LoadData("settings/soundVolume");
                        }
                    }
                }
            }

            SteamAchievements.LoadStats();
        }
    }

    public static void StartNewGame() {
        SaveData("currentGame/levelIsCompleted", 0);
        SaveData("currentGame/time", 0);

        SaveData("currentGame/player/posX", -2.5f);
        SaveData("currentGame/player/posY", 2.2f);
        SaveData("currentGame/player/currentSkinID", 0);

        SaveData("currentGame/player/velocityX", 0);
        SaveData("currentGame/player/velocityY", 0);

        SaveData("currentGame/currentGameStats/fallAmount", 0);
        SaveData("currentGame/currentGameStats/deathAmount", 0);
        SaveData("currentGame/currentGameStats/jumpAmount", 0);

        SaveData("currentGame/player/playerIsDead", 0);

        for(int i = 0; i < 12; i++) {
            SaveData("currentGame/skinObj" + i, i + 1);
            SaveData("currentGame/skin" + i, 1);
        }
        for(int i = 0; i < 4; i++) {
            SaveData("currentGame/secretRoom" + i, 1);
        }
        for(int i = 0; i < 3; i++) {
            SaveData("currentGame/wiseMovement" + i, 1);
        }

        SteamAchievements.LoadStats();

        SaveDocument();
    }

    public static string LoadData(string path) {
        if(path == "settings/musicVolume" && doc.SelectSingleNode("//game/" + path) == null) {
            CreateData("settings", "musicVolume", "0,15");
        }

        if(doc.SelectSingleNode("//game/" + path) == null) {
            Debug.Log("Incorrect path: " + path);
            return "NULL";
        }

        return doc.SelectSingleNode("//game/" + path).InnerText;
    }

    public static void SaveData(string path, float value) {
        doc.SelectSingleNode("//game/" + path).InnerText = value.ToString();

        SaveDocument();
    }

    public static int CountData(string elementName) {
        int value = 0;

        int iMax = 0;
        if(elementName == "skin") {
            iMax = 12;
        } else if(elementName == "secretRoom") {
            iMax = 4;
        } else if(elementName == "wiseMovement") {
            iMax = 3;
        }

        for(int i = 0; i < iMax; i++) {
            if(LoadData("currentGame/" + elementName + i) == "0") {
                value++;
            }
        }

        return value;
    }
    static void SaveDocument() {
        if(Application.platform == RuntimePlatform.WindowsEditor) {
            doc.Save(filePath);
        } else if(Application.platform == RuntimePlatform.WindowsPlayer) {
            doc.Save(Application.persistentDataPath + "/GameData.xml");
        }
    }

    static void CreateData(string parentPath, string name, string value) {
        XmlNode parent = doc.SelectSingleNode("//game/" + parentPath);
        XmlElement newElement = doc.CreateElement(name);
        parent.AppendChild(newElement);
        newElement.InnerText = value;

        SaveDocument();
    }

}