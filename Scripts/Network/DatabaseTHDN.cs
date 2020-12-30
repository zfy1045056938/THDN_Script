
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using SQLite;
using Mirror;
using UnityEngine.AI;
using System;
using System.Text.RegularExpressions;
//using Unity.Mathematics;
using System.IO;
using UnityEngine;
using GameDataEditor;
using Invector.vItemManager;
////////////////RPG Module
//TODO 1115 ,add tips for player who can use item to learn
//Items
public enum ItemType
{
    JUNK,
    WEAPON,
    ARMOR,
    Shield,
  
    Potion,
    SI,
    //
    QuestTips,
    CItemTips,
    TrapTips,
    //
    KeyStone,
    Hats,   //new slot
}
//////////////DB QUERY => TABLE SAVE APPLICATION PATH
//SQL TABLE QUERY TODO _Update stats ez 
//charactern
#region DB TABLE
class characters
    {
        [PrimaryKey] // important for performance: O(log n) instead of O(n)
        [Collation("NOCASE")] // [COLLATE NOCASE for case insensitive compare. this way we can't both create 'Archer' and 'archer' as characters]
        public string name { get; set; }
        [Indexed] // add index on account to avoid full scans when loading characters
        public string account { get; set; }
        public string classname { get; set; } // 'class' isn't available in C#
        // online status can be checked from external programs with either just
        // just 'online', or 'online && (DateTime.UtcNow - lastsaved) <= 1min)
        // which is robust to server crashes too.

        public string charactername{get;set;}
    //public string religion{get;set;}
    //load model at dungeon


    //character lastpos data 
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
    //

    public string modelName{get;set;}

        public int level { get; set; }

        //points
        public int characterSkillPoints{get;set;}
        public int characterUSkillPoints{get;set;}
        //public int characterPoints{get;set;}

        //basic stats
        public float health { get; set; }
        public float hpRate{get;set;}

        public float mana { get; set; }
        public float manaRate{get;set;}


    public float cStamina { get; set; }
    public float cStaminaRate { get; set; }

    

    //Character Points
    //public int str { get; set; }
    //public int dex { get; set; }
    //public int inte { get; set; }
    //public int con { get; set; }
    //public int cha { get; set; }

    //common ab
    public float damage{get;set;}
        //public int armor{get;set;}
        public int aShield{get;set;}


        //uskill
        public int kissassLevel{get;set;}
        public int lockpickingLevel{get;set;}
        public int scienceLevel{get;set;}
        public int dungeoneeringLevel{get;set;}
        public int leaderLevel{get;set;}

    //model data
    public float cst { get; set; }  
    public float cws { get; set; }
    //roll
    public float crs { get; set; }
    public float crd { get; set; }
    //
    public float cjs { get; set; }
    public float cls { get; set; }  

    //extra ab influence by characterPoints
    public float heavy {get;set;}


    //TODO Update 125_shield&weapon cause others attitube (perc) 
    public float critPerc { get; set; }
    public float flashPerc { get; set; }
    public float blockPerc { get; set; }

    //party Num , limit is 3 that player can customize party with 
    // npc at dungeon who explore or save them , but at least eine main
    // character with leader , und the leader save to database with the basic
    // characterstats in DB
    public int teamNum{get;set;}
        public string bossName { get; set; }
     
        //for achievement when higher than the required num then unlock und got reward by
        //achievement items (hasActive =true und highlight)
        public int ac_killMonster{get;set;}
        public int ac_exploreDungeon{get;set;}
        public int ac_useSkill{get;set;}
        public int ac_criNum{get;set;}
    public int ac_chestNum { get; set; }
   



    public float exp { get; set; }

        //business 
        public int money{get;set;}
        public int dust{get;set;}


        //
        public float CAnti { get; set; }
        public float CEagelEyes { get; set; }
        //KissAss
        public float CCheat { get; set; }
        public float CHardAss { get; set; }
        //Science
        public float CSoulView { get; set; }
        public float CAntiAb { get; set; }

        //Leader
        public float CBusiness { get; set; }
        public float CVRate { get; set; }

    //Set config
    //public bool hasSet{get;set;}
    //public string setName{get;set;}
    //public int setNum{get;set;}

    public string lastsavescenename { get; set; }   //save c last enter scene
    public bool needtur { get; set; }   //if not close the tip for next tip
    public bool online { get; set; }
        public DateTime lastsaved { get; set; }
        public bool deleted { get; set; }
    }
   
 class accounts
    {
        [PrimaryKey] // important for performance: O(log n) instead of O(n)
        public string name { get; set; }
        public string password { get; set; }
        // created & lastlogin for statistics like CCU/MAU/registrations/...
        public DateTime created { get; set; }
        public DateTime lastlogin { get; set; }
        public bool banned { get; set; }
    }
//inventory
//slot items for player data
class character_inventory{
    [PrimaryKey]
    public string character { get; set; }

    public int slot { get; set; }


    public int amount { get; set; }
    public int itemStacksize { get; set; }  
    //
    public string name { get; set; }
     public ItemType itemType{get;set;}
    //common stats
    public int pItemLevel{get;set;}
    //public int itemHP{get;set;}
    //public bool canPU{get;set;}


