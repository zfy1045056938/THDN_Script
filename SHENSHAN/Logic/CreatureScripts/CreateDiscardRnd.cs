using UnityEngine;
using System.Collections;


//CardPlay->hasEffect->CardType->Active->CardEvent->ActiveCommand
public class CreateDiscardRnd : CreatureEffect
{
    public CreateDiscardRnd(Players owner, CreatureLogic creature, int specialAmount,int round,SpellBuffType sbt,DamageElementalType det): base(owner, creature, specialAmount,round,sbt,det)
    {}

    // BATTLECRY
    public override void WhenACreatureIsPlayed()
    {
        //
        Debug.Log("Discard Card");
        GlobalSetting.instance.SETLogs(string.Format("扒手效果触发,丢弃对方{0}张卡牌",specialAmount));
        int cardRnd = Random.Range(0,owner.otherPlayer.hand.CardInHand.Count-1);
       new DiscardACardCommand(owner.otherPlayer,cardRnd).AddToQueue();
       Command.CommandExecutionComplete();
    }
}
