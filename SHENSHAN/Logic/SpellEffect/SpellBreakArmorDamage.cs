using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBreakArmorDamage : SpellEffect
{
 public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
 {
     Debug.Log("SPELL BREAKDEF DAMAGE ACTIVE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
     CreatureLogic[] cl = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();
     //damage
      int totalArmor = TurnManager.instance.WhoseTurn.CreatureDef;
     if (TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.Count > 0)
     {
        foreach(var c in cl){
         //
        int healthAfter = c.MaxHealth;
        int defAfter =c.CreatureDef;
      if(c.CreatureDef - totalArmor >0){

          defAfter -= totalArmor;

          new DealDamageCommand(c.UniqueCreatureId,totalArmor,healthAfter,defAfter,0,DamageElementalType.None).AddToQueue();
          
      }else if(c.CreatureDef-totalArmor<0){
         Debug.Log("Armor -> Health");
         defAfter -= totalArmor;
         int reDef = defAfter -  totalArmor;
         c.CreatureDef=0;
         healthAfter += reDef;
         c.MaxHealth =  healthAfter;

         new DealDamageCommand(c.UniqueCreatureId,totalArmor,healthAfter,defAfter,0,DamageElementalType.None).AddToQueue();
      }else if(c.CreatureDef==0){


         new DealDamageCommand(CreatureLogic.creatureCreatedThisGame[c.ID].ID,totalArmor,target.MaxHealth-=totalArmor,target.CreatureDef-= totalArmor).AddToQueue();
         
         c.MaxHealth-= totalArmor;

        
      }
        }
     }
     else
     {
         Debug.Log("NO CREATURE AT TABLES");
     }
      TurnManager.instance.WhoseTurn.CreatureDef = 0;
 }
}
