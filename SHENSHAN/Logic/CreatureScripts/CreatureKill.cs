using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureKill : CreatureEffect
{

    
    public CreatureKill(Players owner, CreatureLogic creature, int specialAmount,int round,SpellBuffType sbt,DamageElementalType det): base(owner, creature, specialAmount,round,sbt,det)
    {
    }

    

    public override void WhenACreatureIsPlayed()
    {

        if (TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.Count > 0)
        {
            int rndCreature = Random.Range(0, TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.Count);
            new DealDamageCommand(owner.otherPlayer.table.creatureOnTable[rndCreature].ID,
                owner.otherPlayer.table.creatureOnTable[rndCreature].MaxHealth,
                owner.otherPlayer.table.creatureOnTable[rndCreature].MaxHealth -
                owner.otherPlayer.table.creatureOnTable[rndCreature].MaxHealth, 0).AddToQueue();
            
            owner.otherPlayer.table.creatureOnTable[rndCreature].MaxHealth -=
                owner.otherPlayer.table.creatureOnTable[rndCreature].MaxHealth;

                 GlobalSetting.instance.SETLogs(string.Format("对{0}发动猎杀效果,{1}死亡",owner.otherPlayer.table.creatureOnTable[rndCreature].card.name,
                 owner.otherPlayer.table.creatureOnTable[rndCreature].card.name));
        }
    }
}
