using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTargetDamage : SpellEffect
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        Debug.Log("Spell Target Damage====> Active");
        int edamage = 0;
        if (TurnManager.instance.WhoseTurn.ExtraSpellDamage > 0)
        {
            Debug.Log("GOT ESD ");
            edamage += TurnManager.instance.WhoseTurn.ExtraSpellDamage;
            specialAmount += edamage;

        }
        //deal damage to target
        if (target.CreatureDef <= 0)
        {
            target.MaxHealth -= specialAmount;
        }else if (target.CreatureDef - specialAmount > 0)
        {
            target.CreatureDef -= specialAmount;
        }else if(target.CreatureDef-specialAmount > 0)
        {
            //counter overflow damage
            int odam = target.CreatureDef - specialAmount;

            target.MaxHealth += odam;

        }


          //damage effect 
       new DealDamageCommand(target.ID,specialAmount,target.MaxHealth-specialAmount,0).AddToQueue();
    }
}
