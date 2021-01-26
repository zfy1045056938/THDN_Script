using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Net.Mime;
using UnityEngine.UI;
using Mirror;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using GameDataEditor;


[System.Serializable]
public class CardInfo{
    public CardRatityOption ratityOption;
    
    public int CraftCost;
    
    public int DisenchantOutCome;
}

//建造卡牌页面,包含
//1.基本信息
//2.合成(coin)与分解(dust)
public class CraftingScreen:MonoBehaviour{
    public static CraftingScreen instance;

    public GameObject content;

    public GameObject creatureCard;
    public GameObject statPanel;
    public GameObject spellCard;
    public GameObject radarObj;
    public Text CraftText;

    public Text DiscenchantText;

    public Text QuantityText;
    public Text maxQuText;
    public Text atkText;
    public Text healText;
    public Text ArmorText;
    
    
    // public Text StrText;
    // public Text DexText;
    // public Text IntText;
    public Text SpdText;
    public Text ResText;
    public Text cardDes;
    
    public Text FRText;
    public Text IRText;
    public Text PRText;
    public Text ERText;
    public Text CFText;

    public Text edText;
    public Text EffectDetailText; 
    
    public RadarPolygon rp;
  
    //
    [Header("BTN")] public Button CraftBtn;
    public Button DisconnectBtn;
    public Button CancelBtn;
    public CardInfo[] tradingCostsArray;

    private CardAsset currentCard;
    private int maxCard = 2;
    public Dictionary<CardRatityOption,CardInfo> tradingCost = new Dictionary<CardRatityOption, CardInfo>();
   
    private  PlayerData playerdata;
   
    
    
    public bool Visiable { get; internal set; }

    // public bool visiable{return content.}
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        foreach(CardInfo cost in tradingCostsArray){
            tradingCost.Add(cost.ratityOption,cost);    //according to ratityoption to add dictionary
        }
        
