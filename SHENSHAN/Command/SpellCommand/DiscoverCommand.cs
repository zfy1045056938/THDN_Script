using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoverCommand : Command
{
    public int targetID;
    public int amount;
   
    public DiscoverType discoverType;

    public 
        DiscoverCommand(int targetID, int amount, DiscoverType discoverType)
    {
        this.targetID = targetID;
        this.amount = amount;
        this.discoverType = discoverType;
    }

    public override void StartCommandExecution()
    {
      TurnManager.instance.WhoseTurn.DiscoverSelectACard(amount,discoverType);
      new DelayCommand(0.5f).AddToQueue();
      CommandExecutionComplete();
    }

  
  
}
