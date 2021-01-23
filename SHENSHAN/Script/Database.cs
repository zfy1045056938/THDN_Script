using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using System.IO;
using Mirror;
using Mono.Data.Sqlite;
using System.Linq;
using PixelCrushers;


public class Database : MonoBehaviour
{

    public static Database instance;
    public string databaseFile = "Database.sqlite";

    private SqliteConnection sqlConn;
    public List<SaveSlots> saveSlot;
    
    //Execute Query
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    /// <summary>
    /// Connect Database und Initialize Data
    /// </summary>
    public void Connect()
    {
        //Get DB path
        string path = Path.Combine(Directory.GetParent(Application.dataPath).FullName, databaseFile);

        //
        if (!File.Exists(path))
        {
            SqliteConnection.CreateFile(path);
            Debug.Log("Create New File");
        }
        
        sqlConn=new SqliteConnection("URI=file:"+path);
        sqlConn.Open();
        Debug.Log("Open Success");
        
        //////SQL QUERY EXECUTE/////

        //SET ACCOUNT INFO
      ExecuteNonQuery(@"CREATE  TABLE IF NOT EXISTS characters (
                            name TEXT NOT NULL PRIMARY KEY COLLATE NOCASE,
                            account TEXT NOT NULL,
                            
                            level INTEGER NOT NULL,
                            health INTEGER NOT NULL,
                            

                            strength INTEGER NOT NULL,
                            magic INTEGER NOT NULL,
                            dex INTEGER NOT NULL,

                            FR INTEGER NOT NULL,
                            IR INTEGER NOT NULL,
                            PR INTEGER NOT NULL,
                            PHR INTEGER NOT NULL,

                            WEAPON_ATK INTEGER NOT NULL,
                            WEAPON_COUNT INTEGER NOT NULL,
                            ARMOR_SUM INTEGER NOT NULL,
                            ARMOR_COUNT INTEGER NOT NULL,
            
                            experience INTEGER NOT NULL,
                            skillExperience INTEGER NOT NULL,
                            gold INTEGER NOT NULL,
                            coins INTEGER NOT NULL,
                            dust INTEGER NOT NULL,
                            special INTEGER NOT NULL,

                            online INTEGER NOT NULL,
                            lastsaved DATETIME NOT NULL,
                            deleted INTEGER NOT NULL)");
      
      //Account
      ExecuteNonQuery("CREATE INDEX IF NOT EXISTS characters_by_account ON characters (account)");
          ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS accounts (
                            name TEXT NOT NULL PRIMARY KEY,
                            password TEXT NOT NULL,
                            created DATETIME NOT NULL,
                            lastlogin DATETIME NOT NULL,
                            banned INTEGER NOT NULL)");
          
          //SET INVENTORY INFO
         ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS inventory(
             character TEXT NOT NULL,
             slot INTEGER NOT NULL,
             width INTEGER NOT NULL,
             height INTEGER NOT NULL,
            
            itemName TEXT NOT NULL,
            PRIMARY KEY(character,slot)
         )");
        //SET INVENTORY SLOT INFO
        //SET EQUIPMENT SLOT INFO
        //SET QUEST PROCESS
        //SET ACHIEVEMENT INFO
        //SET CARDASSET INFO
       
    
        //SET ITEMASSET INFO
        


        ///////SQL QUERY END////////
        //Utils
        Utils.InvokeMany(typeof(Database),this,"Initialize_");
        Utils.InvokeMany(typeof(Database),this,"Connect_");
        
        //
        Debug.Log("Connect DB Complete");
    }
    #region CoreExecute
    //Execute Query
    private void ExecuteNonQuery(string sql,params SqliteParameter[] args)
    {
        using (SqliteCommand command = new SqliteCommand(sql, sqlConn))
        { 
            foreach (SqliteParameter param in args)
                command.Parameters.Add(param);
            command.ExecuteNonQuery();
        }
    }

