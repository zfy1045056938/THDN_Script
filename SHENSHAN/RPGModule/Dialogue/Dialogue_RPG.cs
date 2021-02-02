using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using TravelSystem;
using Cinemachine;
//For dialogue call rpg
public class Dialogue_RPG : MonoBehaviour
{
  public bool updateServerOnConverEnd = true;
  public bool updateServerOnQuestState = true;


  public bool allowLuaOutCon = true;
  public bool allowLuaToChangeData = true;


  void Awake()
  {
    Lua.RegisterFunction("UpdateServer", this, SymbolExtensions.GetMethodInfo(() => UpdateServer()));
    Lua.RegisterFunction("GetPlayerName", this, SymbolExtensions.GetMethodInfo(() => GetPlayerName()));
    Lua.RegisterFunction("OpenDeckBuilding", this, SymbolExtensions.GetMethodInfo(() => OpenDeckBuilding()));
    //Town
    Lua.RegisterFunction("GiveItems", this, SymbolExtensions.GetMethodInfo(() => GiveItems((Double)0)));
    Lua.RegisterFunction("GiveMoney", this, SymbolExtensions.GetMethodInfo(() => GiveMoney((double) 0)));
    Lua.RegisterFunction("AddExp", this, SymbolExtensions.GetMethodInfo(() => AddExp((float) 0)));
    Lua.RegisterFunction("OpenShop", this, SymbolExtensions.GetMethodInfo(() => OpenShop()));
      Lua.RegisterFunction("OpenNBPanel", this, SymbolExtensions.GetMethodInfo(() =>OpenNBPanel()));
    Lua.RegisterFunction("OpenPackSelection", this, SymbolExtensions.GetMethodInfo(() => OpenPackSelection()));
    Lua.RegisterFunction("GetNpc", this, SymbolExtensions.GetMethodInfo(() => GetNpc(string.Empty)));
    Lua.RegisterFunction("RemoveItem", this, SymbolExtensions.GetMethodInfo(() => RemoveItem(string.Empty)));
    Lua.RegisterFunction("RemoveItemAmount", this, SymbolExtensions.GetMethodInfo(() => RemoveItemAmount(string.Empty,(double)0)));
    Lua.RegisterFunction("CheckItemEnough", this, SymbolExtensions.GetMethodInfo(() => CheckItemEnough(string.Empty,(double)0)));
    Lua.RegisterFunction("CheckMoneyEnough", this, SymbolExtensions.GetMethodInfo(() => CheckMoneyEnough((double)0)));
    Lua.RegisterFunction("SetPlayerValue", this, SymbolExtensions.GetMethodInfo(() => SetPlayerValue(string.Empty,(double)0)));
    Lua.RegisterFunction("AddOthers", this, SymbolExtensions.GetMethodInfo(() => AddOthers((double)0)));
    Lua.RegisterFunction("LeaveTown", this, SymbolExtensions.GetMethodInfo(() => LeaveTown()));
    Lua.RegisterFunction("FaceNpc", this, SymbolExtensions.GetMethodInfo(() => FaceNpc()));
    Lua.RegisterFunction("LeaveNpc", this, SymbolExtensions.GetMethodInfo(() => LeaveNpc()));
    Lua.RegisterFunction("CheckHasPack", this, SymbolExtensions.GetMethodInfo(() => CheckHasPack()));
     Lua.RegisterFunction("OpenCraft", this, SymbolExtensions.GetMethodInfo(() => OpenCraft()));
    

    Lua.RegisterFunction("UnlockLoc",this,SymbolExtensions.GetMethodInfo(()=>UnlockLoc(string.Empty)));
    
    Lua.RegisterFunction("ChangeDungeon",this,SymbolExtensions.GetMethodInfo(()=>ChangeDungeon(string.Empty)));
    Lua.RegisterFunction("SetItemAmount",this,SymbolExtensions.GetMethodInfo(()=>SetItemAmount((Double)0,(Double)0)));
    //Dungeon
    Lua.RegisterFunction("Dungeon_BattleConfig", this, SymbolExtensions.GetMethodInfo(() => Dungeon_BattleConfig()));
    Lua.RegisterFunction("Dungeon_GetItems", this, SymbolExtensions.GetMethodInfo(() => Dungeon_GetItems((Double)0,(Double)0)));
    Lua.RegisterFunction("Dungeon_GetMoney", this, SymbolExtensions.GetMethodInfo(() => Dungeon_GetMoney((Double)0)));
    Lua.RegisterFunction("Dungeon_GetDust", this, SymbolExtensions.GetMethodInfo(() => Dungeon_GetDust((Double)0)));
    Lua.RegisterFunction("Dungeon_GetExp", this, SymbolExtensions.GetMethodInfo(() =>Dungeon_GetExp((Double)0)));
    
    //NBD
    Lua.RegisterFunction("NB_OpenCollection", this, SymbolExtensions.GetMethodInfo(() =>NB_OpenCollection()));
    Lua.RegisterFunction("NB_OpenInventory", this, SymbolExtensions.GetMethodInfo(() =>NB_OpenInventory()));
    
    //Module
     Lua.RegisterFunction("MatchesModule", this, SymbolExtensions.GetMethodInfo(() =>MatchesModule()));
     Lua.RegisterFunction("CheckFreeVersion", this, SymbolExtensions.GetMethodInfo(() =>CheckFreeVersion()));
    
    
  }

