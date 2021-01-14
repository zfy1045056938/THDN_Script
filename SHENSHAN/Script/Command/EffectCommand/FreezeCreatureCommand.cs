using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeCreatureCommand : Command
{

    public Players owner;
    public FreezeCreatureCommand(int targetId, int amount, bool froze)
    {
        targetID = targetId;
        this.amount = amount;
        this.froze = froze;
    }

    public int targetID;
    public int amount;
    public bool froze;


    public override void StartCommandExecution()
    {
        owner.playerArea.handVisual.FreezeCreature(targetID, amount, froze);
        
        CommandExecutionComplete();
    }
}
