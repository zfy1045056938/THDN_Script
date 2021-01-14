using UnityEngine;
using System.Collections;

/// <summary>
/// This class include all enum of game 
/// </summary>

public enum MainAtkType
{
    PHY,
    MAGIC,
    NONE,
    SPECIAL,
}

public enum CardRatityOption
{
    NORMAL,
    RARE,
    LEGEND,
    Epic,
    Ancient
}
public enum CardType
{
    None,
    Trap,
    Buff,
    Enviorment,
    Elements,
    Human,
    Grass,
    Machine,
    Fly,
    Ghost,
    St_Holy,
    JavaCafe,
    Building,
    Animals,
    Qika,
    Worker,
    SeaWheel,
    RuiKi,
    Kai,
}

//玩家基本职业划分
public enum PlayerJob
{
    None,       //Neu
    Magic,
    Hunter,
    Survicer,

}

/// <summary>
/// 卡牌词缀
/// </summary>
public enum CardEffects
{
    Rage,
    Hate,
    PointAt,
    CombineAtk,
    Fear,
    Planner,
    Lost,
    ToGhost,
    None,
}

/// <summary>
/// 随从类型
/// </summary>
public enum CreatureType
{
    Single,
    Group,
    None,

}


//攻击行为
public enum CardMotivion
{
    NONE,
    Self,
    Creature,
    Group,
    Character,


}


/// <summary>
/// 卡牌伤害效果
/// </summary>
public enum DamageEffects
{
    None,
    Explosion,
    Freeze,
    CrazyAtk,
    DeadPlace,
    HateAtk,
    RageAtk,
    PointAtk,
    //elements
    WaterPour,
    ElectronicRange,
    GhostAtk,
    Assiets,

}


///卡牌页面类型
public enum TypeOfCards
{
    Creature,
    Spell,
    Token,
}

public enum DamageElementalType
{
    None,
    Damage,
    Fire,
    Ice,
    Posion,
    Electronic,
    Bloody,
    GroupBloody,
    
    Freeze,
    Rage,
    IgnoreDamage,
    
    //atk buff
    CanAtk,
    MinusAtk,
    HeroDamage,
    HeroArmor,
    HeroHealth,
    //
    Notice,
    
    //
    HeroElec,
    //
    TimeBomb,
    ExtraSpell,

   

    DeadView,
    Token,
    Lock,
    //Absorb
    Absorb,
    AbsorbBurning,
AbsorbArmor,
AbsorbHaste,
AbsorbTreasure,
AbsorbPosion,
AbsorbFreeze,
AbsorbBloody,

}

//except for damage 
public enum SpellBuffType
{
    None,
    Health,
    Armor,
    CharacterArmor,
    // CHArmor,
    Atk,
    DoubleAtk,
    AtkDur,
    
    FireRes,
    IceRes,
    PosRes,
    PhyRes,
    ElecRes,
    
    STR,
    DEX,
    Flash,
    INT,
    ExtraSpell,
    
    Taunt,
    HurtTaunt,
    TableAmount,
    
    //group
    GroupAtk,
    Machine,
    Hyper,
    FireArmor,
    IceArmor,
    PosionArmor,
    ElecArmor,
    MachineArmor,
    MachineAtk,
    MachineHyper,
    MachineHeal,

    //
    SoulView,
    Haste,
    Clean,
    //
    Block,
    
}

/// <summary>
/// 指向目标,用于dragAction
/// </summary>
public enum TargetOptions
{
    None,
    AllCharacter,
    Creature,

    Worker,
    Castle,
    //敌方物体
    EmenyCharacter,
    EmenyCreature,
    EmenyCastle,
    //
    YoursCharacters,    //Check top n low character,when in pvp then check 

    AllCreature
}

/// <summary>
/// AreaPosition(low||top)
/// </summary>
public enum AreaPositions
{
    Low,
    Top,
}

//游戏设计的元素
public enum Element
{
    FIRE,
    ICE,
    POTION,
    EVIL,
    ELECTRONIC,
    WOOD,
    SPECIAL,
    MIXTURE,
    NONE,
};