    //DB READ STORGE DATA
    private List<List<object>> ExecuteReader(string sql, params SqliteParameter[] conn)
    {
         List< List<object> > result = new List< List<object> >();
        
                using (SqliteCommand command = new SqliteCommand(sql, sqlConn))
                {
                    foreach (SqliteParameter param in conn)
                        command.Parameters.Add(param);
        
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        // the following code causes a SQL EntryPointNotFoundException
                        // because sqlite3_column_origin_name isn't found on OSX and
                        // some other platforms. newer mono versions have a workaround,
                        // but as long as Unity doesn't update, we will have to work
                        // around it manually. see also GetSchemaTable function:
                        // https://github.com/mono/mono/blob/master/mcs/class/Mono.Data.Sqlite/Mono.Data.Sqlite_2.0/SQLiteDataReader.cs
                        //
                        //result.Load(reader); (DataTable)
                        //
                        // UPDATE: DataTable.Load(reader) works in Net 4.X now, but it's
                        //         20x slower than the current approach.
                        //         select * from character_inventory x 1000:
                        //           425ms before
                        //          7303ms with DataRow
                        while (reader.Read())
                        {
                            object[] buffer = new object[reader.FieldCount];
                            reader.GetValues(buffer);
                            result.Add(buffer.ToList());
                        }
                    }
                }
        
