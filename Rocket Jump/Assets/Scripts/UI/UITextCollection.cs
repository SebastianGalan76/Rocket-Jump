using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class UITextCollection : MonoBehaviour
{
    [SerializeField] private Text Ttime, Tjumps, Tfalls, Tdeaths, TnewGame, TloadGame, TwarningLine1, TwarningLine2, Tyes, Tno, Texit, TtrailerMusic, TsoundsInGame, TsoundsInGame_Info, Tgraphics, TBack;
    [SerializeField] private Text[] TmainMenu, Toptions, Tcredits;

    //Settings panel
    [SerializeField] private Text Tresolution, TfullScreen, Tlanguage, Tsounds, Tmusic;

    public void ChangeLanguage()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            Ttime.text = TranslateSystem.LoadValue("time");
            Tjumps.text = TranslateSystem.LoadValue("jumps");
            Tfalls.text = TranslateSystem.LoadValue("falls");
            Tdeaths.text = TranslateSystem.LoadValue("deaths");
            TmainMenu[1].text = TranslateSystem.LoadValue("mainMenu");
            TBack.text = TranslateSystem.LoadValue("back");
        }
        else
        {
            TnewGame.text = TranslateSystem.LoadValue("newGame");
            TloadGame.text = TranslateSystem.LoadValue("loadGame");
            Tyes.text = TranslateSystem.LoadValue("yes");
            Tno.text = TranslateSystem.LoadValue("no");
            TwarningLine1.text = TranslateSystem.LoadValue("newGameWarningLine1");
            TwarningLine2.text = TranslateSystem.LoadValue("newGameWarningLine2");
            Texit.text = TranslateSystem.LoadValue("exit");
            TmainMenu[1].text = TranslateSystem.LoadValue("mainMenu");
            Tcredits[0].text = TranslateSystem.LoadValue("credits");
            Tcredits[1].text = TranslateSystem.LoadValue("credits");
            TtrailerMusic.text = TranslateSystem.LoadValue("trailerMusic");
            TsoundsInGame.text = TranslateSystem.LoadValue("soundsInGame");
            TsoundsInGame_Info.text = TranslateSystem.LoadValue("soundsInGame-Info");
            Tgraphics.text = TranslateSystem.LoadValue("graphics");
        }

        //Settings panel
        Tresolution.text = TranslateSystem.LoadValue("resolution");
        TfullScreen.text = TranslateSystem.LoadValue("fullScreen");
        TmainMenu[0].text = TranslateSystem.LoadValue("mainMenu");
        Toptions[0].text = TranslateSystem.LoadValue("options");
        Toptions[1].text = TranslateSystem.LoadValue("options");
        Tlanguage.text = TranslateSystem.LoadValue("language");
        Tsounds.text = TranslateSystem.LoadValue("sounds");
        Tmusic.text = TranslateSystem.LoadValue("music");
    }
}