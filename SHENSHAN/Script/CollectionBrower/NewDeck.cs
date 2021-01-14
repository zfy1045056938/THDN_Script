using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDeck : MonoBehaviour {

    public void NewPack(){
        DeckBuilderScreen.instance.HideScreen();
       
            
        CharacterSelectionScreen.instance.ShowScreen();

       
    }
}
