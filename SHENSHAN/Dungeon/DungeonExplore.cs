using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers;
using UnityEngine.UI;
using PlayfulSystems;
using Mirror;
using TMPro;
public enum DungeonType{
   None,
   Init,
    Kill,
   Explore,
   Boss,
   Escape,
   Collect,
   Hard,
}

public enum DungeonDifficult{
    Normal,
    Hard,
    Evil,
}

//when loc is dungeon load ExploreManager and start goalCounter
//when DungeonCurrent>Goal,Show Boss until die unlock the exit and give reward
//Loc -> TownManagerActive Map.
public class DungeonExplore : MonoBehaviour
{
    public static DungeonExplore instance;
   public UIPanel panel;
   public MapLocation mapLocation;
   public EnemyDeckSelection enemySelection;
   public DiscoverManager rewardItems;
   public GameObject monsterAsset;
   public GameObject bossAsset;
   public GameObject dropItem;
   public GameObject dungeonChest;
   //加载地下城事件
   public DungeonEvent Chaos;
   public List<GameObject> enemyAssets;
   public List<GameObject> bossList;
   public List<GameObject> dropItems;
   public List<GameObject> dungeonIcons;
   [Header("Goal")]
   public  int current;
   public int goal;
   public DungeonDifficult dungeonDifficult =DungeonDifficult.Normal;
   [Header("Dungeon Info")]
   public TextMeshProUGUI currentText;
   public TextMeshProUGUI goalText;
   public Text currentStateText;
   [Header("Dungeon RP Panel")] 
   public Text dungeonStateText;
   public TextMeshProUGUI moneyText;
   public TextMeshProUGUI dustText;
   public TextMeshProUGUI expText;
   public TextMeshProUGUI othersText;

   public TextMeshProUGUI rewardText;
   public TextMeshProUGUI arText;
   [Header("Dungeon CP")] 
   public Text RMText;
   public Text RDText;
   public Text ROText;
   
   //
  
   public int maxEnemyCount=3;
   public ProgressBarPro slider;
   public bool hasCurrentDone=false;
   public bool hasKillBoss=false;
   public bool canLeave=false;
   public bool canGotItems = false;
   public bool hasClear = false;
   public bool hasBossPack = false;
   public Transform monsterPos;
   public Transform bossPos;
   public Transform itemSPos;
   public Transform dungeonEffectPos;
   public GameObject bossPanel;
   public GameObject enemyPanel;
   public GameObject itemPanel;
   public GameObject dungeonEffectIcon; 
   public UIPanel returnPanel;
   
   public Button readyStartBtn;
   public Button dCollectionBtn;

   [Header("Select Dungeon Effect Icon")]
   public TextMeshProUGUI nameText;
   public TextMeshProUGUI detailText;
   public TextMeshProUGUI rewardBounsText;
   public TextMeshProUGUI enemyBounsText;

   public TextMeshProUGUI absorbEntityText;
   //dungeon collect pool add when dungeon leave with win state
   public static int moneyPool;
   public static int dustPool;
   public static int specialsPool;
   public static int expPool;
   
   #region Difficult Points

   public float DyDifficultPoints = 0.0f;
    
    //DUNGEON BOUNS
    public static int DUNGEONEXTRAPOINTS = 0;
   public static float DUNGEONEXTRAITEMPERC=0f;
   public static int DATK=0;
   public static int DCATK=0;
   //
   public static int DESD=0;
   public static int DDUR=0;
   //
   public static int DArmor=0;
   public static int DCARMOR=0;
   //
   public static int DHHeal=0;
   public static int DCHeal=0;
   public static int DEReources=0;

   public static int DUNGEONENEMYBOUNS=0;
   public static float DUNGEONREWARDBOUNS=0;
   
   #endregion

   [Header("Pack")] public Text packText;
   public Image avaSprite;

   void Start()
   {
       instance = this;
   }

