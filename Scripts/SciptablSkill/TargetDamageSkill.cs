using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Text;

public class TargetDamageSkill:DamageSkill{

	
    /// <summary>
    /// use skill when player cast skill
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="level"></param>
    public override void Apply(Entity caster, int skillLevel)
    {
        // deal damage directly with base damage + skill damage
        caster.DealDamageToTarget(caster.target,
                            caster.damage + damage.Get(skillLevel),
                            stunChance.Get(skillLevel),
                            stunTime.Get(skillLevel));
        
    }

    public override float castTimeRemaining()
    {
        throw new NotImplementedException();
    }

    public override bool CheckSelf(Entity caster, int skillLevel)
    {
        return caster != null;
    }

    public override bool CheckTarget(Entity caster)
    {
        return caster.target != null;
    }



    //
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

   


    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="gps"></param>
    /// <returns></returns>





    //For Event or equipment ,active effect to target
    public void SpawnSkill(Entity entity){
    		//Generate FBX data

    		//entity Data



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