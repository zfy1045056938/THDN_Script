using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using DG.Tweening;
public class PlayerTurnMaker:TurnMaker
{
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        // dispay a message that it is player`s turn
        Debug.Log("Player Turn Module");
        Sequence s = DOTween.Sequence();

        s.PrependInterval(0.4f); 
        p.DrawACard();
        //Change button 
      
        new ShowMessageCommand("你的回合!", 2.0f).AddToQueue();
        s.PrependInterval(0.3f); 
       
        s.OnComplete(() =>
        {
           
            Command.CommandExecutionComplete();
        });
       

    }
}