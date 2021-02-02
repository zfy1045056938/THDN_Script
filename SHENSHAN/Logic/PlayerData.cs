using UnityEngine;
using System.Collections;
using SimpleJSON;
using ChuMeng;
using System.Collections.Generic;
using System;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Random = System.Random;
using Mirror;
using System.Linq;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine.AI;

public enum LocType
{
    None,
    Dungeon,
    Map,
    Town,
}
public delegate bool DsValidateInt(PlayerData p, int x);

public delegate bool DSValidateItem(PlayerData p, string itemName, int amount);

[Serializable]
public class PlayerData : Entity
{
    [Header("Component")] public NetworkNavMeshAgentRubberbanding banging;
    public NavMeshAgent agent;
    public string state="IDLE";
    private int _playerID;
    private int _playerLevel;
    private string _playerName;
    private List<CardAsset> _playerCardAsset;
    private bool isActive;
    private ItemManager item;

    public GameObject SavePanel;
    //For Server
    public List<InventorySlot> inventorySlot;
    public List<EquipmentSlot> equipSlot;
    private CardCollection cardCollection;
    
    //public RoleUpgradeConfigData info;

    private ItemManager ItemManager;
      //Bind Info

      [HideInInspector]
    public InventorySystem inventory;

    [SyncVar] public float speed=0.25f;
    //dialogue
    [SyncVar]
    public string dialogueData;

    public bool usePorForCon = false;

    private PixelCrushers.DialogueSystem.CharacterInfoPanel statPanel;
    
    [SerializeField, HideInInspector]
    public bool isNewBee;  //first
    public bool hasSet { get; set; }
    //test
    public static Dictionary<string,PlayerData> onlinePlayers = new Dictionary<string, PlayerData>();

    private bool localPlayerClickThrough;

    public static LocType LOCTYPE = LocType.None;
    public PlayerData(){}

    public PlayerData(int playerID, int playerLevel, string playerName, bool isActive)
    {
        _playerID = playerID;
        _playerLevel = playerLevel;
        _playerName = playerName;
        this.isActive = isActive;
    }

    

    #region PlayerStat



    [SerializeField]
    public int PlayerID
    {
        get
        {

            return _playerID;
        }

        set
        {
            _playerID = IDFactory.GetUniqueID();
        }
    }

   
    [SerializeField]
    public int PlayerLevel
    {
        get
        {
            return _playerLevel;
        }

        set
        {
            
            _playerLevel = value;
            PlayerPrefs.SetInt("PlayerLevel",PlayerLevel);
        }
    }

    private int _playerGem;

    [SerializeField]
    public int playerGem
    {
        get { return _playerGem; }
        set { _playerGem = value;
            PlayerPrefs.SetInt("playerGem",_playerGem);}
    }

    [SerializeField]
    public int mana { get; set; }

    private int _specItem;

    [SerializeField]
    public int specItem
    {
        get { return _specItem; }
        set
        {
            _specItem = value;
            PlayerPrefs.SetInt("specItem",_specItem);
        }
    }
    
    [SerializeField]
    private string _className;
    public string className{
        get{return _className;}
        set{
            _className=value;
        }
    }
    
    [SerializeField]
    public string PlayerName
    {
        get
        {
            return _playerName;
        }

        set
        {
            _playerName = value;
            
        }
    }
    [SerializeField]
    public bool IsActive
    {
        get
        {
            return isActive;
        }

        set
        {
            isActive = value;
        }
    }

    [SerializeField]
    public List<CardAsset> PlayerCardAsset{

        get
        {
            return _playerCardAsset;

        }
        set{
            if (_playerCardAsset!=null)
            {
                _playerCardAsset = PlayerCardAsset;
            }else{
                _playerCardAsset = value;
            }
        }
    }

 
    private Sprite playerAvatar; 
    [SerializeField]
  public Sprite PlayerAvatar
    {
        get
        {
            return playerAvatar;
        }
        set
        {
            playerAvatar = value;
        }
    }

  
   
    private int _money;
    [SerializeField]
    public int money
    {
        get { return _money; }
        set { _money = value; }
      
    }

    private int _dust;
    [SerializeField]
    public int dust
    {
        get
        {
            return _dust ;
        }
        set { _dust =value; }
     
    }

