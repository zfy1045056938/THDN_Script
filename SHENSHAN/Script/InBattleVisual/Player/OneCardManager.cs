using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// public class PackInfo{
//     public string packName;
//     public List<CardAsset> cardList;
//     public int packGem;
//     public Sprite packSprite;

    
//     public PackInfo(){}
//     public PackInfo(string p,List<CardAsset> cs,int gem){
//         this.packName= p;
//         this.cardList=new List<CardAsset>();
//         this.packGem=gem;
//     }


// }

/// <summary>
/// One card manager.
/// </summary>
public class OneCardManager : MonoBehaviour {

    //public TextMesh nameTexts;
    public CardAsset cardAsset;
    public OneCardManager previewManager;
    // public Text nameText;
    public TextMeshProUGUI nameText;
    public Text manaCostText;
    // public Text descriptionText;
    public TextMeshProUGUI descriptionText;
    public Text healthText;
    public Text defText;
    public Text atkText;
    // public Text typeText;
    // public TextMeshProUGUI typeText;

    [Header("RibbonImage")]
    public Image cardTopRibbonImage;
    public Image cardLowRibbonImage;
    public Image cardGraphicImage;
    public Image cardBodyImage;
    public Image cardFaceFrameImage;
    public Image cardFaceGlowImage;
    public Image cardBackGlowImage;
    
    public Image ratityOptionImage;
    public Image cardOwnerImage;
    
    //creature stats sprite
    public Image atkSprite;
    public Image defSprite;
    public Image healSprite;
    
    
    
     void Awake()
    {

        // if (cardAsset!=null)
        // {
        //     ReadCardFromAsset();
        // }
    }

     private bool _cardOwner;

     public bool cardOwner
     {
         get { return _cardOwner; }
         set {
           
                 cardOwnerImage.gameObject.SetActive(true);
                 
         }
     }

    //TODO
    private bool canbePlayNow=false;
    public bool CanbePlayNow{
        get { return canbePlayNow; }
        set{
            canbePlayNow = value;
           
            cardFaceGlowImage.gameObject.SetActive(true);
            cardFaceGlowImage.enabled = value;

          
        }
    }

    /// <summary>
    /// Reads the card from asset.
    /// </summary>
    public void ReadCardFromAsset()
    {
        //角色卡牌
//        if (cardAsset.characterAsset != null)
//        {
//            cardBodyImage.color = cardAsset.characterAsset.classCardTint;
//            cardFaceFrameImage.color = cardAsset.characterAsset.classCardTint;
//            cardTopRibbonImage.color = cardAsset.characterAsset.classRibbonsTint;
//            cardLowRibbonImage.color = cardAsset.characterAsset.classRibbonsTint;
//
//        }else{
//            cardFaceFrameImage.color = Color.white;
//        }
           
        //卡牌名字
        nameText.text = cardAsset.name;
      
        //卡牌水晶数
        manaCostText.text = cardAsset.manaCost.ToString();

        //卡牌效果解释
        descriptionText.text = cardAsset.cardDetail;

        //卡牌图像
        cardGraphicImage.sprite = cardAsset.cardSprite;

        
       
       
        
        //卡牌为随从显示数值
        if (cardAsset.typeOfCards == TypeOfCards.Creature)
        {
            // typeText.text = cardAsset.creatureType.ToString();
            
            atkText.text = cardAsset.cardAtk.ToString();
            defText.text = cardAsset.cardDef.ToString();
            healthText.text = cardAsset.cardHealth.ToString();
            
            // LocliaztionType();

        }else{
            Debug.Log("it's spell");
        }
       

        if (previewManager!=null)
        {
            previewManager.cardAsset = cardAsset;

            previewManager.ReadCardFromAsset();
            
        }

        if (ratityOptionImage==null)
        {
            Debug.Log("null object" + gameObject.name);
        }else{


//           ratityOptionImage.DOColor() = RatityColors.instance.colorsDictionary[cardAsset.ratityOption];
        ratityOptionImage.sprite = RatityColors.instance.colorsDictionary[cardAsset.ratityOption];
        }
    }

    // void LocliaztionType(){
    //     switch(cardAsset.creatureType){
    //         case CardType.Human:
    //             typeText.text="人类";
    //             break;
    //         case CardType.Animals:
    //         typeText.text="动物";
    //         break;
    //         case CardType.Building:
    //         typeText.text="建筑";
    //         break;
    //         case CardType.Elements:
    //             typeText.text = "元素";
    //             break;
    //         case CardType.Machine:
    //             typeText.text="机械";
    //             break;

                
    //         case CardType.SeaWheel:
    //             typeText.text ="海轮帮";
    //             break;
    //         case CardType.Qika:
    //             typeText.text="奎卡";
    //             break;
    //         case CardType.Kai:
    //             typeText.text="凯安";
    //             break;
        
    //         default:
    //             typeText.text = "未知";
    //             break;
    //     }
    // }



   
}
