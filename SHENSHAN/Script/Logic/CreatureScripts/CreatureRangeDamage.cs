using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureRangeDamage : CreatureEffect
{

    public CreatureRangeDamage(Players owner, CreatureLogic creature, int specialAmount,int round,SpellBuffType sbt,DamageElementalType det): base(owner, creature, specialAmount,round,sbt,det)
    {}
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
       int rd = Random.Range(1,specialAmount);
       CreatureLogic[] cl = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();
        int rnd = Random.Range(0,cl.Length);
       if (cl.Length > 0)
        {
            if (cl[rnd].CreatureDef > 0)
            {
                new DealDamageCommand(cl[rnd].ID, rd, cl[rnd].MaxHealth,
                    cl[rnd].CreatureDef - rd).AddToQueue();

            }
            else if (cl[rnd].CreatureDef < 0)
            {
                new DealDamageCommand(cl[rnd].ID, rd,
                    cl[rnd].MaxHealth + (cl[rnd].CreatureDef - rd),
                    cl[rnd].CreatureDef - rd).AddToQueue();
            }
            else
            {
                new DealDamageCommand(cl[rnd].ID, rd,
                    cl[rnd].MaxHealth + (cl[rnd].CreatureDef - rd),
                    cl[rnd].CreatureDef - rd).AddToQueue();
            }

            if (cl[rnd].CreatureDef > 0)
            {
                cl[rnd].CreatureDef -= rd;
                if (cl[rnd].CreatureDef - rd < 0)
                {
                    cl[rnd].MaxHealth+=cl[rnd].CreatureDef - rd;
                }
            }
            else
            {
                cl[rnd].MaxHealth -= rd;
            }
        }else{
         Debug.Log("DO NOTHING");
        }

             GlobalSetting.instance.SETLogs(string.Format("远程效果:DICE值为{0},{1}受到{2}点伤害",rd.ToString(),cl[rnd].card.name,rd.ToString()));
   
    }

    public override void WhenCreatureAtking()
    {
        base.WhenCreatureAtking();
    }
}
