using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.Serialization;
using System.Text;
//Common For Buff,Self,
public abstract class BounsSkill:ScriptableSkill{
[FormerlySerializedAs("bonusHealthMax")] public LinearInt healthMaxBonus;
    [FormerlySerializedAs("bonusManaMax")] public LinearInt manaMaxBonus;
    [FormerlySerializedAs("bonusDamage")] public LinearInt damageBonus;
    [FormerlySerializedAs("bonusDefense")] public LinearInt defenseBonus;
    [FormerlySerializedAs("bonusBlockChance")] public LinearFloat blockChanceBonus; // range [0,1]
    [FormerlySerializedAs("bonusCriticalChance")] public LinearFloat criticalChanceBonus; // range [0,1]
    [FormerlySerializedAs("bonusHealthPercentPerSecond")] public LinearFloat healthPercentPerSecondBonus; // 0.1=10%; can be negative too
    [FormerlySerializedAs("bonusManaPercentPerSecond")] public LinearFloat manaPercentPerSecondBonus; // 0.1=10%; can be negative too
    [FormerlySerializedAs("bonusSpeed")] public LinearFloat speedBonus; // can be negative too
    
    public override string Tooltip(string tooltip, bool isRequirement = false)
    {
        return base.Tooltip(tooltip, isRequirement);
    }
}