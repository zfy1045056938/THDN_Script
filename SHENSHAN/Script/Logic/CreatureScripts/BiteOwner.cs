using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BiteOwner : CreatureEffect
{  
    public BiteOwner(Players owner, CreatureLogic creature, int specialAmount,int round,SpellBuffType sbt,DamageElementalType det): base(owner, creature, specialAmount,round,sbt,det)
    {}

    public override void RegisterEventEffect()
    {
        owner.EndTurnEvent += CauseEventEffect;
        //owner.otherPlayer.EndTurnEvent += CauseEventEffect;
        Debug.Log("Registered bite effect!!!!");
    }

    public override void UnRegisterEventEffect()
    {
        owner.EndTurnEvent -= CauseEventEffect;
    }

    public override void CauseEventEffect()
    {
        Debug.Log("InCauseEffect: owner: "+ owner + " specialAmount: "+ specialAmount);
//        new DealDamageCommand(new List<DamageCommandInfo>
//        {
//            new DamageCommandInfo()
//            {
//                amount = owner.playerID,
//                armorAfter =  creature.card.cardDef-specialAmount,
//                healthAfter = owner.MaxHealth-specialAmount,
//                
//            }
//        } ).AddToQueue();
//        //
        
        owner.MaxHealth -= specialAmount;
    }


}
