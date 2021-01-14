using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using TMPro;
using System.Linq;
using Mirror;

[System.Serializable]
public class CraftedItem {

    public Items item;
    public List<Items> materials;
    public List<int> materialIDs;
    public List<int> materialRequiredAmount;
	public Texture2D cIcon;
	///
	public int cItemCost;
	public float craftPerc;
	public bool canSplit;
	public int splitDust;
	
    public CraftButton button;
    public string cItemID;
    public CraftingTabType baseType;
    public float craftTimer;
    public float craftTime;

	//Save playerprefs
	public bool hasLearn  { get; set; }	
    public CraftedItem (){}

    public CraftedItem(Items item, List<Items> materials, List<int> materialIDs, List<int> materialRequiredAmount, Texture2D cIcon, int cItemCost, float craftPerc, bool canSplit, int splitDust, CraftButton button, string cItemID, CraftingTabType baseType, float craftTimer, float craftTime)
    {
        this.item = item;
        this.materials = materials;
        this.materialIDs = materialIDs;
        this.materialRequiredAmount = materialRequiredAmount;
        this.cIcon = cIcon;
        this.cItemCost = cItemCost;
        this.craftPerc = craftPerc;
        this.canSplit = canSplit;
        this.splitDust = splitDust;
        this.button = button;
        this.cItemID = cItemID;
        this.baseType = baseType;
        this.craftTimer = craftTimer;
        this.craftTime = craftTime;
    }
}
public class CraftManager : MonoBehaviour
{
	public static CraftManager instance;
   	
	

	[HideInInspector]
	public InventorySystem inventory;
	
	public CraftedItem selectedItem;
	
	public int amountToCraft = 1;

	public List<GameObject> materials;

	public CraftTab selectedTab;


	public float materialIconSlotSize;
	public float iconSlotSize;
	// public float tabWidth;
	// public float tabHeight;

	public Color tabInactiveColor;
	public Color tabActiveColor;
	public List<Texture2D> idb;
	public ItemDatabase database;
	public Animator animator;
	
	// public GameObject tabPrefab;
	public GameObject tabsObj;
	[Header("Materials")]
	public GameObject materialPrefab;
	public Transform materialsPos;
	[Header("Tabs Item List")]
	public GameObject itemListPos;
	public GameObject craftingItemPrefab;
	public List<GameObject> items;
	public Image craftingItemImage;
	//
	public TextMeshProUGUI selectedItemNameText;
	public TextMeshProUGUI craftCostLabel;
	public TextMeshProUGUI craftPercText;
	public TextMeshProUGUI amountToCraftLabel;
	public TextMeshProUGUI craftStateText;
	// public Sprite armorTab;
	// public Sprite weaponTab;	//reagant
	public Transform tabPos;
	public Button acceptButton;
	
	public TooltipManager tooltip;

	private PlayerData player;
	public List<int> materialAmounts;
	public List<GameObject> craftingTabs;
	private bool crafting;

	// Use this for initialization
	void Start () {

		instance=this;
		//Find the player
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
		//Find the inventory
		inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventorySystem>();

		// Texture2D[] imageDB = Resources.LoadAll<Texture2D>("");
		// if(imageDB.Length>0){
		// 		for(int i=0;i<imageDB.Length;i++){
		// 			idb.Add(imageDB[i]);
		// 		}
		// }
		// Debug.Log(idb.Count+"has been add todb");
		selectedItem = new CraftedItem();

		//Init
		selectedItemNameText.text="";
		craftCostLabel.text="";
		craftPercText.text ="0%";
		amountToCraftLabel.text="0";
		craftStateText.text="制作状态";


	
	}

