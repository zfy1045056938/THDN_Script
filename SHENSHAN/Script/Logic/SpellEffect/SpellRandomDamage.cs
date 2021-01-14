using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellRandomDamage : SpellEffect
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {

        int rndCID =Random.Range( 0,TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.Count);
        bool hasDef = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable[rndCID].HasArmor;

        
        int breakArmorHealAfter =
            TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable[rndCID].CreatureDef - specialAmount > 0
                ? TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable[rndCID].MaxHealth
                : TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable[rndCID].MaxHealth +
                  (TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable[rndCID].CreatureDef -
                    specialAmount);
        if (rndCID != -1)
        {
            if (hasDef)
            {
                new DealDamageCommand(TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable[rndCID].ID,
                    specialAmount,
                    breakArmorHealAfter,
                    TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable[rndCID].CreatureDef-=specialAmount).AddToQueue();
             
            }else
            {
                new DealDamageCommand(TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable[rndCID].ID,
                    specialAmount,
                    TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable[rndCID].MaxHealth - specialAmount,
                    0).AddToQueue();
                TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable[rndCID].MaxHealth -= specialAmount;
            }
        }
        new DelayCommand(0.4f).AddToQueue();
    }

}
