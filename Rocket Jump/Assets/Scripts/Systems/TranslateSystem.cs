using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class TranslateSystem : MonoBehaviour {
    static string filePath = "Assets/Resources/Locale.xml";
    static TextAsset localeFile;

    static XmlDocument doc;
    static XmlNode locale;

    public static string localeID = "pl_PL";

    public static TranslateSystem instance;

    private void Awake() {
        LoadDocument();

        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        } else if(instance != this) {
            Destroy(gameObject);
        }
    }

    public static string LoadValue(string elementName) {
        if(doc == null) { LoadDocument(); }
        locale = doc.SelectSingleNode("//" + localeID);

        return locale.SelectSingleNode(elementName).InnerText;
    }

    static void LoadDocument() {
        doc = new XmlDocument();
        if(Application.platform == RuntimePlatform.WindowsEditor) {
            doc.Load(filePath);
            locale = doc.SelectSingleNode("//" + localeID);
        } else if(Application.platform == RuntimePlatform.WindowsPlayer) {
            localeFile = (TextAsset)Resources.Load("Locale");
            doc.LoadXml(localeFile.text);
        }
    }
}
