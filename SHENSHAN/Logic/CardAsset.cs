using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//卡牌资源创建
//includes 
//Card base info
//AtkType
//CardType
[System.Serializable]
public class CardAsset :IComparable<CardAsset>
{
    [Header("General Info")]
    public string name;
    public string ename;
    public int cardID;
    // public CharacterAsset characterAsset;
    public CharacterAsset characterAsset;
    [TextArea(2, 4)]
    public string cardDetail;
    public Sprite cardSprite;
    public CardRatityOption ratityOption;
    public string tags; 
    [UnityEngine.Range(0, 10)]
    public int manaCost;
    public int OverrideLimitOfThisCardsInDeck = -1;
    //CardNumer Limit=>3 if number =0 HideThem until Allcard toogle is on  && default =0
   
   public CardAsset card;
    
    public TypeOfCards typeOfCards;


    [Header("CreatureInfo")] 
    public CardType creatureType = CardType.None;
  
    public int cardAtk;
 
    public int cardDef;
   
    public int cardHealth;

    public bool isTokenCard;

    public bool hasToken;
    //Check is The Initials Card

   
    public string tokenCardAsset; //For Token Can't enter discard pool
 
    public string creatureScriptName;
    public bool taunt = false;
    public int specialCreatureAmount;
    public bool hasRound;
    public int RoundTime;    
    public int atkForOneTurn = 1;    //Can Atk One Round
    public bool charge;
    
    [Header("Resistance")] 
    [UnityEngine.Range(0, 10)] public int fireResistance;
    [UnityEngine.Range(0, 10)] public int iceResistance;
     [UnityEngine.Range(0, 10)] public int poisonResistance;
    [UnityEngine.Range(0, 10)] public int electronicResistance;
        [UnityEngine.Range(0, 10)] public int physicsResistance;
   

    [UnityEngine.Range(0, 10)] public int STR;
    [UnityEngine.Range(0, 10)] public int DEX;
    [UnityEngine.Range(0, 10)] public int INT;
    [UnityEngine.Range(0, 10)] public int SPD;
    [UnityEngine.Range(0, 10)] public int RES;


    [Header("SpellInfo")]
    public TargetOptions target;    //指向对象
    public string spellScriptName;    
    public int SpecialSpellAmount;     //buff Number

    public DamageElementalType damageEType;
    public bool hasDET;
    public SpellBuffType spellBuffType;
    public bool hasBuff;
    public bool hasDetailCreatureType;
    public CreatureType detailCreatureType;
    public string cardFrom;
    public DiscoverType disType;
    public ArtifactType artifactType;
    public GameObject cardTaunt;

    #region For Dungeon
    public bool isTemp;


    #endregion

    //

    //For EnemyCard if not ist init card
    public bool isEnemyCard;
    



    /// <summary>
    /// Compares to.
    /// </summary>
    /// <returns>The to.</returns>
    /// <param name="other">Other.</param>
    // public int CompareTo(CardAsset other)
    // {
    //     if (other.manaCost < this.manaCost)
    //     {
    //         return 1;

    //     }else if(other.manaCost > this.manaCost){
    //         return 0;
    //     }else{
    //         return name.CompareTo(other.name);
    //     }
    // }
     public int CompareTo(CardAsset other)
    {
        if (other.manaCost < this.manaCost)
        {
            return 1;

        }else if(other.manaCost > this.manaCost){
            return -1;
        }else{
            return name.CompareTo(other.name);
        }
    }


    // //
    public static bool operator >  ( CardAsset openand1 ,CardAsset openand2){
        return openand1.CompareTo(openand2) == 1;
    }
    public static bool operator <(CardAsset openand1, CardAsset openand2)
    {
        return openand1.CompareTo(openand2) == -1;
    }
    public static bool operator >=(CardAsset openand1, CardAsset openand2)
    {
        return openand1.CompareTo(openand2) >=0;
    }
    public static bool operator <=(CardAsset openand1, CardAsset openand2)
    {
        return openand1.CompareTo(openand2) <= 0;
    }
}


