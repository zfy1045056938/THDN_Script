using UnityEngine;
using Mirror;
using System.Collections.Generic;
using Invector.vItemManager;
using System.Linq;

public enum MonsterType
{
    None,
    Boss,
    Others,
    Reward,
    Bouns,
    Quest,

}
/// <summary>
/// Monster almost show at dungeon und battle with player ,at a special rooms
/// when player arrived at rooms needs battle with enemy , spawn the rooms element und config by dungeon
/// enemy with indie skill load by gde db ,und when player dialogue with target show the dialogue first then
/// choice one of the battle btn . monster in game includes
/// 1.Skill
/// 2.Items
/// 3.flow stats by dungeon(monster.stas * dungeondifficult )
/// 4.
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NetworkName))]
[RequireComponent(typeof(NetworkNavAgentBanding))]
public class Monster : Entity
{
    //Common
    #region Monster Stats



        public override int healthMax{
            get{
                int equipBouns=0;
                // foreach(vItemSlot slot in equipment)
                //     if(slot.amount>0)
                //         equipBouns += ((EquipmentItem)slot.item.data).healthBouns;
                return base.healthMax + equipBouns ;
                        
            }
        }

    //
        [SyncVar]
        public string races;
       
        
          

         //
       
        [SyncVar]
        public float critPerc;
    
        [SyncVar]
        public float flashPerc;
        
        [SyncVar]
        public float blockPerc;


    public MonsterType monsterType;



        public override float manaMax { get; set; }

        public override  float damage{
            get
            {
                int equipBouns = 0;
                // foreach (vItemSlot slot in equipment)
                // {
                //     if (slot.amount > 0)
                //     {
                //         equipBouns += ((EquipmentItem) slot.item.data).damageBouns;
                //     }
                // }
            //check element bouns
           
                return base.damage+equipBouns;
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
                //         equipBouns += ((EquipmentItem) slot.item.data).armorBouns;
                //         return base.armor + equipBouns;
                //     }

                return base.armor;
            }
        }

        public override float speed { get; }

        [SerializeField]
        public override  float blockChance{
            get{
                float equipBouns=0f;
                // foreach(vItemSlot slot in equipment)
                //         if(slot.amount>0){
                //             equipBouns += ((EquipmentItem)slot.item.data).BlockPer;
                //             //
                //             return base.blockChance + equipBouns;
                //         }
                return base.blockChance;
            }
        } //
        [SerializeField]
        public override float crit{
            get{
                float equipBouns=0f;
                // foreach(vItemSlot slot in equipment)
                //         if(slot.amount>0){
                //             equipBouns += ((EquipmentItem)slot.item.data).CriPer;
                //             //
                //             return base.crit + equipBouns;
                //         }
                return base.crit;
            }
        } //

        [SyncVar] private float _mana;

        public float mana
        {
            get { return Mathf.Min(_mana, manaMax); }
            set { mana = Mathf.Clamp(_mana, 0, manaMax); }
        }


    public override int aShield => base.aShield;

    
    public override EntityAnimState entityState { get => base.entityState; set => base.entityState = value; }
    public override int lockpick { get => base.lockpick; set => base.lockpick = value; }
    public override int science { get => base.science; set => base.science = value; }
    public override int leader { get => base.leader; set => base.leader = value; }
    public override int kissass { get => base.kissass; set => base.kissass = value; }
    public override float heavy { get => base.heavy; set => base.heavy = value; }
    public override int dungeoneering { get => base.dungeoneering; set => base.dungeoneering = value; }
    public override float Stamina { get => base.Stamina; set => base.Stamina = value; }
    public override float SprintStamina { get => base.SprintStamina; set => base.SprintStamina = value; }
    public override float WalkSpeed { get => base.WalkSpeed; set => base.WalkSpeed = value; }
    public override float RollSpeed { get => base.RollSpeed; set => base.RollSpeed = value; }
    public override float RollDistance { get => base.RollDistance; set => base.RollDistance = value; }
    public override float JumpSpeed { get => base.JumpSpeed; set => base.JumpSpeed = value; }
    public override float LadderStamina { get => base.LadderStamina; set => base.LadderStamina = value; }

