using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellGiveCard : SpellEffect
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        int cards = specialAmount;
        while (cards > 0)
        {
            TurnManager.instance.WhoseTurn.DrawACard(fast:true);
            cards --;
        }
    }

  
}
