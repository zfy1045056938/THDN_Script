using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Text;

/// <summary>
///  Buff skill 
/// </summary>
public class BuffSkill:ScriptableSkill{

    [Header("Damage")]
    public LinearFloat amount = new LinearFloat();
    public LinearFloat castTime = new LinearFloat();
    public LinearFloat effectTime = new LinearFloat();      //active effect time
    public BuffType buffType = BuffType.None;
    public LinearFloat cdTime = new LinearFloat();
    public float buffTime=0f;
    public bool remainAfterDeath=false;
    public BuffSkillEffect effect;  //ass Obj show app
    public int maxLevel =5;
    public override void Apply(Entity entity, int level)
    {
        throw new NotImplementedException();
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

   


    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    public void SpawnEffect(Entity caster, Entity spawnTarget)
    {
        if (effect != null)
        {
            GameObject go = Instantiate(effect.gameObject, spawnTarget.transform.position, Quaternion.identity);
            BuffSkillEffect effectComponent = go.GetComponent<BuffSkillEffect>();
            effectComponent.caster = caster;
            effectComponent.target = spawnTarget;
            effectComponent.buffName = name;
            NetworkServer.Spawn(go);
        }
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