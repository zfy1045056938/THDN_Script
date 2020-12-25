using UnityEngine;
using Mirror;
using System.IO;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Linq;
using Unity.Mathematics;
using TMPro;
using Cinemachine;
using System;
using Invector;
using Invector.vCamera;
using Invector.vMelee;
using Invector.vItemManager;
using Invector.vCharacterController;
//TODO combat state type
//thdn shield gp has anti_shield perc ,when cast the effect show state for anti_s
public enum CombatType
{
    NORMAL,
    CASTING,
    SHIELD,
    ANTI_SHIELD
}


[System.Serializable]
public class LinearInt{
    public int baseValue;
    public int bounsLevel;
   

    public int Get(int v)=>bounsLevel*(v-1)+baseValue;
}



[System.Serializable]
public class LinearFloat{
    public float baseValue;
    public LinearFloat (){}
    public LinearFloat(float f){
        this.baseValue=f;
    }
    public  float Get(float fs)
    {
        return fs;
    }

}

//Game Entity Base Classes Includes(For THDN has 3 Entities(Player,NPC,Monster))
//1.Stats 
//2.C/S Callback Handler Msg
//3.FSM handler
//4.extra for thdn ,player in dungeon needs interactive object,base on the entity und add extra tools
//for player 
//5.entity for all object who can interactive  with each other 
//6.TODO IF 

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(NetworkProximityChecker))]
public abstract class Entity : NetworkBehaviourNonAlloc
{
    //Delagate listen for update method 
   


    [Header("Component For Object")]
    public NavMeshAgent agent;
    public NetworkProximityChecker checker;
    public NetworkNavAgentBanding banding;
    public Animator animator;
    public AudioSource audioSource;
    public Collider collider;
    public NpcType npcType = NpcType.NORMAL;  //default for entity 
    [Header("Invector")]
    public vThirdPersonController thirdPersonController;
    public vThirdPersonInput thirdPersonInput;
    public vMeleeManager vMeleeManager;
   
    //Extra Common
    //public Outline objO/utline;

    [Header("State FSM")]
    [SyncVar, SerializeField] string _state = "IDLE";    //defaul state if move change state from entity
    public string state => _state;
    [SyncVar]
    public double lastCombatTime = 0.0;
    [SyncVar]
    public double lastStateTime = 0.0;

    //
    [SyncVar] public string conversationName;

    [Header("Component Stats")]
    //for component with basic stats (static )
    [SyncVar] GameObject _target;
    public Entity target {
        get { return _target != null ? _target.GetComponent<Entity>() : null; }
        set {
            _target = value != null ? value.gameObject : null;
        }
    }

    [SyncVar]
    public int level = 1;

    [SyncVar] public int skillPoints = 0;   //skill points
    [SyncVar] public int characterPoints = 0; //personal points 
   


    //[SerializeField] private LinearInt _str = new LinearInt { baseValue = 0 };
    //public virtual int str {
    //    //extra bouns from items or shrine

    //    get {
    //        int passiveBouns = 0;
    //        int buffBouns = 0;   //skills
    //        return _str.Get(level) + passiveBouns + buffBouns;
    //    }
    //}

    [SerializeField] protected LinearInt _aShield = new LinearInt { baseValue = 0 };
    public virtual int aShield {
        get {
            return _aShield.Get(level);
        }
    }

    //[SerializeField] protected LinearInt _dex = new LinearInt { baseValue = 0 };
    //public virtual int dex
    //{
    //    //extra bouns from items or shrine

    //    get
    //    {
    //        int passiveBouns = 0;
    //        int buffBouns = 0;   //skills
    //        return _dex.Get(level) + passiveBouns + buffBouns;
    //    }
    //}
    //[SerializeField] protected LinearInt _inte = new LinearInt { baseValue = 0 };
    //public virtual int inte
    //{
    //    //extra bouns from items or shrine

    //    get
    //    {
    //        int passiveBouns = 0;
    //        int buffBouns = 0;   //skills
    //        return _inte.Get(level) + passiveBouns + buffBouns;
    //    }
    //}
    //[SerializeField] protected LinearInt _con = new LinearInt { baseValue = 0 };
    //public virtual int con
    //{
    //    //extra bouns from items or shrine

    //    get
    //    {
    //        int passiveBouns = 0;
    //        int buffBouns = 0;   //skills
    //        return _con.Get(level) + passiveBouns + buffBouns;
    //    }
    //}