    public double deadTime;
    public double deadTimeEnd;
    #endregion

    //only for monster need bind to AI->Loot Items
    [Header("Monster Loot")]
    public List<vItem> itemLoot;    //when dead can loot
    
    
   
    


    [Header("Battle State")]
    public bool canAtk = false;
    public bool canUseSkill = false; //when use skill currentskill[index]->try use skill
    public bool hasbuff = false;    //when cause element effec hasBuff->true
    // public bool isMatches = false;  //when enemy ai matches then set true for check the normal atk or skill
    

    #region Client&Server

    #endregion

    #region UpdateServer_Motion


    //for monster at dungeon, needs animation for every state show monster state,because of the monster always stay at dungeon
    // so it have 4 state for enemy
    //1.IDLE state mean player who arrive at the next room ist Battle Room  , player interactive with the object und start battle
    //2.matches und casting state show at the player who start battle with enemy , the bf will split part of two , monster board matches auto when
    // got matches piece change state for player notice the monster state und can cast skill motion for Atk or defend.
    //3.DEAD state mean win the battle und show the chest , when target is dead ,unlock nearest lock door except special door needs key or events cause

    [Server]
    public override string UpdateServer(){

        if (state == "IDLE")     return UpdateServer_IDLE();

        // if (state == "Matches") return UpdateServer_Matches();
        
        // if (state == "DIALOGUE") return UpdateServer_DIALOGUE();
        
        if (state == "CASTING")  return UpdateServer_CASTING();

        if (state == "COMBAT") return UpdateServer_COMBAT();
       
        if (state == "DEAD")     return UpdateServer_DEAD();
        return "IDLE";
    }
     [Server]
    public override string UpdateServer_DEAD(){
       //when monster die,show console panel und player got reward
        if (EventDead())
        {
            return "DEAD";
        }
       

        return "DEAD";
    }
     [Server]
    public override string UpdateServer_IDLE(){


        if (EventIdle())
        {
            return "IDLE";
        }
        if (EventDead())
        {
            return "DEAD";
        }

        //at board matches und use skill
        if (EventCombat())
        {
            return "COMBAT";
        }

        //with playe then idle to matches
        if (EventMatches())
        {
            return "MATCHES";

        }
        if (EventStartCasting())
        {
            //use skill
            return "CASTING";
        }

        if (EventFinishCasting())
        {
            return "MATCHES";
        }
        //Camp


        return "IDLE";
    }

    

