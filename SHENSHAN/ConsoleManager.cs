using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using PixelCrushers.DialogueSystem;
using System.Linq;
using PixelCrushers;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;
using DG.Tweening;
using TravelSystem;

[System.Serializable]
public class console
{
  public console(int money, int exp, List<Items> item)
  {
    this.money = money;
    this.exp = exp;
    this.item = new List<Items>();
  }
public console(){}
  public int money;
  public int exp;
  public List<Items> item;
}

public enum SceneType
{
  None,
  Town,
  Dungeon,
  Battle,
  Map,
}
/// <summary>
/// Console over give money und item
/// when player return town 
/// </summary>
public class ConsoleManager : MonoBehaviour
{
public static bool frombt=false;
 public static SceneType sceneType =SceneType.None;
  public static ConsoleManager instance;
  private PlayerData p;
  public UIPanel panel;
  public static NetworkManagerShenShan manager;
  public UIPanel worldMap;
  public Text moneyText;
 
  public Text expText;
  public Text dustText;
  public DungeonExplore explore;
 
  //Got Item outside the town
  public static int MONEY;
  public static float EXP;
  public static int DUST;
  public Transform itemPos;
  public GameObject itemObj;
  public DiscoverManager discoverManager;
  public static CardAsset CARDASSET;
  

  public delegate void ReturnConsole(PlayerData p);
  public static ReturnConsole rc =null;


public static bool GetReward=false;
  public bool returnWithItems = false;
  


 
  void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    

//    DontDestroyOnLoad(this);
  }
 
  void Start()
  {
    
    p = PlayerData.localPlayer;
 manager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManagerShenShan>();
   
  
    
  }

