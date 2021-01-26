using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;


public class ItemEditorManager : EditorWindow
{
    // Start is called before the first frame update
    Items item =new Items();
	public CraftedItem craftItem;

	ItemTypeToCreate itemTypeToCreate;

	ArmorTypeToCreate armorTypeToCreate;

	OtherTypeToCreate otherTypeToCreate;

	ConsumableTypeToCreate consumableTypeToCreate;

	RingType ringType ;
	WeaponType weaponType ;
	
	bool offhandOnly;
	
	static ItemDatabase itemDatabase;

	bool createItems;
	bool manageItems;
	bool createCraftingItems;
	bool manageCraftingItems;

	int craftItemID;

	Items itemToManage;
	CardAsset cardAssetManage;

	public CraftedItem crafItemToManage;
	
	SerializedObject serObj;

	private Vector2 scrollPosition;

	private enum SelectedAction {
		createItem,
		manageItems,
		createCraftingItem,
		manageCraftingItems
	}

	private SelectedAction selectedAction;

	// Add menu named "My Window" to the Window menu
	
	[MenuItem("Items/ItemManager")]
	static void Init () {
		Debug.Log("Show Panel");
		itemDatabase = (ItemDatabase)Resources.Load("ItemDatabase", typeof(ItemDatabase)) as ItemDatabase;
		EditorWindow.GetWindow(typeof(ItemEditorManager));
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
//		else if(selectedAction == SelectedAction.createCraftingItem) {
//			CreateCraftingItem();
//		}
//		else if(selectedAction == SelectedAction.manageCraftingItems) {
//			ManageCraftingItems();
//		}

		serObj.ApplyModifiedProperties();
		
		if(GUI.changed) {
			//Apply the changes to the item database
			EditorUtility.SetDirty(itemDatabase);
			//Save the item database prefab
			PrefabUtility.SetPropertyModifications(PrefabUtility.GetPrefabInstanceHandle(itemDatabase), PrefabUtility.GetPropertyModifications(itemDatabase));
		}
	}

