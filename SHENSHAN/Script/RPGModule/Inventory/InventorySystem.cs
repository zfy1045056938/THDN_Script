 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Reflection;
using DG.Tweening;
 using Mirror.Examples.Pong;
 using PixelCrushers.DialogueSystem;
using Mirror;
 using PixelCrushers;
 using Steamworks;
 using Unity.Mathematics;
 using GameDataEditor;

public enum SetDetailType{
	None,
	Str,
	Dex,
	Inte,
	Hp,
	Mana,
	Damage,
	Armor,
	Flash,
	SED,
	FR,
	IR,
	PR,
	ER,
	BR,

}
 public class InventorySystem : MonoBehaviour
 {

	 public UIPanel panel;
	public static InventorySystem instance;
public float slotIconSize = 39f;					//Size of the icons
	public int inventoryHeight;							//How many rows should the inventory contain?
	public int inventoryWidth;							//How many coloumns should the inventory contain?
	
	public List<EquipmentSlot> equipmentSlots;		//List of the equipment slots

	public bool dragSwap;								//Can you swap items when dragging?
	public bool autoFindEquipmentSlot;					//Should the equipment slot be found automaticly?
	public bool rightClickUnequipItems;					//Can you unequip equipped items be right clicking them?
	public bool closeIfMerchantOpen;					//Also close when the merchant closes
	public bool snapItemWhenDragging;

	//Color for when drag snapping
	private Color snapCanFitColor=Color.green;
	private Color snapCannotFitColor =Color.red;

	//Colors for the item qualities
	[Header("ItemRatity Color")]
	public Color ancientColor;
	public Color legendaryColor;
	public Color normalColor;
	public Color magicColor;
	public Color rareColor;
	public Color junkColor;
	public Color setColor;

	public Image dragItem;								//The icon of the dragged item
	public Image dragItemBackground;					//The background of the dragged item
	public Text dragStackText;							//The stacksize label of the dragged item
	public GameObject splitWindow;						//The GameObject of the split item window
	public Sprite buyBackSprite;						//The sprite to add to the merchant when an item is sold
	public ItemDatabase database;						//Reference to the item database
	public TooltipManager tooltip;							//The tooltip
//	 public TooltipManager3 tooltip;
//	public TooltipManager2 tooltip;
	// public AudioClip sellSound;							//Sound to play when an item is sold
	public Transform inventorySlots;					//The transform that holds the slots
	public GameObject slotPrefab;						//The inventory slot prefab
	public GameObject itemCanvas;						//Canvas to add to items that's dropped on the ground
	public GameObject identifyObj;
	[Header("Series")]
	private bool _hasSet;
	public bool hasSet{
		get{
			return _hasSet;
		}
		set{
			_hasSet=value;
		}
	}

    public int setNum { get => _setNum; set => _setNum = value; }
    public string setName { get => _setName; set => _setName = value; }

    private int _setNum = 0;
    private string _setName;


    [Header("Sound")] public AudioClip equipSound;
	public AudioClip dragSound;
	public AudioClip SellSound;

	public AudioClip potionSound;
	//Variables that doesn't need to be displayed in the inspector but must be public

	[HideInInspector]
	public int dragStartIndex;
	[HideInInspector]
	public Merchant merchant;
	[HideInInspector]
	public PlayerData player;
	[HideInInspector]
	public InventorySlot itemToSplit;
	[HideInInspector]
	public Items draggedItem;
	[HideInInspector]
	public List<InventorySlot> items;
	[HideInInspector]
	public bool dragging;
	[HideInInspector]
	public MessageManager messageManager;
	[HideInInspector]
	public bool identifying;
	[HideInInspector]
	public bool startIdentifying;

	[HideInInspector] 
	public bool equipGem;
	[HideInInspector]
	public InventorySlot identifyingScrollOrignalSlot;
	[HideInInspector]
	public bool showInventory;
	[HideInInspector]
	public InventorySlot hoveredSlot;
	[HideInInspector] public CharacterInfoPanel statPanel;
	[HideInInspector] public Items unItem;
	//delegate
	public delegate void CheckSet(int setNum);
	// Use this for initialization
	void Awake ()
	{

		if (instance == null) instance = this;
		
		
		//
		statPanel = GameObject.FindObjectOfType<CharacterInfoPanel>();
		//
		
		
		//Add the amount of slots we want
		for(int i = 0; i < inventoryWidth * inventoryHeight; i++) {
			GameObject slot = Instantiate(slotPrefab) as GameObject;
			slot.transform.SetParent(inventorySlots);
			slot.name = i.ToString();
			slot.transform.localScale = Vector3.one;

			//
			InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();

			//
			inventorySlot.item = new Items();
			inventorySlot.item.itemName = "";
			inventorySlot.itemStartNumber = i;
			items.Add(inventorySlot);

		}
		//Get the grid component of the inventory slot transform
		GridLayoutGroup grid = GetComponentInChildren<GridLayoutGroup>();
		//Set the amount of coloumns equal to the width of the inventory
		grid.constraintCount = inventoryWidth;
		//Set the size of the slots equal to the slot size
		grid.cellSize = new Vector2(slotIconSize,slotIconSize);
		//Reset all the names of the equipment slots
		for(int i = 0; i < equipmentSlots.Count; i++) {
			equipmentSlots[i].item = new Items();
			equipmentSlots[i].item.itemName = "";
		}
		
		
		//
	
		//Hide the inventory
		// OpenCloseInventory(false);
	}

	void Start() {


		
			//Find the instance of the message manager
			messageManager = GameObject.FindGameObjectWithTag("MessageManager").GetComponent<MessageManager>();
			//Find the instance of the player
			player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
			//Find the instance of the merchant
			merchant = GameObject.FindGameObjectWithTag("Merchant").GetComponent<Merchant>();
			//
			statPanel = GameObject.FindObjectOfType<CharacterInfoPanel>();
			//
			if (instance == null) instance = this;

			//Set all the slots equal to un-interactable
			for (int i = 0; i < items.Count; i++)
			{
				items[i].GetComponent<CanvasGroup>().interactable = false;
			}

			//LoadDB
			if (NetworkClient.isConnected)
			{

				ShenShanDB.singleton.LoadInventory(player);
			}

			
		

			//
			player.equipSlot = equipmentSlots;
			player.inventory = this;
			//Hide the inventory
//			OpenCloseInventory(false);
		//    panel.Close();

	}

	

	public void LoadContent()
	{
		//Set all the slots equal to un-interactable
		for (int i = 0; i < items.Count; i++)
		{
			items[i].GetComponent<CanvasGroup>().interactable = false;
		}

		//LoadDB
		if (NetworkClient.isConnected)
		{

			ShenShanDB.singleton.LoadInventory(player);
		}

		LoadInventory();
		LoadEquipment();
			
		//Add the amount of slots we want
		for (int i = 0; i < inventoryWidth * inventoryHeight; i++)
		{
			GameObject slot = Instantiate(slotPrefab) as GameObject;
			slot.transform.SetParent(inventorySlots);
			slot.name = i.ToString();
			slot.transform.localScale = Vector3.one;

			//
			InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();

			//
			inventorySlot.item = new Items();
			inventorySlot.item.itemName = "";
			inventorySlot.itemStartNumber = i;
			items.Add(inventorySlot);

		}

		//Get the grid component of the inventory slot transform
		GridLayoutGroup grid = GetComponentInChildren<GridLayoutGroup>();
		//Set the amount of coloumns equal to the width of the inventory
		grid.constraintCount = inventoryWidth;
		//Set the size of the slots equal to the slot size
		grid.cellSize = new Vector2(slotIconSize, slotIconSize);
		//Reset all the names of the equipment slots
		for (int i = 0; i < equipmentSlots.Count; i++)
		{
			equipmentSlots[i].item = new Items();
			equipmentSlots[i].item.itemName = "";
		}

		//
		player.equipSlot = equipmentSlots;
		player.inventory = this;
	}
	public void LoadInventory()
	{
		
			for (int i = 0; i < items.Count; i++)
			{
				if (PlayerPrefs.HasKey("ItemID_" + i))
				{
					//has key load by id
					string itemID = PlayerPrefs.GetString("ItemID_" + i);
					int nItemId = Int32.Parse(itemID);
					int itemHeight = PlayerPrefs.GetInt("ItemHeight_" + i+"_"+nItemId);
					int itemWidth = PlayerPrefs.GetInt("ItemWidth_" + i+"_"+nItemId);
//					string iconName = PlayerPrefs.GetString("ItemIcon" + i+"_"+nItemId);
					string itemName = PlayerPrefs.GetString("ItemName_" + i+"_"+nItemId);

//					int nItemId = Int32.Parse(itemID);
					Items item = ItemDatabase.instance.FindItemLoad(nItemId);
					//
					if (item != null)
					{
						//Load Item By PO data 

						//Here we calculate wether there's any items occupying the slots that the item would fill
						int counter = 0;
						for (int j = 0; j < itemHeight; j++)
						{
							for (int k = 0; k < itemWidth; k++)
							{
								//Add Count For item 
								if (items[i + inventoryWidth * j + k].item.itemName != "")
								{
									counter++;
								}
							}
						}

						//There's no items in the slots that the item occupies
						//So we can add the item now
						if (counter == 0)
						{
							for (int l = 0; l < itemHeight; l++)
							{
								for (int m = 0; m < itemWidth; m++)
								{
									//First we add the items to the slots the it fills and set their slots to clear
									items[i + inventoryWidth * l + m].item = DeepCopy(item);

									items[i + inventoryWidth * l + m].itemStartNumber = i;
									items[i + inventoryWidth * l + m].GetComponent<Image>().color = Color.clear;
									items[i + inventoryWidth * l + m].stackSizeText.gameObject.SetActive(false);
									//If it's the first index of the added item
									if (items.IndexOf(items[i + inventoryWidth * l + m]) == i)
									{
										//
										SetSlotImageSprite(items[i + inventoryWidth * l + m], item.icon);
										items[i + inventoryWidth * l + m].itemFrame.gameObject.SetActive(true);
										items[i + inventoryWidth * l + m].itemFrame.GetComponent<CanvasGroup>()
											.interactable = true;
										items[i + inventoryWidth * l + m].itemFrame.GetComponent<CanvasGroup>()
											.blocksRaycasts = true;
										items[i + inventoryWidth * l + m].GetComponent<CanvasGroup>().blocksRaycasts =
											true;
										items[i + inventoryWidth * l + m].itemFrame.rectTransform.sizeDelta =
											new Vector2(itemWidth * slotIconSize, itemHeight * slotIconSize);
//								//If the item is stackable
								if(item.stackable) {
									items[i + inventoryWidth * l + m].item.stackSize=int.Parse(PlayerPrefs.GetString("StackSize_"+item.itemName+"_"+i, item.stackSize.ToString()));
									items[i + inventoryWidth * l + m].stackSizeText.gameObject.SetActive(true);
									items[i + inventoryWidth * l + m].stackSizeText.text =
										PlayerPrefs.GetString("StackSize_"+item.itemName+"_"+i, item.stackSize.ToString());
								}
								
								bool hasUni = PlayerPrefsX.GetBool("Unid_"+item.itemName);
//								//The item is unidentified
								if(item.unidentified) {
									items[i + inventoryWidth * l + m].itemImage.color = Color.red;
									items[i + inventoryWidth * l + m].unidentfied.gameObject.SetActive(hasUni);
								}
									}
								}
							}
						}
					}
					else
					{
						//do nothing
					}
				}
				else
				{
					//not id in ps
				}
			}
		
	}

	public void LoadEquipment()
	{
		
		for (int i = 0; i < equipmentSlots.Count; i++)
		{
			if (PlayerPrefs.HasKey("EquipID" + i+"_"+equipmentSlots[i].equipmentSlotType))
			{
				string equipItemID = PlayerPrefs.GetString("EquipID" + i+"_"+equipmentSlots[i].equipmentSlotType);
				string equipItemType = PlayerPrefs.GetString("EquipSlot"+i);
				bool hasUni = PlayerPrefsX.GetBool("Unid_" + equipmentSlots[i].item.itemName);
				//Get item
				Items item = ItemDatabase.instance.FindItem(int.Parse(equipItemID));
				
				for (int j = 0; j < equipmentSlots.Count; j++)
				{
					if (equipmentSlots[j].equipmentSlotType == item.equipmentSlotype)
					{
						equipmentSlots[j].item = DeepCopy(item);
						equipmentSlots[j].GetComponentInChildren<EquipmentSlot>().itemIcon.sprite = item.icon;
						//Set Sprite To target slot
//						SetSlotImageSprite(equipmentSlots[j], equipmentSlots[j].item.icon);
						equipmentSlots[j].itemIcon.rectTransform.sizeDelta = 
							new Vector2(item.width * slotIconSize * equipmentSlots[j].iconSclarFactor,
								item.height * slotIconSize *  equipmentSlots[j].iconSclarFactor);
						equipmentSlots[j].itemIcon.gameObject.SetActive(true);
						equipmentSlots[j].transform.Find("ItemBackground").gameObject.SetActive(false);
						equipmentSlots[i].item.unidentified = hasUni;
						//
						if (equipmentSlots[i].item.stackable==true)
						{
							equipmentSlots[i].item.stackSize = item.stackSize;
							equipmentSlots[j].item.stackSize=PlayerPrefs.GetInt("EquipPotion"+i+"_"+"Count",item.stackSize);
						}
					}
					else
					{
						
					}
				}

			}
			else
			{
				//no key
			}
		}
		//LoadSet
		if(PlayerPrefs.HasKey("PlayerSetNum")){
			setNum = PlayerPrefs.GetInt("PlayerSetNum");
		}
		if(PlayerPrefs.HasKey("PlayerSetName")){
			setName=PlayerPrefs.GetString("PlayerSetName");
		}
		
	}
	// Update is called once per frame
	//FIXED:: LAYER SET INVENTORY TO DEFAULT UND BACKGROUND LAYER SET TO UI LAYER 
	void Update ()
	{
		player = PlayerData.localPlayer;
		
		  if (Input.GetKeyDown(KeyCode.I))
        {
            showInventory = !showInventory;
            OpenCloseInventory(showInventory);
        }



        //if drag item
        if (dragging)
        {
            if (snapItemWhenDragging)
            {
                if (hoveredSlot)
                {
	                
                    dragItem.rectTransform.position = hoveredSlot.transform.position;
//			dragItem.rectTransform.position= new Vector3(Input.mousePosition.x+dragItem.rectTransform.sizeDelta.x,
//				Input.mousePosition.y-dragItem.rectTransform.sizeDelta.x);
//	                dragItem.rectTransform.position = new Vector3(Input.mousePosition.x
//	                                                              +dragItem.rectTransform.sizeDelta.x
//	                                                              *dragItem.rectTransform.lossyScale.x *0.5f,
//		                Input.mousePosition.y - dragItem.rectTransform.sizeDelta.x 
//		                * dragItem.rectTransform.lossyScale.y * 0.5f,-20f);
                    dragItem.rectTransform.sizeDelta=new Vector2(draggedItem.width*slotIconSize,draggedItem.height*slotIconSize);
                    dragItem.rectTransform.position=new Vector3(Input.mousePosition.x+90.0f,Input.mousePosition.y-70f,-20f);
                    dragItemBackground.rectTransform.position = new Vector3(Input.mousePosition.x+90.0f,Input.mousePosition.y-70f,-20f);
                    dragItemBackground.rectTransform.sizeDelta = new Vector2(draggedItem.width*slotIconSize ,
                        draggedItem.height*slotIconSize );
                    //check edge
                    if (CheckItemFit(draggedItem,hoveredSlot,false))
                    {
                         dragItemBackground.color = snapCanFitColor;
                    }
                    else
                    {
                        dragItemBackground.color = snapCannotFitColor;
                    }
                }
                else
                {
                    dragItem.rectTransform.position = Input.mousePosition;
                    dragItemBackground.rectTransform.position = Input.mousePosition;
                    dragItemBackground.color = snapCannotFitColor;
                }
            }
            else
            {
                //Drag item equals mouse pos
                dragItem.rectTransform.position = new Vector3(Input.mousePosition.x
               +dragItem.rectTransform.sizeDelta.x
               *dragItem.rectTransform.lossyScale.x ,
           Input.mousePosition.y - dragItem.rectTransform.sizeDelta.x 
           * dragItem.rectTransform.lossyScale.y,-20f);
            }

            //
//            if (draggedItem.stackable)
//            {
//                dragStackText.gameObject.SetActive(true);
//                dragStackText.text = draggedItem.maxStackSize.ToString();
//            }
//            else
//            {
//                dragStackText.gameObject.SetActive(false);
//            }


            if(Input.GetMouseButtonDown(1)){
                ReturnDraggedItem();
            }
        }

        //If the player is currently identifying an item
        if (identifying)
        {
            //Abort if the player right clicks or presses escape
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                identifying = false;
              identifyObj.gameObject.SetActive(false);
            }
        }

        //We need to delay this because of the delay of the buttons
        if (startIdentifying)
        {
            startIdentifying = false;
            identifying = true;
        }
		
        //save 
        


	}

	//Instantiate the dropped item in world space using the world object of the dragged item
	public void DropDraggedItem() {
		// GameObject obj = Instantiate(draggedItem.worldObject, 
		// 	player.transform.position + player.transform.forward,
		// 	 Quaternion.identity) as GameObject;
		// ItemsController ic = obj.AddComponent<ItemsController>();
		// ic.item = DeepCopy(draggedItem);
		// GameObject itemCanvasObj = Instantiate(itemCanvas) as GameObject;
		// itemCanvasObj.transform.SetParent(obj.transform);
		// itemCanvasObj.transform.localPosition = new Vector3(0,1,0);
		// StopDragging();
	}

	//Add a stackable item to the inventory
	public bool AddStackableItem(Items item) {
		Debug.Log("Add StackItem"+item.itemName);
		//Run through all the slots
		for(int i = 0; i < items.Count; i++) {
			//If the item in the slot is the same and the one the player is trying to add and the item isn't already at max stacksize
			if(items[i].item.itemName != "" 
				&& items[i].item.itemName == item.itemName 
				&& items[i].item.stackSize != items[i].item.maxStackSize) {
					Debug.Log("Have Same Item then add stacksize curr stacksize"+items[i].item.stackSize);
				//Calculate the complete stacksize
				int count = items[i].item.stackSize + item.stackSize;
				//If the complete stacksize is below max then add the stack to the item
				if (count <= items[i].item.maxStackSize)
				{
					items[i].item.stackSize = count;
					items[i].stackSizeText.text = items[i].item.stackSize.ToString();
					
				Debug.Log("Save StackSize for items");
				PlayerPrefs.SetString("StackSize_"+items[i].item.itemName+"_"+i, items[i].item.stackSize.ToString());
					return true;
				}
				else if(count > items[i].item.maxStackSize) {
					Debug.Log("Stack max new size for items");
					Items temp = DeepCopy(item);
					temp.stackSize = count - items[i].item.maxStackSize;
					if(AddItem(temp)) {
						items[i].item.stackSize = items[i].item.maxStackSize;
						return true;
					}
					else {
						return false;
					}
				}
			}
			else
			{
				//We've searched all of the slots and there's no item matching the stackable item
//				//So add it to the inventory
				if (i == items.Count )
				{
					AddItem(item);
					return true;
				}
			}
		}
		return false;
	}

	//Adds an item to the inventory
	public bool AddItem(Items item) {
		for (int i = 0; i < items.Count; i++) {
			//Found an empty slot
			if(items[i].item.itemName == "") {
				//If the item doesn't fit in the slow just go to the next
				if(!CheckItemFit(item,items[i], false)) {
					continue;
				}
				//Here we calculate wether there's any items occupying the slots that the item would fill
				int counter = 0;
				for(int j = 0; j < item.height; j++) {
					for(int k = 0; k < item.width; k++) {
						if(items[i + inventoryWidth * j + k].item.itemName != "") {
							counter++;
						}
					}
				}
				//There's no items in the slots that the item occupies
				//So we can add the item now
				if(counter == 0) {
					Debug.Log("Enter Counter\t\t"+item.itemName);
					for(int l = 0; l < item.height; l++) {
						for(int m = 0; m < item.width; m++) {
							//First we add the items to the slots the it fills and set their slots to clear
							items[i + inventoryWidth * l + m].item = DeepCopy(item);
							PlayerPrefs.SetString("ItemID_"+i,item.itemID);
							Debug.Log("Set Item ID"+item.itemID);
							PlayerPrefs.SetString("ItemName_"+i+"_"+item.itemID,item.itemName);
							items[i + inventoryWidth * l + m].itemStartNumber = i;
							items[i + inventoryWidth * l + m].GetComponent<Image>().color = Color.clear;
							items[i + inventoryWidth * l + m].stackSizeText.gameObject.SetActive(false);
							//If it's the first index of the added item
							if(items.IndexOf(items[i + inventoryWidth * l + m]) == i) {
								SetSlotImageSprite(items[i + inventoryWidth * l + m], item.icon);
							
								
								
								items[i + inventoryWidth * l + m].itemFrame.gameObject.SetActive(true);
								items[i + inventoryWidth * l + m].itemFrame.GetComponent<CanvasGroup>().interactable = true;
								items[i + inventoryWidth * l + m].itemFrame.GetComponent<CanvasGroup>().blocksRaycasts = true;
								items[i + inventoryWidth * l + m].GetComponent<CanvasGroup>().blocksRaycasts = true;
								items[i + inventoryWidth * l + m].itemFrame.rectTransform.sizeDelta = 
								new Vector2(item.width * slotIconSize, item.height * slotIconSize);
								//Save Item size
//								PlayerPrefs.SetString("ItemIcon"+i+"_"+item.itemID,item.icon.name);
								PlayerPrefs.SetInt("ItemWidth"+"_"+i+"_"+item.itemID,item.width);
								PlayerPrefs.SetInt("ItemHeight"+"_"+i+"_"+item.itemID,item.height);
								
								//If the item is stackable
								if(item.stackable) {
									items[i + inventoryWidth * l + m].stackSizeText.gameObject.SetActive(true);
									items[i + inventoryWidth * l + m].stackSizeText.text = item.stackSize.ToString();
									
								}

								
								//The item is unidentified
								if(item.unidentified)
								{
									items[i + inventoryWidth * l + m].itemImage.color = Color.red;
									items[i + inventoryWidth * l + m].unidentfied.gameObject.SetActive(true);
									PlayerPrefsX.SetBool("Unid_"+item.itemName,true);
								}
								
								//
							}
						}
					}
					//Item succesfully added
					return true;
				}
			}
		}
		//Item unsuccesfully added
		return false;
	}

	//Add an item at a specific slot
	public bool AddItemAtSlot(Items item, InventorySlot slot) {
		int i = items.IndexOf(slot);
		for(int j = 0; j < item.height; j++) {
			for(int k = 0; k < item.width; k++) {
				//The item we want to add doesn't fit so just return
				if(!CheckItemFit(item,slot,true)) {
					return false;
				}
				//There's something in the slot we want to add the item
				if(items[i + inventoryWidth * j + k].item.itemName != "") {
					//If the player can drag and swap items
					if(dragSwap) {
						//Replace the dragged item and item in the slot
						int counter = 0;
						InventorySlot foundSlot = null;
						int itemStartNumber = Mathf.RoundToInt(Mathf.Infinity);
						for(int l = 0; l < item.height; l++) {
							for(int m = 0; m < item.width; m++) {
								if(items[slot.itemStartNumber + inventoryWidth * l + m].item.itemName 
									!= "" && itemStartNumber != items[slot.itemStartNumber + inventoryWidth * l + m].itemStartNumber) 
								{
									itemStartNumber = items[slot.itemStartNumber + inventoryWidth * l + m].itemStartNumber;
									counter++;
									foundSlot = items[slot.itemStartNumber + inventoryWidth * l + m];
								}
								
							}
						}
						if(counter == 1) {
							Items tempItem = DeepCopy(draggedItem);
							DragItemFromSlot(foundSlot);
							AddItemAtSlot(tempItem, slot);
							// transform.root.GetComponent<AudioSource>().PlayOneShot(items[i + inventoryWidth * j + k].item.itemSound);
							return false;
						}
						else {
							return false;
						}
					}
					else {
						return false;
					}
				}
				//There's no items in the slots that the item occupies
				//So we can add the item now
				if(j == item.height - 1 && k == item.width - 1) {
					for(int l = 0; l < item.height; l++) {
						for(int m = 0; m < item.width; m++) {
							//Add the item to the slots that the item fills and set their icons to clear
							items[i + inventoryWidth * l + m].item = DeepCopy(item);
							items[i + inventoryWidth * l + m].itemStartNumber = i;
							items[i + inventoryWidth * l + m].GetComponent<Image>().color = Color.clear;
							items[i + inventoryWidth * l + m].stackSizeText.gameObject.SetActive(false);
							PlayerPrefs.SetString("ItemID_"+i,item.itemID);
							PlayerPrefs.SetString("ItemName_"+i+"_"+item.itemID,item.itemName);
							//If it's the first index of the added item
							//Set the icon and frame to the size of the item and the color to white
							if(items.IndexOf(items[i + inventoryWidth * l + m]) == i) {
								SetSlotImageSprite(items[i + inventoryWidth * l + m],item.icon);
								items[i + inventoryWidth * l + m].itemFrame.gameObject.SetActive(true);
								items[i + inventoryWidth * l + m].itemFrame.GetComponent<CanvasGroup>().interactable = true;
								items[i + inventoryWidth * l + m].itemFrame.GetComponent<CanvasGroup>().blocksRaycasts = true;
								items[i + inventoryWidth * l + m].GetComponent<CanvasGroup>().blocksRaycasts = true;
								items[i + inventoryWidth * l + m].itemFrame.rectTransform.sizeDelta = new Vector2
								(item.width * slotIconSize, item.height * slotIconSize);
								//
//								PlayerPrefs.SetString("ItemIcon"+i+"_"+item.itemID,item.icon.name);
								PlayerPrefs.SetInt("ItemWidth"+"_"+i+"_"+item.itemID,item.width);
								PlayerPrefs.SetInt("ItemHeight"+"_"+i+"_"+item.itemID,item.height);

								//If the item is unidentified
								if(item.unidentified) {
									items[i + inventoryWidth * l + m].itemImage.color = Color.red;
									items[i + inventoryWidth * l + m].unidentfied.gameObject.SetActive(true);
									PlayerPrefsX.SetBool("Unid_"+item.itemName,true);
								}
								//If the item is stackable
								if(item.stackable) {
									items[i + inventoryWidth * l + m].stackSizeText.gameObject.SetActive(true);
									items[i + inventoryWidth * l + m].stackSizeText.text = item.stackSize.ToString();
									PlayerPrefs.SetString("StackSize_"+item.itemName, item.stackSize.ToString());
								}
							}
						}
					}
					//Item was successfully added
					return true;
				}
			}
		}
		//Item was unsuccessfully added
		return false;
	}

	//Swap the dragged item and the item of the slot clicked
	public bool SwapItems(InventorySlot slot) {
		//Make a copy of the item in the slot
		Items item = DeepCopy(items[slot.itemStartNumber].item);
		//Remove the item from the slot
		RemoveItemFromSlot(slot);
		AddItemAtSlot(draggedItem, slot);
		//Start dragging
		dragging = true;
		draggedItem = item;
		dragItem.sprite = item.icon;
		dragItem.rectTransform.sizeDelta = new Vector2(item.width * slotIconSize, item.height * slotIconSize);
		dragItem.gameObject.SetActive(true);
		
		return true;
	}

	//Remove item from it's slot and start dragging
	public bool DragItemFromSlot(InventorySlot slot) {
		//If the merchant's repair window is open and the item is added to the repair slot then remove it from the repair slot
		if(merchant.repair.activeSelf) {
			if(items[slot.itemStartNumber].item == merchant.itemToRepair) {
				merchant.itemToRepair = new Items();
				merchant.itemToRepair.itemName = "";
//				merchant.itemRepairIcon.gameObject.SetActive(false);
//				merchant.singleRepairCoin.gameObject.SetActive(false);
//				merchant.singleRepairLabel.gameObject.SetActive(false);
//				merchant.repairSingleText.text = "Place item to see repair cost.";
			}
		}
		//Make a copy of the item in the slot and remove it from the slot and set the copied item equal to the dragged item

		Items item = DeepCopy(slot.item);
	    SoundManager.instance.PlaySound(dragSound);
		dragStartIndex = slot.itemStartNumber;
		RemoveItemFromSlot(slot);
		dragging = true;
		draggedItem = item;
		dragItem.sprite = item.icon;
		dragItem.rectTransform.sizeDelta = 
		new Vector2(item.width * slotIconSize, item.height * slotIconSize);
		dragItem.gameObject.SetActive(true);
		//CursorManager.ChangeCursor("Default");
		return true;
			
		
	}

	//Return the dragged item to the slot it came from
	public void ReturnDraggedItem() {

		AddItemAtSlot(draggedItem, items[dragStartIndex]);

		StopDragging();
	}

	//Equip an item for armor und ring
	public bool EquipItem(InventorySlot slot) {
		//First we check if the item is of type offhand
		SoundManager.instance.PlayClipAtPoint(equipSound, Vector3.zero, SoundManager.instance.musicVolume,
    						true);
//			for(int i = 0; i < equipmentSlots.Count; i++) {
//				//If the player already has a two handed weapon equipped and can't equip both a shield and a two handed weapon
//				//Return the two handed weapon to the inventory
//				if(equipmentSlots[i].equipmentSlotType == EquipmentSlotType.armor ) {
//				
//						AddItem(equipmentSlots[i].item);
//						RemoveEquippedItem(equipmentSlots[i]);
//					
//				}
//			}
		
//		//Check if the item is of type weapon and is two-handed
//		if(slot.item.itemType == EquipmentSlotType.weapon && slot.item.twoHanded) {
//			for(int i = 0; i < equipmentSlots.Count; i++) {
//				//If the player already has a offhand equipped and can't equip both a shield and a two handed weapon
//				//Return the offhand to the inventory
//				if(equipmentSlots[i].equipmentSlotType == EquipmentSlotType.offHand ) {
//					if(equipmentSlots[i].item.itemType == EquipmentSlotType.offHand) {
//						AddItem(equipmentSlots[i].item);
//						RemoveEquippedItem(equipmentSlots[i]);
//					}
//				}
//			}
//		}
		//Run through all the equipment slots

		if (RequiredEquip(slot.item))
		{
			for (int i = 0; i < equipmentSlots.Count; i++)
			{
				//We've found the right slot
				if (equipmentSlots[i].equipmentSlotType == slot.item.itemType)
				{
					//There's no item in the slot
					if (equipmentSlots[i].item.itemName == "")
					{
						//Equip the item at the slot
						equipmentSlots[i].item = DeepCopy(slot.item);
						//Save equip by itemid and equipslot type
						PlayerPrefs.SetString("EquipSlot", slot.item.itemType.ToString());
						PlayerPrefs.SetString("EquipID" + i, equipmentSlots[i].item.itemID);
						SetSlotImageSprite(equipmentSlots[i], equipmentSlots[i].item.icon);
						equipmentSlots[i].transform.Find("ItemBackground").gameObject.SetActive(false);
						//Check Has Set
							if(equipmentSlots[i].item.hasSet==true){
								hasSet=true;
								
						}
						return true;
					}
					//There's something in the slot
					else
					{
						//Make a copy of the item in the equipment slot
						Items item = DeepCopy(equipmentSlots[i].item);
						Items tempSlotItem = DeepCopy(slot.item);
						//Check if the item can fit in the slot where the original item came from
						if (CheckItemFit(item, items[slot.itemStartNumber], true))
						{
							int startNumber = slot.itemStartNumber;
							RemoveItemFromSlot(items[startNumber]);
							AddItemAtSlot(item, items[startNumber]);
							equipmentSlots[i].item = tempSlotItem;
							SetSlotImageSprite(equipmentSlots[i], equipmentSlots[i].item.icon);
							// transform.root.GetComponent<AudioSource>().PlayOneShot(item.itemSound);
							equipmentSlots[i].transform.Find("ItemBackground").gameObject.SetActive(false);
							StartCoroutine(tooltip.ShowTooltip(true, slot.item, SlotType.Inventory,
								slot.itemStartNumber, slot.GetComponent<RectTransform>(), false));
							return false;
						}
						//if the item doesn't fit in the slot it came from then just add the item to the inventory
						else
						{
							RemoveItemFromSlot(slot);
							equipmentSlots[i].item = tempSlotItem;
							AddItem(item);
							SetSlotImageSprite(equipmentSlots[i], equipmentSlots[i].item.icon);
							// transform.root.GetComponent<AudioSource>().PlayOneShot(item.itemSound);
							OnMouseEnter(slot.gameObject);
							equipmentSlots[i].transform.Find("ItemBackground").gameObject.SetActive(false);
							StartCoroutine(tooltip.ShowTooltip(true, slot.item, SlotType.Inventory,
								slot.itemStartNumber, slot.GetComponent<RectTransform>(), false));
							return false;
						}
					}
				}
			}

			return false;
		}
		else
		{
			return false;
		}

		return false;
	}

	//Equip an item at a specific equipment slot
	public void EquipItemAtSlot(EquipmentSlot slot, Items item) {
		//
		SoundManager.instance.PlayClipAtPoint(equipSound, Vector3.zero, SoundManager.instance.musicVolume,
			true);
		//Create a copy of the item to equip
		slot.item = DeepCopy(item);

		SetSlotImageSprite(slot, item.icon);

		slot.transform.Find("ItemBackground").gameObject.SetActive(false);

		// //Check to see if the item equipped is of type two-handed weapon
		// if(slot.item.itemType == EquipmentSlotType.weapon && slot.item.itemName!="" ) {
		// 	for(int i = 0; i < equipmentSlots.Count; i++) {
        //    AddItem(equipmentSlots[i].item);
        //    RemoveEquippedItem(equipmentSlots[i]);
		// 	}
			
		// }
//		}
//when player equip item must set und add set state with player
// 
		if(item.hasSet==true){
			player.hasSet=true;
			hasSet=true;
			int tmpSno = 0;
			//
			string tmpSetName = item.setName;
			PlayerPrefs.SetString("PlayerSetName",item.setName);
			for(int i=0;i<equipmentSlots.Count;i++){
				if(equipmentSlots[i].item.setName==tmpSetName){
						tmpSno++;
						if(tmpSno==2){
							setNum=tmpSno;
							setName = tmpSetName;
							//Active target SetDetail1
							SetSetDetail(tmpSetName,2);
							PlayerPrefs.SetInt("PlayerSetNum",setNum);
							PlayerPrefs.SetString("PlayerSetName",setName);
						}
						//
						
						if(tmpSno==3){
							setNum=tmpSno;
							//Active targe SetDetail2
SetSetDetail(tmpSetName,3);
PlayerPrefs.SetInt("PlayerSetNum",setNum);
						}
				}
			}
		}
	}

