using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using DG.Tweening;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using Cinemachine;

/// <summary>
/// Manager Match3 Module
/// 
/// </summary>
public class UIManager : Singleton<UIManager>
{
   public static UIManager instance;
  
   public UIPanel panel;
   public Board board;
   public Camera gameCamera;
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
   
   public GameObject gameManager;

   public Button BackBtn;
   //Common Module
   public GlobalSetting setting;
   public Transform pos;

   void Start() 
   {
      instance=this;
      
   
   }

    public bool WinnerPanel()
   {
      if (winPanel != null)
      {
         winPanel.gameObject.SetActive(true);
         winPanel.transform.DOLocalMove(pos.position,1.0f);
      }

      return true;
   }

   public bool LosePanel()
   {
      return false;
   }

  
}
