using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SlotManager : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{

    public SpriteRenderer GlowImage;
    public void OnPointerEnter(PointerEventData eventData)
    {
       GlowImage.gameObject.SetActive(true);
        Debug.Log("u enter the"+this.gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      GlowImage.gameObject.SetActive(false);
    }
}
