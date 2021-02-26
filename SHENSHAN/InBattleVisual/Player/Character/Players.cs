
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using DG.Tweening;
using Mirror;
using Random = System.Random;
using Unity.Mathematics;
using System.Linq;

//
/// <summary>
/// 玩家类,用于控制玩家基本属性,控制其余实现功能
/// ChangeLog 2021-1-18
/// 1.delete stats(str & dex & inte)
/// 2.
/// </summary>
[SerializeField]
public class Players : MonoBehaviour, ICharacter
{
    /// // PUBLIC FIELDS
    //that we get from ID factory
    public static Players instance = null;
    public int playerID;    //玩家id
    public CharacterAsset charAsset;    //
    public PlayerArea playerArea;
    public EnemyArea enemyArea;
//    public EnemyArea enemyArea; 

    public SpellEffect heroPowerEffect;
    public bool usedHeroPowerThisTurn = false;  //是否使用英雄技能
    private QuestManager quest;
    private int boundsManaThisTurn = 0;
    public PlayerData playerData;
    public List<EquipmentSlot> slots;
    public List<EquipmentSlot> playerEquipmentSlot;

    [Header("Basic Game Elements")]
    public Hand hand;
    public Deck deck;
    public Table table;
    public Castle castle;
    public DiscardPool discardPool;

    public Players() { }
    //public int Crystals;
    //public int MaxCrystals;
  
    
    private ManaPoolVisual manaVisual;
    private TurnManager tman;

    //public CardAsset ca;

    public bool useWeapon = false;

    private CardCollection cardCollection;
    public static  Players[] players ;
    
    //EXTRA
    //荆棘
    // public bool hasHurtDef=false;
    // public int hurtDef =1;
    public bool hasESD=false;
    public int ExtraSpellDamage = 0;    //额外法术增伤

    public bool dungeonEffect=false;
    // public bool hasFlash=false;
    // public float flashPerc=0.0f;


    //

    private bool _isFirstCard;
    public bool isFirstCard{
        get{
            return _isFirstCard;
            }
    
    set{
        _isFirstCard=value;
    }}
    public int playCardThisTurn{get;set;}
    public bool hasCoin=false;
    private int _coinNumber;
    public int coinNumber{get{
        return _coinNumber;}
        set{
            _coinNumber=value;
        }}

    #region  Resistance

    public int FR { get; set; }
    public int IR { get; set; }
    public int ER { get; set; }
    public int PR { get; set; }

    #endregion
    #region G/S
    // PROPERTIES 
    // this property is a part of interface ICharacter
    public int ID
    {
        get { return playerID; }
    }

    // opponent player
    public Players otherPlayer
    {
        get
        {

            if (players[0]==this)
            {
               
                return players[1];
            }
            else
                return players[0];

        }
    }

    // total mana crystals that this player has this turn
    private int _manaThisTurn;
    public int manaThisTurn
    {
        get { return _manaThisTurn; }
        set
        {
            if (value < 0)
                _manaThisTurn = 0;
            else if (value > playerArea.manaThisTurn.crystals.Length)
                _manaThisTurn = playerArea.manaThisTurn.crystals.Length;
            else
                _manaThisTurn = value;
            //playerArea.ManaBar.TotalCrystals = manaThisTurn;

            new UpdateManaCrystalsCommand(this, manaThisTurn, manaLeft).AddToQueue();
            //StartCoroutine(UpdateMana(this, manaThisTurn, manaLeft));
        }
    }

    //TODO 2021-1-18
    public bool shenshanModule; //active the module when mana>=7 
    public bool isSecond;



    //For Enemy Asset
   public void LoadEnemyAssetFromVisual(Players p,int dif)
   {
        //HARD MODE
        var dBouns = 0;
        //EVIL MODE
        var evilDamage = 0;
        var evilArmor = 0;
        var evilHealth = 0;
        //
        if(dif==0){
                p.MaxHealth = BattleStartInfo.SelectEnemyDeck.enemyAsset.Health;
        }else if(dif==1){
                dBouns = 5;
                p.MaxHealth = BattleStartInfo.SelectEnemyDeck.enemyAsset.Health+dBouns;

          }     else if(dif==2){
                //PVP Mode 
                 evilDamage = 3;
                 evilArmor = 10;
                 evilHealth = 10;

                p.MaxHealth = BattleStartInfo.SelectEnemyDeck.enemyAsset.Health + evilHealth;
                p.CreatureAtk = BattleStartInfo.SelectEnemyDeck.enemyAsset.damage+evilDamage;
                p.CreatureDef = BattleStartInfo.SelectEnemyDeck.enemyAsset.def +evilArmor;

        }
        
      var e = BattleStartInfo.SelectEnemyDeck.enemyAsset;
       enemyArea.playerPortraitVisual.enemyAsset= BattleStartInfo.SelectEnemyDeck.enemyAsset;
        //
        
      enemyArea.playerPortraitVisual.EnemyHead.sprite = e.Head;
      //DB
      //MaxHealth+= DungeonExplore.DUNGEONENEMYBOUNS;
       enemyArea.playerPortraitVisual.healthText.text=MaxHealth.ToString();
       enemyArea.playerPortraitVisual.enemyCardList = new List<CardAsset>(BattleStartInfo.SelectEnemyDeck.cards);
      
      
       //  Equipmentslot, 0 hide them
       if(GlobalSetting.instance.topPlayer.CreatureDef <= 0 ){
            GlobalSetting.instance.topPlayer.playerArea.playerPortraitVisual.ArmorImage.gameObject.SetActive(false);
       }else if(GlobalSetting.instance.topPlayer.CreatureAtk<=0){
            GlobalSetting.instance.topPlayer.playerArea.playerPortraitVisual.WeaponImage.gameObject.SetActive(false);
       }
       
       //Quest Entry if has 
               GlobalSetting.instance.topPlayer.playerArea.playerPortraitVisual.gameObject
                   .AddComponent<IncrementOnDestroy>().GetComponent<IncrementOnDestroy>();
               GlobalSetting.instance.topPlayer.playerArea.playerPortraitVisual.gameObject
                  .GetComponent<IncrementOnDestroy>().increment = 1;
                  if(enemyArea.playerPortraitVisual.enemyAsset.isBoss==true){
                      GlobalSetting.instance.topPlayer.playerArea.playerPortraitVisual.gameObject
                   .GetComponent<IncrementOnDestroy>().variable ="DungeonBoss";
                  }else {
               GlobalSetting.instance.topPlayer.playerArea.playerPortraitVisual.gameObject
                   .GetComponent<IncrementOnDestroy>().variable ="DungeonMonster";
                  }
               GlobalSetting.instance.topPlayer.playerArea.playerPortraitVisual.gameObject
                  .GetComponent<IncrementOnDestroy>().max = 
                   DialogueLua.GetVariable("DungeonGoal").asInt;
                
     

   }

