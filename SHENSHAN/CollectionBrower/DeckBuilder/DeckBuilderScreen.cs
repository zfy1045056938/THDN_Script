using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

using PixelCrushers.DialogueSystem;
using System;


//构建卡牌页面,显示隐藏制定页面并开始建卡操作
//1.选择角色卡组页面
//2.预卡组列表
//3.页面内容
//4.构建资产
public class DeckBuilderScreen:MonoBehaviour{
    public GameObject screenContent;
    public GameObject readyDeckList;
    public GameObject draftList;
    public GameObject cardsInDeckScript;
    public DeckBuilder buildScript;
    public ListOfDeckInCollection listOfReadyMadeDeckScript;
    public CollectionBrower collectionBroswerScript;
    public CharacterSelectionTabs tabScript;
    
    private PlayerData player;
    //
    private DeckBuilder deckBuilderScript;
    private bool isShow=false;

    private bool _fb;
    [SerializeField]
    public bool firstBuild{
        get{return _fb;}
        set{_fb=value;}
    }
    public bool draft =false;
    public static DeckBuilderScreen instance;
  

    public bool showReduceQuantitiesInDeckBuilding =true;

   

    public  void Awake()
    {
//        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        player = GameObject.FindObjectOfType<PlayerData>();
        HideScreen();

        instance = this;
    }

    public void Start()
    {
//        if (player.isLocalPlayer)
//        {
//            DeckStorge.instance.LoadDecksFromPlayerPrefs();
//        }
    }

    ///显示卡组编辑页面
    public void ShowScreenForCollectionBrower(){
        DeckStorge.instance.LoadDecksFromPlayerPrefs();
//        tabScript.CreateTabs();
        screenContent.SetActive(true);
        readyDeckList.SetActive(true);
        cardsInDeckScript.SetActive(false);
        buildScript.InDeckBuildingMode = false;
        listOfReadyMadeDeckScript.UpdateList();
        // collectionBroswerScript.allCharacterTabs.gameObject.SetActive(true);
        // collectionBroswerScript.oneCharacterTabs.gameObject.SetActive(false);
        Canvas.ForceUpdateCanvases();

        if (draft == true)
        {
           draftList.gameObject.SetActive(false);
        }
        collectionBroswerScript.ShowCollectionForBrowsing();

    }
    
    public void CloseCB(){
        screenContent.gameObject.SetActive(false);
    }
   

    /// <summary>
    /// Shows the screen for deck building.
    /// </summary>
    public void ShowScreenForDeckBuilding(){

        screenContent.SetActive(true);
        readyDeckList.SetActive(false);
        cardsInDeckScript.SetActive(true);

        // collectionBroswerScript.allCharacterTabs.gameObject.SetActive(false);
        // collectionBroswerScript.oneCharacterTabs.gameObject.SetActive(true);
        Canvas.ForceUpdateCanvases();
    }

public void SetDraft(){
      //Pay
      if(PlayerData.localPlayer.special>=1000){
            PlayerData.localPlayer.special -= 1000;
    draft=true;
    CharacterSelectionScreen.instance.ShowScreenWithDraft();
    HideScreen();
      }else{
        MessageManagers.instance.AddMessage("灵感不足",1.0f);

      }
}
    /// <summary>
    /// Builds the deck for.
    /// </summary>
    /// <param name="asset">Asset.</param>
    public void BuildDeckFor(CharacterAsset asset){
       if(draft==false){
           tabScript.SetClassOnClassTab(asset);
        ShowScreenForDeckBuilding();
        // collectionBroswerScript.ShowCollectionForDeckBuilding(asset);
//        DeckBuilderScreen.instance.tabScript.SetClassOnClassTab(asset);
        
        buildScript.BuildADeckFor(asset);
       }else{
           Debug.Log("Draft Mode");
                // DraftCardBuild(firstBuild,asset);
                 ShowScreenForDeckBuilding();
                  buildScript.BuildADeckForDraft(asset);
       }
    }

    private void DraftCardBuild(bool firstBuild,CharacterAsset asset)
    {
      
        screenContent.gameObject.SetActive(true);
        readyDeckList.SetActive(false);
        cardsInDeckScript.SetActive(true);
        ShowDraftBuilding();

        // StartCoroutine(buildScript.StartDraft(firstBuild,asset));
    }

    /// <summary>
    /// Hides the screen.
    /// </summary>
    public void HideScreen(){
        screenContent.gameObject.SetActive(false);
        collectionBroswerScript.ClearCreatedCards();
        tabScript.ClearObj();
        
    }

    //
    public void ShowDraftBuilding(){
        
        
        draftList.gameObject.SetActive(true);
        
    }

    /// <summary>
    /// Shows the screen.
    /// </summary>
    public void ShowScreen(){
        if (screenContent == null)
        {
            return;
        }
        else
        {
            screenContent.gameObject.SetActive(true);
            readyDeckList.gameObject.SetActive(false);
            cardsInDeckScript.gameObject.SetActive(true);
        }
    }
}