    #region personal skill
    //TODO Set when use pp when reward or upgrade 
    [SerializeField] protected int _lockpick ;
    public virtual int lockpick {
        //extra bouns from items or shrine

        get {
            int passiveBouns = 0;
            int buffBouns = 0;   //skills
            return _lockpick + passiveBouns + buffBouns;
        }

        set
        {
            _lockpick = value;
        }


    }
    [SerializeField] protected int _science;
    public virtual int science {
        //extra bouns from items or shrine
        get {
            int passiveBouns = 0;
            int buffBouns = 0;   //skills
            return _science + passiveBouns + buffBouns;
        }
        set
        {
            _science = value;
        }
    }

    //living skills

    [SerializeField] protected int _leader ;
    public virtual int leader {
        //extra bouns from items or shrine
        get {
            int passiveBouns = 0;
            int buffBouns = 0;   //skills
            return _leader + passiveBouns + buffBouns;
        }
        set
        {
            _leader = value;

        }
    }
    [SerializeField] protected int _kissass;
    public virtual int kissass
    {
        //extra bouns from items or shrine
        get
        {
            int passiveBouns = 0;
            int buffBouns = 0;   //skills
            return _kissass + passiveBouns + buffBouns;
        }
        set
        {
            _kissass = value;

        }
    }
    //[SerializeField] protected LinearFloat _rage = new LinearFloat { baseValue = 0f };
    //public virtual int rage {
    //    //extra bouns from items or shrine
    //    get {
    //        int passiveBouns = 0;
    //        int buffBouns = 0;   //skills
    //        return _leader.Get(level) + passiveBouns + buffBouns;
    //    }
    //}


    [SerializeField] protected float _heavy =100.0f;
    public virtual float heavy {
        //extra bouns from items or shrine

        get {

            return _heavy;
        }
        set
        {
            _heavy = value;
        }
    }






    //player basic in entity are player health + skillability+powerBouns
    //in player have weapon und  equipment bouns then add to the entity values

    [SerializeField] protected int _dungeoneering ;
    public virtual int dungeoneering {
        get {
            //Basic + skillBouns + initExtraAbility +(player)equipmentBouns+ (special)itemBouns
            int passiveBouns = 0;
            //skill bouns

            //buff bouns
            int buffBouns = 0;

            return _dungeoneering+ passiveBouns + buffBouns;

        }
        set
        {
            _dungeoneering = value;
        }
    }

    #endregion

    [SerializeField] protected LinearFloat _manaMax = new LinearFloat { baseValue = 10f };
    public virtual float manaMax {
        get {
            int passiveBouns = 0;
            //
            int buffBouns = 0;
            return _manaMax.Get(level) + passiveBouns + buffBouns;
        }
        set { value = _manaMax.Get(manaMax); }
    }
    public float manaRate { get; set; }
    

    [SerializeField] protected float _cstaminaMax =100.0f;
    public virtual float Stamina
    {
        get
        {
            int passiveBouns = 0;
            //
            int buffBouns = 0;
            return _cstaminaMax + passiveBouns + buffBouns;
        }
        set { value = _cstaminaMax; }
    }
    public float staminaRate { get; set; }

    public bool canhpRate { get; set; }


    public bool canmanaRate { get; set; }


    public bool canstmainaRate { get; set; }
    [SerializeField] protected LinearFloat _damage = new LinearFloat { baseValue = 0f };
    public virtual float damage {
        get {
            int buffBouns = 0;
            int skillBouns = 0;
            return _damage.Get(level) + buffBouns + skillBouns;
        }


    }

    [SerializeField] LinearInt _armor = new LinearInt { baseValue = 10 };
    public virtual int armor {
        get {
            int bufferBouns = 0;
            int skillBouns = 0;
            return _armor.Get(level) + bufferBouns + skillBouns;
        }

    }

    [SerializeField] LinearFloat _crit = new LinearFloat { baseValue = 0.0f };
    public virtual float crit {
        get {
            return _crit.Get(level) + crit;
        }
    }

    [SerializeField] LinearFloat _speed = new LinearFloat { baseValue = 5 };

    public virtual float speed
    {
        get { return _speed.Get(level); }
    }

