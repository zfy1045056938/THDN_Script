using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;



/// <summary>
/// 单张卡牌的基本信息描述
/// </summary>
public class OnecardInformation : MonoBehaviour
{
    public static OnecardInformation instance;
    //
    public int cardId;
    //卡牌名称
    public Text cardName;
    //卡牌血量
    public Text cardHealth;
    //卡牌防御
    public Text cardDef;
    //卡牌解释
    public Text cardDescription;
    //卡牌稀有图像
    public Text cardManacost;
    public Text creatureSpeed;
    public Text creatureDef;
    public Text atkText;
    public Text defText;


    //卡牌稀有度
    public CardRatityOption cardRatityOption;
    public CreatureType creatureType;
    public CardType cardType;
    /// <summary>
    /// 图像
    /// </summary>
    public Image cardHeadImage; //头像
    public Image cardBackImage; //卡背
    public Image cardFrameImage;  //卡框架
    public Image cardRatityOptionImage;  //卡稀有程度图像
    public Image cardMainAtkType;   //卡牌主要攻击类型(力量/魔法/敏捷)
    public Image cardTopRibbonImage;
    public Image cardLowRibbonImage;
    public Image cardGrphicImage;
    public Image cardFaceGlowImage; //highlight
    //



    public OnecardInformation previewManager;

    public CardType spellType;
    public CardEffects cardEffect;

    //public List<CardAsset> cardAssets = new List<CardAsset>();
    public CardAsset cardAsset;
    public bool canBePlayNow;

    private bool _canAtkNow = false;
    public bool canAtkNow{
        get{
            return _canAtkNow;
        }

        set{

            _canAtkNow = value;

            cardFaceGlowImage.enabled = value;
        }

    }
    //public ItemClass cardItem;

    [Header("Craft Required")]
    public int costAmonut=1;

    public void Awake()
    {
        instance = this;
        if (cardAsset != null)
        {
            ReadCardFromAsset();
        }
    }

    /// <summary>
    /// Reads the card from asset. TODO
    /// </summary>
    public void ReadCardFromAsset()
    {
        if (cardAsset.characterAsset != null)
        {
            // cardHeadImage.color = cardAsset.characterAsset.classCardTint;
            // cardFrameImage.color = cardAsset.characterAsset.classCardTint;
            // cardTopRibbonImage.color = cardAsset.characterAsset.classRibbonsTint;
            // cardLowRibbonImage.color = cardAsset.characterAsset.classRibbonsTint;
        }
        else
        {
            cardFrameImage.color = Color.white;
        }

        //读取卡牌名字
        cardName.text = cardAsset.name;
        //水晶花费
        cardManacost.text = cardAsset.manaCost.ToString();
        //解释
        cardDescription.text = cardAsset.cardDetail;
        //图像
        cardGrphicImage.sprite = cardAsset.cardSprite;
        //
        if (cardAsset.typeOfCards == TypeOfCards.Creature)
        {
            atkText.text = cardAsset.cardAtk.ToString();
            defText.text = cardAsset.cardDef.ToString();
            cardHealth.text = cardAsset.cardHealth.ToString();
        }
        //
        if (previewManager != null)
        {
            previewManager.cardAsset = cardAsset;
            previewManager.ReadCardFromAsset();

        }

        //
        if (cardRatityOptionImage == null)
        {
            cardRatityOptionImage.sprite = RatityColors.instance.colorsDictionary[cardAsset.ratityOption];
        }
    }

}
