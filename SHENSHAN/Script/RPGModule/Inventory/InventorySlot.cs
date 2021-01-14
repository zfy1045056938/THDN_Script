using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Mirror;
using PixelCrushers.DialogueSystem;

[Serializable]
public class InventorySlot : MonoBehaviour,IPointerEnterHandler,IPointerClickHandler,IPointerExitHandler
{

    public Items item;

    public bool containItem;
    public bool hasGem;
    public int itemStartNumber;
    public InventorySystem inventory;
    public Text stackSizeText;

    public Image itemImage;
    public Image unidentfied;
    public Image itemFrame;
        
    public InventorySlot()
    {
    }

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventorySystem>();
    }


   

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item.itemName!="")
        {
            inventory.OnMouseEnter(gameObject);
        }

        inventory.hoveredSlot = this;

        if (inventory.dragging)
        {
            inventory.dragItemBackground.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item.itemName != "")
        {
            inventory.OnMouseExit(gameObject);
        }

        inventory.hoveredSlot = null;
        if (inventory.dragging)
        {
            inventory.dragItemBackground.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button==PointerEventData.InputButton.Left){
            inventory.OnSlotClick(gameObject,0);
        }else if(eventData.button==PointerEventData.InputButton.Right){

            if (!inventory.startIdentifying)
            {
                inventory.OnSlotClick(gameObject, 1);
            }

            if (!inventory.equipGem)
            {
                inventory.OnSlotClick(gameObject, 1);
            }

        Debug.Log("Check Item Dialogue");
            if (item.covName != null   )
            {
                DialogueManager.StartConversation(item.covName);
            }else{
                Debug.Log("No Check");
            }
        }
    }
}

