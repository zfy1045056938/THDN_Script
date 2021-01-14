using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEventCommand : Command
{

public Players owner;
    public DungeonEventType det =DungeonEventType.None;
    public int amount;

    public DungeonEventCommand(Players owner,DungeonEventType det, int amount)
    {
        this.owner=owner;
        this.det = det;
        this.amount = amount;
    }

  

    public override void StartCommandExecution()
    {
      //Check Has Dungen
      if(BattleStartInfo.DungeonDifficult=="Hard"){
          //Check DE Type
          if(BattleStartInfo.DungeonEventType!=null){
             owner.ActiveDungeonEffect(owner,det,amount);
             Command.CommandExecutionComplete();
            
          }
      }
    }

  
   
}
