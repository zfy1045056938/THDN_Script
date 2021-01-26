using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;
using System;
using System.IO;
using System.Linq;
using Mirror.Examples.Pong;

public partial class ShenShanDB : MonoBehaviour
{
   // singleton for easier access
    public static ShenShanDB singleton;

    // file name
    public string databaseFile = "";

    // connection
    SQLiteConnection connection;

    // database layout via .NET classes:
    // https://github.com/praeclarum/sqlite-net/wiki/GettingStarted
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
    class characters
    {
        [PrimaryKey] // important for performance: O(log n) instead of O(n)
        [Collation("NOCASE")] // [COLLATE NOCASE for case insensitive compare. this way we can't both create 'Archer' and 'archer' as characters]
        public string name { get; set; }
        [Indexed] // add index on account to avoid full scans when loading characters
        public string account { get; set; }
        public string classname { get; set; } // 'class' isn't available in C#
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public int level { get; set; }
        public int health { get; set; }
        public int mana { get; set; }
        //public int strength { get; set; }
        //public int intelligence { get; set; }
        public int extraSpellDamage { get; set; }
        public float esdPerc { get; set; }
        //public int dex { get; set; }
        public float flashPerc { get; set; }
        public int damage { get; set; }
        public int damageDur { get; set; }
        public int  armor { get; set; }
        public int armorDur { get; set; }
        public int Fr { get; set; }
        public int IR { get; set; }
        public int POR { get; set; }
        public int ER { get; set; }
        public float experience { get; set; } // TODO does long work?
        public float skillExperience { get; set; } // TODO does long work?
        public int gold { get; set; } // TODO does long work?
        public int coins { get; set; } // TODO does long work?

        public int dust { get; set; }
        public int specials { get; set; }
        // online status can be checked from external programs with either just
        // just 'online', or 'online && (DateTime.UtcNow - lastsaved) <= 1min)
        // which is robust to server crashes too.

        public bool hasSet { get; set; }
        public bool online { get; set; }
        public DateTime lastsaved { get; set; }
        public bool deleted { get; set; }
    }
    class character_inventory
    {
        public string character { get; set; }
        public int slot { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        
        public string itemName { get; set; }
        public int amount { get; set; }
      
        // PRIMARY KEY (character, slot) is created manually.
    }
    class character_equipment  // same layout
    {
       public string character{get;set;}
       public int slot{get;set;}
       public string itemName { get; set; }
    }

    class DeckCollection
    {
        public string character { get; set; }
        public string  deckName { get; set; }
        public string deckHero { get; set; }
    }

    class CardCollection
    {
        public string character { get; set; }
        public string cardName { get; set; }
        public int cardNumber { get; set; }
        public bool hasCollect { get; set; }
       
    }

    class character_dialoguesystem
    {
    [PrimaryKey]
        public string character { get; set; }
    [Indexed]
        public string data { get; set; }
    }
    class character_skills
    {
        public string character { get; set; }
        public string name { get; set; }
        public int level { get; set; }
        public float castTimeEnd { get; set; }
        public float cooldownEnd { get; set; }
        // PRIMARY KEY (character, name) is created manually.
    }
    class character_buffs
    {
        public string character { get; set; }
        public string name { get; set; }
        public int level { get; set; }
        public float buffTimeEnd { get; set; }
        // PRIMARY KEY (character, name) is created manually.
    }
    class character_quests
    {
        public string character { get; set; }
        public string name { get; set; }
        public int progress { get; set; }
        public bool completed { get; set; }
        // PRIMARY KEY (character, name) is created manually.
    }
    class character_orders
    {
        // INTEGER PRIMARY KEY is auto incremented by sqlite if the insert call
        // passes NULL for it.
        [PrimaryKey] // important for performance: O(log n) instead of O(n)
        public int orderid { get; set; }
        public string character { get; set; }
        public long coins { get; set; }
        public bool processed { get; set; }
    }
    class character_guild
    {
        // guild members are saved in a separate table because instead of in a
        // characters.guild field because:
        // * guilds need to be resaved independently, not just in CharacterSave
        // * kicked members' guilds are cleared automatically because we drop
        //   and then insert all members each time. otherwise we'd have to
        //   update the kicked member's guild field manually each time
        // * it's easier to remove / modify the guild feature if it's not hard-
        //   coded into the characters table
        [PrimaryKey] // important for performance: O(log n) instead of O(n)
        public string character { get; set; }
        // add index on guild to avoid full scans when loading guild members
        [Indexed]
        public string guild { get; set; }
        public int rank { get; set; }
    }
    class guild_info
    {
        // guild master is not in guild_info in case we need more than one later
        [PrimaryKey] // important for performance: O(log n) instead of O(n)
        public string name { get; set; }
        public string notice { get; set; }
    }

