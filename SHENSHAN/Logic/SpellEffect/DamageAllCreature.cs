using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;


[System.Serializable]
public struct DamageInfo{
    public int targetID;
    public TableVisual table;

    public DamageInfo(int targetID, TableVisual table)
    {
        this.targetID = targetID;
        this.table = table;
    }
}


/// <summary>
/// Damage all creature.
/// </summary>
public class DamageAllCreature : SpellEffect
{
    public DamageEffects damageEffect = DamageEffects.None;
    public List<DamageInfo> target = new List<DamageInfo>();

  
    /// <summary>
    /// Actives the effect.
    /// </summary>
    public override void ActiveEffect(int specialAmount =0, ICharacter target = null)
    {

        CreatureLogic[] cl = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();
        //
        foreach (CreatureLogic c in cl)
        {
            if (c.CreatureDef > 0)
            {
                new DealDamageCommand(c.ID,specialAmount,c.MaxHealth+(c.CreatureDef-specialAmount),c.CreatureDef).AddToQueue();
                int cf = c.CreatureDef - specialAmount;
                if (cf < 0)
                {
                    c.CreatureDef = 0;
                }
                c.MaxHealth -= cf;
            }else
            {
                new DealDamageCommand(c.ID,specialAmount,c.MaxHealth-specialAmount,c.CreatureDef).AddToQueue();
                c.MaxHealth-=specialAmount;
            }
        }
    }


    
}