    //public float hpRate{get;set;}
    //public int itemMP{get;set;}
    //public float mpRate{get;set;}
    //public int damage{get;set;}
    ////public int armor{get;set;}
    //public int aShield{get;set;}

    ////extra
    //public float blockPerc{get;set;}
    //public float flashPerc{get;set;}
    //public float critPerc{get;set;}

    //


    //public double buyPrice{get;set;}
    //public double sellPrice{get;set;}
    
      
}
 
//equipment
    class character_equipment:character_inventory{}

    class character_achievement{
        [PrimaryKey]
        public string character{get;set;}
        public int aid{get;set;}
        public string aName{get;set;}
        public bool hasLock{get;set;}

    }


    //influence by character who has explore dungeon
    //when player first enter eine dungeon ,set to db to record the state()
//TODO
class character_dungeon{
        [PrimaryKey]
        public string character{get;set;}
        public string dungeonName{get;set;}
    // EP = explorestage / SideDungeon <= 100%
    public bool unlock { get; set; }
       
        public bool hasExplore { get; set; }

    }

//skill for player progress data
    class character_skills{
        [PrimaryKey]
        public string character { get; set; }
       
        public string name { get; set; }


        public int level { get; set; }
        //when use skill add add skillexp with range(0.1,0.5), when exp == 100.0% level
        //up und add bouns by level 
        public float skillExp{get;set;}
    public bool hasLearn { get; set; }    //1 y,0 n
        

        public float sAmount{get;set;}

        public float castTimeEnd { get; set; }
        public float cooldownEnd
         { get; set; }



}


/// <summary>
/// 
/// </summary>
    class character_buffs
{
        [PrimaryKey]
        public string character { get; set; }

        public string name { get; set; }
        public int level { get; set; }
        public string equipment_buff{get;set;}
        public int buff_amount{get;set;}
       
        public float buffTimeEnd { get; set; }
        
    }



class character_dialoguedata
{
[PrimaryKey]
    public string character { get; set; }
    [Indexed]
    public string data { get; set; }
}


//Monster
//Skill
//RogueItems =>TODO
#endregion


#region TWG DB PRE
 class twgCharacter
{
    [PrimaryKey]
    public string character { get; set; }
    public string cName { get; set; }
    public string cRaces { get; set; }
    //model stats , effect for dungeon player, some static
    //equipment can effect to object 
    public float moveSpeed { get; set; }
    public float sprintSpeed { get; set; }
    public float reloadSpeed { get; set; }
    public float meleeSpeed { get; set; }
    
    //base stats
    public float cHealth { get; set; }
    public float cStamia { get; set; }
    public float cMana { get; set; }
    public float cDamage { get; set; }
    public float cArmor { get; set; }
    public float cCrit { get; set; }
    public float cFlash { get; set; }
    //player stats
    public int leadLevel { get; set; }
    public int lpLevel { get; set; }
    public int scienceLevel { get; set; }
    public int dungeoneeringLevel { get; set; }
    //others
    public bool isDelete { get; set; }
    public bool online { get; set; }
    
    public DateTime lastSave { get; set; }
    
//achievement
public int killNum { get; set; }
public int deadCount { get; set; }
}



//skill ability when player equip keystone to the hat slot
//active that effect und save to DB
// hat with keyStone r dy, every upgrade needs save to db
 class KeyStone
{
    [PrimaryKey]
    public string character { get; set; }
    public int ksID { get; set; }
    public float ksAmount { get; set; }
    public float ksCastTime { get; set; }
    public float ksCoolDown { get; set; }
    public int ksLevel { get; set; }
}

//inventory in twg for player r static ,just save item obj when player
//load game, but can upgrade in dungeon, reset when leave dungeon
class twgInventory {
    [PrimaryKey]
    public string character { get; set; }
    public string itemName { get; set; }
    public float itemAmount { get; set; }
    public string itemType { get; set; }   //
    public int itemLevel { get; set; }

}

class twgEquipment {
    [PrimaryKey]
    public string character { get; set; }
    public string itemName { get; set; }
    public float itemAmount { get; set; }
    public string itemType { get; set; }   //
    public int itemLevel { get; set; }
}

//as dungeon progress
class twgDungeon {
}

class twgAccount
{
    [PrimaryKey] // important for performance: O(log n) instead of O(n)
    public string name { get; set; }
    public string password { get; set; }
    // created & lastlogin for statistics like CCU/MAU/registrations/...
    public DateTime created { get; set; }
    public DateTime lastlogin { get; set; }
    public bool banned { get; set; }
}

class twgcharacter_dialoguedata
{
    [PrimaryKey]
    public string character { get; set; }
    [Indexed]
    public string data { get; set; }
}



#endregion
//GAME DATABSE STORGE GAMES DATA INCLUDE
//Players & ACCOUNT ->CREATE & LOADSAVE &LOGIN
//CHARACTER PO DATA INCLUDES (INVENTORY & EQUIPMENT)
//LOAD&SAVE Character
public class DatabaseTHDN:MonoBehaviour{
    public static DatabaseTHDN instance;

    //DB file storge at applicationpath
    public string dbFiles = "THDN.sqlite";
    public NetworkManagerTHDN nettmanager;

    public SQLiteConnection sqlConect;

    void Awake()
    {
        if(instance=null)instance=this;

        //ADD HOOKS
    }

