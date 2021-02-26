using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EnemyAsset 
{
    public string EnemyName;
    public string eEnemyName;
    public string Tags;
    public NpcType npcType;
    public Sprite Head;
    public Sprite Frame;
    public int Health;
    public ItemRatity ratity;
    public string conver;

    public int damage;
    public int def;
    public int str;
    public int dex;
    public int inte;
    public int fr;
    public int ir;
    public int er;
    public int pr;

    public float flashPerc;
    public float extraSpellDamage;

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
   
}
