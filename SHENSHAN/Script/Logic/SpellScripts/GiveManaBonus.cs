using UnityEngine;
using System.Collections;

public class GiveManaBonus: SpellEffect 
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        TurnManager.instance.WhoseTurn.GetBonusMana(specialAmount);
    }

   
}
