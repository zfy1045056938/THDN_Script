using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class CreatureHastes : CreatureEffect
{
 

    public CreatureHastes(Players owner, CreatureLogic creature, int specialAmount,int round,SpellBuffType sbt,DamageElementalType det): base(owner, creature, specialAmount,round,sbt,det)
    {
    }

    public override void WhenACreatureIsPlayed()
    {
        creature.AttacksForThisTurn += specialAmount;
    }
}