#region 套装配置

public void RemoveSet(){
	setNum--;

	if(setNum<3){
		List<GDESetListData>allList= GDEDataManager.GetAllItems<GDESetListData>();
		for(int i=0;i<allList.Count;i++){
			if(allList[i].SetListName == setName){
				if(setNum==2){
					//Active 1 effect
					RemoveSetDetail(GetSetVType(allList[i].SetType1),allList[i].Set1Amount);
				}else 
				if(setNum==3){
						RemoveSetDetail(GetSetVType(allList[i].SetType2),allList[i].SetAmount2);
				}else{
					hasSet=false;
				}
			}
		}
	}
}
	public void SetSetDetail(string setName,int num){
		List<GDESetListData>allList= GDEDataManager.GetAllItems<GDESetListData>();
		for(int i=0;i<allList.Count;i++){
			if(allList[i].SetListName == setName){
				if(num==2){
					//Active 1 effect
					SetPlayerDetail(GetSetVType(allList[i].SetType1),allList[i].Set1Amount);
				}
				if(num==3){
						SetPlayerDetail(GetSetVType(allList[i].SetType2),allList[i].SetAmount2);
				}
			}
		}
	}
	public SetDetailType GetSetVType(string s){

		if(s=="HP"){
			return SetDetailType.Hp;
		}
		return SetDetailType.None;
	}
	public void SetPlayerDetail(SetDetailType st,int num){
		switch(st){
			case SetDetailType.Hp:
				PlayerData.localPlayer.playerHealth+=num;
			break;
		}
	}
	public void RemoveSetDetail(SetDetailType st,int num){
		switch(st){
			case SetDetailType.Hp:
				PlayerData.localPlayer.playerHealth-=num;
				break;
			
			default:
			break;
		}
	}
