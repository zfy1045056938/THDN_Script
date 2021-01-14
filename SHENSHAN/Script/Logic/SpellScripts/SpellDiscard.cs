using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDiscard : SpellEffect
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        Debug.Log("SPELL DISCARD ACTIVE!!!!!!!!!!!!!!!");
        while (specialAmount > 0)
        {
            if (TurnManager.instance.WhoseTurn.otherPlayer.hand.CardInHand.Count > 0)
            {
                int rnd = Random.Range(0, TurnManager.instance.WhoseTurn.otherPlayer.hand.CardInHand.Count);
                TurnManager.instance.WhoseTurn.otherPlayer.DiscardCardAtIndex(rnd);
                specialAmount--;
            }
            else
            {
                specialAmount--;
            }
        }
    }
}
