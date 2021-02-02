using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMana : CreatureEffect
{
    public override void WhenACreatureIsPlayed()
    {
        TurnManager.instance.WhoseTurn.manaLeft += specialAmount;
             GlobalSetting.instance.SETLogs(string.Format("激活效果:恢复{0}点能量",specialAmount));
   
    }

    public CreatureMana(Players owner, CreatureLogic creature, int specialAmount, int round) : base(owner, creature, specialAmount, round)
    {
    }
}
