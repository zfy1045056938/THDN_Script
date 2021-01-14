using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonArchitect;
using DungeonArchitect.Builders.GridFlow;
using DungeonArchitect.Builders.Grid;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
using Mirror;
using UnityEngine.AI;
using DungeonArchitect.Navigation;
// using DestroyIt;

using Michsky.LSS;
using Invector.vItemManager;
using System.Linq;
public enum RoomType
{
    None,
    Business,
    Matches,
    SE,
    
}
/// <summary>
/// TODO DUNGEON CONFIG  Old
/// Manager Dungeon config with new scene
/// player at dungeon needs interactive with obj , because of the whole dungeon
///  split with small rooms pieces that means player at dungeon enter next rooms with state (next & p.h>0)
///  when player enter to next room ,current ceils will destory und preload the new ceil when player arrived
///  
/// 1. Player First Spawning starting room, select adj door to nextRoom (DOOR->Room)
/// 2. when player at nextroom ,star RoomEvent check util it done
/// 3. got reward to pools for console
/// 4. check room state if it's final room , check DUNGEONDAY und enter to Camp for next day start
/// 5.CAMP can manager player stats and upgrade items for next dungeon with higher than before (ENEMYBOUNS*=0.2)
/// 6.dungeon exists interactive object for player needs use by items, check inventory items enough and DICE that unlock or open the item
/// some items has special effect for player explore .
/// ===========================================================
/// 1.Save fixed dungeon elements 
/// 2. 
/// </summary>
public class GlobalSetting : MonoBehaviour
{

    public static GlobalSetting instance;

#region Old

    // public delegate void EndTurn();

    // //when at new rooms start event
    // public delegate void OnStartExploreRoom();
    // public event OnStartExploreRoom onStartExploreRoom;

    // //finish the current event
    // public delegate void OnEndExploreRoom();
    // public event OnEndExploreRoom onEndExploreRoom;

    // public delegate void OnLTorch();
    // public event OnLTorch onLTorch;

    // //Update day
    // public delegate void OnDayChanged();
    // public event OnDayChanged onDayChanged;

    // //Change Room start event Module by room type
    // public delegate void OnChangeRoom();
    // public event OnChangeRoom onChangeRoom;

    // //


       
    // // [Header("Entity Board")]
    // // public static PlayerStats players;
    // // public static EnemyStats enemy;
    
    // // public Dictionary<uint, Entity> playerList = new Dictionary<uint, Entity>();
    // // public static MatchTurnManager turnManager;
    // // public static Board board;
    // // public static CollectionsPool pool;

    // // [Header("Dungeon Config")]
    // // public DungeonAsset dungeonAsset;   // bind from DUNGEONSTARTINFOs
    // // public GridDungeonBuilder builder;
    // // public GridFlowDungeonConfig config;
    // // public GridDungeonModel dungeonModel;
    // // public DungeonThemeEngine currentTheme;
   
    // // public GridFlowMinimap miniMap;  //current model  will load next ceil
    // // public GridFlowMinimapTrackedObject playerObj;      //show in mini Map
    // // public uint seedID;     //player can record the seed in DIVMAP
    // // public LinkedList<DungeonDoor> allDoors;
    // // // public LinkedList<DungeonRooms> allRooms;
    // // // public LinkedList<torch> allTorches;

    // [Header("Logs")]
    // public List<GameObject> logsList;
    // public TextMeshProUGUI logMessage;
    // public Transform logPos;
    // public GameObject battleLogPanel;

    // [Header("Dungeon State")]
    // //Dungeon State just for dungeon 
    // public bool atDungeon = false;
    // public bool atCamp = false;
    // public bool configEvent = false;    //rnd event to manager the data
    // public bool isMove = false; //when walk by waypoints changestate
    // public bool configMap = false;  //if config ,can record target cell to map(items)
    // public bool complateEvent = false;  //when done change states
    // public int dungeonDay = 1; //default 1 dungeosize will reduce by day (25-15-9) for reduce the explore times
    // //[Header("Rooms Info")]
    // //public int roomId = -1;
    // //public bool canNext = false;
    // //public bool endRoom = false;
   
    // // public DungeonRooms dungeonRoom;   
    // [Header("Others")]
    // public GameObject FadeScreen;   //when enter next room fadeScreen show for load obj
    // public GameObject winScreen;
    // public GameObject loseScreen;
    // public GameObject preloadingUI;
    // public CinemachineVirtualCamera mainCamera;

    // [Header("UI Module")]
    // //common 
    // public GameObject dungeonObj;
    // public TextMeshProUGUI nameText;
    // public TextMeshProUGUI sizeText;
    // //progress 
    // public Slider dungeonProgress;
    // public TextMeshProUGUI current;
    // public TextMeshProUGUI goal;
    // public TextMeshProUGUI stateText;

