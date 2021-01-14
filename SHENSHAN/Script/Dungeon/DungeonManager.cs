using System.Collections;
using System.Collections.Generic;
using DAShooter;
using DG.Tweening;
using UnityEngine;
using DungeonArchitect.Navigation;
using DungeonArchitect;
using JackRabbit;
using Mirror;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using Unity.Mathematics;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using DAShooter.TwoD;
using DungeonArchitect.Builders.Grid;
using DungeonArchitect.Builders.GridFlow;

using UnityEngine.AI;



///Client Load Dungeon
//When Map Load Level To Dungeon
public class DungeonManager : MonoBehaviour
{
  public static DungeonManager instance;
  public GridDungeonConfig config;
  private PlayerData player;
  public Dungeon dungeon;
  public DungeonNavMesh navMesh;
  public SpecialRoomFinder2D finder;
  public Camera playerCamera;
  public LevelNpcSpawner npc;
  public WaypointGenerator waypoint;
    public DungeonUIManager uiManager;
  public Image LoadingBG;
  public Text LoadingNoticeText;
  public GameObject playerObj;
  public UIPanel GoalPanel;
  public SpecialRoomFinder2D roomFinder;
  public EnemyAsset[] enemyList;
  public GameObject miniMapObj;
  public string enemyPath="NPC/Enemy/Chap01";
  private int dungeonCount=1;
  private int maxDungeonCount=3;
  [Header("Goal Panel Info")] public Text LocText;
  public Text currLevelText;
  public Text maxLevelText;
  public Text sizeText;
  public Text requiredText;
  public Text mainQuestText;
  public Text itemsAtDungeonText;
  public Text seedText;
  public bool isfirstLoad=true;

  [Header("DungeonAsset")]
  public MapLocation mapLocation;
  public int dungeonCurrent;
  public int dungeonGoal;
  
  
 /// <summary>
  /// Awake is called when the script instance is being loaded.
  /// </summary>
  void Awake()
  {
      instance=this;
      if (BattleStartInfo.player != null)
      {
          player = BattleStartInfo.player;
          Debug.Log("Got Player"+player.PlayerName);
      }
      else
      {
         
      }
      
  }

 /// <summary>
  /// Start is called on the frame when a script is enabled just before
  /// any of the Update methods is called the first time.
  /// </summary>
  void Start()
  {
      npc  = FindObjectOfType<LevelNpcSpawner>();
      waypoint = GetComponent<WaypointGenerator>();
      roomFinder = FindObjectOfType<SpecialRoomFinder2D>();
//      enemyList = Resources.LoadAll<EnemyAsset>(enemyPath);
      config = FindObjectOfType<GridDungeonConfig>();
      uiManager = FindObjectOfType<DungeonUIManager>();

    //   foreach (EnemyAsset enemy in enemyList)
    //   {
    //       if (enemy != null)
    //       {
    //           Debug.Log(enemy.EnemyName);
    //       }
    //   }

      // if(player!=null){
      //   GenerateLevel();
      // }
       GenerateLevel();
       LoadQuest();

  }
void Update(){
    dungeonCurrent = DialogueLua.GetVariable("DungeonMonster").asInt;
}
  void GenerateLevel(){
      int seed = Mathf.FloorToInt(Random.value*int.MaxValue);
      seedText.text = seed.ToString();
      dungeon.Config.Seed=(uint)seed;
      //
      StartCoroutine(RebuildRoutine(dungeon));

  }

