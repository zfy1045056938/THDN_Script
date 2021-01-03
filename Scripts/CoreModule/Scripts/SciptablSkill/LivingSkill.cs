using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Collections.Generic;

using UnityEngine.Serialization;
using System.Text;

//Common For Buff,Self,
public abstract class LivingSkill:ScriptableSkill{

    public LinearFloat skillexp = new LinearFloat();
    public LiveSkillType skillType = LiveSkillType.None;


    /// <summary>
    /// When Player use skill for target obj ,
    /// </summary>
    public abstract void UseSkill(Entity target);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
      public virtual bool CanCast(Skill skill,int lsRequirement)
    {
        if(skill.level >= lsRequirement)
        {

            return true;
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="skill"></param>
    /// <param name="targetPerc"></param>
    /// <returns></returns>
    public virtual bool CheckSuccess(Skill skill,float targetPerc)
    {
        float rnd = Random.Range(0, 1);
        if (rnd >= (1 - targetPerc)){
            //success und apply target skill
            return true;
        }
        return false;
    }

    public override string Tooltip(string tooltip, bool isRequirement = false)
    {
        return base.Tooltip(tooltip, isRequirement);
    }
}