using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;
public class PortraitMenu:MonoBehaviour,IPointerClickHandler
{
    public CharacterAsset asset;
    private PlayerPortraitVisual portraitVisual;
    private float targetScale=1.3f;
    private float InitialScale=1f ;
    private bool selected = false;


    public void Awake()
    {
        Debug.Log("CharacterPortrait Module Begin-------");
        portraitVisual = GetComponent<PlayerPortraitVisual>();

        portraitVisual.characterAsset = asset;
        portraitVisual.ApplyLookFromAsset();
        InitialScale = transform.localScale.x;
    }

    public void OnMouseEnter()
    {
        transform.DOScale(targetScale, 0.1f);
    }

    /// <summary>
    /// Deselect this instance.
    /// </summary>
    public void Deselect()
    {
        selected = false;
        transform.DOScale(InitialScale, 0.1f);

    }



    public void OnPointerClick(PointerEventData eventData)
    {

        if (!selected)
        {
            selected = true;
            transform.DOScale(targetScale, 0.5f);
            CharacterSelectionScreen.instance.heroPanel.SelectCharacter(this);
            PortraitMenu[] allMenu = FindObjectsOfType<PortraitMenu>();
            foreach (PortraitMenu item in allMenu)
            {
                
                if (item != this)
                {
                    item.Deselect();
                }


            }
        }
        else
        {

            Deselect();
            CharacterSelectionScreen.instance.heroPanel.SelectCharacter(null);
        }


    }
}
