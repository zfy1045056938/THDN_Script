using UnityEngine;
using System.Collections;

public class SpellBuff : SpellEffect
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        base.ActiveEffect(specialAmount, target);
    }


    //only For player
    public override void ActiveEffectToTargetStat(int specialAmount = 0, ICharacter target = null, SpellBuffType type = SpellBuffType.None)
    {
        
        new DealBuffCommand(TurnManager.instance.WhoseTurn.ID,amount,type,0).AddToQueue();
        
    }

    public override void ActiveEffectToTargetStat(int specialAmount = 0, ICharacter target = null, DamageElementalType type = DamageElementalType.None)
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
       
        if(TurnManager.instance.WhoseTurn.table.creatureOnTable.Count>0 || TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.Count>0){
            GameObject obj = IDHolder.GetComponentWithID(target.ID);
             Debug.Log("Effect to target ist "+obj.name);
        new DealBuffCommand(target.ID,amount,type,roundTime).AddToQueue();
        }
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