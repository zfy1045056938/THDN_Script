using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDamageRndEndTurn : CreatureEffect
{
    public CreatureDamageRndEndTurn(Players owner, CreatureLogic creature, int specialAmount, int round, SpellBuffType sbt, DamageElementalType det) : base(owner, creature, specialAmount, round, sbt, det)
    {
    }

    public CreatureDamageRndEndTurn(Players owner, CreatureLogic creature, int specialAmount, int round) : base(owner, creature, specialAmount, round)
    {
    }

    public override void CauseEventEffect()
    {
        Debug.Log("CREATURE SCRIPT DAMAGE RND END TURN======>DAMAGERND");
        CreatureLogic[] cl = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();
        int rnd = Random.Range(0, cl.Length);


        if (cl[rnd].CreatureDef > 0)
        {
            new DealDamageCommand(cl[rnd].ID, specialAmount, cl[rnd].MaxHealth ,
                cl[rnd].CreatureDef - specialAmount).AddToQueue();
            
        }
        else if(cl[rnd].CreatureDef<0)
        {
            new DealDamageCommand(cl[rnd].ID, specialAmount, cl[rnd].MaxHealth + (cl[rnd].CreatureDef-specialAmount),
                cl[rnd].CreatureDef - specialAmount).AddToQueue();
        }  else 
        {
            new DealDamageCommand(cl[rnd].ID, specialAmount, cl[rnd].MaxHealth + (cl[rnd].CreatureDef-specialAmount),
                cl[rnd].CreatureDef - specialAmount).AddToQueue();
        }

        cl[rnd].MaxHealth -= specialAmount;

    }
}
