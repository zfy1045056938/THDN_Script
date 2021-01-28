using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleStartInfo
{
    public static DeckInfo SelectDeck;
    public static PlayerData player;
    public static EnemyInfo SelectEnemyDeck;
    public static string sceneName;
    public static MapLocation dungeon;
    //Common GamePlay
    public static GameDifficult GameDifficult;
    
    //For Battle Leave state
    public static bool AtDungeon;
    public static bool IsWinner;
    //public static string DungeonDifficult;
    
    //player 
    public static Items Weapon;
    public static Items Armor;
    public static Items Ring;
    //public static Items Potion1;
    //public static Items Potion2;
    //public static Items Potion3;
    
    //
    public static int DungeonExtraBouns =0;
    public static DungeonEventType DungeonEventType = DungeonEventType.None;
}

public enum ScreenToLoad
{
    MainMenu,
    ConfigScreen,
    BattleScreen,
    
}

