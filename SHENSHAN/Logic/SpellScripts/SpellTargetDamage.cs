using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTargetDamage : SpellEffect
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        Debug.Log("Spell Target Damage====> Active");
       target.MaxHealth -= specialAmount;
       new DealDamageCommand(target.ID,specialAmount,target.MaxHealth-specialAmount,0).AddToQueue();
    }

    public override void ActiveEffectToTargetStat(int specialAmount = 0, ICharacter target = null, SpellBuffType type = SpellBuffType.None)
    {
        base.ActiveEffectToTargetStat(specialAmount, target, type);
    }

    public override void ActiveEffectToTargetStat(int specialAmount = 0, ICharacter target = null, DamageElementalType type = DamageElementalType.None)
    {
        base.ActiveEffectToTargetStat(specialAmount, target, type);
    }

    public override void ActiveEffectToTargetStat(int specialAmount = 0, ICharacter target = null, ArtifactType type = ArtifactType.None)
    {
        base.ActiveEffectToTargetStat(specialAmount, target, type);
    }

    public override void ActiveRoundEffect(int amount = 0, ICharacter target = null, int roundTime = 0, DamageElementalType type = DamageElementalType.None)
    {
        base.ActiveRoundEffect(amount, target, roundTime, type);
    }

    public override void CauseEventEffect()
    {
        base.CauseEventEffect();
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void RegisterEventEffect()
    {
        base.RegisterEventEffect();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public override void UnRegisterEventEffect()
    {
        base.UnRegisterEventEffect();
    }
}
