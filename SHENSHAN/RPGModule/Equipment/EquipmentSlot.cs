using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Mirror;

[Serializable]
public class EquipmentSlot : MonoBehaviour,IPointerEnterHandler,IPointerClickHandler,IPointerExitHandler
{

    [HideInInspector]
    public Items item;

    public EquipmentSlotType equipmentSlotType;
    [HideInInspector]
    public InventorySystem inventorySystem;
    public float iconSclarFactor =1.0f;
    
    public Image itemIcon;


    

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button==PointerEventData.InputButton.Left){
            inventorySystem.OnEquipmentSlotClick(gameObject,0);
            
            
        }
        else if(eventData.button ==PointerEventData.InputButton.Right){
            inventorySystem.OnEquipmentSlotClick(gameObject,1);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    inventorySystem.OnMouseEnter(gameObject);
    
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       inventorySystem.OnMouseExit(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        inventorySystem = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventorySystem>();
       
    }

  
}

// public class SyncListEquipmentSlot:SyncList<EquipmentSlot>{}