                return result;
    }

    //return one value
    public object ExecuteScalar(string sql, params SqliteParameter[] args)
    {
        using (SqliteCommand command =new SqliteCommand(sql,sqlConn))
        {
            foreach (SqliteParameter p in args)
            
                command.Parameters.Add(p);
                return command.ExecuteScalar();
            
        }

       
    }
    #endregion
    
    public void CharacterSaveMany(IEnumerable<PlayerData> ps , bool online=true){
        ExecuteNonQuery("BEGIN");
        foreach(PlayerData pl in ps){
            CharacterSave(pl,online,false);
        }
        ExecuteNonQuery("END");
    }

    //Remove Query By msg
    public void CharacterDel(string v)
    {
        ExecuteNonQuery("UPDATE PlayerData SET deleted=1 WHERE name=@character",
        new SqliteParameter("@character",v));
    }

   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="msgName"></param>
    /// <returns></returns>
    public bool CharacterExists(string msgName)
    {
     return (long)ExecuteScalar("SELECT COUNT(*) FROM characters where name=@name",
                new SqliteParameter("name",msgName))==1;
    }

 
  public object CharacterForDateTime(string account,int str,int dex,int magic){
     return ExecuteReader("SELECT strength,dex,magic,lastsaved FROM characters where @name,@strength,@dex,@magic",
     new SqliteParameter("@name",account),
     new SqliteParameter("@strength",str),
     new SqliteParameter("@dex",dex),
     new SqliteParameter("@magic",magic));
  }

  //

    public void CharacterSave(PlayerData player, bool online,bool useTransaction=true){
            if(useTransaction) ExecuteNonQuery("BEGIN");
            ExecuteNonQuery(
                "INSERT OR REPLACE INTO characters VALUES(@name,@account,@level,@health,@strength,@magic,@dex,@FR,@IR,@PR,@PHR,@WEAPON_ATK,@WEAPON_COUNT,@ARMOR_SUM,@ARMOR_COUNT,@experience,@skillExperience,@gold,@coins,@dust,@special,@online,@lastsaved,0)",
            new SqliteParameter("@name",player.PlayerName),
            new SqliteParameter("@account",player.account),
            new SqliteParameter("@level",player.PlayerLevel),
            new SqliteParameter("@health",player.playerHealth),
                
            new SqliteParameter("@strength",player.Strength),
            new SqliteParameter("@magic",player.Magic),
                new SqliteParameter("@dex",player.Dex),
                
            new SqliteParameter("@FR",player.FR),
            new SqliteParameter("@IR",player.IR),
            new SqliteParameter("@PR",player.PR),
            new SqliteParameter("@PHR",player.PhyR),

            new SqliteParameter("@WEAPON_ATK",player.atk),
            new SqliteParameter("@WEAPON_COUNT",player.atkCount),
            new SqliteParameter("@ARMOR_SUM",player.ArmorDef),
            new SqliteParameter("@ARMOR_COUNT",player.ArmorDur),

            new SqliteParameter("@experience",player.experience),
            new SqliteParameter("@skillExperience",player.skillExperience),

            new SqliteParameter("@gold",player.money),
            new SqliteParameter("@coins",player.coins),
            new SqliteParameter("@dust",player.dust),
            new SqliteParameter("@special",player.special),

            new SqliteParameter("@online",player.online?1:0),
                new SqliteParameter("@lastsaved",DateTime.Now)
            );

            //
            // SaveInventory(player);
            //add on
            Utils.InvokeMany(typeof(Database),this,"CharacterSave_",player);
            
            
            //
            if(useTransaction) ExecuteNonQuery("END");
            
            Debug.Log("Character Save Success");
        
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public GameObject LoadCharacter(string character,List<PlayerData> prefabs,bool isPreview=false)
    {
        List<List<object>> table = ExecuteReader(" SELECT * FROM characters WHERE name=@name",
            new SqliteParameter("@name", character));
       
        if (table.Count==1)
        {
            List<object> mr = table[0];
            string pns = (string)mr[0];
            //Load Player Data From Database
                PlayerData prefab = FindObjectOfType<PlayerData>();

                if(prefab!=null){
                GameObject go = Instantiate(prefab.gameObject);
                PlayerData pd = go.GetComponent<PlayerData>();
                pd.PlayerName = (string) mr[0];
                pd.account = (string) mr[1];
                pd.PlayerLevel = Convert.ToInt32((long) mr[2]);
                pd.playerHealth = Convert.ToInt32((long) mr[3]);
                pd.Strength = Convert.ToInt32((long) mr[4]);
                pd.Magic = Convert.ToInt32((long) mr[5]);
                pd.Dex = Convert.ToInt32((long) mr[6]);
                //
                pd.FR = Convert.ToInt32((long) mr[7]);
                pd.IR = Convert.ToInt32((long) mr[8]);
                pd.PR = Convert.ToInt32((long) mr[9]);
                pd.PhyR = Convert.ToInt32((long) mr[10]);
                //
                pd.atk = Convert.ToInt32((long) mr[11]);
                pd.atkCount = Convert.ToInt32((long) mr[12]);
                pd.ArmorDef = Convert.ToInt32((long) mr[13]);
                pd.ArmorDef = Convert.ToInt32((long) mr[14]);
                //
                pd.experience = Convert.ToInt32((long) mr[15]);
                pd.skillExperience = Convert.ToInt32((long) mr[16]);

                pd.money = Convert.ToInt32((long) mr[17]);
                pd.coins = Convert.ToInt32((long) mr[18]);
                pd.dust = Convert.ToInt32((long)mr[19]);
                pd.special=Convert.ToInt32((long)mr[20]);

                if (!isPreview)
                    ExecuteNonQuery("UPDATE characters SET online=1, lastsaved=@lastsaved WHERE name=@name",
                        new SqliteParameter("@name", character), new SqliteParameter("@lastsaved", DateTime.Now));

                //
                // LoadInventory(prefab);

                Utils.InvokeMany(typeof(Database), "this", "CharacterLoad_", pd);

                Debug.Log("Load Character Done!!!!!");
                return go;
                }
        }

        return null;

    }

    public object AddItemToDatabase(Items item)
    {
        throw new NotImplementedException();
    }

    #region LoadPlayerFromDB
    private void LoadEquipment(PlayerData player)
    {
        throw new NotImplementedException();
    }

    //  character TEXT NOT NULL,
            //  slot INTEGER NOT NULL,
            //  width INTEGER NOT NULL,
            //  height INTEGER NOT NULL,
            // itemStartNumber INTEGER NOT NULL,
            // itemName TEXT NOT NULL,
            // PRIMARY KEY(character,slot)
    public void SaveInventory(PlayerData player,Items item){
        ExecuteNonQuery("DELETE FROM inventory WHERE character=@character ",new SqliteParameter("@character",player.PlayerName));
        //Save Inventory by slot
       
        for(int i=0;i<player.inventory.items.Count;i++){
            InventorySlot s = player.inventory.items[i];
            if(player.inventory.items[i].item.itemName!=""){
                //has item then save to inventory
                ExecuteNonQuery(@"INSERT INTO inventory VALUES(@character,@slot,@width,@height,@itemName)",
                new  SqliteParameter("@character",player.PlayerName),
                new SqliteParameter("@slot",i),
                new SqliteParameter("width",s.item.width),
                new SqliteParameter("height",s.item.height),
                new SqliteParameter("itemName",s.item.itemName)

                 );
            }
        }
    }
    private void LoadInventory(PlayerData player)
    {
        for(int i=0;i<player.inventory.items.Count;i++){
            List<List<object>> table= 
                ExecuteReader("SELECT itemName,slot,width,height FROM inventory where character=@name",new SqliteParameter("@name",player.PlayerName));
                //
                foreach(List<object> t in table){
                    Items s = player.inventory.items[i].item;
                        string name = (string)t[0];
                        int slot = Convert.ToInt32((long)t[1]);
                        s.width =Convert.ToInt32((string)t[2]);
                        s.height=Convert.ToInt32(t[3]);

                        

                }
        }
    }
    #endregion
    
    /// <summary>
    /// Check Account Exists
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public List<string> CharacterForAccount(string account)
    {
        List<List<object>> table =
            ExecuteReader("SELECT name From characters WHERE account =@name and deleted=0",
            new SqliteParameter("@name", account));
        return table.Select(row => (string) row[0]).ToList();
    }

   

/// <summary>
/// 
/// </summary>
/// <param name="account"></param>
/// <param name="pwd"></param>
/// <returns></returns>
    public bool TryLogin(string account, string pwd)
    {
        if (!string.IsNullOrWhiteSpace(account) && !string.IsNullOrWhiteSpace(pwd))
        {
            ExecuteNonQuery("INSERT OR IGNORE INTO accounts VALUES(@name,@password,@created,@created,0)",
                new SqliteParameter("@name", account), new SqliteParameter("@password", pwd),
                new SqliteParameter("@created", DateTime.UtcNow));
            
            //
            bool valid = 
            ((long)ExecuteScalar("SELECT Count(*) FROM accounts WHERE name=@name AND password=@password AND banned=0",
             new SqliteParameter("@name", account), new SqliteParameter("@password", pwd))) == 1;
            //
           if (valid)
            {
                // save last login time and return true
                ExecuteNonQuery("UPDATE accounts SET lastlogin=@lastlogin WHERE name=@name", 
                new SqliteParameter("@name", account), new SqliteParameter("@lastlogin", DateTime.Now));
                return true;
            }
        }

        return false;
    }


#region GetCashFromDB

public object GetMoney(string account)
{
    return ExecuteScalar("SELECT gold FROM characters WHERE name=@c",
        new SqliteParameter("@c", account)
    );
}
public object GEtCoins(string account)
{
    return ExecuteScalar("SELECT coins FROM characters WHERE name=@c",
        new SqliteParameter("@c", account)
    );
}
public object GetDust(string account)
{
    return   ExecuteScalar("SELECT dust FROM characters WHERE name=@c",
                   new SqliteParameter("@c", account)
               );;
}
public object GetSpec(string account)
{
    return ExecuteScalar("SELECT special FROM characters WHERE name=@c",
        new SqliteParameter("@c", account)
    );
}
#endregion
}
