using System;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Collections.Generic;

using UnityEngine.Serialization;
using System.Text;


    public class LockPick:LivingSkill
    {
        public LockPick()
        {
        }

    public override void Apply(Entity entity, int level)
    {
        throw new NotImplementedException();
    }

    public override bool CanCast(Skill skill, int lsRequirement)
    {
        return base.CanCast(skill, lsRequirement);
    }

    public override float castTimeRemaining()
    {
        throw new NotImplementedException();
    }

    public override bool CheckSelf(Entity entity, int skillLevel)
    {
        throw new NotImplementedException();
    }

    public override bool CheckSuccess(Skill skill, float targetPerc)
    {
        return base.CheckSuccess(skill, targetPerc);
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

    public override void UseSkill(Entity target)
    {
        throw new NotImplementedException();
    }
}
