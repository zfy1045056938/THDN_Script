using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellGroupBuff : SpellEffect
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        base.ActiveEffect(specialAmount, target);
    }

    public override void ActiveEffectToTargetStat(int specialAmount = 0, ICharacter target = null,
        SpellBuffType type = SpellBuffType.None)
    {
        // Debug.Log("GROUP BUFF ACTIVE==============>ACTIVE");
        // CreatureLogic[] cl = TurnManager.instance.WhoseTurn.table.creatureOnTable.ToArray();
        // if (cl.Length > 0)
        // {
        //     foreach (CreatureLogic c in cl)
        //     {
              
        //         new DealBuffCommand(c.ID, specialAmount, type, round:0).AddToQueue();
        //     }
        // }
        // else
        // {
        //     Debug.Log("No Creature At table");
        // }
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
        Debug.Log("GROUP BUFF ACTIVE==============>ACTIVE");
        CreatureLogic[] cl = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();
        if (cl.Length > 0)
        {
            foreach (CreatureLogic c in cl)
            {
              
                new DealBuffCommand(c.ID, amount, type, roundTime).AddToQueue();
            }
        }
        else
        {
            Debug.Log("No Creature At table");
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