#endregion
	//Swap an inventory item and a equipmentslot item
	public void SwapInvItemEquipped(EquipmentSlot equipSlot, InventorySlot invSlot) {
		//Create a copy of the item in the equipment slot
		Items item = DeepCopy(equipSlot.item);
		//Create a copy of the item in the inventory slot and add it to the equipment slot
		equipSlot.item = DeepCopy(invSlot.item);
		SetSlotImageSprite(equipSlot, equipSlot.item.icon);
		int startNumber = invSlot.itemStartNumber;
		//Remove the item from the inventory
		RemoveItemFromSlot(items[startNumber]);
		//Add the item from the equipment slot to the inventory
		AddItemAtSlot(item, items[startNumber]);

		//Show the tooltip for the swapped item in the inventory
		StartCoroutine(tooltip.ShowTooltip(true, invSlot.item, SlotType.Inventory, invSlot.itemStartNumber, equipSlot.GetComponent<RectTransform>(), false));
		//Play the sound of the item in the inventory
		// transform.root.GetComponent<AudioSource>().PlayOneShot(invSlot.item.itemSound);
		//
		SoundManager.instance.PlayClipAtPoint(equipSound, Vector3.zero, SoundManager.instance.musicVolume,
			true);
		//Check to see if the item equipped is of type offhand
		if(equipSlot.item.itemType == EquipmentSlotType.offHand) {
			for(int i = 0; i < equipmentSlots.Count; i++) {
				//If the player has a two-handed weapon equipped and the player can't equip both a shield and a two handed weapon
				//Then add the weapon to the inventory
				if(equipmentSlots[i].equipmentSlotType == EquipmentSlotType.weapon ) {
					if(equipmentSlots[i].item.twoHanded) {
						AddItem(equipmentSlots[i].item);
						RemoveEquippedItem(equipmentSlots[i]);
					}
				}
			}
		}
		//Check to see if the item equipped is of type two-handed weapon
		if(equipSlot.item.itemType == EquipmentSlotType.weapon && equipSlot.item.twoHanded) {
			for(int i = 0; i < equipmentSlots.Count; i++) {
				//If the player has a shield equipped and can't equip both a two-handed weapon and a shield
				//Then add the shield to the inventory
				if(equipmentSlots[i].equipmentSlotType == EquipmentSlotType.offHand ) {
					if(equipmentSlots[i].item.itemType == EquipmentSlotType.offHand) {
						AddItem(equipmentSlots[i].item);
						RemoveEquippedItem(equipmentSlots[i]);
					}
				}
			}
		}

	}

	//Function used to find the ring slot
	public bool EquipRing(InventorySlot slot) {
		SoundManager.instance.PlayClipAtPoint(equipSound, Vector3.zero, SoundManager.instance.musicVolume,
			true);
		//Run through all the equipment slots
		for(int i = 0; i < equipmentSlots.Count; i++) {
			//We've found the first ring slot
			if(equipmentSlots[i].equipmentSlotType == EquipmentSlotType.ring) {
				//If there's nothing in the slot
				if(equipmentSlots[i].item.itemName == "") {
					//Equip the item
					EquipItemAtSlot(equipmentSlots[i], slot.item);
					
					return true;
				}
				//There's already a ring in the slot
				else {
					//Run through the equipment slots again and find the next ring slot
					for(int j = 0; j < equipmentSlots.Count; j++) {
						//If the slot isn't the same as the first we found and it's of type ring
						if(equipmentSlots[i] != equipmentSlots[j] && equipmentSlots[j].equipmentSlotType == EquipmentSlotType.ring) {
							//There's nothing in this ring slot so equip the ring here
							if(equipmentSlots[j].item.itemName == "") {
								EquipItemAtSlot(equipmentSlots[j], slot.item);
								// slot.transform.Find("ItemBackground").gameObject.SetActive(false);
								return true;
							}
							//There's a ring in the slot so we swap it with the one in the inventory
							else {
								SwapInvItemEquipped(equipmentSlots[i], slot);
								slot.transform.Find("ItemBackground").gameObject.SetActive(false);
								//We need to return false here because else the ring in the inventory would get deleted
								return false;
							}
						}
					}
				}
			}
		}
		//Only returns here if there's no ring slot
		return false;
	}

	//Removes an item from a slot
	public void RemoveItemFromSlot(InventorySlot slot) {
		//Make a copy of the item in the slot
		Items item = DeepCopy(slot.item);
		//Run through all the slot that item occupies and remove it from the slot
		for(int i = 0; i < item.height; i++) {
			for(int j = 0; j < item.width; j++) {
				for (int m = 0; m < items.Count; m++)
				{
					string ItemIDs = PlayerPrefs.GetString("ItemID_" + m);
					Debug.Log(ItemIDs+"\t\ttry get fs");
					if (PlayerPrefs.HasKey("ItemID_" + m))
					{
						if(item.itemID == ItemIDs){
							PlayerPrefs.DeleteKey("ItemWidth"+"_"+m+"_"+item.width);
							PlayerPrefs.DeleteKey("ItemHeight"+"_"+ m+"_"+item.height);
							PlayerPrefs.DeleteKey("ItemName"+"_"+m+"_"+item.itemName);
//							PlayerPrefs.DeleteKey("ItemIcon" + m+"_"+item.icon);

							PlayerPrefs.DeleteKey("ItemID_"+m);
							Debug.Log("has delete" + item.itemID + "\t\t"+item.itemName);
}
					}
					
				}

				items[slot.itemStartNumber + inventoryWidth * i + j].item = new Items();
				items[slot.itemStartNumber + inventoryWidth * i + j].item.itemName = "";
				items[slot.itemStartNumber + inventoryWidth * i + j].itemImage.color = Color.white;
				items[slot.itemStartNumber + inventoryWidth * i + j].unidentfied.gameObject.SetActive(false);
				items[slot.itemStartNumber + inventoryWidth * i + j].GetComponent<Image>().color = Color.white;
				items[slot.itemStartNumber + inventoryWidth * i + j].itemImage.gameObject.SetActive(false);
				items[slot.itemStartNumber + inventoryWidth * i + j].itemFrame.gameObject.SetActive(false);
				items[slot.itemStartNumber + inventoryWidth * i + j].stackSizeText.gameObject.SetActive(false);
				items[slot.itemStartNumber + inventoryWidth * i + j].GetComponent<CanvasGroup>().blocksRaycasts = true;
			}
		}
		//Reset the item start numbers
		ResetItemStartNumbers();
		//Remove Set if have
		
	}

	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="itemNames"></param>
	public void RemoveItemByName(string itemNames)
	{
		for (int i = 0;i < items.Count; i++)
		{
			if (items[i].item.itemName == itemNames)
			{
				RemoveItemFromSlot(items[i]);
			}
		}
	}

	//Stoppes drag
	public void StopDragging() {
		dragging = false;
		draggedItem = new Items();
		draggedItem.itemName = "";
		dragItem.gameObject.SetActive(false);
		dragItemBackground.gameObject.SetActive(false);
	}

	public void SetSlotImageSprite(InventorySlot slot, Sprite sprite) {
		//Change Icon
		for(int i=0;i<ItemDatabase.instance.SpriteList.Count;i++){
			if(ItemDatabase.instance.SpriteList[i].name == slot.item.iconName){
				slot.itemImage.sprite = Sprite.Create(ItemDatabase.instance.SpriteList[i],new Rect(0,0,ItemDatabase.instance.SpriteList[i].width,ItemDatabase.instance.SpriteList[i].height),Vector2.zero);
			}
		}
		// slot.itemImage.sprite = slot.item.icon;
		slot.itemImage.rectTransform.sizeDelta = new Vector2(slot.item.width * slotIconSize , slot.item.height * slotIconSize);
		
		slot.itemImage.gameObject.SetActive(true);
	}
	
	public void SetSlotImageSprite(InventorySlot slot, string sprite)
	{
		slot.itemImage.sprite.name = sprite;
		slot.itemImage.rectTransform.sizeDelta = new Vector2(slot.item.width * slotIconSize , slot.item.height * slotIconSize);
		slot.itemImage.gameObject.SetActive(true);
	}
	
	

	public void SetSlotImageSprite(EquipmentSlot slot, Sprite sprite)
	{
		slot.itemIcon.sprite = slot.item.icon;
		slot.itemIcon.rectTransform.sizeDelta = new Vector2(slot.item.width * slotIconSize * slot.iconSclarFactor, slot.item.height * slotIconSize * slot.iconSclarFactor);
		slot.itemIcon.gameObject.SetActive(true);
	}
	
	
	//Check to see if an item can fit in the slot
	public bool CheckItemFit(Items item, InventorySlot slot, bool skipLastCheck) {
		for (int i = 0; i <item.height ; i++)
        {
            for (int j = 0; j < item.width; j++)
            {
                if (slot.itemStartNumber + inventoryWidth *i+j 
                >=items.Count)
                {
                    return false;
                }
                //
                for (int k = 0; k< item.height; k++)
                {
                    if (slot.itemStartNumber + inventoryWidth * k + j 
                    != slot.itemStartNumber + inventoryWidth *k)
                    {
                        if (((slot.itemStartNumber +inventoryWidth *i +j) % inventoryWidth ==0) && item.width !=1)
                        {
                            return false;
                        }
                    }
                }
                //
                if (!skipLastCheck)
                {
                    if (items[slot.itemStartNumber + inventoryWidth * i+j].itemStartNumber  != slot.itemStartNumber +inventoryWidth * i+j)
                    {
                        return false;
                    }
                }
                else
                {
                    List<int> counter = new List<int>();
                    for (int l = 0; l < item.height; l++)
                    {
                        for (int m = 0; m < item.width; m++)
                        {
                            if ((slot.itemStartNumber + inventoryWidth * (item.height-1) + (item.width-1)) < items.Count-1 &&
                                items[slot.itemStartNumber+inventoryWidth*l+m].itemStartNumber != slot.itemStartNumber &&
                                items[slot.itemStartNumber + inventoryWidth * l+m].item.itemName !=""&&
                                !counter.Contains(items[slot.itemStartNumber +inventoryWidth * l+m].itemStartNumber))
                            {
                                counter.Add(items[slot.itemStartNumber +inventoryWidth * l +m].itemStartNumber);
                            }
                        }
                    }
                    //
                    if (counter.Count>1)
                    {
                        return false;
                    }else if (counter.Count==1)
                    {
                        return true;
                    }
                }
            }
        }

        return true;
	}

	//Reset the item start number of the empty slots
	public void ResetItemStartNumbers() {
		for(int i = 0; i < items.Count; i++) {
			if(items[i].item.itemName == "") {
				items[i].itemStartNumber = i;
			}
		}
	}

	//Called when the equipment slot is clicked
	public void OnEquipmentSlotClick(GameObject obj, int mouseIndex)
	{
		EquipmentSlot slot = obj.GetComponent<EquipmentSlot>();

		if (mouseIndex == 0)
		{
			if (dragging)
			{
				if (draggedItem.unidentified)
				{
					return;
				}

				if (draggedItem.itemType == slot.equipmentSlotType)
				{
					if(slot.item.itemName=="")
					{
//						UpdateCharacterInfo(slot.item);
						EquipItemAtSlot(slot);
						for (int i = 0; i < equipmentSlots.Count; i++)
						{
							if (slot.item.equipmentSlotype == equipmentSlots[i].equipmentSlotType)
							{
								PlayerPrefs.SetString("EquipID" + i + "_" + equipmentSlots[i].equipmentSlotType,
									slot.item.itemID);
								PlayerPrefs.SetString("EquipSlot" + i, slot.item.itemType.ToString());
							}

//					 (AddItem(slot.item))
						}

						
					
					
					}	else if(slot.item.itemName!="")
				{
					DragSwapEquippedItem(slot);
					UpdateCharacterInfos(slot.item,draggedItem);
					
				}
					
					//Sound
					SoundManager.instance.PlaySound(equipSound);
				
				}else{
					ReturnDraggedItem();
				}
			}
			
		}
		else
		{
           SoundManager.instance.PlaySound(equipSound);
			if (!identifying && !dragging)
			{
				int count = 0;
				//UnEquip Relative
				for (int i = 0; i < equipmentSlots.Count; i++)
				{
					if (AddItem(slot.item))
					{
						//CheckSet
						if (slot.item.setName == setName)
						{
							//
							setNum--;
							RemoveSet();
						}
						UnEquipUpdate(slot.item);
						RemoveEquippedItem(slot);
						if (equipmentSlots[i].item.setName != setName)
						{
							count++;
							if (count == 0)
							{
								hasSet = false;
								player.hasSet = false;
							}
						}

					}
				}
			}

		}
	}

	

	//Equip an item at a specific slot
	public void EquipItemAtSlot(EquipmentSlot slot) {
		//Make a copy of the dragged item
		slot.item = DeepCopy(draggedItem);
		UpdateCharacterInfo(slot.item);
		SetSlotImageSprite(slot, draggedItem.icon);
		slot.transform.Find("ItemBackground").gameObject.SetActive(false);
		StopDragging();
		//show tooltip if player enter die object
		StartCoroutine(tooltip.ShowTooltip(true, slot.item, 
		SlotType.Equipment,0, slot.GetComponent<RectTransform>(), false));

//Check to see if the item equipped is of type offhand
//		if(slot.item.itemType == EquipmentSlotType.offHand) {
//			for(int i = 0; i < equipmentSlots.Count; i++) {
//				//If the player has a two-handed weapon equipped and the player can't equip both a shield and a two handed weapon
//				//Then add the weapon to the inventory
//				if(equipmentSlots[i].equipmentSlotType == EquipmentSlotType.weapon ) {
//					if(equipmentSlots[i].item.twoHanded) {
//						AddItem(equipmentSlots[i].item);
//						RemoveEquippedItem(equipmentSlots[i]);
//					}
//				}
//			}
//		}
//		//Check to see if the item equipped is of type two-handed weapon
//		if(slot.item.itemType == EquipmentSlotType.weapon && slot.item.twoHanded) {
//			for(int i = 0; i < equipmentSlots.Count; i++) {
//				//If the player has a shield equipped and can't equip both a two-handed weapon and a shield
//				//Then add the shield to the inventory
//				if(equipmentSlots[i].equipmentSlotType == EquipmentSlotType.offHand ) {
//					if(equipmentSlots[i].item.itemType == EquipmentSlotType.offHand) {
//						AddItem(equipmentSlots[i].item);
//						RemoveEquippedItem(equipmentSlots[i]);
//					}
//				}
//			}
//		}//		
	}
	
	

	//Swap the dragged item with the item in the equipment slot
	public void DragSwapEquippedItem(EquipmentSlot slot) {
		Items item = DeepCopy(slot.item);
		slot.item = DeepCopy(draggedItem);
		SetSlotImageSprite(slot, slot.item.icon);
		draggedItem = item;
		dragItem.sprite = item.icon;
		dragItem.rectTransform.sizeDelta = new Vector2(item.width * slotIconSize, item.height * slotIconSize);
		
		//Check to see if the item equipped is of type offhand
//		if(slot.item.itemType == EquipmentSlotType.offHand) {
//			for(int i = 0; i < equipmentSlots.Count; i++) {
//				//If the player has a two-handed weapon equipped and the player can't equip both a shield and a two handed weapon
//				//Then add the weapon to the inventory
//				if(equipmentSlots[i].equipmentSlotType 
//				== EquipmentSlotType.weapon 
//				) {
//					if(equipmentSlots[i].item.twoHanded) {
//						AddItem(equipmentSlots[i].item);
//						RemoveEquippedItem(equipmentSlots[i]);
//					}
//				}
//			}
//		}
		//Check to see if the item equipped is of type two-handed weapon
//		if(slot.item.itemType == EquipmentSlotType.weapon && slot.item.twoHanded) {
//			for(int i = 0; i < equipmentSlots.Count; i++) {
//				//If the player has a shield equipped and can't equip both a two-handed weapon and a shield
//				//Then add the shield to the inventory
//				if(equipmentSlots[i].equipmentSlotType == EquipmentSlotType.offHand ) {
//					if(equipmentSlots[i].item.itemType == EquipmentSlotType.offHand) {
//						AddItem(equipmentSlots[i].item);
//						RemoveEquippedItem(equipmentSlots[i]);
//					}
//				}
//			}
//		}
	}

	//Drag an equipped item from the equipment slot
	public void DragEquippedItem(EquipmentSlot slot) {
		//Make a copy of the item in the equipment slot and assign it to the item that's being dragged
		draggedItem = DeepCopy(slot.item);
		dragItem.sprite = slot.item.icon;
		dragItem.rectTransform.sizeDelta = new Vector2(slot.item.width * slotIconSize, slot.item.height * slotIconSize);
		dragItem.gameObject.SetActive(true);
		dragging = true;
		slot.transform.Find("ItemBackground").gameObject.SetActive(true);
	}

	//Removes an item from the equipment slot
	public void RemoveEquippedItem(EquipmentSlot slot) {
		slot.item = new Items();
		slot.item.itemName = "";
		slot.itemIcon.gameObject.SetActive(false);
		//remove key
		for (int i = 0; i < equipmentSlots.Count; i++)
		{
			if (PlayerPrefs.HasKey("EquipID" + i+"_"+equipmentSlots[i].equipmentSlotType))
			{
				PlayerPrefs.DeleteKey("EquipID" + i+"_"+equipmentSlots[i].equipmentSlotType);
				PlayerPrefs.DeleteKey("EquipSlot" + i);
			}
		}

		tooltip.HideTooltip();
		slot.transform.Find("ItemBackground").gameObject.SetActive(true);
		//
		

	

	}

	//Slot Click Need To Do
	//Slot==1
	//Equip
	//Value Change happen::(Equip,UnEquip,Swap,Dragin,DragUp)
	//Swap Equipment 
	//Slot==0
	//Drag Item
	//Drag To Equipment Or Swap Item
	//Drag To Shop them Sell
	
	public void OnSlotClick(GameObject obj, int mouseIndex) {
		 InventorySlot slot = obj.GetComponent<InventorySlot>();
        //Index==0 => Click 
        //Drag Item or swap item
       if(mouseIndex==0){
           if(slot.item.itemName!=""){
               //when the shop is open TODO 
               if(merchant.draggedItem){
                   if(slot.item.itemName == merchant.draggedItem.item.itemName){
	                   SoundManager.instance.PlaySound(dragSound);
                       //if item have stacksize
                       if(slot.item.stackSize < slot.item.maxStackSize){
                            slot.item.stackSize+= merchant.draggedItem.item.stackSize;
                            slot.stackSizeText.text+= slot.item.stackSize.ToString();
                            merchant.dragging=false;
                            StopDragging();
                            return;
                       }
                   }else{
                       return;
                   }

               }
               //::REMOVE FEATURE::
               if(identifying)
               {
	               identifyObj.gameObject.SetActive(true);
                    if(slot.item.unidentified){
                        //IdentifyItem(slot);
                    }
               }
               else if(dragging)
               {

	               SoundManager.instance.PlaySound(dragSound);
                   if(draggedItem.itemName == slot.item.itemName && slot.item.stackable){
                       slot.item.stackSize += draggedItem.stackSize;
                       if(slot.item.stackSize > slot.item.maxStackSize){
                           int tempStack = slot.item.maxStackSize - slot.item.stackSize;
                           slot.item.stackSize = slot.item.maxStackSize;
                           draggedItem.stackSize = Mathf.Abs(tempStack);
                           slot.stackSizeText.text = slot.item.stackSize.ToString();
                           
                           return;
                       }else{
                           slot.stackSizeText.text = slot.item.stackSize.ToString();
                           StopDragging();
                           return;
                       }
                   }

                   //
                   int counter =0;
                    InventorySlot foundSlot =null;
                    int itemStartNumber = Mathf.RoundToInt(Mathf.Infinity);
                    for(int i=0;i<draggedItem.height;i++){
                        for(int j=0;j<draggedItem.width;j++){
                            if(items[slot.itemStartNumber+inventoryWidth * i+j].item.itemName!="" && 
                            itemStartNumber!= items[slot.itemStartNumber *i+j].itemStartNumber)
                            {
                                itemStartNumber = items[slot.itemStartNumber *i +j].itemStartNumber;
                                counter++;
                                foundSlot = items[slot.itemStartNumber * inventoryWidth *i +j];
                            }
                        }
                    }
                    //
                    if(counter ==1){
                        if(SwapItems(foundSlot)){
                            OnMouseEnter(slot.gameObject);
                        }
                    }
               }
               else{
                   if(DragItemFromSlot(slot) ){
                       RemoveItemFromSlot(slot);
                       tooltip.HideTooltip();
                   }
               }
           }else{
               if(merchant.dragging){
                   if(CheckItemFit(merchant.draggedItem.item,slot,false) &&PlayerData.localPlayer.money >= draggedItem.buyPrice){
                        player.money -= merchant.draggedItem.item.buyPrice;
                        merchant.avaliableGoldText.text=player.money.ToString();
                        //success add slot from merchant slot
                        if(merchant.draggedItem.item.stackable){
                            AddItemAtSlot(merchant.draggedItem.item,slot);
							

                        }else{
                            AddItemAtSlot(merchant.draggedItem.item,slot);
                            merchant.RemoveItem(merchant.draggedItem.item,merchant.draggedItem);
                        }
                        //
                        merchant.dragging=false;
                        merchant.draggedItem=null;
                        StopDragging();
                        merchant.ResetTabs();
                        
                   }
               }else if(dragging){
				  //drag to equipmentslot
                    if(AddItemAtSlot(draggedItem,slot)  ){
                        StopDragging();
						UpdateCharacterInfo(draggedItem);
                       ////statPanel.UpdateStateValue();
                        OnMouseEnter(slot.gameObject);
                    
				   }
               }else{
				   ReturnDraggedItem();
               }
           }

       }
            //mouseIndex==1 =>BUY :: merchant
            //INVENTORY->EQUIP ITEM
            //EQUIPMENT->UNEQUIP ITEM
        else if(mouseIndex==1){
            if(identifying || slot.item.unidentified){
                return;
            }
            //
            if(slot.item.itemName != ""){
                if(Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.RightShift))){
                    if(slot.item.stackSize <=1){
                        return;
                    }
                    //
                    itemToSplit=slot;
                    splitWindow.SetActive(true);
                    // splitWindow.GetComponent<SplitWindow>().Start();
                }

                //Inventory Equip or unequip
                else if (!merchant.showMerchant)
                {
	                //Potion 使用药水
//	                if (slot.item.itemType == EquipmentSlotType.consumable)
//	                {
//		                SoundManager.instance.PlayClipAtPoint(potionSound, Vector3.zero,
//			                SoundManager.instance.musicVolume,
//			                true);
//		                //relative equip or unequip
//		               
//
//	                }
//	                else
//	                {
//use identify
	                if (slot.item.itemName == "鉴定卷轴")
	                {
		                UseItem(slot);
	                }

					// if(slot.item.itemType == EquipmentSlotType.reagent && slot.item.covName!=null){
					// 	Debug.Log("Start Conversation"+slot.item.covName.ToString());
					// 	DialogueManager.StartConversation(slot.item.covName);
					// }
		                //Equip Items
		                if (RequiredEquip(slot.item))
		                {
			                for (int i = 0; i < equipmentSlots.Count; i++)
			                {
				                if (equipmentSlots[i].equipmentSlotType == slot.item.itemType)
				                {

					                if (equipmentSlots[i].item.itemName == "")
					                {
						                //relative equip or unequip
						                EquipItemAtSlot(equipmentSlots[i], slot.item);
						                //Save by
						                PlayerPrefs.SetString("EquipID" + i + "_" + equipmentSlots[i].equipmentSlotType,
							                slot.item.itemID);
						                PlayerPrefs.SetString("EquipSlot" + i, slot.item.itemType.ToString());
										
						                //
						                if (equipmentSlots[i].item.stackable==true)
						                {
							                equipmentSlots[i].item.stackSize = slot.item.stackSize;
							                PlayerPrefs.SetInt("EquipPotion"+i+"_"+"Count",slot.item.stackSize);
						                }
						                //Delete Inventoty Ps ID TODO
//								      if(PlayerPrefs.HasKey("ItemID_"+i))
//								      {
//									      PlayerPrefs.DeleteKey("ItemID_"+i);
//								      }
						                //Update Character INfo
						                UpdateCharacterInfo(equipmentSlots[i].item);
						                //////statPanel.UpdateStateValue();
						                RemoveItemFromSlot(slot);
					                }
					                //Swap Item Between EquipmentSlot und inventorySlot
					                else if (equipmentSlots[i].item.itemName != "")
					                {
						                SwapInvItemEquipped(equipmentSlots[i], slot);
						                UpdateSwapCharacterInfo(equipmentSlots[i].item, slot.item);
						                //////statPanel.UpdateStateValue();
						                return;
					                }
					                else
					                {
						                SwapInvItemEquipped(equipmentSlots[i], slot);
						                UpdateSwapCharacterInfo(equipmentSlots[i].item, slot.item);
						                //////statPanel.UpdateStateValue();
						                return;
					                }

//				                }
				                }
			                }
			                //Include Armor und Ring
//                    else if(slot.item.itemType ==EquipmentSlotType.armor)
//                    {
//						EquipItem(slot);
//	                    UpdateCharacterInfo(slot.item);
//                        RemoveItemFromSlot(slot);
//                    }else if(slot.item.itemType ==EquipmentSlotType.ring) {
//						EquipRing(slot);
//							UpdateCharacterInfo(slot.item);
//                        RemoveItemFromSlot(slot);
//						
//					}
							//Sound
							SoundManager.instance.PlaySound(equipSound);

		                }
		                else
		                {
			                //repair Page player Can Upgrade or repair Item need Pay materials or special  
			                if (merchant.repair.activeSelf)
			                {
				                //Repair Module TODO
				                // if(slot.item.curDurability != slot.item.maxDurability){
				                //     merchant.itemToRepair = items[slot.itemStartNumber].item;
				                //     merchant.itemToRepaiIcon.sprite=slot.item.icon;
				                //     merchant.itemToRepairIcon.gameObject.SetActive(true);
				                //     merchant.itemToRepair
				                // }
			                }
			                else
			                {
				                Debug.Log("CAN'T EQUIP");
			                }

		                }
	                
                }
            }else{
//				if(merchant.isActiveAndEnabled){
//                SellItem(slot);
//				}
	            //Do nothing 
            }
        }
	}


	#region UpdateCharacter Info
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="item"></param>
	public void UpdateCharacterInfo(Items item)
	{
		 player = FindObjectOfType<PlayerData>();
		if (item != null && player != null)
		{
			//Check Data
			//if (item.strength != 0)
			//{
			//	player.Strength +=Mathf.FloorToInt(item.strength);
			//}

			//if (item.dexterity != 0)
			//{
			//	player.Dex += Mathf.FloorToInt(item.dexterity);
			//}

			//if (item.magic != 0)
			//{
			//	player.Magic += Mathf.FloorToInt(item.magic);
			//}
			
			//Resistance
			if (item.fireResistance != 0)
			{
				player.FR += item.fireResistance;
			}

			if (item.iceResistance != 0)
			{
				player.IR += item.iceResistance;
			}

			if (item.electronicResistance != 0)
			{
				player.ER += item.electronicResistance;
			}

			if (item.posionResistance != 0)
			{
				player.PR += item.posionResistance ;
			}
			
			//Item
			if (item.damage != 0.0)
			{
				player.atk += Mathf.FloorToInt(item.damage);
			}

			if (item.weaponDur != 0)
			{
				player.atkCount +=Mathf.FloorToInt(item.weaponDur);
			}


			if (item.armor != 0.0)
			{
				player.ArmorDef += Mathf.FloorToInt(item.armor);
			}

			if (item.armorDur != 0)
			{
				player.ArmorDur += Mathf.FloorToInt(item.armorDur);
			}

			//当属性点为3的倍数作为额外增长点数, 
			//概率判定为基础值+(属性有效点) eg.flash = 0.3 + ((dex)4/3 ==1 /100)
//			if(player.Dex %3==0){
//				player.ArmorDef += player.Dex / 3;
//				player.extraFlash = 0.3f+(player.Dex/3.0f/100.0f);
//			}
//
//			if (player.Strength %3== 0)
//			{
//				player.playerHealth += player.Strength / 3;
//				player.atk += player.Strength / 3;
//
//			}
//
//			if (player.Magic %3== 0)
//			{
//				player.extraSpellDamage = 1;
//				player.ESDPerc = 0.4f + (player.Magic / 3.0f / 100.0f);
//			}


		}
	}

	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="item"></param>
	public void UnEquipUpdate(Items item)
	{
		player = FindObjectOfType<PlayerData>();
		//
		if (player != null)
		{
			//player.Strength -= Mathf.FloorToInt(item.strength);
			//player.Dex -= Mathf.FloorToInt(item.dexterity);
			//player.Magic -= Mathf.FloorToInt(item.magic);
			////
			player.FR -= item.fireResistance;
			player.IR -= item.iceResistance;
			player.PR -= item.posionResistance;
			player.ER -= item.electronicResistance;
			
			//
			player.atk -=Mathf.FloorToInt(item.damage);
			player.atkCount -= item.weaponDur;
			//
			player.ArmorDef -= Mathf.FloorToInt(item.armor);
			player.ArmorDur -= Mathf.FloorToInt(item.armorDur);
			//
//			if (player.Strength  %3==  0)
//			{
//				int oldS= player.Strength;
//
//				player.atk -= Mathf.FloorToInt(player.Strength/3);
//				player.playerHealth -=Mathf.FloorToInt(player.Strength/3);
//			}
//			if (player.Dex  %3 == 0)
//			{
//				//new bouns
//				int na =player.ArmorDef+ Mathf.FloorToInt( player.Dex / 3);
//				float nfp =0.3f+ Mathf.FloorToInt(player.Dex / 3/100);
//				
//				player.ArmorDef = na;
//
//				player.extraFlash =nfp;
//
//				if (player.Dex < 3)
//				{
//					player.extraFlash = 0f;
//				}
//			}
//			if (player.Magic  %3 == 0)
//			{
//				float nesperc = 0.4f + Mathf.FloorToInt(player.Magic / 3 / 100);
//
//				player.ESDPerc = nesperc;
//			}
//			if(player.Magic<3){
//				player.extraSpellDamage=0;
//			}
			
			//
		
			
			//
			//if dex!= n/3==0 extraFlash 
			
			
			
			


		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="nItem"></param>
	/// <param name="oItem"></param>
	void UpdateSwapCharacterInfo(Items nItem, Items oItem)
	{
		Items tmp = new Items();
		player = FindObjectOfType<PlayerData>();
		if (player != null && nItem != null && oItem != null)
		{
			tmp = nItem;
			nItem = oItem;
			oItem = tmp;
			
		}
		
		UpdateCharacterInfos(oItem,nItem);

	}
	

	/// <summary>
	/// 
	/// </summary>
	/// <param name="oItem"></param>
	/// <param name="nItem"></param>
	void UpdateCharacterInfos(Items oItem, Items nItem)
	{
		player = FindObjectOfType<PlayerData>();
		if (player != null && oItem != null && nItem != null)
		{
			
				//player.Dex = Mathf.RoundToInt(player.Dex - nItem.dexterity + oItem.dexterity);
				//player.Strength = Mathf.RoundToInt(player.Strength - nItem.strength + oItem.strength);
				//player.Magic = Mathf.RoundToInt(player.Magic - nItem.magic + oItem.magic);
				////
				player.FR = Mathf.RoundToInt(player.FR );
				player.IR = Mathf.RoundToInt(player.IR );
				player.PR = Mathf.RoundToInt(player.PR );
				//player.PhyR = Mathf.RoundToInt(player.PhyR - nItem.phyicsResistance + oItem.phyicsResistance);
				
				//
				player.atk = Mathf.RoundToInt(player.atk - nItem.damage + oItem.damage);
				player.atkCount = Mathf.RoundToInt(player.atkCount - nItem.weaponDur + oItem.weaponDur);
				player.ArmorDef = Mathf.RoundToInt(player.ArmorDef - nItem.armor + oItem.armor);
				player.ArmorDur = Mathf.RoundToInt(player.ArmorDef - nItem.armorDur + oItem.armorDur);
				
		}
	}




	#endregion

	
	
	/// <summary>
	/// Limit Check If Exists TODO
	/// </summary>
	/// <param name="item"></param>
	/// <returns></returns>
	private bool RequiredEquip(Items item)
	{
		player = FindObjectOfType<PlayerData>();
       return player.PlayerLevel >= item.itemLevel ;
    }

    //Sell an item from the inventory
    public void SellItem(InventorySlot slot) {
		//Change the cursor
		//CursorManager.ChangeCursor("Default");
		//Hide the tooltip
		tooltip.HideTooltip();
		//Add the sell price to the player
		player.money += slot.item.sellPrice;
		//Update the avaliable gold text
		merchant.avaliableGoldText.text = player.money.ToString();
		//
		SoundManager.instance.PlaySound(SellSound);
		//Find the buyback tab and add the item to it
		//If the buyback tab isn't already there then instantiate it
		for(int i = 0; i < merchant.selectedMerchant.tabs.Count; i++) {
			if(merchant.selectedMerchant.tabs[i].tabType == TabType.buyBack) {
				if(merchant.selectedMerchant.tabs[i].items.Count == 0) {
					for(int j = 0; j < merchant.tabs.Count; j++) {
						if(merchant.tabs[j].tabType == TabType.buyBack) {
							break;
						}
						if(j == merchant.tabs.Count - 1) {
							GameObject tempTab = Instantiate(merchant.tabPrefab) as GameObject;
							tempTab.transform.SetParent(merchant.tabsObj.transform);
							if(!merchant.selectedMerchant.canRepair) {
								tempTab.transform.SetSiblingIndex(merchant.tabs.Count);
							}
							else {
								tempTab.transform.SetSiblingIndex(merchant.tabs.Count - 1);
							}
							tempTab.transform.localScale = Vector3.one;
							tempTab.GetComponent<Image>().color = merchant.tabInactiveColor;
							MerchantTabs tab = tempTab.AddComponent<MerchantTabs>();
							tab.tabType = TabType.buyBack;
							tab.items = new List<Items>();
							tempTab.GetComponent<Image>().sprite = buyBackSprite;
							merchant.tabs.Add(tab);
							merchant.tabsObj.GetComponent<RectTransform>().sizeDelta = new Vector2(merchant.tabWidth, merchant.tabHeight * merchant.tabs.Count);
						}
					}
				}
			}
		}

		for(int i = 0; i < merchant.selectedMerchant.tabs.Count; i++) {
			if(merchant.selectedMerchant.tabs[i].tabType == TabType.buyBack) {
				Items item = DeepCopy(slot.item);
				item.buyPrice = item.sellPrice;
				merchant.selectedMerchant.tabs[i].items.Add(item);
			}
		}
		//Remove the sold item
		RemoveItemFromSlot(slot);
		//Reset the tabs of the merchant
		merchant.ResetTabs();
	}

	//Use an item when right clicked
	public void UseItem(InventorySlot slot) {
		//Create a new game object, add the use effect script to it and then execute the script
		//When done remove the created item
		GameObject obj = new GameObject("Use Effect");
		UseEffect effect = obj.AddComponent(System.Type.GetType(slot.item.useEffectScriptName)) as UseEffect;
		effect.item = slot.item;
		string message = effect.Use();
		//If the message returned is of type potion then remove one from the stacksize of the potion
		if(message == "Potion") {
			items[slot.itemStartNumber].item.stackSize--;
			items[slot.itemStartNumber].stackSizeText.text = items[slot.itemStartNumber].item.stackSize.ToString();
			// transform.root.GetComponent<AudioSource>().PlayOneShot(slot.item.useSound);
			//remove the item if the stacksize is zero
			if(items[slot.itemStartNumber].item.stackSize == 0) {
				RemoveItemFromSlot(items[slot.itemStartNumber]);
			}
		}
		//If the returned message is of type Identify then start identifying
		else if(message == "Identify") {
			identifyingScrollOrignalSlot = slot;
			identifyObj.gameObject.SetActive(true);
		}
		//If the returned message isn't one of the above consider it a warning and display the warning
		else {
			messageManager.AddMessage(Color.red,message);
		}
		//Destroy the Use Effect game object after 1 second
		DestroyObject(obj, 1);
	}

	//Identify the item
//	public void IdentifyItem(InventorySlot slot) {
		
		
////		RemoveItemFromSlot(slot);
//		//Generate Items
//	   Items newItems=ItemDatabase.instance.GenerateItem(slot.item,false,true);
//	   		RemoveItemFromSlot(slot);
//		slot.item =newItems;
//		slot.item.icon = newItems.icon;
//		//
//		if (AddItem(DeepCopy(slot.item)))
//		{
//			ItemDatabase.instance.FindItem(int.Parse(slot.item.itemID));
//		}
		
//		//Run through all the slots that the item fills and remove the unidentified flag from them
//		for(int i = 0; i < slot.item.height; i++) {
//			for(int j = 0; j < slot.item.width; j++) {
//				items[slot.itemStartNumber + inventoryWidth * i + j].item.unidentified = false;
//			}
//		}
//		//hide identify icon
//		items[slot.itemStartNumber].itemImage.color = Color.white;
//		items[slot.itemStartNumber].unidentfied.gameObject.SetActive(false);
//		//Generate Items
////		ItemDatabase.instance.GenerateItem(slot.item,false,true);
//		//
//		identifying = false;
////		identifyingScrollOrignalSlot.item.stackSize--;
////		identifyingScrollOrignalSlot.stackSizeText.text = identifyingScrollOrignalSlot.item.stackSize.ToString();
//		//CursorManager.ChangeCursor("Default");
//		RemoveItemFromSlot(identifyingScrollOrignalSlot);
		
		
//		//
//		identifyObj.gameObject.SetActive(false);
//		//updates the tooltip
//		OnMouseEnter(slot.gameObject);
//		//
//		Canvas.ForceUpdateCanvases();
//	}

	//Called when the mouse enters a slot
	public void OnMouseEnter(GameObject obj) {

		Items item = new Items();

		//If the entered slot is of type inventory slot
		if(obj.GetComponent<InventorySlot>()) {
			item = obj.GetComponent<InventorySlot>().item;
			for(int i = 0; i < items.Count; i++) {
				//If the tooltip isn't already showing the show it
				if(!tooltip.showTooltip) {
					StartCoroutine(
						tooltip.ShowTooltip
						(true, item, SlotType.Inventory,
						 obj.GetComponent<InventorySlot>().itemStartNumber, 
						 obj.GetComponent<RectTransform>(), false));
				}
			}
		}
		//Else if the entered slot is of type equipment slot
		else if(obj.GetComponent<EquipmentSlot>()) {
			item = obj.GetComponent<EquipmentSlot>().item;
		}

		//If there's nothing in the slot or the player is dragging then return
		if(item.itemName == "" || dragging) {
			return;
		}

		//If the player isn't not identifying and the merchant is active then show the sell cursor
		// if(!identifying && merchant.showMerchant) {
		// 	//CursorManager.ChangeCursor("SellCursor");
		// }

		//if the entered slot is of type equipment slot
		if(obj.GetComponent<EquipmentSlot>()) {
			StartCoroutine(tooltip.ShowTooltip(true, item, SlotType.Equipment, 0, obj.GetComponent<RectTransform>(), false));
		}
	}

	//Called when the mouse leaves a slot
	public void OnMouseExit(GameObject obj) {
		//Hide the tooltip
		tooltip.HideTooltip();

		//If the player is not identifying then show the normal cursor
		if(!identifying) {
			//CursorManager.ChangeCursor("Default");
		}
	}

	//Find the color based on the rarity of the item
	public Color FindColor(Items item) {
		if(item.itemRatity == ItemRatity.Junk) {
			return junkColor;
		}
		else if(item.itemRatity == ItemRatity.Legendary) {
			return legendaryColor;
		}
		else if(item.itemRatity == ItemRatity.Ancient) {
			return magicColor;
		}
		else if(item.itemRatity == ItemRatity.Normal) {
			return normalColor;
		}
		else if(item.itemRatity == ItemRatity.Rare) {
			return rareColor;
		}
		
		return Color.clear;
	}

	//Open or close the inventory
	public void OpenCloseInventory(bool state) {
//		for(int i = 0; i < transform.childCount; i++) {
//			transform.GetChild(i).gameObject.SetActive(state);
//		}
panel.Open();
	}

	//Creates a complete copy of an item
	public static Items DeepCopy(Items obj) {
		// GameObject oj = obj.worldObject;
		Sprite tempTex = obj.icon;
		obj.icon = null;
		
		if(obj==null)
			throw new ArgumentNullException("Object cannot be null");
		Items i = (Items)Process(obj);
		// i.worldObject = oj;
		i.icon = tempTex;
		// obj.worldObject = oj;
		obj.icon = tempTex;
		
		return i;
	}
	
	static object Process(object obj) {
		if(obj==null)
			return null;
		Type type=obj.GetType();
		if(type.IsValueType || type==typeof(string)) {
			return obj;
		}
		else if(type.IsArray) {
			Type elementType=Type.GetType(
				type.FullName.Replace("[]",string.Empty));
		DestroyImmediate(GameObject.Find("New Game Object"));
			var array=obj as Array;
			Array copied=Array.CreateInstance(elementType,array.Length);
			for(int i=0; i<array.Length; i++) {
				copied.SetValue(Process(array.GetValue(i)),i);
			}
			return Convert.ChangeType(copied,obj.GetType());
		}
		else if(type.IsClass) {
			object toret=Activator.CreateInstance(obj.GetType());
			FieldInfo[] fields=type.GetFields(BindingFlags.Public| 
			                                  BindingFlags.NonPublic|BindingFlags.Instance);
			foreach(FieldInfo field in fields) {
				object fieldValue=field.GetValue(obj);
				if(fieldValue==null)
					continue;
				field.SetValue(toret, Process(fieldValue));
			}
			return toret;
		}
		else
			throw new ArgumentException("Unknown type");
	}

}
