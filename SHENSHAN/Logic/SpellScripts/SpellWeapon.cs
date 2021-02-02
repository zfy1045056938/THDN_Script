using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellWeapon : SpellEffect
{
  public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
  {
    SoundManager.instance.PlaySound(GlobalSetting.instance.weaponClip);
    TurnManager.instance.WhoseTurn.CreatureAtk += specialAmount;
    // TurnManager.instance.WhoseTurn.atkDur += 2;
    TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.BUpdateV();
  }
}
