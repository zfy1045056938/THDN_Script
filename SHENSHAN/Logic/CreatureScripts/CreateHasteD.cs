using UnityEngine;
using System.Collections;


//CardPlay->hasEffect->CardType->Active->CardEvent->ActiveCommand
public class CreateHasteD : CreatureEffect
{
    public CreateHasteD(Players owner, CreatureLogic creature, int specialAmount,int round,SpellBuffType sbt,DamageElementalType det): base(owner, creature, specialAmount,round,sbt,det)
    {}

    // BATTLECRY
    public override void WhenACreatureIsPlayed()
    {
     creature.AttacksForThisTurn=2;
    }
}
