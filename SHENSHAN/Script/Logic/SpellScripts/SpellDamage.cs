using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDamage : SpellEffect
{
  

    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        Debug.Log("CREATURE SCRIPT DAMAGE RND======>DAMAGERND");
        CreatureLogic[] cl = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();
        int rnd = Random.Range(0, cl.Length);
        
        if (cl.Length > 0)
        {
            if(CreatureLogic.creatureCreatedThisGame.ContainsKey(target.ID)){
            if (CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef > 0)
            {
                new DealDamageCommand(CreatureLogic.creatureCreatedThisGame[target.ID].ID, specialAmount, CreatureLogic.creatureCreatedThisGame[target.ID].MaxHealth,
                    CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef - specialAmount).AddToQueue();

            }
            else if (CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef < 0)
            {
                new DealDamageCommand(CreatureLogic.creatureCreatedThisGame[target.ID].ID, specialAmount,
                    CreatureLogic.creatureCreatedThisGame[target.ID].MaxHealth + (CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef - specialAmount),
                    CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef - specialAmount).AddToQueue();
            }
            else
            {
                new DealDamageCommand(CreatureLogic.creatureCreatedThisGame[target.ID].ID, specialAmount,
                    CreatureLogic.creatureCreatedThisGame[target.ID].MaxHealth + (CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef - specialAmount),
                    CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef - specialAmount).AddToQueue();
            }

            if (CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef > 0)
            {
                CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef -= specialAmount;
                if (CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef - specialAmount < 0)
                {
                    CreatureLogic.creatureCreatedThisGame[target.ID].MaxHealth+=CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef - specialAmount;
                }
            }
            else
            {
                CreatureLogic.creatureCreatedThisGame[target.ID].MaxHealth -= specialAmount;
            }
        }else{
         Debug.Log("DO NOTHING");
        }
        }else{

             int defafter =0;
        int healthAfter = 0;
        int hurtTaunt=0;
            //Character
             if (owner.otherPlayer.CreatureDef > 0)
            {
                defafter =owner.otherPlayer.CreatureDef- specialAmount;
                healthAfter = owner.otherPlayer.MaxHealth;
                //if less than 0
                if (defafter <= 0)
                {
                    healthAfter =owner.otherPlayer.MaxHealth+ defafter;
                    // defafter = 0;
                    //Check has braek effect or artifact effect
//                    if (owner.otherPlayer.hasBreakTaunt == true)
//                    {
//                        MaxHealth -= 1;
//                    }
defafter=0;
                }
            }
            else
            {
                healthAfter =owner.otherPlayer.MaxHealth- specialAmount;
            }
//        int atkHealAfter = owner.otherPlayer.MaxHealth - CreatureAtk;

            Debug.Log("GO FACE RESULT"+defafter+"\t"+healthAfter);
            owner.otherPlayer.CreatureDef = defafter;
            Debug.Log("Def After ist"+defafter);
            owner.otherPlayer.MaxHealth = healthAfter;

        

            // if(owner.otherPlayer.hurtDef>0){
            //      new CreatureAttackCommand(owner.otherPlayer.playerID,
            //  UniqueCreatureId, CreatureAtk,
            //     hurtTaunt,
            //     hurtDef, healthAfter,
            //     CreatureDef,
            //    defafter ).AddToQueue();
            // }else{
            //Attack Command
            new DealDamageCommand(owner.otherPlayer.playerID,
             specialAmount, healthAfter,
                defafter
               ).AddToQueue();
            // }



        }
        }
    

    

}
