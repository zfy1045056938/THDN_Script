using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SelectionIndex : MonoBehaviour,IPointerClickHandler 
{

  public int index;


  public void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.button == PointerEventData.InputButton.Left)
    {
      DiscoverManager.instance.ButtonHandler(index);
    }
    
  }
}