   public void Connect(){
        //Create File
        Debug.Log("Connect DB");
        string path = Path.Combine(Directory.GetParent(Application.dataPath).FullName,dbFiles);
        Debug.Log("Path"+path.ToString());
      
          Debug.Log("Connect");
         sqlConect = new SQLiteConnection(path);


          Debug.Log("create table");
        // create tables if they don't exist yet or were deleted
        sqlConect.CreateTable<accounts>();
        sqlConect.CreateTable<characters>();
        sqlConect.CreateTable<character_inventory>();
        sqlConect.CreateIndex(nameof(character_inventory), new[] {"character", "slot"});
        sqlConect.CreateTable<character_equipment>();
        sqlConect.CreateIndex(nameof(character_equipment), new[] {"character", "slot"});
        sqlConect.CreateTable<character_skills>();
        sqlConect.CreateIndex(nameof(character_skills), new[] {"character", "name"});
        //sqlConect.CreateTable<character_buffs>();
        //sqlConect.CreateIndex(nameof(character_buffs), new[] {"character", "name"});
        //TODO
        sqlConect.CreateTable<character_dungeon>();
      
        sqlConect.CreateTable<character_dialoguedata>();
        

        // addon system hooks
        Util.InvokeMany(typeof(DatabaseTHDN), this, "Initialize_"); // TODO remove later. let's keep the old hook for a while to not break every single addon!
        Util.InvokeMany(typeof(DatabaseTHDN), this, "Connect_"); // the new hook!

        Debug.Log("connected to database");
    }

   void OnApplicationQuit()
    {
        sqlConect?.Close();
    }

    // account data ////////////////////////////////////////////////////////////
    // try to log in with an account.
    // -> not called 'CheckAccount' or 'IsValidAccount' because it both checks
    //    if the account is valid AND sets the lastlogin field
    public bool TryLogin(string account, string password)
    {
        // this function can be used to verify account credentials in a DatabaseTHDN
        // or a content management system.
        //
        // for example, we could setup a content management system with a forum,
        // news, shop etc. and then use a simple HTTP-GET to check the account
        // info, for example:
        //
        //   var request = new WWW("example.com/verify.php?id="+id+"&amp;pw="+pw);
        //   while (!request.isDone)
        //       print("loading...");
        //   return request.error == null && request.text == "ok";
        //
        // where verify.php is a script like this one:
        //   <?php
        //   // id and pw set with HTTP-GET?
        //   if (isset($_GET['id']) && isset($_GET['pw'])) {
        //       // validate id and pw by using the CMS, for example in Drupal:
        //       if (user_authenticate($_GET['id'], $_GET['pw']))
        //           echo "ok";
        //       else
        //           echo "invalid id or pw";
        //   }
        //   ?>
        //
        // or we could check in a MYSQL DatabaseTHDN:
        //   var dbConn = new MySql.Data.MySqlClient.MySqlsqlConect("Persist Security Info=False;server=localhost;DatabaseTHDN=notas;uid=root;password=" + dbpwd);
        //   var cmd = dbConn.CreateCommand();
        //   cmd.CommandText = "SELECT id FROM Account WHERE id='" + account + "' AND pw='" + password + "'";
        //   dbConn.Open();
        //   var reader = cmd.ExecuteReader();
        //   if (reader.Read())
        //       return reader.ToString() == account;
        //   return false;
        //
        // as usual, we will use the simplest solution possible:
        // create account if not exists, compare password otherwise.
        // no CMS communication necessary and good enough for an Indie MMORPG.
        print("Try Login "+account);
        // not empty?
        if (!string.IsNullOrWhiteSpace(account) && !string.IsNullOrWhiteSpace(password))
        {
            print("Try Login module"+account);
            // demo feature: create account if it doesn't exist yet.
            // note: sqlite-net has no InsertOrIgnore so we do it in two steps
            print("create account if null");
            if (sqlConect.FindWithQuery<accounts>("SELECT * FROM accounts WHERE name=?", account) == null)
                sqlConect.Insert(new accounts{ name=account, password=password});

            // check account name, password, banned status
            if (sqlConect.FindWithQuery<accounts>("SELECT * FROM accounts WHERE name=? AND password=? and banned=0", account, password) != null)
            {
                // save last login time and return true
                sqlConect.Execute("UPDATE accounts SET lastlogin=? WHERE name=?", DateTime.UtcNow, account);
                return true;
            }
        }
        return false;
    }

   

