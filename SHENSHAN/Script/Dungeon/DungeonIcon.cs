using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;


public class DungeonIcon : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
     public DungeonEventClass dvc;
      public GameObject icon;
     public DungeonEventType det = DungeonEventType.None;
     public int amount = 0;
     public Image IconSprite;

    public void OnPointerEnter(PointerEventData eventData)
    {
       icon.transform.DOScale(1.5f,0.3f);
       Debug.Log("Show Tip");
       DungeonExplore.instance.ShowTip(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        icon.transform.DOScale(1f,0.3f);
    }
}
