using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using PixelCrushers;
using GameDataEditor;
using TMPro;
using Mirror;




/// <summary>
/// 全局类用于控制场面元素
/// 1.Player
/// 2.HandCard
/// 3.DeckCard->creature && spell && worker &&  castle
/// 4.enviorment
/// 5.
/// </summary>
public class GlobalSetting : MonoBehaviour
{
    //用于控制战斗场面元素,prefab&&elements ...
    [Header("In-Game")]
    public Players lowPlayer;
    public Players topPlayer;
    public Players[] playerList;

    //[Header("Castle Info")]
    //public Castle lowCastle;
    //public Castle topCastle;

    [Header("Color")]
    public Color32 CardBodyStandardColor;
    public Color32 CardGlowColor;
    public Color32 CardRibbonStandardColor;


    [Header("Prefab")]
    public GameObject CreaturePrefab;
    public GameObject CreatureCardPrefab;
    //public Hand HandPrefab;
    //public GameObject DeckPrefab;
    public GameObject NoTargetSpellCardPrefab;
    public GameObject TargetSpellCardPrefab;
    public GameObject CardAvaPrefab;
    public GameObject tPrefab;

    public List<GameObject> tpList;

    [Header("CreatureDamageEffect")]
    public GameObject DamagePrefab;
    public GameObject BuffPrefab;
    public GameObject ExplosionPrefab;

    [Header("BattleMenu")] public GameObject menuPanel;
    public Transform showpos;
    public Transform returnpos;
    public Transform previewPos;    
    public Transform logPos;
    //
    public Text endTurnText;
    public TextMeshProUGUI logText;
    public SpriteRenderer TopNoticeSprite;
    public SpriteRenderer LowNoticeSprite;
    
    [Header("Castle und Worker")]
    //public CastleManager CastlePrefab;
    //public GameObject resourceCollection;
    //public GameObject shieldPrefab;
    //public GameObject lowCastlePrefab;
    //public GameObject topCastlePrefab;
    //public GameObject lowWorkerPrefab;
    //public GameObject topWorkerPrefab;  //pvp 

    [Header("Other")]
    public Button EndBtn;

    public UIPanel WinPanel;
    public UIPanel losePanel;
    public GameObject GameOverPanel;
    public GameObject ConsolePanel; //Console Show und load scene name mainScene
    public GameObject logPanel;
    public List<string> logList;
    public MessageManagers messageManagers;
    public ConsoleManager ConsoleManager;
    //public Sprite Joker;    //发牌员
    public GameObject fbxObj;
    public GameObject buffIcon;
    public GameObject artifactIcon;
    public GameObject viewEffectPrefab;
    public GameObject shenshanPrefab;
    
    //1-18
    public GameObject SSObj;
    
    //
    public Button b_LeaveTownBtn;
    public Button b_GiveUpBtn;

    //Clock
    //public Clock clockTime; 


    //[Header("Extra Resource")]
    //private GameObject ExtraResourcePrefab;
    //private GameObject ExtraEffectPrefab;
    //private Color32 ExtraResourceColor;

    //K,V add Player
    public Dictionary<AreaPositions, Players> playerInGame = new Dictionary<AreaPositions, Players>();
    //private Dictionary<AreaPositions, Castle> castles = new Dictionary<AreaPositions, Castle>();

    [Header("N n V")]
    public float CardPreviewTime = 1f;
    public float CardTransitionTime = 1f;
    public float CardTransitionTimeFast = 1.0f;
    public float CardPreviewTimeFast = 0.2f;
    public float CardTransitionTimeSet = 0.5f;
    public static GlobalSetting instance;

    [Header("SoundClip")] public AudioClip drawClip;
    public AudioClip playClip;
     public AudioClip discardClip;
     public AudioClip atkClip;
    public AudioClip weaponClip;
    public AudioClip armorClip;
    public AudioClip burningClip;
    public AudioClip freezeClip;
     public AudioClip elecClip;
    public AudioClip posionClip;
    public AudioClip AbsorbClip;
    public AudioClip healClip;

    public AudioClip buffClip;
    public AudioClip machineClip;
    public AudioClip bloodyClip;


    [Header("Particle System Manager")]
    public ParticleSystem drawCardEffect;
    //elemental
    public ParticleSystem burningEffect;
    public ParticleSystem freezeEffect;
    public ParticleSystem electronicEffect;
    public ParticleSystem posionEffect;
    public ParticleSystem critEffect;
    public ParticleSystem healEffect;
    public ParticleSystem bloodEffect;
    //Common
    public ParticleSystem singleAttackEffect;
    public ParticleSystem groupAttackEffect;
    public ParticleSystem RangedAttackEffect;
    public ParticleSystem cardInTableEffect;
    public ParticleSystem cardAttackTailEffect;
    public ParticleSystem cardDeadEffect;


