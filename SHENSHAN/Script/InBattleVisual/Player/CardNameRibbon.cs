using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;
public class CardNameRibbon:MonoBehaviour,IPointerEnterHandler,IPointerExitHandler{
    public TextMeshProUGUI nameText;
    
    public TextMeshProUGUI quantityText;

    public Image RibbonImage;

    public OneCardManager previewManager;
    public CardAsset asset;

    public int Quantity { get; set; }

    private float initScale = 1.0f;
    private float maxScale = 1.2f;

    // public GameObject CreatureValue;
    // public GameObject typeObj;
    
    
    public void ApplyAsset(CardAsset ca,int quantity){
//        if (ca.characterAsset != null)RibbonImage.color =ca.characterAsset.classCardTint;

        asset=ca;
        
        nameText.text= ca.name;

        SetQuantity(quantity);
    }

    public void SetQuantity(int quantity){
        if(quantity == 0)return;

        quantityText.text= "x" +quantity.ToString();
        Quantity=quantity;
    }

    /// <summary>
    /// Reduce
    /// </summary>
    public void ReduceNumber(){
        //different of the townType
        if (TownManager.instance.atDungeon == false)
        {
            //
            DeckBuilderScreen.instance.buildScript.RemoveCard(asset);
        }
        else
        {
            //Dungeon
            DungeonCardEdit.instance.RemoveCardToTmp(asset);
        }
    }

    public void OnMouseEnter()
    {
        // HoverPreview.previewAllowed = true;
        previewManager.gameObject.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(maxScale, 0.4f);
        if (previewManager != null)
        {
            previewManager.gameObject.SetActive(true);
            previewManager.cardAsset = asset;
            previewManager.ReadCardFromAsset();
            //
            // if (asset.typeOfCards == TypeOfCards.Spell)
            // {
            //     typeObj.gameObject.SetActive(false);
            //     CreatureValue.gameObject.SetActive(false);
            // }else if (asset.typeOfCards == TypeOfCards.Creature)
            // {
            //     typeObj.gameObject.SetActive(true);
            //     CreatureValue.gameObject.SetActive(true);
            // }
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(initScale, 0.4f);
        previewManager.gameObject.SetActive(false);
    }
}