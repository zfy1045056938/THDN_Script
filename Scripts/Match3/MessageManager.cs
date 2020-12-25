using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using DG.Tweening;


//Manager Goal,Quest,Reward
public class MessageManager : MonoBehaviour
{
    public static MessageManager instance;
    public UIPanel panel;
    private Players players;
    
    public Button startBtn;
    
    public GameObject rewardPanel;
    public GameObject goalPanel;
    public GameObject QuestPanel;
    
    public Transform showPos;
    void Start (){
        instance=this;
        players=Players.localPlayer;

    }



    void ShowWinPanel(){

    }

    void LosePanel(){

    }

   public void ShowMessagePanel(){
        if(players !=null && players.target){
            
            // panel.Open();
            panel.transform.DOLocalMove(showPos.transform.position,0.5f);
        }
    }
}
