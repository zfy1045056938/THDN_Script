using System;
using Mirror;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// non-sync  to player, just use at dungeon, destory when leave dungeon,
/// when arrive at the dungeon ,active buff for equipment who equip or stay in inventory
/// 
/// </summary>
[Serializable]
    public partial struct Buffs 
    {

    // hashcode used to reference the real ScriptableSkill (can't link to data
    // directly because synclist only supports simple types). and syncing a
    // string's hashcode instead of the string takes WAY less bandwidth.
    public int hash;

    // dynamic stats (cooldowns etc.)
    public int level;
    public double buffTimeEnd; // server time. double for long term precision.
    

    // contructors
    public Buffs(BuffSkill data, int level)
    {
        hash = data.name.GetStableHashCode();
        this.level = level;
        buffTimeEnd = NetworkTime.time + data.buffTime; // start buff immediately
    }

    // wrappers for easier access
    public BuffSkill data
    {
        get
        {
            // show a useful error message if the key can't be found
            // note: ScriptableSkill.OnValidate 'is in resource folder' check
            //       causes Unity SendMessage warnings and false positives.
            //       this solution is a lot better.
            if (!ScriptableSkill.dict.ContainsKey(hash))
                throw new KeyNotFoundException("There is no ScriptableSkill with hash=" + hash + ". Make sure that all ScriptableSkills are in the Resources folder so they are loaded properly.");
            return (BuffSkill)ScriptableSkill.dict[hash];
        }
    }
    public string name => data.name;
    public Sprite image => data.sSprite;
    public float buffTime => data.castTime.Get(level);
    public bool remainAfterDeath => data.remainAfterDeath;
    public BuffType buffType=> data.buffType;
    public int maxLevel => data.maxLevel;

   

    public float BuffTimeRemaining()
    {
        // how much time remaining until the buff ends? (using server time)
        return NetworkTime.time >= buffTimeEnd ? 0 : (float)(buffTimeEnd - NetworkTime.time);
    }
}

public class SyncListBuff : SyncList<Buffs> { }