    //aborb
    public ParticleSystem absorbEffect;
    public ParticleSystem absorbArmorEffect;
    public ParticleSystem absorbExtraEffect;

    //card effect
    public ParticleSystem tokenCardEffect;
    public ParticleSystem extradamageEffect;

    [Header("Effect Object")]
    public GameObject ESDPrefab;
    public GameObject BObj;
    public GameObject killObj;

    [Header("Second Player")]
    public List<CardAsset> secondList;
    
      
    /// <summary>
    /// Awake this instance.
    /// </summary>
    public  void Awake()
    {
        instance = this;
        messageManagers =FindObjectOfType<MessageManagers>();
        ConsoleManager =FindObjectOfType<ConsoleManager>();
        playerList = GetComponents<Players>();
        
        
        playerInGame.Add(AreaPositions.Top, topPlayer);
        playerInGame.Add(AreaPositions.Low, lowPlayer);

         PlayerPrefs.GetString("EnemyName", topPlayer.playerID.ToString());
        PlayerPrefs.GetString("PlayerName", lowPlayer.playerID.ToString());
LoadCard();
		Debug.Log("Load Card");
        

    }

    void Update()
    {
        // if (TownManager.instance.atDungeon == true)
        // {
        //     b_GiveUpBtn.gameObject.SetActive(true);
        //     b_LeaveTownBtn.gameObject.SetActive(false);
        // }
        // else
        // {
        //     b_GiveUpBtn.gameObject.SetActive(false);
        //     b_LeaveTownBtn.gameObject.SetActive(true);
        // }
    }

    /// <summary>
    /// Ises the player.
    /// </summary>
    /// <returns><c>true</c>, if player was ised, <c>false</c> otherwise.</returns>
    /// <param name="targetUniqueID">Target unique identifier.</param>
    public bool IsPlayer(int targetUniqueID)
    {
        if (lowPlayer.ID == targetUniqueID
            || topPlayer.ID == targetUniqueID)
        {
            return true;
        }
        else
            return false;
    }

    public void LoadCard()
    {
//      List<GDECardAssetData> allCardData= GDEDataManager.GetAllItems<GDECardAssetData>();
//	  Debug.Log("all card gde"+allCardData.Count);
//
//	  for (int i = 0; i < allCardData.Count; i++)
//	  {
//		  GDECardAssetData ncard = new GDECardAssetData(allCardData[i].Key);
//		  Debug.Log(ncard.CardName + "Load it");
//		  CardAsset nItem = new CardAsset();
//		  nItem.name = ncard.CardName;
//		  nItem.ratityOption = Utils.GetCardRarity(ncard.CardRatity);
//		  nItem.tags = ncard.Tag;
//
//		  nItem.cardDetail = ncard.CardDetail;
//		  nItem.cardSprite = Sprite.Create(ncard.CardSprite,
//			  new Rect(0, 0, ncard.CardSprite.width, ncard.CardSprite.height), Vector3.zero);
//		  nItem.manaCost = ncard.CardMana;
//		  nItem.OverrideLimitOfThisCardsInDeck = ncard.OverrideLimitOfThisCardInDeck;
//		  nItem.typeOfCards = Utils.GetTypeOfCard(ncard.CardType);
//		  nItem.characterAsset = Utils.GetCardCharacterAsset(nItem, ncard.Character);
//
//		  //if Creature
//		  nItem.creatureType = Utils.GetCreatureType(ncard.CreatureType);
//		  nItem.cardAtk = UnityEngine.Random.Range(ncard.CardMinAtk, ncard.CardMaxAtk);
//		  nItem.cardDef = UnityEngine.Random.Range(ncard.CardMinArmor, ncard.CardMaxArmor);
//		  nItem.cardHealth = UnityEngine.Random.Range(ncard.CardMinHealth, ncard.CardMaxHealth);
//		  //
//		  nItem.STR = UnityEngine.Random.Range(ncard.MinStr, ncard.MaxStr);
//		  nItem.DEX = UnityEngine.Random.Range(ncard.MinDex, ncard.MaxDex);
//		  nItem.INT = UnityEngine.Random.Range(ncard.MinInt, ncard.MaxInt);
//		  //
//		  nItem.fireResistance = UnityEngine.Random.Range(ncard.MinFireRes, ncard.MaxFIreRes);
//		  nItem.iceResistance = UnityEngine.Random.Range(ncard.MinIceRes, ncard.MaxIceRes);
//		  nItem.poisonResistance = UnityEngine.Random.Range(ncard.MinPoisonRes, ncard.MaxPoisonRes);
//		  nItem.electronicResistance = UnityEngine.Random.Range(ncard.MinElecRes, ncard.MaxElecRes);
//		  //
//		  nItem.creatureScriptName = ncard.CreatureScriptName;
//		  nItem.specialCreatureAmount = ncard.CreatureAmount;
//		  //Spell
//		  nItem.target = Utils.GetSpellTarget(ncard.SpellTarget);
//		  nItem.spellScriptName = ncard.SpellScripName;
//		  nItem.SpecialSpellAmount = ncard.SpecialSpellAmount;
//
//		  //Others,Aura,
//		  nItem.hasToken = ncard.HasToken;
//		  nItem.tokenCardAsset = ncard.TokenAsset;
//		  nItem.isEnemyCard = ncard.IsEnemyCard;
//		  nItem.hasDET = ncard.HasDamageEffect;
//		  nItem.damageEType = Utils.GetElementalDamageType(ncard.DamageEffectType);
//		  nItem.spellBuffType = Utils.GetSpellType(ncard.SpellType);
//		  nItem.hasRound = ncard.HasRound;
//
//		  //Generate Items
//		  nItem.cardID = CardCollection.instance.allCardsArray.Count;
//		  CardCollection.instance.allCardsArray.Add(nItem);
//	  }
    }
    

