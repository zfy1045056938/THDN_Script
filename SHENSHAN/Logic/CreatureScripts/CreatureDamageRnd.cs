using UnityEngine;
using System.Collections;


//CardPlay->hasEffect->CardType->Active->CardEvent->ActiveCommand
public class CreatureDamageRnd : CreatureEffect
{
    public CreatureDamageRnd(Players owner, CreatureLogic creature, int specialAmount, int round, SpellBuffType sbt,
        DamageElementalType det) : base(owner, creature, specialAmount, round, sbt, det)
    {
        
    }

    public override void WhenACreatureIsPlayed()
    {
        Debug.Log("CREATURE SCRIPT DAMAGE RND======>DAMAGERND");
        CreatureLogic[] cl = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();
        int rnd = Random.Range(0, cl.Length);

        if (cl.Length > 0)
        {
            if (cl[rnd].CreatureDef > 0)
            {
                new DealDamageCommand(cl[rnd].ID, specialAmount, cl[rnd].MaxHealth,
                    cl[rnd].CreatureDef - specialAmount).AddToQueue();

            }
            else if (cl[rnd].CreatureDef < 0)
            {
                new DealDamageCommand(cl[rnd].ID, specialAmount,
                    cl[rnd].MaxHealth + (cl[rnd].CreatureDef - specialAmount),
                    cl[rnd].CreatureDef - specialAmount).AddToQueue();
            }
            else
            {
                new DealDamageCommand(cl[rnd].ID, specialAmount,
                    cl[rnd].MaxHealth + (cl[rnd].CreatureDef - specialAmount),
                    cl[rnd].CreatureDef - specialAmount).AddToQueue();
            }

            if (cl[rnd].CreatureDef > 0)
            {
                cl[rnd].CreatureDef -= specialAmount;
                if (cl[rnd].CreatureDef - specialAmount < 0)
                {
                    cl[rnd].MaxHealth+=cl[rnd].CreatureDef - specialAmount;
                }
            }
            else
            {
                cl[rnd].MaxHealth -= specialAmount;
            }
            
         GlobalSetting.instance.SETLogs(string.Format("先发效果,对{0}造成{1}点伤害",cl[rnd].card.name,specialAmount));
        }else{
         Debug.Log("DO NOTHING");
        }

    }
}
