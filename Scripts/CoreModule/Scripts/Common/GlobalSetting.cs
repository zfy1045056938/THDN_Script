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
    
    private Players mainPlayer;
    public Dictionary<string,Players> playerTeam;

    
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
    
    private void Start()
    {
        instance = this;

        //
        mainPlayer =  Players.localPlayer;
        // playerList.Add(DungeonStartInfo.PLAYERS.netId, DungeonStartInfo.PLAYERS);
        //
        // playerTeam.Add(mainPlayer.name,mainPlayer);
        //Rnd Generae Prop
      ReloadDungeonAsset();
    }

/*
* public GameObject[]  Chest;
public GameObject [] torch;
public GameObject [] enemy;
public GameObject[] DungeonTrap;
public GameObject[] dungeonShrine;
*/

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

    

    /// <summary>
    /// Trace States
    /// player at dungeon needs explore room after rooms until arrived boss room
    /// every room have indie event for player to explore und spawn at starting room
    /// every dungeon have starting room for player who first to dungeon , 
    /// GlobalSetting for dungeon to trace player state ,state change will update by delegate includes
    /// NEXTRoom->
    /// 
    /// 
    /// </summary>
    private void Update()
    {
        //dungeon state

        //room state

    }




    /// <summary>
    ///  when player enter next room check roomType then start event
    ///  
    /// </summary>
    /// <param name="currentCeil"></param>
    /// <param name="nextCeil"></param>
    public void MoveNextRooms()
    {

        // List<Cell> roomList = builder.GetCellsOfType(CellType.Room);
        // if (roomList.Count == 0)
        // {
        //     return;
        // }

        // //when at starting room with 0

        // if (roomID == -1)
        // {
        //     //at starting room  to 0 must be starting room
        //     //then add  to adj room between the srs
        //     Cell currentRoom = dungeonModel.GetCell(roomID);
        //     //Get ConnectId
        //     foreach (var connectRid in currentRoom.ConnectedRooms)
        //     {
        //         //
        //         Cell other = dungeonModel.GetCell(connectRid);
        //         if (other == null) return;
        //         // got other room between currnet
        //         //what's current ,staring count to spawning room --> CELLROOM[0]++ to check adj
        //         float roomDistance = GridDungeonBuilder.GetDistance(currentRoom.Center, other.Center);
        //         //
        //         if (roomDistance > 0)
        //         {
        //             //Move Player to Next room center then Check the dungeon type
        //             //TODO MOVE PLAYER TEST TO CENTER POS
        //         }


        //     }
        // }
    }
          
     

    /// <summary>
    /// As Trigger when player at first room set ceils und check adj ceil id 
    /// </summary>
    /// <param name="ceilId"></param>
    public void SetCeil(GridDungeonModel ceilId)
    {


    }

    /// <summary>
    /// 
    /// </summary>
    public void DestoryOldDungeon(){

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="day"></param>
    public void LoadCamp(int day){

    }


    /// <summary>
    /// 
    /// </summary>
    public void DungeonConsole(){
        
    }

    /// <summary>
    /// with trigger when player select 
    /// </summary>
    /// <param name="id"></param>
    public void SetNextRoomDoors(int id)
    {
         //= id;
    }

    /// <summary>
    /// when player arriver unknown room start event by type
    /// </summary>
    /// <param name="type"></param>
    public void StartRoomEvent(RoomType type)
    {
        //if()

    }

    /// <summary>
    /// TODO  DUNGEON GP::Tmp inventory REMOVE !!!!!!!!
    /// </summary>
    /// <param name="p"></param>
    public void InitTmpInventory(Players p)
    {
        //as tmp slot destory when player leave dungeon
        // player can config inventory slot or upgrade by leader
        ////
        //p.tmpInventory = new List<InventorySlot>();

        ////
        //for (int i = 0; i < p.tmpInventory.Count; i++)
        //{
        //    p.tmpInventory[i].isTmp = true;
        //    //
        //    p.tmpInventory[i].item.itemName = "";   //reset
        //    //
        //}
    }


    public bool TmpEnoughSlots( )
    {
        return false;
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
      
        TownManager.instance.ChangeScene(scene);
    }

    
}
