using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

//public struct DamageCommandInfo
//{
//    public int targetID;
//    public int amount;
//    public int healthAfter;
//    public int armorAfter;
//
//   
//    
//  
//
//}

public class DealDamageCommand : Command
{
    public DealDamageCommand(int targetId, int amount, int healthAfter, int armorAfter, int roundTime, DamageElementalType det)
    {
        targetID = targetId;
        this.amount = amount;
        this.healthAfter = healthAfter;
        this.armorAfter = armorAfter;
        this.roundTime = roundTime;
        this.det = det;
    }

//    private List<DamageCommandInfo> targets;
    public DealDamageCommand(int targetId, int amount, int healthAfter, int armorAfter)
    {
        targetID = targetId;
        this.amount = amount;
        this.healthAfter = healthAfter;
        this.armorAfter = armorAfter;
    }

    public int targetID;
    public int amount;
    public int healthAfter;
    public int armorAfter;
    public int roundTime;
    public DamageElementalType det;

//    public DealDamageCommand(List<DamageCommandInfo> target)
//    {
//        this.targets = targets;
//    }
    
    
    public override void StartCommandExecution()
    {
        Debug.Log("Deal Message Command ==>Active");

        GameObject target=IDHolder.GetComponentWithID(targetID);
        
//        CreatureLogic cl = new CreatureLogic(TurnManager.instance.WhoseTurn,target.GetComponent<OneCreatureManager>().cardAsset);

      if(roundTime==0){
          if (targetID == GlobalSetting.instance.lowPlayer.playerID ||
              targetID == GlobalSetting.instance.topPlayer.playerID)
          {

              target.GetComponent<PlayerPortraitVisual>().TakeDamage(amount, healthAfter,armorAfter);
          }
          else
          {
              //creature
              Debug.Log("Creature damage effect");
              CreatureLogic cl =CreatureLogic.creatureCreatedThisGame[targetID];

              target.GetComponent<OneCreatureManager>().TakeDamage(amount, healthAfter,armorAfter);
          }
      }else
      {
          Debug.Log("Check damage elements  ");
          CreatureLogic cl = new CreatureLogic(TurnManager.instance.WhoseTurn,target.GetComponent<OneCreatureManager>().cardAsset);

          //round time damage
          switch (det)
          {
              case DamageElementalType.Posion:
                  target.GetComponent<OneCreatureManager>().ElementalBuff(cl,amount, roundTime,det);
                  break;
              case DamageElementalType.Bloody:
                  target.GetComponent<OneCreatureManager>().ElementalBuff(cl,amount,roundTime,det);
                  break;
              case DamageElementalType.Fire:
              target.GetComponent<OneCreatureManager>().ElementalBuff(cl,amount,roundTime,det);
                  break;
              case DamageElementalType.Ice:
              target.GetComponent<OneCreatureManager>().ElementalBuff(cl,amount,roundTime,det);
                  break;
              case DamageElementalType.Electronic:
              target.GetComponent<OneCreatureManager>().ElementalBuff(cl,amount,roundTime,det);
                  break;
              case DamageElementalType.Freeze:
              target.GetComponent<OneCreatureManager>().ElementalBuff(cl,amount,roundTime,det);
                  break;
          }
      }


      Sequence s = DOTween.Sequence();
        s.PrependInterval(0.4f);
        s.OnComplete(() =>
        {
            Command.CommandExecutionComplete();
        });

    }
}
