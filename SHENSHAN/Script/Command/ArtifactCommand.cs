using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArtifactType {
  None,
  Atk,
  Hp,
  Mp,
  GiveCard,
  Token,
  Armor,
  Mana,
  Posion,
  
}

public class ArtifactCommand : Command
{
    public ArtifactCommand(Players p, ArtifactType type, int amount)
    {
        this.p = p;
        this.type = type;
        this.amount = amount;
        
    }

    public Players p;
    public ArtifactType type;
    public int amount;
    public GameObject obj;
    
    public override void StartCommandExecution()
    {
        p.PlayArtifact(type,amount);
        CommandExecutionComplete();
    }
}
