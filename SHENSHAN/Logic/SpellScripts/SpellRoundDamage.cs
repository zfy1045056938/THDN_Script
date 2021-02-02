using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellRoundDamage : SpellEffect
{
 
  public override void RegisterEventEffect()
  {
    owner.StartTurnEvent += CauseEventEffect;
  }

  public override void UnRegisterEventEffect()
  {
    owner.StartTurnEvent -= CauseEventEffect;
  }

  
  public override void ActiveRoundEffect(int amount , ICharacter target , int roundTime ,
    DamageElementalType type )
  {
   
   Debug.Log(roundTime);
      Debug.Log(amount);
      new DealDamageCommand(target.ID, amount, target.MaxHealth - amount, target.CreatureDef , roundTime, type)
        .AddToQueue();
      target.MaxHealth -= amount;
      roundTime--;
      Debug.Log("Remain Time "+roundTime);
    
  }


  
}