    // full mana crystals available right now to play cards / use hero power 
    private int _manaLeft;
    public int manaLeft
    {
        get
        { return _manaLeft; }
        set
        {
            if (value < 0)
                _manaLeft = 0;
            else if (value > playerArea.manaThisTurn.crystals.Length)
                _manaLeft = playerArea.manaThisTurn.crystals.Length;
            else
                _manaLeft = value;

            //playerArea.ManaBar.AvailableCrystals = manaLeft;
            //StartCoroutine(UpdateMana(this, manaThisTurn, manaLeft));
            new UpdateManaCrystalsCommand(this, manaThisTurn, manaLeft).AddToQueue();
            //Debug.Log(manaLeft);
            if (TurnManager.instance.WhoseTurn == this)
                HighlightPlayableCards();
        }
    }

    //
    private int _atk;

    public int CreatureAtk
    {
        get { return _atk; }
        set { 

            // if (playerData.Strength % 3 == 0)
            // {
            //     _atk = value+(Mathf.RoundToInt(playerData.Strength/3));
            // }

            _atk = value;

        }
    }

    // private int _atkDur;

    // public int atkDur
    // {
    //     get { return _atkDur; }
    //     set { _atkDur = value; }
    // }



    private int health;
    public int MaxHealth
    {
        get { return health; }
        set
        {
            //Health = Equipment + effect  +base
            // if (playerData.Strength % 3 == 0)
            // {
            //     health = value + Mathf.RoundToInt(playerData.Strength / 3);
            // }

            health=value;
            //TODO 2011-1-18
            // var Equipmentslot = FindObjectsOfType<EquipmentSlot>();
            
            // var effect = FindObjectOfType<BuffList>();

            // int eb=0;
            // int effectb=0;
            

            // health = base.health + eb + effectcb;
            if (value <= 0)
                Die();
        }
    }

    private int _armor;
    public int CreatureDef {
        get { return _armor; }
        set
        {
            // if (playerData.Dex % 3 == 0)
            // {
            //     _armor = value + Mathf.RoundToInt(playerData.Dex / 3);
            // }

            if (_armor < 0)
            {
                _armor = 0;
            }
            _armor = value;
        }
    }
    // public int STR { get; set; }
    // public int DEX { get; set; }
    // public int INTE { get; set; }

    public EnemyAsset enemyAsset { get; set; }

    #endregion

    // CODE FOR EVENTS TO LET CREATURES KNOW WHEN TO CAUSE EFFECTS
    public delegate void VoidWithNoArguments();
    
    public event VoidWithNoArguments CreaturePlayedEvent;
    public event VoidWithNoArguments SpellPlayedEvent;
    public event VoidWithNoArguments StartTurnEvent;
    public event VoidWithNoArguments AAEvent;
    public event VoidWithNoArguments EndTurnEvent;



    // ALL METHODS
    void Awake()
    {
        
     
        // find all scripts of type Player and store them in Players array
        // (we should have only 2 players in the scene)
        players = GameObject.FindObjectsOfType<Players>();
        playerData = PlayerData.localPlayer;
        // obtain unique id from IDFactory
        playerID = IDFactory.GetUniqueID();
        //Equipslot
       
       
    }

   

