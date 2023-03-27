using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    [SerializeField] private Text time;
    private static float timeValue;

    private void Start()
    {
        LoadTime();
    }

    private void Update()
    {
        timeValue += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        time.text = getTimeFormatted();
    }

    public string getTimeFormatted()
    {
        int hour = (int)timeValue / 3600;
        int minute = (int)timeValue / 60;
        int second = (int)timeValue % 60;

        string timeMinute, timeSecond;

        if (hour > 0)
        {
            if (minute < 10)
            {
                timeMinute = "0" + minute;
            }
            else
            {
                timeMinute = minute.ToString();
            }
            if (second < 10)
            {
                timeSecond = "0" + second;
            }
            else
            {
                timeSecond = second.ToString();
            }

            return hour + ":" + timeMinute + ":" + timeSecond;
        }
        else
        {
            if (minute < 10)
            {
                timeMinute = "0" + minute;
            }
            else
            {
                timeMinute = minute.ToString();
            }
            if (second < 10)
            {
                timeSecond = "0" + second;
            }
            else
            {
                timeSecond = second.ToString();
            }
            return timeMinute + ":" + timeSecond;
        }
    }

    private void LoadTime()
    {
        timeValue = float.Parse(FileManager.LoadData("currentGame/time"));
    }

    public static float TimeValue
    {
        get { return timeValue; }
        set { timeValue = value; }
    }
}
