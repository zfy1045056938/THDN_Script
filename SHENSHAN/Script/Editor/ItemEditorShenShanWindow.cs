using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ItemEditorShenShanWindow : EditorWindow
{
  
	Items item = new Items();
	public CraftedItem craftItem;

	ItemTypeToCreate itemTypeToCreate;

	ArmorTypeToCreate armorTypeToCreate;

	OtherTypeToCreate otherTypeToCreate;

	ConsumableTypeToCreate consumableTypeToCreate;
	
	bool offhandOnly;
	
	static ItemDatabase itemDatabase;

	bool createItems;
	bool manageItems;
	bool createCraftingItems;
	bool manageCraftingItems;

	int craftItemID;

	Items itemToManage;
	public CraftedItem crafItemToManage;
	
	SerializedObject serObj;

	private Vector2 scrollPosition;

	private enum SelectedAction {
		createItem,
		manageItems,
		createCraftingItem,
		manageCraftingItems,
	
	}

	private SelectedAction selectedAction;

	// Add menu named "My Window" to the Window menu
	[MenuItem ("Window/ShenShanManageItems")]
	static void Init () {
		Debug.Log("LOad DB");
		itemDatabase = (ItemDatabase)Resources.Load("ItemDatabases", typeof(ItemDatabase)) as ItemDatabase;
		EditorWindow.GetWindow(typeof(ItemEditorShenShanWindow));
	}

	void OnEnable() {
		serObj = new SerializedObject(this);

	}

	void OnInspectorUpdate() {
		itemDatabase = (ItemDatabase)Resources.Load("ItemDatabase", typeof(ItemDatabase)) as ItemDatabase;
	}

	void OnGUI () {
		serObj.Update();

		GUILayout.Space(10);


		GUI.color = Color.green;

		GUILayout.BeginHorizontal();

		//Create items button

		if(selectedAction == SelectedAction.createItem) {
			GUI.color = Color.green;
		}
		else {
			GUI.color = Color.white;
		}
		if(GUILayout.Button("Create items")) {
			selectedAction = SelectedAction.createItem;
		}

		//Manage items button

		if(selectedAction == SelectedAction.manageItems) {
			GUI.color = Color.green;
		}
		else {
			GUI.color = Color.white;
		}
		if(GUILayout.Button("Manage items")) {
			selectedAction = SelectedAction.manageItems;
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();

		//Create crafting items button

		if(selectedAction == SelectedAction.createCraftingItem) {
			GUI.color = Color.green;
		}
		else {
			GUI.color = Color.white;
		}
		if(GUILayout.Button("Create crafted items")) {
			selectedAction = SelectedAction.createCraftingItem;
			craftItem = new CraftedItem();
			craftItem.materialIDs = new System.Collections.Generic.List<int>();
		}

		//Manage crafting items butotn

		if(selectedAction == SelectedAction.manageCraftingItems) {
			GUI.color = Color.green;
		}
		else {
			GUI.color = Color.white;
		}
		if(GUILayout.Button("Manage crafted items")) {
			selectedAction = SelectedAction.manageCraftingItems;
		}
		GUILayout.EndHorizontal();

		//Reset the gui color

		GUI.color = Color.white;

		GUILayout.Space(25);


		if(selectedAction == SelectedAction.createItem) {
			CreateItem();
		}
		else if(selectedAction == SelectedAction.manageItems) {
			ManageItems();
		}
		

		serObj.ApplyModifiedProperties();
		
		if(GUI.changed) {
			//Apply the changes to the item database
			EditorUtility.SetDirty(itemDatabase);
			//Save the item database prefab
			PrefabUtility.SetPropertyModifications(PrefabUtility.GetPrefabObject(itemDatabase), PrefabUtility.GetPropertyModifications(itemDatabase));
		}
	}
public void ClearAllItems(){
	if(itemDatabase.items.Count!=0){
		for(int i=0;i<itemDatabase.items.Count;i++){
			itemDatabase.items.Remove(itemDatabase.items[i]);
		}
	}else{
		Debug.Log("There's no items in itemdatabase");
	}
}
	void CreateItem() {
		itemTypeToCreate = (ItemTypeToCreate)EditorGUILayout.EnumPopup("Item type:",itemTypeToCreate);
		
		item.itemName = EditorGUILayout.TextField("Item name:",item.itemName);
		
		item.width = EditorGUILayout.IntField(new GUIContent("Width:","This controls the width size of the item."), item.width);
		
		item.height = EditorGUILayout.IntField(new GUIContent("Height:","This controls the height size of the item."), item.height);
		
		item.buyPrice = EditorGUILayout.IntField("Buy price:", item.buyPrice);
		
		item.sellPrice = EditorGUILayout.IntField("Sell price:", item.sellPrice);
		
		item.itemRatity = (ItemRatity)EditorGUILayout.EnumPopup("Item quality:", item.itemRatity);
		
		item.icon = (Sprite)EditorGUILayout.ObjectField("Icon name:", item.icon, typeof(Sprite), false);
		
        item.canSplit =EditorGUILayout.Toggle("CanSplit",item.canSplit);
		// item.itemSound = (AudioClip)EditorGUILayout.ObjectField("Item sound:",item.itemSound, typeof(AudioClip),false);
		GUILayout.BeginHorizontal();
            item.itemSpecialName = EditorGUILayout.TextField("Item Effect",item.itemSpecialName);
            GUILayout.EndHorizontal();
            	GUILayout.BeginHorizontal();
            item.cooldown = EditorGUILayout.IntField("Item ColdDown",item.cooldown);
            GUILayout.EndHorizontal();
		
		if(itemTypeToCreate == ItemTypeToCreate.weapon) {
			item.weaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon type:", item.weaponType);
			
            item.weaponDur = EditorGUILayout.IntField("Weapon Dur",item.weaponDur);
			offhandOnly = EditorGUILayout.Toggle("Offhand", offhandOnly);
			if(offhandOnly)
				item.twoHanded = false;
			if(!offhandOnly)
				item.twoHanded = EditorGUILayout.Toggle("Two handed", item.twoHanded);
			item.itemLevel = EditorGUILayout.IntField("Item level requirement:", item.itemLevel);
			
			GUILayout.BeginHorizontal();
			item.minDamage = EditorGUILayout.IntField("Minimum  damage:", (int)item.minDamage);
			item.maxDamage = EditorGUILayout.IntField("Maximum  damage:", (int)item.maxDamage);
			GUILayout.EndHorizontal();
			
			
			
			// GUILayout.BeginHorizontal();
			// item.minStrength = EditorGUILayout.IntField("Min strength:",(int)item.minStrength);
			// item.maxStrength = EditorGUILayout.IntField("Max strength:",(int)item.maxStrength);
			// GUILayout.EndHorizontal();
			
			// GUILayout.BeginHorizontal();
			// item.minDexterity = EditorGUILayout.IntField("Min dexterity:",(int)item.minDexterity);
			// item.maxDexterity = EditorGUILayout.IntField("Max dexterity:",(int)item.maxDexterity);
			// GUILayout.EndHorizontal();
			
		
			// GUILayout.BeginHorizontal();
			// item.minMagic = EditorGUILayout.IntField("Min magic:",(int)item.minMagic);
			// item.maxMagic = EditorGUILayout.IntField("Max magic:",(int)item.maxMagic);
			// GUILayout.EndHorizontal();

            
		}
		else if(itemTypeToCreate == ItemTypeToCreate.armor) {
			armorTypeToCreate = (ArmorTypeToCreate)EditorGUILayout.EnumPopup("Armor type:", armorTypeToCreate);
			
			string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
			
			for(int i = 0; i < itemTypes.Length; i++){
				if(itemTypes[i] == armorTypeToCreate.ToString()) {
					item.itemType = (EquipmentSlotType)System.Enum.Parse(typeof(EquipmentSlotType), itemTypes[i]);
					break;
				}
			}
			
			item.itemLevel = EditorGUILayout.IntField("Item level requirement:", item.itemLevel);
			
			item.armorDur = EditorGUILayout.IntField("Max durability:", item.armorDur);
			
			GUILayout.BeginHorizontal();
			item.minArmor = EditorGUILayout.IntField("Min armor:",(int)item.minArmor);
			item.maxArmor = EditorGUILayout.IntField("Max armor:",(int)item.maxArmor);
			GUILayout.EndHorizontal();
			
			if(armorTypeToCreate == ArmorTypeToCreate.offHand) {
				GUILayout.BeginHorizontal();
				item.minBlockChance = EditorGUILayout.IntField("Min block chance:",(int)item.minBlockChance);
				item.maxBlockChance = EditorGUILayout.IntField("Max block chance:",(int)item.maxBlockChance);
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				item.minBlockAmount = EditorGUILayout.IntField("Minimum  block amount:",(int)item.minBlockAmount);
				item.maxBlockAmount = EditorGUILayout.IntField("Maximum  block amount:",(int)item.maxBlockAmount);
				GUILayout.EndHorizontal();
				
			
			}
			
			// GUILayout.BeginHorizontal();
			// item.minStrength = EditorGUILayout.IntField("Min strength:",(int)item.minStrength);
			// item.maxStrength = EditorGUILayout.IntField("Max strength:",(int)item.maxStrength);
			// GUILayout.EndHorizontal();
			
			// GUILayout.BeginHorizontal();
			// item.minDexterity = EditorGUILayout.IntField("Min dexterity:",(int)item.minDexterity);
			// item.maxDexterity = EditorGUILayout.IntField("Max dexterity:",(int)item.maxDexterity);
			// GUILayout.EndHorizontal();
			
		
			
			// GUILayout.BeginHorizontal();
			// item.minMagic = EditorGUILayout.IntField("Min magic:",(int)item.minMagic);
			// item.maxMagic = EditorGUILayout.IntField("Max magic:",(int)item.maxMagic);
			// GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minFireRes = EditorGUILayout.IntField("Min Fire resistance:",item.minFireRes);
			item.maxFireRes = EditorGUILayout.IntField("Max Fire resistance:",item.maxFireRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minIceRes = EditorGUILayout.IntField("Min Ice resistance:",item.minIceRes);
			item.maxIceRes = EditorGUILayout.IntField("Max Ice resistance:",item.maxIceRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minPosionRes = EditorGUILayout.IntField("Min Posion resistance:",item.minPosionRes);
			item.maxPosionRes = EditorGUILayout.IntField("Max Posion resistance:",item.maxPosionRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minElecRes = EditorGUILayout.IntField("Min electronic resistance:",item.minElecRes);
			item.maxElecRes = EditorGUILayout.IntField("Max electronic resistance:",item.maxElecRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minPhyRes = EditorGUILayout.IntField("Min physics resistance:",item.minPhyRes);
			item.maxPhyRes = EditorGUILayout.IntField("Max physics resistance:",item.maxPhyRes);
			GUILayout.EndHorizontal();
			
		
		}
		// else if(itemTypeToCreate == ItemTypeToCreate.Other) {
		// 	otherTypeToCreate = (OtherTypeToCreate)EditorGUILayout.EnumPopup("Item type:", otherTypeToCreate);
			
		// 	string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
			
		// 	for(int i = 0; i < itemTypes.Length; i++){
		// 		if(itemTypes[i] == otherTypeToCreate.ToString()) {
		// 			item.itemType = (EquipmentSlotType)System.Enum.Parse(typeof(EquipmentSlotType), itemTypes[i]);
		// 			break;
		// 		}
		// 	}
			
		// 	if(item.itemType == EquipmentSlotType.consumable) {
				
		// 		consumableTypeToCreate = (ConsumableTypeToCreate)EditorGUILayout.EnumPopup("Consumable type:", consumableTypeToCreate);
				
		// 		string[] consumableTypes = System.Enum.GetNames (typeof(ConsumableType));
				
		// 		for(int j = 0; j < consumableTypes.Length; j++){
		// 			if(consumableTypes[j] == consumableTypeToCreate.ToString()) {
		// 				item.consumableType = (ConsumableType)System.Enum.Parse(typeof(ConsumableType), consumableTypes[j]);
		// 				break;
		// 			}
		// 		}

        //         //


		// 		// item.useSound = (AudioClip)EditorGUILayout.ObjectField("Use sound:", item.useSound,typeof(AudioClip), false);
				
		// 		item.stackable = EditorGUILayout.Toggle("Stackable:",item.stackable);
				
		// 		if(item.stackable) {
		// 			item.maxStackSize = EditorGUILayout.IntField("Maximum stacksize:", item.maxStackSize);
		// 		}
		// 	}else if(itemToManage.itemType ==EquipmentSlotType.ring){
        //         GUILayout.BeginHorizontal();
        //         item.ringDur = EditorGUILayout.IntField("RingDur",item.ringDur);
        //         GUILayout.EndHorizontal();

        //         GUILayout.BeginHorizontal();
		// 	item.minStrength = EditorGUILayout.IntField("Min strength:",(int)item.minStrength);
		// 	item.maxStrength = EditorGUILayout.IntField("Max strength:",(int)item.maxStrength);
		// 	GUILayout.EndHorizontal();
			
		// 	GUILayout.BeginHorizontal();
		// 	item.minDexterity = EditorGUILayout.IntField("Min dexterity:",(int)item.minDexterity);
		// 	item.maxDexterity = EditorGUILayout.IntField("Max dexterity:",(int)item.maxDexterity);
		// 	GUILayout.EndHorizontal();
			
		
			
		// 	GUILayout.BeginHorizontal();
		// 	item.minMagic = EditorGUILayout.IntField("Min magic:",(int)item.minMagic);
		// 	item.maxMagic = EditorGUILayout.IntField("Max magic:",(int)item.maxMagic);
		// 	GUILayout.EndHorizontal();
			
		// 	GUILayout.BeginHorizontal();
		// 	item.minFireRes = EditorGUILayout.IntField("Min Fire resistance:",item.minFireRes);
		// 	item.maxFireRes = EditorGUILayout.IntField("Max Fire resistance:",item.maxFireRes);
		// 	GUILayout.EndHorizontal();
			
		// 	GUILayout.BeginHorizontal();
		// 	item.minIceRes = EditorGUILayout.IntField("Min Ice resistance:",item.minIceRes);
		// 	item.maxIceRes = EditorGUILayout.IntField("Max Ice resistance:",item.maxIceRes);
		// 	GUILayout.EndHorizontal();
			
		// 	GUILayout.BeginHorizontal();
		// 	item.minPosionRes = EditorGUILayout.IntField("Min Posion resistance:",item.minPosionRes);
		// 	item.maxPosionRes = EditorGUILayout.IntField("Max Posion resistance:",item.maxPosionRes);
		// 	GUILayout.EndHorizontal();
			
		// 	GUILayout.BeginHorizontal();
		// 	item.minElecRes = EditorGUILayout.IntField("Min electronic resistance:",item.minElecRes);
		// 	item.maxElecRes = EditorGUILayout.IntField("Max electronic resistance:",item.maxElecRes);
		// 	GUILayout.EndHorizontal();
			
		// 	GUILayout.BeginHorizontal();
		// 	item.minPhyRes = EditorGUILayout.IntField("Min physics resistance:",item.minPhyRes);
		// 	item.maxPhyRes = EditorGUILayout.IntField("Max physics resistance:",item.maxPhyRes);
		// 	GUILayout.EndHorizontal();

        //     }else if(item.itemType==EquipmentSlotType.potion){
        //         GUILayout.BeginHorizontal();
        //         item.minHealAmount=EditorGUILayout.IntField("Min Heal Amount",item.minHealAmount);
        //          item.maxHealAmount=EditorGUILayout.IntField("Max Heal Amount",item.maxHealAmount);
        //          GUILayout.EndHorizontal();

        //         GUILayout.BeginHorizontal();
        //         item.healType = (HealType)EditorGUILayout.EnumPopup("HealType",item.healType);
        //         GUILayout.EndHorizontal();
                 
                

        //     }
		// }
		
		item.descriptionText = EditorGUILayout.TextField("Description text:",item.descriptionText);
		
		if(GUILayout.Button("Create item")) {
			item.itemID = itemDatabase.items.Count.ToString();
			itemDatabase.AddItem(item);
			item = new Items();
			UpdateDatabase();
		}
	}

	
	void ManageItems() {
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		for(int i = 0; i < itemDatabase.items.Count; i++) {
			if(itemDatabase.items[i] != itemToManage) {
				GUILayout.BeginHorizontal();
				if(GUILayout.Button(">","label",GUILayout.Width(10))) {
					itemToManage = itemDatabase.items[i];
					string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
					
					for(int j = 0; j < itemTypes.Length; j++){
						if(itemTypes[j] == itemToManage.itemType.ToString()) {
							if(itemToManage.itemType == EquipmentSlotType.weapon) {
								break;
							}
							else if(itemToManage.itemType != EquipmentSlotType.reagent 
							        && itemToManage.itemType != EquipmentSlotType.consumable 
							        && itemToManage.itemType != EquipmentSlotType.socket) {
								armorTypeToCreate =
									(ArmorTypeToCreate)System.Enum.Parse(typeof(ArmorTypeToCreate),
										itemTypes[j]);
								break;
							}
							else {
								otherTypeToCreate = 
									(OtherTypeToCreate)System.Enum.Parse(typeof(OtherTypeToCreate),
										itemTypes[j]);
								break;
							}
						}
						
					}
				}
				if(GUILayout.Button(itemDatabase.items[i].itemName)) {
					itemToManage = itemDatabase.items[i];
					string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
					
					for(int j = 0; j < itemTypes.Length; j++){
						if(itemTypes[j] == itemToManage.itemType.ToString()) {
							if(itemToManage.itemType == EquipmentSlotType.weapon) {
								break;
							}
							else if(itemToManage.itemType != EquipmentSlotType.reagent && itemToManage.itemType != EquipmentSlotType.consumable && itemToManage.itemType != EquipmentSlotType.socket) {
								armorTypeToCreate = (ArmorTypeToCreate)System.Enum.Parse(typeof(ArmorTypeToCreate), itemTypes[j]);
								break;
							}
							else {
								otherTypeToCreate = (OtherTypeToCreate)System.Enum.Parse(typeof(OtherTypeToCreate), itemTypes[j]);
								break;
							}
						}
					}
				}
				GUI.color = Color.red;
				if(GUILayout.Button("X",GUILayout.Width(25))) {
					if(EditorUtility.DisplayDialog("Remove item", "Are you sure you want to remove the item?", "Remove", "Cancel")) {
						itemDatabase.items.Remove(itemDatabase.items[i]);
						UpdateDatabase();
					}
				}
				GUILayout.EndHorizontal();
				GUI.color = Color.white;
			}
			else {
				GUI.color = Color.green;
				GUILayout.BeginHorizontal();
				if(GUILayout.Button("v","label",GUILayout.Width(10))) {
					itemToManage = null;
					return;
				}
				if(GUILayout.Button(itemDatabase.items[i].itemName)) {
					itemToManage = null;
					return;
				}
				GUI.color = Color.red;
				if(GUILayout.Button("X",GUILayout.Width(25))) {
					if(EditorUtility.DisplayDialog("Remove item", "Are you sure you want to remove the item?", "Remove", "Cancel")) {
						itemDatabase.items.Remove(itemDatabase.items[i]);
						UpdateDatabase();
					}
				}
				GUILayout.EndHorizontal();
				GUI.color = Color.white;
				
				itemToManage.itemID = EditorGUILayout.TextField("Item ID:", itemToManage.itemID);
				
				itemToManage.itemName = EditorGUILayout.TextField("Item name:",itemToManage.itemName);
				
				itemToManage.width = EditorGUILayout.IntField(new GUIContent("Width:","This controls the width size of the item."), itemToManage.width);
				
				itemToManage.height = EditorGUILayout.IntField(new GUIContent("Height:","This controls the height size of the item."), itemToManage.height);
				itemToManage.itemLevel = EditorGUILayout.IntField("Item level requirement:", itemToManage.itemLevel);
				itemToManage.buyPrice = EditorGUILayout.IntField("Buy price:", itemToManage.buyPrice);
				
				itemToManage.sellPrice = EditorGUILayout.IntField("Sell price:", itemToManage.sellPrice);
				
				itemToManage.itemRatity = (ItemRatity)EditorGUILayout.EnumPopup("Item quality:", itemToManage.itemRatity);
				
				itemToManage.icon = (Sprite)EditorGUILayout.ObjectField("Icon name:", itemToManage.icon, typeof(Sprite), false);
			    
                itemToManage.useEffectScriptName =EditorGUILayout.TextField("Effect Name",itemToManage.useEffectScriptName);
				itemToManage.canSplit= EditorGUILayout.Toggle("CanSplit",itemToManage.canSplit);
				if(itemToManage.itemType == EquipmentSlotType.weapon) {
					itemToManage.weaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon type:", itemToManage.weaponType);
					
					itemToManage.twoHanded = EditorGUILayout.Toggle("Two handed", itemToManage.twoHanded);
					
					itemToManage.weaponDur = EditorGUILayout.IntField("Max durability:", itemToManage.weaponDur);
					
					GUILayout.BeginHorizontal();
					itemToManage.minDamage = EditorGUILayout.IntField("Minimum  damage:", (int)itemToManage.minDamage);
					itemToManage.maxDamage = EditorGUILayout.IntField("Maximum  damage:", (int)itemToManage.maxDamage);
					GUILayout.EndHorizontal();
					
					// GUILayout.BeginHorizontal();
					// itemToManage.minStrength = EditorGUILayout.IntField("Min strength:",(int)itemToManage.minStrength);
					// itemToManage.maxStrength = EditorGUILayout.IntField("Max strength:",(int)itemToManage.maxStrength);
					// GUILayout.EndHorizontal();
					
					// GUILayout.BeginHorizontal();
					// itemToManage.minDexterity = EditorGUILayout.IntField("Min dexterity:",(int)itemToManage.minDexterity);
					// itemToManage.maxDexterity = EditorGUILayout.IntField("Max dexterity:",(int)itemToManage.maxDexterity);
					// GUILayout.EndHorizontal();
					
				
					
					// GUILayout.BeginHorizontal();
					// itemToManage.minMagic = EditorGUILayout.IntField("Min magic:",(int)itemToManage.minMagic);
					// itemToManage.maxMagic = EditorGUILayout.IntField("Max magic:",(int)itemToManage.maxMagic);
					// GUILayout.EndHorizontal();
				
					
				}
				else if(itemToManage.itemType != EquipmentSlotType.reagent 
				        && itemToManage.itemType != EquipmentSlotType.consumable 
				        && itemToManage.itemType != EquipmentSlotType.socket&& itemToManage.itemType != EquipmentSlotType.ring) {
					armorTypeToCreate = (ArmorTypeToCreate)EditorGUILayout.EnumPopup("Armor type:", armorTypeToCreate);
					
					string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
					
					for(int j = 0; j < itemTypes.Length; j++){
						if(itemTypes[j] == armorTypeToCreate.ToString()) {
							itemToManage.itemType = (EquipmentSlotType)System.Enum.Parse(typeof(EquipmentSlotType), itemTypes[j]);
							break;
						}
					}
					
				
					itemToManage.armorDur = EditorGUILayout.IntField("Max durability:", itemToManage.armorDur);
					
					GUILayout.BeginHorizontal();
					itemToManage.minArmor = EditorGUILayout.IntField("Min armor:",(int)itemToManage.minArmor);
					itemToManage.maxArmor = EditorGUILayout.IntField("Max armor:",(int)itemToManage.maxArmor);
					GUILayout.EndHorizontal();
					
					if(itemToManage.itemType == EquipmentSlotType.offHand) {
						GUILayout.BeginHorizontal();
						itemToManage.minBlockChance = EditorGUILayout.IntField("Min block chance:",(int)itemToManage.minBlockChance);
						itemToManage.maxBlockChance = EditorGUILayout.IntField("Max block chance:",(int)itemToManage.maxBlockChance);
						GUILayout.EndHorizontal();
						
						
						
						GUILayout.BeginHorizontal();
						itemToManage.minBlockAmount = EditorGUILayout.IntField("Minimum  block amount:",itemToManage.minBlockAmount);
						itemToManage.maxBlockAmount = EditorGUILayout.IntField("Maximum  block amount:",itemToManage.maxBlockAmount);
						GUILayout.EndHorizontal();
					}
					
					// GUILayout.BeginHorizontal();
					// itemToManage.minDexterity = EditorGUILayout.IntField("Min dexterity:",(int)itemToManage.minDexterity);
					// itemToManage.maxDexterity = EditorGUILayout.IntField("Max dexterity:",(int)itemToManage.maxDexterity);
					// GUILayout.EndHorizontal();
					
					
					
					// GUILayout.BeginHorizontal();
					// itemToManage.minMagic = EditorGUILayout.IntField("Min magic:",(int)itemToManage.minMagic);
					// itemToManage.maxMagic = EditorGUILayout.IntField("Max magic:",(int)itemToManage.maxMagic);
					// GUILayout.EndHorizontal();
					
					GUILayout.BeginHorizontal();
			itemToManage.minFireRes = EditorGUILayout.IntField("Min Fire resistance:",itemToManage.minFireRes);
			itemToManage.maxFireRes = EditorGUILayout.IntField("Max Fire resistance:",itemToManage.maxFireRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minIceRes = EditorGUILayout.IntField("Min Ice resistance:",itemToManage.minIceRes);
			itemToManage.maxIceRes = EditorGUILayout.IntField("Max Ice resistance:",itemToManage.maxIceRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minPosionRes = EditorGUILayout.IntField("Min Posion resistance:",itemToManage.minPosionRes);
			itemToManage.maxPosionRes = EditorGUILayout.IntField("Max Posion resistance:",itemToManage.maxPosionRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minElecRes = EditorGUILayout.IntField("Min electronic resistance:",itemToManage.minElecRes);
			itemToManage.maxElecRes = EditorGUILayout.IntField("Max electronic resistance:",itemToManage.maxElecRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minPhyRes = EditorGUILayout.IntField("Min physics resistance:",itemToManage.minPhyRes);
			itemToManage.maxPhyRes = EditorGUILayout.IntField("Max physics resistance:",itemToManage.maxPhyRes);
			GUILayout.EndHorizontal();
				}
				else {
					otherTypeToCreate = (OtherTypeToCreate)EditorGUILayout.EnumPopup("Item type:", otherTypeToCreate);
					
					string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
					
					for(int j = 0; j < itemTypes.Length; j++){
						if(itemTypes[j] == otherTypeToCreate.ToString()) {
							itemToManage.itemType = (EquipmentSlotType)System.Enum.Parse(typeof(EquipmentSlotType), itemTypes[j]);
							break;
						}
					}
					
					if(itemToManage.itemType == EquipmentSlotType.consumable) {
						
						consumableTypeToCreate = (ConsumableTypeToCreate)EditorGUILayout.EnumPopup("Consumable type:", itemToManage.consumableType);
						
						itemToManage.useEffectScriptName = EditorGUILayout.TextField("Use effect script same:",itemToManage.useEffectScriptName);
						
						string[] consumableTypes = System.Enum.GetNames (typeof(ConsumableType));
						
						for(int j = 0; j < consumableTypes.Length; j++){
							if(consumableTypes[j] == consumableTypeToCreate.ToString()) {
								itemToManage.consumableType = (ConsumableType)System.Enum.Parse(typeof(ConsumableType), consumableTypes[j]);
								break;
							}
						}

						// itemToManage.useSound = (AudioClip)EditorGUILayout.ObjectField("Use sound:", itemToManage.useSound, typeof(AudioClip), false);
						
						itemToManage.stackable = EditorGUILayout.Toggle("Stackable:",itemToManage.stackable);
						
						if(itemToManage.stackable) {
							itemToManage.maxStackSize = EditorGUILayout.IntField("Maximum stacksize:", itemToManage.maxStackSize);
						}
					}

					if(itemToManage.itemType == EquipmentSlotType.reagent) {
						itemToManage.stackable = EditorGUILayout.Toggle("Stackable:",itemToManage.stackable);
						
						if(itemToManage.stackable) {
							itemToManage.maxStackSize = EditorGUILayout.IntField("Maximum stacksize:", itemToManage.maxStackSize);
						}
					}else if(itemToManage.itemType==EquipmentSlotType.ring){
  GUILayout.BeginHorizontal();
                item.ringDur = EditorGUILayout.IntField("RingDur",item.ringDur);
                GUILayout.EndHorizontal();

            //     GUILayout.BeginHorizontal();
			// itemToManage.minStrength = EditorGUILayout.IntField("Min strength:",(int)itemToManage.minStrength);
			// itemToManage.maxStrength = EditorGUILayout.IntField("Max strength:",(int)itemToManage.maxStrength);
			// GUILayout.EndHorizontal();
			
			// GUILayout.BeginHorizontal();
			// itemToManage.minDexterity = EditorGUILayout.IntField("Min dexterity:",(int)itemToManage.minDexterity);
			// itemToManage.maxDexterity = EditorGUILayout.IntField("Max dexterity:",(int)itemToManage.maxDexterity);
			// GUILayout.EndHorizontal();
			
		
			
			// GUILayout.BeginHorizontal();
			// itemToManage.minMagic = EditorGUILayout.IntField("Min magic:",(int)itemToManage.minMagic);
			// itemToManage.maxMagic = EditorGUILayout.IntField("Max magic:",(int)itemToManage.maxMagic);
			// GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minFireRes = EditorGUILayout.IntField("Min Fire resistance:",itemToManage.minFireRes);
			itemToManage.maxFireRes = EditorGUILayout.IntField("Max Fire resistance:",itemToManage.maxFireRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minIceRes = EditorGUILayout.IntField("Min Ice resistance:",itemToManage.minIceRes);
			itemToManage.maxIceRes = EditorGUILayout.IntField("Max Ice resistance:",itemToManage.maxIceRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minPosionRes = EditorGUILayout.IntField("Min Posion resistance:",itemToManage.minPosionRes);
			itemToManage.maxPosionRes = EditorGUILayout.IntField("Max Posion resistance:",itemToManage.maxPosionRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minElecRes = EditorGUILayout.IntField("Min electronic resistance:",itemToManage.minElecRes);
			itemToManage.maxElecRes = EditorGUILayout.IntField("Max electronic resistance:",itemToManage.maxElecRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minPhyRes = EditorGUILayout.IntField("Min physics resistance:",itemToManage.minPhyRes);
			itemToManage.maxPhyRes = EditorGUILayout.IntField("Max physics resistance:",itemToManage.maxPhyRes);
			GUILayout.EndHorizontal();

            }else if(itemToManage.itemType==EquipmentSlotType.potion){
                // GUILayout.BeginHorizontal();
                // itemToManage.minHealAmount=EditorGUILayout.IntField("Min Heal Amount",itemToManage.minHealAmount);
                //  itemToManage.maxHealAmount=EditorGUILayout.IntField("Max Heal Amount",itemToManage.maxHealAmount);
                //  GUILayout.EndHorizontal();

                 GUILayout.BeginHorizontal();
                 itemToManage.healType =(HealType)EditorGUILayout.EnumPopup("HealType",itemToManage.healType);
                 GUILayout.EndHorizontal();

                
				}
				
				itemToManage.descriptionText = EditorGUILayout.TextField("Description text:",itemToManage.descriptionText);
				
			}
		}
		GUILayout.EndScrollView();
	}

    }
	void UpdateDatabase() {
		for(int i = 0; i < itemDatabase.items.Count; i++) {
			itemDatabase.items[i].itemID = i.ToString();
		}
	}
}
