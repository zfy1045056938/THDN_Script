using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ListOfDeckInCollection :MonoBehaviour{
    public Transform Content;
    public GameObject DeckInListPrefab;
    public GameObject NewDeckButtonPrefab;





    /// <summary>
    /// Updates the list.
    /// </summary>
    public void UpdateList(){
        foreach(Transform t  in Content){
            if(t!= Content){
                Destroy(t.gameObject);
            }
        }

        //
        foreach(DeckInfo info in DeckStorge.instance.AllDecks){
            
            GameObject g = Instantiate(DeckInListPrefab,Content);
          
            
            g.SetActive(true);
            DeckInScrollList decksInscrollListComponent =g.GetComponent<DeckInScrollList>(); 
            decksInscrollListComponent.ApplyInfo(info);
        }

        if(DeckStorge.instance.AllDecks.Count< 9){
            GameObject g = Instantiate(NewDeckButtonPrefab,Content) ;
            
        }
//       
    }
}