    //block needs equipment required that has percent by block(0.0~0.5)
    [SerializeField] LinearFloat _blockChance = new LinearFloat { baseValue = 0.0f };
    public virtual float blockChance {
        get {
            int buffBouns = 0;
            int skillBouns = 0;
            return _blockChance.Get(level) + skillBouns + buffBouns;
        }
    }

    [SerializeField] float _health = 0f;

    public float health
    {
        get { return _health; }
        set { _health = math.clamp(value, 0, healthMax); }
    }

    [SerializeField] protected LinearInt _healthMax = new LinearInt { baseValue = 100 };
    public virtual int healthMax {
        get {
            
            int passiveBouns = 0;
            return _healthMax.Get(level) + passiveBouns;
        }

    }

    [SerializeField] float _healthRate = 3;

    public float healthRate {
        get { return _healthRate; }
        set { _healthRate = value; }
    }


  



    [Header("COMMON")]
    [SyncVar, SerializeField] int _gold = 0;
    public int gold { get { return _gold; } set { _gold = math.max(value, 0); } }

    protected int Level { get => level; set => level = value; }

    #region model data
    //cst = Players.SprintStamina,
    //        cws = Players.WalkSpeed,
    //        crs = Players.rollSpeed,
    //        crd = Players.rollDistance,
    //        cjs = Players.JumpSpeed,
    //        cls = Players.LadderStamina,
    public virtual float SprintStamina { get; set; }
    public virtual float WalkSpeed { get; set; }
    public virtual float RollSpeed { get; set; }
    public virtual float RollDistance { get; set; }
    public virtual float JumpSpeed { get; set; }
    public virtual float LadderStamina { get; set; }




    #endregion

    [HideInInspector] public bool invincibsle = false;
    [Header("TextUI")]
    public GameObject damagePopupPrefab;
    public GameObject stunnedOverlay;

    protected double stunTimed;
    public virtual EntityAnimState entityState { get; set; }
    [HideInInspector] public bool inSafeZone;    //Save house

    [Header("Other")]

    [HideInInspector] public float interactiveRange = 0.1f;
    [HideInInspector] public bool inDungeon = false;
    public CinemachineVirtualCamera faceCamera;

    //sync player inventory und equipment
    // public SyncItemSlot inventory = new SyncItemSlot();

    // public SyncItemSlot equipment = new SyncItemSlot();
    //sync list load from db -> party->skill->buffs
    public SyncListSkill skills = new SyncListSkill();  //update sync skill
    public SyncListBuff buffs = new SyncListBuff(); //update skill after load skill done
    // public SyncCharacter party = new SyncCharacter();   //sync character 
   
    //colelct pools collect the gp at battlefield
    //public Dictionary<GamePiece ,int> collectPool; //collect gp who matches und set to the cp

    public ScriptableSkill[] skillTemplates;

    //For Dungeon event 
    //public SyncBuff buff = new SyncBuff();


    //Use battle when got ED from skills
    //TODO 128 buffs 
    public bool isFire = false;
    public bool isFreeze = false;
    public bool isElec = false;
    public bool isShenShan = false;
    public bool isPosion = false;
    public bool isFrighting = false;
    public bool isMatches = false;
    public bool hasElemental = false;

    

    //extra for entity

    public string religion;
    public Dice dice;

    public string covName;



    #region TESTING
    public bool  usecs { get; set; }
    
    #endregion


    public virtual void Awake() {


        Util.InvokeMany(typeof(Entity), this, "_Awake");
    }
    public virtual void Start()
    {
        if (!isClient) animator.enabled = false;

        Util.InvokeMany(typeof(Entity), this, "_Start");
    }

    //Update state by FSM
    public virtual void Update() {
        if (IsWorthUpdate())
        {
            agent.speed = speed;

            //
            if (isClient)
            {
                //Update
                UpdateClient();
            }

            if (isServer)
            {
                if (target != null && target.IsHidden()) target = null;
                _state = UpdateServer();
            }

        }
        //
        if (!isServerOnly) UpdateOverlays();
        Util.InvokeMany(typeof(Entity), this, "Update_");
    }
    public float Healthper()
    {
        return (health != 0 && healthMax != 0) ? (float)health / (float)healthMax : 0;
    }

    public virtual bool IsWorthUpdate()
    {
        return netIdentity.observers == null ||
               netIdentity.observers.Count > 0 ||
               IsHidden();
    }
    public bool IsHidden() => checker.forceHidden;

    public virtual void LateUpdate()
    {

    }