  void Update(){

      if(panel.isActiveAndEnabled){
     slider.SetValue(current,goal);
    //  if (BattleStartInfo.DungeonDifficult == "普通")
    //  {
         currentText.text = DialogueLua.GetVariable("DungeonMonster").asString;
         current = DialogueLua.GetVariable("DungeonMonster").asInt;
         goal = mapLocation.NeedsKill;
    //  }else if (BattleStartInfo.DungeonDifficult == "困难")
    //  {

    //      currentText.text = DialogueLua.GetVariable("DMPerc").asString;
    //      current = DialogueLua.GetVariable("DMPerc").asInt;
    //      goal =Mathf.FloorToInt(1.0f);
    //  }

     //Boss Battle
     if(current>=goal){
       Debug.Log("Boss Stage Show Boss \t\t\t\t\t");
          hasCurrentDone=true;

          if (hasClear == false)
          {
              hasClear = true;
              foreach (var en in enemyAssets)
              {
                  if (Application.isPlaying && en != null )
                  {
                      Destroy(en);
                  }
              }

              foreach (var ed in EnemyDeckSelection.instance.deckIcons)
              {
                  if (ed != null )
                  {
                      Destroy(ed);
                    
                  }
              }
          }

        //   enemyPanel.gameObject.SetActive(false);
        // bossPanel.gameObject.SetActive(true);
        if(bossAsset!=null){
        Debug.Log("Create Boss");
           GameObject eObj = TownManager.instance.LoadItemFromAB(mapLocation.bossAsset.model,mapLocation.bossAsset);
           int rnd = Random.Range(0,mapLocation.enemyPos.Count);
           eObj.transform.position = new Vector3(mapLocation.enemyPos[rnd].x,mapLocation.enemyPos[rnd].y,mapLocation.enemyPos[rnd].z);
           //
            eObj.GetComponent<Monster>().enemyAsset=mapLocation.bossAsset;
            // eObj.GetComponent<Monster>().avaSprite.sprite=mapLocation.bossAsset.Head;
            eObj.GetComponent<Monster>().converName=mapLocation.bossAsset.conver;
            // eObj.GetComponent<Monster>().frameSprite.sprite=mapLocation.bossAsset.Frame;
            // eObj.GetComponent<Monster>().HP = mapLocation.bossAsset.Health;
            Debug.Log("Now Monster health is"+eObj.GetComponent<Monster>().HP);
            eObj.GetComponent<Monster>().cardList=new List<CardAsset>(mapLocation.bossAsset.cardList);
            
                        
bossList.Add(eObj);
            // NetworkServer.Spawn(eObj);

            }
    
        
        if (hasBossPack == false)
        {
            Debug.Log("Generate boss pack");
            if (mapLocation.bossAsset.hasCard)
            {
                GameObject enemyPack =
                    Instantiate(enemySelection.deckIcon, enemySelection.contentPos.position,
                        Quaternion.identity) as GameObject;
                enemyPack.transform.SetParent(enemySelection.contentPos);
                enemyPack.GetComponent<EnemyPortraitVisual>().enemyAsset = mapLocation.bossAsset;
                enemyPack.GetComponent<EnemyPortraitVisual>().EnemyHead.sprite = mapLocation.bossAsset.Head;
                //
                if(TownManager.CheckLan()==true){
                enemyPack.GetComponent<EnemyPortraitVisual>().EnemyName.text = mapLocation.bossAsset.EnemyName;
                }else{
                      enemyPack.GetComponent<EnemyPortraitVisual>().EnemyName.text = mapLocation.bossAsset.eEnemyName;
                }
                //
                enemyPack.GetComponent<EnemyPortraitVisual>().enemyCardList =
                    new List<CardAsset>(mapLocation.bossAsset.cardList);

                enemySelection.deckIcons.Add(enemyPack);
                hasBossPack = true;
            }
        }

        current = DialogueLua.GetVariable("DungeonBoss").asInt;
        currentText.text=current.ToString();
        goal=1;
        goalText.text=goal.ToString();
        currentStateText.text="击杀头目";
        
       DialogueLua.SetVariable("DungeonMonster",0);


       if(DialogueLua.GetVariable("DungeonBoss").asInt>0){
           panel.Close();
        //    TownManager.instance.leaveObj.SetActive(true);
       }
      }


        hasKillBoss = DialogueLua.GetVariable("KillBossDone").asBool;
      if(hasKillBoss==true){
        Debug.Log("========================CONSOLE  DUNGEON MODULE ============================");
          //Clear Boss 
          foreach(var b in bossList){
              if(b!=null){Destroy(b);}
          }
          currentStateText.text="已击杀首领";
          Debug.Log("Kill Boss Done und can leave the dungeon");
          //Win Panel Get Reward and leave dungeon if has conver then show the conversa
          DialogueLua.SetVariable("KillBossDone",true);
          canLeave=true;
          BattleStartInfo.IsWinner=true;
           panel.Close();
           if (mapLocation.hasEvent == false)
           {
            //    TownManager.instance.leaveObj.SetActive(true);
               LDRoutline(BattleStartInfo.IsWinner);
           }
           Debug.Log("================NEXT LOCATION MODULE==================");
           //Unlock Next Loc
           if(mapLocation.nextLocation.Count>0){
               for(int i=0;i<mapLocation.nextLocation.Count;i++){
                   //Unlock
                   for(int j=0;j<TravelSystem.TravelSystem.instance.areaCollection.Count;j++)
                       if(mapLocation.nextLocation[i] == TravelSystem.TravelSystem.instance.areaCollection[j].areaName){
                           TravelSystem.TravelSystem.instance.areaCollection[j].Locked=false;
                           TravelSystem.TravelSystem.instance.areaCollection[j].gameObject.SetActive(true);

                           PlayerPrefs.SetInt(TravelSystem.TravelSystem.instance.areaCollection[j].location.elocationName+"_Lock",0);
                       }
               }
           }
      }
      
      //Pack
      if (BattleStartInfo.SelectDeck != null)
      {
          packText.text = BattleStartInfo.SelectDeck.deckName.ToString();
          avaSprite.gameObject.SetActive(true);
          avaSprite.sprite = BattleStartInfo.SelectDeck.characterAsset.avatarImage;
          
      }
      else
      {
          packText.text = "";
         avaSprite.gameObject.SetActive(false);
      }
      }
      

      ////////////FAILED EXPLORE ////////////////////
      //give up explore leave relative
      if (canLeave == true && hasKillBoss==false && TownManager.instance.atDungeon==true)
      {
          Debug.Log("Failed Explore Dungeon Return Map");
          BattleStartInfo.IsWinner = false;
          
              canGotItems = false;
              LDRoutline(BattleStartInfo.IsWinner);
              ClearDungeon();
      }
      
      //
      moneyText.text = moneyPool.ToString();
      dustText.text = dustPool.ToString();
      othersText.text = specialsPool.ToString();
      expText.text = expPool.ToString();

      rewardText.text=DUNGEONREWARDBOUNS*100+"%";
      arText.text=DUNGEONENEMYBOUNS.ToString();

      
      //extra
      readyStartBtn.interactable = (BattleStartInfo.SelectDeck != null);
  }

