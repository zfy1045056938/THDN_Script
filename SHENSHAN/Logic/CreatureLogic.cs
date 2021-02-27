using System.Collections.Generic;
using UnityEngine;
using System.Collections;

using DG.Tweening;
using Random = System.Random;

[System.Serializable]
public class CreatureLogic : ICharacter
{

    //player
    public Players owner;

    //
    public CardAsset card;

    //
    public CreatureEffect effect;


    public CardEffects cardEffects = CardEffects.None;

    //Effect counter  collect creature elemental effect state
    public int UniqueCreatureId;
    //private bool frozen = false;
    private bool taunt = false;
    //private bool hasHurtTaunt = false;
    private bool isFreeze = false;
    private bool hasAura = false;
    private bool hasBuff = false;
    private bool rage = false;

    //checking for det state
    private bool hasFire = false;
    private bool hasIce = false;
    private bool hasPosion = false;
    public int posPoint = 0;
    public int burningPoint = 0;
    private bool hasElec = false;
    private bool hasBloody = false;
    private bool machine = false;
    private bool sleep = false;


    //dungeon event;
    public bool dungeonEffect=false;
    
    
    public int ID
    {
        get { return UniqueCreatureId; }

    }

    public int hurtDef { get; set; }

    //生命值
    private int baseHealth;
    public int MaxHealth
    {
        get
        {
            return baseHealth;
        }
        set
        {
            if (value>0)
            {
                baseHealth = MaxHealth;
            }
            if (value <=0)
            {
                if(Machine==false){
                Die();
                }else{
                    Debug.Log("SLEEP MODE");
                   Sleep=true;
                }
                //DeadWish
                if (effect != null)
                {
                    effect.CauseEventEffect();
                }
            } else baseHealth = value;
        }
    }

    


    private int baseAtk;
    public int CreatureAtk
    {
        get { return baseAtk; }
        set { baseAtk = value; }
    }

    private int baseDef;
    public int CreatureDef
    {
        get { return baseDef; }
        set { baseDef = value; }
    }

        
    //Have Armoe 
    private bool hasarmor;

    public bool HasArmor
    {
        get;
        set;
    }

    public CardType c = CardType.None; //默认为随从

    

    public bool CanBeAtk
    {
        get
        {
            bool ownersTurn = (TurnManager.instance.WhoseTurn == owner);    //declare variable to playerTurn
            return (ownersTurn && (AttacksForThisTurn > 0) && IsFreeze==false);   //对当前回合以及攻击次数
        }
       
    }

    private int attacksForThisTurn=1;
    public int AttacksForThisTurn
    {
       get;
       set;
    }

    public int Str { get; set; }
    public int Dex { get; set; }
    public int Inte { get; set; }

    public int FR { get; set; }
    public int IR { get; set; }
    public int PR { get; set; }
    public int ER { get; set; }

     
    public bool Taunt { get => taunt; set => taunt = value; }
    
    public bool IsFreeze { get => isFreeze; set => isFreeze = value; }
    public bool HasAura { get => hasAura; set => hasAura = value; }
    public bool HasBuff { get => hasBuff; set => hasBuff = value; }
    public bool Rage { get => rage; set => rage = value; }
    public bool HasFire { get => hasFire; set => hasFire = value; }
    public bool HasIce { get => hasIce; set => hasIce = value; }
    public bool HasPosion { get => hasPosion; set => hasPosion = value; }
    public int PosPoint { get => posPoint; set => posPoint = value; }
    public bool HasElec { get => hasElec; set => hasElec = value; }
    public bool HasBloody { get => hasBloody; set => hasBloody = value; }
    public bool Machine { get => machine; set => machine = value; }
    public bool Sleep { get => sleep; set => sleep = value; }

    //For Dex


    //for inte
    public int magicExtra = 0;

