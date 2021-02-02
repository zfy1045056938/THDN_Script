using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveAMana : SpellEffect
{
    

    public override void ActiveEffect(int specialSpellAmount = 0, ICharacter iCharacterId = null)
    {
        base.ActiveEffect(specialSpellAmount, iCharacterId);

        TurnManager.instance.WhoseTurn.GetBonusMana(specialSpellAmount);
    }

   
}
