using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Character filiter tabs.
/// </summary>
public class CharacterFiliterTabs:MonoBehaviour,IPointerClickHandler
{

    public CharacterAsset characterAsset;
    public bool showAllCharacter = false;
    public TextMeshProUGUI nameText;
    
    private CharacterSelectionTabs tabsScript;
    private float selectionTransitionTime = 0.5f;
    private Vector3 initialScale = Vector3.one;
    private float scaleMultipler = 1.2f;
//
//    public void TabButtonHandler (){
//        DeckBuilderScreen.instance.tabScript.SelectTab(this, false);
//     
//      
//    }

    public void Select(bool instant = false){
        if (instant)
        {
            transform.localScale = initialScale * scaleMultipler;
        }
        else
            transform.DOScale(initialScale.x * scaleMultipler, selectionTransitionTime);
    }

    /// <summary>
    /// Deselect the specified instant.
    /// </summary>
    /// <param name="instant">If set to <c>true</c> instant.</param>
    public void Deselect(bool instant =false){
        if (instant)
        {
            transform.localScale = initialScale;
        }
        else
            transform.DOScale(initialScale, selectionTransitionTime);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button==PointerEventData.InputButton.Left){
            Debug.Log("this gameobj"+this.gameObject.ToString());
             DeckBuilderScreen.instance.tabScript.SelectTab(this.gameObject, false);
        }

    }
}