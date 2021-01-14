using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class SpellWeaponDamage : SpellEffect
{
    //检测条件
    //1.是否有荆棘如果有对攻击者造成伤害
    //2.是否有闪避,低于则无法攻击到对方
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        bool hasTaunt = false;
        int tauntIndex=0;
        CreatureLogic tc=null;
        GameObject objs = null;
        int healthAfter = 0;
        int defAfter = 0;
        
        CreatureLogic[] cl = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();

        if (cl.Length > 0)
        {
            foreach (var c in cl)
            {
                if (c.Taunt == true)
                {
                    hasTaunt = true;
                    tauntIndex = c.ID;
                    objs = IDHolder.GetComponentWithID(tauntIndex);
                    
                     tc = CreatureLogic.creatureCreatedThisGame[tauntIndex];
                     Debug.Log("tc creature "+tc.MaxHealth+"\t"+tc.CreatureDef);
                }
            }

            int rnd = Random.Range(0, cl.Length);
            
           

            if (hasTaunt==false)
            {
                //
                Debug.Log("WEAPON EFFECT ACTIVE to player!!!!!!!!!!!!!!!");

                if (TurnManager.instance.WhoseTurn.otherPlayer.CreatureDef > 0)
                {
                    Debug.Log("Minus Armor");
                    defAfter= TurnManager.instance.WhoseTurn.otherPlayer.CreatureDef -
                        TurnManager.instance.WhoseTurn.CreatureAtk;
                        //
                    if(TurnManager.instance.WhoseTurn.otherPlayer.CreatureDef - TurnManager.instance.WhoseTurn.CreatureAtk <0)
                {
                    Debug.Log("Character Armor -> Health");
                   healthAfter= TurnManager.instance.WhoseTurn.otherPlayer.MaxHealth +
                        (TurnManager.instance.WhoseTurn.otherPlayer.CreatureDef -
                         TurnManager.instance.WhoseTurn.CreatureAtk);
                   defAfter = 0;

                   TurnManager.instance.WhoseTurn.MaxHealth = healthAfter;
                   
                   TurnManager.instance.WhoseTurn.CreatureDef = defAfter;
                }
                    TurnManager.instance.WhoseTurn.otherPlayer.CreatureDef = defAfter;
                    
                }
                // else if(TurnManager.instance.WhoseTurn.otherPlayer.CreatureDef - TurnManager.instance.WhoseTurn.CreatureAtk <0)
                // {
                //     Debug.Log("Character Armor -> Health");
                //    healthAfter= TurnManager.instance.WhoseTurn.otherPlayer.MaxHealth +
                //         (TurnManager.instance.WhoseTurn.otherPlayer.CreatureDef -
                //          TurnManager.instance.WhoseTurn.CreatureAtk);
                //    defAfter = 0;

                //    TurnManager.instance.WhoseTurn.MaxHealth = healthAfter;
                   
                //    TurnManager.instance.WhoseTurn.CreatureDef = defAfter;
                // }
                else
                {
                    Debug.Log("Minus Health ");
                   healthAfter= TurnManager.instance.WhoseTurn.otherPlayer.MaxHealth -
                        TurnManager.instance.WhoseTurn.CreatureAtk;
                    
                    TurnManager.instance.WhoseTurn.otherPlayer.MaxHealth = healthAfter;
                }
                
                new DealDamageCommand(TurnManager.instance.WhoseTurn.otherPlayer.playerID, TurnManager.instance.WhoseTurn.CreatureAtk, healthAfter,
                    defAfter).AddToQueue();

                     GlobalSetting.instance.SETLogs(string.Format("武器攻击对面英雄受到{0}点伤害",amount));

                
                if(TurnManager.instance.WhoseTurn.otherPlayer.hasHurtDef==true){
                    TurnManager.instance.WhoseTurn.MaxHealth -= TurnManager.instance.WhoseTurn.otherPlayer.hurtDef;

 new DealDamageCommand(TurnManager.instance.WhoseTurn.playerID, TurnManager.instance.WhoseTurn.otherPlayer.hurtDef, healthAfter,
                    defAfter).AddToQueue();



                }
            }else
            {

                healthAfter = tc.MaxHealth;
                defAfter = tc.CreatureDef;
                //    atk creature who has taunt
                Debug.Log("WEAPON EFFECT ACTIVE TO CREATURE WHO HAS TAUNT!!!" + tc.card.name.ToString());
                GlobalSetting.instance.SETLogs(string.Format("武器攻击嘲讽随从{0}受到{1}点伤害",tc.card.name.ToString(),amount));
                if(tc.CreatureDef>0){

                if (tc.CreatureDef-TurnManager.instance.WhoseTurn.CreatureAtk < 0)
                {
                    Debug.Log("Armor -> Health");
                   defAfter=tc.CreatureDef - TurnManager.instance.WhoseTurn.CreatureAtk;
                   healthAfter+=defAfter;
                   Debug.Log("HF"+healthAfter);
                    defAfter=0;
 tc.MaxHealth = healthAfter;
                tc.CreatureDef=defAfter;

                Debug.Log(tc.card.name
                +"now creature v ist"+tc.CreatureDef+":"+tc.MaxHealth);
             new DealDamageCommand(tauntIndex, TurnManager.instance.WhoseTurn.CreatureAtk, healthAfter,
                    defAfter).AddToQueue();
                }else{
                     Debug.Log("Minus CL Armor");
                    defAfter -= TurnManager.instance.WhoseTurn.CreatureAtk;
                    Debug.Log("After def is"+defAfter.ToString());
                    tc.CreatureDef=defAfter;
                    Debug.Log(tc.CreatureDef+"ist def");

                     new DealDamageCommand(tauntIndex, TurnManager.instance.WhoseTurn.CreatureAtk, healthAfter,
                    defAfter).AddToQueue();
                }

                


                }
                else
                {
                  healthAfter-=  TurnManager.instance.WhoseTurn.CreatureAtk;
//                    new DealDamageCommand(TurnManager.instance.WhoseTurn.otherPlayer.playerID, TurnManager.instance.WhoseTurn.CreatureAtk, TurnManager.instance.WhoseTurn.otherPlayer.MaxHealth - TurnManager.instance.WhoseTurn.CreatureAtk,
//                        0).AddToQueue();
//                    TurnManager.instance.WhoseTurn.otherPlayer.CreatureAtk = healthAfter;
                    tc.MaxHealth = healthAfter;
                    new DealDamageCommand(tauntIndex, TurnManager.instance.WhoseTurn.CreatureAtk, healthAfter,
                    defAfter).AddToQueue();

                     GlobalSetting.instance.SETLogs(string.Format("武器攻击对面英雄受到{0}点伤害",amount));

                }
                
             
             
            }
            
            
        }else
        {
            
            //no creature relative atk player
//            new DealDamageCommand(TurnManager.instance.WhoseTurn.otherPlayer.playerID, TurnManager.instance.WhoseTurn.CreatureAtk, TurnManager.instance.WhoseTurn.otherPlayer.MaxHealth - TurnManager.instance.WhoseTurn.CreatureAtk,
//                TurnManager.instance.WhoseTurn.otherPlayer.CreatureDef - TurnManager.instance.WhoseTurn.CreatureAtk).AddToQueue();
//            
//
//            TurnManager.instance.WhoseTurn.otherPlayer.MaxHealth -= TurnManager.instance.WhoseTurn.CreatureAtk;
            if (TurnManager.instance.WhoseTurn.otherPlayer.CreatureDef-TurnManager.instance.WhoseTurn.CreatureAtk > 0)
            {
               defAfter=  TurnManager.instance.WhoseTurn.otherPlayer.CreatureDef -
                    TurnManager.instance.WhoseTurn.CreatureAtk;
               TurnManager.instance.WhoseTurn.otherPlayer.CreatureDef = defAfter;
            }
            else if(TurnManager.instance.WhoseTurn.otherPlayer.CreatureDef - TurnManager.instance.WhoseTurn.CreatureAtk <0)
            {
               healthAfter= TurnManager.instance.WhoseTurn.otherPlayer.MaxHealth +
                    (TurnManager.instance.WhoseTurn.otherPlayer.CreatureDef -
                     TurnManager.instance.WhoseTurn.CreatureAtk);

               TurnManager.instance.WhoseTurn.otherPlayer.MaxHealth = healthAfter;
               TurnManager.instance.WhoseTurn.otherPlayer.CreatureDef = 0;
            }
            else
            {
               healthAfter= TurnManager.instance.WhoseTurn.otherPlayer.MaxHealth -
                    TurnManager.instance.WhoseTurn.CreatureAtk;

               TurnManager.instance.WhoseTurn.otherPlayer.MaxHealth = healthAfter;
            }


                
            new DealDamageCommand(TurnManager.instance.WhoseTurn.otherPlayer.playerID, TurnManager.instance.WhoseTurn.CreatureAtk, healthAfter,
                defAfter).AddToQueue();
        }
        
        
           
        healthAfter = 0;
        defAfter=0;

    }
}
