using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

[System.Serializable]
public class MerchantItemList
{
    public TabType tabType;
    [HideInInspector]
    public List<Items> items;
    public Sprite tabSprite;
}


/// <summary>
/// Controller Merchant Tools
/// </summary>
public class MerchantController : MonoBehaviour,IPointerClickHandler
{
   public List<MerchantItemList> tabs;

	
	[HideInInspector]
	public Merchant merchant;

	public bool canRepair;

	public Sprite repairSprite;
	public List<string> itemIDs;

	public AudioClip talk;
	public AudioClip business;

	private PlayerData player;

	public Image GlowImage;

	public Text NoticeText;
	public Text npcText;
	public Text detailText;



	void Start() {
//		itemDatabase = (ItemDatabase)Resources.Load("ItemDatabase", typeof(ItemDatabase)) as ItemDatabase;
		merchant = GameObject.FindGameObjectWithTag("Merchant").GetComponent<Merchant>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
		


		//Find and add the items based on their IDs
		
	}

	//Finds the tab type of the item.
	TabType FindTabType(Items item) {
		//Item is of type weapon
		if(item.itemType == EquipmentSlotType.weapon) {
			return TabType.weapon;
		}
		//Item is neither of type weapon, reagent, consumable or socket
		//This means the item is of type armor or jewelry
		else if(item.itemType == EquipmentSlotType.armor) {
			return TabType.armor;
		}else if(item.itemType==EquipmentSlotType.ring){
			return TabType.ring;
		}
		//Item is of type weapon, reagent, consumable or socket
		else {
			return TabType.misc;
		}
	}

	// Update is called once per frame
	void Update () {
		//If the player right clicks
//		if(Input.GetMouseButtonDown(1)) {
//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//			RaycastHit hit;
//			//Send a ray to the position of the mouse
//			if(Physics.Raycast(ray,out hit,Mathf.Infinity)) {
//				//If the ray hits the merchant
//				if(hit.transform.CompareTag("MerchantController") && hit.transform == transform) {
//
//					//Set the selected merchant to be this one
//					merchant.selectedMerchant = this;
//
//					//Open the merchant
//					merchant.OpenCloseMerchant(true);
//
//					//remove all the tabs of the merchant
//					foreach(MerchantTabs t in merchant.tabsObj.GetComponentsInChildren<MerchantTabs>()) {
//						merchant.tabs.Remove(t);
//						Destroy(t.gameObject);
//					}
//
//					//instantiate the tabs that this merchant has
//					for(int i = 0; i < tabs.Count; i++) {
//						if(tabs[i].tabType != TabType.buyBack || 
//						   (tabs[i].tabType == TabType.buyBack && tabs[i].items.Count != 0)) {
//							GameObject tempTab = Instantiate(merchant.tabPrefab) as GameObject;
//							tempTab.transform.SetParent(merchant.tabsObj.transform);
//							tempTab.transform.localScale = Vector3.one;
////							tempTab.GetComponent<Image>().color = merchant.tabInactiveColor;
//							MerchantTabs tab = tempTab.AddComponent<MerchantTabs>();
//							tab.tabType = tabs[i].tabType;
//							tab.items = tabs[i].items;
//							tempTab.GetComponent<Image>().sprite = tabs[i].tabSprite;
//							merchant.tabs.Add(tab);
//						}
//					}
//
//					//If the merchant can repair then add the repair tab
//					if(canRepair) {
//						GameObject tempTab = Instantiate(merchant.tabPrefab) as GameObject;
//						tempTab.transform.SetParent(merchant.tabsObj.transform);
//
//						tempTab.transform.localScale = Vector3.one;
////						tempTab.GetComponent<Image>().color = merchant.tabInactiveColor;
//						MerchantTabs tab = tempTab.AddComponent<MerchantTabs>();
//						tab.tabType = TabType.repair;
//						tab.items = new List<Items>();
//						tempTab.GetComponent<Image>().sprite = repairSprite;
//						merchant.tabs.Add(tab);
//					}
//
//					//Set the seleted tab of the merchant to be equal to the first of this merchants tabs
//					merchant.selectedTab = merchant.tabs[0];
////					merchant.selectedTab.GetComponent<Image>().color = merchant.tabActiveColor;
//					merchant.ChangeTab(merchant.selectedTab);
//
//					merchant.tabsObj.GetComponent<RectTransform>().sizeDelta = new Vector2(merchant.tabWidth, merchant.tabHeight * merchant.tabs.Count);
//
//					//set the items of the merchant's tabs
//					for(int i = 0; i < merchant.tabs.Count; i++) {
//						for(int j = 0; j < tabs.Count; j++) {
//							if(merchant.tabs[i].tabType == tabs[j].tabType) {
//								merchant.tabs[i].items = tabs[j].items;
//							}
//						}
//					}
//				}
//			}
//		}
		//If the player is further away from the merchant than the interaction range of the merchant then close the merchant window
//		if(Vector3.Distance(player.transform.position, transform.position) > interactionRange && merchant.selectedMerchant == this) {
//			merchant.OpenCloseMerchant(false);
//		}
	}