	public void Update() {
		//Open the crafting tab when the player presses O
		
		//If the selected item button is clicked change the selected item
		// if(selectedItem.button) {
		// 	ChangeCraftingItem(selectedItem.button);
		// 	selectedItem.button.UpdateText();
		// }
		// // // If the player have enough money set the accept button to interactable
		// if(selectedItem.cItemCost * amountToCraft <= player.money) {
		// 	// acceptButton.interactable = true;
		// 	craftCostLabel.color = Color.green;
		// }
		// //If the player doesn't have enough money set the accept button to uninteractable
		// else {
		// 	// acceptButton.interactable = false;
		// 	craftCostLabel.color = Color.red;
		// }
		//If the player presses escape then close the crafting window
		// if(Input.GetKeyDown(KeyCode.Escape)) {
		// 	OpenCloseWindow(false);
		// }
	}

	//Add an item to the tabs
	public void AddCraftingItem(int ID) {
		CraftedItem item = database.FindCraftItem(ID);
		if(craftingTabs.Count > 0) {
			int counter = 0;
			for(int j = 0; j < craftingTabs.Count; j++) {
				if(craftingTabs[j].GetComponent<CraftTab>().tabType == item.baseType) {
					if(craftingTabs[j].GetComponent<CraftTab>().items == null) {
						craftingTabs[j].GetComponent<CraftTab>().items = new List<CraftedItem>();
					}
					craftingTabs[j].GetComponent<CraftTab>().items.Add(item);
					counter++;
				}
				if(j == craftingTabs.Count - 1 && counter == 0) {
					GameObject tab = Instantiate(tabsObj) as GameObject;
					// CraftTab tab = tempTab.AddComponent<CraftTab>();
					tab.transform.SetParent(tabsObj.transform);
					tab.GetComponent<RectTransform>().localScale = Vector3.one;
					tab.GetComponent<CraftTab>().tabType = item.baseType;
					// if(tab.GetComponent<CraftTab>().tabType == CraftingTabType.Equipment) {
					// 	tab.GetComponent<Image>().sprite = armorTab;
					// }
					// else if(tab.GetComponent<CraftTab>().tabType == CraftingTabType.Reagent) {
					// 	tab.GetComponent<Image>().sprite = weaponTab;
					// }
					craftingTabs.Add(tab);
					// tabsObj.GetComponent<RectTransform>().sizeDelta = new Vector3(tabWidth, tabHeight * craftingTabs.Count);
				}
			}
		}
	}

	//Change the current selected tab
	public void ChangeCraftingTab(CraftTab tab) {
		//If the tab is already selected just return
		if(selectedTab == tab) {
			return;
		}
		//Set the color of all the tabs to inactive
		// for(int i = 0; i < craftingTabs.Count; i++) {
		// 	craftingTabs[i].GetComponent<Image>().color = tabInactiveColor;
		// }
		//Clear old obj
	

		selectedTab = tab;
		// selectedTab.GetComponent<Image>().color = tabActiveColor;
		//Remove all the tab items
		// while(itemListPos.transform.childCount != 0) {
		// 	DestroyImmediate(itemListPos.transform.GetChild(0).gameObject);
		// }
		foreach(var v in items){
			if(v!=null){
				Destroy(v);
			}
		}


		//Recreate the buttons
		for(int i = 0; i < selectedTab.items.Count; i++) {
			GameObject tItem = Instantiate(craftingItemPrefab,itemListPos.transform.position,Quaternion.identity) as GameObject;
			tItem.transform.SetParent(itemListPos.transform);
			tItem.transform.localScale = Vector3.one;
			tItem.GetComponent<CraftButton>().item = selectedTab.items[i];
		tItem.GetComponent<CraftButton>().text.text = selectedTab.items[i].item.itemName;tItem.GetComponent<CraftButton>().text.text = selectedTab.items[i].item.itemName;
		Debug.Log("TRY GOT Sprite by names");
		tItem.GetComponent<CraftButton>().itemSprite.sprite =Sprite.Create(selectedTab.items[i].cIcon,new Rect(0,0,selectedTab.items[i].cIcon.width,selectedTab.items[i].cIcon.height),Vector2.zero);
			//
			items.Add(tItem);
			NetworkServer.Spawn(tItem);

		
		}
		//Set the selected crafting item to the first instance
		if(itemListPos.transform.GetChild(0).GetComponent<CraftButton>()) {
			ChangeCraftingItem(itemListPos.transform.GetChild(0).GetComponent<CraftButton>());
		}
		//Reset the scrolling position
		// scrollRect.verticalNormalizedPosition = 1f;
	}

