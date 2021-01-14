using UnityEngine;
using System.Collections;


//CardPlay->hasEffect->CardType->Active->CardEvent->ActiveCommand
public class CreatureDiscover : CreatureEffect
{
   
    public CreatureDiscover(Players owner, CreatureLogic creature, int specialAmount,DiscoverType dt): base(owner, creature, specialAmount,dt)
    {}

    // BATTLECRY
    public override void WhenACreatureIsPlayed()
    {
     new DiscoverCommand(creature.ID,creature.card.specialCreatureAmount,creature.card.disType).AddToQueue();
     new DelayCommand(0.4f).AddToQueue();
    }
}