     [Server]
    public override string UpdateServer_CASTING(){


        if (EventIdle())
        {
            return "IDLE";
        }
        if (EventDead())
        {
            return "DEAD";
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

        if (EventFinishCasting())
        {
            return "MATCHES";
        }
        //Camp


        return "CASTING";
    }

    //  [Server]
    // public override string UpdateServer_Matches(){


    //     if (EventIdle())
    //     {
    //         return "IDLE";
    //     }
    //     if (EventDead())
    //     {
    //         return "DEAD";
    //     }

    //     //at board matches und use skill
    //     if (EventCombat())
    //     {
    //         return "COMBAT";
    //     }
    //     if (EventMatches())
    //     {
    //         return "MATCHES";

    //     }
    //     if (EventStartCasting())
    //     {
    //         //use skill
    //         return "CASTING";
    //     }

    //     if (EventFinishCasting())
    //     {
    //         return "MATCHES";
    //     }
    //     //Camp


    //     return "MATCHES";

      
    // }



     [Server]
    public override string UpdateServer_COMBAT(){
        if (EventIdle())
        {
            return "IDLE";
        }
        if (EventDead())
        {
            return "DEAD";
        }
       
        //at board matches und use skill
        if (EventCombat())
        {
            //Start Battle
            DealDamageToTarget(target, damage, 1f, 1f, DamageType.Normal);
            //
            RpcOnDamageReceived(damage, DamageType.Normal);
            //


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
       
        if (EventFinishCasting())
        {
            return "MATCHES";
        }
        //Camp
       

        return "COMBAT";
    }
   


    #endregion

    #region UpdateClient_
    [Client]
 protected override void UpdateClient()
    {
        

        Util.InvokeMany(typeof(Monster),this,"UpdateClient_");
    }

    [Client]
    public override string UpdateClient_IDLE(){
        return "IDLE";
    }

    #endregion

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public override void Start()
    {
        base.Start();

        //
        //if(isClient)
        //{
        //    InvokeRepeating(typeof())
        //}
    }

    

    public override void Update()
    {
          Util.InvokeMany(typeof(Monster),this,"_UpdateClient");
    }

   

   

  


    #region State Check Includes AI Matches 
    public bool HasLoot()
    {
        return health <= 0;
    }


    // public bool CanUseSkill(int skillIndex) {
    //     //target Skill
    //     Skill s = skills[skillIndex];

    //     return health > 0 && mana > s.manaCosts ;
    // }
    // public bool CanAtk() {
    //     return target.health > 0;
    // }
    // //AI needs move tile auto when got matches change state to atk
    // // Monster->UpdateServer-> state=Combat
    // public bool MoveTile() {
    //     return health > 0 && !isFreeze; 
    // }


    


    #endregion 

    

    
    //TODO
    /// <summary>
    /// always start when player at current room
    /// </summary>
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }


    [Server]
    public override void OnDeath()
    {

        // take care of entity stuff
        base.OnDeath();

        // set death and respawn end times. we set both of them now to make sure
        // that everything works fine even if a monster isn't updated for a
        // while. so as soon as it's updated again, the death/respawn will
        // happen immediately if current time > end time.
        deadTimeEnd = NetworkTime.time + deadTime;


        ////when monster dead , got reward by monster+difficult+CurrentBouns
        // generate gold 
        gold = Random.Range(gold, gold/2);

        // generate items (note: can't use Linq because of SyncList)
        // foreach (ItemDropChance itemChance in dropChances)
        //     if (Random.value <= itemChance.probability)
        //         inventory.Add(new ItemSlot(new Item(itemChance.item)));

        //Update Dungeon state for player who can change next room
        //when at boss room ,check the dungeon day (<3 camp -> winner consoles)
        if(monsterType == MonsterType.Boss)
        {
            //End The explore
            // if(GlobalSetting.instance.dungeonDay <= 3)
            // {
            //     //Change to camp
            //     GlobalSetting.instance.DestoryOldDungeon();
            //     //load camp und add day (update difficult bouns 
            //     GlobalSetting.instance.LoadCamp(GlobalSetting.instance.dungeonDay);
            // }
            // else
            // {
            //     //End the dungeon und show winner panel
            //     DungeonStartInfo.ISWINNER = true;
            //     GlobalSetting.instance.DungeonConsole();

            // }

        }

        base.OnDeath();
    }


    public override void Awake()
    {
        base.Awake();
    }

    public override bool IsWorthUpdate()
    {
        return base.IsWorthUpdate();
    }

    
   

    public override void OnAggro(Entity e)
    {
        base.OnAggro(e);
    }

    /// <summary>
    /// ////////////////////////FSM Event Module/////////////////////////
    /// </summary>
    /// <returns></returns>
    #region Event  Module
    //player first meet monster at new rooms 
    public override bool EventIdle()
    {
        return state=="IDLE" && !IsMoving() && !GameManagers.instance.inBattle
            && !GameManagers.instance.isFinalRoom;
    }

    public override bool EventMoving()
    {
        // throw new NotImplementedException();
        return state=="MOVING";
    }
    //at matches board  player start matches with enemy
    //when enemy matches then auto atk to player
    //state : idle -> combat
    // UPdateServer_Combat->StartCombat()->CmddalDamagetotarget
    public override bool EventCombat()
    {
        return state == "COMBAT" && !canAtk && !isMatches;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override bool EventMatches()
    {
        return state == "MATCHES" && !isMatches && target.health>0;
    }

    public override bool EventDead()
    {


        return state == "DEAD" && GameManagers.instance.inBattle == false && health <= 0;
    }

    public override bool EventStartCasting()
    {
        return state=="CASTING"  && !canUseSkill;
    }

    public override bool EventFinishCasting()
    {
        return state == "MATCHES" ;
    }

    #endregion

   

   

    

    public override void DealDamageToTarget(Entity entity, float amount, float fTime = 0, float lTime = 0, DamageType type = DamageType.Normal, ElementDamageType det = ElementDamageType.None)
    {
        base.DealDamageToTarget(entity, amount, fTime, lTime, type, det);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    ///
    // [Server]
    // public override string UpdateServer_DIALOGUE()
    // {
    //     if (EventIdle())
    //     {
    //         return "IDLE";
    //     }
    //     if (EventDead())
    //     {
    //         return "DEAD";
    //     }

    //     //at board matches und use skill
    //     if (EventCombat())
    //     {
    //         //Start Battle
    //         DealDamageToTarget(target, damage, 1f, 1f, DamageType.Normal);
    //         //
    //         RpcOnDamageReceived(damage, DamageType.Normal);
    //         //


    //         return "COMBAT";
    //     }
    //     if (EventMatches())
    //     {
    //         return "MATCHES";

    //     }
    //     if (EventStartCasting())
    //     {
    //         //use skill
    //         return "CASTING";
    //     }

    //     if (EventFinishCasting())
    //     {
    //         return "MATCHES";
    //     }
    //     //Camp


    //     return "COMBAT";
    // }

    public override string UpdateServer_MOVING()
    {
        throw new System.NotImplementedException();
    }

    public override bool EventMoveEnd()
    {
        throw new System.NotImplementedException();
    }

    public override bool EventCancelCasting()
    {
        throw new System.NotImplementedException();
    }

    public override bool EventSkillRequest()
    {
        throw new System.NotImplementedException();
    }

    public override bool EventSkillFinish()
    {
        throw new System.NotImplementedException();
    }

    public override bool EventCamp()
    {
        throw new System.NotImplementedException();
    }

    public override bool EventCraft()
    {
        throw new System.NotImplementedException();
    }

    public override bool EventStartCraft()
    {
        throw new System.NotImplementedException();
    }

    public override bool EventEndCraft()
    {
        throw new System.NotImplementedException();
    }

    public override bool EventDialogue()
    {
        throw new System.NotImplementedException();
    }

    public override bool EventTrade()
    {
        throw new System.NotImplementedException();
    }

    public override bool EventStartTrade()
    {
        throw new System.NotImplementedException();
    }

    public override bool EventEndTrade()
    {
        throw new System.NotImplementedException();
    }

    public override string UpdateServer_Sprint()
    {
        throw new System.NotImplementedException();
    }

    public override string UpdateServer_ROLL()
    {
        throw new System.NotImplementedException();
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public override bool InvokeCommand(int cmdHash, NetworkReader reader)
    {
        return base.InvokeCommand(cmdHash, reader);
    }

    public override bool InvokeRPC(int rpcHash, NetworkReader reader)
    {
        return base.InvokeRPC(rpcHash, reader);
    }

    public override bool InvokeSyncEvent(int eventHash, NetworkReader reader)
    {
        return base.InvokeSyncEvent(eventHash, reader);
    }

    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
        return base.OnSerialize(writer, initialState);
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        base.OnDeserialize(reader, initialState);
    }

  

    public override void OnNetworkDestroy()
    {
        base.OnNetworkDestroy();
    }

  

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
    }

    public override void OnStopAuthority()
    {
        base.OnStopAuthority();
    }

    protected override void UpdateOverlays()
    {
        base.UpdateOverlays();
    }

    public override void RpcShowComboTip(int num)
    {
        base.RpcShowComboTip(num);
    }

    public override void Warp(Vector3 pos)
    {
        throw new System.NotImplementedException();
    }
}
