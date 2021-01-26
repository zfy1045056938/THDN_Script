using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using System.Collections;
using Michsky.UI.Zone;
using TMPro;
public enum BuildMode{
    Normal,
    Draft,
    PayDraft,
}
/// <summary>
/// 卡牌构建流程
/// 1.建立卡牌（amountofcard<9)
/// 2.选择角色(selectcharacter)
/// 3.CRUD
/// 4.2021-1-26 Added Common Card(10)n,10-n
/// </summary>
public class DeckBuilder:MonoBehaviour{

    public GameObject CardNamePrefab;   //卡牌预制品
    public GameObject CommonCardPrefab;
    public InputField DeckName;          //牌组名称
    public Transform Content;

    public DraftManager draftManager;



    public int sameCardLimit = 2;
    public int amountOfCardsInDeck=20 ;

    public Text total;
    public Text curr;
    public TextMeshProUGUI atkNum;
    public TextMeshProUGUI defNum;
    public int currentPackCardCount;

    public GameObject deckCompleteFrame;

    public List<CardAsset> deckList = new List<CardAsset>();
    
    

    public Dictionary<CardAsset, CardNameRibbon> ribbons = new Dictionary<CardAsset, CardNameRibbon>();

    private bool inDeckBuildingMode ;

    public bool InDeckBuildingMode
    {
        get => inDeckBuildingMode;
        set => inDeckBuildingMode = value;
    }

    [SerializeField]
    public CharacterAsset buildingForCharacter;

    public static DeckBuilder instance;

    public Button buildBtn;
    public Animator panelFade;
    public BlurManager manager;

    public bool draft=false;
    private bool firstDraft;
    public bool FirstDraft{
        get{
            return firstDraft;
        }

        set{
            firstDraft=value;
        }
    }

  
    public int cs;
    
    public  void Awake()
    {
        instance = this;

        deckCompleteFrame.GetComponent<Image>().raycastTarget = false;
    }

    void Update()
    {

        
      buildBtn.gameObject.SetActive(deckList.Count==amountOfCardsInDeck);
      
    }

    /// <summary>
    /// 添加卡片卡牌编辑器
    /// </summary>
    /// <param name="asset">Asset.</param>
    public void AddCard(CardAsset asset){
       
        ++cs;
        
        if (InDeckBuildingMode==false)
                     {
                         return;
                     }

      
        if (deckList.Count==amountOfCardsInDeck)
        {
            return;
        }
        
        int count = NumberOfThisCardInDeck(asset);
        
        int limitOfThisCardInDeck = sameCardLimit;
        total.text =amountOfCardsInDeck.ToString();
        curr.text =(1+ deckList.Count).ToString();
       
        if (asset.OverrideLimitOfThisCardsInDeck>0)
        {
            limitOfThisCardInDeck = asset.OverrideLimitOfThisCardsInDeck;
        }

       
        //Add Cards
        if (count<limitOfThisCardInDeck)
        {
            deckList.Add(asset);
            CheckCompleteFrame();

            count++;

            if (ribbons.ContainsKey(asset))
            {
                ribbons[asset].SetQuantity(count);
            }
            else
            {
                Debug.Log("Create card");
                GameObject cardName = Instantiate(CardNamePrefab, Content) as GameObject;
                cardName.transform.SetAsLastSibling();
                cardName.transform.localScale = Vector3.one;
                CardNameRibbon ribbon = cardName.GetComponent<CardNameRibbon>();
                ribbon.ApplyAsset(asset, count);
                ribbons.Add(asset, ribbon);

            }

            if(count==limitOfThisCardInDeck){
                
            }

        }
    }

    /// <summary>
    /// Checks the complete frame.
    /// </summary>
    void CheckCompleteFrame(){
        deckCompleteFrame.SetActive(deckList.Count == amountOfCardsInDeck);
    }

    /// <summary>
    /// Nunbers the of this card in deck.
    /// </summary>
    /// <returns>The of this card in deck.</returns>
    /// <param name="asset">Asset.</param>
    public int NumberOfThisCardInDeck(CardAsset asset){
        int count = 0;

        foreach (CardAsset ca in deckList)
        {
            if (ca==asset)
            {
                count++;
            }

        }
        return count;
    }

    
  
