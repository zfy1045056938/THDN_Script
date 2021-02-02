using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAtkBuff : CreatureEffect
{
  
   

    public CreatureAtkBuff(Players owner, CreatureLogic creature, int specialAmount, int round, SpellBuffType sbt, DamageElementalType det) : base(owner, creature, specialAmount, round, sbt, det)
    {
    }


    public override void WhenCreatureAtking()
    {
        Debug.Log("CREATURE ATK BUFF ===================> ACTIVE");
        new DealBuffCommand(creature.ID,specialAmount,sbt,det,round).AddToQueue();
          
    }
}