	//Change the selected crafting item
	public void ChangeCraftingItem(CraftButton item) {
		//Position the selected highlight on top of the selected item
		// buttonHighlight.transform.position = item.transform.position;
		// while(materialsPos.transform.childCount != 0) {
    	// 	DestroyImmediate(materialsPos.transform.GetChild(0).gameObject);
    	// }
		// foreach(var g in materials){
		// 	if(g!=null){Destroy(g);}
		// }

		Debug.Log("Select Items"+item.name);
		selectedItem = item.item;
		crafting=false;

		//Instantiate all the materials
		materialAmounts = new List<int>();
		
		//create materials items
				// for(int j = 0; j < item.item.materials.Count; j++) {
				// 	GameObject material = Instantiate(materialPrefab,materialsPos.position,Quaternion.identity) as GameObject;
				// 	material.transform.SetParent(materialsPos);
				// 	//Set Dataj
				// 	material.GetComponent<CraftMaterials>().nametext.text = item.item.materials[j].itemName.ToString();
				// 	// material.GetComponent<CraftMaterials>().co.text = item.item.materials[i].itemName.ToString();
					
				// 	// material.GetComponent<CraftMaterials>().current.text = InventorySystem.instance.CheckItemEnough(material.GetComponent<CraftMaterials>())
				// 	// material.GetComponent<CraftMaterials>().needs.text =item.item.materialRequiredAmount[i].ToString();
				// 	materials.Add(material);
				// }
			
	
		// while(materialsPos.transform.childCount != 0) {
		// 	DestroyImmediate(materialsPos.transform.GetChild(0).gameObject);
		// }

		//clear old materials
		

		//
		Debug.Log("Add Materials");
			
				//foreach item mList with id, needs get item by id to get detail then load sprite 
				//TODO 
				for(int i=0;i<item.item.materials.Count;i++){
					if(materials.Count==0){
				for(int j = 0; j < item.item.materials.Count; j++) {
					
					GameObject mObj = Instantiate(materialPrefab,materialsPos.position,Quaternion.identity) as GameObject;
					mObj.transform.SetParent(materialsPos);
					//Set Data TODO1117 Set Data To MObj (string)
					Items gItem = ItemDatabase.instance.FindItem(int.Parse(item.item.materials[j].itemID));
					mObj.GetComponent<CraftMaterials>().nametext.text = item.item.materials[j].itemName.ToString();
						mObj.GetComponent<CraftMaterials>().mItem = gItem;
						//TODO
						mObj.GetComponent<CraftMaterials>().icon.sprite= GotItems(item.item.materials[j].iconName);
						mObj.name = gItem.itemName.ToString();

						//Got requreied
			// 				int count = 0;
			
					// material.GetComponent<CraftMaterials>().current.text = InventorySystem.instance.CheckItemEnough(material.GetComponent<CraftMaterials>());
					// mObj.GetComponent<CraftMaterials>().current.text =item.item.materialRequiredAmount[i].ToString();
					materials.Add(mObj);
				}
				}
				}
			
		
		//
		Debug.Log("Add  materials current");
		if(materials.Count>0){
		//Position all the materials
		bool avaliable = false;
		for(int j = 0; j <materials.Count; j++) {
			if(materials[j]!=null){
				Debug.Log(materials[j].GetComponent<CraftMaterials>().mItem.itemName);
		
				
			materials[j].transform.SetParent(materials[j].transform);
			materials[j].transform.localScale = Vector3.one;
			materials[j].GetComponent<CraftMaterials>().nametext.text =item.item.materials[j].itemName;
			materials[j].GetComponent<CraftMaterials>().icon.sprite =GotItems(item.item.materials[j].iconName);
			materials[j].GetComponent<CraftMaterials>().mItem = item.item.materials[j];
	
			 
			//try got item at inventory 
			int count = 0;
			for(int k = 0; k < inventory.items.Count; k++) {
				if(inventory.items[k].item.itemName == item.item.materials[j].itemName) {
					//current item size in inventory
					count += inventory.items[inventory.items[k].itemStartNumber].item.stackSize;
				}
			}
			//
			if(count < item.item.materialRequiredAmount[j]) {
				materials[j].GetComponent<CraftMaterials>().current.color = Color.red;
				// Stop();
				avaliable = false;
			}
			else {
				materials[j].GetComponent<CraftMaterials>().current.color = Color.green;
				acceptButton.interactable = true;
			}
			if(!avaliable) {
				acceptButton.interactable = false;
			}
			//
			materials[j].GetComponent<CraftMaterials>().current.text = count.ToString()+"/"+item.item.materialRequiredAmount[j];
			materialAmounts.Add(count/item.item.materialRequiredAmount[j]);
			

			}
		}

		}
		
			

		//select detail
		craftingItemImage.sprite =GotItems(selectedItem.item.iconName);
		// craftingItemImage.rectTransform.sizeDelta = new Vector2(item.item.item.width * iconSlotSize, item.item.item.height * iconSlotSize);
		selectedItemNameText.text = item.item.item.itemName;
		craftPercText.text= selectedItem.craftPerc+"%";
		
		craftCostLabel.text = (selectedItem.craftPerc * amountToCraft).ToString();
		// craftCostLabel.rectTransform.sizeDelta = new Vector2(craftCostLabel.preferredWidth, craftCostLabel.rectTransform.sizeDelta.y);
	}



	
	//Stop crafting
	public void Stop() {
		StopCoroutine("CraftItems");
		StopCoroutine("CraftItem");
		// buttonHighlight.fillAmount = 0;
		selectedItem.craftTimer = 0;
		acceptButton.interactable = true;
		crafting = false;
	}