    #region Client
    public virtual void OnStartClient() { }
    protected abstract void UpdateClient();
    public abstract string UpdateClient_IDLE();
    #endregion

    #region Server
    public virtual void OnStartServer() {
       //
       InvokeRepeating(nameof(Recover),1,1);
       //
       if(health==0)_state="DEAD";
        //
        Util.InvokeMany(typeof(Entity), this, "OnStartServer_");
    }

    /// <summary>
    /// TODO 1128
    /// </summary>
    [Server]
    public string Recover()
    {
        if (enabled && health > 0)
        {
            if (canhpRate) health += healthRate;
            if (canmanaRate) manaMax += manaRate;
            if (canstmainaRate) Stamina += staminaRate;
        }
        return "";
    }

    //Abstract UpdateServer Event(FSM)
    public abstract string UpdateServer();
    public abstract string UpdateServer_IDLE();
    // public abstract string UpdateServer_Matches();          //TODO do nothing
    public abstract string UpdateServer_CASTING();  
    public abstract string UpdateServer_COMBAT();
    public abstract string UpdateServer_DEAD();
    // public abstract string UpdateServer_DIALOGUE();

    public abstract string UpdateServer_MOVING();


    //Object server Msg
    public abstract string UpdateServer_Sprint();
    public abstract string UpdateServer_ROLL();
    // public abstract string UpdateServer_INTERACTIVE();
    //public abstract string UpdateServer_INTERACTIVE();


    //FSM Event
    public abstract bool EventIdle();
    public abstract bool EventMoving();
    public abstract bool EventMoveEnd();
    //atk
    public abstract bool EventCombat();

    //skill
    public abstract bool EventMatches();
    public abstract bool EventStartCasting();
    public abstract bool EventFinishCasting();
    //if the skill in casttime use others skill , cause cancel cast sequence
    public abstract bool EventCancelCasting();
    //
    public abstract bool EventSkillRequest();
    public abstract bool EventSkillFinish();
      

    //motion
    public abstract bool EventDead();
    public abstract bool EventCamp();
    public abstract bool EventCraft();
    public abstract bool EventStartCraft();
    public abstract bool EventEndCraft();
    public abstract bool EventDialogue();


    public abstract bool EventTrade();
    public abstract bool EventStartTrade();
    public abstract bool EventEndTrade();

    //
    public virtual void OnDeath()
    {

        Util.InvokeMany(typeof(Entity), this, "OnDeath_");
    }

    #endregion




    protected virtual void UpdateOverlays() { }

    #region SkillModule
    [ClientRpc]
    public void RpcCastSkillStarted(Skill skill) {
        // validate: still alive?
        if (health > 0)
        {
            // call scriptableskill event
            skill.data.OnCastStarted(this);
        }
    }
    [ClientRpc]
    public void RpcCastSkillFinished(Skill skill) {
        // validate: still alive?
        if (health > 0)
        {
            // call scriptableskill event
            skill.data.OnCastFinished(this);

            // maybe some other component needs to know about it too
            SendMessage("OnSkillCastFinished", skill, SendMessageOptions.DontRequireReceiver);
        }

    }
    [ClientRpc]
    public virtual void RpcShowComboTip(int num){
        
    }

    public abstract void Warp(Vector3 pos);



    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="index"></param>
    //public void StartCastSkill(Skill index) {
    //    // start casting and set the casting end time cooldown
    //    index.castTimeEnd = NetworkTime.time + index.castTime;
        

    //    // save modifications
    //    skills[currentSkill] = index;

    //    // rpc for client sided effects
    //    // -> pass that skill because skillIndex might be reset in the mean
    //    //    time, we never know
    //    RpcCastSkillStarted(index);
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="skill"></param>
    //public void FinishCastSkill(Skill skill) {
    //    // * check if we can currently cast a skill (enough mana etc.)
    //    // * check if we can cast THAT skill on THAT target
    //    // note: we don't check the distance again. the skill will be cast even
    //    //   if the target walked a bit while we casted it (it's simply better
    //    //   gameplay and less frustrating)
    //    if (CheckSelf(skill) && CheckTarget(skill) && CheckBoard())

    //    {
    //        // let the skill template handle the action
    //        skill.Apply(this);

    //        // rpc for client sided effects
    //        // -> pass that skill because skillIndex might be reset in the mean
    //        //    time, we never know
    //        RpcCastSkillFinished(skill);

