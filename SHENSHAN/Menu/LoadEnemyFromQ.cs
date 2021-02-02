using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Load Enemy Asset From Quest who choose ,load enemy decks und Item & player stats
public  class LoadEnemyFromQ :MonoBehaviour
{
    private Players p;
    private void Awake()
    {
//        EnemyPortraitVisual enemy = BattleStartInfo.SelectEnemyDeck.visual;
        EnemyPortraitVisual visual = FindObjectOfType<EnemyPortraitVisual>();

        if(BattleStartInfo.SelectEnemyDeck!=null )
        {
            Debug.Log("Load Enemy From Town");
            p = GetComponent<Players>();
           
            p= GlobalSetting.instance.topPlayer;
            
           
            if(BattleStartInfo.SelectEnemyDeck.enemyAsset!=null ){

                //read
                visual.enemyAsset= BattleStartInfo.SelectEnemyDeck.enemyAsset;
                visual.healthText.text =BattleStartInfo.SelectEnemyDeck.enemyAsset.Health.ToString();
               
                visual.EnemyHead.sprite = BattleStartInfo.SelectEnemyDeck.enemyAsset.Head;
                visual.EnemyName.text=BattleStartInfo.SelectEnemyDeck.enemyAsset.EnemyName;
               
               
                //Load Card
                if(BattleStartInfo.SelectEnemyDeck.cards!=null )
                {
                    p.deck.cards =new List<CardAsset>(BattleStartInfo.SelectEnemyDeck.enemyAsset.cardList);
                }
                //Load Equipment who equip in the  Main Game TODO
            }
        }else{
            Debug.Log("Can't get top data");
        }
    }

    
//     void Start()
//    {
//        EnemyPortraitVisual enemy = BattleStartInfo.SelectEnemyDeck.visual;
//        EnemyPortraitVisual visual = FindObjectOfType<EnemyPortraitVisual>();
//
//        if(BattleStartInfo.SelectEnemyDeck!=null )
//        {
//            Debug.Log("Load Enemy From Town");
//            p = GameObject.FindWithTag("TopPlayer").GetComponent<Players>();
//           
//            p= GlobalSetting.instance.topPlayer;
//            
//           
//            if(BattleStartInfo.SelectEnemyDeck.enemyAsset!=null ){
//
//                //read
//               visual.enemyAsset= BattleStartInfo.SelectEnemyDeck.enemyAsset;
//               visual.healthText.text =BattleStartInfo.SelectEnemyDeck.enemyAsset.Health.ToString();
//               
//                visual.EnemyHead.sprite = BattleStartInfo.SelectEnemyDeck.enemyAsset.Head;
//                visual.EnemyName.text=BattleStartInfo.SelectEnemyDeck.enemyAsset.EnemyName;
//               
//               
//            //Load Card
//            if(enemy.enemyCardList!=null )
//            {
//              p.deck.cards =new List<CardAsset>(BattleStartInfo.SelectEnemyDeck.visual.enemyCardList);
//            }
//            //Load Equipment who equip in the  Main Game TODO
//            }
//        }else{
//            Debug.Log("Can't get top data");
//        }
//    }
}
