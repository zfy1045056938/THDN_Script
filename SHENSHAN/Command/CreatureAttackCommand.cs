using UnityEngine;
using System.Collections;

public class CreatureAttackCommand : Command 
{
    public CreatureEffect SpellEffect { get; private set; }
    public event VOIDWITHNOARUGMENT CreatureAtkEvent;

    public delegate void VOIDWITHNOARUGMENT();
    // position of creature on enemy`s table that will be attacked
    // if enemyindex == -1 , attack an enemy character 
    private int TargetUniqueID;
    private int AttackerUniqueID;
    private int AttackerHealthAfter;
    private int TargetHealthAfter;
    private int DamageTakenByAttacker;
    private int DamageTakenByTarget;

    private int AtkDefAfter;
    private int TargetDefAfter;

    public CreatureAttackCommand(int targetID, int attackerID, int damageTakenByAttacker, 
    int damageTakenByTarget, int attackerHealthAfter,
     int targetHealthAfter,int atkDefAfter,int targetDefAfter)
    {
        this.TargetUniqueID = targetID;
        this.AttackerUniqueID = attackerID;
        
        this.AttackerHealthAfter = attackerHealthAfter;
        this.TargetHealthAfter = targetHealthAfter;
        
        this.DamageTakenByTarget = damageTakenByTarget;
        this.DamageTakenByAttacker = damageTakenByAttacker;
        
        this.AtkDefAfter = atkDefAfter;
        this.TargetDefAfter = targetDefAfter;
    }

    public override void StartCommandExecution()
    {
        if (CreatureAtkEvent != null)
        {
            
            CreatureAtkEvent.Invoke();
        }
        
        GameObject Attacker = IDHolder.GetComponentWithID(AttackerUniqueID);
        GameObject target =IDHolder.GetComponentWithID(TargetUniqueID);

       if(target!=null){
           Debug.Log("Atker  ist"+Attacker.GetComponent<OneCreatureManager>().cardAsset.name);
        Attacker.GetComponent<CreatureAtkVisual>().AttackTargetToObject(
            TargetUniqueID,DamageTakenByTarget,
        DamageTakenByAttacker,AttackerHealthAfter,
        TargetHealthAfter,
        AtkDefAfter,TargetDefAfter);
       }else{
           Debug.Log("target has dead ");
           new DelayCommand(0.4f).AddToQueue();
       }
    }
}
