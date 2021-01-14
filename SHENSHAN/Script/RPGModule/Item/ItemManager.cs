using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class ItemManager
{
      public string itemName;
  public string itemID;
  public int width;
  public int height;
  public int sellPrice;
  public int buyPrice;
  public bool stackable;
  public int stackSize;
  public int maxStackSize;
  public int Durability;
  public string descriptionText;
  public Sprite icon;
  public int itemLevel;

    [Header("Weapon")]
    public float strength;
    public float magic;
    public float dexterity;
    [Range(0,10)]
  public float minStrength;
  [Range(0,10)]
public float maxStrength;
[Range(0, 10)]
    public float minMagic;
    [Range(0,10)]
    public float maxMagic;
    [Range(0, 10)]
    public float minDexterity;
    [Range(0,10)]
    public float maxDexterity;
    [Range(0, 100)]
    public int itemHeavy;
    //耐久度
    public float curDurability;
    public float maxDurability;
    
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
    

    #region 抗性
    public int minfireResistance;
    public int miniceResistance;
    public int minposionResistance;
    public int minelectronicResistance;
    public int minphyicsResistance;
    public int maxfireResistance;
    public int maxiceResistance;
    public int maxposionResistance;
    public int maxelectronicResistance;
    public int maxphyicsResistance;
    public int fireResistance;
    public int iceResistance;
    public int posionResistance;
    public int electronicResistance;
    public int phyicsResistance;
    #endregion

    //护甲属性
    public float armor;
    public float minArmor;
    public float maxArmor;
    public int totalRepairCost;
    public int repairNumber;
    
    
    public string tooltipHeader;
    
    //Damage
    
    public float minDamage;
    public float maxDamage;
    public float damage ;
    public bool twoHanded;
   
    


    //Potion 药水属性
    public int healAmount { get; set; }    //Potion Heal Amount
    public string itemSpecialName { get; set; }
    public string armorEffectScriptName { get; set; }
    public string useEffectScriptName { get; set; }
    public int cooldown;

}
