﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCoin : CreatureEffect
{
     public CreatureCoin(Players owner, CreatureLogic creature, int specialAmount,int round,SpellBuffType sbt,DamageElementalType det): base(owner, creature, specialAmount,round,sbt,det)
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
        TurnManager.instance.WhoseTurn.hasCoin=true;
        TurnManager.instance.WhoseTurn.coinNumber +=specialAmount;
        TurnManager.instance.WhoseTurn.ShowCoinPanel();
       
    }

    public override void WhenCreatureAtking()
    {
        base.WhenCreatureAtking();
    }

  
}