    /// <summary>
    /// Removes the card.
    /// </summary>
    /// <param name="asset">Asset.</param>
    public void RemoveCard(CardAsset asset){

        Debug.Log("RemoveCard");
        --cs;
        CardNameRibbon ribbonToRemove = ribbons[asset];
        //
        ribbonToRemove.SetQuantity(ribbonToRemove.Quantity - 1);
        //
        if (NumberOfThisCardInDeck(asset) == 1)
        {
            ribbons.Remove(asset);
            //
            Destroy(ribbonToRemove.gameObject);
        }
        //
        deckList.Remove(asset);
        //
        CheckCompleteFrame();

        //
        curr.text= cs.ToString();

        //
        DeckBuilderScreen.instance.collectionBroswerScript.UpdatePage();
    }
    /// <summary>
    /// Builds the AD eck for.
    /// </summary>
    /// <param name="asset">Asset.</param>
    public void BuildADeckFor(CharacterAsset asset){

        
        //clear Tab Obj
        // DeckBuilderScreen.instance.tabScript.ClearObj();
        
        InDeckBuildingMode = true;
        buildingForCharacter = asset;
        currentPackCardCount=0;
        //
        while (deckList.Count>0)
        {
            RemoveCard(deckList[0]);
        }

      
        //Add common by character
        atkNum.text = asset.atkNum.ToString();
        defNum.text=asset.defNum.ToString();
        currentPackCardCount = 10;
        //pre build pack build common card for current amount until cpc <=0
        while (currentPackCardCount > 0)
        {
            //Build common
            int damageCard = asset.atkNum;

            while (damageCard > 0) {
                //add damage card
                //as spell card
                GameObject common = Instantiate(CommonCardPrefab,Content) as GameObject ;
                    damageCard--;
                }
            //
            int armorCard = currentPackCardCount - damageCard;

            while (armorCard > 0)
            {

                GameObject common = Instantiate(CommonCardPrefab, Content) as GameObject;

                armorCard--;


                   
            }
            //
            currentPackCardCount--;
        }

        
        DeckBuilderScreen.instance.tabScript.CreateClassTabs(asset.className);
        DeckBuilderScreen.instance.collectionBroswerScript.ShowCollectionForDeckBuilding(asset);
         curr.text=currentPackCardCount.ToString();
        CheckCompleteFrame();
        DeckName.text = "新建卡组"+DeckStorge.instance.AllDecks.Count;
    }

    public void BuildADeckForDraft(CharacterAsset asset){
           InDeckBuildingMode = true;
        buildingForCharacter = asset;
        //
        while (deckList.Count>0)
        {
            RemoveCard(deckList[0]);
        }
        // DeckBuilderScreen.instance.tabScript.SetClassOnClassTab(asset);
        DeckBuilderScreen.instance.draftList.gameObject.SetActive(true);
draftManager.StartDraft();
       
         curr.text=(0).ToString();
        CheckCompleteFrame();
        DeckName.text = "新建卡组"+DeckStorge.instance.AllDecks.Count;
    }

    /// <summary>
    /// Dones the button handler.
    /// </summary>
    public void DoneButtonHandler(){
      ///CLEAR OBJ 
    //   for(int i=0;i<DeckBuilderScreen.instance.tabScript.classTbas.Count;i++){
    //       if(DeckBuilderScreen.instance.tabScript.classTbas[i]==null)return;

    //       Destroy(DeckBuilderScreen.instance.tabScript.classTbas[i]);
    //   }
      
      if(BattleStartInfo.AtDungeon==false){
            Debug.Log("At town set Pack");
            DeckInfo deckToSave = new DeckInfo(DeckName.text, buildingForCharacter, deckList,buildingForCharacter.atkNum,buildingForCharacter.defNum);
            DeckStorge.instance.AllDecks.Add(deckToSave);
            Debug.Log("Add To Deck");
            //save to prefs
            DeckStorge.instance.SaveDecksIntoPlayerPrefs();
            //
            if(DeckBuilderScreen.instance.draft==true){
                DeckBuilderScreen.instance.draft=false;
                DeckBuilderScreen.instance.draftList.gameObject.SetActive(false);

            }
            DeckBuilderScreen.instance.ShowScreenForCollectionBrower();
            InDeckBuildingMode=false;

      
    }else{
        Debug.Log("Dungeon Pack Config");
          //At dungeon save tmp Collections
          DeckInfo deckToSave = new DeckInfo(DeckName.text, buildingForCharacter, deckList,buildingForCharacter.atkNum,buildingForCharacter.defNum);
          BattleStartInfo.SelectDeck=deckToSave;
          DungeonCardEdit.instance.DCBtn.enabled=true;
          DungeonCardEdit.instance.content.gameObject.SetActive(false);
          
          InDeckBuildingMode=false;
      }
    
      DeckBuilderScreen.instance.tabScript.CreateTabs();

}

public void CancelBuild(){

    
    if(DeckBuilderScreen.instance.draft==true){
        UIConfirmation.instance.Show("取消灵感不返还",()=>{
            DeckBuilderScreen.instance.draft=false;
            DeckBuilderScreen.instance.ShowScreenForCollectionBrower();
        });
    }else{
        //Edit Mode;
        UIConfirmation.instance.Show("是否取消构筑卡组",()=>{
             DeckBuilderScreen.instance.ShowScreenForCollectionBrower();
        });
        
    }
}

public void SetDeckMode(){
    InDeckBuildingMode=true;
}

    private void OnApplicationQuit()
    {
        DoneButtonHandler();
    }

}