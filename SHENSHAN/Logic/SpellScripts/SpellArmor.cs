using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellArmor : SpellEffect
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        Debug.Log("Armor Card Effect");
        TurnManager.instance.WhoseTurn.CreatureDef += specialAmount;
        TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.defText.text = TurnManager.instance.WhoseTurn.CreatureDef.ToString();
        //
        // new DealDamageCommand(TurnManager.instance.WhoseTurn.playerID,0,specialAmount,0).AddToQueue();
    }

    public override void ActiveEffectToTargetStat(int specialAmount = 0, ICharacter target = null, SpellBuffType type = SpellBuffType.None)
    {
        base.ActiveEffectToTargetStat(specialAmount, target, type);
    }

    public override void ActiveEffectToTargetStat(int specialAmount = 0, ICharacter target = null,
        DamageElementalType type = DamageElementalType.None)
    {
        base.ActiveEffectToTargetStat(specialAmount, target, type);
    }

    public override void ActiveEffectToTargetStat(int specialAmount = 0, ICharacter target = null, ArtifactType type = ArtifactType.None)
    {
        base.ActiveEffectToTargetStat(specialAmount, target, type);
    }

    public override void ActiveRoundEffect(int amount = 0, ICharacter target = null, int roundTime = 0,
        DamageElementalType type = DamageElementalType.None)
    {
        base.ActiveRoundEffect(amount, target, roundTime, type);
    }

    public override void CauseEventEffect()
    {
        base.CauseEventEffect();
    }

    public override void RegisterEventEffect()
    {
        base.RegisterEventEffect();
    }

    public override void UnRegisterEventEffect()
    {
        base.UnRegisterEventEffect();
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