//50% get items if has spawn to the itemslot und console
  void CollectItemForPlayer()
  {

    panel.gameObject.SetActive(true);
    TownManager.instance.enviormentPrefab.gameObject.SetActive(true);
      int gotMoney =Mathf.FloorToInt(Random.Range(BattleStartInfo.SelectEnemyDeck.enemyAsset.gold/2,BattleStartInfo.SelectEnemyDeck.enemyAsset.gold));
      // p.money += gotMoney;
      moneyText.text = gotMoney.ToString();
      
    
     int  gotExp=Mathf.FloorToInt(Random.Range(BattleStartInfo.SelectEnemyDeck.enemyAsset.exp/2,BattleStartInfo.SelectEnemyDeck.enemyAsset.exp));
    //  p.experience += gotExp;
      expText.text=gotExp.ToString();
      
      UIExp.instance.slider.Value = BattleStartInfo.SelectEnemyDeck.enemyAsset.exp;

      
      int gotDust =Mathf.FloorToInt(Random.Range(BattleStartInfo.SelectEnemyDeck.enemyAsset.dustReward/2,BattleStartInfo.SelectEnemyDeck.enemyAsset.dustReward));
      // p.dust += gotDust;
      dustText.text = gotDust.ToString();
    if (frombt && BattleStartInfo.IsWinner==true )
    {
      if (TownManager.instance.atDungeon == true)
      {

        if (BattleStartInfo.SelectEnemyDeck.enemyAsset != null)
        {
          //try get items
          Debug.Log("Try Get Item From Enemy==>"+BattleStartInfo.SelectEnemyDeck.enemyAsset.EnemyName.ToString());
          if (BattleStartInfo.SelectEnemyDeck.enemyAsset.itemShopIDs.Count>0)
          {
            int itemCounter=0;
            //rnd 1 items
            var enemyItemList= BattleStartInfo.SelectEnemyDeck.enemyAsset.itemShopIDs.ToList();
            //Got Rnd 1 items from enemy every defeat it with rnd(1-3) counter
            int rnd = Random.Range(0,enemyItemList.Count);
            int icrnd=Random.Range(1,3);
            itemCounter=icrnd;
              Items gotItem = ItemDatabase.instance.FindItemByName(BattleStartInfo.SelectEnemyDeck.enemyAsset.itemShopIDs[rnd]);
              Debug.Log("Roll Item ist =>"+gotItem.itemName.ToString()+"und Counter ist=>"+itemCounter);
              float rnds = Random.Range(0.0f, 1.0f);
              Debug.Log(rnds + "\t\tis Got items perc if higher than 1-iperc than can got");
              // if (rnds > (1 - gotItem.perc))
              // {
                //Got Items Add to canPickup Slot
                GameObject obj = Instantiate(DungeonExplore.instance.dropItem,
                  DungeonExplore.instance.itemSPos.position,
                  Quaternion.identity) as GameObject;
                obj.transform.parent = DungeonExplore.instance.itemSPos;
                obj.GetComponent<DungeonPickItems>().item = gotItem;
                obj.GetComponent<DungeonPickItems>().itemText.text = gotItem.itemName;
                obj.GetComponent<DungeonPickItems>().itemSprite.sprite = gotItem.icon;
                //Check item number 
                
                  obj.GetComponent<DungeonPickItems>().itemNumber += itemCounter;
                

                obj.GetComponent<DungeonPickItems>().numText.text =
                  obj.GetComponent<DungeonPickItems>().itemNumber.ToString();
                //
                DungeonExplore.instance.dropItems.Add(obj);
                NetworkServer.Spawn(obj);
              // }
              // else
              // {
              //   Debug.Log("NO Item Got");
              // }
            
            
          }
          else
          {
            Debug.Log("No Items");
          }
          
        }

        //
          if (BattleStartInfo.SelectEnemyDeck != null)
    {
     

      //reward Pool
      moneyText.text = gotMoney.ToString();
      expText.text = gotExp.ToString();
      dustText.text =gotDust.ToString();
      

      //

    }
       
        else
        {
          Debug.Log("No enemy asset ,maybe been clear");
        }
      }
      else
      {
        Debug.Log("TOWN NPC");
        //Town Npc
       

//Get one of reward if ddifficult is hard
// if(BattleStartInfo.DungeonDifficult=="Hard"){
//   Debug.Log("Hard Mode Add card und add perc for the current progress");
//   discoverManager.ShowDiscover(CardCollection.instance.allCardsArray,1,DiscoverType.HardMode);
// }
     //try to got items if rndv > 
      }
      //GetItems
    //  DiscoverManager.instance.ShowDiscover(CardCollection.instance.allCardsArray,1,DiscoverType.Rnd);
     Debug.Log("Select Event");
    //  DungeonExplore.instance.Chaos.ShowDungeonEvent();

      ConsoleManager.frombt = false;
     BattleStartInfo.SelectEnemyDeck=null;

     PlayerData.localPlayer.target=null;

     //
     //TODO TESTING
     //
//  DialogueLua.SetVariable("DungeonMonster",3);
    }
    else
    {
      ConsoleManager.frombt = false;
      BattleStartInfo.SelectEnemyDeck=null;
    }

    
  }
  public void ReturnCal(PlayerData ps)
  {

    TownManager.instance.cameraGroup.gameObject.SetActive(true);
    if(GetReward==true){
      //try get enemy model
      if(ps.target!=null){

      }
      //
      // GameObject obj =GameObject.Instantiate(DungeonExplore.instance.dungeonChest)as GameObject;
      // obj.transform.position =new Vector3(ps.target.transform.position.x,ps.target.transform.position.y+100,ps.target.transform.position.z);
      // NetworkServer.Spawn(obj);
      //
    CollectItemForPlayer();
    if (BattleStartInfo.SelectEnemyDeck != null)
    {
      //Update Dungeon Reward Pool
      int rndMoney = Mathf.FloorToInt(Random.Range(BattleStartInfo.SelectEnemyDeck.enemyAsset.gold / 2,
        BattleStartInfo.SelectEnemyDeck.enemyAsset.gold));
      int rndExp = Mathf.FloorToInt(Random.Range(BattleStartInfo.SelectEnemyDeck.enemyAsset.exp / 2,
        BattleStartInfo.SelectEnemyDeck.enemyAsset.exp));
      int rndDust = Mathf.FloorToInt(Random.Range(BattleStartInfo.SelectEnemyDeck.enemyAsset.dustReward / 2,
        BattleStartInfo.SelectEnemyDeck.enemyAsset.dustReward));


     Debug.Log("Return Console"+rndMoney +"::"+rndExp+"::"+rndDust);

      DungeonExplore.moneyPool += rndMoney;
      DungeonExplore.dustPool +=rndDust;
      DungeonExplore.expPool += rndExp;

      //reward Pool
      moneyText.text =Mathf.Clamp( DungeonExplore.moneyPool,0, DungeonExplore.moneyPool).ToString();
      expText.text = Mathf.Clamp(  DungeonExplore.dustPool,0, DungeonExplore.dustPool).ToString();
      dustText.text = Mathf.Clamp( DungeonExplore.expPool,0, DungeonExplore.expPool).ToString();

    }
    frombt=false;
    GetReward=false;
    }else{
      Debug.Log("Failed return map");
    }
  }
 
 public void CheckSceneState()
  {
    Debug.Log("CheckState");
    switch (sceneType)
    {
      case SceneType.None:
        //main menu ->game
        Debug.Log("Login To Town");
        break;
      case SceneType.Battle:
       
        //town|| dungeon->battle
        Debug.Log("enter to battle");
        DialogueManager.SendUpdateTracker();
        break;
      case SceneType.Town:
               
      if(p!=null){
        
        MenuManager.instance.content.gameObject.SetActive(false);
        TownManager.instance.ShowTown(true);
        DialogueManager.SendUpdateTracker();
        frombt = false;
      }
      break;
      case SceneType.Dungeon:
        Debug.Log("Enter To Dungeon");
        //Map->dungeon
        break;
  }
  }

 //Back To Town
 public void ShowTown()
 {
   //Show the npc model
   foreach(var e in TownManager.instance.npcModels){
		if(e!=null){
			e.gameObject.SetActive(true);
		}
	}
   if (PlayerData.LOCTYPE == LocType.Town)
   {
     p = PlayerData.localPlayer;
     PlayerData.onlinePlayers[name] = PlayerData.localPlayer;

    //  TownManager.instance.gameCamera.enabled = true;
     TownManager.instance.content.gameObject.SetActive(true);
     TownManager.instance.cameraGroup.gameObject.SetActive(true);
     TownManager.instance.enviormentPrefab.gameObject.SetActive(true);
    //  TownManager.instance.mainCamera.Priority=100;
//     TownManager.instance.ShowTown(true);
//  Debug.Log("Show");
     DialogueManager.SendUpdateTracker();
   }else if (PlayerData.LOCTYPE == LocType.Map)
   {
     p=PlayerData.localPlayer;
     PlayerData.onlinePlayers[name]=PlayerData.localPlayer;
     //
     TownManager.instance.worldMapCamera.enabled = true;
     TownManager.instance.gameCamera.enabled = false;
       worldMap.Open();
   
   }else if(PlayerData.LOCTYPE==LocType.Dungeon){
      p = PlayerData.localPlayer;
     PlayerData.onlinePlayers[name] = PlayerData.localPlayer;
     //Check is Boss
     bool isBossKill =DialogueLua.GetVariable("KillBossDone").asBool;
     if(isBossKill){

     }
     //
     TownManager.instance.enviormentPrefab.gameObject.SetActive(true);
     TownManager.instance.gameCamera.enabled = true;
     TownManager.instance.content.gameObject.SetActive(true);
     CollectItemForPlayer();
      DialogueManager.SendUpdateTracker();

   }

 }
  
}
