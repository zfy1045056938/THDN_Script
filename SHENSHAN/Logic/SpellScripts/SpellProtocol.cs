using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProtocol : SpellEffect
{
  public override void ActiveRoundEffect(int amount = 0, ICharacter target = null, int roundTime = 0,
        DamageElementalType type = DamageElementalType.None)
    {
        foreach(var p in TurnManager.Players){
        

            new DealBuffCommand(p.ID,amount,type,roundTime).AddToQueue();
        }
    }

}
