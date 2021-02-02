using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers;
using UnityEngine;
using UnityEngine.UI;
using PlayfulSystems.ProgressBar;
using PlayfulSystems;
using DG.Tweening;
// using DungeonArchitect;
using UnityEngine.SceneManagement;

//Active Panel then when the time out clear and unload scene according locType to change
public class DungeonLeaveUI : MonoBehaviour
{

    public static DungeonLeaveUI instance;
    // public Dungeon dungeon;
    public UIPanel panel;
    public Text moneyT;
    public Text dustT;
    public Text expT;
    public Text remainT;
    public ProgressBarPro pb;
    public float time = 10.0f;
    private PlayerData players;

    private void Start()
    {

        players = FindObjectOfType<PlayerData>();
       PreLevel();
    }


   
    void PreLevel()
    {
        // DungeonManager.instance.isfirstLoad = true;
        StartCoroutine(LoopRoutine());
        
    
    }

    IEnumerator LoopRoutine()
    {
        bool canLeave = false;
        if (ConsoleManager.MONEY != null && ConsoleManager.DUST != null && ConsoleManager.EXP != null )
        {
            moneyT.text = ConsoleManager.MONEY.ToString();
            players.money += ConsoleManager.MONEY;
            dustT.text = ConsoleManager.DUST.ToString();
            players.dust += ConsoleManager.DUST;
            expT.text = ConsoleManager.EXP.ToString();
            players.experience+= ConsoleManager.EXP;
        }
        else
        {
            Debug.Log("INVALID OBJECT");
        }
        
        while (time > 0f)
        {
            yield return new WaitForSeconds(1.0f);
            remainT.text=time.ToString();
            pb.SetValue(time,10);
            --time;
           
            
            

            if (time == 0)
            {
                canLeave = true;
            }
        }

        Debug.Log("Can leave");


        if (canLeave == true)
        {
            PlayerData.LOCTYPE = LocType.Map;
            // dungeon.DestroyDungeon();
            // DestroyImmediate(DungeonManager.instance.playerObj);
            Camera.main.enabled = false;
            DungeonUIManager.instance.ClearOldData();
             canLeave = false;
            panel.Close();
            //Start Leave
            SceneReloader.instance.ReturnFromDungeon(BattleStartInfo.sceneName);
           
           
        }

        yield return 0;
    }
}