 public void LDRoutline(bool iswinner)
  {
      if(ConsoleManager.GetReward==true){
      returnPanel.gameObject.SetActive(true);
      if (iswinner == false)
      {
          dungeonStateText.text = "失败直接移动区域入口";
          moneyPool = 0;
          dustPool = 0;
          specialsPool=0;
          expPool = 0;
          BattleStartInfo.SelectEnemyDeck = null;
          BattleStartInfo.SelectDeck = null;
          

      }
      else if(iswinner==true && BattleStartInfo.IsWinner==true && TownManager.instance.atDungeon==true
              && hasKillBoss==true)
      {
            ConsoleManager.GetReward=false;
          dungeonStateText.text = "成功探索并获得物品";
          PlayerData.localPlayer.money += Mathf.FloorToInt(moneyPool * DUNGEONREWARDBOUNS);
          PlayerData.localPlayer.dust +=Mathf.FloorToInt( dustPool*DUNGEONREWARDBOUNS);
          PlayerData.localPlayer.special += Mathf.FloorToInt(specialsPool*DUNGEONREWARDBOUNS);
          PlayerData.localPlayer.experience +=Mathf.FloorToInt( expPool*DUNGEONREWARDBOUNS);
      }

      
      //
      RMText.text = moneyPool.ToString();
      RDText.text = dustPool.ToString();
      ROText.text = expPool.ToString();
      
      //
      DialogueManager.SendUpdateTracker();
      }
  }


