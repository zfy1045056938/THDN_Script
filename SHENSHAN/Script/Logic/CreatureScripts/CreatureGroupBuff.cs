using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreatureGroupBuff : CreatureEffect
{
     public CreatureGroupBuff(Players owner,CreatureLogic cl, int amount,int round,SpellBuffType type,DamageElementalType det):base(owner,cl,amount,round,type,det){}

    public override void CauseEventEffect()
    {
        base.CauseEventEffect();
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void RegisterEventEffect()
    {
        base.RegisterEventEffect();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public override void UnRegisterEventEffect()
    {
        base.UnRegisterEventEffect();
    }

    public override void WhenACreatureDies()
    {
        base.WhenACreatureDies();
    }

    public override void WhenACreatureIsPlayed()
    {
        Debug.Log("GROUP BUFF ACTIVE==============>ACTIVE");
        CreatureLogic[] cl = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();
        if (cl.Length > 0)
        {
            foreach (CreatureLogic c in cl)
            {
              
                new DealBuffCommand(c.ID, specialAmount, det, round).AddToQueue();
            }
        }
        else
        {
            Debug.Log("No Creature At table");
        }
         GlobalSetting.instance.SETLogs(string.Format("群体效果,对面场上随从获得{0}效果",det.ToString()));
    }

    public override void WhenCreatureAtking()
    {
        base.WhenCreatureAtking();
    }

    // Start is called before the first frame update
}
