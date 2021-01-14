using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureGiveCard : CreatureEffect
{
    
    public CreatureGiveCard(Players owner, CreatureLogic creature, int specialAmount,int round,SpellBuffType sbt,DamageElementalType det): base(owner, creature, specialAmount,round,sbt,det)
    {

        OneCardManager oc = new OneCardManager
        {
            name = "力量药水"
        };
        
        CardLogic cl = new CardLogic
        {
            owner = owner,
            card = oc.cardAsset,
            
        };
        
        
        new DrawACardCommand(cl,owner,false,false).AddToQueue();
    }
}
