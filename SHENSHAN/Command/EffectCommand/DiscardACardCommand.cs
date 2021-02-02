using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardACardCommand : Command
{
    public Players player;
    public int index;

    public DiscardACardCommand(Players p, int index)
    {
        this.player = p;
        this.index = index;
    }

    public override void StartCommandExecution()
    {
        Debug.Log("Discard Card Command");
        int index=Random.Range(0,player.playerArea.handVisual.CardsInHand.Count);
        player.playerArea.handVisual.DiscardCardAtIndex(index);
           CommandExecutionComplete();
    }
    
    
}