  public void UpdateServer()
  {
    var player = PlayerData.localPlayer;
    if (player) player.UpdateDiaSystemData();
  }

  public void OnQuestStateChange(string qName)
  {
    if (updateServerOnQuestState) UpdateServer();
  }

  public void OnConversactionEnd(Transform actor)
  {
    if (updateServerOnConverEnd) UpdateServer();
  }

  public static string GetPlayerName()
  {
    var player = PlayerData.localPlayer;
    return player ? player.name : "Player";
  }


  public void OpenDeckBuilding()
  {
    var deck = FindObjectOfType<DeckBuilderScreen>();
    deck.ShowScreenForCollectionBrower();
  }

  public void OpenShop()
  {
   var player = FindObjectOfType<PlayerData>().GetComponent<PlayerData>();
   if(player!=null){
     if(player.target!=null){
       if(player.target is NPCManager){
         FindObjectOfType<Merchant>().selectedMerchant = player.target.GetComponentInChildren<MerchantController>();
         player.target.merchantController.LoadMerchant();
       }
     }
   }
  }

  public void GiveItems(double itemID)
  {
    Items items= ItemDatabase.instance.FindItem((int)itemID);
    if (items != null)
    {
      InventorySystem.instance.AddItem(items);
    }
    else
    {
      Debug.Log("INVALID ITEMS");
    }
  }

  public void ChangeDungeon(string locName)
  {
     TownManager.instance.WorldMap();
     //Move To Target Area
     var travel = GameObject.FindObjectOfType<TravelSystem.TravelSystem>().GetComponent<TravelSystem.TravelSystem>();
     if (travel != null)
     {
       TravelArea loc = new TravelArea();
       
       //Got Area
       foreach (var g in travel.areaCollection)
       {
         if (g.areaName == locName)
         {
           loc = g;
         }
       }
       //
       travel.SetCurrentArea(loc);
       travel.areaWindow.SetTravelAreaWindow(loc);
      
       travel.SaveTravel();

       TownManager.instance.EnterTown(locName,true);
       TownManager.instance.worldMap.gameObject.SetActive(false);
       TownManager.instance.worldMapCamera.enabled=false;
       
     }

  }

  public void RemoveItem(string itemName)
  {
    var inventory = GameObject.FindWithTag("Inventory").GetComponent<InventorySystem>();
    if (inventory != null)
    {
      inventory.RemoveItemByName(itemName);
    }
  }

  public void RemoveItemAmount(string itemName,double amount)
  {
    var inventory = GameObject.FindWithTag("Inventory").GetComponent<InventorySystem>();
    if (inventory != null)
    {
      while (amount > 0)
      {
        inventory.RemoveItemByName(itemName);

        amount--;
      }
    }
  }

  public void GiveMoney(double amount)
  {
    var player = PlayerData.localPlayer;
    if (player) player.CmdAddMoney((int) amount);
    player.CmdAddDust((int) amount);
  }

  public void AddOthers(double amount){
     var Players = PlayerData.localPlayer;

     if(Players)Players.CmdAddOthers((int)amount);
  }

