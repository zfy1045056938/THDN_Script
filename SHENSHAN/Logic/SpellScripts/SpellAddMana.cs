using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAddMana : SpellEffect
{
   public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
   {
     TurnManager.instance.WhoseTurn.GetBonusMana(specialAmount);
           
   }

}
