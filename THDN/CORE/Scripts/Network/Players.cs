using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
//using Unity.Mathematics;
using UnityEngine.AI;
//using Cinemachine;
using UnityEngine.Diagnostics;
using DungeonArchitect;
using DungeonArchitect.Builders.Grid;
using DungeonArchitect.Builders.GridFlow;
using PixelCrushers.DialogueSystem;
using Cinemachine;
using UnityEngine.EventSystems;
using Invector.vItemManager;
using UnityEngine.Events;
using GameDataEditor;

//For Dialogue Add-on
public delegate bool DSValideInt(Players p, int x);
public delegate bool DSValidateItem(Players p, string itemName, int amount);

public class SyncDictionaryIntDouble : SyncDictionary<int, double> { }

#region RPG Base Classes
//////
///用于客户端调用[command]
//////

//装备槽位获取，用于装备和物体的装备变更
[SerializeField]
public partial struct EquipmentInfo
{
    public string requiredCategory;
    public Transform location;
    public ScriptObjectItemUndAmount defaultItem;
}

[SerializeField]
public partial struct SkillbarEntry
{
    public string reference;
    public KeyCode hotKey;
    public bool canEquip;
}
#endregion


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NetworkName))]
[RequireComponent(typeof(NetworkNavAgentBanding))]
public class Players : Entity
{
    //TODO 127 add event in players for command invoke events
    #region EVENTHANDLER

    //Item Handler

    public UnityEvent onEnable;
    public UnityEvent onDisable;
    public UnityEvent onClick;
    //Skill Module
    public UnityEvent onLevelSkill;
    public UnityEvent onSelectSkill;

    //character points
    public UnityEvent onAddedPoints;
    public UnityEvent onDeincreasePoints;
    //




    #endregion
    public static Players localPlayer;
    [Header("Component")]

    public Camera avatarCamera;
    public NetworkNavAgentBanding moveMent;

    public string racename;
    [Header("Text Mesh")]


    [Header("Icons")]
    public Sprite portrainIcon;

    [HideInInspector] public string account = "";
    [HideInInspector] public string className = "";

