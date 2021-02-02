using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DropItems : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject iObj;
    public Items items;
    public Image itemIcon;
    public Text itemName;
    public Text itemNumber;
    private DungeonExplore explore;
   private TooltipManager2 tooltip;

   void Start(){
       tooltip=FindObjectOfType<TooltipManager2>();
       explore = FindObjectOfType<DungeonExplore>();
   }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button==PointerEventData.InputButton.Left){
      explore.AddItemToInventotry(items);
      DestroyImmediate(iObj);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
     
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       
    }
}
