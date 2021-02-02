using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellGroupHaste : SpellEffect
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        if (TurnManager.instance.WhoseTurn.table.creatureOnTable.Count > 0)
        {

            foreach (var c in TurnManager.instance.WhoseTurn.table.creatureOnTable)
            {
                c.AttacksForThisTurn = 1;
            }
    }
}
}
