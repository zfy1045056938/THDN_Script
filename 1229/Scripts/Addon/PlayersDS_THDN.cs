using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using PixelCrushers.DialogueSystem;


public partial class PlayerDS_THDN :NetworkBehaviour
{
    [SyncVar]
    public string dialogueData;
    
    #region Init

    public  void OnStartLocalPlayer_DialogueSystem()
    {
        DialogueManager.ResetDatabase(DatabaseResetOptions.KeepAllLoaded);
        PersistentDataManager.ApplySaveData(dialogueData);
        var player = Players.localPlayer;
        if (player.portrainIcon!=null&&player&& usePorIconForConvastions )
        {
            DialogueLua.SetActorField("Player",DialogueSystemFields.CurrentPortrait,player.portrainIcon.name);
        }

        StartCoroutine(UpdateQuestStateListenerAfterOneFrame());
    }

    IEnumerator UpdateQuestStateListenerAfterOneFrame()
    {
        yield return null;
        foreach (var qs in FindObjectsOfType<QuestStateListener>())
        {
            qs.UpdateIndicator();
        }
    }

    #endregion

    #region Update DS on Server

    private Coroutine m_updateDSDataCor = null;

    public void UpdateDialogueSystemData()
    {
        if (m_updateDSDataCor == null)
        {
            m_updateDSDataCor = StartCoroutine(UpdateDSDCoroutine());
        }
    }

    private IEnumerator UpdateDSDCoroutine()
    {
        yield return new  WaitForEndOfFrame() ;
        m_updateDSDataCor = null;
        UpdateDSDN();

    }

    private void UpdateDSDN()
    {
        dialogueData = PersistentDataManager.GetSaveData();
        //if (!isServer)
        //{
        //    CmdUpdateDSDOnServer(dialogueData);
        //}
    }

    [Command]
    private void CmdUpdateDSDOnServer(string data)
    {
        dialogueData = data;
    }
    
    

    #endregion
    private bool usePorIconForConvastions;
  

    
    



    #region Server Command Modify Player

    //[Command]
    //public void CmdWarp_DS(Vector3 destination)
    //{
    //    agent.Warp(destination);
    //}

    #endregion
}