    private int _special;
    public int special { get{return _special; } set{
        PlayerPrefs.SetInt("PlayerSpec",special);
        _special=Mathf.Max(value,0);
    } }
    //////////////////////////////RPG//////////////////////////////////
    #region Resistance Module
    private int fr;

    [SerializeField]
    public int FR
    {
        get
        {
            return fr;
        }
        set { fr = value; }
    }

private int er;
     [SerializeField]
    public int ER
    {
        get
        {
            return er;
        }
        set { er = value; }
    }
    

    private int ir;

    [SerializeField]
    public int IR
    {
        get { return ir; }
        set { ir = value; }
    }
    private int pr;

    [SerializeField]
    public int PR
    {
        get { return pr; }
        set { pr = value; }
    }


    #endregion



    // private int def;

    // [SerializeField]
    // public int Def
    // {
    //     get { return def; }
    //     set { def = value; }
    // }
    private int weaponDef;

    public int WeaponDef
    {
        get { return weaponDef; }
        set { weaponDef = value; }
    }
    private int armorDef;

    [SerializeField]
    public int ArmorDef
    {
        get { return armorDef; }
        set
        {
            
            armorDef = value;
        }
    }

    // private int weaponDur;
    // [SerializeField]
    // public int WeaponDur
    // {
    //     get { return weaponDur; }
    //     set { weaponDur = value; }
    // }

    private int armorDur;

    public int ArmorDur
    {
        get { return armorDur; }
        set { armorDur = value; }
    }
    
    //Extra
//     private float _ef;
//      [SerializeField]
//     public float extraFlash { get{
//         return _ef;
//     } set{
//    _ef=value;
//     }
//     }
    private int _esd;
     [SerializeField]
    public int extraSpellDamage { get{
        return _esd;
    } set{
      
               _esd=value;
        
    } }
    // private float _edsPerc;
    //  [SerializeField]
    // public float ESDPerc { get{return _edsPerc;}
    //     set
    //     {
           
    //             _edsPerc = value;
            
    //     }
    // } 
    [SerializeField]
    public string account{ get; set; }

    private ExponentialLong _expMax = new ExponentialLong{mult=100,bv=1.1f};
    [SerializeField] public float expMax{
        get{
        return _expMax.Get(PlayerLevel);
}

    }

    private float _exp;
    public float experience
    {
        get { return _exp;}
        set
        {
            if (value <= _exp)
            {
                _exp=Mathf.Max(value,0);
            }else{
                _exp=value;
            //
            while(_exp>=expMax&&PlayerLevel<maxLevel){
                //level up
                _exp-=expMax;
                ++PlayerLevel;
                SoundManager.instance.PlaySound(SoundManager.instance.levelClip);
                //
                Utils.InvokeMany(typeof(PlayerData),this,"OnLevelUp_");
            }
            //
            if(_exp>expMax)_exp=expMax;
            }
        }
    }

    [SerializeField]
    public float skillExperience { get; set; }
    [SerializeField]
    public bool deleted { get; set; }
    private int _pHealth;
    [SerializeField]
    public int playerHealth { get{
        return _pHealth;
    } set{
        _pHealth=value;
    } }
    
    //Player Weapon damage und Dur
    private int damage;

    [SerializeField]
    public int atk {
        get { return damage; }
        set
        {

                damage = value;
        
        }
    }
    private int _atkCount;
    [SerializeField]
    public int atkCount { get{
        return _atkCount;
    } set{
        _atkCount=value;
    } }
    [SerializeField]
    public bool online { get; set; }
    [SerializeField]
    public int coins { get; set; }
    #endregion

public static PlayerData localPlayer;
    internal float remainingLogoutTime;

    private void Awake()
    {
     if(localPlayer==null)   
     localPlayer=this;   
     
     BattleStartInfo.player=this;
     
//     DontDestroyOnLoad(this.gameObject);

    }


    protected override string UpdateServer()
    {
        throw new NotImplementedException();
    }

    protected override void UpdaetClient()
    {
        //
        Debug.Log("====Upate Client Module====");

        Utils.InvokeMany(typeof(PlayerData), this, "UpdateServer_");
    }


    public override void OnStartLocalPlayer(){
        localPlayer =this;
        
//        OnStartLocalPlayer_DS();
        //
        Utils.InvokeMany(typeof(PlayerData),this,"OnStartLocalPlayer_");
    }

