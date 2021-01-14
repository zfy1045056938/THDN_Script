using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

//卡牌收藏管理，用于调用页面和资源管理,包含
//1.卡牌管理
//2.角色
//3.条件查询
//4.单张卡牌查看
//5.随从菜单管理
public class CollectionBrower : MonoBehaviour
{

    // public Transform[] slot;
    public List<GameObject> cardObj;
    public Transform slot;
    public Transform packSlot;
    //
    public GameObject spellMenuPrefab;      //分解页面
    public GameObject creaturePrefab;       //随从

    public GameObject creatureMenuPrefab;   //显示单个随从包含信息,建立，删除,强化
   

    public GameObject oneCharacterTabs;
    public GameObject allCharacterTabs;
    public GameObject ratityTabs;
   
   public Text curr;
   public Text total;
    public ManaFiliter manaFiliter;
    //public KeyWordInputField keywordInputField;
    public CardToogle toggle;
    private List<GameObject> CreatedCards = new List<GameObject>();
    private CharacterFiliterTabs tabs;
    public static CollectionBrower instance;
    //
    public GameObject nextObj;
    public GameObject previewObj;



    #region    筛选条件，包含各种条件以及页面调度

    private bool _showingCardsPlayerDoesNotOwn = false;
    public bool showingCardsPlayerDoesNotOwn
    {
        get { return _showingCardsPlayerDoesNotOwn; }
        set
        {
         
            _showingCardsPlayerDoesNotOwn = value;
            UpdatePage();
        }
    

    }
    private int _pageIndex =1;
    public int pageIndex{
        get{return _pageIndex;}
        set{
            _pageIndex=value;
            
            UpdatePage();
        }
    }

    private bool _includeAllRatities=true;
    public bool includeAllRatities{
        get{return _includeAllRatities; }
        set{
            _includeAllRatities=value;
            UpdatePage();
        }
    }

    private CardRatityOption _ratity;
    public CardRatityOption ratity{
        get { return _ratity; }
        set{
            _ratity = value;
            UpdatePage();
        }
    }
    private bool _includeAllCharacter =true;
    public bool includeAllCharacter{
        get{return _includeAllCharacter;}
        set{
            _includeAllCharacter=value;
            pageIndex = 0;
            UpdatePage();
        }

    }


  
    private int _manaCost;
    public int ManaCost{
        get{return _manaCost;}
        set{

            _manaCost = value;
 _pageIndex = 0;
            UpdatePage();
        }
    }

    private bool _includeTokenCards =false;
    public bool includeTokenCards{
        get{
            return _includeTokenCards;
        }
        set{
            _includeTokenCards=value;
            UpdatePage();
        }
    }

    
   
//
//    private CharacterAsset _characterAsset= null ;
//   
//    public CharacterAsset characterAsset{
//        get{
//            return _characterAsset;
//        }
//        set{
//            _characterAsset=value;
//            _pageIndex =0;
//            UpdatePage();
//        }
//    }
    private PlayerJob _characterAsset =PlayerJob.None  ;

    public PlayerJob characterAsset
    {
        get
        {
            return _characterAsset; 
            
        }
        set
        {
            _characterAsset = value;
            _pageIndex = 0;
            UpdatePage();
        }
    }
    

    /// <summary>
    /// 搜索框搜索关键字
    /// </summary>
    private string _keyword=" ";
    public  string Keyword{
        get{
            return _keyword;
        }
        set{
            //_keyword = keywordInputField.inputField.text;
            _keyword = value;
           
            UpdatePage();
        }
    }

   

    #endregion


    private void Awake()
    {
        instance = this;
    }

   void Update(){
       curr.text =(pageIndex+1).ToString();
   }

    /// <summary>
    /// Shows the collection for browsing.
    /// </summary>
    public void ShowCollectionForBrowsing(){
        DeckBuilderScreen.instance.tabScript.CreateTabs();
        //KeyWordInputField.instance.Clear();
        toggle.SetValue(false);
        manaFiliter.RemoveAllFiliter();

        //显示卡牌 
        ShowCard(false,0,true,false,-1,null,PlayerJob.None,false);
        //default neu 
        DeckBuilderScreen.instance.tabScript.SelectTab(DeckBuilderScreen.instance.tabScript.neutralTabWhenCollectionBrowsing,instant:true);
        // DeckBuilderScreen.instance.tabScript.neutralTabWhenCollectionBrowsing.GetComponent<CharacterFiliterTabs>().Select(instant: true);
    }


