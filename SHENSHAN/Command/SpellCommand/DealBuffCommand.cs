using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealBuffCommand : Command
{
    public DealBuffCommand(int targetId, int amount, SpellBuffType buffType,int round)
    {
        targetID = targetId;
        this.amount = amount;
        this.buffType = buffType;
        this.round = round;
    }
    
    public DealBuffCommand(int targetId, int amount, DamageElementalType detype,int round)
    {
        targetID = targetId;
        this.amount = amount;
        this.det = detype;
        this.round = round;
        
    }
    
    
    public DealBuffCommand(int targetId, int amount, SpellBuffType buffType, DamageElementalType det, int round)
    {
        targetID = targetId;
        this.amount = amount;
        this.buffType = buffType;
        this.det = det;
        this.round = round;
    }

    public int targetID;
    public int amount;
    public SpellBuffType buffType;
    public DamageElementalType det;
    public int round;
    
    public override void StartCommandExecution()
    {
        Debug.Log("DEAL BUFF COMMAND START");
        GameObject target = IDHolder.GetComponentWithID(targetID);

        if(target!=null){
        CreatureLogic cl = new CreatureLogic(TurnManager.instance.WhoseTurn,target.GetComponent<OneCreatureManager>().cardAsset);

        Debug.Log("bufftype ist"+buffType.ToString() + "det ist"+det.ToString());
        
            if (buffType != SpellBuffType.None)
            {
                Debug.Log("Active Buff");
                target.GetComponent<OneCreatureManager>().AddBuffWithCreature(cl, amount, buffType,round);
            }else if (det != DamageElementalType.None)
            {
                Debug.Log("Active DET Buff");
                target.GetComponent<OneCreatureManager>().ElementalBuff(cl, amount, round,det);
            }

            Command.CommandExecutionComplete();
        }else{
            Debug.LogWarning("Can't Got Obj");
             Command.CommandExecutionComplete();
        }
    }
}
