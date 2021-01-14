using System.Collections;
using System.Collections.Generic;
using DungeonArchitect;
using DungeonArchitect.Graphs;
using UnityEngine;


public enum TownType
{
  Main,
  Side
}

[System.Serializable]
public class MapLocation 
{
    public MapLocation(){}

    public MapLocation(string locationID, string locationName, string locationDetail,
     EnemyAsset[] npcList, bool islock, bool isLock, Sprite locationBG, 
     TownType townType, float itemPriceflow, bool hasCrash, bool isDungeon, 
     int minLevel, int maxLevel, bool hasEvent, List<EnemyAsset> enemyList,
      QuestInfos[] events, Graph dungeonTheme, bool hasBoss, EnemyAsset bossAsset,
       EnemyAsset[] bossList, List<string> itemList, DungeonType dungeonType, int needsKill
      )
    {
        this.locationID = locationID;
        this.locationName = locationName;
        this.locationDetail = locationDetail;
        this.npcList = npcList;
        _islock = islock;
        this.isLock = isLock;
        this.locationBG = locationBG;
        this.townType = townType;
        this.itemPriceflow = itemPriceflow;
        this.hasCrash = hasCrash;
        this.isDungeon = isDungeon;
        this.minLevel = minLevel;
        this.maxLevel = maxLevel;
        this.hasEvent = hasEvent;
        this.enemyList = enemyList;
        this.events = events;
        this.dungeonTheme = dungeonTheme;
        this.hasBoss = hasBoss;
        this.bossAsset = bossAsset;
        this.bossList = bossList;
        this.itemList = itemList;
        this.dungeonType = dungeonType;
        NeedsKill = needsKill;
    
    }

    public string locationID;
 public string locationName;
 public string locationScene;
 public string locationDetail;
 public EnemyAsset[] npcList;

 private bool _islock;

 [SerializeField]
 public bool isLock{
   get{return _islock;}
   set{_islock=value;}
 }
 public Sprite locationBG;
 public TownType townType;
 public float itemPriceflow;
 public bool hasCrash;
 
 //dungeon Info
 public bool isDungeon;
 public int minLevel;
 public int maxLevel;
 public bool hasEvent;
 public List<EnemyAsset> enemyList;
 public List<Vector3> enemyPos;
 public QuestInfos[] events;
 
 public Graph dungeonTheme;
 
 public Vector3 camPos;
 public EnemyAsset camObj;
 public EnemyAsset DireAsset;
 public bool hasBoss;
 public EnemyAsset bossAsset;
 public EnemyAsset[] bossList;
 public List<string> itemList;
 public DungeonType dungeonType;
 public int NeedsKill;
 public string dungeonChest;
 public string sceneName;
 
 public Vector3 Spos;

}
