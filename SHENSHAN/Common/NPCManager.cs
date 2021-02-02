using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Mirror;
using Cinemachine;
//public class EnemyInfo{
//    public int enemyID;
//    public string enemyName;
//    public Dictionary<int,CardCollection>cardList   = new Dictionary<int, CardCollection>();
//    public List<ItemManager> item = new List<ItemManager>();
//    public int dropExp;45
//    public Sprite enemyAvatar;
//    
//}

public class NPCManager : Entity,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{

    public Image glowImage;
   

    public EnemyAsset asset;    
    private PlayerData player;
    private Merchant merchant;
    public GameObject model;
   
    public MerchantController controller;
    
    //NPC Asset
    private bool _isActive;
    public bool isLock{
        get{
            return _isActive;
        }
        set{
            _isActive=value;
        }
    }
  




    // Update is called once per frame
   void Start()
   {
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
    //    {
           
    //        if (hit.transform.CompareTag("Enemy") && hit.transform == transform)
    //        {
    //            Debug.Log("Click it");
    //            DialogueManager.StartConversation("Battle Time");
    //        }
    //    }
  
   }

   

    public void OnPointerEnter(PointerEventData eventData)
    {
        // glowImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // glowImage.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
         faceCamera.Priority=200;
        if(eventData.button== PointerEventData.InputButton.Left){
            player.target=this;
            Debug.Log("player select target ist"+player.target);
                if(converName!=null ){
                    Debug.Log("Start con"+converName.ToString());
                    DialogueManager.StartConversation(converName);
                }
        }else if(eventData.button==PointerEventData.InputButton.Right){

        }
    }
    private void OnMouseEnter() {
       outline.enabled=true;
   }

   private void OnMouseExit() {
       outline.enabled=false;
   }
     private void OnMouseDown() {

        //  faceCamera.Priority=200;
        if(Utils.IsClickUI())return;
         PlayerData.localPlayer.target=this;
            Debug.Log("player select target ist"+PlayerData.localPlayer.target);
                if(converName!=null ){
                    Debug.Log("Start con"+converName.ToString());
                    DialogueManager.StartConversation(converName);
                }else{
                    Debug.Log("Not Dialogue ");
                }
    }

    [Command]
    public void CmdOpenShop(){
        merchantController.LoadMerchant();
    }

    protected override string UpdateServer()
    {
        throw new NotImplementedException();
    }

    protected override void UpdaetClient()
    {
        throw new NotImplementedException();
    }
}
