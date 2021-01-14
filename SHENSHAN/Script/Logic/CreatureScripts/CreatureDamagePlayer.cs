using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureDamagePlayer : CreatureEffect
{
    

    public CreatureDamagePlayer(Players owner, CreatureLogic creature, int specialAmount, int round,SpellBuffType buff,DamageElementalType det) : base(owner, creature, specialAmount, round,buff,det)
    {
       
    }


    public override void WhenACreatureIsPlayed()
    {
        Debug.Log("CREATURE DAMAGE PLAYER ACTIVE!!!!!!!!!!!!!!");
        if (owner.otherPlayer.CreatureDef > 0)
        {
            new DealDamageCommand(TurnManager.instance.WhoseTurn.otherPlayer.playerID, specialAmount, owner.otherPlayer.MaxHealth - specialAmount,
                owner.otherPlayer.MaxHealth -= specialAmount).AddToQueue();
//            owner.otherPlayer.Def -= specialAmount;
        }
        else if(owner.otherPlayer.CreatureDef-specialAmount <0)
        {
            new DealDamageCommand(TurnManager.instance.WhoseTurn.otherPlayer.playerID, specialAmount, owner.otherPlayer.MaxHealth + (owner.otherPlayer.CreatureDef- specialAmount),
                owner.otherPlayer.MaxHealth -= specialAmount).AddToQueue();
//            owner.otherPlayer.Def -= specialAmount;
//            owner.otherPlayer.MaxHealth += (owner.otherPlayer.CreatureDef - specialAmount);
//            
        }
        else
        {
            new DealDamageCommand(TurnManager.instance.WhoseTurn.otherPlayer.playerID, specialAmount, owner.otherPlayer.MaxHealth - specialAmount,
                0).AddToQueue();
           
            Debug.Log(owner.otherPlayer.MaxHealth+"Now ist ");
        }
        owner.otherPlayer.MaxHealth -= specialAmount;
        Debug.Log(owner.otherPlayer.MaxHealth+"Now ist ");
    }
}
