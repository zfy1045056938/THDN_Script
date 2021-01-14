using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Merchant Manager
/// 1.When PlayerData Click Merchant then show the ShopScreen und PlayerData can do these things
///  2.BuyBack
/// 3.repair
/// 4.Business
/// 5.Talking      
/// </summary>
public class Merchant : MonoBehaviour
{

    public static Merchant instance;
    public GameObject panel;


  
    private PlayerData player;
    [HideInInspector]
    public List<MerchantSlot> items;
    
    public TooltipManager tooltip;
    [HideInInspector]
    public InventorySystem inventory;
    [HideInInspector]
    public bool showMerchant;
    [HideInInspector]
    public MessageManager messageManager;
    [HideInInspector]
    public List<MerchantTabs> tabs;
    
    [HideInInspector]
    public MerchantTabs selectedTab;
    // [HideInInspector]
    public MerchantController selectedMerchant;
    [HideInInspector]
    public Items itemToRepair;
    [HideInInspector]
    public bool dragging;
    [HideInInspector]
    public MerchantSlot draggedItem;

    public bool removeStackableItemsWhenBought;
    public Color tabInactiveColor;
    public Color tabActiveColor;
    public int merchantWidth;
    public int merchantHeight;
    public float iconSlotSize = 40f;
    public int tabWidth = 70;
    public int tabHeight = 140;
    public Vector3 tabOffset;


    public Image dragImage;
    public Transform merchantSlots;
    public GameObject slotPrefab;
    public Text avaliableGoldText;
    public GameObject tabsObj;
    public GameObject tabPrefab;
    public GameObject repair;
    // public Image itemRepairIcon;
    // public Text repairSingleText;
    // public Text repairEquippedText;
    // public Text repairAllText;
    // public Text singleRepairLabel;
    // public Image singleRepairCoin;
    // public AudioClip repairSound;

    // Use this for initialization

    public AudioClip buySound;
    public AudioClip CraftSound;
    void Start()
    {
        player = FindObjectOfType<PlayerData>().GetComponent<PlayerData>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventorySystem>();
        messageManager = GameObject.FindGameObjectWithTag("MessageManager").GetComponent<MessageManager>();
        // selectedMerchant = GameObject.FindGameObjectWithTag("MerchantController").GetComponent<MerchantController>();
        ResetItems();
        avaliableGoldText.text = player.money.ToString();
        
        //
        OpenCloseMerchant(false);
      
        
    }



    // Update is called once per frame
    void Update()
    {
       
        //维修费用
        // if (repair.activeSelf == true)
        // {
        //     //Calculate the repair costs
        //     int repairCostEquipped = 0;
        //     int repairCostAll = 0;
        //     for (int i = 0; i < inventory.equipmentSlots.Count; i++)
        //     {
        //         if (inventory.equipmentSlots[i].item.itemName != "" && itemToRepair.repairNumber <= 0)
        //         {
        //             repairCostEquipped += (int)(inventory.equipmentSlots[i].item.totalRepairCost * 0.1f);
        //         }
        //     }
        //     //Update repair cost label
        //     repairEquippedText.text = repairCostEquipped.ToString();
        //     repairEquippedText.rectTransform.sizeDelta = new Vector2(repairEquippedText.preferredWidth, repairEquippedText.preferredHeight);

        //     //Calculate the repair cost for all items
        //     for (int i = 0; i < inventory.items.Count; i++)
        //     {
        //         if (i == inventory.items[i].itemStartNumber && inventory.items[i].item.itemName != "")
        //         {
        //             repairCostAll += (int)(inventory.items[i].item.totalRepairCost * 0.1f);
        //         }
        //         //Update total repair cost labels
        //         repairAllText.text = (repairCostEquipped + repairCostAll).ToString();
        //         repairAllText.rectTransform.sizeDelta = new Vector2(repairAllText.preferredWidth, repairAllText.preferredHeight);
        //     }

//        if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            ShowOrClosePage(true);
//        }
            if (dragging && Input.GetMouseButtonDown(1))
            {
                dragging = false;
                inventory.dragItem.gameObject.SetActive(false);
            }

            if (dragging)
            {
                inventory.dragItem.rectTransform.position = new Vector3(Input.mousePosition.x + inventory.dragItem.rectTransform.sizeDelta.x * inventory.dragItem.rectTransform.lossyScale.x * 0.5f, Input.mousePosition.y - inventory.dragItem.rectTransform.sizeDelta.x * inventory.dragItem.rectTransform.lossyScale.y * 0.5f, -20);
            }
        
        //
      
    }

