using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreatureEDBuff : CreatureEffect
{
    public CreatureEDBuff(Players owner,CreatureLogic cl, int amount,int round,SpellBuffType type,DamageElementalType det):base(owner,cl,amount,round,type,det){}

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
        Debug.Log("For Element Dam to target");
        CreatureLogic[] cl = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();
         int rnd =Random.Range(0,cl.Length);
        if(cl.Length>0){
            Debug.Log("Has target ");
           
            new DealBuffCommand(cl[rnd].ID,specialAmount,det,round).AddToQueue();
             GlobalSetting.instance.SETLogs(string.Format("对{0}发动{1}效果",cl[rnd].card.name,det.ToString()));
        }else{
            Debug.Log("Not target");
        }
        //
       
    }

    public override void WhenCreatureAtking()
    {
        base.WhenCreatureAtking();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
