using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellGroupHeal : SpellEffect
{
  public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
  {
   
   SoundManager.instance.PlaySound(GlobalSetting.instance.healClip);
    CreatureLogic[] cs = TurnManager.instance.WhoseTurn.table.creatureOnTable.ToArray();

    
    foreach (var vs in cs)
    {

      vs.MaxHealth += specialAmount;
    }
  }
}