  //When Player Into dungeon , load dungeon with playerinfo , then load player 
  IEnumerator RebuildRoutine(Dungeon dungeon){
      
    
      LoadingNoticeText.text="加载地下城....";
      
      dungeon.DestroyDungeon();
      
      yield return 0;
      //
      GetDungeonData();
     
      //
      dungeon.Build();
      yield return 0;


      LoadingNoticeText.text="构建敌人路径";
      BuildNav();
      
    
      yield return 0;


//      var miniMap = FindObjectOfType<MiniMapRebuilder>();
//      if (miniMap != null)
//      {
//          miniMap.OnPostDungeonBuild(this.dungeon,dungeon.ActiveModel);
//      }
//      
      LoadingNoticeText.text = "生成路径";
      waypoint.BuildWaypoints(dungeon.ActiveModel,dungeon.Markers);
//        playerObj.transform.position = FindObjectOfType<SpecialRoomFinder2D>().GetComponent<SpecialRoomFinder2D>()
//            .startSpawner.transform.position;
      yield return 0;
      roomFinder.FindSpecialRooms(dungeon.ActiveModel);
        
        LoadingNoticeText.text="生成玩家";
        
        var startPoints = FindObjectOfType<SpawnPoints2D>();
        var player = FindObjectOfType<PlayerData>();
       
        
        if (playerObj != null && isfirstLoad==true)
        { 
            playerObj = Instantiate(playerObj,startPoints.transform.position,Quaternion.identity) as GameObject ;
                 
            playerObj.gameObject.AddComponent<CharacterController2D>();
            
            playerObj.gameObject.GetComponent<CharacterController2D>().rig2D = playerObj.GetComponent<Rigidbody2D>();
            playerObj.gameObject.GetComponent<CharacterController2D>().box2D = playerObj.GetComponent<BoxCollider2D>();
            playerObj.gameObject.GetComponent<CharacterController2D>().agent = playerObj.GetComponent<NavMeshAgent>();
            playerCamera.gameObject.AddComponent<FollowTarget>().GetComponent<FollowTarget>().target = playerObj.transform; 
            playerCamera.gameObject.GetComponent<FollowTarget>().SetTarget(playerObj.transform.position);
        }
        else
        {
            
        }
       
        DialogueManager.SendUpdateTracker();
        yield return 0;
        
//        if (miniMapObj != null)
//        {
//            miniMapObj.gameObject.SetActive(true);
//            var miniTrack = FindObjectOfType<MiniMapCameraTracker>();
//            if (miniTrack != null)
//            {
//                miniTrack.trackingTransform = playerObj.transform;
//            }
//        }

        yield return  0;
       

        
        LoadingNoticeText.text="生成敌人";
        npc.OnPostDungeonBuild(dungeon,dungeon.ActiveModel);
        yield return 0;
        //
       
//        var enemyController = GameObject.FindObjectsOfType<AIController2D>();
//        var playerPos = playerObj.transform.position;
//        foreach (var enemy in enemyController)
//        {
//            var en = enemy.transform.position;
//            var distance = (playerPos - en).magnitude;
//            if (distance < 10f)
//            {
//                Destroy(enemy);
//            }
//        }
        yield return new WaitForSeconds(0.3f);
        //Generate Quest Goal (common)
      
    
       //Load Dungeon Info To Goal
       if (BattleStartInfo.dungeon != null)
       {
           LocText.text = BattleStartInfo.dungeon.locationName;
           maxLevelText.text = BattleStartInfo.dungeon.maxLevel.ToString();
           currLevelText.text = dungeonCount.ToString();
           sizeText.text = CheckDungeonSize();
           requiredText.text = CheckDungeonType();
           mainQuestText.text = GetQuestInfo();
           itemsAtDungeonText.text = GetItems();
       }
       else
       {
           Debug.LogError("No dungeon");
       }
       
       //quest goal
       //Init Data
       DialogueLua.SetVariable("DungeonMonster",0);
       uiManager.dungeonGoal.panel.Open();
        dungeonGoal= DialogueLua.GetVariable("DungeonGoal").asInt;
        uiManager.dungeonGoal.Goal.text= dungeonGoal.ToString();
         dungeonCurrent=DialogueLua.GetVariable("DungeonCurrent").asInt;
        uiManager.dungeonGoal.Current.text = dungeonCurrent.ToString();
        //
        uiManager.dungeonGoal.currentSlider.SetValue(dungeonCurrent/dungeonGoal,dungeonGoal);
        
       GoalPanel.gameObject.SetActive(true);
        GoalPanel.transform.DOLocalMoveY(-900f,0.3f);
        
        yield return 0;
                uiManager.Init();
               
        
        while (GoalPanel.isActiveAndEnabled)
        {
            
            yield return null;
        }
        //  
        
        //
        //
        yield return 0;
        //All Done
        LoadingNoticeText.gameObject.SetActive(false);
        LoadingBG.gameObject.SetActive(false);
        
        //Start count 
        StartCoroutine(DungeonUIManager.instance.TimerRoutine());


  }

  void BuildNav(){
      navMesh.Build();
  }

  void LoadQuest()
  {
      
  }

//1.dungeon config,2.enemy asset 3.loc type 4.enemy.5boss asset
  public void GetDungeonData(){
    if(BattleStartInfo.dungeon!=null){
        mapLocation =BattleStartInfo.dungeon;
        
        //enemy list
        npc.bossAsset = BattleStartInfo.dungeon.bossAsset;
        npc.enemyAL = new List<EnemyAsset>(BattleStartInfo.dungeon.npcList);

        //
    }
  }


  public void NextLevel()
  {
     
      if(dungeonCount < maxDungeonCount)
      {
          LoadingBG.gameObject.SetActive(true);
          roomFinder.OnDungeonDestroyed(dungeon);
          npc.OnDungeonDestroyed(dungeon);
          dungeon.DestroyDungeon();
         Debug.Log("rebuild Dungeon");
         isfirstLoad = false;
        GenerateLevel();
          LoadingBG.gameObject.SetActive(false);
      }
      dungeonCount++;

  }



  public string CheckDungeonSize()
  {
      if (config.NumCells <= 50)
      {
          return sizeText.text = "小型";
      }else if (config.NumCells > 50 && config.NumCells <= 100)
      {
          return sizeText.text = "中型";
          
      }else if (config.NumCells > 100)
      {
          return sizeText.text = "大型";
      }

      return "";
  }

  public string CheckDungeonType()
  {
      switch (BattleStartInfo.dungeon.dungeonType)
      {
          case DungeonType.None:
              return "探索区域并不被击败即可通关区域";
              break;
         default:
             break;
          
      }

      return "";
  }

  public string GetQuestInfo()
  {
      foreach (var q in QuestLog.GetAllQuests())
      {
          if (q != null)
          {
              return QuestLog.GetQuestDescription(q);
          }
          else
          {
              return mainQuestText.text = "找点事做";
          }
      }

      return "";
  }

  public string GetItems()
  {
      foreach (var i in BattleStartInfo.dungeon.itemList)
      {
          if (i != null)
          {
              Items item= ItemDatabase.instance.FindItem(int.Parse(i));
              if (item != null)
              {
                  return item.itemName.ToString();
              }
          }
      }

      return "无";
  }

  
}