    //判断是否为玩家
    public bool CanControlPlayer(AreaPositions owner)
    {
        bool playersTurn = (TurnManager.instance.WhoseTurn == playerInGame[owner]);
        bool NotDrawingCards = !Command.CardDrawPending();
        return playerInGame[owner].playerArea.AllowedToControllThisPlayer &&
               playerInGame[owner].playerArea.ControlsOn && playersTurn && NotDrawingCards;
    }


    /// <summary>
    /// 当前玩家可控制回合
    /// </summary>
    /// <returns><c>true</c>, if control player was caned, <c>false</c> otherwise.</returns>
    /// <param name="player">Player.</param>
    public bool CanControlPlayer(Players owner)
    {
        bool PlayersTurn = (TurnManager.instance.WhoseTurn == owner);
        bool NotDrawingAnyCards = !Command.CardDrawPending();

        return owner.playerArea.AllowedToControllThisPlayer &&
                    owner.playerArea.ControlsOn && PlayersTurn 
                    && NotDrawingAnyCards;
    }

    ///
//    public void ShowConsolePanel(Player p){
//        ConsoleManager.Show();
//        
//    }


    /// <summary>
    /// Enables the end turn button on start.
    /// </summary>
    /// <param name="whsoeTurn">Whsoe turn.</param>
    public void EnableEndTurnButtonOnStart(Players p)
    {
        if ( p==GlobalSetting.instance.lowPlayer)
        {
            // EndTurnButton.FindObjectOfType<Image>().color = Color.grey;
        Debug.Log("Your Turn");
            endTurnText.text = "回合结束";
            EndBtn.interactable=true;
        }else if( p==GlobalSetting.instance.topPlayer){
            Debug.Log("Enemy Turn");
            endTurnText.text="对方回合";
            EndBtn.interactable=false;
        }

    }

    public void ShowPanel()
    {
        
        menuPanel.gameObject.SetActive(true);
        menuPanel.gameObject.transform.DOMove(showpos.transform.position, 0.4f);
      
    }

    //TOOD 1-18
    // Show panel und active 3  effect util player select
    public void ShenShanModule(int defaultNum=3,bool secondPart=false){
        Debug.Log("ShenShan Module");
        if(secondPart==false){
        int round = 0;
        shenshanPrefab.GetComponent<ShenShanModule>().Init(round,defaultNum);
        round++;
        }else{
            //
            GameDebug.Log("Second Player Select Part");
            shenshanPrefab.GetComponent<ShenShanModule>().ShenShanModules(secondPart);
        }
    }

    //Set amount und logs to lp
    public  void SETLogs(string v){
        string txt = v;

        GameDebug.Log(txt);
        //
        // GameObject txtPrefab = Instantiate(tPrefab, logPos.position,Quaternion.identity)as GameObject;
        // txtPrefab.GetComponent<BLog>().ltext.text = txt.ToString();
        // txtPrefab.transform.SetParent(logPos);
        // txtPrefab.transform.localScale=Vector3.one;

    
        // //
        // tpList.Add(txtPrefab);

        logText.text+="\n"+v;


        
    }
    
    public void HidePanel()
        {
           
            menuPanel.gameObject.SetActive(false);
            menuPanel.gameObject.transform.DOMove(returnpos.transform.position, 0.4f);
        }

        public void ClearLP(){
           foreach(var l in tpList){
               if(l!=null){
                   Destroy(l);
               }
           }
        }



    #region Effect Module
    /// <summary>
    /// when cast effect ,generate obj to target 
    /// </summary>
    /// <param name="pos"></param>
    public void ESDBulletShoot(Vector3 pos)
    {

    }

    #endregion
}

