using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDataEditor;


//default is kuno init when generate
//common is personal skill for player 
public enum SkillCharacter
{
    Common,
    Kuno,
    Zino,
    
}

public enum SkillType
{
    None,
    Personal,
    Battle,
    Items,
}
/// <summary>
/// TODO 1222
/// player skill have 3 type  for game obj
/// 1.battleskill
///   the battle use with enemy , cast to target or self .Player with the hats active 
/// 2.personal skill
/// 3. item skill (specials item||Hats)
/// //1.only sskill load from skill save in resources , player load by dict-> character.type ==skill.type
/// </summary>
public  partial class  ScriptableSkill:ScriptableObjectNonAlloc{


   public int sID;
	[Header("Check")]
    public bool canUse;		//enough skill requirement that can use
    public Sprite sSprite;
    public Texture2D spriteName;
    public bool canCalcel;	//	cancel skill when use 
    public bool hasInstant;	//different of the bar 
    public bool rageTime;	//when eageBouns equals 100.0 that active rageBouns
  


    [Header("Properties")]
    public float sAmount;
   
    //skill time
    public float castTime;
    public float cooldown;

    public float manaCost;

    public int requirementLevel;
   
    public float skillExp;
    public int requiredLevel;
    //
    public float requiredSkillExperience;
    public float sUseExp;   //every success add the skillexp
    
    public AudioClip castSound;
    public GameObject fbxObj;       //load ab,when cast instance und use in time
    public int maxLevel;
    public int level;
    //
    public bool hasLearn;
    public bool canSelfLevel;

    //
    public string SkillCharacter;
    public SkillType skillType = SkillType.None;
    public SkillCastType castType = SkillCastType.None;
    

    //Pre use skill
    public virtual bool CheckSelf(Entity entity,int skillLevel){
        return false;
    }
    public virtual bool CheckTarget(Entity entity){
        return false;
    }
    
    public virtual void OnCastFinished(Entity entity) { }
    public virtual void OnCastStarted(Entity entity) { }

   

    private static Dictionary <int, ScriptableSkill> cache;
    public float sCostRage;
    public float sCostMana;
    public float sEffectTime;
    public float sCoolDown;

    //
    public SkillTargetType sTarget;
    public SkillCastType sCastType;
    public ElementDamageType elementDamageType;
    // public SkillType skillType = SkillType.None;
    // //
    public string sDetail;
    public bool sHasElement;
    public SkillElementType sElementType;
    public bool sCanSelfLevel;
    public float sSpeed;


    /// <summary>
    /// load detail from dic when needs load skill 
    /// </summary>
    public static Dictionary<int,ScriptableSkill> dict{
    	get{
    		if(cache == null){
    			ScriptableSkill[]  skillr = Resources.LoadAll<ScriptableSkill>("");
    				//add skill dur from cache
    				List<string> dup = skillr.ToList().FindDup(skills => skills.name);
    				//
    				if(dup.Count==0){
    					cache = skillr.ToDictionary(skill =>skill.name.GetStableHashCode(),skill=>skill);
    					//
                    }else{
    					foreach(var v in dup){
    						Debug.LogError(v.ToString());
    					}

                    }
            }
    			 return cache;
            
        }

    }

    public float SCastTime { get; set; }

    //Common tooltip
    public virtual string Tooltip(string tooltip,bool isRequirement=false)
    {

        StringBuilder sb = new StringBuilder(tooltip);
       


        return sb.ToString();
    }


    //use Skill if enough requirement 
    public virtual float castTimeRemaining(){return 0.0f;}
    public virtual float cooldownRemaining(){return 0.0f;}
    public virtual void Apply(Entity entity,int level){}

   
 
}