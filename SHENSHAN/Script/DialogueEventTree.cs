using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class DialogueEventTree : MonoBehaviour
{
  public static int Money;
  public ItemDatabase item;
  public  PlayerData player;
  [HideInInspector]
  public InventorySystem inventory;

  [HideInInspector]
  public CardCollection card;
  public  NetworkManagerShenShan managerShenShan;

    void Start(){

        player=GetComponent<PlayerData>();
        item=GetComponent<ItemDatabase>();
        managerShenShan=GetComponent<NetworkManagerShenShan>();
   

    }

//
//   public  void GetMoney(int money){
//      PlayerData.localPlayer.GetMoney(money);
//    }

  public void GetItem(string item){
    inventory.AddItem(new Items(item));
  }

  public void GetCard(string cardName){

  }
}