	//Called when the accept button is pressed
	public void Accept() {
		Debug.Log("Start Crafting");
			CraftItems();
		
	}

	//Start crafting the item
	public void CraftItems() {
		int tempAmount = amountToCraft;
		Debug.Log("Craft amount ist"+tempAmount);
		// acceptButton.interactable = false;
		while(tempAmount>0){
			CraftItem();
			tempAmount--;
		}
	}

	//Craft the item over time
	public void CraftItem() {
		// yield return new WaitForEndOfFrame();

		// while(selectedItem.craftTimer < selectedItem.craftTime) {
		// 	yield return new WaitForEndOfFrame();
		// 	selectedItem.craftTimer += Time.deltaTime;
		// 	// buttonHighlight.fillAmount = (selectedItem.craftTimer/selectedItem.craftTime);
		// }

		// selectedItem.craftTimer = 0;
		// buttonHighlight.fillAmount = 0;
		// if(amountToCraft > 0) {
		// 	amountToCraft--;
		// }
		// amountToCraftLabel.text = amountToCraft.ToString();
		// craftCostLabel.text = (selectedItem.craftTime * a+mountToCraft).ToString();
		// craftCostLabel.rectTransform.sizeDelta = new Vector2(craftCostLabel.preferredWidth, craftCostLabel.rectTransform.sizeDelta.y);
		// AddItem();
		// ChangeCraftingItem(selectedItem.button);
		// for(int i = 0; i < selectedTab.items.Count; i++) {
		// 	selectedTab.items[i].button.UpdateText();
		// }
		// acceptButton.interactable = true;
	float rnd= Random.Range(0f,1f);
		if(rnd > (1-selectedItem.item.perc)){
			//success
			Debug.Log(" rnd "+rnd+"Can Craft "+(1-selectedItem.item.perc)+"\t"+selectedItem.item.itemName.ToString());
			MessageManagers.instance.ShowMessageCoroutine("制作成功",1.0f);
		amountToCraftLabel.text = amountToCraft.ToString();
		craftCostLabel.text = (selectedItem.craftTime * amountToCraft).ToString();
		craftCostLabel.rectTransform.sizeDelta = new Vector2(craftCostLabel.preferredWidth, craftCostLabel.rectTransform.sizeDelta.y);
		AddItem();
		//
		ChangeCraftingItem(selectedItem.button);
		Debug.Log("Update materials item count");
		// UpdateText(selectedItem);
		for(int i = 0; i < selectedTab.items.Count; i++) {
			selectedTab.items[i].button.UpdateText();
		}
		craftStateText.text="制作成功";
		}else{

			Debug.Log("rnd"+rnd+"==> craft failed");
			RemoveItem();
			MessageManagers.instance.ShowMessageCoroutine("制作失败",1.0f);
			craftStateText.text="制作失败"; 
			ChangeCraftingItem(selectedItem.button);
		}
		
		amountToCraft=0;
			amountToCraftLabel.text="0";
		acceptButton.interactable = true;
		
	}

	

