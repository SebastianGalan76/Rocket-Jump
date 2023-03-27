using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSystemMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] backgrounds;
    [SerializeField] private GameObject[] tiles;

    private void Start()
    {
        //Load background depending on the player's position.
        float posY = float.Parse(FileManager.LoadData("currentGame/player/posY"));

        int i = (int)(posY / 154);
        backgrounds[i].SetActive(true);
        tiles[i].SetActive(true);
    }
}