    public int hurtTaunt = 0;
//    public int AtkthisTurn { get; private set; }


public CreatureLogic (){}

public delegate void VoidWithNoArugment();

public event VoidWithNoArugment CreatureAtkEvent;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CreatureLogic"/> class.
    /// </summary>
    /// <param name="owner">Owner.</param> 
    public CreatureLogic(Players owner, CardAsset ca)
    {
        //Add Charater Ab 
        //str  -> atk,hp 
        //dex  -> armor,flash(Application)
        //inte -> ED(players)
        // int healthBouns=0;
        // int atkBouns=0;
        // int armorBouns=0;
        
        this.owner = owner;
        this.card = ca;

        this.CreatureAtk = ca.cardAtk;
        this.CreatureDef = ca.cardDef;
        this.MaxHealth = ca.cardHealth;
       
        

        //
        FR = ca.fireResistance;
        IR = ca.iceResistance;
        PR = ca.poisonResistance;
        ER = ca.electronicResistance;
        
        // Str = ca.STR;
        // Dex = ca.DEX;
        // Inte = ca.INT;
        
       attacksForThisTurn =ca.atkForOneTurn;
       Debug.Log("Atk Turn"+AttacksForThisTurn);

        //GetUnique from  IDFactory
        UniqueCreatureId = IDFactory.GetUniqueID();
        //atkForOneTurn
        if(ca.charge){
            AttacksForThisTurn =attacksForThisTurn;
        }else{
            AttacksForThisTurn=0;
        }
        Taunt = ca.taunt;
            
        

//        //Show Taunt Icon
//        Sequence s = DOTween.Sequence();
//        if (Taunt ==true)
//        {
//            ca.cardTaunt.gameObject.SetActive(true);
//            s.OnComplete(() =>
//            {
//                s.Append(ca.cardTaunt.transform.DOScale(Vector3.one, 1.0f).SetEase(Ease.Linear));
//                s.PrependInterval(0.4f);
//            });
//        }
//        
    Debug.Log("Creature Logic Start Load ");
        //SPELL SCRIPT IF HAVE THEN ACTIVE 
        if (ca.creatureScriptName != null && card.creatureScriptName != ""&& card.disType==DiscoverType.None)
        {
            //Generate Skill of Spell For Creature Who has spell
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.creatureScriptName),
                new System.Object[] {owner, this, ca.specialCreatureAmount,ca.RoundTime,ca.spellBuffType,ca.damageEType}) as CreatureEffect;
            effect.RegisterEventEffect();
        }else if (ca.disType != DiscoverType.None )
        {
            Debug.Log("Discover Effect");
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.creatureScriptName),
                new System.Object[] {owner, this, ca.specialCreatureAmount,ca.disType}) as CreatureEffect;
        }

        creatureCreatedThisGame.Add(UniqueCreatureId, this);
    }

    //Creature Atk Behaviour

    /// <summary>
    /// Gos the face.
    /// </summary>
    public void GoFace()
    {
//        int attackAfter=0;
//        int targetHealAfter=0;
//        int creatureAtk=0;
//        int atkDef = 0;
//        int targetDef = 0;
        int defafter =0;
        int healthAfter = 0;
        int hurtTaunt=0;
        AttacksForThisTurn--; //此回合攻击次数为零

//if(owner.otherPlayer.Def==0){
//    defafter = owner.otherPlayer.Def - CreatureAtk;
//
//    if(defafter <=0){
//        owner.otherPlayer.MaxHealth += defafter;
//    }
//}
        //Check dex
        // float rnd = UnityEngine.Random.Range(0.0f, 1.0f); 
        // if (rnd > (1 - TurnManager.instance.WhoseTurn.otherPlayer.flashPerc))
        // {
        //     GlobalSetting.instance.SETLogs(string.Format("闪避判定为{0},躲闪成功",rnd));
        //     //NoAtk
        //     DamageEffect.ShowBuffEffect(GlobalSetting.instance.lowPlayer.playerArea.playerPortraitVisual.transform.position,SpellBuffType.Block,0);
        // }
        // else
        // {
            if (owner.otherPlayer.CreatureDef > 0)
            {
                defafter =owner.otherPlayer.CreatureDef- CreatureAtk;
                healthAfter = owner.otherPlayer.MaxHealth;
                //if less than 0
                if (defafter <= 0)
                {
                    healthAfter =owner.otherPlayer.MaxHealth+ defafter;
                    // defafter = 0;
                    //Check has braek effect or artifact effect
//                    if (owner.otherPlayer.hasBreakTaunt == true)
//                    {
//                        MaxHealth -= 1;
//                    }
defafter=0;
                }
            }
            else
            {
                healthAfter =owner.otherPlayer.MaxHealth- CreatureAtk;
            }
//        int atkHealAfter = owner.otherPlayer.MaxHealth - CreatureAtk;

            Debug.Log("GO FACE RESULT"+defafter+"\t"+healthAfter);
            owner.otherPlayer.CreatureDef = defafter;
            Debug.Log("Def After ist"+defafter);
            owner.otherPlayer.MaxHealth = healthAfter;

            //Check has hurtDEf
            // int hurtDef =0;
            // if(owner.otherPlayer.hasHurtDef==true){
            //     if(CreatureDef>0){
            //         CreatureDef-=owner.otherPlayer.hurtDef;
            //     }else {
            //     MaxHealth-=owner.otherPlayer.hurtDef;
            //     }
            //     hurtDef = MaxHealth;
            // }

            // if(owner.otherPlayer.hurtDef>0){
            //      new CreatureAttackCommand(owner.otherPlayer.playerID,
            //  UniqueCreatureId, CreatureAtk,
            //     hurtTaunt,
            //     hurtDef, healthAfter,
            //     CreatureDef,
            //    defafter ).AddToQueue();
            // }else{
            //Attack Command
            new CreatureAttackCommand(owner.otherPlayer.playerID,
             UniqueCreatureId, CreatureAtk,
                0,
                MaxHealth, healthAfter,
                CreatureDef,
               defafter ).AddToQueue();
            // }

            GlobalSetting.instance.SETLogs(string.Format("{0}对{1}造成发{2}点伤害",card.name,TurnManager.instance.WhoseTurn.otherPlayer.playerData.PlayerName.ToString(),CreatureAtk.ToString()));

        // }
    }


    //根据ID攻击指定对象
    public void AtkWithCreatureWithID(int UniqueCreatureId)
    {
        CreatureLogic target = CreatureLogic.creatureCreatedThisGame[UniqueCreatureId];
        AtkWithCreature(target);
    }

    /// <summary>
    /// Atks the with creature.
    /// </summay>
    /// <param name="target">Target.</param>
    private void AtkWithCreature(CreatureLogic target)
    {
        AttacksForThisTurn-=1;
        //after
        int targetHealthAfter=target.MaxHealth;      
        int targetDefAfter=target.CreatureDef ;
        int atkHealAfter=MaxHealth ;
        int atkdefAfter=CreatureDef ;
        
        if (CreatureAtkEvent != null)
        {
            CreatureAtkEvent.Invoke();
        }
        //Flash Check
        // float rnd = UnityEngine.Random.Range(0,1f);
        // if(rnd>(1-(TurnManager.instance.WhoseTurn.otherPlayer.flashPerc))){
        //      GlobalSetting.instance.SETLogs(string.Format("闪避判定为{0},躲闪成功",rnd));
        //     //NoAtk
        //     DamageEffect.ShowBuffEffect(GlobalSetting.instance.lowPlayer.playerArea.playerPortraitVisual.transform.position,SpellBuffType.Block,0);
        // }else{
        
        //存在护甲攻击力优先伤害护甲,如果护甲<伤害总值则减少卡牌血量
        if (target.CreatureDef > 0 && CreatureDef > 0)
        {
            Debug.Log("TC has def"+target.CreatureDef +"atk"+CreatureDef);

            targetDefAfter -= CreatureAtk;
            atkdefAfter -= target.CreatureAtk;
            
            // target.CreatureDef -= CreatureAtk;
            // CreatureDef -= target.CreatureAtk;
            //- data add to health 
            if (targetDefAfter <= 0)
            {
                targetHealthAfter += targetDefAfter;
                targetDefAfter = 0;
            }

            if (atkdefAfter <= 0)
            {
                atkHealAfter += atkdefAfter;
                atkdefAfter = 0;
            }
//            targetHealthAfter = target.MaxHealth;
//            atkHealAfter = MaxHealth;

            target.MaxHealth = targetHealthAfter;
            MaxHealth = atkHealAfter;
            //flow
            //def<0 -atk => health
//          if(targetDefAfter<0 && atkdefAfter >0){
//           
//            targetHealthAfter = target.MaxHealth+targetDefAfter;
        }
//        }else if (targetDefAfter > 0 && atkdefAfter == 0)
//        {
//
//            atkHealAfter = MaxHealth + atkdefAfter; //flow to health 
//        }
        // 
        else if(target.CreatureDef==0 && CreatureDef >0){
             Debug.Log("TC  def"+target.CreatureDef +"atk has def"+CreatureDef);
            atkdefAfter -= target.CreatureAtk;
            if (atkdefAfter < 0)
            {
                atkHealAfter += atkdefAfter;
                atkdefAfter = 0;
            }

            MaxHealth += atkdefAfter;
            targetHealthAfter -= CreatureAtk;
//            if (atkdefAfter < 0)
//            {
//                targetHealthAfter = target.MaxHealth - CreatureAtk;
//                atkHealAfter = MaxHealth + atkdefAfter;
//            }else{
//             // if target.atk= 2 selfdef=1 so selfhealth += (self+target.atk)
//             targetHealthAfter = target.MaxHealth - CreatureAtk; //self + -def(atk)
//             atkHealAfter = MaxHealth;
//            }
        }else if( target.CreatureDef>0 && CreatureDef ==0 ){
                Debug.Log("TC has def"+target.CreatureDef +"atk"+CreatureDef);
            //3-4 =-1 => health + defafter
             targetDefAfter -= CreatureAtk;
             if (targetDefAfter < 0)
             {
                 targetHealthAfter += targetDefAfter;
                 targetDefAfter=0;
             }
             
             target.MaxHealth = targetHealthAfter;
             atkHealAfter -= target.CreatureAtk;
             //when target def <=0 then remain add to health
//             if (targetDefAfter <= 0)
//             {
//                 targetHealthAfter = target.MaxHealth + targetDefAfter;
//                 atkHealAfter = MaxHealth - target.CreatureAtk;
//             }
//                else
//             {
//                 targetHealthAfter = target.MaxHealth;
//                 atkHealAfter = MaxHealth -target.CreatureAtk; //flow to health 
//             }
        }
        //双方无格挡值
        else
        {
            targetHealthAfter -= CreatureAtk;
            atkHealAfter -= target.CreatureAtk;
        }


        //命令指向目标
        //new CreatureCommand()
        new CreatureAttackCommand(target.UniqueCreatureId, UniqueCreatureId, CreatureAtk, 
            target.CreatureAtk, atkHealAfter, targetHealthAfter,atkdefAfter,targetDefAfter).AddToQueue();
        
        //target
        target.MaxHealth = targetHealthAfter;
        target.CreatureDef=targetDefAfter;
        //attacker
        MaxHealth = atkHealAfter;
        CreatureDef = atkdefAfter;

        GlobalSetting.instance.SETLogs(string.Format("{0}对{1}造成{1}点伤害",card.name,target.card.name,CreatureAtk));
        // }


        
        //cause
        if (effect!=null)
        {
            effect.WhenCreatureAtking();
        }

        //

    }
    
    
    /// <summary>
    /// Creature Die if Have DeadWish Then active
    /// </summary>
    public void Die()
    {

        owner.table.creatureOnTable.Remove(this);
//        owner.discardPool.cardList.Add(ID,card);
GlobalSetting.instance.SETLogs(string.Format("{0}失去所有生命值死亡",card.name));
        if (effect != null)
        {
            //Dead Wish
            effect.WhenACreatureDies();
            effect.UnRegisterEventEffect();
            effect = null;
        }
        
        new CreatureDieCommand(UniqueCreatureId,owner).AddToQueue();
        
    }

    public static Dictionary<int, CreatureLogic> creatureCreatedThisGame = new Dictionary<int, CreatureLogic>();
    public float hitPercent{
        get{
            return _hitPercent;
        }

        set{
            _hitPercent=value;
        }
    }
    public float _hitPercent;

    public void OnTurnStart()
    {

        if(Sleep==false || IsFreeze==false){
            
        AttacksForThisTurn = attacksForThisTurn;
        }else{
            AttacksForThisTurn=0;
        }
        

    }

  


   
}

