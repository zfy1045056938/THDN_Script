using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SelectionCard : MonoBehaviour,IPointerClickHandler
{
  public int index;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button==PointerEventData.InputButton.Left){
            Debug.Log("u Select this card"+name);
         
        }
    }
}