    //Sync List for dy data
    // at town default true 
    [Header("Skillbar")]
    public SkillbarEntry[] skillbar = {
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha1,canEquip=true},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha2,canEquip=true},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha3,canEquip=true},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha4,canEquip=true},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha5,canEquip=true},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha6,canEquip=true},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha7,canEquip=true},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha8,canEquip=true},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha9,canEquip=true},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha0,canEquip=true},
    };


    #region character stats



    public override int healthMax
    {
        get
        {
            // int equipBouns = 0;
            // foreach (vItemSlot slot in equipment)
            //     if (slot.amount > 0)
            //         equipBouns += ((EquipmentItem)slot.item.data).healthBouns;
            return base.healthMax ;

        }
    }

  


    //
    //[SyncVar]
    //public string races;
    //[SyncVar]
    //public int str;
    //[SyncVar]
    //public int dex;
    //[SyncVar]
    //public int inte;
    //[SyncVar]
    //public int con;
    //[SyncVar]
    //public int cha;


    ////
    //[SyncVar]
    //public int lockpick;
    //[SyncVar]
    //public int science;
    //[SyncVar]
    //public int leader;
    //[SyncVar]
    //public int rage;


   
    //[SyncVar]
    //public int kissassLevel;
    //[SyncVar]
    //public int lockpickingLevel;
    //[SyncVar]
    //public int scienceLevel;
    //[SyncVar]
    //public int dungeoneeringLevel;
    //[SyncVar]
    //public int leaderLevel;

    ////
    [SerializeField]
    public float critPerc { get; set; }

    [SerializeField]
    public float flashPerc { get; set; }

    [SerializeField]
    public float blockPerc { get; set; }


    //Team manager player at dungeon can party with 2 characters

    [Header("Team Info ")]
    public int maxCharacter = 2;

    public int teamNum;

    //TODO Trace team guy
    public List<Entity> partyGuy;   // save from dungeon 


    public string bossName;


    


    //TODO
    public string lastSceneName { get; set; }


    public override float manaMax { get; set; }

    public override int damage
    {
        get
        {
            int equipBouns = 0;
            // foreach (vItemSlot slot in equipment)
            // {
            //     if (slot.amount > 0)
            //     {
            //         equipBouns += ((EquipmentItem)slot.item.data).damageBouns;
            //     }
            // }
            return base.damage + equipBouns;
        }
    }

    private float _absorbValue { get; set; }

    public float absorbValue
    {
        get
        {
            //Equipment
            int equipBouns = 0;
            // foreach (vItemSlot slot in equipment)
            // {
            //     if (slot.amount > 0)
            //     {
            //         equipBouns += ((EquipmentItem)slot.item.data).damageBouns;
            //     }
            // }
            //Buffs 
            int buffBouns = 0;

            return _absorbValue + equipBouns + buffBouns;
        }
    }



    public override int armor
    {
        get
        {
            int equipBouns = 0;
            // foreach (vItemSlot slot in equipment)
            //     if (slot.amount > 0)
            //     {
            //         equipBouns += ((EquipmentItem)slot.item.data).armorBouns;
            //         return base.armor + equipBouns;
            //     }

            return base.armor;
        }
    }

    public override float speed { get; }

    [SerializeField]
    public override float blockChance
    {
        get
        {
            float equipBouns = 0f;
            // foreach (vItemSlot slot in equipment)
            //     if (slot.amount > 0)
            //     {
            //         equipBouns += ((EquipmentItem)slot.item.data).BlockPer;
            //         //
            //         return base.blockChance + equipBouns;
            //     }
            return base.blockChance;
        }
    } //
    [SerializeField]
    public override float crit
    {
        get
        {
            float equipBouns = 0f;
            // foreach (vItemSlot slot in equipment)
            //     if (slot.amount > 0)
            //     {
            //         equipBouns += ((EquipmentItem)slot.item.data).CriPer;
            //         //
            //         return base.crit + equipBouns;
            //     }
            return base.crit;
        }
    } //

    [SyncVar] private float _mana;

    public float mana
    {
        get { return Mathf.Min(_mana, manaMax); }
        set { mana = Mathf.Clamp(_mana, 0, manaMax); }
    }

   


    [SyncVar]
    public string dialogueData;

    [SyncVar, SerializeField] public float _exp = 0;

    [SerializeField] protected long _expMax;
    public long expMax { get { return _expMax; } }
    public float exp
    {
        get { return _exp; }
        set
        {
            _exp = value;


            //
            Util.InvokeMany(typeof(Players), this, "OnLevelUp_");
        }
    }



    [SyncVar, SerializeField] private float _skillExp = 0;

    public float skillExp
    {
        get
        {
            return _skillExp;

        }
        set { _skillExp = value; }
    }
