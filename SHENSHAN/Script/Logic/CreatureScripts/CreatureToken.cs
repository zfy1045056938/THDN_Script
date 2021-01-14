using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//CardPlay->hasEffect->CardType->Active->CardEvent->ActiveCommand
public class CreatureToken : CreatureEffect
{
    public CreatureToken(Players owner, CreatureLogic creature, int specialAmount,int round,SpellBuffType sbt,DamageElementalType det): base(owner, creature, specialAmount,round,sbt,det)
    {}

    public override void WhenACreatureIsPlayed()
    {
        Debug.Log("CREATURESCRIT:TOKEN\n");
       
       CardAsset tk =CardCollection.instance.GetCardAssetByName(creature.card.tokenCardAsset);
        CardLogic cl = new CardLogic
        {
            owner = owner,
            card = tk,
        };
        
        if(TurnManager.instance.WhoseTurn.table.creatureOnTable.Count <=6){
            if (cl != null)
            {
               
                while (specialAmount > 0)
                {
                    int rndp = Random.Range(0, 6);
                        int tablePos = owner.playerArea.tableVisual.TablePosForNewCreature(owner.playerArea.tableVisual.slots.children[0].transform.position.x);
//                        new PlayCreatureCommand(cl, owner, tablePos, cl.UniqueCardID).AddToQueue();
                    owner.PlayACreatureFromHand(cl,tablePos);
                    specialAmount--;

                }
                     GlobalSetting.instance.SETLogs(string.Format("同党效果:{0}名{1}置入场中",specialAmount,tk.name));
   
            }
        }else{
                Debug.Log("table full");
        }
        
    }
}
