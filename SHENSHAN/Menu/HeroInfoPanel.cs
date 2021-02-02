using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/// <summary>
/// 选择英雄页面,包含以下内容
/// 1.当选中时，显示该卡组信息,当卡组不满足条件时则显示图标，信息包含(名称，城堡信息，工人信息，天赋树,卡组概况)
/// 2.英雄信息包含：背包栏(4*4),装备信息(6slot),天赋树(tree)
/// 3.卡组编辑btn,检测是否选中卡组，如果选中则跳转当前卡组, 
/// DeckIcon -> HeroInfoPanel ->SelctDeckScreen
/// </summary>
public class HeroInfoPanel : MonoBehaviour
{
    public PlayerPortraitVisual playerPortrait; //hero info panel


    public Text t;
    public Button playBtn;
    public Button editBtn;
    public Button BackBtn;

    public static HeroInfoPanel instance;
    public PortraitMenu selectPortraitMenu { get; set; }
    //
    public DeckIcon selectDeck { get; set; }

    void Awake()
    {
        if (instance != null) instance = this;
    }

    void Start(){
        OnOpen();
       
        
        Debug.Log("Now pp is "+playerPortrait.name);
       
    }

   

    ///<summary>
    ///
    ///
    ///</summary>
    public  void SelectDeck(DeckIcon deck)
    {
//        || !deck.DeckInformation.IsComplete()KC
        if (deck==null&& selectDeck==deck)
        {
            
          playerPortrait.gameObject.SetActive(false);
            selectDeck = null;
       

        }else{ 
            selectDeck = deck; 
            
            
            
            playerPortrait.characterAsset = deck.DeckInformation.characterAsset;
            
            
            
            playerPortrait.ApplyLookFromAsset();
           playerPortrait.gameObject.SetActive(true);
           Debug.Log(selectDeck.DeckNameText.text);

            BattleStartInfo.SelectDeck = selectDeck.DeckInformation;
           
        }

      

    }


    /// <summary>
    /// Ons the open.
    /// </summary>
    public void OnOpen()
    {
        SelectCharacter(null);
       SelectDeck(null);
    }

   

    /// <summary>
    /// Selects the character.
    /// </summary>
    /// <param name="menu">Menu.</param>
    public void SelectCharacter(PortraitMenu menu)
    {

        if (menu == null || selectPortraitMenu == menu)
        {

            playerPortrait.gameObject.SetActive(false);
            selectDeck = null;
         
        }
            else
            {
                playerPortrait.characterAsset = menu.asset;
                playerPortrait.ApplyLookFromAsset();
                if(playerPortrait.detailText!=null){
                playerPortrait.detailText.text = menu.asset.description.ToString();
                }
                playerPortrait.gameObject.SetActive(true);
                selectPortraitMenu = menu;
          
            }
    }

    public void ToDeckBuilding(){
        if (selectPortraitMenu==null)
        {
            return;
        }
            
            
            // if(CharacterSelectionScreen.instance.isShow==true){
            //     CharacterSelectionScreen.instance.ShowPrePanel(selectPortraitMenu.asset.className);
            // }
             DeckBuilderScreen.instance.BuildDeckFor(selectPortraitMenu.asset);
        
    }





    public void EditScreen()
    {
        if (playerPortrait==null)
        {
            return;
        }

        DeckBuilderScreen.instance.BuildDeckFor(playerPortrait.characterAsset);
    }
}