  public void AddBouns(DungeonEventClass dec){
      //Add bouns for player
      switch(dec.DET){
          case DungeonEventType.ATK:
            DATK += dec.deAmount;
          break;

          case DungeonEventType.HEAL:
            DHHeal+=dec.deAmount;
          break;

          case DungeonEventType.HARMOR:
            DArmor += dec.deAmount;
          break;

          case DungeonEventType.CARMOR:
            DCARMOR += dec.deAmount;
          break;
          case DungeonEventType.CATK:
            DCATK += dec.deAmount;
          break;
         
          case DungeonEventType.ESD:
            DESD += dec.deAmount;
          break;

          case DungeonEventType.WDUR:
            DDUR += dec.deAmount;
            break;
          
      }
      //Add bouns for enemy
      DUNGEONENEMYBOUNS+= dec.DAAmount;

      //reward Bounds
      DUNGEONREWARDBOUNS += dec.DEReward;

      Debug.Log("DUNGEONENEMYBOUNS IST==>"+DUNGEONENEMYBOUNS.ToString());

  }

 public void ConfigDungeon()
 {
     Debug.Log("Config Dungeon");
     
     //reset Data
     foreach (GameObject ea in enemyAssets)
     {
         if (ea != null)
         {
             Destroy(ea);
         }
     }
     foreach (GameObject ea in bossList)
     {
         if (ea != null)
         {
             Destroy(ea);
         }
     }


     //Init State
     canLeave = false;
     hasKillBoss = false;
     hasCurrentDone = false;
     TownManager.instance.PreSelectPack = true;
     current = 0;
     //Dungeon Bouns
     DUNGEONENEMYBOUNS=0;
     DUNGEONEXTRAITEMPERC=0;
     DUNGEONEXTRAPOINTS=0;
     DUNGEONREWARDBOUNS=0;
     

    //
    nameText.text="";
    detailText.text="";
    rewardBounsText.text="";
    absorbEntityText.text="";
    enemyBounsText.text="";
    rewardText.text="0%";
    arText.text="0";

    //
     DialogueLua.SetVariable("DungeonMonster", 0);
     DialogueLua.SetVariable("KillBossDone",false);
     DialogueLua.SetVariable("DungeonBoss", 0);
     int getBoss = DialogueLua.GetVariable("DungeonBoss").asInt;
    
     DialogueManager.SendUpdateTracker();

     //
     panel.gameObject.SetActive(true);
//
Debug.Log("DungeonType Ist"+mapLocation.dungeonType);
     //Type for game Mode
     switch (mapLocation.dungeonType)
     {
         case DungeonType.Kill:
             //kill enemy enough then unlock boss
             current = DialogueLua.GetVariable("DungeonMonster").asInt;
             currentText.text = current.ToString();
             goal = mapLocation.NeedsKill;
             goalText.text = goal.ToString();
             slider.SetValue(current / goal, goal);

             currentStateText.text = string.Format("击败{0}个怪物后发现首领", goal);
             //Load Enemy
             LoadEnemy();
             break;
         case DungeonType.Boss:
             //just kill the boss
             hasCurrentDone = true;
             DialogueLua.SetVariable("DungeonMonster",3);
             
             LoadBoss();
             break;

            case DungeonType.Init:
            LoadBoss();
            break;

         case DungeonType.Explore:

             break;
        case DungeonType.Hard:
          
        break;
     }

 }