    public virtual void OnTurnStart()
    {
        if (StartTurnEvent != null)
        {
            StartTurnEvent.Invoke();
        }
        
        //ability TODO
        // if(BattleStartInfo.DungeonDifficult=="困难" && BattleStartInfo.DungeonEventType!=DungeonEventType.None){
        // new DungeonEventCommand(this,BattleStartInfo.DungeonEventType,BattleStartInfo.DungeonExtraBouns).AddToQueue();
        // }

        //
    //      if(TurnManager.instance.WhoseTurn.isSecond==true){
    //        //Select Part then set false
    //         GlobalSetting.instance.ShenShanModule();
    //        TurnManager.instance.WhoseTurn.isSecond=false;
    //    }
       
        // add one mana crystal to the pool;
        Debug.Log("In ONTURNSTART for " + gameObject.name);
        usedHeroPowerThisTurn = false;
        //Check is first card
       TurnManager.instance.WhoseTurn.isFirstCard =true;
      TurnManager.instance.WhoseTurn.playCardThisTurn=0;


        //if (TurnManager.instance.WhoseTurn == GlobalSetting.instance.lowPlayer)
        //{
        //    playerArea.playerPortraitVisual.weapon.WasUsed = false;
        //}

        ++manaThisTurn;
        manaLeft = manaThisTurn;
        //TODO 123
        Debug.Log("=========Check ShenShanModule ===============");
        if(TurnManager.instance.WhoseTurn.manaLeft >=7){
            GlobalSetting.instance.ShenShanModule();
        }
        
        //creature Buff Update
       for(int cl=0;cl<table.creatureOnTable.Count;cl++){
            if (cl != null)
            {
                table.creatureOnTable[cl].OnTurnStart();
                
                //Artifact State
                Debug.Log("=========================================>Artifact State");
                if (playerArea.playerPortraitVisual.artifactList.Count > 0)
                {
                    for (int i = 0; i < playerArea.playerPortraitVisual.artifactList.Count; i++)
                    {
                        playerArea.playerPortraitVisual.UpdateArtifact(playerArea.playerPortraitVisual.artifactList[i].GetComponent<ArtifactIcon>().type,
                            playerArea.playerPortraitVisual.artifactList[i].GetComponent<ArtifactIcon>().amount);
                    }
                }
                
                Debug.Log("============================================>Buff State ");
                //BuffIconState
                GameObject objs = IDHolder.GetComponentWithID(table.creatureOnTable[cl].ID);
                for (int i = 0; i < objs.GetComponent<OneCreatureManager>().buffList.Count; i++)
                {
                    if (objs.GetComponent<OneCreatureManager>().buffList[i] != null)
                    {
                        objs.GetComponent<OneCreatureManager>().UpdateBuff(table.creatureOnTable[cl]);
                    }
                }
            }

        }
       //player ability 
       if (playerArea.playerPortraitVisual.artifactList.Count > 0)
       {
           for (int i = 0; i < playerArea.playerPortraitVisual.artifactList.Count; i++)
           {
               playerArea.playerPortraitVisual.UpdateStates(
                   playerArea.playerPortraitVisual.artifactList[i].GetComponent<ArtifactIcon>().type,
                   playerArea.playerPortraitVisual.artifactList[i].GetComponent<ArtifactIcon>().amount);
           }
       }

       //playerArea.heroPowerBtn.WasUsed = false;

        // if(otherPlayer==GlobalSetting.instance.lowPlayer){
        //     Debug.Log("Lock weapon");
        //otherPlayer.playerArea.playerPortraitVisual.weapon.usedPanel.gameObject.SetActive(true);
        // }else{
        //     otherPlayer.playerArea.playerPortraitVisual.weapon.usedPanel.gameObject.SetActive(false);
        // }

        //Reset the Armor
       TurnManager.instance.WhoseTurn.CreatureDef =  0;


    }
    
    

    public void OnTurnEnd()
    {
        if (EndTurnEvent != null)
        {

            Debug.Log("EndTurn Event");
            EndTurnEvent.Invoke();
        }
        
        manaThisTurn -= boundsManaThisTurn;
        boundsManaThisTurn = 0;
        
        GetComponent<TurnMaker>().StopAllCoroutines();
    }

    // STUFF THAT OUR PLAYER CAN DO

    // get mana from coin or other spells 
    public void GetBonusMana(int amount)
    {
        boundsManaThisTurn += amount;
        manaThisTurn += amount;
        manaLeft += amount;
    }

    //Todo
public void LoadBattleInfo(PlayerData p){
    playerArea.bci.SetInfo(p);
}

public void LoadBattleInfoEnemy(EnemyAsset e){
    enemyArea.playerPortraitVisual.SetInfo(e);
}


    // draw a single card from the deck
   