	//The item was created now add it to the inventory
	public void AddItem() {
		for(int i = 0; i < inventory.items.Count; i++) {

			if(inventory.CheckItemFit(selectedItem.item, inventory.items[i],false)) {
				if(selectedItem.item.stackable==true){
					inventory.AddStackableItem(selectedItem.item);
				}else{
					inventory.AddItem(selectedItem.item);
				}
				//remove materials
				RemoveItem();
				
				crafting = false;
				return;
			}else{
				MessageManagers.instance.ShowMessageCoroutine("背包已满",1.0f);
				// Stop();	//stop craft 
			}
			if(i == inventory.items.Count - 1) {
				//Display message
			}
		}
		crafting = false;
	}

	//The item was crafted now remove the crafting materials from the inventory
	public void RemoveItem() {
		Debug.Log("Remove target materials");
		for(int j = 0; j < selectedItem.materials.Count; j++) {
			int stackToRemove = selectedItem.materialRequiredAmount[j];
			for(int i = 0; i < inventory.items.Count; i++) {
				if(inventory.items[i].item.itemName == selectedItem.materials[j].itemName) {
					inventory.items[i].item.stackSize -= stackToRemove;
					if(inventory.items[i].item.stackSize == 0) {
						inventory.RemoveItemFromSlot(inventory.items[i]);
					}
					else if(inventory.items[i].item.stackSize < 0) {
						stackToRemove = Mathf.Abs(inventory.items[i].item.stackSize);
						inventory.RemoveItemFromSlot(inventory.items[i]);
					}
					else { 
						inventory.items[i].stackSizeText.text = inventory.items[i].item.stackSize.ToString();
						break;
					}
				}
			}
		}
	}

	//Increase the amount int
	public void IncreaseAmountToCraft() {

		int count = 0;
		for(int i = 0; i < materialAmounts.Count; i++) {
			if(materialAmounts[i] < count || (count == 0 && i == 0)) {
				count = materialAmounts[i];
			}
		}

		if(amountToCraft < count) {
			amountToCraft++;
		}

		amountToCraftLabel.text = amountToCraft.ToString();

		craftCostLabel.text = (selectedItem.cItemCost * amountToCraft).ToString();

		craftCostLabel.rectTransform.sizeDelta = new Vector2(craftCostLabel.preferredWidth, craftCostLabel.rectTransform.sizeDelta.y);
		if(amountToCraft>0){
		acceptButton.interactable=true;
		}
	}

	//Descrease the amount int
	public void DescreaseAmountToCraft() {
		if(amountToCraft > 0) {
			amountToCraft--;
		}
		amountToCraftLabel.text = amountToCraft.ToString();
		craftCostLabel.text = (selectedItem.cItemCost * amountToCraft).ToString();

		if(amountToCraft==0){
			acceptButton.interactable=false;
		}
	}

