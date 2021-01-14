using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellArtifact : SpellEffect
{
   public ArtifactType types =ArtifactType.GiveCard;
   
   public override void ActiveEffectToTargetStat(int specialAmount = 0, ICharacter target = null,
      ArtifactType type = ArtifactType.None)
   {
      Debug.Log("Artifact Active");
      
      new ArtifactCommand(TurnManager.instance.WhoseTurn,type,specialAmount).AddToQueue();
   }

//   public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
//   {
//      Debug.Log("test artifact");
//      new ArtifactCommand(TurnManager.instance.WhoseTurn,types,specialAmount).AddToQueue();
//   }
}
