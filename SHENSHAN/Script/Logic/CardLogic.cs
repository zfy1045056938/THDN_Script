
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


//卡牌逻辑用于存放于牌库管理,用于管理玩家自身所拥有的财产,只包含卡牌资源不包括属性
[System.Serializable]
public class CardLogic : IIdentifiable
{
    //玩家
    public Players owner;
    //独立卡牌id
    public int UniqueCardID;
    //卡牌资产
    public CardAsset card;
    //卡牌效果
    public SpellEffect spellEffect;
   
    

    public static Dictionary<int, CardLogic> CardsCreatedThisInGame = new Dictionary<int, CardLogic>();

    public static CardLogic instance;
    private int id;
    public int ID
    {
        get { return UniqueCardID; }
    }
    public int currentManaCost { get; set; }
    
    public CardLogic(){}

    /// <summary>
    /// Gets a value indicating whether this <see cref="T:CardLogic"/> can be player.
    /// </summary>
    /// <value><c>true</c> if can be player; otherwise, <c>false</c>.</value>
    public bool CanBePlayer
    {
        get
        {
            bool ownerTurn = TurnManager.instance.WhoseTurn == owner;
            bool fieldNotFull = true;

            if (card.cardHealth > 0)
            {
                fieldNotFull = owner.table.creatureOnTable.Count < 6;
                return ownerTurn && fieldNotFull && (currentManaCost <= owner.manaLeft);
            }

            return ownerTurn  && (currentManaCost <= owner.manaLeft);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CardLogic"/> class.
    /// </summary>
    /// <param name="owner">Owner.</param>
    /// <param name="ca">Ca.</param>
    public CardLogic(Players owner, CardAsset ca)
    {
        
        this.owner = owner;
        this.card = ca;

        UniqueCardID = IDFactory.GetUniqueID();  //Get ID from factory

      
        ResetManaCost();
        
        
        Debug.Log("PRE LOAD SPN"+ca.spellScriptName);
      
        if (ca.spellScriptName != null && card.spellScriptName != "" )
        {
            Debug.Log("Spell Script name start load");
            spellEffect = System.Activator.CreateInstance(System.Type.GetType(ca.spellScriptName)) as SpellEffect;
            spellEffect.owner = owner; 
            
        } 
        
//        effect = System.Activator.CreateInstance(System.Type.GetType(ca.creatureScriptName),
//            new System.Object[] {owner, this, ca.specialCreatureAmount,ca.disType}) as CreatureEffect;
        
        CardsCreatedThisInGame.Add(UniqueCardID, this);
    }

    public CardLogic( CardAsset ca)
    {
       
        this.card = ca;

        UniqueCardID = IDFactory.GetUniqueID();  //Get ID from factory

      
        ResetManaCost();
      
        if (ca.spellScriptName != null && card.spellScriptName != "")
        {
            spellEffect = System.Activator.CreateInstance(System.Type.GetType(ca.spellScriptName)) as SpellEffect;
            spellEffect.owner = owner; 
        }
        
        CardsCreatedThisInGame.Add(UniqueCardID, this);
    }

   

    /// <summary>
    /// 绑定水晶数
    /// </summary>
    private void ResetManaCost()
    {
        currentManaCost = card.manaCost;
    }

    public void UpdateValue( int amount, int cardGoal)
    {
        
    }
   

    /// <summary>
    /// Compares to.
    /// </summary>
    /// <returns>The to.</returns>
    ///// <param name="other">Other.</param>
    // public int CompareTo(CardLogic other){
    //     if (other.card < this.card)
    //     {
    //         return 1;
    //     }
    //     else if (other.card > this.card)
    //     {
    //         return -1;
    //     }
    //     else
    //         return 0;
    // }

}