	/// <summary>
	/// Add Items To MerchantSlot for player
	/// </summary>
	public void MerchantAddItemFromNPC(){
		//Reset the list of items in the tabs
		for(int i = 0; i < tabs.Count; i++) {
			tabs[i].items = new List<Items>();
		}
		
		for(int i = 0; i < itemIDs.Count; i++) {
			Items item = ItemDatabase.instance.FindItemByName(itemIDs[i]);
			TabType tabType = FindTabType(item);
			for(int j = 0; j < tabs.Count; j++) {
				if(tabs[j].tabType == tabType) {
					Debug.Log("add item"+item.itemName+"\n"+item.equipmentSlotype);
					tabs[j].items.Add(item);
				}
			}
		}
	}

	public void LoadMerchant()
	{
	merchant.panel.SetActive(true);
		
		//
		foreach (MerchantTabs m in merchant.GetComponentsInChildren<MerchantTabs>())
		{
			merchant.tabs.Remove(m);
			Destroy(m.gameObject);
		}
		
			for (int i = 0; i < tabs.Count; i++)
			{
				
					GameObject tabObj = Instantiate(merchant.tabPrefab,merchant.tabsObj.transform.position,Quaternion.identity)as GameObject;
					tabObj.transform.SetParent(merchant.tabsObj.transform);
					tabObj.transform.localScale = Vector3.one;
					//Set Active Color=>
//					tabObj.GetComponent<Image>().color = Color.grey;
					//
					MerchantTabs tab = tabObj.AddComponent<MerchantTabs>();
					tab.tabType = tabs[i].tabType;
					tab.items = tabs[i].items;
					tab.GetComponent<Image>().sprite = tabs[i].tabSprite;
					merchant.tabs.Add(tab);
				
			}
			
			//If the merchant can repair then add the repair tab
			if(canRepair) {
				GameObject tempTab = Instantiate(merchant.tabPrefab) as GameObject;
				tempTab.transform.SetParent(merchant.tabsObj.transform);

				tempTab.transform.localScale = Vector3.one;
//						tempTab.GetComponent<Image>().color = merchant.tabInactiveColor;
				MerchantTabs tab = tempTab.AddComponent<MerchantTabs>();
				tab.tabType = TabType.repair;
				tab.items = new List<Items>();
				tempTab.GetComponent<Image>().sprite = repairSprite;
				merchant.tabs.Add(tab);
			}

			//Set the seleted tab of the merchant to be equal to the first of this merchants tabs
			merchant.selectedTab = merchant.tabs[0];
//					merchant.selectedTab.GetComponent<Image>().color = merchant.tabActiveColor;
			merchant.ChangeTab(merchant.selectedTab);

			merchant.tabsObj.GetComponent<RectTransform>().sizeDelta = new Vector2(merchant.tabWidth, merchant.tabHeight * merchant.tabs.Count);

			//set the items of the merchant's tabs
			for(int i = 0; i < merchant.tabs.Count; i++) {
				for(int j = 0; j < tabs.Count; j++) {
					if(merchant.tabs[i].tabType == tabs[j].tabType) {
						merchant.tabs[i].items = tabs[j].items;
					}
				
			}
		}
		//Change to inventory
		TownManager.instance.topPanelManager.PanelAnim(3);
		// InventorySystem.instance.OpenCloseInventory(true);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
//		
//		if (eventData.button == PointerEventData.InputButton.Left)
//		{
//			SoundManager.instance.PlayClipAtPoint(talk, Vector3.zero, SoundManager.instance.fxVolume, false);
//			// LoadMerchant();
//		}else if (eventData.button == PointerEventData.InputButton.Right)
//		{
//			SoundManager.instance.PlayClipAtPoint(business, Vector3.zero, SoundManager.instance.fxVolume, false);
//			LoadMerchant();
//			
//		}
	}

	
	
}
