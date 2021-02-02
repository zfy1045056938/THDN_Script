using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDiscover : SpellEffect
{
  

  
    private DiscoverType discoverType;
    
    
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        new DiscoverCommand(TurnManager.instance.WhoseTurn.ID,specialAmount,DiscoverType.Rnd).AddToQueue();
        
    }

   
}
