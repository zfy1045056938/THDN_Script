using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCombo : CreatureEffect
{

     public CreatureCombo(Players owner, CreatureLogic creature, int specialAmount,int round,SpellBuffType sbt,DamageElementalType det): base(owner, creature, specialAmount,round,sbt,det)
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
        Debug.Log(TurnManager.instance.WhoseTurn.isFirstCard+"ist state");
        if(TurnManager.instance.WhoseTurn.isFirstCard==false){
            Debug.Log("Combo Effect Active");
            new DealBuffCommand(creature.ID,specialAmount,sbt,round).AddToQueue();
        }
    }

    public override void WhenCreatureAtking()
    {
        base.WhenCreatureAtking();
    }

}