	// Called when the material icon is hovered over
	public void OnMaterialIconEnter(CraftMaterials item) {
		StartCoroutine(tooltip.ShowTooltip(false, item.mItem, SlotType.Crafting, 0, item.GetComponent<RectTransform>(),false));
	}

	// Called when the material icon isn't hovered over anymore
	public void OnMaterialIconExit(CraftMaterials item) {
		tooltip.HideTooltip();
	}

	// //Called when the craft item icon is hovered over
	// public void OnIconEnter(GameObject obj) {
	// 	StartCoroutine(tooltip.ShowTooltip(false, selectedItem.item, SlotType.Crafting, 0, obj.GetComponent<RectTransform>(),false));
	// }

	//Called when the craft item icon isn't hovered over anymore
	// public void OnIconExit() {
	// 	tooltip.HideTooltip();
	// }

	//Open or close the crafting window 
	// player with fixed craft system got item by collect 
	//player in game collect item und craft them, has category change type for items
	// craft always use in town ,can't use at dungeon
	public void OpenCloseWindow(bool state) {
		// foreach(Transform trans in transform) {
		// 	trans.gameObject.SetActive(state);
		// }
		// if(state == true) {
		// 	GameObject.FindGameObjectWithTag("Merchant").GetComponent<Merchant>().OpenCloseMerchant(!state);
		// 	ChangeCraftingItem(selectedTab.items[0].button);
		// }
		 animator.Play("Panel In");
		 LoadCraftItems();
	}

