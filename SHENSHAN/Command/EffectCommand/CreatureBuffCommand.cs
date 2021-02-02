using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureBuffCommand : Command
{
    public CreatureLogic creature;
    public Players p;
    public int amount;
    
        

    public CreatureBuffCommand(CreatureLogic creature, Players player, int amount)
    {
        this.creature = creature;
        this.p = player;
        this.amount = amount;
    }
    
        
    public override void StartCommandExecution()
    {
        GameObject target = IDHolder.GetComponentWithID(p.ID);
        //
        
    }
}