    void Awake()
    {
        // initialize singleton
        if (singleton == null) singleton = this;
    }

    // connect /////////////////////////////////////////////////////////////////
    // only call this from the server, not from the client. otherwise the client
    // would create a database file / webgl would throw errors, etc.
    public void Connect()
    {
        // database path: Application.dataPath is always relative to the project,
        // but we don't want it inside the Assets folder in the Editor (git etc.),
        // instead we put it above that.
        // we also use Path.Combine for platform independent paths
        // and we need persistentDataPath on android
#if UNITY_EDITOR
        string path = Path.Combine(Directory.GetParent(Application.dataPath).FullName, databaseFile);
#elif UNITY_ANDROID
        string path = Path.Combine(Application.persistentDataPath, databaseFile);
#elif UNITY_IOS
        string path = Path.Combine(Application.persistentDataPath, databaseFile);
#else
        string path = Path.Combine(Application.dataPath, databaseFile);
#endif

        // open connection
        // note: automatically creates database file if not created yet
        connection = new SQLiteConnection(path);

        string test = "ss_d";
        Debug.Log(test.Substring(0,test.LastIndexOf("_")));

        // create tables if they don't exist yet or were deleted
        connection.CreateTable<accounts>();
        connection.CreateTable<characters>();
        connection.CreateTable<character_inventory>();
        connection.CreateIndex(nameof(character_inventory), new []{"character", "slot"});
        connection.CreateTable<character_equipment>();
        connection.CreateIndex(nameof(character_equipment), new []{"character", "slot"});
        connection.CreateTable<DeckCollection>();
        connection.CreateIndex(nameof(DeckCollection), new[] { "character","deckName"});
         connection.CreateTable<CardCollection>();
                connection.CreateIndex(nameof(CardCollection), new[] { "character","cardName"});
                connection.CreateTable<DeckCollection>();
                connection.CreateIndex(nameof(DeckCollection), new[] { "character","deckName"});
                
                connection.CreateTable<character_skills>();
        connection.CreateIndex(nameof(character_skills), new []{"character", "name"});
        connection.CreateTable<character_buffs>();
        connection.CreateIndex(nameof(character_buffs), new []{"character", "name"});
        connection.CreateTable<character_quests>();
        connection.CreateIndex(nameof(character_quests), new []{"character", "name"});
        connection.CreateTable<character_dialoguesystem>();
        connection.CreateTable<character_orders>();
        connection.CreateTable<character_guild>();
        connection.CreateTable<guild_info>();

        // addon system hooks
        Utils.InvokeMany(typeof(ShenShanDB), this, "Initialize_"); // TODO remove later. let's keep the old hook for a while to not break every single addon!
        Utils.InvokeMany(typeof(ShenShanDB), this, "Connect_"); // the new hook!

        //Debug.Log("connected to database");
    }

    // close connection when Unity closes to prevent locking
    void OnApplicationQuit()
    {
        connection?.Close();
    }

