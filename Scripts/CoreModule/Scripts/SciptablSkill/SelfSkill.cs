using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class SelfSkill : BounsSkill
{
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

   

    public override string Tooltip(string tooltip, bool isRequirement = false)
    {
        return base.Tooltip(tooltip, isRequirement);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}