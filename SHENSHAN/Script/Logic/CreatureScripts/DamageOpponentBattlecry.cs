using UnityEngine;
using System.Collections;


//CardPlay->hasEffect->CardType->Active->CardEvent->ActiveCommand
public class DamageOpponentBattlecry : CreatureEffect
{
    public DamageOpponentBattlecry(Players owner, CreatureLogic creature, int specialAmount,int round,SpellBuffType sbt,DamageElementalType det): base(owner, creature, specialAmount,round,sbt,det)
    {}

    // BATTLECRY
    
}
