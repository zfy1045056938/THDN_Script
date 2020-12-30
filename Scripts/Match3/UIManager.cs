using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using DG.Tweening;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using Cinemachine;
using System;

/// <summary>
/// TODO
/// Manager Match3 Module
/// 
/// </summary>
public class UIManager : Singleton<UIManager>
{
   public UIPanel panel;
   public Board board;
    public GameManagers manager;
   public CinemachineVirtualCamera gameCamera;
   public GameObject BoardgameObject;
   public UIPanel winPanel;
   public GameObject losePanel;
   public PlayerStats player;
   public EnemyStats enemy;
   public CollectionsPool collectPool;
   public MatchTurnManager turnManager;
   public int collectionGoalBaseWidth = 125;
   public Button ConfigBtn;
   public MessageManager message;
    //
    public GameObject rageBar;
    public GameObject skillBar;
    public GameObject configBar;
   public GameObject gameManager;

   public Button BackBtn;
   //Common Module
   public GlobalSetting setting;
   public Transform pos;

   void Start() 
   {

        //ResetPanel TODO
        //
        ResetPanel();
   
   }

   
    public bool WinnerPanel()
   {
      if (winPanel != null)
      {
         winPanel.gameObject.SetActive(true);
         winPanel.transform.DOLocalMove(pos.position,1.0f);
      }

        //return dungeon
        GameManager.instance.ClearBoard();

      return true;
   }



    /// <summary>
    /// when lose return to dungeon und show notice how's win the explore
    /// </summary>
    /// <returns></returns>
   public bool LosePanel()
   {

        GameDebug.Log("LOSE GAME RETURN TO MAP");
      return false;
   }

   public void ResetPanel(){
      
   }

  
}
