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

    public override float damage
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
    public override float SprintStamina { get => base.SprintStamina; set => base.SprintStamina = value; }
    public override float WalkSpeed { get => base.WalkSpeed; set => base.WalkSpeed = value; }
    public override float RollSpeed { get => base.RollSpeed; set => base.RollSpeed = value; }
    public override float RollDistance { get => base.RollDistance; set => base.RollDistance = value; }
    public override float JumpSpeed { get => base.JumpSpeed; set => base.JumpSpeed = value; }
    public override float LadderStamina { get => base.LadderStamina; set => base.LadderStamina = value; }
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

    public override EntityAnimState entityState { get => base.entityState; set => base.entityState = value; }


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


    ////delegate TODO _CHANGELOG_1124
    //public delegate void onRoomChanged();

    //public event onRoomChanged OnRoomChange;

    //Sync list
    // public SyncList<Item> items;
    // public SyncList<Skill> skill;
    // public SyncList<Buffs> buffs;
    //
    //public SyncList<Dungeon> dungeons;

    //[HideInInspector]
    //private Board gameBoard;

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
        if (isLocalPlayer)
        {
            if (state == "IDLE")
            {
                if (isLocalPlayer)
                {
                    //Update By Moving
                    //SelectionHandling();
                    //
                    //WASDHandling();

                    //if (Input.GetKeyDown(cancelActionKey))
                    //{
                    //    agent.ResetPath();
                    //    CmdCancelAction();
                    //}
                    //SKill out of range
                }
            }else if (state == "Sprint")
            {
                //
            }
            else if (state == "Roll")
            {
                //
            }
            else if (state == "Interactive")
            {
                //
            }
            else if (state == "Sprint")
            {
                //
            }
            else if (state == "Casting")
            {

            }
            else if (state == "Dead")
            {

            }
            else if (state == "COMBAT")
            {

            }
            else if (state == "CAMP") { }


        }
        //hooks
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

  


    //FSM with player has these motion with state
    //1.IDLE
    //2.MOVING(S->E)
    //3.TRADE(S->E)
    //4.BATTLE(THDN S->E)
    //5.DIED
    //--------RPG---------------
    //6.CRAFT
    //7.LOOT FROM ENEMY
    //8.SKILLCASTING(S->E)
    // ->SKILLRequest
    // ->SkillStart
    // ->SkillFinish
    ///desc: after the cal player can use skill to enemy in runtime(Match Time)
    //
    ////////////////////////////



    //FSM SERVER

    //IDLE=>Only Check Dead 
    [Server]
    public override string UpdateServer_IDLE()
    {
        // events sorted by priority (e.g. target doesn't matter if we died)

        if (EventDied())
        {
            OnDeath();
            return "DEAD";
        }
        //At Dungeon move next room
        if (EventMoveStart())
        {
            //Start Explore NextRoom TODO
            //ChangeLog_1124
            // if(GlobalSetting.instance.atDungeon==true && health > 0)
            // {
            //     //ChangeRoom
            //     GlobalSetting.instance.MoveNextRooms();
            //     //reward for change room add health bouns
            //     //health += OnRoomChange;

            // }

            return "MOVING";
        }
        //Arrive New Room Got RoomsType
        if (EventMoveEnd())
        {
            //New Rooms
            return "IDLE";

        }

        //////
        //at board matches und use skill
        if (EventCombat())
        {
            return "COMBAT";
        }
        if (EventMatches())
        {
            return "MATCHES";

        }
        if (EventStartCasting())
        {
            //use skill
            return "CASTING";
        }
        if (EventCancelCasting())
        {
            return "MATCHES";
        }
        if (EventFinishCasting())
        {
            return "MATCHES";
        }
        //Camp
        if (EventCamp())
        {
            return "IDLE";
        }
        if (EventDialogue())
        {
            return "DIALOGUE";
        }

        if (EventStartCraft())
        {
            return "CRAFT";
        }
        if (EventEndCraft())
        {
            return "IDLE";
        }
        if (EventTrade())
        {
            return "IDLE";
        }
        if (EventStartTrade())
        {
            return "IDLE";
        }
        if (EventEndTrade())
        {
            return "IDLE";
        }


        return "IDLE"; // nothing interesting happened

    }



    [Server]
    public override string UpdateServer_MOVING()
    {

        if (EventIdle())
        {
            return "IDLE";
        }
        if (EventDied())
        {
            return "DEAD";
        }
        //At Dungeon move next room
        if (EventMoveStart())
        {
            return "MoVING";
        }
        if (EventMoveEnd())
        {
            //TODO 
            // if (GlobalSetting.instance.atDungeon == true && health > 0 && GlobalSetting.instance.isMove == false)
            // {
            //     //start re by room
            //     //GlobalSetting.instance.StartRoomEvent(GlobalSetting.instance.currentRoom);
            // }
            return "IDLE";

        }
        //at board matches und use skill
        if (EventCombat())
        {
            return "COMBAT";
        }
        if (EventMatches())
        {
            return "MATCHES";

        }
        if (EventStartCasting())
        {
            //use skill
            return "CASTING";
        }
        if (EventCancelCasting())
        {
            return "MATCHES";
        }
        if (EventFinishCasting())
        {
            return "MATCHES";
        }
        //Camp
        if (EventCamp())
        {
            return "IDLE";
        }
        if (EventDialogue())
        {
            return "DIALOGUE";
        }

        if (EventStartCraft())
        {
            return "CRAFT";
        }
        if (EventEndCraft())
        {
            return "IDLE";
        }
        if (EventTrade())
        {
            return "IDLE";
        }
        if (EventStartTrade())
        {
            return "IDLE";
        }
        if (EventEndTrade())
        {
            return "IDLE";
        }
        return "Moving";
    }




    [Server]
    public override string UpdateServer_CASTING()
    {
        if (EventIdle())
        {
            return "IDLE";
        }
        if (EventDied())
        {
            OnDeath();
            return "DEAD";
        }
        //At Dungeon move next room
        if (EventMoveStart())
        {
            return "MoVING";
        }
        if (EventMoveEnd())
        {
            return "IDLE";

        }
        //at board matches und use skill
        if (EventCombat())
        {
            return "COMBAT";
        }
        if (EventMatches())
        {
            return "MATCHES";

        }
        if (EventStartCasting())
        {
            //use skill
            return "CASTING";
        }
        if (EventCancelCasting())
        {
            return "MATCHES";
        }
        if (EventFinishCasting())
        {
      
            //

            return "MATCHES";
        }
        //Camp
        if (EventCamp())
        {
            return "IDLE";
        }
        if (EventDialogue())
        {
            return "DIALOGUE";
        }

        if (EventStartCraft())
        {
            return "CRAFT";
        }
        if (EventEndCraft())
        {
            return "IDLE";
        }
        if (EventTrade())
        {
            return "IDLE";
        }
        if (EventStartTrade())
        {
            return "IDLE";
        }
        if (EventEndTrade())
        {
            return "IDLE";
        }
        return "IDLE";
    }

    [Server]
    public override string UpdateServer_DEAD()
    {
        if (EventIdle())
        {
            return "IDLE";
        }
        if (EventDied())
        {
            return "DEAD";
        }
        //At Dungeon move next room
        if (EventMoveStart())
        {
            return "MoVING";
        }
        if (EventMoveEnd())
        {
            return "IDLE";

        }
        //at board matches und use skill
        if (EventCombat())
        {
            return "COMBAT";
        }
        if (EventMatches())
        {
            return "MATCHES";

        }
        if (EventStartCasting())
        {
            //use skill
            return "CASTING";
        }
        if (EventCancelCasting())
        {
            return "MATCHES";
        }
        if (EventFinishCasting())
        {
            return "MATCHES";
        }
        //Camp
        if (EventCamp())
        {
            return "IDLE";
        }
        if (EventDialogue())
        {
            return "DIALOGUE";
        }

        if (EventStartCraft())
        {
            return "CRAFT";
        }
        if (EventEndCraft())
        {
            return "IDLE";
        }
        if (EventTrade())
        {
            return "IDLE";
        }
        if (EventStartTrade())
        {
            return "IDLE";
        }
        if (EventEndTrade())
        {
            return "IDLE";
        }
        return "IDLE";
    }


    [Client]
    public override string UpdateClient_IDLE()
    {


        return "IDLE";
    }

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

    #region Motion


    #endregion


    #region Dungeon Module

    //At Dungeon needs check dungeon rooms state,when the isDone ==true,
    //unlock normal door connect next rooms except specials door(lockpick or specials skill)
    [Command]
    public void CmdChangeRoom(int currentId, int nextId)
    {
        
    }

    //when get reward by enemy , check chest type und open ,it must has unlock by perc, use dice to check likes
    //1d10,
    // at party(pvp),needs balance level between player level und check 
    [Command]
    public void CmdLootChest(int level, float perc)
    {


    }

    [Command]
    public void CmdCamp(int day) { }

    [Command]
    public void CmdRollDice(int num)
    {

    }


    #endregion

    protected override void UpdateOverlays()
    {
        base.UpdateOverlays();
    }




    #region Exp Module

    #endregion



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

    private string UpdateServer_CRAFTING()
    {
        if (EventIdle())
        {
            return "IDLE";
        }
        if (EventDied())
        {
            return "DEAD";
        }
        //At Dungeon move next room
        if (EventMoveStart())
        {
            return "MOVING";
        }
        if (EventMoveEnd())
        {
            return "IDLE";

        }
        //at board matches und use skill
        if (EventCombat())
        {
            return "COMBAT";
        }
        if (EventMatches())
        {
            return "MATCHES";

        }
        if (EventStartCasting())
        {
            //use skill
            return "CASTING";
        }
        if (EventCancelCasting())
        {
            return "MATCHES";
        }
        if (EventFinishCasting())
        {
            return "MATCHES";
        }
        //Camp
        if (EventCamp())
        {
            return "IDLE";
        }
        if (EventDialogue())
        {
            return "DIALOGUE";
        }

        if (EventStartCraft())
        {
            return "CRAFT";
        }
        if (EventEndCraft())
        {
            return "IDLE";
        }
        if (EventTrade())
        {
            return "IDLE";
        }
        if (EventStartTrade())
        {
            return "IDLE";
        }
        if (EventEndTrade())
        {
            return "IDLE";
        }

        return "CRAFT";
    }

  
   


    #region  Equipment Module
    /// <summary>
    /// 
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="index"></param>
    /// <param name="oldSlot"></param>
    /// <param name="newSlot"></param>
    //Func<T> with equipment info 
    // public void OnEquipmentChanged(vitems.Operation operation, int index, vItemSlot oldSlot, vItemSlot newSlot)
    // {
    //     RefreashLoc(index);
    // }

    //Get All Bone with player
    bool CanReplaceAllBones(SkinnedMeshRenderer skin)
    {
        foreach (Transform s in skin.bones)
        {
            if (s != null)
            {
                if (skinBones.ContainsKey(s.name))
                {
                    Debug.Log(s.name + "exists");
                }
            }
        }
        return true;
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



    //When Item Refreash 
    //refreash itemslot und equipment
    // public void RefreashLoc(int equipIndex)
    // {
    //     vItemSlot itemSlot = new vItemSlot();
    //     EquipmentInfo equipInfo = new EquipmentInfo();
    //     //
    //     if (equipInfo.requiredCategory != null && equipInfo.location != null)
    //     {
    //         //overwrite
    //         if (equipInfo.location.childCount > 0) Destroy(equipInfo.location.GetChild(0).gameObject);
    //         EquipmentItem item = (EquipmentItem)itemSlot.item.data;
    //         //has slot
    //         if (itemSlot.amount > 0)
    //         {
    //             //Load item prefab
    //             GameObject itemPrefab = Instantiate(item.modelPrefab, equipInfo.location, false);
    //             itemPrefab.name = item.modelPrefab.name;
    //             //
    //             SkinnedMeshRenderer skin = itemPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
    //             if (skin != null && CanReplaceAllBones(skin))
    //             {
    //                 ReplaceAllBone(skin);
    //             }

    //             //animator replace player damage style
    //             Animator animator = itemPrefab.GetComponent<Animator>();
    //             if (animator != null)
    //             {
    //                 animator.runtimeAnimatorController = animator.runtimeAnimatorController;

    //                 //restart all animators
    //                 RebindAnimators();
    //             }
    //         }

    //     }

    // }

    //refreash animator 
    void RebindAnimators()
    {
        foreach (Animator a in GetComponentsInChildren<Animator>())
        {
            a.Rebind();
        }
    }
    #endregion

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

    [Command]
    public void CmdStartDialogue(string covName)
    {
        if (state == "DIALOGUE" && covName != null)
        {
            DialogueManager.StartConversation(covName);
        }
        else
        {

        }

    }


    #region EVENT FOR FSM 
    bool EventDied()
    {
        // return target.health == 0;
        return health==0;
    }

    bool EventTargetDisappeared() { return target == null; }
    bool EventMoveStart() { return state != "Moving" && IsMoving(); }


    //target is  npc who can business or special
    bool EventTradeStart() { return state != "IDLE" && !CanTrade(); }

    private bool CanTrade()
    {
        throw new NotImplementedException();
    }

    bool EventTradeOver() { return state == "IDLE" && CanTrade(); }
    //Emeny in Dungeon or event in NPC
    bool EventBattle() { return state == "IDLE" && CanBattle(); }

    private bool CanBattle()
    {
        throw new NotImplementedException();
    }

    bool EventTalk() { return state == "IDLE" && CanTalk(); }

    private bool CanTalk()
    {
        throw new NotImplementedException();
    }


    #region Player Common Module
    //Movement ,player click target place then can move und show indic
    //if trigger on entity check that type und active
    //type==Merchant => business || quest accept || talking(dialogue system)
    //type==Enemy => battle && quest track 
    //typr==Normal=>InteractiveObject(Chest || EventPieces)
    //[Client]
    //void SelectionHandling()
    //{
    //    if (Input.GetMouseButtonDown(0) && Input.touchCount <= 1 && !Util.IsCursorOverUserInterface())
    //    {
    //        Debug.Log("Moveed");
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        Debug.Log("Ray Pos" + ray);
    //        //
    //        RaycastHit hit;

    //        bool cast = localPlayerClickThrough ? Physics.Raycast(ray, out hit, Mathf.Infinity) : Util.RaycastWithout(ray, out hit, target.gameObject);
    //        if (cast)
    //        {
    //            Entity entity = hit.transform.GetComponent<Entity>();
    //            //has target pos different to tags
    //            // npc(enemy || merchant || npc) start cov
    //            // resources(when select add to inventory (needs check enough slots))
    //            // at camp when slect
    //            // interactive obj (chest || torch || others) animation und  
    //            if (hit.transform.CompareTag("NPC") && entity != this)
    //            {

    //                //ouline show
    //                //entity.GetComponent<Outline>().LoadSmoothNormals();
    //                Debug.Log("Hit NPC");
    //                if (entity is NPC && entity.health > 0 &&
    //                    Util.CloserDistance(collider, entity.collider) <= interactionRange)
    //                {
    //                    //Get CheckDM
    //                    if (entity.conversationName != "" && entity.GetComponent<DialogueSystemTrigger>() != null)
    //                    {
    //                        localPlayer.target = entity;
    //                        Debug.Log("Target is" + localPlayer.target.name);

    //                        DialogueSystemTrigger dst = entity.GetComponent<DialogueSystemTrigger>();
    //                        foreach (var en in entity.GetComponents<DialogueSystemTrigger>())
    //                        {
    //                            if (en.trigger == DialogueSystemTriggerEvent.OnUse) dst = en;
    //                        }
    //                        dst.OnUse(localPlayer.transform);
    //                    }
    //                }

    //            }

    //            Vector3 bd = agent.NearestValidDestination(hit.point);

    //            agent.stoppingDistance = 0;
    //            agent.destination = bd;
    //        }
    //        else
    //        {
    //            localPlayer.target = null;
    //        }
    //    }

    //    if (Input.GetMouseButtonDown(1) && Input.touchCount <= 1 && !Util.IsCursorOverUserInterface())
    //    {
    //        Debug.Log("RightClick");
    //        //Right Click
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;
    //        bool cast = Physics.Raycast(ray, out hit, Mathf.Infinity);
    //        if (cast)
    //        {
    //            Entity entity = hit.transform.GetComponent<Entity>();
    //            if (hit.transform.CompareTag("NPC") && entity != this)
    //            {
    //                if (entity is NPC npc && entity.health > 0 &&
    //                    Util.CloserDistance(collider, entity.collider) <= interactionRange)
    //                {
    //                    Debug.Log("Show Merchant Panel");
    //                    var shop = FindObjectOfType<UITradePanel>();
    //                    var inv = FindObjectOfType<UIInventory>();
    //                    if (shop != null && inv != null)
    //                    {
    //                        //  shop.ShowShopPanel(npc);

    //                    }
    //                }
    //            }
    //        }

    //    }

    //}



    //[Client]
    //void WASDHandling()
    //{
    //    if (!Util.AnyInyActival())
    //    {
    //        float Horzonial = Input.GetAxis("Horizontal");
    //        float Vertial = Input.GetAxis("Vertical");

    //        //
    //        if (Horzonial != 0 || Vertial != 0)
    //        {
    //            //
    //            Vector3 input = new Vector3(Horzonial, 0, Vertial);
    //            if (input.magnitude > 1) input = input.normalized;
    //            //
    //            Vector3 angle = Camera.main.transform.rotation.eulerAngles;
    //            angle.x = 0;
    //            Quaternion rotation = Quaternion.Euler(angle);

    //            //
    //            Vector3 direction = rotation * input;
    //            //
    //            if (direction != Vector3.zero)
    //            {
    //                ClearUbducatorIfNotPar();
    //            }
    //            //
    //            agent.ResetMovement();
    //            //
    //            useSkillWhenClose = -1;
    //        }
    //    }
    //}
    #endregion
    public void ClearUbducatorIfNotPar()
    {
        if (indicator != null && indicator.transform.parent == null) Destroy(indicator);
    }

    //    [Client]
    //    void TargetNearest()
    //    {
    //        if (Input.GetKeyDown(targetNearestKey))
    //        {
    //            
    //        }
    //    }
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
            //upgrade rtskill data for db player save current state
            //when update reload the whole current data
            List<SkillData> sd = GdeManager.GetAll<SkillData>();
            if (sd.Count > 0)
            {

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




    #region CombatSystem
    //combat system in thdn needs rules these things
    //1.The damage event always show after the turn end 
    //2.System compare with the enemy speed to deside who go first(p.speed > entity.speed ? p.turn : entity.turn)
    //3.player combat to target cal by equipment ability und matches count(MatchCollect.value) then sum(p+v) to enemy 
    //4.entity -> player (health>0)=> command by entity ability
    //5.when entity(2) health<=0 ,show console panel check winorlose:win->return:gameover
    //6.about the exp manager,(solo or team(needs share))

    public void DamageTarget(Entity enemy, int amount)
    {
        if (enemy != null && enemy.health > 0 && CanAttack(enemy))
        {
            //damage
            DealDamageToTarget(enemy, amount);

        }
    }

    private bool CanAttack(Entity entity)
    {
        return (entity is NPC) ||
        (entity is Monster) ||
        (entity is Players);
    }

    [ClientRpc]
    public override void RpcShowComboTip(int num)
    {

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

    #endregion



    #region Loot Module
    [Command]
    public void CmdLootItem(int index)
    {
        //get item by index

    
    }

    [Command]
    public void CmdLootMoney(int money)
    {
        gold += money;
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


    ///////////////////////////////////////////////
    ////UpdateServer Module Update by Entity Animation(FSM)
    /// FSM includes clip for character when change animation
    /// IDLE
    /// MOVING
    /// DEAD
    /// DIALOGUE
    /// MATCHES(COMBAT,CASTING(START,END,CD,CANCEL),)
    /// BUSINESS
    /// CRAFT
    /// CAMP
    /// CASTING
    /// 
    ///////////////////////////////////////////////
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    // public override string UpdateServer_Matches()
    // {
    //     if (EventIdle())
    //     {
    //         return "IDLE";
    //     }
    //     if (EventDied())
    //     {
    //         return "DEAD";
    //     }
    //     //At Dungeon move next room
    //     if (EventMoveStart())
    //     {
    //         return "MOVING";
    //     }
    //     if (EventMoveEnd())
    //     {
    //         return "IDLE";

    //     }
    //     //at board matches und use skill
    //     if (EventCombat())
    //     {
    //         return "COMBAT";
    //     }
    //     if (EventMatches())
    //     {
    //         //Matches Module
    //         // CmdDragToTile(targetTile);

    //         return "MATCHES";

    //     }
    //     if (EventStartCasting())
    //     {
    //         //use skill
    //         return "CASTING";
    //     }
    //     if (EventCancelCasting())
    //     {
    //         return "MATCHES";
    //     }
    //     if (EventFinishCasting())
    //     {
    //         return "MATCHES";
    //     }
    //     //Camp
    //     if (EventCamp())
    //     {
    //         return "IDLE";
    //     }
    //     if (EventDialogue())
    //     {
    //         return "DIALOGUE";
    //     }

    //     if (EventStartCraft())
    //     {
    //         return "CRAFT";
    //     }
    //     if (EventEndCraft())
    //     {
    //         return "IDLE";
    //     }
    //     if (EventTrade())
    //     {
    //         return "IDLE";
    //     }
    //     if (EventStartTrade())
    //     {
    //         return "IDLE";
    //     }
    //     if (EventEndTrade())
    //     {
    //         return "IDLE";
    //     }

    //     return "MATCHES";
    // }

    public override string UpdateServer_COMBAT()
    {

        if (EventIdle())
        {
            return "IDLE";
        }
        if (EventDied())
        {
            return "DEAD";
        }
        //At Dungeon move next room
        if (EventMoveStart())
        {
            return "MoVING";
        }
        if (EventMoveEnd())
        {
            return "IDLE";

        }
        //at board matches und use skill
        if (EventCombat())
        {
            return "COMBAT";
        }
        ///
        if (EventMatches())
        {
            return "MATCHES";

        }
        if (EventStartCasting())
        {
            //use skill
            return "CASTING";
        }
        if (EventCancelCasting())
        {
            return "MATCHES";
        }
        if (EventFinishCasting())
        {
            return "MATCHES";
        }
        //Camp
        if (EventCamp())
        {
            return "IDLE";
        }
        if (EventDialogue())
        {
            return "DIALOGUE";
        }

        if (EventStartCraft())
        {
            return "CRAFT";
        }
        if (EventEndCraft())
        {
            return "IDLE";
        }
        if (EventTrade())
        {
            return "IDLE";
        }
        if (EventStartTrade())
        {
            return "IDLE";
        }
        if (EventEndTrade())
        {
            return "IDLE";
        }
        return "COMBAT";
    }


    #region Event Module
    public override bool EventIdle()
    {
        return !IsMoving();
    }

    /// <summary>
    /// At Dungeon , the doors r open one of them ,except lock, active
    /// moving animation to new room und cause events(e,b,bs)
    /// </summary>
    /// <returns></returns>
    public override bool EventMoving()
    {
        return GameManagers.instance.CanMoveNextRoom == true && GameManagers.instance.atDungeon == true;
    }

    public override bool EventMoveEnd()
    {
       return state=="IDLE";
    }


   

    public override bool EventCancelCasting()
    {
        throw new NotImplementedException();
    }



    public override bool EventSkillFinish()
    {
        throw new NotImplementedException();
    }




    //BATTLE EVENT
    public override bool EventMatches()
    {
        return GameManagers.instance.inBattle && target.health > 0
            && GameManagers.instance.gameTime > 0;
    }
    public override bool EventCombat()
    {
        return isMatches && target.health > 0 && CanAttack(this);
    }

  

    //BUSINESS EVENT

    //DUNGEON EVENT

    //COMMON

    public override bool EventCamp()
    {
        throw new NotImplementedException();
    }

    public override bool EventDead()
    {
        return target.health <= 0 || health <= 0;
    }





    bool EventTargetDied()
    {
        //win the game
        return target != null && target.health == 0 && WinnerConsole(target);
    }




    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    bool EventTradeStarted()
    {
        // did someone request a trade? and did we request a trade with him too?

        return target != null;
    }
    bool EventTradeDone()
    {
        // trade canceled or finished?
        return state == "TRADING";
    }

    bool craftingRequested;
    bool EventCraftingStarted()
    {
        bool result = craftingRequested;
        craftingRequested = false;
        return result;
    }

    //bool EventCraftingDone()
    //{
    //    return state == "CRAFTING" ;
    //}



    public override bool EventCraft()
    {
        throw new NotImplementedException();
    }

    public override bool EventStartCraft()
    {
        throw new NotImplementedException();
    }

    public override bool EventEndCraft()
    {
        throw new NotImplementedException();
    }

    public override bool EventDialogue()
    {
        throw new NotImplementedException();
    }

    public override bool EventTrade()
    {
        throw new NotImplementedException();
    }

    public override bool EventStartTrade()
    {
        throw new NotImplementedException();
    }

    public override bool EventEndTrade()
    {
        throw new NotImplementedException();
    }

    public override string UpdateServer_Sprint()
    {
        return "Sprint";
    }

    public override string UpdateServer_ROLL()
    {
       return "Roll";
    }

    public override void Warp(Vector3 pos)
    {
        banding.RpcWarp(pos);
    }

    public override bool EventStartCasting()
    {
        throw new NotImplementedException();
    }

    public override bool EventFinishCasting()
    {
        throw new NotImplementedException();
    }

    public override bool EventSkillRequest()
    {
        throw new NotImplementedException();
    }




    #endregion

    //TODO
    #region Items Module
    /// <summary>
    /// MODULE RULES
    /// player can use item who collect at dungeon , SyncList save all inventory items,
    /// player when click the item needs check requirement that EXECUTE apply to start functions by type
    /// same as SHENSHAN && TGDN && WILD GUYS
    /// </summary>
    /// <param name="id"></param>


    #endregion
}
#endregion