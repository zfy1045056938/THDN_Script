using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMachine : CreatureEffect
{
     public CreatureMachine(Players owner, CreatureLogic creature, int specialAmount,int round,SpellBuffType sbt,DamageElementalType det): base(owner, creature, specialAmount,round,sbt,det)
    {}

    //
    public override void CauseEventEffect()
    {
       Debug.Log("Creature Machine Active");
       new DealBuffCommand(creature.ID,specialAmount,sbt,round).AddToQueue();
         GlobalSetting.instance.SETLogs(string.Format("机甲效果触发,回合结束获得{0}点{1}加成",specialAmount,sbt.ToString()));
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
          owner.EndTurnEvent += CauseEventEffect;
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public override void UnRegisterEventEffect()
    {
          owner.EndTurnEvent -= CauseEventEffect;
    }

    public override void WhenACreatureDies()
    {
        base.WhenACreatureDies();
    }

    public override void WhenACreatureIsPlayed()
    {
        base.WhenACreatureIsPlayed();
    }

    public override void WhenCreatureAtking()
    {
        base.WhenCreatureAtking();
    }
}
