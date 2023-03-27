using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadSystem : MonoBehaviour {
    [SerializeField] private Transform playerT;
    [SerializeField] private MovingObject[] movingObjects;

    private Player player;

    private void Start()
    {
        player = playerT.GetComponent<Player>();
    }

    public void SaveGame()
    {
        SavePlayer();
        SaveTime();
        SaveStats();
        SaveMovingObjects();

        void SavePlayer()
        {
            FileManager.SaveData("currentGame/player/posX", playerT.position.x);
            FileManager.SaveData("currentGame/player/posY", playerT.position.y);

            Vector2 velocity = player.Velocity;
            FileManager.SaveData("currentGame/player/velocityX", velocity.x);
            FileManager.SaveData("currentGame/player/velocityY", velocity.y);

            if (player.IsDead)
            {
                FileManager.SaveData("currentGame/player/playerIsDead", 1);
            }
            else
            {
                FileManager.SaveData("currentGame/player/playerIsDead", 0);
            }

        }
        void SaveTime()
        {
            FileManager.SaveData("currentGame/time", TimeSystem.TimeValue);
        }
        void SaveStats()
        {
            FileManager.SaveData("currentGame/currentGameStats/jumpAmount", SteamAchievements.JumpAmountCurrentGame);
            FileManager.SaveData("currentGame/currentGameStats/fallAmount", SteamAchievements.FallAmountCurrentGame);
            FileManager.SaveData("currentGame/currentGameStats/deathAmount", SteamAchievements.DeathAmountCurrentGame);

            FileManager.SaveData("stats/jumpAmount", SteamAchievements.JumpAmount);
            FileManager.SaveData("stats/deathAmount", SteamAchievements.DeathAmount);
            FileManager.SaveData("stats/fallAmount", SteamAchievements.FallAmount);
        }
        void SaveMovingObjects()
        {
            float posX, posY;
            int targetID;
            bool moveBack;
            for (int i = 0; i < movingObjects.Length; i++)
            {
                movingObjects[i].SaveTrap(out posX, out posY, out targetID, out moveBack);

                FileManager.SaveData("currentGame/traps/trap"+i + "/posX", posX);
                FileManager.SaveData("currentGame/traps/trap"+i + "/posY", posY);
                FileManager.SaveData("currentGame/traps/trap"+i + "/target", targetID);
                if (moveBack)
                {
                    FileManager.SaveData("currentGame/traps/trap"+i + "/moveBack", 1);
                }
                else
                {
                    FileManager.SaveData("currentGame/traps/trap"+i + "/moveBack", 0);
                }
            }
        }
    }
    public void LoadGame()
    {
        LoadPlayer();
        LoadTime();
        LoadStats();
        LoadTraps();

        void LoadPlayer()
        {
            float playerPosX, playerPosY;
            float.TryParse(FileManager.LoadData("currentGame/player/posX"), out playerPosX);
            float.TryParse(FileManager.LoadData("currentGame/player/posY"), out playerPosY);
            playerT.transform.position = new Vector3(playerPosX, playerPosY);

            float velocityX, velocityY;
            float.TryParse(FileManager.LoadData("currentGame/player/velocityX"), out velocityX);
            float.TryParse(FileManager.LoadData("currentGame/player/velocityY"), out velocityY);
            player.Velocity = new Vector2(velocityX, velocityY);

            if (FileManager.LoadData("currentGame/player/playerIsDead")=="1")
            {
                player.IsDead = true;
            }
            else
            {
                player.IsDead = false;
            }
        }
        void LoadTime()
        {
            float timeValue;
            float.TryParse(FileManager.LoadData("currentGame/time"), out timeValue);
            TimeSystem.TimeValue = timeValue;
        }
        void LoadStats()
        {
            SteamAchievements.JumpAmountCurrentGame = int.Parse(FileManager.LoadData("currentGame/currentGameStats/jumpAmount"));
            SteamAchievements.FallAmountCurrentGame = int.Parse(FileManager.LoadData("currentGame/currentGameStats/fallAmount"));
            SteamAchievements.DeathAmountCurrentGame = int.Parse(FileManager.LoadData("currentGame/currentGameStats/deathAmount"));

            SteamAchievements.JumpAmount = int.Parse(FileManager.LoadData("stats/jumpAmount"));
            SteamAchievements.FallAmount = int.Parse(FileManager.LoadData("stats/fallAmount"));
            SteamAchievements.DeathAmount = int.Parse(FileManager.LoadData("stats/deathAmount"));
        }
        void LoadTraps()
        {
            float posX, posY;
            int targetID;
            bool moveBack;

            for (int i = 0; i < movingObjects.Length; i++)
            {
                posX = float.Parse(FileManager.LoadData("currentGame/traps/trap"+i + "/posX"));
                posY = float.Parse(FileManager.LoadData("currentGame/traps/trap"+i + "/posY"));
                targetID = int.Parse(FileManager.LoadData("currentGame/traps/trap"+i + "/target"));
                if (FileManager.LoadData("currentGame/traps/trap" + i + "/target") == "1")
                {
                    moveBack = true;
                }
                else
                {
                    moveBack = false;
                }

                movingObjects[i].LoadTrap(posX, posY, targetID, moveBack);
            }
        }
    }
    
}
