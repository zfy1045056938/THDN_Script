using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellFireBall : SpellEffect
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        if (target.CreatureDef > 0)
        {
            new DealDamageCommand(target.ID, specialAmount, target.MaxHealth - specialAmount,
                target.CreatureDef).AddToQueue();
        }
        else if (target.CreatureDef - specialAmount < 0)
        {
            new DealDamageCommand(target.ID, specialAmount, target.MaxHealth + (target.CreatureDef-specialAmount),
                target.CreatureDef - specialAmount).AddToQueue();
        }
        else
        {
            new DealDamageCommand(target.ID, specialAmount, target.MaxHealth ,
                target.CreatureDef - specialAmount).AddToQueue();
        }

    
        
        
    }

   
}