  public void OpenPackSelection()
  {
    var ps = GameObject.FindGameObjectWithTag("PlayerSelectDeck").GetComponent<DeckSelectionScreen>();
    if (ps != null)
    {
      ps.ShowScreen(true);
    }
  }

//
public void MatchesModule(){

}
  public void SetItemAmount(double itemId,double amount){
    var inventory = FindObjectOfType<InventorySystem>();
    Items it = ItemDatabase.instance.FindItem((int)itemId);
      while((int)amount>0){
        amount--;
        Debug.Log(it.itemName);
        
          for(int i =0;i<inventory.items.Count;i++){
             if(inventory.items[i].item.itemName == it.itemName
                && inventory.items[i].item.stackSize>0){
                inventory.RemoveItemFromSlot(inventory.items[i]);
                
             }
          }
        

        if(amount==0){
          DialogueLua.SetVariable("ItemEnough",true);
        }
      }
  }
  
  public void Confirmtion(string msg,string sceneName){
    var UIConfirm = FindObjectOfType<UIConfirmation>();
    if(UIConfirm != null){
      UIConfirmation.instance.Show(msg,()=>{
        //leave town and lock
        TownManager.instance.WorldMap();
        //load dungeon at world map
         SceneReloader.instance.ChangeScene(sceneName);   
      });
    }
  }

  public  void GetNpc(string npcName)
  {
    var townNPC = FindObjectOfType<TownManager>();
    if (townNPC != null)
    {
      Debug.Log(npcName+"Got");
      for (int i = 0; i < townNPC.locationEnemy.Count; i++)
      {
        if (townNPC.locationEnemy[i].EnemyName == npcName)
        {
          // PlayerPrefsX.SetBool(townNPC.locationEnemy[i].EnemyName + "isLock", false);
          // if (PlayerPrefs.HasKey(townNPC.locationEnemy[i].EnemyName + "isLock"))
          // {
          //   bool getstate = PlayerPrefsX.GetBool(townNPC.locationEnemy[i].EnemyName + "isLock");
          //   townNPC.npcManager.isLock = getstate;
          // }
          // else
          // {
          //  townNPC.npcManager.isLock = false;
          // }
          PlayerPrefs.SetInt(townNPC.locationEnemy[i].EnemyName+"isLock",0);
          if(PlayerPrefs.HasKey(townNPC.locationEnemy[i].EnemyName+"isLock")){
            int getKey = PlayerPrefs.GetInt(townNPC.locationEnemy[i].EnemyName+"isLock");
            if(getKey==0){
               townNPC.locationEnemy[i].isLock=false;
               
            }else if(getKey==1){
townNPC.locationEnemy[i].isLock=true;
            }
          }
          townNPC.UpdateTown();
          UpdateServer();
         
          Debug.Log("NPC Names"+npcName+"Show");
        }
        else
        {
          Debug.Log("No target NPC");
        }
        
      }
    }
    else
    {
      Debug.LogError("No Town");
    }

    
  }

  public void CheckItemEnough(string itemName, double amount)
  {
    int count = 0;
    var inventory = GameObject.FindWithTag("Inventory").GetComponent<InventorySystem>();
    if (inventory != null)
    {
      for (int i = 0; i < inventory.items.Count; i++)
      {
        if (inventory.items[i].item.itemName == itemName)
        {
          if (inventory.items[i].item.stackSize >= 3) 
          {
            count = inventory.items[i].item.stackSize;
            break;
          }
          //has Item
          count++;
          
        }
      }
      //
      if (count >= amount)
      {
        DialogueLua.SetVariable("ItemEnough",true);
      }
    }
  }

  public void CheckMoneyEnough(double money)
  {
    var player = PlayerData.localPlayer;

    if (player != null)
    {
      if (player.money >= money)
      {
        DialogueLua.SetVariable("MoneyEnough",true);
      }
      else
      {
        DialogueLua.SetVariable("MoneyEnough",false);
      }
    }
  }
  //
  public void OpenCraft()
  {
    var cr = GameObject.FindWithTag("CraftSystem").GetComponent<CraftManager>();
    if (cr != null)
    {
      cr.OpenCloseWindow(true);
    }
  }

  public void AddExp(float exp)
  {
    var player = FindObjectOfType<PlayerData>();
    var expBar = FindObjectOfType<UIExp>();
    if (player != null)
    {
     player.CmdAddExp(exp);
      Debug.Log("player exp is"+player.experience);
    expBar.slider.Value+=exp;
    }
  }

public void LeaveTown(){
  var town =FindObjectOfType<TownManager>();
  if(town!=null){
    //
    if(town.atDungeon==true){
        //that's mean it explore successful ,so add count to achievement after the required enough
        // SteamAchievement.EXPLOREDUNGEON+=1;
        Debug.Log("DUNGEON TO MAP");
        town.WorldMap();
    }else{
      Debug.Log("TOWN TO MAP");
      town.WorldMap();
    }
  }
}

