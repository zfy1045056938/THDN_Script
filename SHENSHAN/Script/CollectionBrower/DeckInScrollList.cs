using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeckInScrollList:MonoBehaviour,IPointerEnterHandler,IPointerExitHandler{
        public  Image AvatarImage;

        public Text NameText;

        public Button DeleteDeckButton;
        
        
[SerializeField]
    public DeckInfo saveDeckInfo;
    
         void Awake()
        {
        //this.gameObject.SetActive(true);
//            DeleteDeckButton.SetActive(false);
    DeleteDeckButton.gameObject.SetActive(false);
        }


     

         /// <summary>
    /// Edits the this deck.
    /// </summary>
        public void EditThisDeck(){
          
            DeckBuilderScreen.instance.HideScreen();
           
            DeckBuilder.instance.SetDeckMode();
           
            DeckBuilderScreen.instance.buildScript.BuildADeckFor(saveDeckInfo.characterAsset );
            DeckBuilderScreen.instance.buildScript.DeckName.text =saveDeckInfo.deckName.ToString();
            DeckBuilder.instance.buildingForCharacter=saveDeckInfo.characterAsset;

            foreach(CardAsset asset in saveDeckInfo.cardAssets)
                DeckBuilderScreen.instance.buildScript.AddCard(asset);
             
                DeckStorge.instance.AllDecks.Remove(saveDeckInfo);   
            
                //TODO
        DeckBuilderScreen.instance.tabScript.SetClassOnClassTab(saveDeckInfo.characterAsset);
        //
        // DeckBuilderScreen.instance.collectionBroswerScript.ShowCollectionForDeckBuilding(saveDeckInfo.characterAsset);
                DeckBuilderScreen.instance.ShowScreenForDeckBuilding();
            
        }

         /// <summary>
         /// 
         /// </summary>
         public void DeleteThisDeck()
         {


                 for (int i = 0; i < DeckStorge.instance.AllDecks.Count; i++)
                 {
             if (saveDeckInfo.characterAsset != null && saveDeckInfo.deckName != null &&
                 saveDeckInfo.cardAssets != null)
             {
                 PlayerPrefs.DeleteKey(saveDeckInfo.characterAsset.ToString());
                 PlayerPrefs.DeleteKey(saveDeckInfo.deckName.ToString());
                 PlayerPrefs.DeleteKey(saveDeckInfo.cardAssets.ToString());
                 PlayerPrefs.DeleteKey(DeckStorge.instance.AllDecks[i].cardAssets.ToString());
                 DeckStorge.instance.AllDecks.Remove(saveDeckInfo);
                 Destroy(gameObject);
             }

                 
         }


         //Delete Gamobject
          

           

         }

         /// <summary>
    /// Applies the info.
    /// </summary>
    /// <param name="info">Info.</param>
            public void ApplyInfo(DeckInfo info){
        AvatarImage.sprite = info.characterAsset.avatarImage;
        NameText.text=info.deckName;
                saveDeckInfo =info;
            }


            public void OnPointerEnter(PointerEventData data){
        DeleteDeckButton.gameObject.SetActive(true);
            }

            public void OnPointerExit(PointerEventData data){
        DeleteDeckButton.gameObject.SetActive(false);
            }
}