 //enemy
    public void LoadEnemy(){ 
        int m=0;
        Debug.Log(" Load Normal Monster");
        for(int i=0; i<TownManager.instance.mapLocation.NeedsKill ;i++){
            int j = Random.Range(0,mapLocation.enemyList.Count);
            //  if(mapLocation.enemyList[j].npcType
            //      ==NpcType.Enemy  || mapLocation.enemyList[j].npcType ==NpcType.Others){
            GameObject eObj = TownManager.instance.LoadItemFromAB(mapLocation.enemyList[j].model,mapLocation.enemyList[j]);
           
            eObj.transform.position = new Vector3(mapLocation.enemyPos[m].x,mapLocation.enemyPos[m].y,mapLocation.enemyPos[m].z);
            m++;
            // Monster mo=eObj.AddComponent<Monster>().GetComponent<Monster>();

            eObj.transform.parent=monsterPos;
            eObj.name=mapLocation.enemyList[j].EnemyName;
           
            //     Debug.Log("Has monster");
            eObj.GetComponent<Monster>().enemyAsset=mapLocation.enemyList[j];
            // // eObj.GetComponent<Monster>().avaSprite.sprite=mapLocation.enemyList[rnd].Head;
            // // eObj.GetComponent<Monster>().frameSprite.sprite=mapLocation.enemyList[rnd].Frame;
            // eObj.GetComponent<Monster>().HP = mapLocation.enemyList[j].Health;
            // Debug.Log("Now Monster health is"+eObj.GetComponent<Monster>().HP);
            if(mapLocation.enemyList[j].hasCard==true){
                    eObj.GetComponent<Monster>().cardList=new List<CardAsset>(mapLocation.enemyList[j].cardList);
            }
         
            eObj.GetComponent<Monster>().converName=mapLocation.enemyList[j].conver;

            //Pack
            if(mapLocation.enemyList[j].hasCard==true){
                //enemy has card
               GameObject enemyPack = Instantiate(enemySelection.deckIcon,enemySelection.contentPos.position,Quaternion.identity)as GameObject;
                       enemyPack.transform.parent = enemySelection.contentPos;
                       enemyPack.GetComponent<EnemyPortraitVisual>().enemyAsset = mapLocation.enemyList[j];
                       enemyPack.GetComponent<EnemyPortraitVisual>().EnemyHead.sprite = mapLocation.enemyList[j].Head;
                       if(TownManager.CheckLan()){
                       enemyPack.GetComponent<EnemyPortraitVisual>().EnemyName.text = mapLocation.enemyList[j].EnemyName;
                       }else{
                           enemyPack.GetComponent<EnemyPortraitVisual>().EnemyName.text = mapLocation.enemyList[j].eEnemyName;
                       }
                       //
                       enemyPack.GetComponent<EnemyPortraitVisual>().enemyCardList = new List<CardAsset>(mapLocation.enemyList[j].cardList);
                       
                       enemySelection.deckIcons.Add(enemyPack);
            }

            enemyAssets.Add(eObj);
            //  }
        }
        
     
        var dm =  FindObjectOfType<DungeonManager>();
        if(dm!=null){
            for(int i=0;i<dm.enemiesList.Count;i++){
                for(int j=0;j<mapLocation.enemyList.Count;j++)
                    if(dm.enemiesList[i].name == mapLocation.enemyList[j].EnemyName){
                        var eObj = dm.enemiesList[i];
                        eObj.transform.parent=monsterPos;
          
            eObj.name=mapLocation.enemyList[j].EnemyName;
           
            //     Debug.Log("Has monster");
            eObj.GetComponent<Monster>().enemyAsset=mapLocation.enemyList[j];
            // // eObj.GetComponent<Monster>().avaSprite.sprite=mapLocation.enemyList[rnd].Head;
            // // eObj.GetComponent<Monster>().frameSprite.sprite=mapLocation.enemyList[rnd].Frame;
            // eObj.GetComponent<Monster>().HP = mapLocation.enemyList[j].Health;
            // Debug.Log("Now Monster health is"+eObj.GetComponent<Monster>().HP);
            if(mapLocation.enemyList[j].hasCard==true){
                    eObj.GetComponent<Monster>().cardList=new List<CardAsset>(mapLocation.enemyList[j].cardList);
            }
         
            eObj.GetComponent<Monster>().converName=mapLocation.enemyList[j].conver;
                    }
            }
        }
   
    
    }
 public void ShowTip(DungeonIcon icon){
    if(icon!=null){
        nameText.text= icon.dvc.deName.ToString();
        detailText.text=icon.dvc.deDetail.ToString();
        rewardBounsText.text=icon.dvc.DEReward*100+"%";
        absorbEntityText.text=icon.dvc.DAAmount.ToString();
    }
}
public void LoadBoss(){
     if(bossAsset!=null){
        Debug.Log("Create Boss");
           GameObject eObj = TownManager.instance.LoadItemFromAB(mapLocation.bossAsset.model,mapLocation.bossAsset);
           int rnd = Random.Range(0,mapLocation.enemyPos.Count);
           eObj.transform.position = new Vector3(mapLocation.enemyPos[rnd].x,mapLocation.enemyPos[rnd].y,mapLocation.enemyPos[rnd].z);
           //
            eObj.GetComponent<Monster>().enemyAsset=mapLocation.bossAsset;
            // eObj.GetComponent<Monster>().avaSprite.sprite=mapLocation.bossAsset.Head;
            eObj.GetComponent<Monster>().converName=mapLocation.bossAsset.conver;
            // eObj.GetComponent<Monster>().frameSprite.sprite=mapLocation.bossAsset.Frame;
            // eObj.GetComponent<Monster>().HP = mapLocation.bossAsset.Health;
            // Debug.Log("Now Monster health is"+eObj.GetComponent<Monster>().HP);
            eObj.GetComponent<Monster>().cardList=new List<CardAsset>(mapLocation.bossAsset.cardList);
            
bossList.Add(eObj);
            // NetworkServer.Spawn(eObj);

            }
    
}
  public void AddItemToInventotry(Items item){
      PlayerData.localPlayer.CmdAddItems(item.itemID);
    
    
  }

  
  /// <summary>
  /// canleave and health<0 || currgoal lose and lost all items u got+
  /// canleave and health >0 || cur>goal && boss is kill win and got items 
  /// give up lost items from dungeon und 
  /// </summary>
  public void LeaveDungeon()
  {
      canLeave = true;
      BattleStartInfo.SelectEnemyDeck = null;
      BattleStartInfo.SelectDeck = null;
  }

