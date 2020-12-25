using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class Dialogue_THDN : MonoBehaviour
{
    public bool allowLuaOutsideConversations = true;

    public bool allowLuaToChangeData = true;
    public bool allowLuaOutsideConversationEnd = true;
    public bool allowLuaToChangeQuestStae = true;

    private void Awake()
    {
        Lua.RegisterFunction("UpdateServer",this,SymbolExtensions.GetMethodInfo(()=>UpdateServer()));
        Lua.RegisterFunction("GetPlayerNames",this,SymbolExtensions.GetMethodInfo(()=>GetPlayerNames()));
        Lua.RegisterFunction("OpenShop",this,SymbolExtensions.GetMethodInfo(()=>OpenShop()));
        Lua.RegisterFunction("GivePlayerMoney",this,SymbolExtensions.GetMethodInfo(()=>GivePlayMoney((double)0)));
        Lua.RegisterFunction("LoadBattle",this,SymbolExtensions.GetMethodInfo(()=>LoadBattle()));
    }

    public bool IsLuaDataChangePermitted()
    {
        return allowLuaOutsideConversations && (allowLuaOutsideConversations && DialogueManager.isConversationActive);
    }

    public bool IsLuaPermitted()
    {
        return IsLuaDataChangePermitted();
        
    }

    public void OnConversationEnd(Transform actor)
    {
        if (allowLuaOutsideConversationEnd) UpdateServer();
    }

    public void OnQuestStateChange(string questName)
    {
        if (allowLuaToChangeQuestStae) UpdateServer();
    }
    

    private string GetPlayerNames()
    {
        var player = Players.localPlayer;
        return player ? player.name : "Player";
    }

    void GivePlayMoney(double money)
    {
        var Player = Players.localPlayer;
        if (Player) Player.CmdAddMoney_DS((int) money);
    }
    private void OpenShop()
    {
        var shop = FindObjectOfType<UITradePanel>();
        // var inventory = FindObjectOfType<UIInventory>();
        var p = Players.localPlayer;
        if (shop != null && p!=null)
        {  
        }
    }
    
    public  void LoadBattle()
    {
        Debug.Log("Load Battle");
        var Players = FindObjectOfType<Players>();
        var Camera = GameObject.FindGameObjectWithTag("MatchCamera").GetComponent<Camera>();
        var MainCamera=GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (Players != null && Players.target!=null)
        {
            Debug.Log("Battle with"+Players.target.name.ToString());
         
            UIManager.instance.gameManager.gameObject.SetActive(true);
            
            
           MainCamera.gameObject.SetActive(false);
            //
            Camera.depth = 100;
           
            
            //
        }else{
            Debug.LogWarning("VALID OBJECT!!!!");
        }


    }

    private void UpdateServer()
    {
        var player = Players.localPlayer;
        if (player) player.UpdateDialogueSystemData();
    }

   
}
