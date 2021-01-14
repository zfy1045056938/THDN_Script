
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MerchantSlot:MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    [HideInInspector]
    public Items item;
    [HideInInspector]
    public InventorySystem inventory;
    [HideInInspector]
    public Merchant merchant;
    public int itemStartNumber;
    public Image uni;


    // Use this for initialization
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventorySystem>();
        merchant = GameObject.FindGameObjectWithTag("Merchant").GetComponent<Merchant>();
    }

    public void OnPointerClick(PointerEventData data){

        if(data.button == PointerEventData.InputButton.Left) {
            merchant.OnMerchantSlotClick(gameObject, 0);
        }
        else if(data.button == PointerEventData.InputButton.Right) {
            merchant.OnMerchantSlotClick(gameObject, 1);
        }
    }

    public void OnPointerEnter(PointerEventData data)
{
    merchant.OnMerchantSlotEnter(gameObject);
    //CursorManager.ChangeCursor("BUSINESS");
}

public void OnPointerExit(PointerEventData data)
{
    merchant.OnMerchantSlotExit();
    //CursorManager.ChangeCursor("Default");
}
}