    // [Header("DIV MAP")]
    // //for player who config eine div map can record the room piece when lighting the torch Range(9,20)s
    // public bool useDM = false;
    // public int recordNum = 0;
    // public Item divMap;
    // public bool canRecord = false;
    

    // [Header("Dungeon Trace State")]
    // public bool changeState = false;    //for change state


    // [Header("Dungeon Cell")]
    // public List<GameObject> doorList;   //save all dungeon door when player select door check state
    // public List<GameObject> roomPieces; //split dungeon to pieces when player select target then move(FADE)
    // public int roomID = -1; //when dungeon at starting room set room id
    // public HashSet<int> adjId;
    // public Battle_CharacterInfo battle_Character;
    // public Battle_SkillPanel battle_skill;

    // public InventorySlot[] tmpInventory;// tmp slots 

#endregion
    
    private Players player;
    public Dictionary<string,Players> playerTeam;

    public ScriptableDungeon currentDungeon;
    public Transform startPosition;
    [Header("Prop")]
    public GameObject[]  Chest;
public GameObject [] torch;
public GameObject [] enemy;
public GameObject[] DungeonTrap;
public GameObject[] dungeonShrine;


    [Header("DungeonInfo")]
    public string dungeonName;
    public DungeonType dungeonType;

    public NavMeshSurface surface;

    public DungeonNavAgent3D dungeonNavAgent;

    //for random generate item or ability 
    //
    public List<DungeonChest> chestList;
    public List<Entity> enemyList;
    public List<DungeonShrine> shrineList;

    [Header("Other GP")]
    public GameObject TrapWall;
    public GameObject DeadObj;
    public ItemDatabase itemDatabase;

    [Header("Common Game State")]
    public static bool canCraft=false;
    public static bool stateRate=true;

    public static float rewardBouns=0f;
    

    
    private void Start()
    {
        instance = this;

        //
        player =  Players.localPlayer;
        // playerList.Add(DungeonStartInfo.PLAYERS.netId, DungeonStartInfo.PLAYERS);
        //
        // playerTeam.Add(mainPlayer.name,mainPlayer);
        //Rnd Generae Prop
      ReloadDungeonAsset();
       //
        StartCoroutine("DungeonTimeRoutine");
    }

    void Update(){
        
        

    }
    public void ReloadDungeonAsset(){
        //clear old object
        foreach(var v in Chest){
            if(v!=null){
            chestList.Add(v.GetComponent<DungeonChest>());
            }
        }
          foreach(var e in enemy){
            if(e!=null){
            enemyList.Add(e.GetComponent<Monster>());
            }
        }
          foreach(var d in dungeonShrine){
            if(d!=null){
            shrineList.Add(d.GetComponent<DungeonShrine>());
            }
        }
       
    }


  
   


    public void ChangeScene(string scene){
         surface.RemoveData();
        //destory
        for(int i=0;i<transform.childCount;i++){
            if(transform.GetChild(i).gameObject!=null){
           Destroy(transform.GetChild(i).gameObject);
            }
        }
        Destroy(this.gameObject);  
        //
        
        TownManager.instance.ChangeScene(scene);
    }

    public void ChangeIndie(string name){
        //destory
        for(int i=0;i<transform.childCount;i++){
            if(transform.GetChild(i).gameObject!=null){
           Destroy(transform.GetChild(i).gameObject);
            }
        }
        Destroy(this.gameObject);
        //
        FindObjectOfType<Players>().lastSceneName=name;
        //
        FindObjectOfType<LoadingScreenManager>().LoadScene(name);
    }
   

    public void BreakWall(){
        TrapWall.GetComponent<Destructible>().ApplyDamage(100.0f);
        TrapWall.GetComponent<AudioSource>().Play();
        
    }

    public void Test_Loot(){
        GameObject obj = Instantiate(DeadObj,transform.position,Quaternion.identity) as GameObject;
        //
        var cm = itemDatabase.itemList.FindAll(ci => ci.type == vItemType.CraftingMaterials);
        int rndItem = Random.Range(0,cm.Count);
        int rndCounter = Random.Range(1,3);
        vItem vi = itemDatabase.GetItemByName(cm[rndItem].name.ToString());
        ItemReference ir = new ItemReference{
            name =vi.name,
            amount=rndCounter,
        };
        obj.GetComponentInChildren <vItemCollection>().items.Add(ir);
        if(obj.GetComponentInChildren<vItemCollection>().items.Count>0){
            Debug.Log("Got items");
        }else{
            Debug.Log("non this item");
        }
        //

    }
    public void DeadConsole(){

    }
}
