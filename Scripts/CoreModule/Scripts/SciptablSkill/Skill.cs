// The Skill struct only contains the dynamic skill properties, so that the
// static properties can be read from the scriptable object. The benefits are
// low bandwidth and easy Player database saving (saves always refer to the
// scriptable skill, so we can change that any time).
//
// Skills have to be structs in order to work with SyncLists.
//
// We implemented the cooldowns in a non-traditional way. Instead of counting
// and increasing the elapsed time since the last cast, we simply set the
// 'end' Time variable to NetworkTime.time + cooldown after casting each time.
// This way we don't need an extra Update method that increases the elapsed time
// for each skill all the time.
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Mirror;
using System.Collections;
using System;



/// <summary>
/// u
/// </summary>
[System.Serializable]
public partial struct Skill
{
    // hashcode used to reference the real ItemTemplate (can't link to template
    // directly because synclist only supports simple types). and syncing a
    // string's hashcode instead of the string takes WAY less bandwidth.
    public int hash;


   
    // dynamic stats (cooldowns etc.)
    public int level; // 0 if not learned, >0 if learned
    public int maxLevel => data.maxLevel;

    public double castTimeEnd; // server time. double for long term precision.
    public double cooldownEnd; // server time. double for long term precision.
   

    // wrappers for easier access
    public ScriptableSkill data
    {
        get
        {
            // show a useful error message if the key can't be found
            // note: ScriptableSkill.OnValidate 'is in resource folder' check
            //       causes Unity SendMessage warnings and false positives.
            //       this solution is a lot better.
         
            return ScriptableSkill.dict[hash];
        }
    }
    public string name => data.name;
    //
    public float castTime ;

    public float cooldown;

    //rtv
    public float amount;
    //
    public float manaCosts ;
   
   
   
    //when currentexp >= maxexp ++level current exp =0 & max => maxExp + (maxExp *10% )
    public float currentexp;
    public float maxExp;
  
    public int upgradeRequiredLevel => data.requiredLevel;
    public float upgradeRequiredSkillExperience => data.requiredSkillExperience;
    //
    public bool hasLearn { get; set; }
    // runtime data increase by level & buffs & equipments 

    //before use skill ,needs check self enough to cast that highlight the skill icon und
    // can use skill ,the requirement for skill has these element check before cast
    //1.normalAtk->matches(dps with speed und PERC(crit,block,flash))
    //, 2. skills -> cp && mana && cooldown==0
    //    3.rage -> ragebouns>=100%
    //
    public bool CheckSelf(Entity caster, bool checkSkillReady=true)
    {
        return (!checkSkillReady || IsReady() &&
               data.CheckSelf(caster, level) && data.CheckTarget(caster.target) );
    }

    //Check target for player who use skill to target ,check target state health or buff can attack to target 100%
    // 
    public bool CheckTarget(Entity caster) { return data.CheckTarget(caster); }
    //public bool CheckDistance(Entity caster, out Vector3 destination) { return data.CheckDistance(caster, level, out destination); }
    public void Apply(Entity caster) { data.Apply(caster, level); }
    public bool CanUse(Skill skill,Entity caster) { return caster.manaMax >= skill.manaCosts && EnoughAtCP(skill); }

    // constructors needs save to databse(skill in game can upgrade by skillpoints
    // every level needs power up the skill(5%~10%)
    public Skill(ScriptableSkill data)
    {
        hash = data.name.GetStableHashCode();

        // learned only if learned by default
        level = 0;
       
        currentexp = 0;
        maxExp = 100;   //default exp level1->100
        //
        hasLearn = false;
        amount = 0;
        castTime=0;
        manaCosts =0;
        cooldown=0;
        // ready immediately
        castTimeEnd = cooldownEnd = NetworkTime.time;
        
    }


    // tooltip - dynamic part
    //public string ToolTip(bool showRequirements = false)
    //{
    //    // unlearned skills (level 0) should still show tooltip for level 1
    //    int showLevel = Mathf.Max(level, 1);

    //    // we use a StringBuilder so that addons can modify tooltips later too
    //    // ('string' itself can't be passed as a mutable object)
    //    StringBuilder tip = new StringBuilder(data.ToolTip(showLevel, showRequirements));

    //    // addon system hooks
    //    Util.InvokeMany(typeof(Skill), this, "ToolTip_", tip);

    //    // only show upgrade if learned and not max level yet
    //    if (0 < level && level < maxLevel)
    //    {
    //        tip.Append("\n<i>Upgrade:</i>\n" +
    //                   "<i>  Required Level: " + upgradeRequiredLevel + "</i>\n" +
    //                   "<i>  Required Skill Exp.: " + upgradeRequiredSkillExperience + "</i>\n");
    //    }

    //    return tip.ToString();
    //}

    // how much time remaining until the casttime ends? (using server time)
    public float CastTimeRemaining() => NetworkTime.time >= castTimeEnd ? 0 : (float)(castTimeEnd - NetworkTime.time);

    // we are casting a skill if the casttime remaining is > 0
    public bool IsCasting() => CastTimeRemaining() > 0;

    // how much time remaining until the cooldown ends? (using server time)
    public float CooldownRemaining() => NetworkTime.time >= cooldownEnd ? 0 : (float)(cooldownEnd - NetworkTime.time);
    
    public bool EnoughAtCP(Skill skill) { return false; }

    //At BatteleField skill stage
    public bool IsOnCooldown() => CooldownRemaining() > 0;



    //Check mana && cp && cooldown 
    public bool IsReady() => !IsCasting() && !IsOnCooldown() ;



}

public class SyncListSkill : SyncList<Skill> {}
