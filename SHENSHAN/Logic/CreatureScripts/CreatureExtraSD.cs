using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureExtraSD : CreatureEffect
{
    public CreatureExtraSD(Players owner, CreatureLogic creature, int specialAmount, DiscoverType dt) : base(owner, creature, specialAmount, dt)
    {
    }

    public CreatureExtraSD(Players owner, CreatureLogic creature, int specialAmount, int round, SpellBuffType sbt, DamageElementalType det) : base(owner, creature, specialAmount, round, sbt, det)
    {
    }

    public CreatureExtraSD(Players owner, CreatureLogic creature, int specialAmount, int round) : base(owner, creature, specialAmount, round)
    {
    }

    public override void WhenACreatureIsPlayed()
    {
        TurnManager.instance.WhoseTurn.ExtraSpellDamage += specialAmount;
    }

    public override void WhenACreatureDies()
    {
        TurnManager.instance.WhoseTurn.ExtraSpellDamage -= specialAmount;
    }
}
