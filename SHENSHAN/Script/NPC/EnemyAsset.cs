using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EnemyAsset 
{
    public string EnemyName;
    public string Tags;
    public NpcType npcType;
    public Sprite Head;
    public Sprite Frame;
    public int Health;
    public ItemRatity ratity;
    public string conver;

    public int damage;
    public int def;
    
    public int fr;
    public int ir;
    public int er;
    public int pr;

    //1123
    public int attackCard;
    public int armorCard;

    

    public float flashPerc;
    public int extraSpellDamage;

    public string Loc;         //MAP LOC WHEN LOAD THE MAPLOCATIOn
    public string powerName;   //Skill Name
    //PLAYER LOOT WHEN WIN THE ENEMY
    public int exp;
    public int gold;
    public bool isLock;
    
    
    public string detail;
    
    public bool hasQuest;
    public QuestInfos[] questList;

    public bool hasEntry;
    public string entryDetail;
    public bool hasReward;
    public int moneyReward;
    public int expReward;
    public int dustReward;
    public int[] rewardId;
    public bool hasCard;
    public List<CardAsset> cardList;

    //shop if tag is merchantController
    public List<string> itemShopIDs;

    public bool isBoss;
    public string model;

    public bool isMerchant;
    public List<int> itemList;
    //3D
    public Vector3 pos;
 public EnemyAsset(){}

    public EnemyAsset(string enemyName, string tags, NpcType npcType, Sprite head,
        Sprite frame, int health, ItemRatity ratity, string conver,
        int damage, int def, int fr, int ir, int er, int pr,
        int attackCard, int armorCard, float flashPerc, int extraSpellDamage,
        string loc, string powerName, int exp, int gold, bool isLock, string detail,
        bool hasQuest, QuestInfos[] questList, bool hasEntry, string entryDetail,
        bool hasReward, int moneyReward, int expReward, int dustReward, int[] rewardId,
        bool hasCard, List<CardAsset> cardList, List<string> itemShopIDs, bool isBoss,
        string model, bool isMerchant, List<int> itemList, Vector3 pos)
    {
        EnemyName = enemyName;
        Tags = tags;
        this.npcType = npcType;
        Head = head;
        Frame = frame;
        Health = health;
        this.ratity = ratity;
        this.conver = conver;
        this.damage = damage;
        this.def = def;
        this.fr = fr;
        this.ir = ir;
        this.er = er;
        this.pr = pr;
        this.attackCard = attackCard;
        this.armorCard = armorCard;
        this.flashPerc = flashPerc;
        this.extraSpellDamage = extraSpellDamage;
        Loc = loc;
        this.powerName = powerName;
        this.exp = exp;
        this.gold = gold;
        this.isLock = isLock;
        this.detail = detail;
        this.hasQuest = hasQuest;
        this.questList = questList;
        this.hasEntry = hasEntry;
        this.entryDetail = entryDetail;
        this.hasReward = hasReward;
        this.moneyReward = moneyReward;
        this.expReward = expReward;
        this.dustReward = dustReward;
        this.rewardId = rewardId;
        this.hasCard = hasCard;
        this.cardList = cardList;
        this.itemShopIDs = itemShopIDs;
        this.isBoss = isBoss;
        this.model = model;
        this.isMerchant = isMerchant;
        this.itemList = itemList;
        this.pos = pos;
    }
}
