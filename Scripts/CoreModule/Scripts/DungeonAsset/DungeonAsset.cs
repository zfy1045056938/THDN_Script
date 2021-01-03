using System;

using Mirror;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using TMPro;
using DungeonArchitect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public enum DungeonType
{
    None,
    Town,
    Dungeon,
}

/// <summary>
/// TODO 1128
/// </summary>
[System.Serializable]
public partial struct DungeonAsset
{
    //hash for load from dictionary<string,dungeon>
    public int hash;

    public string name;
    //record data to database 
    public bool hasExplore;
  
    public bool unlockstate; //1 lock
   
    //save resources
    // public DungeonThemeEngine engineAsset;
    // public DungeonConfig config;
  

    //Dungeon Goal for player
    //player at dungen explore new dungeon for goal und explore at random dungeon
    //
    //construct save data for player explore progress
   public DungeonAsset(ScriptableDungeon data)
    {
        name = data.dungeonName;
        hash = data.name.GetStableHashCode();
        hasExplore = false;
        unlockstate = true;
        // engineAsset = null;
        // config = null;



    }

    // wrappers for easier access
    public ScriptableDungeon data
    {
        get
        {
            // show a useful error message if the key can't be found
            // note: ScriptableItem.OnValidate 'is in resource folder' check
            //       causes Unity SendMessage warnings and false positives.
            //       this solution is a lot better.
            if (!ScriptableDungeon.dict.ContainsKey(hash))
                throw new KeyNotFoundException("There is no ScriptableItem with hash=" + hash + ". Make sure that all ScriptableItems are in the Resources folder so they are loaded properly.");
            return ScriptableDungeon.dict[hash];
        }
    }


}
public class SyncDungeon : SyncList<DungeonAsset> { }