    //        // decrease mana in any case
    //        manaMax -= skill.manaCosts;

    //        // start the cooldown (and save it in the struct)
    //        skill.cooldownEnd = NetworkTime.time + skill.cooldown;

    //        // save any skill modifications in any case
    //        skills[currentSkill] = skill;
    //    }
    //    else
    //    {
    //        // not all requirements met. no need to cast the same skill again
    //        currentSkill = -1;
    //    }
    //}

    public bool CheckSelf(Skill entity) { return health > 0 && manaMax > 0; }
    public bool CheckTarget(Skill entity) { return target.health > 0; }
    public bool CheckBoard() { return GameManagers.instance.gameTime > 0f; }
    public bool CheckMatchesSkill(int num) { return true; }
    public void UseSkillAtCP(List<CollectionsPool> cp) { }

   


    #endregion



    #region Combat System (C/S)


    [SyncVar]
    public int matchesNum = -1;
    //combat system in THDN 
    //combat system at battlefield use GP und skill ,includes ways combat target
    //1.ATK GP , the normal attack happen when player matches und has choice for one of them(armor,atk), when player select one of them,then
    // got this operations by matches 
    //2 CP -> SKill Module
    //after the time over, collect the pieces u collect und cal the final damage to tatget
    //when health ==0 then DEAD win the battle show the console return the dungeon und keep explore
    //TOTALDAMAGE=PlayerDamaager{BASIC(playerBasic+skillBouns+buffBouns)+MatchesCollect(STR||DEX||INT||MUT||Mixed())+AVG(specialItem value)}
    //atk order : UpdateServer_CASTING-> TryUseSkill->startSkill->[RPC]castskillstart->startSkill->

