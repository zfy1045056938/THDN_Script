using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureHeal : CreatureEffect
{
   

    public CreatureHeal(Players owner, CreatureLogic creature, int specialAmount,int round,SpellBuffType sbt,DamageElementalType det): base(owner, creature, specialAmount,round,sbt,det)
    {
    }

    public override void WhenACreatureIsPlayed()
    {
        Debug.Log("Creature Heal Effect Cause !!");
        owner.MaxHealth += specialAmount;
    }
}
