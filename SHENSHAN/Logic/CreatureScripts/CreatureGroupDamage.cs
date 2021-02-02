using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureGroupDamage : CreatureEffect
{
    public CreatureGroupDamage(Players owner, CreatureLogic creature, int specialAmount, int round, SpellBuffType sbt, DamageElementalType det) : base(owner, creature, specialAmount, round, sbt, det)
    {
    }

    public CreatureGroupDamage(Players owner, CreatureLogic creature, int specialAmount, int round) : base(owner, creature, specialAmount, round)
    {
    }

    public override void WhenACreatureIsPlayed()
    {
        var cl = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();

        if (cl.Length > 0)
        {
            foreach (var c in cl)
            {
                 if (cl.Length > 0)
        {
            if (c.CreatureDef > 0)
            {
                new DealDamageCommand(c.ID, specialAmount, c.MaxHealth,
                    c.CreatureDef - specialAmount).AddToQueue();

            }
            else if (c.CreatureDef < 0)
            {
                new DealDamageCommand(c.ID, specialAmount,
                    c.MaxHealth + (c.CreatureDef - specialAmount),
                    c.CreatureDef - specialAmount).AddToQueue();
            }
            else
            {
                new DealDamageCommand(c.ID, specialAmount,
                    c.MaxHealth + (c.CreatureDef - specialAmount),
                    c.CreatureDef - specialAmount).AddToQueue();
            }

            if (c.CreatureDef > 0)
            {
                c.CreatureDef -= specialAmount;
                if (c.CreatureDef - specialAmount < 0)
                {
                    c.MaxHealth+=c.CreatureDef - specialAmount;
                }
            }
            else
            {
                c.MaxHealth -= specialAmount;
            }
        }else{
         Debug.Log("DO NOTHING");
        }
            }
        }
   
         GlobalSetting.instance.SETLogs(string.Format("党同伐异:对方场面随从受到{0}点伤害",specialAmount));
    }
}
