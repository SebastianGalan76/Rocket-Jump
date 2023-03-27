using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] private BackgroundSystem bgSystem;
    [SerializeField] private Transform player;
    [SerializeField] private UISystem ui;

    private int currentLevel, lastLevel;
    private int stageNr;

    private const int LEVEL_HEIGHT = 11;

    //The amount of levels in one stage
    private const int LEVELS_IN_ONE_STAGE = 14;

    private void FixedUpdate()
    {
        currentLevel = (int)player.position.y / LEVEL_HEIGHT;

        if (currentLevel != lastLevel)
        {
            stageNr = currentLevel / LEVELS_IN_ONE_STAGE;

            if (currentLevel > lastLevel && currentLevel % LEVELS_IN_ONE_STAGE == 0)
            {
                ChangeBackground(stageNr);
            }
            if (currentLevel < lastLevel && lastLevel % LEVELS_IN_ONE_STAGE == 0)
            {
                ChangeBackground(stageNr);
            }

            if (currentLevel < lastLevel)
            {
                SteamAchievements.FallAmount += 1;
                SteamAchievements.FallAmountCurrentGame += 1;
            }

            ChangeLevelNr(currentLevel);
            if (currentLevel >= LEVELS_IN_ONE_STAGE)
            {
                SteamAchievements.UnlockAchievement("NEW_ACHIEVEMENT_1_0");
            }
            lastLevel = currentLevel;
        }
    }

    public void ChangeBackground(int stageNr)
    {
        bgSystem.ChangeBackground(stageNr);
    }
    public void ChangeLevelNr(int levelNr)
    {
        ui.ChangeLevelNr(levelNr + 1);
    }
    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}