/// <summary>
/// Start drag behaviour.
/// </summary>
public enum StartDragBehaviour
{
    OnMouseDown,
    InAwake,
}

public enum EndDragBehaviour
{
    OnMouseUp,
    OnMouseDown,
}


//Dice Type
//if get d2 ,then show the number of dice 1,0
//if get d4,then show the number of dice 25,50,75,100
//if get d6 then show the normal dice ()
public enum DiceType
{
    d2,     //range(0,1)
    d4,     //range(0.25,1.0
    d6,
    d8,
    d10,
    d12,
    d16,
}



//--------------------------------------

/// <summary>
/// Castle manager.
/// </summary>
public enum CastleType
{
    None,       //
    AtKCastle,  //red Atk castle    100v
    DefCaslle,  //blue def castle   010v
    PureCastle, //hp castle     000v
    MagicCastle,   //main  magic castle 010 v 
    MonkCastle, //monk castle , 3v
}

//Worker
public enum WorkerType
{
    None,
    AtkWorker,
    DefWorker,
    MagicWorker,
    Monk,
}
//-----------------------------
//制造系统,用于管理制造物品分解物品以及RPG元素
//构造类型包含
//1.卡牌构筑
//2.材料合成
//3.装备建造
//4.
public enum Crafttype
{
    //TCG
    CraftCard,
    CraftMatrial,
    CraftEquipment,
    CraftBuilding,  //Castle Building Moce
    None,
}

/// <summary>
/// 材料稀有度
/// </summary>
public enum ItemRatity
{
    Junk,
    Normal,
    Rare,
    Epic,
    Ancient,
    Legendary
}

public enum NpcType{
    None,
    Merchant,
    Enemy,
    Others,
}




///<summary>
///ItemType
///</summary>
public enum ItemType
{
    None,
    Junk,
    QuestNote,      //4x4 
    Card,
    Creature,
    Pack,
    Scroll,
    Weapon,
    Equipment,
    Magic,
    Other,
    Consumable
}


//
public enum SlotType
{
    Inventory,
    Merchant,
    Battle,
    Equipment,
    Crafting,
    Card,
   
}

#region WeaponType und SloType

/// <summary>
/// RPG part include elements of RPG games
/// </summary>
public enum WeaponType
{
    Sword,
    Gun,
    Strick,

}

public enum RingType
{
    Book,
    Beer,
    HolyCube,
    
}

public enum ArmorType
{
    None,
    Light,        //轻甲
    Heavy,        //重甲
    Magic,        //布甲
}

public enum SocketType{
    STR,
    DEX,
    INTE,
    MULT,
}
// /// <summary>
// /// 武器装备方式
// /// </summary>
// public enum EquipmentType
// {
//     Shield,  //盾牌，副手位置
//     Ring,    //耳环
//     Weapon,  //武器
//     Sword,
// }

public enum EquipmentSlotType
{
    None,
    weapon,
    armor,
    ring,
    light,
magic,
heavy,
    consumable,
    reagent,
    socket,
    offHand,
    potion,
    gem,


}

#endregion

public enum HealType{
    Health,
    Atk,
    Armor,
    Others,
}
public enum TabType
{
    buyBack,
    repair,
    weapon,
    armor,
    misc,
    ring,
    pack,
    Others,
}

#region QuestState



///<summary>
///
///Quest Module
///
///</summary>
public enum QuestType
{
    Main,
    Side,
    Job,
    PartTime,
    Epic,
    other,
}

#endregion


#region Common

public enum CursorType
{
    Normal,
    Use,
    Business,
    Atk,
    Talking,

}

#endregion



#region ShopManager 
public enum ItemTabType
{
    None,
    ItemList,
    CraftList,
    RepairList,
    SellList,
}
#endregion


#region CraftType

public enum CraftingTabType
{
    All,
   
    Equipment,

    Reagent,

}

public enum DiscoverType{
    None,
    Rnd,
    Deck,
    CardCollection,
    Classes,
    Rarity,
    Mana,
    Oppenent,
    HardMode,
    Dungeon,


}

#endregion






public enum ConsumableType{
    None,
    potion,
    Scroll
}

public enum OtherType
{
    None,
}
