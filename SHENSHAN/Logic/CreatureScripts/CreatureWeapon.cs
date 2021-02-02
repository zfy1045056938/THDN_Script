using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureWeapon : CreatureEffect
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public override void RegisterEventEffect()
    {
        base.RegisterEventEffect();
    }

    public override void UnRegisterEventEffect()
    {
        base.UnRegisterEventEffect();
    }

    public override void CauseEventEffect()
    {
        base.CauseEventEffect();
    }

    public override void WhenACreatureIsPlayed()
    {
    //    TurnManager.instance.WhoseTurn.atkDur+=specialAmount;
    //    TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.atkDurText.text = TurnManager.instance.WhoseTurn.atkDur.ToString();
       GlobalSetting.instance.SETLogs(string.Format("铁匠效果:恢复{0}点武器耐久度",specialAmount));
   
  
    }

    public override void WhenCreatureAtking()
    {
        base.WhenCreatureAtking();
    }

    public override void WhenACreatureDies()
    {
        base.WhenACreatureDies();
    }

    public CreatureWeapon(Players owner, CreatureLogic creature, int specialAmount,int round,SpellBuffType sbt,DamageElementalType det): base(owner, creature, specialAmount,round,sbt,det)
    {
    }
}
