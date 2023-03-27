using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIGameCompleted : MonoBehaviour
{
    [SerializeField] private Text jumps, falls, deaths, wiseMovement, secretRoom, playerSkin, time;
    [SerializeField] private Animator gameCompletedAnim;
    [SerializeField] private TimeSystem timeSystem;

    private UICircle circle;

    private void Awake()
    {
        circle = GetComponent<UICircle>();
    }

    public void ShowGameCompleted()
    {
        time.text = timeSystem.getTimeFormatted();

        jumps.text = SteamAchievements.JumpAmountCurrentGame.ToString();
        falls.text = SteamAchievements.FallAmountCurrentGame.ToString();
        deaths.text = SteamAchievements.DeathAmountCurrentGame.ToString();

        wiseMovement.text = CountData("wiseMovement", 3) + " / 3";
        secretRoom.text = CountData("secretRoom", 4) + " / 4";
        playerSkin.text = CountData("skin", 12) + " / 12";

        circle.ShowCircle();
        gameCompletedAnim.gameObject.SetActive(true);
        gameCompletedAnim.Play("GameCompleted");

        int CountData(string elementName, int maxValue)
        {
            int counter = 0;
            for (int i = 0; i < maxValue; i++)
            {
                if (int.Parse(FileManager.LoadData("currentGame/" + elementName + i)) == 0)
                {
                    counter++;
                }
            }

            return counter;
        }
    }
}