using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
// using DAShooter.TwoD;
// using DungeonArchitect.Navigation;
using PixelCrushers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Dungeon UI Manager
//1.Collect Info
//2.Quest TrackInfo
//3.Inventory
public class DungeonUIManager : MonoBehaviour
{

    public static DungeonUIManager instance;
    

    private PlayerData playerData;

    public Items[] collectItems;

    public GameObject items;

    // public DungeonManager dungeon;
    public DungeonGoal dungeonGoal;
    public UIPanel bottomUI;
    public GameObject playerPanel;
    public DungeonCardCollection dungeonCard;
    // public UILoot loot;
    public UIPanel LeavePanel;
    public UIPanel configPanel;
    public GameObject battleConfigPanel;
    public UIPanel consoleDungeonPanel;
    //
    public Text moneyText;
    public Text dustText;
    public Text expText;
    public Text nameText;
    public Text seText;
    public Text minText;
    public GameObject BG;
    public bool isOpenUI;
    public InventorySystem inventory;

    public Button StopBtn;
    public Button PlayBtn;
    public Button LeaveBtn;
    public SceneReloader scene;
    
    private int second;
    private int minute;
    private bool isGaming=false;

    [Header("Common")] public GameObject dungeonUI;

    public GameObject dungeonMap;

    public Camera mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        playerData = FindObjectOfType<PlayerData>();
//        collectItems = GetComponents<Items>();
        inventory = GetComponent<InventorySystem>();


        BattleStartInfo.AtDungeon = true;


    }

  
    public IEnumerator TimerRoutine()
    {
        while(isGaming){
        yield  return new WaitForSeconds(1.0f);
       ++ second;
       seText.text = second.ToString();
       if (second == 60)
       {
           minute += 1;
           minText.text = minute.ToString();
           second = 0;
       }
       }

        yield return 0;
    }
  
    public void Init()
    {
        
        bottomUI.Open();
        if (playerData != null)
        {

            ClearOldData();
            LoadStatsFromConsole();
        }
      //
      isGaming = true;

        playerPanel.gameObject.SetActive(true);
    }
    private void LoadStatsFromConsole()
    {
        nameText.text = playerData.PlayerName.ToString();
        moneyText.text = ConsoleManager.MONEY.ToString();
        dustText.text = ConsoleManager.DUST.ToString();
        expText.text = ConsoleManager.EXP.ToString();
    }

    // Update is called once per frame
//     void Update()
//     {
        
// //        //
// //        while (BattleStartInfo.IsWinner == false)
// //        {
// //            DungeonLeaveUI.instance.panel.Open();
// //        }
//         StopBtn.gameObject.SetActive(!PlayBtn.gameObject.activeSelf);
        
//         StopBtn.onClick.AddListener(() => { Time.timeScale = 0;Debug.Log("Time Stop"+Time.deltaTime); });
//         PlayBtn.onClick.AddListener(() => { Time.timeScale = 1;});
//         LeaveBtn.onClick.AddListener(() =>
//         {
//             UIConfirmation.instance.Show("离开将损失在当前所收集的物品", () => {LeavePanel.Open();});
//         });
        
//         LoadStatsFromConsole();
//         //
//         seText.text = second.ToString();
//         minText.text = minute.ToString();

     
        
//            dungeonGoal.Current.text = dungeon.dungeonCurrent.ToString();
        

//         //
//         if(dungeon.dungeonCurrent >= dungeon.dungeonGoal){
//             Debug.Log("FinalBoss");
//             dungeonGoal.FinalBoss=true;
//             dungeonGoal.isFinish=true;
            
//         }


//     }

    private void SetStatsFromDungeon()
    {
        throw new System.NotImplementedException();
    }


    public void ClearOldData()
    {
        ConsoleManager.MONEY = 0;
        ConsoleManager.DUST = 0;
        ConsoleManager.EXP = 0;
        second = 0;
        minute = 0;
        
        
    }

    public void OpenInventory()
    {
        isOpenUI = true;
        Time.timeScale = 0;
        inventory.OpenCloseInventory(isOpenUI);
    }

    public void CloseInventory()
    {
        isOpenUI = false;
        Time.timeScale = 1;
    }

    /// <summary>
    /// Move to battlefield and hide the ui 
    /// </summary>
    public void ChangeBattlefield()
    {
        if (BattleStartInfo.SelectDeck != null && BattleStartInfo.SelectEnemyDeck != null)
        {
            mainCamera.enabled = false;
            dungeonMap.SetActive(false);
            dungeonUI.SetActive(false);
            BG.SetActive(false);
            //stop all npc ai
            // foreach (var npc in dungeon.npc.LimitNPC)
            // {
            //     var nps = npc.GetComponent<DungeonNavAgent2D>();
            //     if (nps == null) return;
            //     nps.Stop();
            // }
            //
            Debug.Log("Change Scene");
            
            //
           scene.DungeonChangeScene("MainBattleScene");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ReturnBattlefield()
    {
        mainCamera.enabled = true;
        dungeonMap.SetActive(true);
        dungeonUI.SetActive(true);
        BG.SetActive(true);
        consoleDungeonPanel.Open();
        BattleStartInfo.IsWinner = false;
    }
    
    

}
