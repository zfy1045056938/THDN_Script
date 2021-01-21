using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vItemManager;
using GameDataEditor;
using System.Linq;
public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;

    public List<vItem> itemList;
     public List<ScriptableSkill> skillsList;
    
    public List<ScriptableDungeon> dungeonList;
    public List<ScriptableCharacter> cList;
    public List<DungeonEvent> deList;
    public List<GamePiece> gpList;

    

      public static Dictionary<int, ScriptableSkill> skillDic = new Dictionary<int, ScriptableSkill>();


    //K,V->did,dungeon
    public static Dictionary<int, ScriptableDungeon> dunDic = new Dictionary<int, ScriptableDungeon>();

    //K,V->cid,entity
    public static Dictionary<int, ScriptableCharacter> charaDic = new Dictionary<int, ScriptableCharacter>();

    //K,V-> num,gp
    public static Dictionary<int, GamePiece> gpDic = new Dictionary<int, GamePiece>();   //for skill collections pools

    //ava dic
    public static Dictionary<string, Texture2D> avaDic = new Dictionary<string, Texture2D>();
    //

    public static Dictionary<int,vItem> itemDic = new Dictionary<int, vItem>();
    
    // Start is called before the first frame update
    void Start()
    {
    //   DontDestroyOnLoad(this.gameObject);
    Init();
    }

    

   public void Init(){
        Debug.Log("Init Item DB ");
        // foreach(var v in itemList){
        //     if(!itemDic.ContainsKey(v.itemID)){
        //         itemDic.Add(v.itemID,v);
        //     }
        // }
        //
        
    }

      public void GDSetDic()
    {
        //set data to dic
        //skill
        for(int i=0; i < skillsList.Count; i++)
        {
            if (!skillDic.ContainsKey(skillsList[i].sID))
            {
                skillDic.Add(skillsList[i].sID, skillsList[i]);
            }
        }

        //dungeon
        for (int i = 0; i < dungeonList.Count; i++)
        {
            if (!dunDic.ContainsKey(dungeonList[i].dungeonID))
            {
                dunDic.Add(dungeonList[i].dungeonID, dungeonList[i]);
            }
        }

        //de
        // for (int i = 0; i < deList.Count; i++)
        // {
        //    if (!deList.ContainsKey(deList[i].sID))
        //    {
        //        skillDic.Add(deList[i].sID, deList[i]);
        //    }
        // }

        //gps n,gp
        for (int i = 0; i < gpList.Count; i++)
        {
            int counter = 0;
            if (!gpDic.ContainsKey(gpList[i].gid))
            {
                gpDic.Add(counter, gpList[i]);
            }
        }

        //characters
        for (int i = 0; i < cList.Count; i++)
        {
            if (!charaDic.ContainsKey(cList[i].cid))
            {
                charaDic.Add(cList[i].cid, cList[i]);
            }
        }


    }

    public void LoadDungeon(){
	List<GDEDungeonData> gde = GDEDataManager.GetAllItems<GDEDungeonData>();

		//try got every item data at xlxs 
		for (int i = 0; i < gde.Count; i++)
		{
			GDEDungeonData dd = new GDEDungeonData(gde[i].Key);   //details

			//Create ScriptObject
			
			//Bind Data
			ScriptableDungeon s = new ScriptableDungeon()
			{
				dungeonID = dd.DungeonID,
				dungeonName = dd.DungeonName,
				// //
				dungeonType = Util.Convert_DungeonType(dd.DungeonType),
				//dungeonScene=dd.DSName,
				// dungeonLevel = dd.DungeonLevel,

            };

            dungeonList.Add(s);

        }

    }
    public void LoadDungeonEvent(){
    List<GDEDungeonEventData> gde = GDEDataManager.GetAllItems<GDEDungeonEventData>();

    // //try got every item data at xlxs 
    for (int i = 0; i < gde.Count; i++)
    {
        GDEDungeonEventData s = new GDEDungeonEventData(gde[i].Key);   //details

        //Bind Data
        DungeonEvent d = new DungeonEvent()
        {
            DeID = s.EventId,
            DeName = s.EventName,
            deDetail = s.EventDetail,
            dType = Util.Convert_DEType(s.EventType),
            Amount = s.EventAmount,

        };
        //
        deList.Add(d);
        
    }

    }
    public void AddSkill(ScriptableSkill s){
        if(!skillsList.Contains(s)){
skillsList.Add(s);
        }
       
    }
    public void AddDungeon(ScriptableDungeon s)
    {
        dungeonList.Add(s);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<ScriptableDungeon> GetAllMap()
    {
        List<ScriptableDungeon> d = new List<ScriptableDungeon>();
        //


        //
        return d;
    }


    public vItem GetItemByName(string name){
      vItem gotItem = itemList.Find(cItem=>cItem.name==name);
      return gotItem;
    }

    public DungeonEvent GotDE(int index){
        return deList[index];
    }

    public ScriptableCharacter GetEntityById(string id){

        if(charaDic.ContainsKey(id.ToInt())){
            return charaDic[id.ToInt()];
        }
        return null;
    }


    /// <summary>
    /// got enemy by dic
    /// dungeon will generate by GDEDUngeon::enemylist(rnd)
    /// 
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public List<ScriptableCharacter> GetEnemyList(List<string> c)
    {
        List<ScriptableCharacter> cl = new List<ScriptableCharacter>();
        //Contains key
        for(int i = 0; i < c.Count; i++)
        {
            //check contains key(must have else error)
            if (charaDic.ContainsKey(c[i].ToInt()))
            {
               
                //add to list
                cl.Add(charaDic[c[i].ToInt()]);
            }
            else
            {
                //check the db::clist contains ckey

            }
        }

        return cl;
    }

    public DungeonEvent GotDEByIndex(int index){
     return deList[index];   
    }
}
