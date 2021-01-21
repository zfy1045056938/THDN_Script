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


// [System.Serializable]
// public class LinearInt{
//     public int baseValue;
//     public int bounsLevel;
   

//     public int Get(int v)=>bounsLevel*(v-1)+baseValue;
// }



// [System.Serializable]
// public class LinearFloat{
//     public float baseValue;
//     public LinearFloat (){}
//     public LinearFloat(float f){
//         this.baseValue=f;
//     }
//     public  float Get(float fs)
//     {
//         return fs;
//     }

// }

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
   public List<vItem> itemList;
    //Extra Common
    //public Outline objO/utline;

    [Header("State FSM")]
    [SyncVar, SerializeField] string _state = "IDLE";    //defaul state if move change state from entity
    public string state => _state;
    [SyncVar]
    public double lastCombatTime = 0.0;
    [SyncVar]
    public double lastStateTime = 0.0;
    public string characterFrom;
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
   


    [SerializeField] int _aShield =0;
    public virtual int aShield {
        get {
            return _aShield;
        }
        set{
            _aShield=value;
        }
    }

    

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

    #region Layer3 Stats Inflence by personal skill 
    [SerializeField] protected float _cAnti;
    public virtual float CAnti
    {
        //extra bouns from items or shrine
        get
        {
            int passiveBouns = 0;
            int buffBouns = 0;   //skills
            return _cAnti + passiveBouns + buffBouns;
        }
        set
        {
            _cAnti = value;

        }
    }
    [SerializeField] protected float _CEagleEye;
    public virtual float CEagleEye
    {
        //extra bouns from items or shrine
        get
        {
            int passiveBouns = 0;
            int buffBouns = 0;   //skills
            return _CEagleEye + passiveBouns + buffBouns;
        }
        set
        {
            _CEagleEye = value;

        }
    }
    [SerializeField] protected float _CCheat;
    public virtual float CCheat
    {
        //extra bouns from items or shrine
        get
        {
            int passiveBouns = 0;
            int buffBouns = 0;   //skills
            return _CCheat + passiveBouns + buffBouns;
        }
        set
        {
            _CCheat = value;

        }
    }
    [SerializeField] protected float _AntiAb;
    public virtual float CAntiAb
    {
        //extra bouns from items or shrine
        get
        {
            int passiveBouns = 0;
            int buffBouns = 0;   //skills
            return _AntiAb + passiveBouns + buffBouns;
        }
        set
        {
            _AntiAb = value;

        }
    }
    [SerializeField] protected float _CHardAss;
    public virtual float CHardAss
    {
        //extra bouns from items or shrine
        get
        {
            int passiveBouns = 0;
            int buffBouns = 0;   //skills
            return _CHardAss + passiveBouns + buffBouns;
        }
        set
        {
            _CHardAss = value;

        }
    }
    [SerializeField] protected float _CBusiness;
    public virtual float CBusiness
    {
        //extra bouns from items or shrine
        get
        {
            int passiveBouns = 0;
            int buffBouns = 0;   //skills
            return _CBusiness + passiveBouns + buffBouns;
        }
        set
        {
            _CBusiness = value;

        }
    }
    [SerializeField] protected float _CSoulView;
    public virtual float CSoulView
    {
        //extra bouns from items or shrine
        get
        {
            int passiveBouns = 0;
            int buffBouns = 0;   //skills
            return _CSoulView + passiveBouns + buffBouns;
        }
        set
        {
            _CSoulView = value;

        }
    }
    [SerializeField] protected float _CvRate;
    public virtual float CvRate
    {
        //extra bouns from items or shrine
        get
        {
            int passiveBouns = 0;
            int buffBouns = 0;   //skills
            return _CvRate + passiveBouns + buffBouns;
        }
        set
        {
            _CvRate = value;

        }
    }
    #endregion
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

    [SerializeField] protected int _manaMax =100;
    public virtual float manaMax {
        get {
            int passiveBouns = 0;
            //
            int buffBouns = 0;
            return _manaMax + passiveBouns + buffBouns;
        }
        set { value = _manaMax; }
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
    [SerializeField] protected int _damage = 0;
    public virtual int damage {
        get {
            int buffBouns = 0;
            int skillBouns = 0;
            return _damage + buffBouns + skillBouns;
        }

        set{
            _damage=value;
        }


    }

    [SerializeField] int _armor =3;
    public virtual int armor {
        get {
            int bufferBouns = 0;
            int skillBouns = 0;
            return _armor+ bufferBouns + skillBouns;
        }

        set{
            _armor=value;
        }

    }

    [SerializeField] float _crit =0.03f;
    public virtual float crit {
        get {
            return _crit + crit;
        }
    }

    [SerializeField] float _speed =0.5f;

    public virtual float speed
    {
        get { return _speed; }
    }

    //block needs equipment required that has percent by block(0.0~0.5)
    [SerializeField] float _blockChance =0f;
    public virtual float blockChance {
        get {
            int buffBouns = 0;
            int skillBouns = 0;
            return _blockChance + skillBouns + buffBouns;
        }
    }

    [SerializeField]  float _health = 100f;

    public virtual float health
    {
        get { return _health; }
        set
        {
            _health = math.clamp(value, 0, healthMax);
      
        }
    }

    [SerializeField] protected int _healthMax = 100;
    public virtual int healthMax {
        get {
            
            int passiveBouns = 0;
            return _healthMax + passiveBouns;
        }

        set{
            _healthMax=value;
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
    
    [SyncVar, SerializeField] int _dust = 0;
    public int dust { get { return _dust; } set { _dust = math.max(value, 0); } }
    protected int Level { get => level; set => level = value; }
   
    public int lootMoney = 0;
    public float lootExp = 0f;
    public int lootDust = 0;
    
    #region model data
    //cst = Players.SprintStamina,
    //        cws = Players.WalkSpeed,
    //        crs = Players.rollSpeed,
    //        crd = Players.rollDistance,
    //        cjs = Players.JumpSpeed,
    // //        cls = Players.LadderStamina,
    // public virtual float SprintStamina { get; set; }
    // public virtual float WalkSpeed { get; set; }
    // public virtual float RollSpeed { get; set; }
    // public virtual float RunningSpeed { get; set; }
    // public virtual float SprintSpeed { get; set; }
    
    // public virtual float RollDistance { get; set; }
    // public virtual float JumpSpeed { get; set; }
    // public virtual float LadderStamina { get; set; }




    #endregion

    [HideInInspector] public bool invincibsle = false;
    [Header("TextUI")]
    public GameObject damagePopupPrefab;
    public GameObject stunnedOverlay;


    [Header("Other")]

    [HideInInspector] public float interactiveRange = 0.1f;
    //limit for some module just can use non-dungeon
    [HideInInspector] public bool inDungeon = false;
    
    //sync player inventory und equipment
    // public SyncItemSlot inventory = new SyncItemSlot();

    // public SyncItemSlot equipment = new SyncItemSlot();
    //sync list load from db -> party->skill->buffs
    public SyncListSkill skills = new SyncListSkill();  //update sync skill
    public SyncListBuff buffs = new SyncListBuff(); //update skill after load skill done
    // public SyncCharacter party = new SyncCharacter();   //sync character 
    public SyncDungeon dungeon = new SyncDungeon();
   
    //colelct pools collect the gp at battlefield
    //public Dictionary<GamePiece ,int> collectPool; //collect gp who matches und set to the cp

   

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

    
 
    public string covName;

    
    public virtual void Awake() {


        Util.InvokeMany(typeof(Entity), this, "_Awake");
    }
    public virtual void Start()
    {
        // if (!isClient) animator.enabled = false;

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
  
    // public abstract string UpdateServer_INTERACTIVE();
    //public abstract string UpdateServer_INTERACTIVE();


    //FSM Event
  
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
  
    public abstract void Warp(Vector3 pos);




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
                // ShowDamagePoup(amount, type);
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

   


    [ClientRpc]
    public void RpcOnDamageReceived(float damage, DamageType damageType) {
        //ShowDamagePop
        ShowDamagePoup(damage, damageType);
        //

        Util.InvokeMany(typeof(Entity), this, "OnDamageReceived_", damage, damageType);
    }


   

    #region CommandCombatModule
    [Command]
    public void CmdDamageToTarget(float health,int damage)
    {
        health -= damage;
        ShowDamagePoup(damage, DamageType.Normal);
    }



    #endregion

    /// <summary>
    /// TODO DPop
    /// </summary>
    /// <param name="amout"></param>
    /// <param name="damageType"></param>
    [Client]
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


}
#endregion