using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHeal : SpellEffect
{
    public override void ActiveEffectToTargetStat(int specialAmount = 0, ICharacter target = null,
        SpellBuffType type = SpellBuffType.None)
    {
        Debug.Log("SPELL  HEAL ACTIVE!!!!!!!!!!!!!!!!!!!!!!");
        // CreatureLogic []cl = TurnManager.instance.WhoseTurn.table.creatureOnTable.ToArray();

        // foreach (var c in cl)
        // {
        //     new DealBuffCommand(c.ID,specialAmount,SpellBuffType.Health,0).AddToQueue();

        //     target.MaxHealth += specialAmount;
        //     owner.MaxHealth += specialAmount;
        // }
        SoundManager.instance.PlaySound(GlobalSetting.instance.healClip);
        TurnManager.instance.WhoseTurn.MaxHealth+=specialAmount;
    }
}