    public void ShowOrClosePage(bool show)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(show);
        }
    }

    //Reset the items in the slots
    public void ResetItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Destroy(items[i].gameObject);
        }
        items = new List<MerchantSlot>();
        for (int i = 0; i < merchantWidth * merchantHeight; i++)
        {
            GameObject slot = Instantiate(slotPrefab) as GameObject;
            slot.transform.SetParent(merchantSlots);
           
            slot.name = i.ToString();
            slot.transform.localScale = Vector3.one;
            
            
            MerchantSlot merchantSlot = slot.GetComponent<MerchantSlot>();
            merchantSlot.item = new Items();
          
            merchantSlot.item.itemName = "";
            merchantSlot.itemStartNumber = i;
            items.Add(merchantSlot);
        }
    }

    //Add an item to the slots
    public void AddItem(Items item)
    {
        item = InventorySystem.DeepCopy(item);
        for (int i = 0; i < items.Count; i++)
        {
            //There's nothing in the slot
            if (items[i].item.itemName == "")
            {
                //Check the to see if the item can fit in the slot
                if (!CheckFit(item, i))
                {
                    //If it doesn't fit then continue to the next slot
                    continue;
                }
                //The item fits
                else
                {
                    //Run through all the slots and assign the item and hide the slot
                    for (int j = 0; j < item.height; j++)
                    {
                        for (int k = 0; k < item.width; k++)
                        {
                            items[i + merchantWidth * j + k].item = item;
                            items[i + merchantWidth * j + k].itemStartNumber = i;
                            items[i + merchantWidth * j + k].GetComponent<Image>().color = Color.clear;
                            items[i + merchantWidth * j + k].transform.Find("ItemFrame").GetComponent<CanvasGroup>().interactable = false;
                            items[i + merchantWidth * j + k].transform.Find("ItemFrame").GetComponent<CanvasGroup>().blocksRaycasts = false;
                            items[i + merchantWidth * j + k].GetComponent<CanvasGroup>().blocksRaycasts = false;
                            //If it's the first slot then make the icon and frame equal to the size of the item and show them
                            if (i == i + merchantWidth * j + k)
                            {
                                items[i + merchantWidth * j + k].transform.Find("ItemIcon").gameObject.SetActive(true);
                                items[i + merchantWidth * j + k].transform.Find("ItemIcon").GetComponent<Image>().sprite = item.icon;
                                items[i + merchantWidth * j + k].transform.Find("ItemIcon").GetComponent<RectTransform>().sizeDelta 
                                    = new Vector2(item.width*iconSlotSize , item.height*iconSlotSize );
                                items[i + merchantWidth * j + k].transform.Find("ItemFrame").GetComponent<RectTransform>().sizeDelta
                                    = new Vector2(item.width*iconSlotSize , item.height*iconSlotSize );
                               
                                items[i + merchantWidth * j + k].transform.Find("ItemFrame").gameObject.SetActive(true);
                                items[i + merchantWidth * j + k].transform.Find("ItemFrame").GetComponent<CanvasGroup>().interactable = true;
                                items[i + merchantWidth * j + k].transform.Find("ItemFrame").GetComponent<CanvasGroup>().blocksRaycasts = true;
                               
                                items[i + merchantWidth * j + k].GetComponent<CanvasGroup>().blocksRaycasts = true;

                            }
                            //
                          
                        }
                    }
                    //The item was successfully added
                    return;
                }
            }
        }
    }

    //Check to see if the item fits
    public bool CheckFit(Items item, int i)
    {
        for (int j = 0; j < item.height; j++)
        {
            for (int k = 0; k < item.width; k++)
            {
                //There's already an item in the slot
                if (items[i + merchantWidth * j + k].item.itemName != "")
                {
                    return false;
                }
                //Check to see if the slot is at the edge of the merchants slots
                for (int l = 0; l < item.height; l++)
                {
                    if (i + merchantWidth * l + k != i + merchantWidth * l)
                    {
                        if (((i + merchantWidth * j + k) % merchantWidth == 0) && item.width != 1)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    //Buy the item of the slot
    public void BuyItem(MerchantSlot slot)
    {
        Items item = InventorySystem.DeepCopy(slot.item);
    SoundManager.instance.PlaySound(buySound);
        Debug.Log("Buy Items player money is"+player.money.ToString());
        //If the player has enough gold
        if (player.money >= slot.item.buyPrice)
        {
            //If the item is stackable then add the stackable item
           if (item.stackable)
           {
               Debug.Log("Item have stackable");
               item.stackSize = 1;
               if (inventory.AddStackableItem(item))
               {
                   player.money -= slot.item.buyPrice;
                //    if (removeStackableItemsWhenBought)
                //    {
                //        for (int i = 0; i < selectedMerchant.tabs.Count; i++)
                //        {
                //            for (int j = 0; j < selectedMerchant.tabs[i].items.Count; j++)
                //            {
                //                if (selectedMerchant.tabs[i].items[j] == slot.item)
                //                {
                //                    selectedMerchant.tabs[i].items.Remove(slot.item);
                //                }
                //            }
                //        }
                       for (int i = 0; i < item.height; i++)
                       {
                           for (int j = 0; j < item.width; j++)
                           {
                               items[slot.itemStartNumber + merchantWidth * i + j].item = new Items();
                               items[slot.itemStartNumber + merchantWidth * i + j].item.itemName = "";
                               items[slot.itemStartNumber + merchantWidth * i + j].GetComponent<Image>().color = Color.white;
                               items[slot.itemStartNumber + merchantWidth * i + j].transform.Find("ItemIcon").gameObject.SetActive(false);
                               items[slot.itemStartNumber + merchantWidth * i + j].transform.Find("ItemFrame").gameObject.SetActive(false);
                           
                            
                           }
                       }
                   
                   avaliableGoldText.text = player.money.ToString();
                  
               
               }
               //There's no open slots in the inventory so display a fail message
               else
               {
                   MessageManagers.instance.ShowMessageCoroutine("购买失败",0.4f);
               }
           }
            //Add the item to the inventory
          if(  inventory.AddItem(InventorySystem.DeepCopy(slot.item)))
            {
                player.money -= slot.item.buyPrice;
                for (int i = 0; i < selectedMerchant.tabs.Count; i++)
                {
                    for (int j = 0; j < selectedMerchant.tabs[i].items.Count; j++)
                    {
                        if (selectedTab.tabType == selectedMerchant.tabs[i].tabType)
                        {
                            if (selectedMerchant.tabs[i].items[j].itemName == slot.item.itemName)
                            {
                                
                                    //Remove for detail rare equipment
//                                    selectedMerchant.tabs[i].items.Remove(selectedMerchant.tabs[i].items[j]);
                                    for (int k = 0; k < item.height; k++)
                                    {
                                        for (int l = 0; l < item.width; l++)
                                        {
                                            items[slot.itemStartNumber + merchantWidth * k + l].item = new Items();
                                            items[slot.itemStartNumber + merchantWidth * k + l].item.itemName = "";
                                            items[slot.itemStartNumber + merchantWidth * k + l].GetComponent<Image>()
                                                .color = Color.white;
                                            items[slot.itemStartNumber + merchantWidth * k + l].transform
                                                .Find("ItemIcon").gameObject.SetActive(false);
                                            items[slot.itemStartNumber + merchantWidth * k + l].transform
                                                .Find("ItemFrame").gameObject.SetActive(false);

                                            if (items[slot.itemStartNumber + merchantWidth * k + l].item.unidentified ==
                                                true)
                                            {
                                                items[slot.itemStartNumber + merchantWidth * k + l].uni.gameObject.SetActive(true);
                                            }
                                        }
                                        //
                                      
                                    }
                                    avaliableGoldText.text = player.money.ToString();
                            
                            }
                        }
                    }
                }
            }
            //There's no open slots in the inventory so display a fail message
            ResetTabs();
          
        }
        //The player doesn't have enough gold to purchase the item
        else
        {
            Debug.Log("Money Not Enough");
            //Display a fail message
           MessageManagers.instance.ShowMessageCoroutine("金钱不足",0.4f);
        }
    }

    public void RemoveItem(Items item, MerchantSlot slot)
    {
        for (int i = 0; i < selectedMerchant.tabs.Count; i++)
        {
            for (int j = 0; j < selectedMerchant.tabs[i].items.Count; j++)
            {
                if (selectedTab.tabType == selectedMerchant.tabs[i].tabType)
                {
                    if (selectedMerchant.tabs[i].items[j].itemName == slot.item.itemName)
                    {
                        selectedMerchant.tabs[i].items.Remove(selectedMerchant.tabs[i].items[j]);
                        for (int k = 0; k < item.height; k++)
                        {
                            for (int l = 0; l < item.width; l++)
                            {
                                items[slot.itemStartNumber + merchantWidth * k + l].item = new Items();
                                items[slot.itemStartNumber + merchantWidth * k + l].item.itemName = "";
                                items[slot.itemStartNumber + merchantWidth * k + l].GetComponent<Image>().color = Color.white;
                                items[slot.itemStartNumber + merchantWidth * k + l].transform.Find("ItemIcon").gameObject.SetActive(false);
                                items[slot.itemStartNumber + merchantWidth * k + l].transform.Find("ItemFrame").gameObject.SetActive(false);
                            }
                        }
                        return;
                    }
                }
            }
        }
    }

    //Change the current merchant tab
    public void ChangeTab(MerchantTabs tab)
    {
        // //If the selected tab is of type repair then hide the repair window
        if (selectedTab.tabType == TabType.repair)
        {
            repair.SetActive(false);
        }

        //Set the selected tabs color to inactive
//        selectedTab.GetComponent<Image>().color = tabInactiveColor;

        //Assign the clicked tab to the selected tab
        selectedTab = tab;
        //Set the color of the newly selected tab to active
//        selectedTab.GetComponent<Image>().color = tabActiveColor;

        //If the selected tab isn't of type repair
        if (tab.tabType != TabType.repair)
        {
            //Run through all the tabs and the reset the items in them
            ResetTabs();
        }else{
        repair.SetActive(true);
        }

        //Else if it is of type repair then show the repair tab
      
    }

    //Reset the tabs of the merchant
    public void ResetTabs()
    {
        for (int i = 0; i < selectedMerchant.tabs.Count; i++)
        {
            if (selectedMerchant.tabs[i].tabType == selectedTab.tabType )
            {
                ResetItems();
                for (int j = 0; j < selectedMerchant.tabs[i].items.Count; j++)
                {
                    AddItem(selectedMerchant.tabs[i].items[j]);
                }
            }
        }
    }

    //Called when the repair slot is clicked
    // public void OnRepairSlotClick()
    // {
    //     if (inventory.dragging)
    //     {
    //         if (inventory.draggedItem.curDurability != inventory.draggedItem.maxDurability)
    //         {
    //             itemToRepair = inventory.draggedItem;
    //             int repairCost = (int)(itemToRepair.totalRepairCost * 0.1f * (1 - ((float)itemToRepair.curDurability / itemToRepair.maxDurability)));
    //             repairSingleText.text = repairCost.ToString();
    //             repairSingleText.rectTransform.sizeDelta = new Vector2(repairSingleText.preferredWidth, repairSingleText.preferredHeight);
    //             itemRepairIcon.sprite = itemToRepair.icon;
    //             itemRepairIcon.rectTransform.sizeDelta = new Vector2(itemToRepair.width * iconSlotSize, itemToRepair.height * iconSlotSize);
    //             itemRepairIcon.gameObject.SetActive(true);
    //             inventory.ReturnDraggedItem();
    //             singleRepairCoin.gameObject.SetActive(true);
    //             singleRepairLabel.gameObject.SetActive(true);
    //         }
    //         else
    //         {
    //             messageManager.AddMessage(Color.red, "Item is already at max durability.");
    //         }
    //     }
    // }

    // //Called when the repair a single item button is clicked
    // public void OnRepairSingleClick()
    // {
    //     int repairCost = (int)(itemToRepair.totalRepairCost * 0.1f * (1 - ((float)itemToRepair.curDurability / itemToRepair.maxDurability)));
    //     if (itemToRepair.itemName != "")
    //     {
    //         if (player.money >= repairCost)
    //         {
    //             player.money -= repairCost;
    //             avaliableGoldText.text = player.money.ToString();
    //             itemToRepair.curDurability = itemToRepair.maxDurability;
    //             itemToRepair = new Items();
    //             itemToRepair.itemName = "";
    //             itemRepairIcon.gameObject.SetActive(false);
    //             repairSingleText.text = "Place item to see repair cost.";
    //             repairSingleText.rectTransform.sizeDelta = new Vector2(repairSingleText.preferredWidth, repairSingleText.preferredHeight);

    //             singleRepairCoin.gameObject.SetActive(false);
    //             singleRepairLabel.gameObject.SetActive(false);

    //             transform.root.GetComponent<AudioSource>().PlayOneShot(repairSound);
    //         }
    //         else
    //         {
    //             messageManager.AddMessage(Color.red, "I need more gold!");
    //         }
    //     }
    // }

    // //Called when the equipped item repair button is clicked
    // public void OnRepairEquippedClick()
    // {
    //     int repairCost = 0;
    //     for (int i = 0; i < inventory.equipmentSlots.Count; i++)
    //     {
    //         if (inventory.equipmentSlots[i].item.itemName != "" && inventory.equipmentSlots[i].item.curDurability < inventory.equipmentSlots[i].item.maxDurability)
    //         {
    //             repairCost += (int)(inventory.equipmentSlots[i].item.totalRepairCost * 0.1f * (1 - ((float)inventory.equipmentSlots[i].item.curDurability / inventory.equipmentSlots[i].item.maxDurability)));
    //         }
    //     }
    //     if (player.money >= repairCost)
    //     {
    //         for (int i = 0; i < inventory.equipmentSlots.Count; i++)
    //         {
    //             if (inventory.equipmentSlots[i].item.itemName != "" && inventory.equipmentSlots[i].item.curDurability < inventory.equipmentSlots[i].item.maxDurability)
    //             {
    //                 inventory.equipmentSlots[i].item.curDurability = inventory.equipmentSlots[i].item.maxDurability;
    //             }
    //         }
    //         player.money -= repairCost;
    //         avaliableGoldText.text = player.money.ToString();
    //         transform.root.GetComponent<AudioSource>().PlayOneShot(repairSound);
    //     }
    //     else
    //     {
    //         messageManager.AddMessage(Color.red, "I need more gold!");
    //     }
    // }

    // //Called when the repair all button is clicked
    // public void OnRepairAllClick()
    // {
    //     int repairCostAll = 0;
    //     for (int i = 0; i < inventory.equipmentSlots.Count; i++)
    //     {
    //         if (inventory.equipmentSlots[i].item.itemName != "" && inventory.equipmentSlots[i].item.curDurability < inventory.equipmentSlots[i].item.maxDurability)
    //         {
    //             repairCostAll += (int)(inventory.equipmentSlots[i].item.totalRepairCost * 0.1f * (1 - ((float)inventory.equipmentSlots[i].item.curDurability / inventory.equipmentSlots[i].item.maxDurability)));
    //         }
    //     }
    //     for (int i = 0; i < inventory.items.Count; i++)
    //     {
    //         if (i == inventory.items[i].itemStartNumber && inventory.items[i].item.itemName != "" && inventory.items[i].item.curDurability < inventory.items[i].item.maxDurability)
    //         {
    //             repairCostAll += (int)(inventory.items[i].item.totalRepairCost * 0.1f * (1 - ((float)inventory.items[i].item.curDurability / inventory.items[i].item.maxDurability)));
    //         }
    //     }

    //     if (player.money >= repairCostAll)
    //     {
    //         for (int i = 0; i < inventory.equipmentSlots.Count; i++)
    //         {
    //             if (inventory.equipmentSlots[i].item.itemName != "" && inventory.equipmentSlots[i].item.curDurability < inventory.equipmentSlots[i].item.maxDurability)
    //             {
    //                 inventory.equipmentSlots[i].item.curDurability = inventory.equipmentSlots[i].item.maxDurability;
    //             }
    //         }
    //         for (int i = 0; i < inventory.items.Count; i++)
    //         {
    //             if (inventory.items[i].item.itemName != "" && inventory.items[i].item.curDurability < inventory.items[i].item.maxDurability)
    //             {
    //                 inventory.items[i].item.curDurability = inventory.items[i].item.maxDurability;
    //             }
    //         }
    //         player.money -= repairCostAll;
    //         avaliableGoldText.text = player.money.ToString();
    //         transform.root.GetComponent<AudioSource>().PlayOneShot(repairSound);
    //     }
    //     else
    //     {
    //         messageManager.AddMessage(Color.red, "I need more gold!");
    //     }
    // }

    //Called when a merchant slot is clicked
    public void OnMerchantSlotClick(GameObject obj, int mouseIndex)
    {
       MerchantSlot slot = obj.GetComponent<MerchantSlot>();

       if (inventory.dragging)
       {
           int i = inventory.dragStartIndex;
           inventory.ReturnDraggedItem();
           inventory.SellItem(inventory.items[i]);
       }
       
       //
       if (mouseIndex == 0)
       {
           if (slot.item.itemName!="")
           {
               draggedItem = slot.GetComponent<MerchantSlot>();
               draggedItem.item.stackSize = 1;
               //Set Icon
               dragging = true;
               inventory.dragItem.sprite = draggedItem.item.icon;
               inventory.dragItem.gameObject.SetActive(true);
               inventory.dragItem.gameObject.transform.localScale=Vector3.one;
               inventory.dragItem.GetComponent<RectTransform>().sizeDelta= 
                   new Vector2(draggedItem.item.width * iconSlotSize,
                       draggedItem.item.height*iconSlotSize);
           }
           else
           {
               if (dragging==true && Input.GetKeyDown(KeyCode.Mouse1))
               {
                   inventory.ReturnDraggedItem();
                  
               }
           }
           
           //Right Click Buy Item
       }
       else if (mouseIndex ==1)
       {
           if (slot.item.itemName != "")
           {

               Debug.Log("Buy" + slot.item.itemName.ToString());

               BuyItem(items[slot.itemStartNumber]);
           }
       }
       
    }

    //Called when the mouse is hovering above the merchant slot
    public void OnMerchantSlotEnter(GameObject obj)
    {
        Debug.Log("Enter The Item");
        Items item = obj.GetComponent<MerchantSlot>().item;

        if (item.itemName == "" || inventory.dragging) return;
        
        
//            CursorManager.ChangeCursor("SellCursor");
        StartCoroutine(tooltip.ShowTooltip(false, item, SlotType.Merchant, obj.GetComponent<MerchantSlot>().itemStartNumber, obj.GetComponent<RectTransform>(), true));;
                                                                                                                                                                         
         

    }




    //Called when the mouse isn't hovering over the slot anymore
    public void OnMerchantSlotExit()
    {
        tooltip.HideTooltip();
        // CursorManager.ChangeCursor("Default");
 
        }

    //Opens or closes the merchant
    public void OpenCloseMerchant(bool state)
    {
        foreach (Transform trans in transform)
        {
            trans.gameObject.SetActive(state);
        }
        showMerchant = state;
        // repair.SetActive(false);
        // if (state == true)
        // {
        //     GameObject.FindGameObjectWithTag("Crafting").GetComponent<CraftManager>().OpenCloseWindow(!state);
        // }
        if (!inventory.showInventory && state)
        {
            inventory.showInventory = true;
            inventory.OpenCloseInventory(inventory.showInventory);
        }

      
    }

   public void CloseMerchant(){
        OpenCloseMerchant(false);
    }
}

