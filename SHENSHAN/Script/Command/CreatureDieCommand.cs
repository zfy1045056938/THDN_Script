using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class CreatureDieCommand : Command 
{
    private Players p;
    private int DeadCreatureID;

    public CreatureDieCommand(int CreatureID, Players p)
    {
        this.p = p;
        this.DeadCreatureID = CreatureID;
    }

    public override void StartCommandExecution()
    {
     if(CreatureLogic.creatureCreatedThisGame[DeadCreatureID].Machine!=true){   
        p.playerArea.tableVisual.RemoveCreatureWithID(DeadCreatureID);
        //Add TO DP
        DialogueManager.SendUpdateTracker();
     Debug.Log("Die is"+DeadCreatureID);
     }else{
         Debug.Log("STAY WITH SLEEP");
     }
    }
}
