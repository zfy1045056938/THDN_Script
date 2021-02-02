using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DamageAllOpponentCreature : SpellEffect
{

    public override void ActiveEffect(int specialAmount=0,ICharacter characterID=null){
        Debug.Log("Active Group Effect");
        //CreatureLogic[] creatureToDamage = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();
       // List<DamageCommand> targets = new List<DamageCommand>();
       // foreach (CreatureLogic cl in creatureToDamage)
        
       //     targets.Add(new DamageCommand(cl.ID, cl.MaxHealth - specialAmount, specialAmount));
        
        
       //new DamageCommandInfo(targets).AddQueue(); 
        //                foreach (CreatureLogic cl in creatureToDamage)
        //{
        //    cl.MaxHealth -= specialAmount;
        //}
    }


    
}  