   /// <summary>
   /// 显示构筑卡组页面
   /// </summary>
   /// <param name="buildingForScreen">Building for screen.</param>
    public void ShowCollectionForDeckBuilding(CharacterAsset buildingForScreen){
        //keywordInputField.Clear();
        Debug.Log(buildingForScreen.className.ToString()+"Edit mode");
       toggle.SetValue(false);
        manaFiliter.RemoveAllFiliter();
        
        //
//        _characterAsset = buildingForScreen;
        characterAsset = buildingForScreen.jobs;
        //TODO n
        ShowCard(true, 0, true, false, -1,null,characterAsset,false);

        //
        // DeckBuilderScreen.instance.tabScript.classTabs.GetComponent<CharacterFiliterTabs>().Select(true);
        //Create Clases Tabs
        // DeckBuilderScreen.instance.tabScript.CreateClassTabs(characterAsset.ToString());
        //
    DeckBuilderScreen.instance.tabScript.SelectTab(DeckBuilderScreen.instance.tabScript.classTabs, instant: true);
       
    }

 


    /// <summary>
    ///   分页显示卡组信息,一页显示10张
    /// </summary>
    /// <param name="showingCardsPlaysDoesNotOwn">If set to <c>true</c> showing cards plays does not own.</param>
    /// <param name="pageIndex">Page index.</param>
    /// <param name="includeAllRatities">If set to <c>true</c> include all ratities.</param>
    /// <param name="ratity">Ratity.</param>
    /// <param name="includeAllCharacter">If set to <c>true</c> include all character.</param>
    /// <param name="manaCost">Mana cost.</param>
    /// <param name="keyword">Keyword.</param>
    /// <param name="asset">Asset.</param>

    public void ShowCard(bool showingCardsPlaysDoesNotOwn=false,
    int pageIndex=0,bool includeAllRatities=false,
                         bool includeAllCharacter = false,int manaCost=-1,string keyword="",PlayerJob asset=PlayerJob.None,
                         bool includeTokenCards=false,CardRatityOption ratity = CardRatityOption.NORMAL){
        //saveing information to the showing Packs 
        //绑定卡牌属性并显示

        //bind class
        _includeAllCharacter = includeAllCharacter;
        _includeTokenCards = includeTokenCards;
        _includeAllRatities = includeAllRatities;
        _pageIndex = pageIndex;
        _manaCost = manaCost;
        _ratity = ratity;
        _showingCardsPlayerDoesNotOwn = showingCardsPlaysDoesNotOwn;
        _keyword = keyword;
        _characterAsset = asset;
//        _characterAsset=asset;
        
        

        //Show card limit of the page 
        List<CardAsset> CardsOnThisPage = PageSelection(showingCardsPlaysDoesNotOwn,pageIndex,
            includeTokenCards,manaCost,includeAllCharacter,
            includeAllRatities,keyword,asset);

        
        ClearCreatedCards();
        //
        if (CardsOnThisPage.Count==0)
        {
            return;
        }
        //
        for (int i = 0; i < CardsOnThisPage.Count; i++)
        {
          
            
            //作为实例化物体
            GameObject newMenuCard = null;
            //分类显示卡牌
            if (CardsOnThisPage[i].typeOfCards==TypeOfCards.Creature)
            {
                newMenuCard = Instantiate(creaturePrefab, slot.position,Quaternion.identity) as GameObject;
                newMenuCard.transform.SetParent(slot);
            }
            
            else if(CardsOnThisPage[i].typeOfCards==TypeOfCards.Spell)
            {
                // not generate this time
                // newMenuCard = Instantiate(spellMenuPrefab, slot.position, Quaternion.identity) as GameObject;
                // newMenuCard.transform.SetParent(slot);
            }

            if (newMenuCard != null)
            {
                //写入父级位置
                newMenuCard.transform.SetParent(packSlot);
                //newMenuCard.transform.localScale = this.transform.localScale;
                //添加卡牌
                CreatedCards.Add(newMenuCard);
                //

                OneCardManager manager = newMenuCard.GetComponent<OneCardManager>();


                manager.cardAsset = CardsOnThisPage[i];
                //init card
                        manager.ReadCardFromAsset();
                        AddCardToDeck adtoDeck = newMenuCard.GetComponent<AddCardToDeck>();
                        adtoDeck.SetCardAsset(CardsOnThisPage[i]);
                        adtoDeck.UpdateQuantity(); 
            }
            //
            cardObj.Add(newMenuCard);
        }

    }



    /// <summary>
    /// 分页查询下一页
    /// </summary>
    public void Next(){
        if (PageSelection(_showingCardsPlayerDoesNotOwn, _pageIndex + 1, _includeTokenCards, _manaCost, _includeAllCharacter, _includeAllRatities,
                           _keyword, _characterAsset).Count == 0)
          return;
          //n
        ShowCard(true, _pageIndex + 1, _includeAllRatities , _includeAllCharacter,_manaCost, _keyword, _characterAsset);
        
    }
    /// <summary>
    /// 
    /// </summary>
    public void Previous(){
        if (_pageIndex== 0)
            return;
            //n
        ShowCard(true, _pageIndex - 1, _includeAllRatities,_includeAllCharacter,_manaCost ,_keyword, _characterAsset);
        
    }
  

