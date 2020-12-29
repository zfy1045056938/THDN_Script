using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Collections.Generic;
using System;



/// <summary>
/// every damage skill likes deal amount to target use this functions as damage skill before use skill ,nneds check
/// 1.CheckSelf&target ,
/// 2.CheckCp enough to use skill
/// 3.when can use skill , apply make skills to target
/// </summary>
public class DamageSkill : ScriptableSkill {

    [Header("Damage")]
    public LinearInt damage = new LinearInt { baseValue = 1 };
    public LinearFloat stunChance; // range [0,1]
    public LinearFloat stunTime; // in seconds

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

    /// <summary>
    /// when player matches , collect target gp to cp,und when the cp enough to use
    /// the trget skill, highlight the target skill und note player can use it.
    ///  when player click the skll btn needs check functions und check target states
    ///  entity cp ist the gp collect at battlefield when click the skill check skill gp check can use
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    // public override bool EnoughCP(Entity entity,Skill skill)
    // {
    //     List<GamePiece> cpList = new List<GamePiece>();
    //     //for (int i = 0; i < cpList.Count; i++)
    //     //{
    //     //    //if got key then add to 
    //     //    if (entity.collectPool.ContainsKey(cpList[i]))
    //     //    {
    //     //        return true;
    //     //    }
    //     //}



    //     return false;




    // }

   
}