    [Server]
    public virtual void DealDamageToTarget(Entity entity, float amount, float castTime = 0f, float lTime = 0f
            , DamageType type = DamageType.Normal, ElementDamageType det = ElementDamageType.None) {
        float damageDelt = 0;
        
        DamageType damageType = DamageType.Normal;
        //
        
            //3 state(block,normal atk,crit)
            if (UnityEngine.Random.value < blockChance) {
                //block by enemy damage ,got rage bouns und try get combo state
                //BBoouns :: got lots reward bouns to reward pool when cause block twice
                //
                damageType = DamageType.Block;

                //popup show block icon
                ShowDamagePoup(amount, type);
            } else {
                //deal damage
                //Damage   by  elemental as extra bouns 
                if (det != ElementDamageType.None)
                {

                GameDebug.Log(string.Format("Has Elemental Damage target{0} got the {1} effect",entity.name,det.ToString()));
                    // as specials buff to target und with
                    switch (det)
                    {
                        case ElementDamageType.Fire:
                            isFire = true;
                            break;
                        case ElementDamageType.Freeze:
                            isFreeze = true;
                            break;
                        case ElementDamageType.Posion:
                            isPosion = true;
                            break;
                        case ElementDamageType.Frighting:
                            isFrighting = true;
                            break;
                        case ElementDamageType.ShenShan:
                            isShenShan = true;
                            break;

                    }
                }

                damageDelt = Mathf.Max(amount - entity.armor, health);
                if (UnityEngine.Random.value > (1 - crit)) {
                    damageDelt *= 2;
                    damageType = DamageType.Crit;
                   
                }

            GameDebug.Log(string.Format("{0} deal {1} damage to {2}",name.ToString(),damageDelt.ToString(),entity.name.ToString()));
            }
            //attack state ,needs check damage type und have crit(?isCrit)
            if (entity is NPC npc)
            {
                //Damage To target
                npc.health -= amount;

            } else if (entity is Players p)
            {
                p.health -= amount;
                Debug.Log("player" + p.health);
            } else if (entity is Monster m)
            {
                //

                m.health -= damageDelt;
            }



            //

        
        //
        entity.OnAggro(this);
        //show pop at client
        entity.RpcOnDamageReceived(damageDelt, damageType);
        //cast time sync server time to client 
        lastCombatTime = NetworkTime.time;
        entity.lastCombatTime = NetworkTime.time;
        //system hooks
        Util.InvokeMany(typeof(Entity), this, "DealDamageAt", entity, amount);



    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cast"></param>
    /// <returns></returns>
    public bool CanAttack(Entity cast)
    {
        return isMatches && cast.health > 0 && cast.target.health > 0 && !isFreeze;
    }


    [ClientRpc]
    public void RpcOnDamageReceived(float damage, DamageType damageType) {
        //ShowDamagePop
        ShowDamagePoup(damage, damageType);
        //

        Util.InvokeMany(typeof(Entity), this, "OnDamageReceived_", damage, damageType);
    }


    /// <summary>
    /// when cast buff skill then active to target
    /// </summary>
    /// <param name="buff"></param>
    public void AddOrRefreshBuff(Buffs buff)
    {

    }

    #region CommandCombatModule
    [Command]
    public void CmdDamageToTarget(float health,int damage)
    {
        health -= damage;
        ShowDamagePoup(damage, DamageType.Normal);
    }
    [Command]
    public void CmdDamageToTarget(float health, int damage,ElementDamageType type)
    {

    }
    //[Command]
    //public void CmdDamageToTarget(float health, int damage)
    //{

    //}
    //[Command]
    //public void CmdDamageToTarget(float health, int damage)
    //{

    //}


    #endregion

    /// <summary>
    /// TODO DPop
    /// </summary>
    /// <param name="amout"></param>
    /// <param name="damageType"></param>
    [ClientRpc]
    public void ShowDamagePoup(float amout, DamageType damageType) {
        if (damagePopupPrefab != null) {
            Bounds bs = collider.bounds;
            Vector3 pos = new Vector3(bs.max.x, bs.max.y, bs.max.z);

            GameObject pop = Instantiate(damagePopupPrefab, pos, Quaternion.identity);
            switch (damageType)
            {
                case DamageType.Block:
                    pop.GetComponent<TextMeshProUGUI>().text = "Block";
                    break;
            }

        }
    }
    public virtual void OnAggro(Entity e) { }

    #region ENTITY MOTION

    //Match Module  Motion at dungeon needs check state for cast skill und use others event for story line(squence command)

    public bool IsMoving() => state == "MOVING";

    public bool IsDead() { return state == "DEAD" && entityState == EntityAnimState.DEAD; }
    public bool IsCasting() { return state == "CASTING" && entityState == EntityAnimState.CASTING; }
    public bool IsBattle() { return state == "COMBAT" && entityState == EntityAnimState.ATTACK; }

    ////////////////////////////////////////////
    ///UI Module
    /// Client(Offline):
    /// Server:
    /// 
    ////////////////////////////////////////////
    #endregion

    
  
  
  



    // helper function to remove 'n' items from the inventory
    // public bool InventoryRemove(ItemReference item, int amount)
    // {
    //     for (int i = 0; i < inventory.Count; ++i)
    //     {
    //         vItemSlot slot = inventory[i];
    //         // note: .Equals because name AND dynamic variables matter (petLevel etc.)
    //         if (slot.amount > 0 && slot.item.Equals(item))
    //         {
    //             // take as many as possible
    //             amount -= slot.DecreaseAmount(amount);
    //             inventory[i] = slot;

    //             // are we done?
    //             if (amount == 0) return true;
    //         }
    //     }

    //     // if we got here, then we didn't remove enough items
    //     return false;
    // }

    // // helper function to check if the inventory has space for 'n' items of type
    // // -> the easiest solution would be to check for enough free item slots
    // // -> it's better to try to add it onto existing stacks of the same type
    // //    first though
    // // -> it could easily take more than one slot too
    // // note: this checks for one item type once. we can't use this function to
    // //       check if we can add 10 potions and then 10 potions again (e.g. when
    // //       doing player to player trading), because it will be the same result
    // public bool InventoryCanAdd(ItemReference item, int amount)
    // {
    //     // go through each slot
    //     for (int i = 0; i < inventory.Count; ++i)
    //     {
    //         // empty? then subtract maxstack
    //         if (inventory[i].amount == 0)
    //             amount -= item.stackSize;
    //         // not empty. same type too? then subtract free amount (max-amount)
    //         // note: .Equals because name AND dynamic variables matter (petLevel etc.)
    //         else if (inventory[i].item.Equals(item))
    //             amount -= (inventory[i].item.stackSize - inventory[i].amount);

    //         // were we able to fit the whole amount already?
    //         if (amount <= 0) return true;
    //     }

    //     // if we got here than amount was never <= 0
    //     return false;
    // }

    // // helper function to put 'n' items of a type into the inventory, while
    // // trying to put them onto existing item stacks first
    // // -> this is better than always adding items to the first free slot
    // // -> function will only add them if there is enough space for all of them
    // public bool InventoryAdd(ItemReference item, int amount)
    // {
    //     // we only want to add them if there is enough space for all of them, so
    //     // let's double check
    //     if (InventoryCanAdd(item, amount))
    //     {
    //         // add to same item stacks first (if any)
    //         // (otherwise we add to first empty even if there is an existing
    //         //  stack afterwards)
    //         for (int i = 0; i < inventory.Count; ++i)
    //         {
    //             // not empty and same type? then add free amount (max-amount)
    //             // note: .Equals because name AND dynamic variables matter (petLevel etc.)
    //             if (inventory[i].amount > 0 && inventory[i].item.Equals(item))
    //             {
    //                 vItemSlot temp = inventory[i];
    //                 amount -= temp.IncreaseAmount(amount);
    //                 inventory[i] = temp;
    //             }

    //             // were we able to fit the whole amount already? then stop loop
    //             if (amount <= 0) return true;
    //         }

    //         // add to empty slots (if any)
    //         for (int i = 0; i < inventory.Count; ++i)
    //         {
    //             // empty? then fill slot with as many as possible
    //             if (inventory[i].amount == 0)
    //             {
    //                 int add = Mathf.Min(amount, item.itemAmount);
    //                 inventory[i] = new vItemSlot(item, add);
    //                 amount -= add;
    //             }

    //             // were we able to fit the whole amount already? then stop loop
    //             if (amount <= 0) return true;
    //         }
    //         // we should have been able to add all of them
    //         if (amount != 0) Debug.LogError("inventory add failed: " + item.name + " " + amount);
    //     }
    //     return false;
    // }

 

  

    #region SkillUI


    /// <summary>
    /// every level up got eine new points for upgrade , when Onlevelup foreach the skill
    /// und highlight the skill who can upgrade until the sp == 0
    /// </summary>
    /// <param name="skill"></param>
    [Client]
    public void TryUpgradeSkill(int index)
    {
        if (CheckPoints() && CheckState())
        {
            CmdUpgradeSkill(index
        );
        }
    }

    /// <summary>
    /// when skill level needs update skill by sdata-> next level
    /// skill data load indie list from class if s.name&&level==target 
    /// </summary>
    /// <param name="skill"></param>
    [Command]
    public virtual void CmdUpgradeSkill(int skill)
    {
        Skill currentSkill= skills[skill];
        if (currentSkill.level > 0)
        {
            //has learn
            currentSkill.level++;
            // currentSkill.level += OnSkillLevelUpgrade(currentSkill.level);
        }
        else
        {
            //
        }
        //
    }

    [Command]
    public virtual void CmdLearnSkill(int index)
    {
        Skill s = skills[index];
        if (s.data != null)
        {
            //
            s.hasLearn = true;
            //when first learn ist 1(++<=5)
            s.level = 1;
            //
            
            
        }
    }

    /// <summary>
    /// equip select slot when is null or change the slot item
    /// </summary>
    /// <param name="index"></param>
    [Command]
    public virtual void CmdEquipSKill(int index)
    {

    }
    [Command]
    public virtual void CmdUnEquipSkill(int index)
    {

    }




    public bool CheckPoints()
    {
        return skillPoints > 0 || characterPoints > 0;
    }

    
    #endregion


    #region DungeonModule
    /// <summary>
    /// Dungeon Module for player who explore at the dungeon und try interactive with the obj ,player at
    /// dungeon mainly do three things
    /// 1.Explore room und go next rooms : the whole room have range(9,36)size tile for explore by difficult(e,n,h,s), player at new dungeon
    /// 
    /// </summary>
    [Client]
    public void OnDungeon()
    {

        if (isServer)
        {
            //server for dungeon can party with fiends with eine dungeon (config by teamDungeon),declare array for storge entity who
            //GameManagers.instance.Players[]
        }
        else if (isClient) {
            //at client only one player at dungeon und
            //GameManagers.instance.Players[]

        }

        //Util.InvokeMany(typeof(Dungeon), this, "OnDungeon_");


    }

    //[Command]
    //    public void CmdNextRooms(DungeonRooms rooms)
    //    {
            
    //    }

    public bool CheckFinishRoom() { return GameManagers.instance.isFinalRoom == true && health > 0; }

    internal int GetBuffIndexByName(string buffName)
    {
        throw new NotImplementedException();
    }




    #endregion
}
