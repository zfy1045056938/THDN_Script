using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSoulView : SpellEffect
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.soulView = true;
    }
}