    public void DrawACard(bool fast = false,Players p = null)
    {
        if (deck.cards.Count > 0)
        {
            if (hand.CardInHand.Count < playerArea.handVisual.LmitOfCardOfHand)
            {
                Debug.Log("Limit ist=>"+playerArea.handVisual.LmitOfCardOfHand+"und current"+hand.CardInHand.Count);
                
//                 var commonHand = hand.CardInHand.FindAll(ci => ci.card.typeOfCards == TypeOfCards.Common).ToList();
// var EC = hand.CardInHand.FindAll(ci => ci.card.typeOfCards != TypeOfCards.Common).ToList();

               
                    Debug.Log("Draw card");
                    CardLogic nc = new CardLogic(this, deck.cards[0]);
                  
                //   if(nc.card.typeOfCards == TypeOfCards.Common && p == GlobalSetting.instance.lowPlayer && nc.card.name=="攻击"){
                //         nc.card.SpecialSpellAmount = p.CreatureAtk;
                //   }
                
                //   else if(nc.card.typeOfCards == TypeOfCards.Common && p == GlobalSetting.instance.lowPlayer && nc.card.name=="防御"){
                //         nc.card.SpecialSpellAmount = p.CreatureDef;
                //   }

                hand.CardInHand.Insert(0, nc);
                // Debug.Log(hand.CardInHand.Count);
                // 2) logic: remove the card from the deck
                deck.cards.RemoveAt(0);
                // 2) create a command
               
                Sequence s = DOTween.Sequence();
                s.PrependInterval(0.4f);
                new DrawACardCommand(hand.CardInHand[0], this, fast: true, fromDeck: true).AddToQueue();


            }
            else
            {
                // StartCoroutine(DiscordCardSelectionRoutine());
                Debug.Log("Card Full discard one");
                int rnd =UnityEngine.Random.Range(0,TurnManager.instance.WhoseTurn.hand.CardInHand.Count);
                //Discard
                DiscardCardAtIndex(rnd);
            }
        }
        else if(deck.cards.Count==0)
        {
            //ReCharge Card from discardpool
            Debug.Log("DP Card return to deck and gave a card to player");
            for (int i = 0; i < discardPool.cardpool.Count; i++)
            {
                if (discardPool != null )
                {
                    //Add To deck
                   
//                    GameObject obj = Instantiate(GlobalSetting.instance.CardAvaPrefab,discardPool.transform.position,Quaternion.identity) as GameObject;
//                    GameObject obj = discardPool.cardObj[i];
//                    obj.transform.DOLocalMove(deck.transform.position, 0.5f);
//                    obj.transform.localScale=new Vector3(0.6f,0.6f);
//                    obj.transform.parent = deck.transform;
                    Sequence s = DOTween.Sequence();

                    
//                      s.Append( obj.transform.DOMove(deck.transform.position, 0.5f));
//                      s.Insert(0f, obj.transform.DORotate(new Vector3(0f, -179f, 0f), 0.4f));
//                        Destroy(obj);
                    
//                    s.Append( obj.transform.DOMove(deck.transform.position, 3.0f));
//                    s.Insert(0f, obj.transform.DORotate(new Vector3(0f, -179f, 0f), 0.4f));
//                    discardPool.cardObj.Remove(obj);
                        deck.cards.Add(discardPool.cardpool[i]);
//                        discardPool.cardpool.Remove(discardPool.cardpool[i]);
                        Debug.Log(discardPool.cardpool[i].name+"Add To Deck");
                                                                            
                      
                }
                            
            }
            deck.cards.Shuffle();
            
            //then Add a card for player 
            if (deck.cards.Count > 0 && hand.CardInHand.Count<8 )
            {
                Debug.Log("Return to deck from discardPool Draw");
                CardLogic newCard = new CardLogic(this, deck.cards[0]);
                hand.CardInHand.Insert(0, newCard);
                // Debug.Log(hand.CardInHand.Count);
                // 2) logic: remove the card from the deck
                deck.cards.RemoveAt(0);
                // 2) create a command
                new DrawACardCommand(newCard,this,false,true).AddToQueue();
                //TODO
                Sequence s = DOTween.Sequence();
                s.PrependInterval(0.4f);
              
//                DrawACard(true);
                
            }
            else if(hand.CardInHand.Count>8)
            {
                //hand full rnd discard a  card from hand then draw
                int rnd = UnityEngine.Random.Range(0, hand.CardInHand.Count);
                DiscardCardAtIndex(rnd);
                DrawACard(true);
            }

            

        }
        else
        {
           Debug.Log("Test ,minus health");
        }


      
    }

   public IEnumerator DiscordCardSelectionRoutine(){
    //    Debug.Log("Select State");
    //     float delay = 5.0f;
    //     int index =
       

        yield return null;
    }

    public void ExtraSelect(Players p)
    {
        //AI Rnd select by rnd index
        if (p == GlobalSetting.instance.topPlayer)
        {
            int sIndex = UnityEngine.Random.Range(0, 2);


            DiscoverManager.instance.ShowDiscover(GlobalSetting.instance.secondList,sIndex, DiscoverType.SecondPlayer);

        }
        else
        {
            DiscoverManager.instance.ShowDiscover(GlobalSetting.instance.secondList,-1, DiscoverType.SecondPlayer);
        }
        }

    // get card NOT from deck (a token or a coin)
   
    public void GetACardNotFromDeck(CardAsset cardAsset)
    {
        if (hand.CardInHand.Count < playerArea.handVisual.slots.children.Length)
        {
            // 1) logic: add card to hand
            CardLogic newCard = new CardLogic(this, cardAsset);
            
            newCard.owner = this;
            hand.CardInHand.Insert(0, newCard);
            // 2) send message to the visual Deck
            new DrawACardCommand(hand.CardInHand[0], this, fast: true, fromDeck: false).AddToQueue();
        }
        // no removal from deck because the card was not in the deck
    }

    public void GetACardNotFromDeck()
    {
        if (hand.CardInHand.Count < playerArea.handVisual.slots.children.Length)
        {
            var secondCard = CardCollection.instance.allCardsArray.FindAll(sc => sc.tags=="second").ToList();
            if(secondCard.Count>0){

                int rnd = UnityEngine.Random.Range(0,secondCard.Count);
            // 1) logic: add card to hand
            CardLogic newCard = new CardLogic(this, secondCard[rnd]);
            Debug.Log(newCard.card.name +"Added to hand");
            newCard.owner = this;
            hand.CardInHand.Insert(0, newCard);
            // 2) send message to the visual Deck
            new DrawACardCommand(hand.CardInHand[0], this, fast: true, fromDeck: false).AddToQueue();
            }else{
                Debug.Log("Couldn't found the card");
            }
        }
        // no removal from deck because the card was not in the deck
    }