#endregion
    //
    [Header("Inventory")]

    //TODO ChangeLogs_1115
    //Add tmp slot for player when load dungeon
    public int inventorySize = 30;   //Slot is 30
    public ScriptObjectItemUndAmount[] defaultItem;
    //TODO 127
    // public List<vItemSlot> tmpSlot; //
    // public vItemSlot currentSlot;   //when player select slot set current 



    //default equipment in thdn has 3 slot (weapon,armor,specials(can destory ))
    [Header("Equipment Info")]
    public EquipmentInfo[] equipmentInfos ={
            new EquipmentInfo{requiredCategory="Weapon",location=null,defaultItem=new ScriptObjectItemUndAmount()},
            new EquipmentInfo{requiredCategory="Armor",location=null,defaultItem=new ScriptObjectItemUndAmount()},
            new EquipmentInfo{requiredCategory="Specials",location=null,defaultItem=new ScriptObjectItemUndAmount()},
        };

    //Skill Hot key when learn new skill then add to new bar auto 




    public static Dictionary<string, Players> onlinePlayers = new Dictionary<string, Players>();

    [Header("Interaction")] public float interactionRange = 4;

    public KeyCode cancelActionKey = KeyCode.Escape;

    public bool localPlayerClickThrough = true;

    //last server time
    public double allowLogoutTime => lastCombatTime + ((NetworkTime.time));
    public double remainingLogoutTime => NetworkTime.time < allowLogoutTime ? (allowLogoutTime - NetworkTime.time) : 0;

    public override float heavy { get; set; }

  
    
    public override float Stamina { 



        get{
            int equipmentBouns=0;
            // foreach(vItemSlot slot in equipment){
            //     if(slot.amount >0 ){
            //         equipmentBouns += ((EquipmentItem)slot.item.data). 
            //     }
            // }
            return base.Stamina+equipmentBouns;
        }
     }
    // public override float SprintStamina { get => base.SprintStamina; set => base.SprintStamina = value; }
    // public override float WalkSpeed { get => base.WalkSpeed; set => base.WalkSpeed = value; }
    // public override float RollSpeed { get => base.RollSpeed; set => base.RollSpeed = value; }
    // public override float RollDistance { get => base.RollDistance; set => base.RollDistance = value; }
    // public override float JumpSpeed { get => base.JumpSpeed; set => base.JumpSpeed = value; }
    // public override float LadderStamina { get => base.LadderStamina; set => base.LadderStamina = value; }
    public int AC_KillMonster { get; internal set; }
    public int AC_ExploreDungeon { get; internal set; }
    public int AC_UseSkill { get; internal set; }
    public int AC_CriNum { get; internal set; }



    #region Personal Skill
    public override int lockpick
    {
        get;set;
    }

    public override int science
    {
        get;set;
    }

    public override int leader
    {
        get;set;
    }

    public override int dungeoneering
    {
        get;set;
    }

    public override int kissass
    {
        get; set;
    }
    #endregion  

    

    //TODO
    [SyncVar]
    public Classes classType = Classes.Normal;

    [HideInInspector]
    public float useSkillWhenClose = -1;

    [HideInInspector]

    public double nextRiskyActionTime = 1.0;

    //always circle
    [HideInInspector]
    public GameObject indicator;
    public GameObject indicatorPrefab;

    //[Header("Interactive with Movement")]
    // Camera  camera;

    //int CloserDistance;


    //Player bone use by change equipment(armor und weapon)
    Dictionary<string, Transform> skinBones = new Dictionary<string, Transform>();


    //For Lua 
    public DSValideInt dsAddPlayerGold = delegate { return true; };

    //
    SyncDictionaryIntDouble itemCooldown = new SyncDictionaryIntDouble();



    //TODO _128 check current model for set default character in team list
    public List<Entity> teamList;


    public string currentName ;  //for default team list

  
    public override void Awake()
    {
        base.Awake();

        localPlayer = this;

        //Get Mesh Renderer get the skin bone
        foreach (SkinnedMeshRenderer mesh in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            foreach (Transform bone in mesh.bones)
            {
                skinBones[bone.name] = bone;
            }
        }

        //
        Util.InvokeMany(typeof(Players), this, "Awake_");

    }
  

    #region Ds
    #region Init

    //for lua func
    public DSValideInt dsValidateAddMoney = delegate { return true; };

    public void OnStartLocalPlayer_DialogueSystem()
    {


        Debug.Log("player ds Add on");

        DialogueManager.ResetDatabase(DatabaseResetOptions.KeepAllLoaded);
        PersistentDataManager.ApplySaveData(dialogueData);
        var player = Players.localPlayer;
        if (player.portrainIcon != null && player && usePorIconForConvastions)
        {
            DialogueLua.SetActorField("Player", DialogueSystemFields.CurrentPortrait, player.portrainIcon.name);
        }

        StartCoroutine(UpdateQuestStateListenerAfterOneFrame());
    }

    public IEnumerator UpdateQuestStateListenerAfterOneFrame()
    {
        yield return null;
        foreach (var qs in FindObjectsOfType<QuestStateListener>())
        {
            qs.UpdateIndicator();
        }
    }

    #endregion

    #region Update DS on Server

    private Coroutine m_updateDSDataCor = null;

    public void UpdateDialogueSystemData()
    {
        Debug.Log("Update DS For Player");
        if (m_updateDSDataCor == null)
        {
            m_updateDSDataCor = StartCoroutine(UpdateDSDCoroutine());
        }
    }

    private IEnumerator UpdateDSDCoroutine()
    {
        yield return new WaitForEndOfFrame();
        m_updateDSDataCor = null;
        UpdateDSDN();

    }

    private void UpdateDSDN()
    {
        dialogueData = PersistentDataManager.GetSaveData();
        if (!isServer)
        {
            CmdUpdateDSDOnServer(dialogueData);
        }
    }

    [Command]
    private void CmdUpdateDSDOnServer(string data)
    {
        dialogueData = data;
    }



    #endregion
    private bool usePorIconForConvastions;




    #endregion

    #region  Client
    public override void OnStartClient()
    {
        base.OnStartClient();


        //Func with Equipment callback  Update Model
        // equipment.Callback += OnEquipmentChanged;

        // for (int i = 0; i < equipment.Count; ++i)
        // {
        //     RefreashLoc(i);
        // }
    }


    //更新客户端信息
    //state->invector input-> update input -> generic input -> animation handler
    [Client]
    protected override void UpdateClient()
    {
        //根据状态更新行为,分为移动状态,死亡状态，技能状态,攻击状态
        //Inclient
      
        Util.InvokeMany(typeof(Players), this, "UpdateClient_");
    }

    #endregion 

    

    #region  Server

    public override void OnStartServer()
    {
        base.OnStartServer();

        //Invoke Reapeating when start battle

        Util.InvokeMany(typeof(Players), this, "OnStartServer_");
    }



    #endregion

  



    //用于角色动作控制器，通过fsm获取动机并执行对应动作
    //在服务器中，获取报文并将请求发送给客户端
    //客户端接受报文并执行动机
    public override void LateUpdate()
    {
        base.LateUpdate();


        agent.nextPosition = transform.position;
        if (isClient)
        {
            // foreach (Animator anim in GetComponentsInChildren<Animator>())
            // {
            //     if (anim != null)
            //     {
            //         //IDLE -> Moving
            //         anim.SetBool("Moving",
            //             state == "Moving" && IsMoving());
            //         anim.SetBool("CASTING",
            //           state == "CASTING" && CheckBoard());
            //         anim.SetBool("MATCHES",
            //           state == "MATCHES" && CheckBoard());
            //         anim.SetBool("DEAD",
            //           state == "DEAD");


            //         //when player health <=0 then dead
            //         //                anim.SetBool("Dead",state=="Dead" && state=="Dead"&&IsDead() && entityState==EntityAnimState.DEAD);
            //         //                //Use SKill IN THDN THEER skill slot who can config in town, in dungeon player can't change skill .
            //         //                anim.SetBool("Casting",state=="Casting"&&IsCasting()&&entityState==EntityAnimState.CASTING);
            //         //                // anim.SetBool("")
            //     }
            // }
            // //SelectionHandling();
        }


        //hooks
        Util.InvokeMany(typeof(Players), this, "LateUpdate_");


    }


  
    protected override void UpdateOverlays()
    {
        base.UpdateOverlays();
    }






    //////For interactive for object that occur event (loot || business)
    #region Commandes

    [Command]
    public void CmdAddMoney_DS(int x)
    {
        if (!dsValidateAddMoney(this, x)) return;
        gold += x;
    }

    #endregion


    public float HealthPercent()
    {
        return (health != 0 && healthMax != 0) ? (float)health / (float)healthMax : 0;
    }

    public float MPPercent()
    {
        return (mana != 0 && manaMax != 0) ? (float)mana / (float)manaMax : 0;
    }


    //FSM Callback to server
    //for dungeon when p.target.health<=0 then animation.Play("DEAD")
    [Server]
    public override string UpdateServer()
    {
        // if (state == "IDLE") return UpdateServer_IDLE();

      
        // if (state == "CASTING") return UpdateServer_CASTING();

        // if (state == "COMBAT") return UpdateServer_COMBAT();
       
        // if (state == "MOVING") return UpdateServer_MOVING();
        // if (state == "SPRINT") return UpdateServer_Sprint();
        // if (state == "Roll") return UpdateServer_ROLL();
        // // if (state == "MATCHES") return UpdateServer_Matches();      // TODO do nothing 

        // //
        // // if (state == "TRADING") return UpdateServer_TRADING();
        // // if (state == "DIALOGUE") return UpdateServer_DIALOGUE();
        // if (state == "CRAFTING") return UpdateServer_CRAFTING();
        // if (state == "DEAD") return UpdateServer_DEAD();
        return "IDLE";
    }





    //when player equip new equipment at slot 
    //check has equipment in slot then replace (refreash)
    void ReplaceAllBone(SkinnedMeshRenderer skin)
    {
        Transform[] bones = skin.bones;  //get bone
        //
        for (int i = 0; i < bones.Length; i++)
        {
            string bn = bones[i].name;  //get bone name
            //contains key
            if (!skinBones.TryGetValue(bn, out bones[i]))
            {
                //
                Debug.Log(skin.name + "bone" + bones[i].name);
            }
            //change all bone(refreash)
            skin.bones = bones;
        }
    }



    #region Inventory
    //inventory module in thdn needs do these things
    //1.storge item in client, callback server
    //2.equip item to equipSlot
    //3.extra inventory slot if need
    //4.Trash,if player move item to trash then throw or relative throw to world
    //5.use item if player use item(?itemtype),check stacksize und show itemEffect(heal or else),
    //if stack ==0 then destory item
    //6.if loot item has undenifitied ,check inventory has scroll to check the item if not then can't open
    //relationship between animator und inventory 

    //player state  is IDLE
    bool InventoryAllowed()
    {
        return state == "Idle" ||
        state == "Moving" ||
        state == "Casting" ||
        state == "Trading";
    }


    //[Command]
    //public void CmdUseInventoryItem(int index)
    //{
    //    if (InventoryAllowed() && 0 <= index && Inventory.Count>0 && Inventory[index].amount > 0
    //        && Inventory[index].item.data is UsableItem usable)
    //    {
    //        if (usable.CanUse(this, index))
    //        {
    //            Item item = Inventory[index].item;
    //            usable.Use(this,index);
    //            RpcUseItem(item);

    //        }
    //    }
    //}

    /// <summary>
    /// TODO 127 use item for command
    /// Slot.btn -> Item -> itemType->effect ->(Server?) RPC 
    /// </summary>
    /// <param name="item"></param>
    // [Command]
    // public void CmdUseItem(vItem item)
    // {
    //     if (item != null && currentSlot!=null)
    //     {

    //         onAddItem.Invoke(item);
    //         //Update slot
    //         currentSlot.UpdateDisplays();
            
    //     }
    // }

   

    public float GetItemCoolDown(string itemcooldown)
    {
        int hash = itemcooldown.GetStableHashCode();
        //
        if (itemCooldown.TryGetValue(hash, out double coolDownEnd))
        {
            return NetworkTime.time >= coolDownEnd ? 0 : (float)(coolDownEnd - NetworkTime.time);

        }

        return 0;
    }

    public void SetItemCoolDown(string coolDownCategory, float cooldown)
    {
        int hash = coolDownCategory.GetStableHashCode();
        //
        itemCooldown[hash] = NetworkTime.time + cooldown;
    }

   

    #endregion


    public override void Start()
    {
        if (!isServer && !isClient) return;
        //
        base.Start();
        //get name to server player
        onlinePlayers[name] = this;

        //buff effect TODO
        //IN THDN BUFF ALWAYS HAPPEN IN SET OR EQUIPMENT EFFECT
        agent = GetComponent<NavMeshAgent>();
        // camera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<Camera>();
        //
        Util.InvokeMany(typeof(Players), this, "OnStart_");
    }


    //Get Player with netId und get player with camera
    public override void OnStartLocalPlayer()
    {
        // set singleton
        localPlayer = this;

        // find main camera
        // only for local player. 'Camera.main' is expensive (FindObjectWithTag)
        // faceCamera.Priority = 999;

        // setup camera targets
        //cam.GetComponent<CameraMMO>().target = transform;
        //  GameObject.FindWithTag("MinimapCamera").GetComponent<CopyPosition>().target = transform;
        //    if (avatarCamera) avatarCamera.enabled = true; // avatar camera for local player

        // load skillbar after player data was loaded,only at dungeon(valut)
        //Load when config dungeon
        LoadSkillbar();

        // addon system hooks
        Util.InvokeMany(typeof(Players), this, "OnStartLocalPlayer_");

        //
        Util.InvokeMany(typeof(Players), this, "OnStartLocalPlayer_", Camera.main);


    }



    /// <summary>
    /// 
    /// </summary>
    /// 
    [Client]
    public void LoadSkillbar()
    {
        print("loading skillbar for " + name);
        List<Skill> learned = skills.Where(skill => skill.level > 0).ToList();
        for (int i = 0; i < skillbar.Length; ++i)
        {
            // try loading an existing entry
            if (PlayerPrefs.HasKey(name + "_skillbar_" + i))
            {
                string entry = PlayerPrefs.GetString(name + "_skillbar_" + i, "");

                // is this a valid item/equipment/learned skill?
                // (might be an old character's playerprefs)
                // => only allow learned skills (in case it's an old character's
                //    skill that we also have, but haven't learned yet)
                // if (
                //     GetInventoryIndexByname(entry) != -1 ||
                //     GetEquipmentIndexByName(entry) != -1)
                // {
                //     skillbar[i].reference = entry;
                // }
            }
            // otherwise fill with default skills for a better first impression
            else if (i < learned.Count)
            {
                skillbar[i].reference = learned[i].name;
            }
        }
    }

    //public bool HasLearnedSkill(s)



    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
        return base.OnSerialize(writer, initialState);
        //writer.WriteInt32(health);

        //writer.WriteVector3(new Vector3(transform.position.x,transform.position.y,transform.position.z));
    }




  
    #region Skill Module 

    //Check Skill can use includes these requirement
    //1.CP enough to cast it ,highlight when enough
    //2.check has limit or some buff unlock the skil state util canUse
    //3.mana enough or sequence command ist not fully ,what's skill sequence?
    //when player cast GP to target, must have castTime for every skill sequence that needs command
    //the basic skill order ist : cast skill -> cast time->fbx&sounds -> target effect -> ends cast->FBXover->Next skill
    //at battle ,skill config by indie panel und 



    //public Queue<Skill> skillQueue = new Queue<Skill>();
    //public bool queuefull = false;   //check is full


    [Command]
    private void CmdUseSkill(int index)
    {
        //Check State
        if (state == "IDLE" && state == "CASTING")
        {
            //Can ready cast

        }
        //cast indexed
        // cast by skill index 
    }

    [Client]
    public void OnSkillCastFinished(Skill skill) { }

    [Client]
    public void TryUseSkill(int index, bool ignoreSkill)
    {

    }

    //For living skill when event needs check living skill to 
    //check success perc by living level when success 
    public bool CheckLivingSkill(int index, int level, float perc)
    {
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="skill"></param>
    [Command]
    public  void CmdUpgradeSkill(int skill)
    {
        Skill s = skills[skill];

       if(s.data!=null) {

            if(s.maxLevel<5 && s.level > 0)
            {
                //upgrade und increse by level
                s.level++;
                IncreaseByLevel(s,s.level);
            }

        }


    }

    public int currentSkillBar = -1;
    public bool fullSkillBar = false;

    //SKill Bar
    //skill bar have 4 type skill for player who save for battle,includes 
    //1.character skill player can take 5 skill for game und can change at camp(what's camp?)
    //2.common skill
    //3.party skill
    //4.rage skill

    #endregion

    /// <summary>
    /// skill data will upgrade by level , sometime player at dungeon got
    /// effect for indie skill to change the stats , the rtv will storge und update
    /// current the variable , the totalv ===> base.value.GetLevel(int)+ equipment.v + buffs.v + shrine.v
    /// </summary>
    /// <param name="s"></param>
    /// <param name="level"></param>
    public void IncreaseByLevel(Skill s , int level) {
        if (s.data != null)
        {
             List<GDESkillDataData> recordSkill = new List<GDESkillDataData>();
            //upgrade rtskill data for db player save current state
            //when update reload the whole current data
           List<GDESkillDataData> skilldata = GDEDataManager.GetAllItems<GDESkillDataData>();
           //
           for(int i=0;i<skilldata.Count;i++){
               string csName= skilldata[i].SName;
               int indexof = csName.IndexOf("_");
               if(csName.Substring(indexof)== s.data.name){
                    //record the skill
                     recordSkill = new List<GDESkillDataData>();
                    recordSkill.Add(skilldata[i]);
               }
           }
           //Got Current All skill data then increase by level
           for(int j=0;j<recordSkill.Count;j++){
               if(recordSkill[j].SLevel == level){
                   //Update current skill data
                   s.amount =recordSkill[j].SAmount;
                   s.manaCosts =recordSkill[j].SCostMana;
                    s.cooldown=recordSkill[j].SCoolDown;
                    s.castTime = recordSkill[j].SEffectTime;
                    s.level = recordSkill[j].SLevel;
                    s.data.sDetail = recordSkill[j].SDetail;

               }
           }
        }
    }

    /// <summary>
    /// Update Movement For navmesh server update
    /// </summary>
    private void OnValidate()
    {
        Component[] cs = GetComponents<Component>();
        if (Array.IndexOf(cs, GetComponent<NetworkNavAgentBanding>()) > Array.IndexOf(cs, this))
            Debug.LogWarning(name + "is below player component");


        ////Equipment
        //for (int i = 0; i < equipmentInfos.Length; ++i)
        //{
        //    if (equipmentInfos[i].defaultItem.item != null && equipmentInfos[i].defaultItem.amount == 0)
        //        equipmentInfos[i].defaultItem.amount = i;
        //}

        ////default Item for newbee
        //for (int i = 0; i < defaultItem.Length; ++i)
        //{
        //    if (defaultItem[i].item != null && defaultItem[i].amount == 0)
        //    {
        //        defaultItem[i].amount = 1;
        //    }

        //}
    }

    #region  Selection Handling



    private void SetIndicatorViaPos(Vector3 bestDestination)
    {
        if (!indicator) indicator = Instantiate(indicatorPrefab);
        indicator.transform.parent = null;
        indicator.transform.position = bestDestination;
    }

    //target place needs show Circle 
    public void SetIndicator(Transform transform)
    {
        if (!indicator) indicator = Instantiate(indicatorPrefab);
        indicator.transform.SetParent(transform, true);
        indicator.transform.position = transform.position;
    }

    #endregion




    


    #region Reward Module

    public bool WinnerConsole(Entity target)
    {
        if (target.health <= 0 && health > 0)
        {
            //win
            return true;
        }
        else if (target.health > 0 && health <= 0)
        {
            //lose
            return false;
        }
        else if (target.health <= 0 && health <= 0)
        {
            //special Battle Event Check the winner
            //GameManagers.instance.SpecialMatches();
            return false;
        }
        return false;

    }

    [Command]
    public void CmdReward(float exp, int money, int dust)
    {
        exp += exp;
        gold += money;
        dust += dust;
    }
    [Command]
    public void CmdAddExp(float gexp){
        exp += gexp;
    }

    #endregion



   





    /// <summary>
    /// When level up got points for  players(skill,cp,ls) 
    /// </summary>
    /// <param name="level"></param>
    public void OnLevelUp_GetPoints(int level)
    {
        //Add points
        skillPoints += 1;
        characterPoints += 1;
       
        //Highligh the skill from player who achieve the target requirement




    }

    public void UpdateData(Skill index){

    }


   
//
    public override void Warp(Vector3 pos)
    {
        banding.RpcWarp(pos);
    }


//TODO GP
    #region GP
    //When active one of the shrine, set buff  to player the networktime counter when set to player
    //1.buff -> DB && Server.NetworkTime && Set Icon 
    // when networktime over destory the buff (DB && )
    // the relationship between de und buff 
   
    public void ActiveShrine(DungeonEvent de){
        Buffs buff = buffs[de.DeID];
        if(buff.data!=null){
            // bind de amount when blessing success, 
            // buff.data= de;
            buff.amount  =de.Amount;
            

            
        }

    }



//Got de by id
    [Command]
    public void CmdActiveDE(int index){
        DungeonEvent de = ItemDatabase.instance.GotDEByIndex(index);

        //Set to player 
        if(de!=null){
            switch(de.buffType){
                case BuffType.Atk:
                    damage += Mathf.FloorToInt( de.Amount);
                break;
            }
        }
        //

    }

    public override string UpdateClient_IDLE()
    {
        throw new NotImplementedException();
    }

    #endregion

  #region  Stats Module


    [Command]
    public void CmdHealthRecovery(float cHealth){
        health = cHealth;
    }

    [Command]
    public void CmdAddHealth(float cH){
        health += cH;
    }

    [Command]
    public void CmdDealDamage(float d){
        health -= d;
    }

    [Command]
    public void CmdUpdateStatmina(float cs){
        Stamina =cs;
    }





  #endregion
}
