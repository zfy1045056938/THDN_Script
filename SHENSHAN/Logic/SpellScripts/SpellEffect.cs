using UnityEngine;
using System.Collections;

public  class SpellEffect
{
    
   

    public TargetOptions target;
    public Players owner;
    
    public int amount;
    public ArtifactType artType = ArtifactType.None;
    public CreatureLogic logic;

    public virtual void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
       
    }

    public virtual void ActiveEffectToTargetStat(int specialAmount=0, ICharacter target=null,SpellBuffType type=SpellBuffType.None)
    {
       
    }
    
    public virtual void ActiveEffectToTargetStat(int specialAmount=0, ICharacter target=null,DamageElementalType type = DamageElementalType.None)
    {
       
    }
    
    /// <summary>
    /// Artifact only for players influence to creatures or else
    /// </summary>
    /// <param name="specialAmount"></param>
    /// <param name="target"></param>
    /// <param name="type"></param>
    public virtual void ActiveEffectToTargetStat(int specialAmount=0, ICharacter target=null,ArtifactType type = ArtifactType.None)
    {
       
    }

    public virtual void ActiveRoundEffect(int amount = 0, ICharacter target = null, int roundTime = 0,
        DamageElementalType type = DamageElementalType.None)
    {
        
    }
    public virtual void CauseEventEffect(){}
  
  

    
    // METHODS FOR SPECIAL FX THAT LISTEN TO EVENTS
    public virtual void RegisterEventEffect(){}

    public virtual void UnRegisterEventEffect(){}


    
    
        
}
