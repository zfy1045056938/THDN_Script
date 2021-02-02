using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using  UnityEngine.UI;

public class EnemyPanel : MonoBehaviour
{
   
    public EnemyPortraitVisual portrait;
    public Button PlayButton;
   
    public PortraitMenu selectedPortrait{ get; set;}
    public EnemyIcon selectedDeck{ get; set;}
    public static EnemyPanel instance;

    void Awake()
    {
        if (instance == null) instance = this;
        OnOpen();
    }

    void Update()
    {
       
    }
        
    public void OnOpen()
    {
        
        SelectDeck(null);
    }

   


//Choose Deck 
    public void SelectDeck(EnemyIcon deck)
    {
        if (deck == null || selectedDeck == deck )
        {
            portrait.gameObject.SetActive(false);
            selectedDeck = null;
            if (PlayButton!=null)
                PlayButton.interactable = false;
        }
        else
        {           
            portrait.enemyAsset = deck.EnemyInfoDetail.enemyAsset;
            // for(int i=0;i<deck.EnemyInfoDetail.cards.Count;i++){
            portrait.enemyCardList = deck.EnemyInfoDetail.cards;
            // }
            //Load Enemy Asset
            portrait.ReadFromEnemyAsset();
            portrait.gameObject.SetActive(true);
            //set card List to enemydeck
            portrait.enemyCardList = deck.portrait.enemyCardList;
            selectedDeck = deck;
            // instantly load this information to our BattleStartInfo.
            BattleStartInfo.SelectEnemyDeck = selectedDeck.EnemyInfoDetail;

            if (PlayButton!=null)
                PlayButton.interactable = true;
        }
    }

  
}
