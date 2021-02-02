using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers;

public class DungeonCardCollection : MonoBehaviour
{
    public UIPanel panel;
    public GameObject CardNameRib;
    public List<CardAsset>takeCardList; //from town 
    public Transform cardPos;
    public List<CardAsset>tmpCardList;  //from battle selection
    public Transform tmpPos;
    

    // Start is called before the first frame update
    void Start()
    {
        LoadCardList();
       
    }

   void LoadCardList(){
       if(BattleStartInfo.SelectDeck!=null){
           takeCardList=new List<CardAsset>(BattleStartInfo.SelectDeck.cardAssets);
           for(int i=0;i<takeCardList.Count;i++){
               if(CardNameRib!=null && !takeCardList.Contains(takeCardList[i]) ){
                    GameObject card = Instantiate(CardNameRib,cardPos.position,Quaternion.identity)as GameObject;
                    card.transform.parent= cardPos;
                    card.name = takeCardList[i].name;
                    CardNameRib.GetComponent<CardNameRibbon>().ApplyAsset(takeCardList[i],1);
                    
               }
           }
       }
   }

 
}