    // account data ////////////////////////////////////////////////////////////
    // try to log in with an account.
    // -> not called 'CheckAccount' or 'IsValidAccount' because it both checks
    //    if the account is valid AND sets the lastlogin field
    public bool TryLogin(string account, string password)
    {
        // this function can be used to verify account credentials in a database
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
        // or we could check in a MYSQL database:
        //   var dbConn = new MySql.Data.MySqlClient.MySqlConnection("Persist Security Info=False;server=localhost;database=notas;uid=root;password=" + dbpwd);
        //   var cmd = dbConn.CreateCommand();
        //   cmd.CommandText = "SELECT id FROM accounts WHERE id='" + account + "' AND pw='" + password + "'";
        //   dbConn.Open();
        //   var reader = cmd.ExecuteReader();
        //   if (reader.Read())
        //       return reader.ToString() == account;
        //   return false;
        //
        // as usual, we will use the simplest solution possible:
        // create account if not exists, compare password otherwise.
        // no CMS communication necessary and good enough for an Indie MMORPG.

        // not empty?
        if (!string.IsNullOrWhiteSpace(account) && !string.IsNullOrWhiteSpace(password))
        {
            // demo feature: create account if it doesn't exist yet.
            // note: sqlite-net has no InsertOrIgnore so we do it in two steps
            if (connection.FindWithQuery<accounts>("SELECT * FROM accounts WHERE name=?", account) == null)
                connection.Insert(new accounts{ name=account, password=password, created=DateTime.UtcNow, lastlogin=DateTime.Now, banned=false});

            // check account name, password, banned status
            if (connection.FindWithQuery<accounts>("SELECT * FROM accounts WHERE name=? AND password=? and banned=0", account, password) != null)
            {
                // save last login time and return true
                connection.Execute("UPDATE accounts SET lastlogin=? WHERE name=?", DateTime.UtcNow, account);
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
        return connection.FindWithQuery<characters>("SELECT * FROM characters WHERE name=?", characterName) != null;
    }

    public void CharacterDelete(string characterName)
    {
        // soft delete the character so it can always be restored later
        connection.Execute("UPDATE characters SET deleted=1 WHERE name=?", characterName);
    }

    // returns the list of character names for that account
    // => all the other values can be read with CharacterLoad!
    public List<string> CharactersForAccount(string account)
    {
        List<string> result = new List<string>();
        foreach (characters character in connection.Query<characters>("SELECT * FROM characters WHERE account=? AND deleted=0", account))
            result.Add(character.name);
        return result;
    }

    public GameObject CharacterLoad(string characterName, List<PlayerData> prefabs, bool isPreview)
    {
        characters row = connection.FindWithQuery<characters>("SELECT * FROM characters WHERE name=? AND deleted=0", characterName);
        if (row != null)
        {
            // instantiate based on the class name
            PlayerData prefab = prefabs.Find(p => p.name == row.classname);
            if (prefab != null)
            {
                GameObject go = Instantiate(prefab.gameObject);
                PlayerData player = go.GetComponent<PlayerData>();

                player.name         = row.name;
                player.account            = row.account;
                player.className          = row.classname;
                Vector3 position          = new Vector3(row.x, row.y, row.z);
                player.PlayerLevel              = Mathf.Min(row.level, player.maxLevel); // limit to max level in case we changed it
                //player.Strength           = row.strength;
                player.extraFlash         = row.flashPerc;
                //player.Magic               = row.intelligence;
                player.extraSpellDamage    = row.extraSpellDamage;
                player.ESDPerc             = row.esdPerc;
                //player.Dex                 =row.dex;
                

                player.experience         = row.experience;
                player.skillExperience    = row.skillExperience;
                player.money               = row.gold;
                player.coins              = row.coins;
                player.dust = row.dust;
                player.special              =row.specials;
                player.atk = row.damage;
                player.atkCount = row.damageDur;
                player.ArmorDef = row.armor;
                player.ArmorDur = row.armorDur;
                player.FR = row.Fr;
                player.IR = row.IR;
                player.PR = row.POR;
                player.ER = row.ER;

                player.hasSet = row.hasSet;

//                LoadInventory(player);
//            
                LoadDialogueSystem(player);
        //test
//                player.money += 10000;
//                player.dust += 10000;
                
                // set 'online' directly. otherwise it would only be set during
                // the next CharacterSave() call, which might take 5-10 minutes.
                // => don't set it when loading previews though. only when
                //    really joining the world (hence setOnline flag)
                if (!isPreview)
                    connection.Execute("UPDATE characters SET online=1, lastsaved=? WHERE name=?", DateTime.UtcNow, characterName);

                // addon system hooks
                Utils.InvokeMany(typeof(ShenShanDB), this, "CharacterLoad_", player);

                return go;
            }
            else Debug.LogError("no prefab found for class: " + row.classname);
        }
        return null;
    }

    
    // adds or overwrites character data in the database
    public void CharacterSave(PlayerData player, bool online, bool useTransaction = true)
    {
        // only use a transaction if not called within SaveMany transaction
        if (useTransaction) connection.BeginTransaction();

        connection.InsertOrReplace(new characters{
            name = player.name,
            account = player.account,
            classname = player.className,
            x = player.transform.position.x,
            y = player.transform.position.y,
            z = player.transform.position.z,
            level = player.PlayerLevel,
            health = player.playerHealth,
            mana = player.mana,
            //strength = player.Strength,
            //intelligence = player.Magic,
            extraSpellDamage=player.extraSpellDamage,
            esdPerc = player.ESDPerc,
            //dex =player.Dex,
            flashPerc=player.extraFlash,
            damage = player.atk,
            damageDur = player.atkCount,
            armor =  player.ArmorDef,
            armorDur = player.ArmorDur,
            Fr = player.FR,
            IR =player.IR,
            POR = player.PR,
            ER = player.ER,
            experience = player.experience,
            skillExperience = player.skillExperience,
            gold = player.money,
            dust = player.dust,
            specials=player.special,
            coins = player.coins,
            hasSet=player.hasSet,
            online = online,
            lastsaved = DateTime.UtcNow
            
        });
        
        //Save Module
//            SaveInventory(player);
//            SaveEquipment(player);
//            SaveCardCollection(player);
//            SaveDeckCollection(player);
            
            SaveDialogueSystem(player);
            
        // addon system hooks
        Utils.InvokeMany(typeof(ShenShanDB), this, "CharacterSave_", player);

        if (useTransaction) connection.Commit();
    }

    // save multiple characters at once (useful for ultra fast transactions)
    public void CharacterSaveMany(IEnumerable<PlayerData> players, bool online = true)
    {
        connection.BeginTransaction(); // transaction for performance
        foreach (PlayerData player in players)
            CharacterSave(player, online, false);
        connection.Commit(); // end transaction
    }

    public void SaveEquipment(PlayerData p)
    {
        connection.Execute("DELETE FROM character_equipment WHERE character =?", p.name);
        for (int i = 0; i < p.equipSlot.Count; i++)
        {
            connection.InsertOrReplace(new character_equipment
            {
                character = p.name,
                itemName = p.equipSlot[i].item.itemName,
                slot = i,
            });
        }
    }

    public void SaveDeckCollection(PlayerData p)
    {
        
    }

    public void SaveCardCollection(PlayerData p)
    {
        
    }
    
    //for intventory
    public void SaveInventory(PlayerData p)
    {
        connection.Execute("DELETE FROM character_inventory WHERE character=?", p.name);
        
        //slot size(width*height)


//            connection.InsertOrReplace(new character_inventory
//            {
//                character = p.name,
//                slot = slot,
//                itemName = item.itemName,
//                width = item.width,
//                height = item.height,
//                amount = item.stackSize,
//                
//
//            });
//          
        

    }

    public void LoadInventory(PlayerData p)
    {

        foreach (character_inventory row in connection.Query<character_inventory>("SELECT * FROM character_inventory WHERE character =?",p.name))
        {
            for (int m = 0; m < p.inventorySlot.Count; m++)
            {
                p.inventorySlot = new List<InventorySlot>();
                if (row.slot < p.inventorySlot.Count)
                {
                    Items i = new Items(row.itemName);
                    i.width = row.width;
                    i.height = row.height;
                    i.stackSize = row.amount;

                    InventorySystem.instance.SetSlotImageSprite(p.inventorySlot[m],i.icon);
                }
            }
        }
    
    }

    public void LoadDialogueSystem(PlayerData p)
    {
        foreach (character_dialoguesystem row in connection.Query<character_dialoguesystem>(
            "SELECT * FROM character_dialoguesystem WHERE character=? ", p.name))
        {
            p.PlayerName = row.character;
            p.dialogueData = row.data;
        }
    }

    public void SaveDialogueSystem(PlayerData p)
    {
        connection.Execute("DELETE FROM character_dialoguesystem WHERE character=?", p.name);
        connection.InsertOrReplace(new character_dialoguesystem
        {
            character = p.name,
            data = p.dialogueData
        });
    }

}
