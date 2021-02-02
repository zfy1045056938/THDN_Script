using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellGroupDamage : SpellEffect
{
  public SpellBuffType buffType = SpellBuffType.None;
  public override void ActiveRoundEffect(int amount = 0, ICharacter target = null, int roundTime = 0,
    DamageElementalType type = DamageElementalType.None)
  {
    Debug.Log("CREATURE SCRIPT DAMAGE RND======>DAMAGERND");
        CreatureLogic[] cl = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();
        int rnd = Random.Range(0, cl.Length);
        
        if (cl.Length > 0)
        {
          foreach(CreatureLogic c in cl){
            if (c.CreatureDef > 0)
            {
                new DealDamageCommand(CreatureLogic.creatureCreatedThisGame[target.ID].ID, amount, CreatureLogic.creatureCreatedThisGame[target.ID].MaxHealth,
                    CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef - amount).AddToQueue();

            }
            else if (CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef < 0)
            {
                new DealDamageCommand(CreatureLogic.creatureCreatedThisGame[target.ID].ID, amount,
                    CreatureLogic.creatureCreatedThisGame[target.ID].MaxHealth + (CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef - amount),
                    CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef - amount).AddToQueue();
            }
            else
            {
                new DealDamageCommand(CreatureLogic.creatureCreatedThisGame[target.ID].ID, amount,
                    CreatureLogic.creatureCreatedThisGame[target.ID].MaxHealth + (CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef - amount),
                    CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef - amount).AddToQueue();
            }

            if (CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef > 0)
            {
                CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef -= amount;
                if (CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef - amount < 0)
                {
                    CreatureLogic.creatureCreatedThisGame[target.ID].MaxHealth+=CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef - amount;
                }
            }
            else
            {
                CreatureLogic.creatureCreatedThisGame[target.ID].MaxHealth -= amount;
            }


            //
             if(type!=DamageElementalType.None){
               Debug.Log("GRROUP DAMAGE HAS BUFF"+type.ToString());
      new DealBuffCommand(c.ID,amount,type,roundTime).AddToQueue();
    }
        }
    }
    
   
  }

}
