using System;
using Mirror;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using TMPro;
using DungeonArchitect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine;

public class ScriptableDungeon : ScriptableObjectNonAlloc
{
   
    //
    public string dungeonName;
    public int dungeonID;
    public DungeonType dungeonType;
    public string sceneName;    //load from ab 
    public int dungeonSize;
    //public List<string> enemyList;      //as string   load from dic<int,entity> 
    //public List<string> eventList;    //as string  load from dic<int,de>

    public int dungeonLevel;
    //for player
    public bool hasExplore;
    public float exploreProgress;
    public string dungeonScene;
    //flow dungeon 
    // public float dungeonMoney;
    // public float dungeonExp;
    // public float dungeonDust;


   
    //public List<string> rewardItems;  //got dungeonreward when expplore done
    //public bool hasBoss;
    //public ScriptableCharacter bossAsset;    //dic<int,cs>

    //CORE storge locpieces, enemy ,boss und chest item
    public string dungeonTheme; //dungeon graphic
    public string dungeonGridFlowName;
    public int dungeonDay;


    //side dungeon goal for player explore
    public bool hasGoal;
    public bool finishGoal;
    public string dungeonGoals;
    public string covName;  //if have event ,start cov when player start at spawnrooms
    //collect goal item trace quest 
    public List<string> questItem;

    public bool unlockstate;
    //TODO
    public List<ScriptableCharacter> dungeonEnemy;
    //
    static Dictionary<int, ScriptableDungeon> cache;
    public static Dictionary<int, ScriptableDungeon> dict
    {
        get
        {
            // not loaded yet?
            if (cache == null)
            {
                // get all ScriptableItems in resources
                ScriptableDungeon[] items = Resources.LoadAll<ScriptableDungeon>("");

                // check for duplicates, then add to cache
                List<string> duplicates = items.ToList().FindDup(item => item.name);
                if (duplicates.Count == 0)
                {
                    cache = items.ToDictionary(item => item.name.GetStableHashCode(), item => item);
                }
                else
                {
                    foreach (string duplicate in duplicates)
                        Debug.LogError("Resources folder contains multiple ScriptableItems with the name " + duplicate + ". If you are using subfolders like 'Warrior/Ring' and 'Archer/Ring', then rename them to 'Warrior/(Warrior)Ring' and 'Archer/(Archer)Ring' instead.");
                }
            }
            return cache;
        }
    }


}

