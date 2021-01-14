
using UnityEngine;
using System.Collections;


//CardPlay->hasEffect->CardType->Active->CardEvent->ActiveCommand
public class CreatureBuff : CreatureEffect
{
    
    public CreatureBuff(Players owner,CreatureLogic cl, int amount,int round,SpellBuffType type,DamageElementalType det):base(owner,cl,amount,round,type,det){}
    public override void RegisterEventEffect()
    {
        owner.StartTurnEvent += CauseEventEffect;
    }

    public override void UnRegisterEventEffect()
    {
        owner.StartTurnEvent += CauseEventEffect;
    }

    public override void WhenACreatureIsPlayed()
    {
        Debug.Log("CREATURE BUFF ==============> BUFF ACTIVE");

      //get oppnent creature at table
//      CreatureLogic[] cls = TurnManager.instance.WhoseTurn.table.creatureOnTable.ToArray();
      
//      foreach (CreatureLogic cs in cls)
//      {
//          int am = cl.card.specialCreatureAmount;
//          if (cs.card.hasBuff == true )
//          {
//              if(cs.card.spellBuffType!=SpellBuffType.None){
//              SpellBuffType type = cs.card.spellBuffType;
//              switch (type)
//              {
//                  case SpellBuffType.None:
//                      break;
//                  case SpellBuffType.Armor:
//                      new DealBuffCommand(cs.UniqueCreatureId, specialAmount, SpellBuffType.Armor, cs.card.RoundTime)
//                          .AddToQueue();
//                      cs.CreatureDef += specialAmount;
//                      break;
//                  case SpellBuffType.Health:
//                      new DealBuffCommand(cs.ID, specialAmount, SpellBuffType.Health, cs.card.RoundTime).AddToQueue();
//                      cs.MaxHealth += specialAmount;
//                      break;
//
//                  case SpellBuffType.Atk:
//                      new DealBuffCommand(cs.UniqueCreatureId, specialAmount, SpellBuffType.Atk, cs.card.RoundTime)
//                          .AddToQueue();
//                      cs.CreatureAtk += specialAmount;
//
//                      Debug.Log(cs.card.name + "---atk---" + cs.CreatureAtk);
//
//                      break;
//                  case SpellBuffType.DEX:
//                      new DealBuffCommand(cs.ID, specialAmount, SpellBuffType.DEX, cs.card.RoundTime).AddToQueue();
//                      break;
//                  case SpellBuffType.INT:
//                      new DealBuffCommand(cs.ID, specialAmount, SpellBuffType.INT, cs.card.RoundTime).AddToQueue();
//                      break;
//                  case SpellBuffType.STR:
//                      new DealBuffCommand(cs.ID, specialAmount, SpellBuffType.STR, cs.card.RoundTime).AddToQueue();
//                      break;
//                  case SpellBuffType.Taunt:
//                      new DealBuffCommand(cs.ID, specialAmount, SpellBuffType.Taunt, cs.card.RoundTime).AddToQueue();
//                      cs.taunt = true;
//                      break;
//
//              }
//              }else if (cs.card.damageEType != DamageElementalType.None)
//              {
//                  DamageElementalType t = cs.card.damageEType;
//                  switch (t)
//                  {
//                      case DamageElementalType.Bloody:
//                          new DealBuffCommand(cs.ID,specialAmount,DamageElementalType.Bloody,cs.card.RoundTime).AddToQueue();
//                          break;
//                  }
//              }
//          }

        new DealBuffCommand(creature.ID,specialAmount,sbt,det,round).AddToQueue();
        //
         if(det != DamageElementalType.None){
                GlobalSetting.instance.SETLogs(string.Format("对{0}发动{1}效果",creature.card.name,det.ToString()));
            }else if(sbt != SpellBuffType.None){
                GlobalSetting.instance.SETLogs(string.Format("对{0}发动{1}效果",creature.card.name,sbt.ToString()));
            }
    }
      
  

}