using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class UICircle : MonoBehaviour
{
    [SerializeField] private Animator circle;
    [SerializeField] private Color32[] circleColor;

    [SerializeField] private Transform player;

    public void ShowCircle(int circleColorID = -1)
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            Time.timeScale = 0f;
            
            Vector2 playerPos = player.position;
            Vector2 viewportPoint = Camera.main.WorldToViewportPoint(playerPos);

            circle.GetComponent<RectTransform>().anchorMin = viewportPoint;
            circle.GetComponent<RectTransform>().anchorMax = viewportPoint;
        }

        ChangeCircleColor(circleColorID);

        circle.transform.localScale = Vector3.zero;
        circle.gameObject.SetActive(true);

        circle.Play("ShowCircle");
    }
    public void HideCircle(int circleColorID = -1)
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            Time.timeScale = 0f;

            Vector2 playerPos = player.position;
            Vector2 viewportPoint = Camera.main.WorldToViewportPoint(playerPos);

            circle.GetComponent<RectTransform>().anchorMin = viewportPoint;
            circle.GetComponent<RectTransform>().anchorMax = viewportPoint;
        }

        ChangeCircleColor(circleColorID);

        circle.transform.localScale = new Vector3(50, 50, 0);
        circle.gameObject.SetActive(true);

        circle.Play("HideCircle");

        StartCoroutine(wait());
        IEnumerator wait()
        {
            yield return new WaitForSeconds(3f);
            circle.gameObject.SetActive(false);
        }

        StartCoroutine(wait2());
        IEnumerator wait2()
        {
            yield return new WaitForSecondsRealtime(0.8f);
            GameSystem.isPaused = false;
            Time.timeScale = 1f;
        }
    }

    private void ChangeCircleColor(int circleColorID)
    {
        int levelHight = 154;

        if (circleColorID == -1)
        {
            float posY = float.Parse(FileManager.LoadData("currentGame/player/posY"));

            int currentLevel = (int)posY / levelHight;
            circle.gameObject.GetComponent<Image>().color = circleColor[currentLevel];
        }
        else
        {
            circle.gameObject.GetComponent<Image>().color = circleColor[circleColorID];
        }
    }
}