	void LoadCraftItems(){
		
		
		// Run through all the craft item IDs
		//Generate citem 
		for(int i = 0; i < ItemDatabase.instance.craftItems.Count; i++) {

			
			//Find the item in the item database
			CraftedItem item = ItemDatabase.instance.craftItems[i];
			//If there is already any crafting tabs
			if(craftingTabs.Count > 0) {
				int counter = 0;
				//Run through all the crafting tabs
				for(int j = 0; j < craftingTabs.Count; j++) {
					//If the item base type is = to the tabtype of the crafting tab then add the item to the list
					if(craftingTabs[j].GetComponent<CraftTab>().tabType == item.baseType) {
						if(craftingTabs[j].GetComponent<CraftTab>().items == null) {
							craftingTabs[j].GetComponent<CraftTab>().items = new List<CraftedItem>();
						}
						craftingTabs[j].GetComponent<CraftTab>().items.Add(item);
						counter++;
					}
					//If we're at the end of the list and there's no tab with the current item type already
					//Instantiate the tab and add the item to the list of items in the tab
					if(j == craftingTabs.Count - 1 && counter == 0) {
						GameObject tab = Instantiate(tabsObj,tabPos.position,Quaternion.identity) as GameObject;
						tab.transform.SetParent(tabPos);
						// CraftTab tab = tempTab.AddComponent<CraftTab>();
						
						tab.GetComponent<RectTransform>().localScale = Vector3.one;
						tab.GetComponent<CraftTab>().tabType = item.baseType;
						tab.GetComponent<CraftTab>().nameText.text =ConvertTabType(item.baseType);
						// if(tab.GetComponent<CraftTab>().tabType == CraftingTabType.Equipment) {
						// 	tab.GetComponent<Image>().sprite = armorTab;
						// }
						// else if(tab.GetComponent<CraftTab>().tabType == CraftingTabType.Reagent) {
						// 	tab.GetComponent<Image>().sprite = weaponTab;
						// }
						craftingTabs.Add(tab);
						// tabsObj.GetComponent<RectTransform>().sizeDelta = new Vector3(tabWidth, tabHeight * craftingTabs.Count);
						NetworkServer.Spawn(tab);
					}
				}
			}
			//There isn't any tabs
			//Instantiate the tab and add the item
			else {
				//
				Debug.Log("First Load Tabs Load Tbas");
				GameObject tab = Instantiate(tabsObj,tabPos.position,Quaternion.identity) as GameObject;
					
				tab.transform.SetParent(tabPos.transform);
				tab.GetComponent<RectTransform>().localScale = Vector3.one;
				tab.GetComponent<CraftTab>().tabType = item.baseType;
				tab.GetComponent<CraftTab>().nameText.text = ConvertTabType(item.baseType);
				// if(tab.GetComponent<CraftTab>().tabType == CraftingTabType.Equipment) {
				// 	tab.GetComponent<CraftTab>().image.sprite = armorTab;
				// }
				// else if(tab.GetComponent<CraftTab>().tabType == CraftingTabType.Reagent) {
				// 	tab.GetComponent<CraftTab>().image.sprite = weaponTab;
				// }
				craftingTabs.Add(tab);
				craftingTabs[craftingTabs.IndexOf(tab)].GetComponent<CraftTab>().items = new List<CraftedItem>();
				craftingTabs[craftingTabs.IndexOf(tab)].GetComponent<CraftTab>().items.Add(item);
				// tabsObj.GetComponent<RectTransform>().sizeDelta = new Vector3(tabWidth, tabHeight * craftingTabs.Count);
				NetworkServer.Spawn(tab);
				//default
				Debug.Log("First Load default CItems");
				selectedTab =  craftingTabs[craftingTabs.IndexOf(tab)].GetComponent<CraftTab>();
				// Debug.Log("select tab ist"+selectedTab.tabType.ToString());
				// ChangeCraftingTab(selectedTab);

			
			}
		}
		//Set the current selected tab to the first instance of the tabs
		
		//Set the color of all the tabs to inactive
		// for(int i = 0; i < craftingTabs.Count; i++) {
		// 	craftingTabs[i].GetComponent<Image>().color = tabInactiveColor;
		// }
		//Set the selected tabs color to active
		// selectedTab.GetComponent<Image>().color = tabActiveColor;

		//Run through all the items in the selected tab and instantiate them
		Debug.Log("Load Tabs Items");
		for(int i = 0; i < selectedTab.items.Count; i++) {
			GameObject item = Instantiate(craftingItemPrefab,itemListPos.transform.position,Quaternion.identity) as GameObject;
			item.transform.SetParent(itemListPos.transform);
			item.transform.localScale = Vector3.one;
			// selectedTab.items[i].button = item.GetComponent<CraftButton>();
			
			item.GetComponent<CraftButton>().item.item =ItemDatabase.instance.FindItem(int.Parse(selectedTab.items[i].item.itemID));
				 item.GetComponent<CraftButton>().item = selectedTab.items[i] ;
				  item.GetComponent<CraftButton>().text.text =selectedTab.items[i].item.itemName.ToString();
								 item.GetComponent<CraftButton>().itemSprite.sprite = GotItems(selectedTab.items[i].item.iconName);
										 item.GetComponent<CraftButton>().item = selectedTab.items[i] ;
				
			
			NetworkServer.Spawn(item);
			items.Add(item);
			item.transform.name = selectedTab.items[i].ToString();
		}

		// Close the crafting window
		// OpenCloseWindow(false);
	}

	Sprite GotItems(string n){
		for(int i=0;i<ItemDatabase.instance.SpriteList.Count;i++){
			if(ItemDatabase.instance.SpriteList[i].name == n){
				return Sprite.Create(ItemDatabase.instance.SpriteList[i],new Rect(0,0,ItemDatabase.instance.SpriteList[i].width,ItemDatabase.instance.SpriteList[i].height),Vector2.zero);
			}
		}
		return null;
	}


	public string ConvertTabType(CraftingTabType n){
		if(n==CraftingTabType.Equipment){
			return "装备";
			
		}else if(n==CraftingTabType.Reagent){
			return "材料";
		}

		return "";
	}
}
