using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSacrifire : SpellEffect
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {

        specialAmount = target.CreatureAtk;
        Debug.Log("SPELL EFFECT SACRIFIRE ACTIVE!!!!!!!!!");
        new DealDamageCommand(target.ID,target.CreatureAtk,target.MaxHealth-=target.CreatureAtk,0).AddToQueue();

        var oppCl = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();

        if (oppCl.Length > 0)
        {
            foreach (CreatureLogic cl in oppCl)
            {
                if (cl != null)
                {
                    if (cl.CreatureDef > 0)
                    {
                        new DealDamageCommand(cl.ID, specialAmount, cl.MaxHealth,
                            cl.CreatureDef -= specialAmount).AddToQueue();
                    }else if (cl.CreatureDef - specialAmount < 0)
                    {
                        new DealDamageCommand(cl.ID, specialAmount, cl.MaxHealth+( cl.CreatureDef -= specialAmount),
                            cl.CreatureDef -= specialAmount).AddToQueue();
                    }
                    else
                    {
                        new DealDamageCommand(cl.ID, specialAmount, cl.MaxHealth+( cl.CreatureDef -= specialAmount),
                            cl.CreatureDef -= specialAmount).AddToQueue();
                    }
                }
            }
        }
    }
}
