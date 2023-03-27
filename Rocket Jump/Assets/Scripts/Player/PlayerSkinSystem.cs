using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinSystem : MonoBehaviour {
    [SerializeField] SpriteRenderer playerSkin, cannonSkin;
    [SerializeField] UIInfoPanel info;

    [HideInInspector] public int currentSkinID;

    //If this variable isn't equal to -1, it means that the player is into collision with another skin and can be changed.
    int triggerSkinObjectID = -1;
    private Animator skinTrigger;

    public Sprite[] playerSkinSprite;

    public Sprite[] cannonSkinSpriteStage0;
    public Sprite[] cannonSkinSpriteStage1;
    public Sprite[] cannonSkinSpriteStage2;
    public Sprite[] cannonSkinSpriteStage3;

    public GameObject[] skinObj;
    public Animator[] skinDecorationObj;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.E)) {
            if (triggerSkinObjectID == -1) { return; }

            int lastSkinID = currentSkinID;
            ChangeSkin(GetSkinObjID(triggerSkinObjectID));
            triggerSkinObjectID = lastSkinID;
        }
    }

    public void ChangeSkin(int skinObjID) {
        int lastSkinID = currentSkinID;
        int.TryParse(skinObj[skinObjID].name, out currentSkinID);

        //Change the player skin
        playerSkin.sprite = playerSkinSprite[currentSkinID];
        cannonSkin.sprite = cannonSkinSpriteStage0[currentSkinID];

        //Change the skin object
        skinObj[skinObjID].name = lastSkinID.ToString();
        skinObj[skinObjID].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = playerSkinSprite[lastSkinID];
        skinObj[skinObjID].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = cannonSkinSpriteStage0[lastSkinID];

        FileManager.SaveData("stats/skinObj" + skinObjID, 1);
        SteamAchievements.CheckPlayerSkin();

        //If the player first time change this skin show info about it.
        if(FileManager.LoadData("currentGame/skin" + skinObjID) == "1") {
            FileManager.SaveData("currentGame/skin" + skinObjID, 0);
            info.ShowPInfo(1);
        }
    }

    public void LoadSkinSystem() {
        int.TryParse(FileManager.LoadData("currentGame/player/currentSkinID"), out currentSkinID);
        playerSkin.sprite = playerSkinSprite[currentSkinID];
        cannonSkin.sprite = cannonSkinSpriteStage0[currentSkinID];

        for(int i = 0; i < skinObj.Length; i++) {
            int.TryParse(FileManager.LoadData("currentGame/skinObj" + i), out int skinID);
            skinObj[i].name = skinID.ToString();
            skinObj[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = playerSkinSprite[skinID];
            skinObj[i].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = cannonSkinSpriteStage0[skinID];
        }
    }
    public void SaveSkinSystem() {
        FileManager.SaveData("currentGame/player/currentSkinID", currentSkinID);

        for(int i = 0; i < skinObj.Length; i++) {
            float.TryParse(skinObj[i].name, out float skinID);
            FileManager.SaveData("currentGame/skinObj" + i, skinID);
        }
    }

    public void ChangeCannonVisibility(int stage) {
        switch(stage) {
            case 0:
                cannonSkin.sprite = cannonSkinSpriteStage0[currentSkinID];
                break;
            case 1:
                cannonSkin.sprite = cannonSkinSpriteStage1[currentSkinID];
                break;
            case 2:
                cannonSkin.sprite = cannonSkinSpriteStage2[currentSkinID];
                break;
            case 3:
                cannonSkin.sprite = cannonSkinSpriteStage3[currentSkinID];
                break;
        }
    }

    private int GetSkinObjID(int objName) {
        for(int i = 0; i < skinObj.Length; i++) {
            if(objName.ToString() == skinObj[i].name) {
                return i;
            }
        }
        return -1;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "SkinObject") {
            int.TryParse(collision.gameObject.name, out triggerSkinObjectID);
            skinTrigger = skinDecorationObj[GetSkinObjID(triggerSkinObjectID)];
            skinTrigger.SetBool("Show", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.tag == "SkinObject") {
            triggerSkinObjectID = -1;
            skinTrigger.SetBool("Show", false);
        }
    }
}
