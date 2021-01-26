using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine.EventSystems;
using GameDataEditor;

/// <summary>
/// Add card to deck.
/// </summary>
public class AddCardToDeck :  MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler{

    public Text quantityText;
    private float initalScale; 
    private float scaleFactor = 1.1f;
    //[SerializeField]
    //public int limitOfPack = 40;    //卡牌最大限制
    //public List<CardAsset> packs =new List<CardAsset>();
    //private Dictionary<int, CardAsset> packDic = new Dictionary<int,CardAsset>();
    //public int limitOfPackDeck = 10;//牌桌卡组最大限制
    //public int limitOfPackSingleCard = 3;   //卡组单卡限制数
    //
    public CardAsset cardAsset;
    private CraftingScreen craftScreen;
    public OneCardManager cardManager;
    public GameObject EObj;
    public Text EffectText;

    public int tmpCardCount=0;
    private void Awake()
    {
        initalScale = transform.localScale.x;
        craftScreen =GameObject.FindObjectOfType<CraftingScreen>();
        cardManager = FindObjectOfType<OneCardManager>();
    }




    public void SetCardAsset(CardAsset asset) {  cardAsset= asset; }


    void Update()
    {
        if (DeckBuilderScreen.instance.buildScript.InDeckBuildingMode==false&&TownManager.instance.atDungeon==false)
        {
            if(CardCollection.instance.QuantityOfEachCards.ContainsKey(cardAsset)){
            quantityText.text =  CardCollection.instance.QuantityOfEachCards[cardAsset].ToString();
            }else{
              
            }
        }
    }


    /// <summary>
    /// Updates the quantity.
    /// </summary>
    public void UpdateQuantity()
    {
       
        int quantity = CardCollection.instance.QuantityOfEachCards[cardAsset];
       
//        int quantity = cardAsset.cardNumber;
        //
        if (DeckBuilderScreen.instance.buildScript.InDeckBuildingMode &&
            DeckBuilderScreen.instance.showReduceQuantitiesInDeckBuilding)
        {

            quantity -= DeckBuilderScreen.instance.buildScript.NumberOfThisCardInDeck(cardAsset);
//            quantity -= cardAsset.cardNumber;
            //  
            quantityText.text = "X" + quantity;

        }
      
    }

 

    public void OnPointerEnter(PointerEventData eventData)
    {

        this.transform.DOScale(initalScale * scaleFactor, 1.1f);
//        if (cardManager.previewManager != null)
//        {
//            cardManager.previewManager.gameObject.SetActive(true);
//        }
     
    //    List<GDECardEffectGuideData> geg = GDEDataManager.GetAllItems<GDECardEffectGuideData>();
    //    for(int i=0;i<geg.Count;i++){
    //         if(cardAsset.cardDetail.Substring(0,cardAsset.cardDetail.LastIndexOf(":")) ==  geg[i].GName ){
    //            //
    //            if(EObj!=null){
    //              EObj.gameObject.SetActive(true);
    //         //    Debug.Log(cardAsset.cardDetail.Substring(0,cardAsset.cardDetail.LastIndexOf(":"))+"got");
    //            EffectText.text=geg[i].GDetail.ToString();
    //            }
    //        } 
    //    }

    }

    /// <summary>
    /// Hide CardPreview und scale initial
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.DOScale(initalScale, 1f);
//        cardManager.previewManager.gameObject.SetActive(false);
EObj.gameObject.SetActive(false);
    }

    /// <summary>
    /// Ons the pointer click.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (eventData.button==PointerEventData.InputButton.Left)
        {
            //sound
//            SoundManager.instance.PlaySound();
            SoundManager.instance.PlayRndMusic();

            if(TownManager.instance.atDungeon==false){
            Debug.Log("Click\t\t" + cardAsset + "it");
            CardAsset asset = GetComponent<OneCardManager>().cardAsset;
            //Draft Module
            if( DeckBuilderScreen.instance.draft==true){
                //Check Rarity
                if(cardAsset.ratityOption!=CardRatityOption.RARE || cardAsset.ratityOption!=CardRatityOption.NORMAL ){
                    //rare card add count
                    DraftManager.instance.currEpic++;
                    if(DraftManager.instance.currEpic>=DraftManager.instance.MaxepicCount){
                        DraftManager.instance.enoughEpic=true;
                    }
                }
                //Select Target Card And Add to Deck
                 Debug.Log("Add Draft Card to deck");
                 //Check is high than rare
                 if(asset.ratityOption != CardRatityOption.NORMAL || asset.ratityOption!=CardRatityOption.RARE){
                     //Add To Count
                     DraftManager.instance.currEpic ++;

                     if(DraftManager.instance.currEpic>=2){
                         DraftManager.instance.enoughEpic=true;
                     }
                 }
                 
                DraftManager.instance.hasSelect=true;
                CardCollection.instance.DraftAddQuality(cardAsset);
                 DeckBuilderScreen.instance.buildScript.AddCard(asset);
//                 DraftManager.instance.selectCardList.Add(this.gameObject);
                UpdateQuantity();

             }else{
                 Debug.Log("Not Draft");
            //Not Draft Add Card To pack
            if (asset == null)
            {
                return;
            }
            //

            
            if (CardCollection.instance.QuantityOfEachCards[cardAsset]-
                DeckBuilderScreen.instance.buildScript.NumberOfThisCardInDeck(cardAsset) > 0
              )
            {
                DeckBuilderScreen.instance.buildScript.AddCard(asset);
                UpdateQuantity();
            }
            else
            {
             MessageManagers.instance.AddMessage("卡牌数量不足",0.4f);
            }
             }
           

            }else{
                //Add to deck from tmp collection
                  CardAsset asset = GetComponent<OneCardManager>().cardAsset;
                  //
                  Debug.Log("add to card collection");
                  DungeonCardEdit.instance.AddToCard(cardAsset);
          
            }
        }else if (eventData.button== PointerEventData.InputButton.Right)
        {
            // Debug.Log("Select Card"+cardAsset.name);
            SoundManager.instance.PlayRndMusic();
            CraftingScreen.instance.ShowCraftScreen(GetComponent<OneCardManager>().cardAsset);
        }

    }
}
