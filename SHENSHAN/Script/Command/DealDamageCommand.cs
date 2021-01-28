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

            // 21_1_26 elemental damage will reduce by resistance
            //  bd /round ->  bd-target.fr
            //  freeze/ round -> freeze perc - ir%
            // posion! / round++ -> posion! - pr!
            // er: round -> ed- er
            //Check Total amount has influence by players effect(resistance || extra Spell Damage)
            var effectAmount = CheckPlayerStats(amount);

            
            amount = effectAmount;

          //round time damage
          switch (det)
          {
              case DamageElementalType.Posion:
                    // amount!
                   amount=  CheckResistance(amount, det);
                  target.GetComponent<OneCreatureManager>().ElementalBuff(cl,amount, roundTime,det);
                  break;
              case DamageElementalType.Bloody:
                    //amount per round
                  target.GetComponent<OneCreatureManager>().ElementalBuff(cl,amount,roundTime,det);
                  break;
              case DamageElementalType.Fire:
                    // amount per round
                    amount = CheckResistance(amount, det);
                    target.GetComponent<OneCreatureManager>().ElementalBuff(cl,amount,roundTime,det);
                  break;
                    
              case DamageElementalType.Ice:
                    amount = CheckResistance(amount, det);
                    //amount freeze the creature can't move not for player / round fixed
                    target.GetComponent<OneCreatureManager>().ElementalBuff(cl,amount,roundTime,det);
                  break;
              case DamageElementalType.Electronic:
                    amount = CheckResistance(amount, det);
                    // amount  reduce atk / round fixed
                    target.GetComponent<OneCreatureManager>().ElementalBuff(cl,amount,roundTime,det);
                  break;
              case DamageElementalType.Freeze:
                    amount = CheckResistance(amount, det);
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

    /// <summary>
    ///  check lastest amounts
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public int CheckPlayerStats(int amount)
    {
        if (TurnManager.instance.WhoseTurn.hasESD)
        {
            //Has esd check effect
            Debug.Log("Update Amount " + amount);
            amount += TurnManager.instance.WhoseTurn.ExtraSpellDamage;
            return amount;
        }


        return amount;
    }

    public int CheckResistance(int amount, DamageElementalType det)
    {
        switch (det)
        {
            case DamageElementalType.Fire:
                if(TurnManager.instance.WhoseTurn.otherPlayer.FR >0)
                {
                    //Check
                     amount -= TurnManager.instance.WhoseTurn.otherPlayer.FR;
                    if (amount < 0)
                    {
                        amount = 0;
                    }
                    return amount;

                }
                else
                {
                    return amount;
                }

                break;

            case DamageElementalType.Freeze:
                if (TurnManager.instance.WhoseTurn.otherPlayer.IR > 0)
                {
                    //Check
                    amount -= TurnManager.instance.WhoseTurn.otherPlayer.IR;
                    if (amount < 0)
                    {
                        amount = 0;
                    }
                    return amount;

                }
                else
                {
                    return amount;
                }
                break;

            case DamageElementalType.Ice:
                if (TurnManager.instance.WhoseTurn.otherPlayer.IR > 0)
                {
                    //Check
                    amount -= TurnManager.instance.WhoseTurn.otherPlayer.IR;
                    if (amount < 0)
                    {
                        amount = 0;
                    }
                    return amount;

                }
                else
                {
                    return amount;
                }
                break;

            case DamageElementalType.Posion:
                if (TurnManager.instance.WhoseTurn.otherPlayer.PR > 0)
                {
                    //Check
                    amount -= TurnManager.instance.WhoseTurn.otherPlayer.PR;
                    if (amount < 0)
                    {
                        amount = 0;
                    }
                    return amount;

                }
                else
                {
                    return amount;
                }
                break;

            case DamageElementalType.Electronic:
                if (TurnManager.instance.WhoseTurn.otherPlayer.ER > 0)
                {
                    //Check
                    amount -= TurnManager.instance.WhoseTurn.otherPlayer.ER;
                    if (amount < 0)
                    {
                        amount = 0;
                    }
                    return amount;

                }
                else
                {
                    return amount;
                }
                break;
        }

        return amount;
    }


}