        //
        content.transform.SetAsFirstSibling();
    
    }

    void Start()
    {
        playerdata = FindObjectOfType<PlayerData>();

        // rp = GameObject.FindGameObjectWithTag("RadarData").GetComponent<RadarPolygon>();
        
    }

    void Update()
    {
//        if (content.activeSelf)
//        {
//            CraftBtn.onClick.AddListener(() =>
//            {
//                CraftCurrentCard();
//              
//            });
//            
//            DisconnectBtn.onClick.AddListener(() =>
//            {
//                DisenchantCurrentCard();
//                
//            });
//            
//            CancelBtn.onClick.AddListener(() =>
//            {
//                content.SetActive(false);
//            });
//        }

if(!EventSystem.current.IsPointerOverGameObject()){
    Debug.Log("No RayCast");
}

        
        
    }
    

    /// <summary>
    /// Shows the craft screen.
    /// CRAFTCARD VALUE
    /// 1.JUNK CARD ->C:
    /// </summary>
    /// <param name="cardToShow">Card to show.</param>
    public void ShowCraftScreen(CardAsset cardToShow){
        
       
        //
        currentCard = cardToShow;
      
        //
        GameObject cardObject=null;
        if(currentCard.typeOfCards ==TypeOfCards.Creature){
            cardObject=creatureCard;
            
            creatureCard.SetActive(true);
            spellCard.SetActive(false);
//            radarObj.gameObject.SetActive(true);
statPanel.gameObject.SetActive(true);
        }else if(currentCard.typeOfCards ==TypeOfCards.Spell){
            cardObject=spellCard;
           
            radarObj.SetActive(false);
            creatureCard.SetActive(false);
            spellCard.SetActive(true);
            statPanel.gameObject.SetActive(false);
            radarObj.gameObject.SetActive(false);
        }
   
        
       

        //OneCardShowPage
        OneCardManager manager=cardObject.GetComponent<OneCardManager>();
        manager.cardAsset=cardToShow;
        manager.ReadCardFromAsset();
        
        //show stats
        rp.LoadCardAsset(currentCard);
        // StrText.text = cardToShow.STR.ToString();
        // DexText.text = cardToShow.DEX.ToString();
        // IntText.text = cardToShow.INT.ToString();
        //
        atkText.text = cardToShow.cardAtk.ToString();
        healText.text = cardToShow.cardHealth.ToString();
        ArmorText.text = cardToShow.cardDef.ToString();
//        SpdText.text = cardToShow.SPD.ToString();
        FRText.text = cardToShow.fireResistance.ToString();
        IRText.text = cardToShow.iceResistance.ToString();
        PRText.text = cardToShow.poisonResistance.ToString();
        ERText.text = cardToShow.electronicResistance.ToString();
        CFText.text = cardToShow.cardFrom.ToString();
        maxQuText.text = maxCard.ToString();
        //ed

           
       

        if (currentCard.cardDetail != "")
        {
            cardDes.text = cardToShow.cardDetail.ToString();
        }
        else
        {
            cardDes.text = "NULL";
        }

        //CradtCost
        CraftText.text="制作需要"+tradingCost[cardToShow.ratityOption].CraftCost.ToString()+"材料";
        //DisencantCost
        DiscenchantText.text = "分解获得" + 
        tradingCost[cardToShow.ratityOption].DisenchantOutCome+"材料";

        //GetEffect
        List<GDECardEffectGuideData> ggd  = GDEDataManager.GetAllItems<GDECardEffectGuideData>();
        for(int i=0;i< ggd.Count;i++){
           if(cardToShow.cardDetail.Substring(0,cardToShow.cardDetail.LastIndexOf(":")) ==  ggd[i].GName ){
               //
               Debug.Log(cardToShow.cardDetail.Substring(0,cardToShow.cardDetail.LastIndexOf(":"))+"got");
               EffectDetailText.text=ggd[i].GDetail.ToString();
           } 
        }

        
    UpdateQuantityOfCurrentCard();
        //
        content.SetActive(true);      
        
    }


    /// <summary>
    /// Updates the quantity of current card.
    /// </summary>
    public void UpdateQuantityOfCurrentCard(){
        int AmountOfThisCardInYourCollection = CardCollection.instance.QuantityOfEachCards[currentCard];
        QuantityText.text = AmountOfThisCardInYourCollection.ToString();
        
        
        DeckBuilderScreen.instance.collectionBroswerScript.UpdatePage();
    }

    /// <summary>
    /// Crafts the current card.
    /// </summary>
    public void CraftCurrentCard(){
        bool canCraft=false;
     
            if ( !canCraft)
            {
                if (playerdata.dust >= tradingCost[currentCard.ratityOption].CraftCost &&
                    CardCollection.instance.QuantityOfEachCards[currentCard] <
                    CardCollection.instance.DefaultNumberOfBasicCards
                )
                {
                   
                    playerdata.dust -= tradingCost[currentCard.ratityOption].CraftCost;
                    CardCollection.instance.QuantityOfEachCards[currentCard]+=1;
                   UpdateQuantityOfCurrentCard();
                    QuantityText.text = CardCollection.instance.QuantityOfEachCards[currentCard].ToString();
                    Debug.Log("Craft success das card has "+CardCollection.instance.QuantityOfEachCards[currentCard].ToString());
                }
                else
                {
                    new ShowMessageCommand("没有足够的碎息激活",0.3f).AddToQueue();
                }
            }
           
            CardCollection.instance.SaveQuantityOfCardsIntoPlayerPrefs();
     

    }


    /// <summary>
    /// Disenchants the current card.
    /// </summary>
    public void DisenchantCurrentCard(){
        
            if (CardCollection.instance.QuantityOfEachCards[currentCard]>0&& currentCard.ratityOption!=CardRatityOption.NORMAL)
            {
                CardCollection.instance.QuantityOfEachCards[currentCard]-=1;
               
                playerdata.dust = Mathf.Clamp(tradingCost[currentCard.ratityOption].DisenchantOutCome+playerdata.dust,tradingCost[currentCard.ratityOption].CraftCost,
                tradingCost[currentCard.ratityOption].CraftCost+playerdata.dust);
                UpdateQuantityOfCurrentCard();
                QuantityText.text=CardCollection.instance.QuantityOfEachCards[currentCard].ToString();

                foreach (DeckInfo info in DeckStorge.instance.AllDecks)
                {
                    while (info.NumberOfthisCardInDeck(currentCard)>CardCollection.instance.QuantityOfEachCards[currentCard])
                    {
                        info.cardAssets.Remove(currentCard);
                    }
                }
                
                //
                while (DeckBuilderScreen.instance.buildScript.InDeckBuildingMode && DeckBuilderScreen.instance.buildScript.NumberOfThisCardInDeck(currentCard)>CardCollection.instance.QuantityOfEachCards[currentCard])
                {
                    DeckBuilderScreen.instance.buildScript.RemoveCard(currentCard);
                }

            }
            else
            {
                new ShowMessageCommand("普通卡牌无法失去记忆",0.5f).AddToQueue();
            }
            CardCollection.instance.SaveQuantityOfCardsIntoPlayerPrefs();
        
    }

    
}