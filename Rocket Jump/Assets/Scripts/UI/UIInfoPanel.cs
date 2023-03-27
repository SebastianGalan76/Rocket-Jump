using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoPanel : MonoBehaviour
{
    [SerializeField] private Sprite[] PInfo_icons;

    private Animator animator;
    private Image icon;
    private Text text;

    private void Start()
    {
        animator = GetComponent<Animator>();
        icon = transform.Find("Icon").GetComponent<Image>();
        text = transform.Find ("Info").GetComponent<Text>();
    }

    public void ShowPInfo(int infoID)
    {
        animator.Play("ShowPInfo");

        switch (infoID)
        {
            case 1: //skin
                text.text = FileManager.CountData("skin") + "/12";
                icon.sprite = PInfo_icons[0];
                break;
            case 2: //secretRoom
                text.text = FileManager.CountData("secretRoom") + "/4";
                icon.sprite = PInfo_icons[1];
                break;
            case 3: //wiseMovement
                text.text = FileManager.CountData("wiseMovement") + "/3";
                icon.sprite = PInfo_icons[2];
                break;
        }
    }
}