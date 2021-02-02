using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class CreatureEffect 
{
    protected CreatureEffect(Players owner, CreatureLogic creature, int specialAmount, DiscoverType dt)
    {
        this.owner = owner;
        this.creature = creature;
        this.specialAmount = specialAmount;
        this.dt = dt;
    }

    protected CreatureEffect(Players owner, CreatureLogic creature, int specialAmount, int round, SpellBuffType sbt, DamageElementalType det)
    {
        this.owner = owner;
        this.creature = creature;
        this.specialAmount = specialAmount;
        this.round = round;
        this.sbt = sbt;
        this.det = det;
    }

     protected CreatureEffect(Players owner, CreatureLogic creature, int specialAmount, int round ,DamageElementalType det)
    {
        this.owner = owner;
        this.creature = creature;
        this.specialAmount = specialAmount;
        this.round = round;
        this.sbt = sbt;
        this.det = det;
    }

    public Players owner;
    public CreatureLogic creature;
    public int specialAmount;
    public int round;
    public SpellBuffType sbt;
    public DamageElementalType det;
    public DiscoverType dt;

    public CreatureEffect(Players owner, CreatureLogic creature, int specialAmount,int round)
    {
        this.creature = creature;
        this.owner = owner;
        this.specialAmount = specialAmount;
        this.round = round;
    }

    // METHODS FOR SPECIAL FX THAT LISTEN TO EVENTS
    public virtual void RegisterEventEffect(){}

    public virtual void UnRegisterEventEffect(){}

    public virtual void CauseEventEffect(){}

    // when player put creature on table then active effect-> command
    public virtual void WhenACreatureIsPlayed(){}
    
    //when player atk target then active effect
    public virtual  void WhenCreatureAtking(){}

    // when target die then cause effect->command
    public virtual void WhenACreatureDies(){}


}
