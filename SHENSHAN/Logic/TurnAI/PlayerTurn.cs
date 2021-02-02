using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using PixelCrushers.DialogueSystem;
public class PlayerTurn:TurnMaker{

    public override void OnTurnStart(){
        base.OnTurnStart();
        new ShowMessageCommand("你的回合",2.0f).AddToQueue();
     
        //发牌
        p.DrawACard(true);
    }
}