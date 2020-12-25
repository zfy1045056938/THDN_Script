using System;


/// <summary>
/// STATIC VAIRABLE FOR SAVE PLAYER WHO ENTER DUNGEON
/// </summary>
    public static class DungeonStartInfo
    {
    //Common
    public static Players PLAYERS;
    public static string LASTSAVESCENE;
    public static int PARTYGUY = -1;


    //DUNGEON
    public static DungeonAsset DUNGEON;
    public static DifficultType DIFFICULTTYPE = DifficultType.Normal; //default
       


        //STATE
        public static bool ISWINNER = false;
    
    }

