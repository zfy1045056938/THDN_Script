using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSoulDamage : SpellEffect
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        CreatureLogic[] cl = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();

        int rnd = Random.Range(0, cl.Length);
        if (cl.Length > 0)
        {
           new DealDamageCommand(cl[rnd].ID,specialAmount,cl[rnd].MaxHealth-=specialAmount,cl[rnd].CreatureDef-=specialAmount).AddToQueue();   
        }
        //
        TurnManager.instance.WhoseTurn.MaxHealth += specialAmount;
    }
}
