using UnityEngine;
using System.Collections;

public class SpellTrap : SpellEffect {

    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        CardAsset ca = new CardAsset
        {
            name = "尖刺陷阱"
        };


        CardLogic cl = new CardLogic(ca);
        

        int tablePos =Random.Range(0,TableVisual.instance.slots.children.Length);
        
        for (int j = 0; j < specialAmount; j++)
        {
            for (int i = 0; i < owner.table.creatureOnTable.Count; i++)
            {
                if (owner.table.creatureOnTable[i].card == null)
                {
                    owner.playerArea.tableVisual.AddCreatureAtInIndex(cl.card, cl.UniqueCardID, tablePos);
                }
            }
        }

    }

   
}