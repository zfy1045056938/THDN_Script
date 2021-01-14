using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSplit : SpellEffect
{
 public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
 {
  Debug.Log("SPELL EFFECT SPLIT ACTIVE!!!!!!!!!!!!!!!!!!!!!!!!");
  new DealDamageCommand(target.ID,target.MaxHealth,target.MaxHealth-=target.MaxHealth,0).AddToQueue();
   
  GameObject obj =IDHolder.GetComponentWithID(target.ID);
  
  //
  CardAsset tk =CardCollection.instance.GetCardAssetByName(obj.GetComponent<OneCreatureManager>().cardAsset.tokenCardAsset);
  CardLogic cl = new CardLogic
  {
   owner = owner,
   card = tk,
  };
        
  if (owner.playerArea.tableVisual.creatureOnTable.Count <= 6)
  {
   if (cl != null)
   {
               
    while (specialAmount > 0)
    {
     int rndp = Random.Range(0, 6);
     int tablePos = owner.playerArea.tableVisual.TablePosForNewCreature(owner.playerArea.tableVisual.slots.children[rndp].transform.position.x);
//                        new PlayCreatureCommand(cl, owner, tablePos, cl.UniqueCardID).AddToQueue();
     owner.PlayACreatureFromHand(cl,tablePos);
     specialAmount--;

    }
   }
  }
  
 }
}
