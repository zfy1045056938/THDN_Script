using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShrineIcon : MonoBehaviour,IPointerClickHandler
{
   public Button iconBtn;
   public DungeonEvent de;
   public TextMeshProUGUI nameText;
   public int index =-1;
   public GameObject glow;

    public void OnPointerClick(PointerEventData eventData)
    {
      if(eventData.button == PointerEventData.InputButton.Left){
          glow.SetActive(true);
      }
    }
}