  public void ClearDungeon()
  {
      canLeave = false;
      hasCurrentDone = false;
      hasKillBoss = false;
      canGotItems = false;
      hasClear = false;
      hasBossPack = false;
      DUNGEONEXTRAPOINTS = 0;
      
      //Clear Loc
      mapLocation = null;
      TownManager.instance.PreSelectPack = false;
      //clear Enemy
      foreach (var e in enemyAssets)
      {
          if (e != null)
          {
              Destroy(e);
          }
      }
      //boss
      foreach (var e in bossList)
      {
          if (e != null)
          {
              Destroy(e);
          }
      }

      var dm = FindObjectOfType<DungeonManager>();
      if(dm!=null){
          for(int i=0;i<dm.enemiesList.Count;i++){
              Destroy(dm.enemiesList[i]);
          }
      }
      
      //ItemList
      foreach (var i in dropItems)
      {
          if (i != null)
          {
              Destroy(i);
          }
      }
      //dungeon Icon
      foreach (var c in dungeonIcons)
      {
          if (c != null)
          {
              Destroy(c);
          }
      }
      
      panel.gameObject.SetActive(false);
      BattleStartInfo.SelectDeck = null;
      DialogueLua.SetVariable("DungeonMonster",0);
      DialogueLua.SetVariable("DungeonBoss",0);
      DialogueLua.SetVariable("KillBossDone",false);
      //panel
      bossPanel.SetActive(false);
      enemyPanel.SetActive(true);
  }

    #region Dungeon Event
    public void LoadChaos(){
        //(3)Stage show select for the dungeon first when select ,add target values to the dungeon object .
        // Chaos.ShowDungeonEvent();
    }

    #endregion


    #region DungeonChest
public void ShowLoot(List<Items> item){

}
    #endregion
}
