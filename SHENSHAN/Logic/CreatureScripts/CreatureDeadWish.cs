using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDeadWish : CreatureEffect
{
   

    public CreatureDeadWish(Players owner, CreatureLogic creature, int specialAmount,int round,SpellBuffType sbt,DamageElementalType det): base(owner, creature, specialAmount,round,sbt,det)
    {
    }


    public override void WhenACreatureDies()
    {
        Debug.Log("CREATURESCRIT DEADWISH:TOKEN\n");
       
        CardAsset tk =CardCollection.instance.GetCardAssetByName(creature.card.tokenCardAsset);
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
//                    int rndp = Random.Range(1, 6);
                    int tablePos = owner.playerArea.tableVisual.TablePosForNewCreature(owner.playerArea.tableVisual.slots.children[0].transform.position.x);
//                        new PlayCreatureCommand(cl, owner, tablePos, cl.UniqueCardID).AddToQueue();
    Debug.Log("Tablepos ist"+tablePos);
                    owner.PlayACreatureFromHand(cl,tablePos);
                    specialAmount--;

                }
            }
        }
    }
}
