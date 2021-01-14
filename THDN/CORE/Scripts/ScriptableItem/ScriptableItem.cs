using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Mirror;
using GameDataEditor;

//Item base Classes
//in THDN include these variable
//THDN in dungeon explore  can find item who needs , items include equipemtn
//potion und other special items,when player in dungeon can equip or us item 
[CreateAssetMenu(menuName="THDNITEM/General Item",order = 999)]
public partial class ScriptableItem:ScriptableObjectNonAlloc{

    //Variable Common for all item
    public string itemName;

    public ItemType itemType;

    public int stackSize;
    public int itemSell;
      public bool canSellable;
    public bool canRepairable;
    public bool canDestorable;
    public Sprite sprite;
    
    //tooltip
    [SerializeField,TextArea(1,30)]
    protected   string tooltip  ;
    private static Dictionary<int, ScriptableItem> cache;
    public int itemID;
    public string itemDetail;
    public ItemRatity itemRarity;
    public int itemLevel;
    public bool canPU;
    public int maxItemLevel;
    public float itemHP;
    public float itemeMP;
    public float itemDamage;
    //public float itemArmor;
    public float itemAShield;
    //public float critPerc;
    //public float blockPerc;
    //public float flashPerc;
    public float itemPrice;
    public bool stackable;
   
    public string itemScript;

    public static Dictionary<int,ScriptableItem> dict{
        get{
            if (cache == null)
            {
              //
             ScriptableItem[] items = Resources.LoadAll<ScriptableItem>("");
                //
                if (items != null)
                {
                    //create instance for item
                    List<string> dup = items.ToList().FindDup(item => item.name);
                    
                    //to dic
                    if (dup.Count == 0)
                    {
                        cache = items.ToDictionary(item => item.name.GetStableHashCode(), item=>item);
                    }
                    
                }
            }
            return cache;
        }
    }


    
    //default item in inventory
    [System.Serializable]
    public struct ScriptObjectItemUndAmount
    {
        public ScriptableItem item;
        public int amount;
    }
        
}