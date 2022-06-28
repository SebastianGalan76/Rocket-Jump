using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoPanel : MonoBehaviour {
    [SerializeField] private Animator PInfo;
    [SerializeField] private Image PInfo_icon;
    [SerializeField] private Sprite[] PInfo_icons;
    [SerializeField] private Text PInfo_text;

    public void ShowPInfo(int infoID) {
        PInfo.Play("ShowPInfo");

        switch (infoID) {
            case 1: //skin
                PInfo_text.text = FileManager.CountData("skin") + "/12";
                PInfo_icon.sprite = PInfo_icons[0];
                break;
            case 2: //secretRoom
                PInfo_text.text = FileManager.CountData("secretRoom") + "/4";
                PInfo_icon.sprite = PInfo_icons[1];
                break;
            case 3: //wiseMovement
                PInfo_text.text = FileManager.CountData("wiseMovement") + "/3";
                PInfo_icon.sprite = PInfo_icons[2];
                break;
        }
    }
}
