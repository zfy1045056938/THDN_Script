using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;
using UnityEngine.UI;
//different of the 
public class DungeonCardEdit : MonoBehaviour
{
    public UIPanel content;
    public static DungeonCardEdit instance;
  public DeckBuilder deckBuilder;

  public Transform dungeonCardPos;
  //obj for save
  public List<GameObject> tcObj;
  public GameObject creatureCard;
  public GameObject spellCard;
  //
  public GameObject doneBtn;
  public Button DCBtn;
  
  public Dictionary<CardAsset,int>tc = new Dictionary<CardAsset, int>();

  void Start(){
      instance=this;
  }

void Update(){
    if(!Utils.IsClickUI())return;
}


  public void ShowDungeonCollection(){
      content.gameObject.SetActive(true);
      DCBtn.enabled=false;
      deckBuilder.amountOfCardsInDeck= 10;
      deckBuilder.InDeckBuildingMode=true;
    
            deckBuilder.BuildADeckFor(BattleStartInfo.SelectDeck.characterAsset );
            deckBuilder.DeckName.text =BattleStartInfo.SelectDeck.deckName;
            foreach(CardAsset asset in BattleStartInfo.SelectDeck.cardAssets){
                deckBuilder.AddCard(asset);
                }

             GameObject cObj =null;
            //Show tmp Collection
            // if(tmpCard.Count>0){
            //     for(int i=0;i<tmpCard.Count;i++){
                   
            //         if(tmpCard[i].GetComponent<OneCardManager>().cardAsset.typeOfCards == TypeOfCards.Creature){
            //              cObj = Instantiate(creatureCard,dungeonCardPos.position,Quaternion.identity)as GameObject;
            //                  }else if(tmpCard[i].GetComponent<OneCardManager>().cardAsset.typeOfCards == TypeOfCards.Spell){
            //              cObj = Instantiate(spellCard,dungeonCardPos.position,Quaternion.identity)as GameObject;
            //         }
            //          cObj.transform.SetParent(dungeonCardPos);
            //             cObj.GetComponent<OneCardManager>().cardAsset = tmpCard[i].GetComponent<OneCardManager>().cardAsset;
            //             //
            //             tcObj.Add(cObj);
            //     }
            // }else{
            //     Debug.Log("No Card At tmp Collection");

            // }
            
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="card"></param>
  public void AddCollectionAsTemp(OneCardManager card){
 int count = 0;
      //Check tmp has Card else add count
     if(card != null && card.name!="" && !tc.ContainsKey(card.cardAsset))
     {
        
         GameObject obj = null;
         if (card.cardAsset.typeOfCards == TypeOfCards.Creature)
         {
              obj = Instantiate(creatureCard,dungeonCardPos.position,Quaternion.identity)as GameObject;
             
         }
         else
         {
              obj = Instantiate(spellCard,dungeonCardPos.position,Quaternion.identity)as GameObject;

         }
         obj.transform.SetParent(dungeonCardPos);
         obj.GetComponent<OneCardManager>().cardAsset = card.cardAsset;
         obj.GetComponent<OneCardManager>().ReadCardFromAsset();
         
         //add tmp dic
         tc.Add(card.cardAsset,count);

         obj.GetComponent<AddCardToDeck>().cardAsset = card.cardAsset;
        
         //
         if (!tc.ContainsKey(card.cardAsset))
         {
             count = 1;
             //New Count
             tc.Add(card.cardAsset,count);
         }
         else
         {
            //has asset then add count
            tc[card.cardAsset] += 1;
             obj.GetComponent<AddCardToDeck>().quantityText.text = tc[card.cardAsset].ToString();
        
         }
         tcObj.Add(obj);


     }else{
         //Add Count to target card
         tc[card.cardAsset]+=1;
         
     }
  }

  /// <summary>
  /// 确认临时牌库卡牌数量,当卡牌数量为一获取时,删除对象.dic 清楚
  /// </summary>
  /// <param name="asset"></param>
public void AddToCard(CardAsset asset){
    //check current and add 1 to collect if quailty -1 ==0 destory obj
    int count = deckBuilder.NumberOfThisCardInDeck(asset);
    for (int i = 0; i < tcObj.Count; i++)
    {
        if (tcObj[i] != null)
        {
            //tmp collection equal pack
            if (tcObj[i].GetComponent<OneCardManager>().cardAsset == asset)
            {
                if (deckBuilder.NumberOfThisCardInDeck(asset) <= 2)
                {
                    count = deckBuilder.NumberOfThisCardInDeck(asset);
                    int limitOfThisCardInDeck = deckBuilder.sameCardLimit;


                    if (asset.OverrideLimitOfThisCardsInDeck > 0)
                    { 
                        limitOfThisCardInDeck = asset.OverrideLimitOfThisCardsInDeck;
                    }
                    
                    //Add Cards rebbon
                    if (count < limitOfThisCardInDeck)
                    {
                        deckBuilder.deckList.Add(asset);

                        count++;

                        if (deckBuilder.ribbons.ContainsKey(asset))
                        {
                            deckBuilder.ribbons[asset].SetQuantity(count);
                            //
                            if (tc[asset] > 0)
                            {
                                tc[asset]--;
                                tcObj[i].GetComponent<AddCardToDeck>().quantityText.text = tc[asset].ToString();
                                //
                                if ( tc[asset]==0)
                                {
                                    tc.Remove(asset);
                                    if (tcObj[i] != null)
                                    {
                                        Destroy(tcObj[i]);
                                    }
                                }
                            }
                            // else
                            // {
                            //     //destory 
                            //     tc.Remove(asset);
                            // }
                        }
                        else
                        {

                            //create new
                            Debug.Log(asset.name.ToString()+"ADD INSTANCE");
                            GameObject cardName =
                                Instantiate(deckBuilder.CardNamePrefab, deckBuilder.Content) as GameObject;
                            cardName.transform.SetAsLastSibling();
                            cardName.transform.localScale = Vector3.one;
                            CardNameRibbon ribbon = cardName.GetComponent<CardNameRibbon>();
                            ribbon.ApplyAsset(asset, count);
                            deckBuilder.ribbons.Add(asset, ribbon);
                            Debug.Log("DECK BUDILER HAS"+deckBuilder.ribbons[asset]);
                            if (tc[asset] > 0)
                            {
                                tc[asset]--;
                                tcObj[i].GetComponent<AddCardToDeck>().quantityText.text = tc[asset].ToString();
                                //
                                if ( tc[asset]==0)
                                {
                                    if (tcObj[i] != null)
                                    {
                                        Destroy(tcObj[i]);
                                    }
                                    tc.Remove(asset);
                                }
                            }
                            // else
                            // {
                            //     //destory 
                            //     tc.Remove(asset);
                                
                            // }

                        }
                    }
                    //dic -=1 tmp collections
//                    if (tc.ContainsKey(asset))
//                    {
//                        if (tcObj[i].GetComponent<OneCardManager>().cardAsset == asset)
//                        {
//                            if (deckBuilder.NumberOfThisCardInDeck(asset)==1 && tc[asset]==1)
//                            {
//                                if (tcObj[i] != null)
//                                {
//                                    Destroy(tcObj[i]);
//                                }
//
//                         
//                            }
//                        }
//                    }
                }
               
            }
            else
            {
                
            }
            

            //卡组限制为两张
            // if (tcObj[i].GetComponent<AddCardToDeck>().tmpCardCount == 1 )
            // {
            //     //
            //     foreach (var g in tcObj)
            //     {
            //         if (g != null)
            //         {
            //             if (g.GetComponent<OneCardManager>().cardAsset == asset && g != null)
            //             {
            //                 Destroy(g);
            //             }
            //         }
            //     }
            // }
        }
    }

  }

public void RemoveCardToTmp(CardAsset asset)
{
    Debug.Log("RemoveCard");
   int counter=0;
    GameObject obj = null;
    CardNameRibbon ribbonToRemove = deckBuilder.ribbons[asset];
    //
    ribbonToRemove.SetQuantity(ribbonToRemove.Quantity - 1);
    //
    if (deckBuilder.NumberOfThisCardInDeck(asset) == 1)
    {
        //check tmp has same card
        deckBuilder.ribbons.Remove(asset);
        //
        Destroy(ribbonToRemove.gameObject);
    }
       //临时不存在相同卡牌且数量等于0m new instance
    if (!tc.ContainsKey(asset))
    {
        Debug.Log(asset.name.ToString()+"Add TMP ");
         counter = 0;
        //Add Card to tmp from deck with number 
        if (asset.typeOfCards == TypeOfCards.Creature)
        {
            obj = Instantiate(creatureCard, dungeonCardPos.position, Quaternion.identity) as GameObject;

        }
        else if (asset.typeOfCards == TypeOfCards.Spell)
        {
            obj = Instantiate(spellCard, dungeonCardPos.position, Quaternion.identity) as GameObject;

        }

        obj.transform.SetParent(dungeonCardPos);
        obj.GetComponent<OneCardManager>().cardAsset = ribbonToRemove.asset;
        obj.GetComponent<OneCardManager>().ReadCardFromAsset();


        //add card
        counter++;
        obj.GetComponent<AddCardToDeck>().cardAsset = ribbonToRemove.asset;
        obj.GetComponent<AddCardToDeck>().tmpCardCount += 1;
        tc.Add(asset,counter);
        obj.GetComponent<AddCardToDeck>().quantityText.text = tc[asset].ToString();
 

        //
        tcObj.Add(obj);
      
    }else{
        Debug.Log("has tc");
        for (int i = 0; i < tcObj.Count; i++)
        {
            if (tcObj[i] != null)
            {
                //存在而且相同卡牌
                if (tc.ContainsKey(asset) && tcObj[i] != null &&
                    tcObj[i].GetComponent<OneCardManager>().cardAsset == asset)
                {
                    tc[asset] ++;
                    tcObj[i].GetComponent<AddCardToDeck>().tmpCardCount += 1;
                    tcObj[i].GetComponent<AddCardToDeck>().quantityText.text =
                       tc[asset].ToString();
                       Debug.Log("\n\n"+asset.name.ToString()+"TMP CARD COLLECTION AT TMP IS"+tc[asset].ToString());
                }
            }
        }
    }

    //三种情况,
    //1.临时存在相同卡牌则添加
    //2.临时不存在卡牌创建临时卡牌存储
    //3.卡组数为1 直接添加并删除卡组ribbon
    // for (int i = 0; i < tcObj.Count; i++)
    //     {
    //         if (tcObj[i] != null)
    //         {
    //             //存在而且相同卡牌
    //             if (tc.ContainsKey(asset) && tcObj[i] != null &&
    //                 tcObj[i].GetComponent<OneCardManager>().cardAsset == asset)
    //             {
    //                 tc[asset] ++;
    //                 tcObj[i].GetComponent<AddCardToDeck>().tmpCardCount += 1;
    //                 tcObj[i].GetComponent<AddCardToDeck>().quantityText.text =
    //                    tc[asset].ToString();
    //             }
    //         }
    //     }


    //
    deckBuilder.deckList.Remove(asset);
    
}

}


