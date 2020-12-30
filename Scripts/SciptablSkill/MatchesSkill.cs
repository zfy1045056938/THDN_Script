using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Unity.Mathematics;

public class MatchesSkill:ScriptableSkill{

    [Header("Damage")]
    public float damage ;
    public LinearFloat stunChance; // range [0,1]
    public LinearFloat stunTime; // in seconds

    //THDN ATK 
    public override void Apply(Entity entity, int level)
    {
        float critBouns = 0f;
        //normal needs check player who got elemental or others buffs und 
        if (entity.isMatches&& entity.target.health>0 && !entity.isFreeze && !entity.isElec)
        {
            //Check Crit
            float critPerc = UnityEngine.Random.Range(0, 1);
            if (critPerc > (1 - entity.crit))
            {

            }
            //Check has Elemental
            if (!entity.hasElemental)
            {
                
                entity.DealDamageToTarget(entity.target, damage,1f,1f, DamageType.Normal, ElementDamageType.None);
            }
            else
            {
                entity.DealDamageToTarget(entity.target, damage,1f,1f, DamageType.Normal, ElementDamageType.None);
            }
        }

    }

    public override float castTimeRemaining()
    {
        throw new NotImplementedException();
    }

    public override bool CheckSelf(Entity entity, int skillLevel)
    {
        throw new NotImplementedException();
    }

    public override bool CheckTarget(Entity entity)
    {
        throw new NotImplementedException();
    }

    public override float cooldownRemaining()
    {
        throw new NotImplementedException();
    }

   

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void OnCastFinished(Entity entity)
    {
        base.OnCastFinished(entity);
    }

    public override void OnCastStarted(Entity entity)
    {
        base.OnCastStarted(entity);
    }

    

    public override string Tooltip(string tooltip, bool isRequirement = false)
    {
        return base.Tooltip(tooltip, isRequirement);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}