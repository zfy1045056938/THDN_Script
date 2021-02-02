using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDraw : SpellEffect
{
   public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
   {
      while (specialAmount > 0)
      {
         TurnManager.instance.WhoseTurn.DrawACard();
         specialAmount--;
      }
   }
}
