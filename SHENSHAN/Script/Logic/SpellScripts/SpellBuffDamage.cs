using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBuffDamage : SpellEffect
{

    public DamageElementalType det;
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        base.ActiveEffect(specialAmount, target);
    }

    public override void ActiveEffectToTargetStat(int specialAmount = 0, ICharacter target = null, SpellBuffType type = SpellBuffType.None)
    {
        base.ActiveEffectToTargetStat(specialAmount, target, type);
    }

    public override void ActiveEffectToTargetStat(int specialAmount = 0, ICharacter target = null, DamageElementalType type = DamageElementalType.None)
    {
        base.ActiveEffectToTargetStat(specialAmount, target, type);
    }

    public override void ActiveEffectToTargetStat(int specialAmount = 0, ICharacter target = null, ArtifactType type = ArtifactType.None)
    {
        base.ActiveEffectToTargetStat(specialAmount, target, type);
    }

    public override void ActiveRoundEffect(int amount = 0, ICharacter target = null, int roundTime = 0, DamageElementalType type = DamageElementalType.None)
    {
           Debug.Log("CREATURE SCRIPT DAMAGE RND======>DAMAGERND");
        CreatureLogic[] cl = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();
        int rnd = Random.Range(0, cl.Length);
        
        if (cl.Length > 0)
        {
            if(CreatureLogic.creatureCreatedThisGame.ContainsKey(target.ID)){
            if (CreatureLogic.creatureCreatedThisGame[target.ID].CreatureDef > 0)
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
                defafter =owner.otherPlayer.CreatureDef- amount;
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
                healthAfter =owner.otherPlayer.MaxHealth- amount;
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
             amount, healthAfter,
                defafter
               ).AddToQueue();
            // }



        }
        //
        if(TurnManager.instance.WhoseTurn.table.creatureOnTable.Count>0 || TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.Count>0){
             Debug.Log("Round Effect");
        new DealBuffCommand(target.ID,amount,det,roundTime).AddToQueue();
        }

    }

    public override void CauseEventEffect()
    {
        base.CauseEventEffect();
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void RegisterEventEffect()
    {
        base.RegisterEventEffect();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public override void UnRegisterEventEffect()
    {
        base.UnRegisterEventEffect();
    }
}