    public override void OnStartClient(){
        base.OnStartClient();
        //

    }

    public float exppercent()
    {
        return (experience != 0 && expMax != 0) ? (float) experience / (float) expMax : 0;
    }

       public void Start(){
            if(!isServer && !isClient)return;

            //
            
                onlinePlayers[name] = this;

            Utils.InvokeMany(typeof(PlayerData),this,"Start_");
        }

    //

    public void UpdateInfo(){
        //if(Strength>=3){
        //    atk+= Mathf.FloorToInt(Strength/3);
        //}
        //if(Dex>=3){
        //    int ed=0;
        //    float edp=0.0f;
        //    //has extra
        //        ed= Mathf.RoundToInt(Dex/3);
        //        edp = Dex/3.0f/100;
        //        armorDef +=ed;
        //   //flash
        //   extraFlash = 0.3f + edp;
        //}
        //if( Magic >=3){
        //    int esd= 0;
        //    float esdp =0.0f;
            
        //        esd = Mathf.RoundToInt(Magic/3);
        //        esdp = Magic/3.0f/100;
            
           //extraSpellDamage =1;
           //ESDPerc = 0.4f + esdp;
        //}
        
    }
 

//    #endregion

    #region Dialogue

    public void OnStartLocalPlayer_DS()
    {
        Debug.Log("dialogue system start");
        DialogueManager.ResetDatabase(DatabaseResetOptions.KeepAllLoaded);
        PersistentDataManager.ApplySaveData(dialogueData);
        var players = localPlayer;
        if (usePorForCon && players != null)
        {
            DialogueLua.SetActorField("Player",DialogueSystemFields.Actor,players.name);
        }

        StartCoroutine(UpdateQuestStateListenerAfterOneFrame());

    }
    
    //
    IEnumerator UpdateQuestStateListenerAfterOneFrame()
    {
        yield return null;
        foreach (var qsl in FindObjectsOfType<QuestStateListener>())
        {
            qsl.UpdateIndicator();
        }
        
        
        
    }

    private Coroutine m_updatedsdc = null;

    public void UpdateDiaSystemData()
    {
        if (m_updatedsdc == null)
        {
            m_updatedsdc = StartCoroutine(UpdateDSC());
        }
    }

    private IEnumerator UpdateDSC()
    {
        yield return new WaitForEndOfFrame();
        m_updatedsdc = null;
        UpdateDSDN();
    }

    public void UpdateDSDN()
    {
        dialogueData = PersistentDataManager.GetSaveData();
        if (!isServer)
        {
            CmdUpdateDSDOS(dialogueData);
        }
    }

    #region   Lua&Command

    public DsValidateInt DSAddMoney = delegate { return true; };
    public DsValidateInt DSAddDust = delegate { return true; };
    public DsValidateInt DSAddOthers= delegate{return true;};
    //Lua Command Command
    [Command]
    public void CmdUpdateDSDOS(string newData)
    {
        Debug.Log("SYNC");
        dialogueData = newData;
    }

    [Command]
         public void CmdAddMoney(int amount)
         {
             if(!DSAddMoney(this,amount))return;
             money += amount;
         }
         
         [Command]
         public void CmdAddDust(int amount)
         {
             if(!DSAddMoney(this,amount))return;
             dust += amount;
         }

        [Command]
         public void CmdAddOthers(int amount){
             if(!DSAddOthers(this,amount))return;
             special += amount;
         }
#endregion


    #endregion


    void Destory()
    {
        if (!isServer && !isClient) return;

        if (NetworkServer.active)
        {
            
        }
        //
        if (isLocalPlayer)
        {
            localPlayer = null;
        }
        //
        onlinePlayers[name] = null;
        //
        Utils.InvokeMany(typeof(PlayerData),this,"OnDestory_");
    }


    #region Command

    [Command]
    public void CmdAddExp(float exp)
    {
        Debug.Log("Get exp"+exp);
        experience += exp;
        
    }
    [Command]
    public void CmdAddItems(string item){
Items itemss = ItemDatabase.instance.FindItem(int.Parse(item));

    InventorySystem.instance.AddItem(itemss);
    }

    #endregion



}
