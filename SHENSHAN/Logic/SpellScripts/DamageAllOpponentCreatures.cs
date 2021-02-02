using UnityEngine;
using System.Collections;

public class DamageAllOpponentCreatures : SpellEffect {

    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        CreatureLogic [] logic = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();
        foreach(CreatureLogic c in logic){
            new DealDamageCommand(c.ID,specialAmount,c.MaxHealth,c.MaxHealth-specialAmount).AddToQueue();
            c.MaxHealth-= specialAmount;
        }
    }

 
}