    // 2 METHODS FOR PLAYING SPELLS
    // it is cnvenient to call this method from visual part
   
    // 1st overload - takes ids as arguments
    public void PlayASpellFromHand(int SpellCardUniqueID, int TargetUniqueID)
    {
        if (TargetUniqueID < 0)
            PlayASpellFromHand(CardLogic.CardsCreatedThisInGame[SpellCardUniqueID], null);
        else if (TargetUniqueID == ID)
        {
            PlayASpellFromHand(CardLogic.CardsCreatedThisInGame[SpellCardUniqueID], this);
        }
        else if (TargetUniqueID == otherPlayer.ID)
        {
            PlayASpellFromHand(CardLogic.CardsCreatedThisInGame[SpellCardUniqueID], this.otherPlayer);
        }
        else
        {
            // target is a creature
            PlayASpellFromHand(CardLogic.CardsCreatedThisInGame[SpellCardUniqueID], CreatureLogic.creatureCreatedThisGame[TargetUniqueID]);
        }

    }


    // 2nd overload - takes CardLogic and ICharacter interface - 
    // this method is called from Logic, for example by AI
   
    public void PlayASpellFromHand(CardLogic playedCard, ICharacter target)
    {
        if(TurnManager.instance.WhoseTurn==GlobalSetting.instance.lowPlayer){
        GlobalSetting.instance.SETLogs(string.Format("{0}使用{1}",TurnManager.instance.WhoseTurn.playerData.name.ToString(),playedCard.card.name.ToString()));
        }else if(TurnManager.instance.WhoseTurn==GlobalSetting.instance.topPlayer){
 GlobalSetting.instance.SETLogs(string.Format("{0}使用{1}",TurnManager.instance.WhoseTurn.enemyArea.playerPortraitVisual.enemyAsset.EnemyName.ToString().ToString(),playedCard.card.name.ToString()));
        }
        if(isFirstCard==true){
            isFirstCard=false;
            playCardThisTurn ++;
        }else{
            playCardThisTurn++;
        }

        manaLeft -= playedCard.currentManaCost;
        //
        Debug.Log("Play Spell ");
        // cause effect instantly:
        if (playedCard.spellEffect != null)
        {
            if (playedCard.card.spellBuffType==SpellBuffType.None && playedCard.card.damageEType==DamageElementalType.None)
            {
                Debug.Log("Spell ");
                playedCard.spellEffect.ActiveEffect(playedCard.card.SpecialSpellAmount, target);
            }else if(playedCard.card.spellBuffType!= SpellBuffType.None){
                //
                Debug.Log("Spell Buff Effect ");
                playedCard.spellEffect.ActiveEffectToTargetStat(playedCard.card.SpecialSpellAmount,target,playedCard.card.spellBuffType);
            }
            else if (playedCard.card.damageEType != DamageElementalType.None)
            {
                Debug.Log("DET  Buff");
                playedCard.spellEffect.ActiveRoundEffect(playedCard.card.SpecialSpellAmount, target,
                    playedCard.card.RoundTime, playedCard.card.damageEType);
            }

            if (playedCard.card.artifactType != ArtifactType.None)
            {
                Debug.Log("Artifact Effect Active");
                playedCard.spellEffect.ActiveEffectToTargetStat(playedCard.card.SpecialSpellAmount,
                    TurnManager.instance.WhoseTurn, playedCard.card.artifactType);
            }
        }
        else 
        {
                Debug.LogWarning("No effect found on card " + playedCard.card.name);
        }
        
            // no matter what happens, move this card to PlayACardSpot
            new PlayASpellCardCommand(this, playedCard).AddToQueue();
            // remove this card from hand
            hand.CardInHand.Remove(playedCard);
            // check if this is a creature or a spell
      
    }

    // METHODS TO PLAY CREATURES 
    // 1st overload - by ID
    
    public void PlayACreatureFromHand(int UniqueID, int tablePos)
    {  
        if(isFirstCard==true){
            isFirstCard=false;
            playCardThisTurn ++;
        }else{
            playCardThisTurn++;
        }
        //
        PlayACreatureFromHand(CardLogic.CardsCreatedThisInGame[UniqueID], tablePos);
        isFirstCard=false;
    }

    // 2nd overload - by logic units
   