 /// <summary>
 /// Pages the selection.
 /// </summary>
 /// <returns>The selection.</returns>
 /// <param name="showingCardPlayerDoesNotOwn">If set to <c>true</c> showing card player does not own.</param>
 /// <param name="includeTokenCard">If set to <c>true</c> include token card.</param>
 /// <param name="manaCost">Mana cost.</param>
 /// <param name="includeAllCharacter">If set to <c>true</c> include all character.</param>
 /// <param name="includeAllRatity">If set to <c>true</c> include all ratity.</param>
 /// <param name="keyword">Keyword.</param>
 /// <param name="characterAsset">Character asset.</param>
    // public List<CardAsset> PageSelection(
    //     bool showingCardPlayerDoesNotOwn=false,
    //     int pageIndex=0,
    // bool includeTokenCard=false,
    //     int manaCost =-1,
    //     bool includeAllCharacter=true,
    //     bool includeAllRatity=false,
    //     string keyword=" ",
    //     CharacterAsset characterAsset=null,
    //     CardRatityOption ratity =CardRatityOption.NORMAL
    //    )
    // {
     

    //     List<CardAsset> returnList = new List<CardAsset>();

    //     List<CardAsset> getCard = new List<CardAsset>();
    //     //Get Card
    //     getCard= CardCollection.instance.GetCard(showingCardPlayerDoesNotOwn, includeAllRatity, includeAllCharacter,
    //     manaCost,ratity,characterAsset,
    //      keyword,
    //     includeTokenCard);
    //     //List<CardAsset> getCard = CardCollection.instance.GetCard(true, true, true, -1, CardRatityOption.NORMAL, null, "", true);
    //     if (getCard.Count>pageIndex * slot.Length)
    //     {
    //         for (int i = 0; i < getCard.Count -pageIndex * slot.Length && i<slot.Length; i++)
    //         {
    //             returnList.Add(getCard[pageIndex * slot.Length + i]);
    //         }

    //     }
    //     return returnList;
    // }
public List<CardAsset> PageSelection(
        bool showingCardPlayerDoesNotOwn=false,
        int pageIndex=0,
    bool includeTokenCard=false,
        int manaCost =-1,
        bool includeAllCharacter=false,
        bool includeAllRatity=false,
        string keyword=" ",
        PlayerJob characterAsset=PlayerJob.None,
        CardRatityOption ratity =CardRatityOption.NORMAL,
        bool isInit=true
       )
    {
   
        List<CardAsset> returnList = new List<CardAsset>();

        List<CardAsset> getCard = new List<CardAsset>();
        //Get Card
        getCard= CardCollection.instance.GetCard(showingCardPlayerDoesNotOwn, includeAllRatity, includeAllCharacter,
        manaCost,ratity,characterAsset,
         keyword,
        includeTokenCard,false);

        total.text= Mathf.FloorToInt(getCard.Count/10).ToString();
     
        //List<CardAsset> getCard = CardCollection.instance.GetCard(true, true, true, -1, CardRatityOption.NORMAL, null, "", true);
        if (getCard.Count>pageIndex * 10)
        {
            for (int i = 0; i < getCard.Count -pageIndex * 10 && i<10; i++)
            {
                returnList.Add(getCard[pageIndex * 10 + i]);

                
            }

        }

        
        return returnList;
    }
   
   public void ChangeRariry(string rn){
       if(rn=="Normal"){ratity=CardRatityOption.NORMAL;}else 
       if(rn=="Rare"){ratity=CardRatityOption.RARE;}else
       if(rn=="Epic"){ratity=CardRatityOption.Epic;}else
       if(rn=="Lengend"){ratity=CardRatityOption.LEGEND;} 

   }

    /// <summary>
    /// Clears the create cards.
    /// </summary>
    public  void ClearCreatedCards()
    {
        while (CreatedCards.Count>0)
        {
            GameObject g = CreatedCards[0];
            CreatedCards.RemoveAt(0);
            Destroy(g);
        }
    }

    public void UpdateQuantitiesOnPage(){
        foreach (GameObject card in CreatedCards)
        {
            AddCardToDeck addCardComponent = card.GetComponent<AddCardToDeck>();
            addCardComponent.UpdateQuantity();

        }
    }


    /// <summary>
    /// Ons the serach field.
    /// </summary>
    /// <param name="name">Name.</param>
    public void OnSerachField(string name){
        //////显示全部卡牌
        //ShowCard(_showingCardsPlayerDoesNotOwn, _pageIndex, _includeAllRatities,CardRatityOption.NORMAL, _includeAllCharacter,
        //         _manaCost, keywordInputField.name, _characterAsset);
        //// 
    }


    /// <summary>
    /// n
    /// </summary>
    public void UpdatePage(){
        ShowCard(_showingCardsPlayerDoesNotOwn, _pageIndex, _includeAllRatities,_includeAllCharacter
        ,_manaCost,_keyword,characterAsset,_includeTokenCards,_ratity);
    }
}