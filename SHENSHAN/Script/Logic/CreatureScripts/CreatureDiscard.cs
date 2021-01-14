using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDiscard : CreatureEffect
{
 

    public CreatureDiscard(Players owner, CreatureLogic creature, int specialAmount,int round,SpellBuffType sbt,DamageElementalType det): base(owner, creature, specialAmount,round,sbt,det)
    {
    }

    public override void WhenACreatureIsPlayed()

    {
        Debug.Log("CREATURECOMMAND===========>DISCARD");
        int rnd = Random.Range(0, TurnManager.instance.WhoseTurn.otherPlayer.playerArea.handVisual.CardsInHand.Count);
       
       if(TurnManager.instance.WhoseTurn.otherPlayer.hand.CardInHand.Count>0){
        while(specialAmount>0){ 
            owner.otherPlayer.DiscardCardAtIndex(rnd);
            specialAmount--;
            
        }
       }else{
           new DelayCommand(0.4f).AddToQueue();
       }

       GlobalSetting.instance.SETLogs(string.Format("扒手效果触发,丢弃对方{0}张卡牌",specialAmount));
    }
}
