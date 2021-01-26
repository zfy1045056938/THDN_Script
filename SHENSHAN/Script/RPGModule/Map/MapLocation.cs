using System.Collections;
using System.Collections.Generic;
// using DungeonArchitect;
// using DungeonArchitect.Graphs;
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
