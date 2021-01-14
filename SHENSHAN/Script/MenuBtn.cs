using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.EventSystems;

namespace PixelCrushers.DialogueSystem.Demo{
public class MenuBtn : MonoBehaviour,IPointerClickHandler
{
  public DemoMenu menu =new DemoMenu();

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Click obj");
       menu.Open();
        }


   
}
}
