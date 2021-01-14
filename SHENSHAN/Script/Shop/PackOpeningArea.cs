using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


[RequireComponent(typeof(BoxCollider))]
public class PackOpeningArea:MonoBehaviour{

    public static PackOpeningArea instance;
    public bool AllowedToDragAPack{get;set;}

    public GameObject SpellCardFromPackPrefab;
    public GameObject CreatureCardFromPackPrefab;
    //
    public Button DoneButton;
    public Button BackButton;
    public Button OpenNextBtn;  //当存在卡包则激活提示打开

    [Header("Proabilities")]
    [Range(0,1)]
    public float LegendaryProability;
    [Range(0,1)]
    public float EpicProabilit;
    [Range(0,1)]
    public float RareProability;

    [Header("Color")]
    public Color32 LegendaryColor;
    public Color32 EpicColor;
    public Color32 RareColor;
    public Color32 CommonColor;

    private bool isChange;  //是否替换
    private bool canChange;

    [Header("Pack Version")]
    public PackVersion version;

    //
    public Dictionary<CardRatityOption,Color32> GlowColorsByRatity = new Dictionary<CardRatityOption, Color32>();
    //
    public bool giveAtLeastOneRare =true;


    private Vector3 initialPosition = Vector3.one;
    //
    public Transform[] SlotsForCards ;

    private BoxCollider collider;
    private List<GameObject> CardsFromPackCreated = new List<GameObject>();
    private int numberOfCardsOpened = 0;
    public int NumberOfCardsOpenedFromPack{
        get{return numberOfCardsOpened;}
        set{
            numberOfCardsOpened=5;

            if(value == SlotsForCards.Length){
                DoneButton.gameObject.SetActive(true);
            }
        }
    }

    void Awake()
    {
        instance=this;
        collider=GetComponent<BoxCollider>();
        AllowedToDragAPack=true;
        //
        OpenNextBtn = GetComponent<Button>();

        GlowColorsByRatity.Add(CardRatityOption.NORMAL,CommonColor);
        GlowColorsByRatity.Add(CardRatityOption.RARE,RareColor);
        GlowColorsByRatity.Add(CardRatityOption.LEGEND,LegendaryColor);
        GlowColorsByRatity.Add(CardRatityOption.Ancient,EpicColor);

    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //
            ShowPackOpening(initialPosition);

        }


    }

    /// <summary>
    /// Cursors the over area.
    /// </summary>
    /// <returns><c>true</c>, if over area was cursored, <c>false</c> otherwise.</returns>
    public bool CursorOverArea(){
        RaycastHit [] hits;
        hits=Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition),30f);

        bool passedThroughTableCollider=false;
        foreach(RaycastHit h in hits){
            if(h.collider == collider){
                passedThroughTableCollider=true;
            }

        }
        return passedThroughTableCollider;
    }

    /// <summary>
    /// 打开卡牌,包含以下特性
    /// 1.一卡包包含5张卡牌，随机5张必出一张稀有
    /// 2.当卡牌全部开完结算时，有一次换牌机会,对指定卡牌进行替换,当更换稀有,则替换稀有卡牌,如果替换普通卡,则有一定几率出其他稀有程度卡牌
    /// </summary>
    /// <param name="cardInitialPosition">Card initial position.</param>
    public void ShowPackOpening(Vector3 cardInitialPosition){
        CardRatityOption[] ratities = new CardRatityOption[SlotsForCards.Length];
        bool AtLeastOneRareGiven =false;


        //
        for(int i =0; i< ratities.Length;i++){
            float prob=Random.Range(0f,1f);
            if (prob < LegendaryProability)
            {
                ratities[i] = CardRatityOption.LEGEND;
                AtLeastOneRareGiven = true;
            }
            else if (prob < RareProability)
            {
                ratities[i] = CardRatityOption.RARE;
                AtLeastOneRareGiven = true;
            }
            else
                ratities[i] = CardRatityOption.NORMAL;
            AtLeastOneRareGiven = true;

            //当几率不存在稀有卡牌则必出
            if (AtLeastOneRareGiven =false && giveAtLeastOneRare)
            {
                ratities[i] = CardRatityOption.RARE;
                AtLeastOneRareGiven = true;
                isChange = true;

            }

           


        }

        //Add To Collection if want to change card then select same type card 
        //if select the rare card then change same ratityoption card
        //if select the normal card then have  change to turn rare or legend cards
        for(int i=0; i< ratities.Length;i++){
            GameObject card =CardsFromPack(ratities[i]);
            //指定卡牌
            OnecardInformation cardNew = GetComponent<OnecardInformation>();
            canChange = true;
            if (cardNew.cardRatityOption ==ratities[i] && isChange && canChange )
            {
                //替换当前卡牌
                for (int j = NumberOfCardsOpenedFromPack; j <= NumberOfCardsOpenedFromPack; j++)
                {
                    //(typeof(CardAsset))card = cardNew.cardAsset;
                    //
                    if (cardNew.cardRatityOption == CardRatityOption.RARE)
                    {
                        card.transform.DOMove(SlotsForCards[i].position,0.5f);
                        numberOfCardsOpened = j;
                    }

                }
            }

            //替换卡牌

            //
            CardsFromPackCreated.Add(card);
            //
            card.transform.position=cardInitialPosition;

            card.transform.DOMove(SlotsForCards[i].position,0.5f);
        }
    }
    

    /// <summary>
    ///  卡组卡牌,根据卡牌稀有度
    /// </summary>
    /// <returns>The from pack.</returns>
    /// <param name="cardRatityOption">Card ratity option.</param>
    public GameObject CardsFromPack(CardRatityOption cardRatityOption)
    {

        //1.获取卡牌稀有度
        List<CardAsset> cardsOfThisRatity = CardCollection.instance.GetCardWithRatity(cardRatityOption);
        CardAsset c = cardsOfThisRatity[Random.Range(0, cardsOfThisRatity.Count)];

        //2.卡牌数量递增
        CardCollection.instance.QuantityOfEachCards[c]--;

        //3.按照类型分类
        GameObject card = GetComponent<GameObject>();
        if (c.typeOfCards ==TypeOfCards.Creature)
        {
            card = Instantiate(SpellCardFromPackPrefab) as GameObject;
        }else if (c.typeOfCards == TypeOfCards.Spell)
        {

            card = Instantiate(CreatureCardFromPackPrefab) as GameObject;

        }
        //
        card.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        //4.读取资产
        OnecardInformation manager = GetComponent<OnecardInformation>();
        manager.cardAsset = c;
        manager.ReadCardFromAsset();
        return card;
    }

    /// <summary>
    /// 完成卡组添加进卡组收藏
    /// </summary>
    public void Done(){
        AllowedToDragAPack = true;
        //
        NumberOfCardsOpenedFromPack = 0;
        while (CardsFromPackCreated.Count>0)
        {
            //
            GameObject g = CardsFromPackCreated[0];
            //
            CardsFromPackCreated.Remove(g);
            //
            Destroy(g);
        }
        BackButton.interactable = true;
        if (CardsFromPackCreated.Count>0 )
        {
            OpenNextBtn.gameObject.SetActive(true);
        }
       
    }
}