  public void UnlockLoc(string loc)
  {
    var areas = FindObjectOfType<TownManager>();
    var mapArea =FindObjectOfType<TravelSystem.TravelSystem>();
    if(areas!=null){
     for(int i=0;i<areas.maps.Count;i++){
       if(areas.maps[i].locationName==loc){
          areas.maps[i].isLock = false;

          foreach(var g in mapArea.areas ){
          if(g.GetComponent<TravelArea>().areaName==loc){
            g.gameObject.SetActive(true);
          }
          }
            PlayerPrefs.SetInt(areas.maps[i].locationName + "_Lock", 1);
         Debug.Log("DIALOGUE===> SET UNLOCK LOC");
       }
     }

     
      // TownManager.instance.leaveObj.gameObject.SetActive(true);
    }
  }

  public void OpenNBPanel(){
    var nbPanel = FindObjectOfType<TownManager>();
    int check=0;
    if(nbPanel!=null){
      if(PlayerPrefs.HasKey("NBP0")){
        check =PlayerPrefs.GetInt("NBP0");
        if(check==0){
           nbPanel.newBeePanel.gameObject.SetActive(true);
        }else if(check==1){
        nbPanel.newBeePanel.gameObject.SetActive(false);
        }
      }else{
nbPanel.newBeePanel.gameObject.SetActive(true);
      }
    }
  }

//
  public void SetPlayerValue(string n,double count){

   
    var p = FindObjectOfType<PlayerData>();
    //  if(n=="STR"){
    //    p.Strength+=(int)count;
    //  }else if(n=="DEX"){
    //    p.Dex+=(int)count;
       
    //  }else if(n=="INTE"){
    //    p.Magic+=(int)count;
    //  }
     p.UpdateInfo();
  }

  public void FaceNpc(){
     var cam =FindObjectOfType<NPCManager>();
 cam.faceCamera.gameObject.SetActive(true);
    cam.GetComponentInChildren<CinemachineVirtualCamera>().Priority=999;
  }
  public void LeaveNpc(){
    var cam =FindObjectOfType<NPCManager>();

    cam.GetComponentInChildren<CinemachineVirtualCamera>().Priority=0;
  }
  public void CheckHasPack(){
    bool hasPack = DialogueLua.GetVariable("HasPack").asBool;

    if(DeckStorge.instance.AllDecks.Count>0){
      DialogueLua.SetVariable("HasPack",true);
    }else{
      DialogueLua.SetVariable("HasPack",false);
      CollectionBrower.instance.ShowCollectionForBrowsing();
    }
  }


  #region Dungeon Module

  public void Dungeon_BattleConfig()
  {

    if (BattleStartInfo.SelectDeck != null)
    {
      EnemyDeckSelection.instance.ShowScreen();
    }
    
  }

  public void Dungeon_GetItems(Double itemID,Double amount)
  {
   
    var player = PlayerData.localPlayer;
    Items getItem= ItemDatabase.instance.FindItem((int)itemID);
    if (player != null)
    {
      while(amount>0){
      InventorySystem.instance.AddItem(getItem);
      amount--;
      }
    }
  }

 

  //Dungeon data is tmp when end explore add to playerdata
  public void Dungeon_GetMoney(double money)
  {
    ConsoleManager.MONEY += (int) money;
  }
  
  public void Dungeon_GetDust(double dust)
    {
      ConsoleManager.DUST +=(int) dust;
    }
  public void Dungeon_GetExp(double exp)
  {
    ConsoleManager.EXP += (int)exp;
  }


  #endregion

  #region 
  public void NB_OpenInventory(){
      InventorySystem.instance.OpenCloseInventory(true);
  }

  public void NB_OpenCollection(){
    DeckBuilderScreen.instance.ShowScreenForCollectionBrower();
  }
  #endregion


  public void CheckFreeVersion(){
    var town = FindObjectOfType<TownManager>();
    if(town.isFree=true){
      DialogueLua.SetVariable("FREEVER",true);
    }else{
      DialogueLua.SetVariable("FREEVER",false);
    }
  }
}