    public void PlayACreatureFromHand(CardLogic playedCard, int tablePos)
    {
        GlobalSetting.instance.SETLogs(string.Format("{0}置入场中",playedCard.card.name));
        // Debug.Log(manaLeft);
        // Debug.Log(playedCard.currentManaCost);
        manaLeft -= playedCard.currentManaCost;
        // Debug.Log("Mana Left after played a creature: " + manaLeft);
        // create a new creature object and add it to Table
        CreatureLogic newCreature = new CreatureLogic(this, playedCard.card);
       
        table.creatureOnTable.Insert(tablePos, newCreature);
        if (newCreature.card.typeOfCards != TypeOfCards.Token)
        {
            discardPool.cardpool.Add(playedCard.card);
        }
        //
        new PlayACreatureCommand(playedCard, this, tablePos, newCreature.UniqueCreatureId).AddToQueue();
        
        // ZHAN HOU TODO
        if (newCreature.effect != null)
            newCreature.effect.WhenACreatureIsPlayed();
        // remove this card from hand
        hand.CardInHand.Remove(playedCard);
        HighlightPlayableCards();
    }
/// <summary>
/// 
/// </summary>
/// <param name="type"></param>
/// <param name="amount"></param>
    public void PlayArtifact(ArtifactType type, int amount)
    {
        playerArea.playerPortraitVisual.AddArtifact(type, amount);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="opRnd"></param>
    public void StealCard(int opRnd)
    {
        GameObject stealCard = TurnManager.instance.WhoseTurn.otherPlayer.playerArea.handVisual.CardsInHand[opRnd];
        //

        //
        CardAsset ca = CardCollection.instance.GetCardAssetByName(stealCard.GetComponent<OneCardManager>().cardAsset.name);
        TurnManager.instance.WhoseTurn.otherPlayer.DiscardCardAtIndex(opRnd);
        CardLogic newCard = new CardLogic(TurnManager.instance.WhoseTurn,ca);
        //Give Card To Player with steal card
        new DrawACardCommand(newCard,TurnManager.instance.WhoseTurn,false,false).AddToQueue();
        
    }

   

    // public void UseWeapon()
    // {
        
    //     useWeapon = true;
    //     if (atkDur > 0 && TurnManager.instance.WhoseTurn==GlobalSetting.instance.lowPlayer)
    //     {
    //         playerArea.playerPortraitVisual.weapon.spellEffect.ActiveEffect();
    //         playerArea.playerPortraitVisual.weapon.WasUsed = true;
         
    //         atkDur--;
    //         if(atkDur==0){
    //             playerArea.playerPortraitVisual.WeaponImage.SetActive(false);
    //         }
    //     }
    //     else if(atkDur > 0 && TurnManager.instance.WhoseTurn==GlobalSetting.instance.topPlayer)
    //     {
    //        Players oppp = TurnManager.instance.WhoseTurn.otherPlayer;
    //         if (oppp.CreatureDef > 0)
    //         {
    //             new DealDamageCommand(oppp.ID, TurnManager.instance.WhoseTurn.CreatureAtk,
    //                 oppp.MaxHealth - TurnManager.instance.WhoseTurn.CreatureAtk,
    //                 oppp.CreatureDef - TurnManager.instance.WhoseTurn.CreatureAtk).AddToQueue();
    //         }else if (oppp.CreatureDef - TurnManager.instance.WhoseTurn.CreatureAtk < 0)
    //         {
    //             new DealDamageCommand(TurnManager.instance.WhoseTurn.otherPlayer.playerID, TurnManager.instance.WhoseTurn.CreatureAtk, TurnManager.instance.WhoseTurn.otherPlayer.MaxHealth +(oppp.CreatureDef-TurnManager.instance.WhoseTurn.CreatureAtk),
    //                 TurnManager.instance.WhoseTurn.otherPlayer.CreatureDef - TurnManager.instance.WhoseTurn.CreatureAtk).AddToQueue();
    //         }
    //         else
    //         {
    //             new DealDamageCommand(TurnManager.instance.WhoseTurn.otherPlayer.playerID, TurnManager.instance.WhoseTurn.CreatureAtk, TurnManager.instance.WhoseTurn.otherPlayer.MaxHealth - TurnManager.instance.WhoseTurn.CreatureAtk,
    //                 0).AddToQueue();
    //         }

    //         useWeapon = true;

    //         atkDur--;
    //         //das weapon is empty ,reset weapon value
    //         if (atkDur == 0)
    //         {
    //             CreatureAtk = 0;
    //             atkDur = 0;
    //         }
    //     }
    //     //
    //     if (atkDur < 0)
    //     {
    //         if (TurnManager.instance.WhoseTurn == GlobalSetting.instance.lowPlayer)
    //         {
    //             playerArea.playerPortraitVisual.WeaponImage.SetActive(false);

    //             GlobalSetting.instance.SETLogs("武器破损本场无法使用武器,哦吼");
    //         }else if (TurnManager.instance.WhoseTurn == GlobalSetting.instance.topPlayer)
    //         {
    //            //TODO
    //         }
    //     }
        
    //     //Sound
    //     SoundManager.instance.PlaySound(GlobalSetting.instance.weaponClip);
    // }
   

    public void Die()
    {
      new DelayCommand(2.0f).AddToQueue();
        //check who die;
        if (GlobalSetting.instance.topPlayer.MaxHealth <= 0 && GlobalSetting.instance.lowPlayer.MaxHealth>0)
        {
            //win 
            
             ConsoleManager.GetReward=true;
            if(GlobalSetting.instance.topPlayer.enemyArea.playerPortraitVisual.enemyAsset.isBoss==true){
                Debug.Log("Boss has Kill"+GlobalSetting.instance.topPlayer.enemyArea.playerPortraitVisual.enemyAsset.EnemyName.ToString());
                DialogueLua.SetVariable("KillBossDone",true);
                DialogueLua.SetVariable("DungeonBoss",1);
                Debug.Log("Get Variable From Dialogue"+DialogueLua.GetVariable(BattleStartInfo.SelectEnemyDeck.enemyAsset.EnemyName));
            }
            //Normal Set
            // DialogueLua.SetVariable("DungeonMonster",1);
            // Debug.Log("now monster counter ist"+DialogueLua.GetVariable("DungeonMonster").asString);
            //QuestTracker
            DialogueLua.SetVariable("isKill",true);
            //
         
            //
            bool isKill = DialogueLua.GetVariable("isKill").asBool;
            Debug.Log("TopPlayer is dead" + isKill);
            BattleStartInfo.IsWinner = true;
            Debug.Log("Winner panel");

             //Update Dungeon Reward Pool
      int rndMoney = Mathf.FloorToInt(UnityEngine.Random.Range(BattleStartInfo.SelectEnemyDeck.enemyAsset.gold / 2,
        BattleStartInfo.SelectEnemyDeck.enemyAsset.gold));
      int rndExp = Mathf.FloorToInt(UnityEngine.Random.Range(BattleStartInfo.SelectEnemyDeck.enemyAsset.exp / 2,
        BattleStartInfo.SelectEnemyDeck.enemyAsset.exp));
      int rndDust = Mathf.FloorToInt(UnityEngine.Random.Range(BattleStartInfo.SelectEnemyDeck.enemyAsset.dustReward / 2,
        BattleStartInfo.SelectEnemyDeck.enemyAsset.dustReward));


     Debug.Log("Return Console"+rndMoney +"::"+rndExp+"::"+rndDust);

      DungeonExplore.moneyPool += rndMoney;
      DungeonExplore.dustPool += rndDust;
      DungeonExplore.expPool += rndExp;
            
            GlobalSetting.instance.WinPanel.Open();
        // DialogueManager.SendUpdateTracker();
        }
        else if(GlobalSetting.instance.topPlayer.MaxHealth > 0 && GlobalSetting.instance.lowPlayer.MaxHealth<=0)
        {

            ConsoleManager.GetReward=false;
            Debug.Log("U Lose");
            // block both players from taking new moves 
            ConsoleManager.EXP += 0;
            ConsoleManager.MONEY += 0;
            ConsoleManager.EXP+=0;
            playerArea.ControlsOn = false;
            otherPlayer.playerArea.ControlsOn = false;
            TurnManager.instance.StopTheTimer();
            //Set BI
            // BattleStartInfo.IsWinner=false;
            DungeonExplore.instance.canLeave=true;
            GlobalSetting.instance.GameOverPanel.SetActive(true);
            new GameOverCommand(this,playerData).AddToQueue();
        }
        // game over
       DialogueManager.SendUpdateTracker();
    }

    // use hero power - activate is effect like you`ve payed a spell
    public void UseHeroPower()
    {
        manaLeft -= 2;
        usedHeroPowerThisTurn = true;
        heroPowerEffect.ActiveEffect();
    }
    
    /// <summary>
    /// cards[0] remove
    /// </summary>
    /// <param name="index"></param>
    public void DiscardCardAtIndex(int index)
    {
      
            //discard pool
//            deck.cards.Insert(index,hand.CardInHand[index].card);
            
            discardPool.cardpool.Add(hand.CardInHand[index].card);
            hand.CardInHand.RemoveAt(index);
            
            new DelayCommand(0.4f).AddToQueue();
            new DiscardACardCommand(this, index).AddToQueue();
            
    }

    public void DiscoverSelectACard(int index,DiscoverType type){
        Debug.Log("card index is"+index);
        DiscoverManager.instance.ShowDiscover(null,index,type);
        
    }
    

    // METHOD TO SHOW GLOW HIGHLIGHTS
    public void HighlightPlayableCards(bool removeAllHighlights = false)
    {
        //Debug.Log("HighlightPlayable remove: "+ removeAllHighlights);
        foreach (CardLogic cl in hand.CardInHand)
        {
            GameObject g = IDHolder.GetComponentWithID(cl.UniqueCardID);
             if (g!=null )
            {
                g.GetComponent<OneCardManager>().CanbePlayNow = (cl.currentManaCost <= manaLeft) && !removeAllHighlights;
             }

             //
             if(isFirstCard=false && g.GetComponent<OneCardManager>().cardAsset.spellScriptName=="CreatureCombo"){
                 Debug.Log("Card combo show special glow");
                g.GetComponent<OneCardManager>().cardFaceGlowImage.color=Color.red;
             }
          
        }

        foreach (CreatureLogic crl in table.creatureOnTable)
        {
            GameObject g = IDHolder.GetComponentWithID(crl.UniqueCreatureId);
            if (g != null)
                g.GetComponent<OneCreatureManager>().CanAtkNow = (crl.AttacksForThisTurn > 0) && !removeAllHighlights;
        }
       

        //highlight hero power
        // usedHeroPowerThisTurn = (!usedHeroPowerThisTurn) && (manaLeft > 1) && !removeAllHighlights;
    }

    // START GAME METHODS
    public void LoadCharacterInfoFromAsset()
    {

        if (BattleStartInfo.SelectDeck.characterAsset != null)
        {
            charAsset = BattleStartInfo.SelectDeck.characterAsset;
            //DB
            MaxHealth = charAsset.maxHealth + DungeonExplore.DHHeal;
            // change the visuals for portrait, hero power, etc...
        playerArea.playerPortraitVisual.characterAsset = charAsset;
            playerArea.playerPortraitVisual.ApplyLookFromAsset();

//            if (charAsset.heroPowerName != null && charAsset.heroPowerName != "")
//            {
//                heroPowerEffect =
//                    System.Activator.CreateInstance(System.Type.GetType(charAsset.heroPowerName)) as SpellEffect;
//            }
//            else
//            {
//                Debug.LogWarning("Check hero powr name for character " + charAsset.className);
//            }
        }
        else
        {

            Debug.Log("INVALID ASSET");
        }
    }
    public void AddDungeonBouns(){
        Debug.Log("Add Dungeon Bouns");
        MaxHealth += DungeonExplore.DHHeal;
        CreatureDef += DungeonExplore.DArmor;
        //
        CreatureAtk += DungeonExplore.DATK;
        //
        // atkDur  += DungeonExplore.DDUR;
    }
    public void LoadStatsFromdata()
    {
        if (playerData != null)
        {
            //load data from data
            //STR = playerData.Strength;
            //DEX =playerData.Dex;
            //INTE=playerData.Magic;

            FR= playerData.FR;
            IR =playerData.IR;
            PR =playerData.PR;
            ER =playerData.ER;
            //
            

            //
            // flashPerc = playerData.extraFlash;
            //DB
            ExtraSpellDamage = playerData.extraSpellDamage + DungeonExplore.DESD;
            
            playerArea.playerPortraitVisual.atkText.text = CreatureAtk.ToString();
            // playerArea.playerPortraitVisual.atkDurText.text = atkDur.ToString();
            playerArea.playerPortraitVisual.defText.text = CreatureDef.ToString();
        }
        else
        {
            Debug.Log("INVALID PLAYERDATA");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void LoadPlayerEquipment()
    {
        if (playerData != null)
        {
            CreatureAtk =Mathf.FloorToInt(playerData.atk);
            // atkDur = Mathf.FloorToInt(playerData.atkCount);

            CreatureDef = playerData.ArmorDef;

        }
        
    }


//EXTTRA MANA
public void ShowCoinPanel(){
    TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.coinPanel.gameObject.SetActive(true);
    TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.coinText.text = TurnManager.instance.WhoseTurn.coinNumber.ToString();
}
    public void TransmitInfoAboutPlayerToVisual()
    {
        
        playerArea.playerPortraitVisual.gameObject.AddComponent<IDHolder>().uniqueID = playerID;
        //TODO
       
        if (GetComponent<TurnMaker>() is AITurnMaker)
        {
            // turn off turn making for this character
            playerArea.AllowedToControllThisPlayer = false;
        }
        else
        {
            // allow turn making for this character
            playerArea.AllowedToControllThisPlayer = true;
        }
    }

    
    //在回合开始确定地下城事件,判断每个随从是否添加属性
    public void ActiveDungeonEffect(Players owner,DungeonEventType type ,int amount){
         var cl = TurnManager.instance.WhoseTurn.table.creatureOnTable.ToArray();

        // switch(type){
        //     case DungeonEventType.STR:
        //         foreach(var c in cl){
        //             if(c.dungeonEffect==false){
        //                 c.dungeonEffect=true;
        //                 c.Str += amount;
        //             }
        //         }
        //     break;
        //     case DungeonEventType.ATK:
        //      foreach(var c in cl){
        //             if(c.dungeonEffect==false){
        //                 c.dungeonEffect=true;
        //                 c.CreatureAtk += amount;
        //             }
        //         }
        //         break;
        //     case DungeonEventType.ARMOR:
        //      foreach(var c in cl){
        //             if(c.dungeonEffect==false){
        //                 c.dungeonEffect=true;
        //                 c.CreatureDef += amount;
        //             }
        //         }
        //         break;
        //           case DungeonEventType.DEX:
        //      foreach(var c in cl){
        //             if(c.dungeonEffect==false){
        //                 c.dungeonEffect=true;
        //                 c.Dex += amount;
        //             }
        //         }
        //         break;
        //           case DungeonEventType.INTE:
        //      foreach(var c in cl){
        //             if(c.dungeonEffect==false){
        //                 c.dungeonEffect=true;
        //                 c.Inte += amount;
        //             }
        //         }
        //         break;
        //           case DungeonEventType.AC:
        //      foreach(var c in cl){
        //             if(c.dungeonEffect==false){
        //                 c.dungeonEffect=true;
        //                 c.CreatureDef += amount;
        //             }
        //         }
        //         break;
        //           case DungeonEventType.RAGE:
        //      foreach(var c in cl){
        //             if(c.dungeonEffect==false){
        //                 c.dungeonEffect=true;
        //                 c.CreatureDef += amount;
        //             }
        //         }
        //         break;
        //           case DungeonEventType.WDUR:
        //      foreach(var c in cl){
        //          if( TurnManager.instance.WhoseTurn.dungeonEffect==false){
        //            TurnManager.instance.WhoseTurn.dungeonEffect=true;
        //            TurnManager.instance.WhoseTurn.atkDur=amount;
        //          }
        //         }
        //         break;
             

        // }
    }

    #region Creature Effect interactive

    public void ShowDiscoverCard(){

    }
    

    #endregion
}