	void CreateItem() {
		itemTypeToCreate = (ItemTypeToCreate)EditorGUILayout.EnumPopup("Item type:",itemTypeToCreate);
		
		item.itemName = EditorGUILayout.TextField("Item name:",item.itemName);

		item.equipmentSlotype =(EquipmentSlotType)EditorGUILayout.EnumPopup("slot Type",item.equipmentSlotype);
		
		item.width = EditorGUILayout.IntField(new GUIContent("Width:","This controls the width size of the item."), item.width);
		
		item.height = EditorGUILayout.IntField(new GUIContent("Height:","This controls the height size of the item."), item.height);
		
		item.buyPrice = EditorGUILayout.IntField("Buy price:", item.buyPrice);
		
		item.sellPrice = EditorGUILayout.IntField("Sell price:", item.sellPrice);
		
		item.itemRatity = (ItemRatity)EditorGUILayout.EnumPopup("Item quality:", item.itemRatity);
		item.perc =EditorGUILayout.FloatField("Item Perc",item.perc);
		item.icon = (Sprite)EditorGUILayout.ObjectField("Icon name:", item.icon, typeof(Sprite), false);
		
	
		if(itemTypeToCreate == ItemTypeToCreate.weapon) {
			item.weaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon type:", item.weaponType);

	string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
			
			for(int i = 0; i < itemTypes.Length; i++){
				if(itemTypes[i] == itemTypeToCreate.ToString()) {
					item.itemType = (EquipmentSlotType)System.Enum.Parse(typeof(EquipmentSlotType), itemTypes[i]);
					break;
				}
			}
			item.itemLevel = EditorGUILayout.IntField("Item level requirement:", item.itemLevel);
			item.weaponDur = EditorGUILayout.IntField("Max durability:", item.weaponDur);
			
			GUILayout.BeginHorizontal();
			item.minDamage = EditorGUILayout.FloatField(" min damage:", item.minDamage);
			item.maxDamage = EditorGUILayout.FloatField(" max damage:", item.maxDamage);
			GUILayout.EndHorizontal();
			// item.attackSpeed = EditorGUILayout.FloatField("Attack speed:", item.attackSpeed);
			
			// GUILayout.BeginHorizontal();
			// item.minStrength = EditorGUILayout.FloatField("Min strength:",item.minStrength);
			// item.maxStrength = EditorGUILayout.FloatField("Max strength:",item.maxStrength);
			// GUILayout.EndHorizontal();
			
			// GUILayout.BeginHorizontal();
			// item.minDexterity = EditorGUILayout.FloatField("Min dexterity:",item.minDexterity);
			// item.maxDexterity = EditorGUILayout.FloatField("Max dexterity:",item.maxDexterity);
			// GUILayout.EndHorizontal();
			
			
			
			// GUILayout.BeginHorizontal();
			// item.minMagic = EditorGUILayout.FloatField("Min magic:",item.minMagic);
			// item.maxMagic = EditorGUILayout.FloatField("Max magic:",item.maxMagic);
			// GUILayout.EndHorizontal();


				GUILayout.BeginHorizontal();
				item.fireResistance = EditorGUILayout.IntField("F R", item.fireResistance);
				item.iceResistance = EditorGUILayout.IntField("I R", item.iceResistance);
				item.posionResistance = EditorGUILayout.IntField("PO R", item.posionResistance);
				item.phyicsResistance = EditorGUILayout.IntField("PH R", item.phyicsResistance);
				GUILayout.EndHorizontal();
			
			
			
		}
		else if(itemTypeToCreate == ItemTypeToCreate.armor) {
			armorTypeToCreate = (ArmorTypeToCreate)EditorGUILayout.EnumPopup("Armor type:", armorTypeToCreate);
			
			string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
			
			for(int i = 0; i < itemTypes.Length; i++){
				if(itemTypes[i] == itemTypeToCreate.ToString()) {
					item.itemType = (EquipmentSlotType)System.Enum.Parse(typeof(EquipmentSlotType), itemTypes[i]);
					break;
				}
			}
			
			item.itemLevel = EditorGUILayout.IntField("Item level requirement:", item.itemLevel);
			
			item.armorDur = EditorGUILayout.IntField("Max durability:", item.armorDur);
			
			GUILayout.BeginHorizontal();
			item.minArmor = EditorGUILayout.FloatField("Min armor:",item.minArmor);
			item.maxArmor = EditorGUILayout.FloatField("Max armor:",item.maxArmor);
			GUILayout.EndHorizontal();
			
		
			// GUILayout.BeginHorizontal();
			// item.minStrength = EditorGUILayout.FloatField("Min strength:",item.minStrength);
			// item.maxStrength = EditorGUILayout.FloatField("Max strength:",item.maxStrength);
			// GUILayout.EndHorizontal();
			
			// GUILayout.BeginHorizontal();
			// item.minDexterity = EditorGUILayout.FloatField("Min dexterity:",item.minDexterity);
			// item.maxDexterity = EditorGUILayout.FloatField("Max dexterity:",item.maxDexterity);
			// GUILayout.EndHorizontal();
			
		
			
			// GUILayout.BeginHorizontal();
			// item.minMagic = EditorGUILayout.FloatField("Min magic:",item.minMagic);
			// item.maxMagic = EditorGUILayout.FloatField("Max magic:",item.maxMagic);
			// GUILayout.EndHorizontal();
		
				GUILayout.BeginHorizontal();
				item.fireResistance = EditorGUILayout.IntField("F R", item.fireResistance);
				item.iceResistance = EditorGUILayout.IntField("I R", item.iceResistance);
				item.posionResistance = EditorGUILayout.IntField("PO R", item.posionResistance);
				item.phyicsResistance = EditorGUILayout.IntField("PH R", item.phyicsResistance);
				GUILayout.EndHorizontal();
			
			
			// GUILayout.EndHorizontal();
		// }else if(itemTypeToCreate==ItemTypeToCreate.ring){
		// 	ringType = (RingType)EditorGUILayout.EnumPopup("Ring Type",ringType);


		// 	string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
			
		// 	for(int i = 0; i < itemTypes.Length; i++){
		// 		if(itemTypes[i] == itemTypeToCreate.ToString()) {
		// 			item.itemType = (EquipmentSlotType)System.Enum.Parse(typeof(EquipmentSlotType), itemTypes[i]);
		// 			break;
		// 		}
		// 	}

			
		// 		GUILayout.BeginHorizontal();
		// 		item.fireResistance = EditorGUILayout.IntField("F R", item.fireResistance);
		// 		item.iceResistance = EditorGUILayout.IntField("I R", item.iceResistance);
		// 		item.posionResistance = EditorGUILayout.IntField("PO R", item.posionResistance);
		// 		item.phyicsResistance = EditorGUILayout.IntField("PH R", item.phyicsResistance);
		// 		GUILayout.EndHorizontal();
			
		// 	item.useEffectScriptName = EditorGUILayout.TextField("useEffect",item.useEffectScriptName);
		// }
		// else if(itemTypeToCreate == ItemTypeToCreate.Other) {
		// 	otherTypeToCreate = (OtherTypeToCreate)EditorGUILayout.EnumPopup("Item type:", otherTypeToCreate);
		
		// 	string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
			
		// 	for(int i = 0; i < itemTypes.Length; i++){
		// 		if(itemTypes[i] == otherTypeToCreate.ToString()) {
		// 			item.itemType = (EquipmentSlotType)System.Enum.Parse(typeof(EquipmentSlotType), itemTypes[i]);
		// 			break;
		// 		}
		// 	}
			
		// 		item.stackable = EditorGUILayout.Toggle("Stackable:",item.stackable);
				
		// 		if(item.stackable) {
		// 			item.maxStackSize = EditorGUILayout.IntField("Maximum stacksize:", item.maxStackSize);
		// 		}

		// 	if(item.itemType == EquipmentSlotType.consumable) {
				
		// 		consumableTypeToCreate = (ConsumableTypeToCreate)EditorGUILayout.EnumPopup("Consumable type:", consumableTypeToCreate);
				
		// 		item.useEffectScriptName = EditorGUILayout.TextField("Use effect script same:",item.useEffectScriptName);
				
		// 		string[] consumableTypes = System.Enum.GetNames (typeof(ConsumableType));
				
		// 		for(int j = 0; j < consumableTypes.Length; j++){
		// 			if(consumableTypes[j] == consumableTypeToCreate.ToString()) {
		// 				item.consumableType = (ConsumableType)System.Enum.Parse(typeof(ConsumableType), consumableTypes[j]);
		// 				break;
		// 			}
		// 		}

		// 		// item.useSound = (AudioClip)EditorGUILayout.ObjectField("Use sound:", item.useSound,typeof(AudioClip), false);
				
		// 	}

		// 	if(item.itemType == EquipmentSlotType.reagent) {
		// 		item.stackable = EditorGUILayout.Toggle("Stackable:",item.stackable);
				
		// 		if(item.stackable) {
		// 			item.maxStackSize = EditorGUILayout.IntField("Maximum stacksize:", item.maxStackSize);
		// 		}
		// 	}
		// }
		
		item.descriptionText = EditorGUILayout.TextField("Description text:",item.descriptionText);
		
		if(GUILayout.Button("Create item")) {
			item.itemID = itemDatabase.items.Count.ToString();
			itemDatabase.AddItem(item);
			//Add Data To Database TODO
			// object vakue =Database.instance.AddItemToDatabase(item);
			
			item = new Items();
			UpdateDatabase();
		}
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
							Debug.Log(itemTypes[j]);
							if(itemToManage.itemType == EquipmentSlotType.weapon) {
								
								break;
							}
							// else if(itemToManage.itemType!=EquipmentSlotType.reagent && itemToManage.itemType != EquipmentSlotType.consumable &&
							// itemToManage.itemType != EquipmentSlotType.socket && itemToManage.itemType!= EquipmentSlotType.ring && itemToManage.itemType != EquipmentSlotType.weapon&&
							// itemToManage.itemType!=EquipmentSlotType.None) {
								else if(itemToManage.itemType == EquipmentSlotType.armor){
								armorTypeToCreate = (ArmorTypeToCreate)System.Enum.Parse(typeof(ArmorTypeToCreate), itemTypes[j]);
								
								break;
							// }else if(itemToManage.itemType!=EquipmentSlotType.reagent && itemToManage.itemType != EquipmentSlotType.consumable &&
							// itemToManage.itemType != EquipmentSlotType.socket && itemToManage.itemType!= EquipmentSlotType.armor
							// ){
								}else if(itemToManage.itemType==EquipmentSlotType.ring){
								ringType = (RingType)System.Enum.Parse(typeof(RingType), itemTypes[j]);
								break;
							}else {
								//scroll or others
								otherTypeToCreate = (OtherTypeToCreate) System.Enum.Parse(typeof(OtherTypeToCreate),itemTypes[i]);

								break;
							}
						
						}
						
					}
				}
				//click button show the item detail
				if(GUILayout.Button(itemDatabase.items[i].itemName)) {
					itemToManage = itemDatabase.items[i];
					string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
					
					for(int j = 0; j < itemTypes.Length; j++){
						if(itemTypes[j] == itemToManage.itemType.ToString()) {
							if(itemToManage.itemType == EquipmentSlotType.weapon) {
								itemTypeToCreate=ItemTypeToCreate.weapon;
								break;
							}
							// else if(itemToManage.itemType!=EquipmentSlotType.reagent && itemToManage.itemType != EquipmentSlotType.consumable &&
							// itemToManage.itemType != EquipmentSlotType.socket && itemToManage.itemType!= EquipmentSlotType.ring) {
						else if(itemToManage.itemType==EquipmentSlotType.armor){
							 itemTypeToCreate=ItemTypeToCreate.armor;		// itemTypeToCreate=ItemTypeToCreate.armor;
								armorTypeToCreate = (ArmorTypeToCreate)System.Enum.Parse(typeof(ArmorTypeToCreate),itemTypes[j]);
								break;
							// }else if(itemToManage.itemType!=EquipmentSlotType.reagent && itemToManage.itemType != EquipmentSlotType.consumable &&
							// itemToManage.itemType != EquipmentSlotType.socket && itemToManage.itemType!= EquipmentSlotType.armor ){
						}
						// else if(itemToManage.itemType==EquipmentSlotType.ring){
						// 		itemTypeToCreate=ItemTypeToCreate.ring;
						// 		ringType = (RingType)System.Enum.Parse(typeof(RingType),itemTypes[j]);
						// 		break;
						// 	}
						// 	else{
						// 		//slot3 ring
						// 		otherTypeToCreate = (OtherTypeToCreate) System.Enum.Parse(typeof(OtherTypeToCreate),itemTypes[i]);
						// 		itemTypeToCreate=ItemTypeToCreate.Other;

						// 		break;
						// 	}
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
				
					itemToManage.equipmentSlotype =(EquipmentSlotType)EditorGUILayout.EnumPopup("slot Type",itemToManage.equipmentSlotype);
				itemToManage.buyPrice = EditorGUILayout.IntField("Buy price:", itemToManage.buyPrice);
				
				itemToManage.sellPrice = EditorGUILayout.IntField("Sell price:", itemToManage.sellPrice);
				itemToManage.perc =EditorGUILayout.FloatField("Item Perc",itemToManage.perc);
				itemToManage.itemRatity = (ItemRatity)EditorGUILayout.EnumPopup("Item quality:", itemToManage.itemRatity);
				
				itemToManage.icon = (Sprite)EditorGUILayout.ObjectField("Icon name:", itemToManage.icon, typeof(Sprite), false);
				
				//Itemtype is weapon
						if(itemToManage.itemType == EquipmentSlotType.weapon ) {
							itemToManage.weaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon type:", itemToManage.weaponType);
			
							itemToManage.twoHanded = EditorGUILayout.Toggle("Two handed", itemToManage.twoHanded);
							itemToManage.itemLevel = EditorGUILayout.IntField("Item level requirement:", itemToManage.itemLevel);
							itemToManage.weaponDur = EditorGUILayout.IntField("Max durability:", itemToManage.weaponDur);
							
							GUILayout.BeginHorizontal();
							itemToManage.minDamage = EditorGUILayout.FloatField(" min damage:", itemToManage.minDamage);
							itemToManage.maxDamage = EditorGUILayout.FloatField(" max damage:", itemToManage.maxDamage);
							itemToManage.damage = EditorGUILayout.FloatField("Damaage",
								Random.Range(Mathf.RoundToInt(itemToManage.minDamage),Mathf.RoundToInt(itemToManage.maxDamage)));
							GUILayout.EndHorizontal();
							// itemToManage.attackSpeed = EditorGUILayout.FloatField("Attack speed:", itemToManage.attackSpeed);
							
							// GUILayout.BeginHorizontal();
							// itemToManage.minStrength = EditorGUILayout.FloatField("Min strength:",itemToManage.minStrength);
							// itemToManage.maxStrength = EditorGUILayout.FloatField("Max strength:",itemToManage.maxStrength);
							// itemToManage.strength = EditorGUILayout.FloatField("strength",
							// 	Random.Range(Mathf.RoundToInt( itemToManage.minStrength),Mathf.RoundToInt (itemToManage.maxStrength)));
							// GUILayout.EndHorizontal();
							
							// GUILayout.BeginHorizontal();
							// itemToManage.minDexterity = EditorGUILayout.FloatField("Min dexterity:",itemToManage.minDexterity);
							// itemToManage.maxDexterity = EditorGUILayout.FloatField("Max dexterity:",itemToManage.maxDexterity);
							// itemToManage.dexterity = EditorGUILayout.FloatField("dex",
							// 	Random.Range(Mathf.RoundToInt(itemToManage.minDexterity), Mathf.RoundToInt(itemToManage.dexterity)));
							// GUILayout.EndHorizontal();
							
						
							
							// GUILayout.BeginHorizontal();
							// itemToManage.minMagic = EditorGUILayout.FloatField("Min magic:",itemToManage.minMagic);
							// itemToManage.maxMagic = EditorGUILayout.FloatField("Max magic:",itemToManage.maxMagic);
							// itemToManage.magic = EditorGUILayout.FloatField("Magic",
							// 	Random.Range(Mathf.RoundToInt(itemToManage.minMagic),Mathf.RoundToInt((itemToManage.maxMagic))));
							// GUILayout.EndHorizontal();

							//Reistance
							GUILayout.BeginHorizontal();
							itemToManage.fireResistance = EditorGUILayout.IntField("FR:",itemToManage.fireResistance);
							GUILayout.EndHorizontal();
							
							GUILayout.BeginHorizontal();
							itemToManage.iceResistance = EditorGUILayout.IntField("IR",
								itemToManage.iceResistance);
							GUILayout.EndHorizontal();

							GUILayout.BeginHorizontal();
							itemToManage.posionResistance = EditorGUILayout.IntField("PR:",itemToManage.posionResistance);
							GUILayout.EndHorizontal();


							GUILayout.BeginHorizontal();
							itemToManage.phyicsResistance = EditorGUILayout.IntField("PHY R",itemToManage.phyicsResistance);
						GUILayout.EndHorizontal();


						}
				//Armor 
				else
				//  if(itemToManage.itemType!=EquipmentSlotType.reagent && itemToManage.itemType != EquipmentSlotType.consumable &&
				// 			itemToManage.itemType != EquipmentSlotType.socket && itemToManage.itemType != EquipmentSlotType.ring ) {
					if(itemToManage.itemType == EquipmentSlotType.ring){
					armorTypeToCreate = (ArmorTypeToCreate)EditorGUILayout.EnumPopup("Armor type:", armorTypeToCreate);
					
					string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
					
					for(int j = 0; j < itemTypes.Length; j++){
						if(itemTypes[j] == armorTypeToCreate.ToString()) {
							itemToManage.itemType = (EquipmentSlotType)System.Enum.Parse(typeof(EquipmentSlotType), itemTypes[j]);
							break;
						}
					}
					
					itemToManage.itemLevel = EditorGUILayout.IntField("Item level requirement:", itemToManage.itemLevel);
					itemToManage.armorDur = EditorGUILayout.IntField("Max durability:", itemToManage.armorDur);
					
					GUILayout.BeginHorizontal();
					itemToManage.minArmor = EditorGUILayout.FloatField("Min armor:",itemToManage.minArmor);
					itemToManage.maxArmor = EditorGUILayout.FloatField("Max armor:",itemToManage.maxArmor);
					itemToManage.armor = EditorGUILayout.FloatField("Armor",Random.Range(Mathf.RoundToInt(itemToManage.minArmor),Mathf.RoundToInt(itemToManage.maxArmor)));
					GUILayout.EndHorizontal();
					
				
					
					// GUILayout.BeginHorizontal();
					// itemToManage.minDexterity = EditorGUILayout.FloatField("Min dexterity:",itemToManage.minDexterity);
					// itemToManage.maxDexterity = EditorGUILayout.FloatField("Max dexterity:",itemToManage.maxDexterity);
					// itemToManage.dexterity = EditorGUILayout.FloatField("dex",Random.Range(Mathf.RoundToInt( itemToManage.minDexterity),Mathf.RoundToInt(itemToManage.maxDexterity)));
					// GUILayout.EndHorizontal();

					// GUILayout.BeginHorizontal();
					// itemToManage.minMagic = EditorGUILayout.FloatField("Min magic:",itemToManage.minMagic);
					// itemToManage.maxMagic = EditorGUILayout.FloatField("Max magic:",itemToManage.maxMagic);
					// itemToManage.magic = EditorGUILayout.FloatField("magic",Random.Range(Mathf.RoundToInt( itemToManage.minMagic),Mathf.RoundToInt(itemToManage.maxMagic)));
					// GUILayout.EndHorizontal();
					
				//Reistance
					GUILayout.BeginHorizontal();
					itemToManage.fireResistance = EditorGUILayout.IntField("FR:",itemToManage.fireResistance);
			
					itemToManage.iceResistance = EditorGUILayout.IntField("IR",
						itemToManage.iceResistance);
					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();
					itemToManage.posionResistance = EditorGUILayout.IntField("PR:",itemToManage.posionResistance);
					GUILayout.EndHorizontal();


					GUILayout.BeginHorizontal();
					itemToManage.phyicsResistance = EditorGUILayout.IntField("PHY R",itemToManage.phyicsResistance);
				GUILayout.EndHorizontal();
				
				}
				else 
				//Get Ring Equipment Info
				// if(itemToManage.itemType!=EquipmentSlotType.reagent && itemToManage.itemType != EquipentSlotType.consumable &&
				// 			itemToManage.itemType != EquipmentSlotType.socket && itemToManage.itemType != EquipmentSlotType.armor 
				// 		){
					if(itemToManage.itemType==EquipmentSlotType.ring){
					ringType = (RingType)EditorGUILayout.EnumPopup("ring type",ringType);
						
					string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
					
					for(int j = 0; j < itemTypes.Length; j++){
						if(itemTypes[j] == armorTypeToCreate.ToString()) {
							itemToManage.itemType = (EquipmentSlotType)System.Enum.Parse(typeof(EquipmentSlotType), itemTypes[j]);
							break;
						}
					}
					GUILayout.BeginHorizontal();
					itemToManage.fireResistance = EditorGUILayout.IntField("FR:",itemToManage.fireResistance);
					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();
					itemToManage.iceResistance = EditorGUILayout.IntField("IR",
						itemToManage.iceResistance);
					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();
					itemToManage.posionResistance = EditorGUILayout.IntField("PR:",itemToManage.posionResistance);
					GUILayout.EndHorizontal();


					GUILayout.BeginHorizontal();
					itemToManage.phyicsResistance = EditorGUILayout.IntField("PHY R",itemToManage.phyicsResistance);
				GUILayout.EndHorizontal();
				
					itemToManage.useEffectScriptName = EditorGUILayout.TextField("use Effect",itemToManage.useEffectScriptName);
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
					}
				}
				
				itemToManage.descriptionText = EditorGUILayout.TextField("Description text:",itemToManage.descriptionText);
				
			}
		}
		GUILayout.EndScrollView();
	}
	
	void UpdateDatabase() {
		for(int i = 0; i < itemDatabase.items.Count; i++) {
			itemDatabase.items[i].itemID = i.ToString();
		}
	}
}
