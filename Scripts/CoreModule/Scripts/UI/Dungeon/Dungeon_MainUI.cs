using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;
using DungeonArchitect.Builders.GridFlow;
using DungeonArchitect;

/// <summary>
/// Dungeon UI Includes
/// inventory with tmp != p.inventory , tmp slot  save tmp items can save to p.i at camp
/// fixed 
/// 
/// </summary>
    public class Dungeon_MainUI:MonoBehaviour
    {
    public static Dungeon_MainUI instance;

    public Players entity;

    //module
    public GlobalSetting setting;
    public Dungeon_SkillBar skillBar;
    public Dungeon_Characterinfo charaInfo;

    //Sync List for dy data load by player who manager at town or camp
    [Header("Skillbar")]
    public SkillbarEntry[] skillbar = {
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha1},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha2},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha3},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha4},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha5},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha6},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha7},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha8},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha9},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha0},
    };

    // 
    public List<InventorySlot> tmpInventory;

    [Header("Dungeon Info")]
    public TextMeshProUGUI dungeonName;
    public GridFlowMinimap miniMap; //trace player object
    public GameObject playerTraceObj;


    [Header("Matches UI ")]
    public Board board;
    public GameManagers boardBoost;


    [Header("Others")]
    public GameObject dungeonSetting;
    public bool atDungeon = true;
    public bool atMatches = false;
    public bool atCamp = false;

    [Header("Party")]
    public int partyGuy = -1;   //default guy
    //Save
    public Dictionary<string, Entity> partyList = new Dictionary<string, Entity>();

}

