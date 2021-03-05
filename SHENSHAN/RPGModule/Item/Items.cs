using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Items
{
   
    public string itemName;
    public string eitemName;
  public string itemID;
  public int width;
  public int height;
  public int sellPrice;
  public int buyPrice;
  public bool stackable;
  public int stackSize;
  public int maxStackSize;
  public int weaponDur;
  public int armorDur;
  public int ringDur;
  public string descriptionText;
  public string eItemDetail;
  public Sprite icon;
  public string iconName;
  public int itemLevel;

    [Header("Weapon")]
    //public float strength;
    //public float magic;
    //public float dexterity;
//    [Range(0,10)]
//  public float minStrength;
//  [Range(0,10)]
//public float maxStrength;
//[Range(0, 10)]
//    public float minMagic;
//    [Range(0,10)]
//    public float maxMagic;
//    [Range(0, 10)]
//    public float minDexterity;
//    [Range(0,10)]
//    public float maxDexterity;
    [Range(0, 100)]
    public int itemHeavy;
  
    
    /////////////////////////////////
    //////////////////ENUM FOR ITEM
    /////////////////////////////////
    ///
    //稀有度
    public ItemRatity itemRatity = ItemRatity.Normal;
    //装备栏类型
    public EquipmentSlotType equipmentSlotype ;
    //物品类型
    public EquipmentSlotType itemType ;
    //武器类型
    public WeaponType weaponType ;
    //消耗品类型
    public ConsumableType consumableType = ConsumableType.None;
    //
    public ArmorType armorType ;
    //
    public OtherType otherType = OtherType.None;
    
    //是否鉴定
  public bool unidentified;

  [Header("Check Item Property")]
    public bool isCraft;
    public bool isPotion;
    public bool isEquipment;
    public bool canDrop;     //Mission Item or some special item
    public bool canSplit;
    public bool hasSet;
   
    #region 抗性
   public int minFireRes;
   public int maxFireRes;
   public int minIceRes;
   public int maxIceRes;
   public int minElecRes;
   public int maxElecRes;
   public int minPhyRes;
   public int maxPhyRes;
   public int minPosionRes;
   public int maxPosionRes;
    public int fireResistance;
    public int iceResistance;
    public int posionResistance;
    public int electronicResistance;
    //public int phyicsResistance;
    #endregion

    //护甲属性
    public float armor;
    public float minArmor;
    public float maxArmor;
    public int totalRepairCost;
    public int repairNumber;
    public int minBlockChance;
    public int maxBlockChance;
    public int minBlockAmount;
    public int maxBlockAmount;
    
    
    public string tooltipHeader;
    
    //Damage
    
    public float minDamage;
    public float maxDamage;
    public float damage ;
    public bool twoHanded;
   
    //
    public int score;
  
  public List<string> setDetail;
  public List<string> setEquipmentName;
  public string setName;

    //Potion 药水属性
    public float healAmount;    //Potion Heal Amount
    public string itemSpecialName;
    public string armorEffectScriptName;
    public string useEffectScriptName;
    public HealType healType;
    public int cooldown;
    public int itemMana;
    public float perc; //爆率
    public GameObject worldObject;
    public SocketType socketType;

    public float minEffectAmount;
    public float maxEffectAmount;

    
    //SAB
    public string sabNames;
    
    
    
    //Gem was extra fot equipment
    public int gemLimit;
    public int gemSlotNumber;
  
    public DamageElementalType det;   //For Gem Effect
    public int useEffectAmount;

    public TargetOptions spellTarget;
    //seriesItem
 
    public int counter;
    public List<string> setList;

    // public string iconName;
    public int setNum;
    //common
    public bool useBattle;
    public string covName;
    public string effectCardName;
    
    public Items(string itemName, string itemId, int width, int height, string iconName)
    {
        this.itemName = itemName;
        itemID = itemId;
        this.width = width;
        this.height = height;
        this.icon.name = iconName;
    }

    public Items (string itemName){
      this.itemName=itemName;
    }

    public Items(Items item)
    {
      item = ItemDatabase.instance.FindItem(int.Parse(item.itemID));
    }
public Items(){}
}
