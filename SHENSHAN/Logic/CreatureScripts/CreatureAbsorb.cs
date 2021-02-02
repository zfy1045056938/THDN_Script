using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CreatureAbsorb : CreatureEffect
{
    

    public CreatureAbsorb(Players owner, CreatureLogic creature, int specialAmount, int round, SpellBuffType sbt, DamageElementalType det) : base(owner, creature, specialAmount, round, sbt, det)
    {
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

    public override void WhenACreatureDies()
    {
        base.WhenACreatureDies();
    }

    public override void WhenACreatureIsPlayed()
    {
      
       Debug.Log("Creature Absorb active , the effect minus health und effect if have");
       CreatureLogic[] ot = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();
       int rnd =Random.Range(0,TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.Count);
       if(ot.Length>0){
           SoundManager.instance.PlaySound(GlobalSetting.instance.AbsorbClip);
           //Rnd Got Damage by absorb damage then got healt 
           if (ot.Length > 0)
        {
            if (ot[rnd].CreatureDef > 0)
            {
                new DealDamageCommand(ot[rnd].ID, specialAmount, ot[rnd].MaxHealth,
                    ot[rnd].CreatureDef - specialAmount).AddToQueue();

            }
            else if (ot[rnd].CreatureDef < 0)
            {
                new DealDamageCommand(ot[rnd].ID, specialAmount,
                    ot[rnd].MaxHealth + (ot[rnd].CreatureDef - specialAmount),
                    ot[rnd].CreatureDef - specialAmount).AddToQueue();
            }
            else
            {
                new DealDamageCommand(ot[rnd].ID, specialAmount,
                    ot[rnd].MaxHealth + (ot[rnd].CreatureDef - specialAmount),
                    ot[rnd].CreatureDef - specialAmount).AddToQueue();
            }

            if (ot[rnd].CreatureDef > 0)
            {
                ot[rnd].CreatureDef -= specialAmount;
                if (ot[rnd].CreatureDef - specialAmount < 0)
                {
                    ot[rnd].MaxHealth+=ot[rnd].CreatureDef - specialAmount;
                }
            }
            else
            {
                ot[rnd].MaxHealth -= specialAmount;
            }
        }else{
         Debug.Log("DO NOTHING");
        }
           ot[rnd].MaxHealth -= specialAmount;
           
           //
           Debug.Log("Check Absorb Have other buff needs actuve");
            if(det != DamageElementalType.None){
                new DealBuffCommand(ot[rnd].ID,specialAmount,sbt,round);
            }else if(sbt != SpellBuffType.None){
                 new DealBuffCommand(ot[rnd].ID,specialAmount,det,round);
            }else{
                new DealDamageCommand(ot[rnd].ID,specialAmount,ot[rnd].MaxHealth-specialAmount,ot[rnd].CreatureDef,round,det).AddToQueue();

            }
            //Heal Player
            SoundManager.instance.PlaySound(GlobalSetting.instance.healClip);
            TurnManager.instance.WhoseTurn.MaxHealth += specialAmount;
            //Show ava face
            DamageEffect.ShowBuffEffect(TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.portraitImage.transform.position,
            SpellBuffType.Health,specialAmount);
            //
            
            TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.healthText.text=TurnManager.instance.WhoseTurn.MaxHealth.ToString();
              //GameLog
                if(det != DamageElementalType.None){
                GlobalSetting.instance.SETLogs(string.Format("侵蚀效果触发,对{0}造成{1}点伤害,{2}获得{3}点血量并获得{4}点{5}加成",creature.card.name,specialAmount,TurnManager.instance.WhoseTurn.otherPlayer.name,
                specialAmount,specialAmount,det.ToString()));
            }else if(sbt != SpellBuffType.None){
                 GlobalSetting.instance.SETLogs(string.Format("侵蚀效果触发,对{0}造成{1}点伤害,{2}获得{3}点血量,{4}获得{5}点{6}效果",creature.card.name,specialAmount,TurnManager.instance.WhoseTurn.otherPlayer.name,
                specialAmount,creature.card.name,specialAmount,det.ToString()));
            }else{
                 GlobalSetting.instance.SETLogs(string.Format("侵蚀效果触发,对{0}造成{1}点伤害,{2}获得{3}点血量",creature.card.name,specialAmount,TurnManager.instance.WhoseTurn.otherPlayer.name,
                specialAmount));
            }
       }else{
           Debug.Log("No creature");
       }
    }

    public override void WhenCreatureAtking()
    {


        
        
    }
}