    // character data //////////////////////////////////////////////////////////
    public bool CharacterExists(string characterName)
    {
        // checks deleted ones too so we don't end up with duplicates if we un-
        // delete one
        return sqlConect.FindWithQuery<characters>("SELECT * FROM characters WHERE name=?", characterName) != null;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="characterName"></param>
    public void CharacterDelete(string characterName)
    {
        // soft delete the character so it can always be restored later
        sqlConect.Execute("UPDATE characters SET deleted=1 WHERE name=?", characterName);
    }

    // returns the list of character names for that account
    // => all the other values can be read with CharacterLoad!
    public List<string> CharacterForAccount(string account)
    {
        List<string> result = new List<string>();
        foreach (characters character in sqlConect.Query<characters>("SELECT * FROM characters WHERE account=? AND deleted=0", account))
            result.Add(character.name);
        return result;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="ps"></param>
    public void CharacterLoad_ds(Players ps)
    {
        foreach (character_dialoguedata r in sqlConect.Query<character_dialoguedata>("SELECT * FROM character_dialoguedata WHERE character=?",ps.name))
        {
            ps.dialogueData = r.data;

        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p"></param>
    public void CharacterSave_ds(Players p)
    {
        sqlConect.Execute("DELETE FROM character_dialoguedata WHERE character=?",p.name);
        sqlConect.InsertOrReplace(new character_dialoguedata()
        {
            character = p.name,
            data = p.dialogueData,
        });
    }

    /// <summary>
    /// TODO 127 load item from db und set to invector-> inventory 
    /// </summary>
    /// <param name="Players"></param>
    // void LoadInventory(Players Players)
    // {

    //     //default is 30 can extra by backage
    //     Debug.Log(" =======>Character Inventory Data Load ");
    //     for (int i = 0; i < Players.inventorySize; i++)
    //     {
    //         Players.inventory.Add(new vItemSlot());
            
    //     }
    //     //
    //     foreach (character_inventory row in sqlConect.Query<character_inventory>(
    //         "SELECT *FROM character_inventory WHERE name=? ", Players.name))
    //     {
    //         //
    //         if (row.slot < Players.inventorySize)
    //         {
    //             //has slot then load
    //             if (vItem.dict.TryGetValue(row.name.GetStableHashCode(), out vItem itemData))
    //             {
    //                 //syncList -> got items 
    //                 ItemReference item =new ItemReference();
                
    //                 item.name = row.name;
    //                 item.itemNum = row.amount;
    //                 item.itemType =row.itemType;
    //                 item.stackSize = row.itemStacksize;
    //                 Players.inventory[row.slot]=new vItemSlot(item,row.itemStacksize);
    //             }
    //             else
    //             {
    //                 Debug.Log("LoadInventory Failed");
    //             }
    //         }
    //         else
    //         {
    //             Debug.Log("LoadInventory Slot");
    //         }
    //     }
        

    // }

    public ItemType ConvertItemType(string n) {
        if (n == "CItemTips") { return ItemType.CItemTips; }
        else if (n == "TrapTips") { return ItemType.TrapTips; }
        else if (n == "WEAPON") { return ItemType.WEAPON; }
        else if (n == "ARMOR") { return ItemType.ARMOR; }
        else if (n == "SI") { return ItemType.SI; }
        else if (n == "Potion") { return ItemType.Potion; }
        else if (n == "KeyStone") { return ItemType.KeyStone; }
        else if (n == "Hats") { return ItemType.Hats; }
        return ItemType.JUNK;
    }

    //public string ConvertItemType(ItemType n)
    //{
    //    if (n == "CItemTips") { return ItemType.CItemTips; }
    //    else if (n == "TrapTips") { return ItemType.TrapTips; }
    //    else if (n == "WEAPON") { return ItemType.WEAPON; }
    //    else if (n == "ARMOR") { return ItemType.ARMOR; }
    //    else if (n == "SI") { return ItemType.SI; }
    //    else if (n == "Potion") { return ItemType.Potion; }
    //    else if (n == "KeyStone") { return ItemType.KeyStone; }
    //    else if (n == "Hats") { return ItemType.Hats; }
    //    return ItemType.JUNK;
    //}


    // void LoadEquipment(Players Players)
    // {
        
    //     Debug.Log("Load Equipment DB");
    //     for (int i = 0; i < Players.equipmentInfos.Length; i++)
    //     {
    //         Players.equipment.Add(new vItemSlot());
    //     }
    //     foreach (character_equipment row in sqlConect.Query<character_equipment>("SELECT * FROM character_equipment WHERE character=?",
    //         Players.name))
    //     {
            
    //        if(vItem.dict.TryGetValue(row.name.GetStableHashCode(),out vItem itemdata))
    //        {
    //             //
    //            ItemReference item = new ItemReference(itemdata);
                
    //             //item.itemHP = row.itemHP;
    //             //item.itemMP = row.itemMP;


    //             ////
    //             //item.damage = row.damage;
    //             //item.armor = row.armor;
    //             //item.aShield = row.aShield;
    //             ////
    //             //item.blockPerc = row.blockPerc;
    //             //item.flashPerc = row.flashPerc;
    //             //item.critPerc = row.critPerc;


    //             Players.equipment[row.slot]=new vItemSlot(item,row.itemStacksize);
    //        }
    //     }
    // }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="player"></param>
    void LoadSkills(Players player)
    {
        // load skills based on skill templates (the others don't matter)
        // -> this way any skill changes in a prefab will be applied
        //    to all existing players every time (unlike item templates
        //    which are only for newly created characters)

        // fill all slots first
        foreach (ScriptableSkill skillData in player.skillTemplates)
            player.skills.Add(new Skill(skillData));

        // then load learned skills and put into their slots
        // (one big query is A LOT faster than querying each slot separately)
        foreach (character_skills row in sqlConect.Query<character_skills>("SELECT * FROM character_skills WHERE character=?", player.name))
        {
            int index = player.skills.FindIndex(skill => skill.name == row.name);
            if (index != -1)
            {
                Skill skill = player.skills[index]; 
                // make sure that 1 <= level <= maxlevel (in case we removed a skill
                // level etc)
                skill.level = Mathf.Clamp(row.level, 0, skill.maxLevel);
                // make sure that 1 <= level <= maxlevel (in case we removed a skill
                // level etc)
                // castTimeEnd and cooldownEnd are based on NetworkTime.time
                // which will be different when restarting a server, hence why
                // we saved them as just the remaining times. so let's convert
                // them back again.
                skill.castTimeEnd = row.castTimeEnd + NetworkTime.time;
                skill.cooldownEnd = row.cooldownEnd + NetworkTime.time;

                //damage bouns
                skill.amount = row.sAmount;
                //TODO
                skill.cooldownEnd = row.cooldownEnd;
                // skill.speed = row.sp;
                skill.amount = row.sAmount;
                // skill.sRageBouns = row.sRageBouns;
                // skill.sPerc = row.sPerc;
                //
                

                skill.hasLearn = row.hasLearn;
                //
                player.skills[index] = skill;
            }
        }
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="player"></param>
    void LoadBuffs(Players player)
    {
        // load buffs
        // note: no check if we have learned the skill for that buff
        //       since buffs may come from other people too
        foreach (character_buffs row in sqlConect.Query<character_buffs>("SELECT * FROM character_buffs WHERE character=?", player.name))
        {
            if (ScriptableSkill.dict.TryGetValue(row.name.GetStableHashCode(), out ScriptableSkill skillData))
            {
                // make sure that 1 <= level <= maxlevel (in case we removed a skill
                // level etc)
                int level = Mathf.Clamp(row.level, 1, skillData.maxLevel);
                Buffs buff = new Buffs((BuffSkill)skillData, level);
                // buffTimeEnd is based on NetworkTime.time, which will be
                // different when restarting a server, hence why we saved
                // them as just the remaining times. so let's convert them
                // back again.
                buff.buffTimeEnd = row.buffTimeEnd + NetworkTime.time;
                player.buffs.Add(buff);
            }
            else Debug.LogWarning("LoadBuffs: skipped buff " + row.name + " for " + player.name + " because it doesn't exist anymore. If it wasn't removed intentionally then make sure it's in the Resources folder.");
        }
    }



    //TODO 
    public GameObject CharacterLoad(string characterName, List<Players> prefabs, bool isPreview)
    {
        characters row = sqlConect.FindWithQuery<characters>("SELECT * FROM characters WHERE name=? AND deleted=0", characterName);
        if (row != null)
        {
            // instantiate based on the class name
            Players prefab = prefabs.Find(p => p.name == row.classname);
            if (prefab != null)
            {
                Debug.Log("Load Player");
                GameObject go = Instantiate(prefab.gameObject)as GameObject;
                Players players = go.GetComponent<Players>();
                players.racename = row.charactername;   //partyname
                players.name               = row.name;
                players.account            = row.account;
                players.className          = row.classname;
                players.racename           =row.charactername;
                Vector3 position = new Vector3(row.x, row.y, row.z);
                players.level = row.level;
                //players.races = row.races;
               //
                players.Stamina = row.cStamina;
                players.staminaRate = row.cStaminaRate;
                //pv
                players.health = row.health;
                players.healthRate =row.hpRate;
                //
                players.manaMax = row.mana;
                players.manaRate=row.manaRate;

              
                //ev
                players.kissass = row.kissassLevel;
  players.lockpick = row.lockpickingLevel;
  players.science = row.scienceLevel;
  players.dungeoneering = row.dungeoneeringLevel;
 players.leader = row.leaderLevel;

                //extra
                players.heavy = row.heavy;
                players.critPerc = row.critPerc;
                players.flashPerc = row.flashPerc;
                players.blockPerc = row.blockPerc;
                //party
                players.teamNum = row.teamNum;
                       players.bossName = row.bossName;
                       // players.bossName = row.bossName;
                        //players.hasSet = row.hasSet;
                        // players.setName = row.setName;
                //
                players.exp = row.exp;
                
                players.gold = row.money;

                //L3 Stats
                /*
                 public float CAnti { get; set; }
        public float CEagelEyes { get; set; }
        //KissAss
        public float CCheat { get; set; }
        public float CHardAss { get; set; }
        //Science
        public float CSoulView { get; set; }
        public float CAntiAb { get; set; }

        //Leader
        public float CBusiness { get; set; }
        public float CVRate { get; set; }
                */

                players.CAnti  = row.CAnti;
                players.CEagleEye = row.CEagleEyes;
                players.CCheat =row.CCheat;
                players.CHardAss = row.CHardAss;
                playerrs.CSoulView = row.CSoulView;
                players.CAntiAb = row.CAntiAb;
                players.CBusiness= row.CBusiness;
                players.CVRate=row.CVRate;
                //check ps
                if (PlayerPrefs.HasKey("_lastsavename"))
                {
                    string lsn = PlayerPrefs.GetString("_lastsavename");
                    players.lastSceneName = lsn;
                }
                else
                {
                    GameDebug.Log("Scene couldn't load in ps ===> set default scene");
                   players.lastSceneName = "SodtyxJails";
                }

                if(PlayerPrefs.HasKey("_charactername")){
                    string cname = PlayerPrefs.GetString("_charactername");
                    players.racename=cname;
                }else{
                    players.racename="Kuzo";
                }

                players.transform.position = new Vector3(0,180,0);

                // is the position on a navmesh?
                // it might not be if we changed the terrain, or if the Players
                // logged out in an instanced dungeon that doesn't exist anymore
                // if (NavMesh.SamplePosition(position, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
                // {
                //     // agent.warp is recommended over transform.position and
                //     // avoids all kinds of weird bugs
                //     players.agent.Warp(position);
                // }
                // // otherwise warp to start position
                // else
                // {
                //     Transform start = NetworkManagerTHDN.GetNearestStartPosition(position);
                //     players.agent.Warp(start.position);
                //     // no need to show the message all the time. it would spam
                //     // the server logs too much.
                //     //Debug.Log(Players.name + " spawn position reset because it's not on a NavMesh anymore. This can happen if the Players previously logged out in an instance or if the Terrain was changed.");
                // }

                Debug.Log("=======>Character Data Load Done ");
                //other
                // LoadInventory(players);
                // LoadEquipment(players);
                CharacterLoad_ds(players);
                //TODO
                // LoadDungeon(players);
                //loadBuff
                LoadSkills(players);
                // LoadBuffs(players);

                // LoadCharacterBuff(players);
//               
               
                // assign health / mana after max values were fully loaded
                // (they depend on equipment, buffs, etc.)
               

               
                // set 'online' directly. otherwise it would only be set during
                // the next Characterave() call, which might take 5-10 minutes.
                // => don't set it when loading previews though. only when
                //    really joining the world (hence setOnline flag)
                if (!isPreview)
                    sqlConect.Execute("UPDATE characters SET online=1, lastsaved=? WHERE name=?", DateTime.UtcNow, characterName);

                // addon system hooks
                Util.InvokeMany(typeof(DatabaseTHDN), this, "CharacterLoad_", players);

                return go;
            }
            else Debug.LogError("no prefab found for class: " + row.classname);
        }
        return null;
    }

    private void LoadCharacterBuff(Players players)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="Players"></param>
    // void SaveInventory(Players Players)
    // {
    //     // inventory: remove old entries first, then add all new ones
    //     // (we could use UPDATE where slot=... but deleting everything makes
    //     //  sure that there are never any ghosts)
    //     sqlConect.Execute("DELETE FROM character_inventory WHERE character =?", Players.name);
    //     for (int i = 0; i < Players.inventory.Count; i++)
    //     {
    //         vItemSlot slot = Players.inventory[i];
    //         //
    //         if (slot.amount > 0)
    //         {
    //             sqlConect.InsertOrReplace((new character_inventory
    //             {
    //                 character =  Players.name,
    //                 slot=i,
    //                 name=slot.item.name,
    //                 amount =slot.amount,    
    //                 //damage = slot.item.damage,
    //                 //armor =slot.item.armor,
    //                 //aShield =slot.item.aShield,

    //                 //itemHP =slot.item.itemHP,
    //                 //hpRate =slot.item.hpRate,
    //                 //itemMP =slot.item.itemMP,
    //                 //mpRate = slot.item.mpRate,

    //                 //blockPerc =slot.item.blockPerc,
    //                 //critPerc =slot.item.critPerc,
    //                 //flashPerc =slot.item.flashPerc,

    //                 pItemLevel = slot.item.itemLevel,

                    



    //             }));
                
    //         }
    //     }
    // }

    // void SaveEquipment(Players Players)
    // {
    //     // equipment: remove old entries first, then add all new ones
    //     // (we could use UPDATE where slot=... but deleting everything makes
    //     //  sure that there are never any ghosts)
    //     sqlConect.Execute("DELETE FROM character_equipment WHERE character=?", Players.name);
    //     for (int i = 0; i < Players.equipment.Count; ++i)
    //     {
    //             vItemSlot slot = Players.equipment[i];
    //         if (slot.amount > 0) // only relevant equip to save queries/storage/time
    //         {
    //             sqlConect.InsertOrReplace(new character_equipment{
    //                 character =  Players.name,
    //                 slot=i,
    //                 name=slot.item.name,
    //                 amount =slot.amount,
    //                 itemType = slot.item.itemType,

    //                 pItemLevel = slot.item.itemLevel

    //             });
    //         }
    //     }
    // }

    ///
    void SaveSkills(Players Players)
    {
        // skills: remove old entries first, then add all new ones
        sqlConect.Execute("DELETE FROM character_skills WHERE character=?", Players.name);
        foreach (ScriptableSkill skill in Players.skillTemplates)
            if (skill.level
                > 0) // only learned skills to save queries/storage/time
            {
                // castTimeEnd and cooldownEnd are based on NetworkTime.time,
                // which will be different when restarting the server, so let's
                // convert them to the remaining time for easier save & load
                // note: this does NOT work when trying to save character data
                //       shortly before closing the editor or game because
                //       NetworkTime.time is 0 then.
                sqlConect.InsertOrReplace(new character_skills {
                    character = Players.name,
                    name = skill.name,
                    level = skill.level,
                  
                    hasLearn = skill.hasLearn,
                    sAmount = skill.sAmount,
                    
                    castTimeEnd = skill.castTimeRemaining(),
                    cooldownEnd=skill.cooldownRemaining()

                }) ;
            }
    }


    /// <summary>
    /// TODO _Update Buff -> save amount
    /// </summary>
    /// <param name="Players"></param>
    void SaveBuffs(Players Players)
    {
        // buffs: remove old entries first, then add all new ones
        sqlConect.Execute("DELETE FROM character_buffs WHERE character=?", Players.name);
        foreach (Buffs buff in Players.buffs)
        {
            // buffTimeEnd is based on NetworkTime.time, which will be different
            // when restarting the server, so let's convert them to the
            // remaining time for easier save & load
            // note: this does NOT work when trying to save character data
            //       shortly before closing the editor or game because
            //       NetworkTime.time is 0 then.
            sqlConect.InsertOrReplace(new character_buffs
            {
                character = Players.name,
                name = buff.name,
                level = buff.level,
                buffTimeEnd = buff.BuffTimeRemaining()
            });
        }
    }



    // adds or overwrites character data in the DatabaseTHDN
    public void CharacterSave(Players Players, bool online, bool useTransaction = true)
    {
        // only use a transaction if not called within SaveMany transaction
        if (useTransaction) sqlConect.BeginTransaction();

        //Query DB for field
        sqlConect.InsertOrReplace(new characters
        {
            //TODO
            name = Players.name,
            account = Players.account,
            classname = Players.className,
            level = Players.level,
            //races = Players.races,
            //pos
            x = Players.transform.position.x,
            y = Players.transform.position.y,
            z = Players.transform.position.z,



            //bv1
            health = Players.health,
            hpRate = Players.healthRate,
            //
            mana = Players.manaMax,
            manaRate = Players.manaRate,
            //
            cStamina = Players.Stamina,
            cStaminaRate = Players.staminaRate,

            //sv
            damage = Players.damage,
            aShield = Players.aShield,

            //model data
            cst = Players.SprintStamina,
            cws = Players.WalkSpeed,
            crs = Players.RollSpeed,
            crd = Players.RollDistance,
            cjs = Players.JumpSpeed,
            cls = Players.LadderStamina,


            //ev
            kissassLevel = Players.kissass,
            lockpickingLevel = Players.lockpick,
            scienceLevel = Players.science,
            dungeoneeringLevel = Players.dungeoneering,
            leaderLevel = Players.leader,

            //extra 
            heavy = Players.heavy,
            critPerc = Players.critPerc,
            flashPerc = Players.flashPerc,
            blockPerc = Players.blockPerc,

            //party config
            teamNum = Players.teamNum,
            bossName = Players.bossName,


            exp = Players.exp,

            online = online,
            lastsaved = DateTime.Now,
            lastsavescenename = Players.lastSceneName,

            //achievement counter
            ac_killMonster = Players.AC_KillMonster,
            ac_exploreDungeon = Players.AC_ExploreDungeon,
            ac_useSkill = Players.AC_UseSkill,
            ac_criNum = Players.AC_CriNum,
            
            /*
              //
        public float CAnti { get; set; }
        public float CEagelEyes { get; set; }
        //KissAss
        public float CCheat { get; set; }
        public float CHardAss { get; set; }
        //Science
        public float CSoulView { get; set; }
        public float CAntiAb { get; set; }

        //Leader
        public float CBusiness { get; set; }
        public float CVRate { get; set; }
            */
            //L3 Stats
            CAnti = Players.CAnti,
            CEagleEyes =Players.CEagleEye,
            CCheat = Players.CCheat,
            CHardAss = Players.CHardAss,
            CSoulView = Players.CSoulView,
            CAntiAb = Players.CAntiAb,
            CBusiness  = Players. CBusiness,
            CVRate =Players.CvRate,


        }) ;
        //check last save

        if (!PlayerPrefs.HasKey("_lastsavename")) {
            string ls=PlayerPrefs.GetString("_lastsavename");
            Players.lastSceneName = ls;
        }
        else {
            PlayerPrefs.SetString("_lastsavename", Players.lastSceneName);
            Players.lastSceneName="SodtyxJails";
        }

        //

        

//
        // SaveInventory(Players);
        // SaveEquipment(Players);
        CharacterSave_ds(Players);

        //TODO
        SaveSkills(Players);
        // SaveBuffs(Players);

        //TODO
        SaveDungeon(Players);

        //       
        // addon system hooks
        Util.InvokeMany(typeof(DatabaseTHDN), this, "CharacterSave_", Players);

        if (useTransaction) sqlConect.Commit();
    }

   

    // save multiple Character at once (useful for ultra fast transactions)
    public void CharacteraveMany(IEnumerable<Players> Playerss, bool online = true)
    {
        sqlConect.BeginTransaction(); // transaction for performance
        foreach (Players Players in Playerss)
            CharacterSave(Players, online, false);
        sqlConect.Commit(); // end transaction
    }

    #region DungeonConfig
    /// <summary>
    /// 
    /// </summary>
    /// <param name="p"></param>
    public void SaveDungeon(Players p)
    {
        sqlConect.Execute("Delete from character_dungeon where character=?", p.name);
        if (p != null)
        {
            List<ScriptableDungeon> mapList = THDNGameDatabase.instance.dungeonList.ToList();
            if (mapList.Count
           > 0)
            {
                for (int i = 0; i < mapList.Count; i++)
                {
                   
                        sqlConect.InsertOrReplace(new character_dungeon()
                        {
                            dungeonName = mapList[i].name,
                            hasExplore = mapList[i].hasExplore,
                            unlock = mapList[i].unlockstate,
                           
                        });
                    
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p"></param>
    public void LoadDungeon(Players players)
    {

        //
        var mapList = TownManager.instance.mapList.ToList();

        if (mapList == null) { Debug.LogError("Can't load dungeon "); return; }
        // load buffs
        // note: no check if we have learned the skill for that buff
        //       since buffs may come from other people too
        foreach (character_dungeon row in sqlConect.Query<character_dungeon>("SELECT * FROM character_dungeon WHERE character=?", players.name))
        {
            //load gde data to resources then try got object from resources 
            if (ScriptableDungeon.dict.TryGetValue(row.dungeonName.GetStableHashCode(), out ScriptableDungeon dungeonData))
            {
                DungeonAsset dungeon = new DungeonAsset(dungeonData);
                dungeon.hasExplore = row.hasExplore;
                dungeon.name = row.dungeonName;
                dungeon.unlockstate = row.unlock;

                mapList.Add(dungeon);
            }
            else Debug.LogWarning("character_dungeon: skipped buff " + row.dungeonName + " for " + players.name + " because it doesn't exist anymore. If it wasn't removed intentionally then make sure it's in the Resources folder.");
        }
    }

    #endregion

    // guilds //////////////////////////////////////////////////////////////////
    // public bool GuildExists(string guild)
    // {
    //     return sqlConect.FindWithQuery<guild_info>("SELECT * FROM guild_info WHERE name=?", guild) != null;
    // }

    // Guild LoadGuild(string guildName)
    // {
    //     Guild guild = new Guild();

    //     // set name
    //     guild.name = guildName;

    //     // load guild info
    //     guild_info info = sqlConect.FindWithQuery<guild_info>("SELECT * FROM guild_info WHERE name=?", guildName);
    //     if (info != null)
    //     {
    //         guild.notice = info.notice;
    //     }

    //     // load members list
    //     List<character_guild> rows = sqlConect.Query<character_guild>("SELECT * FROM character_guild WHERE guild=?", guildName);
    //     GuildMember[] members = new GuildMember[rows.Count]; // avoid .ToList(). use array directly.
    //     for (int i = 0; i < rows.Count; ++i)
    //     {
    //         character_guild row = rows[i];

    //         GuildMember member = new GuildMember();
    //         member.name = row.character;
    //         member.rank = (GuildRank)row.rank;

    //         // is this Players online right now? then use runtime data
    //         if (Players.onlinePlayerss.TryGetValue(member.name, out Players Players))
    //         {
    //             member.online = true;
    //             member.level = Players.level;
    //         }
    //         else
    //         {
    //             member.online = false;
    //             // note: FindWithQuery<Character> is easier than ExecuteScalar<int> because we need the null check
    //             Character character = sqlConect.FindWithQuery<Character>("SELECT * FROM Character WHERE name=?", member.name);
    //             member.level = character != null ? character.level : 1;
    //         }

    //         members[i] = member;
    //     }
    //     guild.members = members;
    //     return guild;
    // }

    // public void SaveGuild(Guild guild, bool useTransaction = true)
    // {
    //     if (useTransaction) sqlConect.BeginTransaction(); // transaction for performance

    //     // guild info
    //     sqlConect.InsertOrReplace(new guild_info{
    //         name = guild.name,
    //         notice = guild.notice
    //     });

    //     // members list
    //     sqlConect.Execute("DELETE FROM character_guild WHERE guild=?", guild.name);
    //     foreach (GuildMember member in guild.members)
    //     {
    //         sqlConect.InsertOrReplace(new character_guild{
    //             character = member.name,
    //             guild = guild.name,
    //             rank = (int)member.rank
    //         });
    //     }

    //     if (useTransaction) sqlConect.Commit(); // end transaction
    // }

    // public void RemoveGuild(string guild)
    // {
    //     sqlConect.BeginTransaction(); // transaction for performance
    //     sqlConect.Execute("DELETE FROM guild_info WHERE name=?", guild);
    //     sqlConect.Execute("DELETE FROM character_guild WHERE guild=?", guild);
    //     sqlConect.Commit(); // end transaction
    // }

    // // item mall ///////////////////////////////////////////////////////////////
    // public List<long> GrabCharacterOrders(string characterName)
    // {
    //     grab new orders from the DatabaseTHDN and delete them immediately
        
    //     note: this requires an orderid if we want someone else to write to
    //     the DatabaseTHDN too. otherwise deleting would delete all the new ones or
    //     updating would update all the new ones. especially in sqlite.
        
    //     note: we could just delete processed orders, but keeping them in the
    //     DatabaseTHDN is easier for debugging / support.
    //     List<long> result = new List<long>();
    //     List<character_orders> rows = sqlConect.Query<character_orders>("SELECT * FROM character_orders WHERE character=? AND processed=0", characterName);
    //     foreach (character_orders row in rows)
    //     {
    //         result.Add(row.coins);
    //         sqlConect.Execute("UPDATE characters_orders SET processed=1 WHERE orderid=?", row.orderid);
    //     }
    //     return result;
    // }
}