using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ItemTypeToCreate{
  // None,
  //   weapon,
  //   armor,
  //   ring,
  //   consumable,
  //   reagent,
  //   socket,
  //   offHand,
  //   Pack,
  //   Card,
	// Other,
  weapon,
  armor,
  other,

}

public enum ArmorTypeToCreate{
  None,
  armor,
light,
magic,
heavy,
offHand,
}

public enum OtherTypeToCreate{
  None,
  consumable,
  reagent,
  socket,
  ring,
}

public enum ConsumableTypeToCreate{
	None,
	potion,
	scroll,
}

public class ItemEditorWindow :EditorWindow 
{
//   Items item =new Items();
//	public CraftedItem craftItem;
//
//	ItemTypeToCreate itemTypeToCreate;
//
//	ArmorTypeToCreate armorTypeToCreate=ArmorTypeToCreate.Light;
//
//	OtherTypeToCreate otherTypeToCreate;
//
//	ConsumableTypeToCreate consumableTypeToCreate;
//
//	RingType ringType =RingType.None;
//	
//	bool offhandOnly;
//	
//	static ItemDatabase itemDatabase;
//
//	bool createItems;
//	bool manageItems;
//	bool createCraftingItems;
//	bool manageCraftingItems;
//
//	int craftItemID;
//
//	Items itemToManage;
//	public CraftedItem crafItemToManage;
//	
//	SerializedObject serObj;
//
//	private Vector2 scrollPosition;
//
//	private enum SelectedAction {
//		createItem,
//		manageItems,
//		createCraftingItem,
//		manageCraftingItems
//	}
//
//	private SelectedAction selectedAction;
//
//	// Add menu named "My Window" to the Window menu
//	
//	[MenuItem("Items/Manager Item")]
//	static void Init () {
//		Debug.Log("Show Panel");
//		itemDatabase = (ItemDatabase)Resources.Load("ItemDatabase", typeof(ItemDatabase)) as ItemDatabase;
//		EditorWindow.GetWindow(typeof(ItemEditorWindow));
//	}
//
//	void OnEnable() {
//		serObj = new SerializedObject(this);
//
//	}
//
//	void OnInspectorUpdate() {
//		itemDatabase = (ItemDatabase)Resources.Load("ItemDatabase", typeof(ItemDatabase)) as ItemDatabase;
//	}
//
//	void OnGUI () {
//		serObj.Update();
//
//		GUILayout.Space(10);
//
//
//		GUI.color = Color.green;
//
//		GUILayout.BeginHorizontal();
//
//		//Create items button
//
//		if(selectedAction == SelectedAction.createItem) {
//			GUI.color = Color.green;
//		}
//		else {
//			GUI.color = Color.white;
//		}
//		if(GUILayout.Button("Create items")) {
//			selectedAction = SelectedAction.createItem;
//		}
//
//		//Manage items button
//
//		if(selectedAction == SelectedAction.manageItems) {
//			GUI.color = Color.green;
//		}
//		else {
//			GUI.color = Color.white;
//		}
//		if(GUILayout.Button("Manage items")) {
//			selectedAction = SelectedAction.manageItems;
//		}
//		GUILayout.EndHorizontal();
//
//		GUILayout.BeginHorizontal();
//
//		//Create crafting items button
//
//		if(selectedAction == SelectedAction.createCraftingItem) {
//			GUI.color = Color.green;
//		}
//		else {
//			GUI.color = Color.white;
//		}
//		if(GUILayout.Button("Create crafted items")) {
//			selectedAction = SelectedAction.createCraftingItem;
//			craftItem = new CraftedItem();
//			craftItem.materialIDs = new System.Collections.Generic.List<int>();
//		}
//
//		//Manage crafting items butotn
//
//		if(selectedAction == SelectedAction.manageCraftingItems) {
//			GUI.color = Color.green;
//		}
//		else {
//			GUI.color = Color.white;
//		}
//		if(GUILayout.Button("Manage crafted items")) {
//			selectedAction = SelectedAction.manageCraftingItems;
//		}
//		GUILayout.EndHorizontal();
//
//		//Reset the gui color
//
//		GUI.color = Color.white;
//
//		GUILayout.Space(25);
//
//
//		if(selectedAction == SelectedAction.createItem) {
//			CreateItem();
//		}
//		else if(selectedAction == SelectedAction.manageItems) {
//			ManageItems();
//		}
//		else if(selectedAction == SelectedAction.createCraftingItem) {
//			CreateCraftingItem();
//		}
//		else if(selectedAction == SelectedAction.manageCraftingItems) {
//			ManageCraftingItems();
//		}
//
//		serObj.ApplyModifiedProperties();
//		
//		if(GUI.changed) {
//			//Apply the changes to the item database
//			EditorUtility.SetDirty(itemDatabase);
//			//Save the item database prefab
//			PrefabUtility.SetPropertyModifications(PrefabUtility.GetPrefabInstanceHandle(itemDatabase), PrefabUtility.GetPropertyModifications(itemDatabase));
//		}
//	}
//
//	void CreateItem() {
//		itemTypeToCreate = (ItemTypeToCreate)EditorGUILayout.EnumPopup("Item type:",itemTypeToCreate);
//		
//		item.itemName = EditorGUILayout.TextField("Item name:",item.itemName);
//		
//		item.width = EditorGUILayout.IntField(new GUIContent("Width:","This controls the width size of the item."), item.width);
//		
//		item.height = EditorGUILayout.IntField(new GUIContent("Height:","This controls the height size of the item."), item.height);
//		
//		item.buyPrice = EditorGUILayout.IntField("Buy price:", item.buyPrice);
//		
//		item.sellPrice = EditorGUILayout.IntField("Sell price:", item.sellPrice);
//		
//		item.itemRatity = (ItemRatity)EditorGUILayout.EnumPopup("Item quality:", item.itemRatity);
//		
//		item.icon = (Sprite)EditorGUILayout.ObjectField("Icon name:", item.icon, typeof(Sprite), false);
//		
//	
//		if(itemTypeToCreate == ItemTypeToCreate.weapon) {
//			item.weaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon type:", item.weaponType);
//			item.itemLevel = EditorGUILayout.IntField("Item level requirement:", item.itemLevel);
//			item.maxDurability = EditorGUILayout.FloatField("Max durability:", item.maxDurability);
//			
//			GUILayout.BeginHorizontal();
//			item.minDamage = EditorGUILayout.FloatField(" min damage:", item.minDamage);
//			item.maxDamage = EditorGUILayout.FloatField(" max damage:", item.maxDamage);
//			GUILayout.EndHorizontal();
//			// item.attackSpeed = EditorGUILayout.FloatField("Attack speed:", item.attackSpeed);
//			
//			GUILayout.BeginHorizontal();
//			item.minStrength = EditorGUILayout.FloatField("Min strength:",item.minStrength);
//			item.maxStrength = EditorGUILayout.FloatField("Max strength:",item.maxStrength);
//			GUILayout.EndHorizontal();
//			
//			GUILayout.BeginHorizontal();
//			item.minDexterity = EditorGUILayout.FloatField("Min dexterity:",item.minDexterity);
//			item.maxDexterity = EditorGUILayout.FloatField("Max dexterity:",item.maxDexterity);
//			GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minVitality = EditorGUILayout.IntField("Min vitality:",item.minVitality);
//			// item.maxVitality = EditorGUILayout.IntField("Max vitality:",item.maxVitality);
//			// GUILayout.EndHorizontal();
//			
//			GUILayout.BeginHorizontal();
//			item.minMagic = EditorGUILayout.FloatField("Min magic:",item.minMagic);
//			item.maxMagic = EditorGUILayout.FloatField("Max magic:",item.maxMagic);
//			GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minArcaneDamage = EditorGUILayout.IntField("Min arcane damage:",item.minArcaneDamage);
//			// item.maxArcaneDamage = EditorGUILayout.IntField("Max arcane damage:",item.maxArcaneDamage);
//			// GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minHolyDamage = EditorGUILayout.IntField("Min holy damage:",item.minHolyDamage);
//			// item.maxHolyDamage = EditorGUILayout.IntField("Max holy damage:",item.maxHolyDamage);
//			// GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minPoisonDamage = EditorGUILayout.IntField("Min poison damage:",item.minPoisonDamage);
//			// item.maxPoisonDamage = EditorGUILayout.IntField("Max poison damage:",item.maxPoisonDamage);
//			// GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minLightningDamage = EditorGUILayout.IntField("Min lightning damage:",item.minLightningDamage);
//			// item.maxLightningDamage = EditorGUILayout.IntField("Max lightning damage:",item.maxLightningDamage);
//			// GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minFrostDamage = EditorGUILayout.IntField("Min frost damage:",item.minFrostDamage);
//			// item.maxFrostDamage = EditorGUILayout.IntField("Max frost damage:",item.maxFrostDamage);
//			// GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minAttackSpeed = EditorGUILayout.IntField("Min attackspeed:",item.minAttackSpeed);
//			// item.maxAttackSpeed = EditorGUILayout.IntField("Max attackspeed:",item.maxAttackSpeed);
//			// GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minCritical = EditorGUILayout.IntField("Min critical percentage:",item.minCritical);
//			// item.maxCritical = EditorGUILayout.IntField("Max critical percentage:",item.maxCritical);
//			// GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minCriticalDamage = EditorGUILayout.IntField("Min critical damage:",item.minCriticalDamage);
//			// item.maxCriticalDamage = EditorGUILayout.IntField("Max critical damage:",item.maxCriticalDamage);
//			// GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minBaseLifeSteal = EditorGUILayout.FloatField("Min lifesteal:",item.minBaseLifeSteal);
//			// item.maxBaseLifeSteal = EditorGUILayout.FloatField("Max lifesteal:",item.maxBaseLifeSteal);
//			// GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minLifePercentage = EditorGUILayout.IntField("Min life percentage:",item.minLifePercentage);
//			// item.maxLifePercentage = EditorGUILayout.IntField("Max life percentage:",item.maxLifePercentage);
//			// GUILayout.EndHorizontal();
//			
//			
//		}
//		else if(itemTypeToCreate == ItemTypeToCreate.armor) {
//			armorTypeToCreate = (ArmorTypeToCreate)EditorGUILayout.EnumPopup("Armor type:", armorTypeToCreate);
//			
//			string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
//			
//			for(int i = 0; i < itemTypes.Length; i++){
//				if(itemTypes[i] == itemTypeToCreate.ToString()) {
//					item.itemType = (EquipmentSlotType)System.Enum.Parse(typeof(EquipmentSlotType), itemTypes[i]);
//					break;
//				}
//			}
//			
//			item.itemLevel = EditorGUILayout.IntField("Item level requirement:", item.itemLevel);
//			
//			item.maxDurability = EditorGUILayout.FloatField("Max durability:", item.maxDurability);
//			
//			GUILayout.BeginHorizontal();
//			item.minArmor = EditorGUILayout.FloatField("Min armor:",item.minArmor);
//			item.maxArmor = EditorGUILayout.FloatField("Max armor:",item.maxArmor);
//			GUILayout.EndHorizontal();
//			
//		
//			GUILayout.BeginHorizontal();
//			item.minStrength = EditorGUILayout.FloatField("Min strength:",item.minStrength);
//			item.maxStrength = EditorGUILayout.FloatField("Max strength:",item.maxStrength);
//			GUILayout.EndHorizontal();
//			
//			GUILayout.BeginHorizontal();
//			item.minDexterity = EditorGUILayout.FloatField("Min dexterity:",item.minDexterity);
//			item.maxDexterity = EditorGUILayout.FloatField("Max dexterity:",item.maxDexterity);
//			GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minVitality = EditorGUILayout.IntField("Min vitality:",item.minVitality);
//			// item.maxVitality = EditorGUILayout.IntField("Max vitality:",item.maxVitality);
//			// GUILayout.EndHorizontal();
//			
//			GUILayout.BeginHorizontal();
//			item.minMagic = EditorGUILayout.FloatField("Min magic:",item.minMagic);
//			item.maxMagic = EditorGUILayout.FloatField("Max magic:",item.maxMagic);
//			GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minHolyResistance = EditorGUILayout.IntField("Min holy resistance:",item.minHolyResistance);
//			// item.minHolyResistance = EditorGUILayout.IntField("Max holy resistance:",item.minHolyResistance);
//			// GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minFrostResistance = EditorGUILayout.IntField("Min frost resistance:",item.minFrostResistance);
//			// item.maxFrostResistance = EditorGUILayout.IntField("Max frost resistance:",item.maxFrostResistance);
//			// GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minArcaneResistance = EditorGUILayout.IntField("Min arcane resistance:",item.minArcaneResistance);
//			// item.maxArcaneResistance = EditorGUILayout.IntField("Max arcane resistance:",item.maxArcaneResistance);
//			// GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minPoisonResistance = EditorGUILayout.IntField("Min poison resistance:",item.minPoisonResistance);
//			// item.maxPoisonResistance = EditorGUILayout.IntField("Max poison resistance:",item.maxPoisonResistance);
//			// GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minAllResistance = EditorGUILayout.IntField("Min all resistance:",item.minAllResistance);
//			// item.maxAllResistance = EditorGUILayout.IntField("Max all resistance:",item.maxAllResistance);
//			// GUILayout.EndHorizontal();
//			
//			// GUILayout.BeginHorizontal();
//			// item.minLifePercentage = EditorGUILayout.IntField("Min life percentage:",item.minLifePercentage);
//			// item.maxLifePercentage = EditorGUILayout.IntField("Max life percentage:",item.maxLifePercentage);
//			// GUILayout.EndHorizontal();
//		}else if(itemTypeToCreate==ItemTypeToCreate.ring){
//			ringType = (RingType)EditorGUILayout.EnumPopup("Ring Type",ringType);
//
//
//			item.useEffectScriptName = EditorGUILayout.TextField("useEffect",item.useEffectScriptName);
//		}
//		else if(itemTypeToCreate == ItemTypeToCreate.other) {
//			otherTypeToCreate = (OtherTypeToCreate)EditorGUILayout.EnumPopup("Item type:", otherTypeToCreate);
//			
//			string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
//			
//			for(int i = 0; i < itemTypes.Length; i++){
//				if(itemTypes[i] == otherTypeToCreate.ToString()) {
//					item.itemType = (EquipmentSlotType)System.Enum.Parse(typeof(EquipmentSlotType), itemTypes[i]);
//					break;
//				}
//			}
//			
//			if(item.itemType == EquipmentSlotType.consumable) {
//				
//				consumableTypeToCreate = (ConsumableTypeToCreate)EditorGUILayout.EnumPopup("Consumable type:", consumableTypeToCreate);
//				
//				item.useEffectScriptName = EditorGUILayout.TextField("Use effect script same:",item.useEffectScriptName);
//				
//				string[] consumableTypes = System.Enum.GetNames (typeof(ConsumableType));
//				
//				for(int j = 0; j < consumableTypes.Length; j++){
//					if(consumableTypes[j] == consumableTypeToCreate.ToString()) {
//						item.consumableType = (ConsumableType)System.Enum.Parse(typeof(ConsumableType), consumableTypes[j]);
//						break;
//					}
//				}
//
//				// item.useSound = (AudioClip)EditorGUILayout.ObjectField("Use sound:", item.useSound,typeof(AudioClip), false);
//				
//				item.stackable = EditorGUILayout.Toggle("Stackable:",item.stackable);
//				
//				if(item.stackable) {
//					item.maxStackSize = EditorGUILayout.IntField("Maximum stacksize:", item.maxStackSize);
//				}
//			}
//
//			if(item.itemType == EquipmentSlotType.reagent) {
//				item.stackable = EditorGUILayout.Toggle("Stackable:",item.stackable);
//				
//				if(item.stackable) {
//					item.maxStackSize = EditorGUILayout.IntField("Maximum stacksize:", item.maxStackSize);
//				}
//			}
//		}
//		
//		item.descriptionText = EditorGUILayout.TextField("Description text:",item.descriptionText);
//		
//		if(GUILayout.Button("Create item")) {
//			item.itemID = itemDatabase.items.Count.ToString();
//			itemDatabase.AddItem(item);
//			item = new Items();
//			UpdateDatabase();
//		}
//	}
//
//	void ManageItems() {
//		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
//		for(int i = 0; i < itemDatabase.items.Count; i++) {
//			if(itemDatabase.items[i] != itemToManage) {
//				GUILayout.BeginHorizontal();
//				if(GUILayout.Button(">","label",GUILayout.Width(10))) {
//					itemToManage = itemDatabase.items[i];
//					string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
//					
//					for(int j = 0; j < itemTypes.Length; j++){
//						if(itemTypes[j] == itemToManage.itemType.ToString()) {
//							if(itemToManage.itemType == EquipmentSlotType.weapon) {
//								itemTypeToCreate =ItemTypeToCreate.weapon;
//								break;
//							}
//							else if(itemToManage.itemType!=EquipmentSlotType.reagent && itemToManage.itemType != EquipmentSlotType.consumable &&
//							itemToManage.itemType != EquipmentSlotType.socket && itemToManage.itemType!= EquipmentSlotType.ring) {
//								// armorTypeToCreate = (ArmorTypeToCreate)System.Enum.Parse(typeof(ArmorTypeToCreate), itemTypes[j]);
//								itemTypeToCreate = ItemTypeToCreate.armor;
//								break;
//							}else if(itemToManage.itemType == EquipmentSlotType.ring){
//								itemTypeToCreate=ItemTypeToCreate.ring;
//								break;
//							}else{
//								//slot3 ring
//								otherTypeToCreate = (OtherTypeToCreate) System.Enum.Parse(typeof(OtherTypeToCreate),itemTypes[i]);
//
//								break;
//							}
//						
//						}
//						
//					}
//				}
//				if(GUILayout.Button(itemDatabase.items[i].itemName)) {
//					itemToManage = itemDatabase.items[i];
//					string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
//					
//					for(int j = 0; j < itemTypes.Length; j++){
//						if(itemTypes[j] == itemToManage.itemType.ToString()) {
//							if(itemToManage.itemType == EquipmentSlotType.weapon) {
//								itemTypeToCreate =ItemTypeToCreate.weapon;
//								break;
//							}
//							else if(itemToManage.itemType!=EquipmentSlotType.reagent && itemToManage.itemType != EquipmentSlotType.consumable &&
//							itemToManage.itemType != EquipmentSlotType.socket) {
//								itemTypeToCreate = ItemTypeToCreate.armor;
//								break;
//							}else if(itemToManage.itemType == EquipmentSlotType.ring){
//								// ringType=(RingType)System.Enum.Parse(typeof(RingType),itemTypes[i]);
//								itemTypeToCreate=ItemTypeToCreate.ring;
//								break;
//							}
//							else{
//								//slot3 ring
//								otherTypeToCreate = (OtherTypeToCreate) System.Enum.Parse(typeof(OtherTypeToCreate),itemTypes[i]);
//								
//								break;
//							}
//						
//						}
//					}
//				}
//				GUI.color = Color.red;
//				if(GUILayout.Button("X",GUILayout.Width(25))) {
//					if(EditorUtility.DisplayDialog("Remove item", "Are you sure you want to remove the item?", "Remove", "Cancel")) {
//						itemDatabase.items.Remove(itemDatabase.items[i]);
//						UpdateDatabase();
//					}
//				}
//				GUILayout.EndHorizontal();
//				GUI.color = Color.white;
//			}
//			else {
//				GUI.color = Color.green;
//				GUILayout.BeginHorizontal();
//				if(GUILayout.Button("v","label",GUILayout.Width(10))) {
//					itemToManage = null;
//					return;
//				}
//				if(GUILayout.Button(itemDatabase.items[i].itemName)) {
//					itemToManage = null;
//					return;
//				}
//				GUI.color = Color.red;
//				if(GUILayout.Button("X",GUILayout.Width(25))) {
//					if(EditorUtility.DisplayDialog("Remove item", "Are you sure you want to remove the item?", "Remove", "Cancel")) {
//						itemDatabase.items.Remove(itemDatabase.items[i]);
//						UpdateDatabase();
//					}
//				}
//				GUILayout.EndHorizontal();
//				GUI.color = Color.white;
//				
//				itemToManage.itemID = EditorGUILayout.TextField("Item ID:", itemToManage.itemID);
//				
//				itemToManage.itemName = EditorGUILayout.TextField("Item name:",itemToManage.itemName);
//				
//				itemToManage.width = EditorGUILayout.IntField(new GUIContent("Width:","This controls the width size of the item."), itemToManage.width);
//				
//				itemToManage.height = EditorGUILayout.IntField(new GUIContent("Height:","This controls the height size of the item."), itemToManage.height);
//				
//				itemToManage.buyPrice = EditorGUILayout.IntField("Buy price:", itemToManage.buyPrice);
//				
//				itemToManage.sellPrice = EditorGUILayout.IntField("Sell price:", itemToManage.sellPrice);
//				
//				itemToManage.itemRatity = (ItemRatity)EditorGUILayout.EnumPopup("Item quality:", itemToManage.itemRatity);
//				
//				itemToManage.icon = (Sprite)EditorGUILayout.ObjectField("Icon name:", itemToManage.icon, typeof(Sprite), false);
//				
//				//Itemtype is weapon
//				if(itemToManage.itemType == EquipmentSlotType.weapon ) {
//					itemToManage.weaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon type:", itemToManage.weaponType);
//					
//					itemToManage.twoHanded = EditorGUILayout.Toggle("Two handed", itemToManage.twoHanded);
//					itemToManage.itemLevel = EditorGUILayout.IntField("Item level requirement:", itemToManage.itemLevel);
//					itemToManage.maxDurability = EditorGUILayout.FloatField("Max durability:", itemToManage.maxDurability);
//					
//					GUILayout.BeginHorizontal();
//					itemToManage.minDamage = EditorGUILayout.FloatField(" min damage:", itemToManage.minDamage);
//					itemToManage.maxDamage = EditorGUILayout.FloatField(" max damage:", itemToManage.maxDamage);
//					itemToManage.damage = EditorGUILayout.FloatField("Damaage",
//						Random.Range(itemToManage.minDamage, itemToManage.maxDamage));
//					GUILayout.EndHorizontal();
//					// itemToManage.attackSpeed = EditorGUILayout.FloatField("Attack speed:", itemToManage.attackSpeed);
//					
//					GUILayout.BeginHorizontal();
//					itemToManage.minStrength = EditorGUILayout.FloatField("Min strength:",itemToManage.minStrength);
//					itemToManage.maxStrength = EditorGUILayout.FloatField("Max strength:",itemToManage.maxStrength);
//					itemToManage.strength = EditorGUILayout.FloatField("strength",
//						Random.Range(itemToManage.minStrength, itemToManage.maxStrength));
//					GUILayout.EndHorizontal();
//					
//					GUILayout.BeginHorizontal();
//					itemToManage.minDexterity = EditorGUILayout.FloatField("Min dexterity:",itemToManage.minDexterity);
//					itemToManage.maxDexterity = EditorGUILayout.FloatField("Max dexterity:",itemToManage.maxDexterity);
//					itemToManage.dexterity = EditorGUILayout.FloatField("dex",
//						Random.Range(itemToManage.minDexterity, itemToManage.dexterity));
//					GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minVitality = EditorGUILayout.IntField("Min vitality:",itemToManage.minVitality);
//					// itemToManage.maxVitality = EditorGUILayout.IntField("Max vitality:",itemToManage.maxVitality);
//					// GUILayout.EndHorizontal();
//					
//					GUILayout.BeginHorizontal();
//					itemToManage.minMagic = EditorGUILayout.FloatField("Min magic:",itemToManage.minMagic);
//					itemToManage.maxMagic = EditorGUILayout.FloatField("Max magic:",itemToManage.maxMagic);
//					itemToManage.magic = EditorGUILayout.FloatField("Magic",
//						Random.Range(itemToManage.minMagic, itemToManage.maxMagic));
//					GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minArcaneDamage = EditorGUILayout.IntField("Min arcane damage:",itemToManage.minArcaneDamage);
//					// itemToManage.maxArcaneDamage = EditorGUILayout.IntField("Max arcane damage:",itemToManage.maxArcaneDamage);
//					// GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minHolyDamage = EditorGUILayout.IntField("Min holy damage:",itemToManage.minHolyDamage);
//					// itemToManage.maxHolyDamage = EditorGUILayout.IntField("Max holy damage:",itemToManage.maxHolyDamage);
//					// GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minPoisonDamage = EditorGUILayout.IntField("Min poison damage:",itemToManage.minPoisonDamage);
//					// itemToManage.maxPoisonDamage = EditorGUILayout.IntField("Max poison damage:",itemToManage.maxPoisonDamage);
//					// GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minLightningDamage = EditorGUILayout.IntField("Min lightning damage:",itemToManage.minLightningDamage);
//					// itemToManage.maxLightningDamage = EditorGUILayout.IntField("Max lightning damage:",itemToManage.maxLightningDamage);
//					// GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minFrostDamage = EditorGUILayout.IntField("Min frost damage:",itemToManage.minFrostDamage);
//					// itemToManage.maxFrostDamage = EditorGUILayout.IntField("Max frost damage:",itemToManage.maxFrostDamage);
//					// GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minAttackSpeed = EditorGUILayout.IntField("Min attackspeed:",itemToManage.minAttackSpeed);
//					// itemToManage.maxAttackSpeed = EditorGUILayout.IntField("Max attackspeed:",itemToManage.maxAttackSpeed);
//					// GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minCritical = EditorGUILayout.IntField("Min critical percentage:",itemToManage.minCritical);
//					// itemToManage.maxCritical = EditorGUILayout.IntField("Max critical percentage:",itemToManage.maxCritical);
//					// GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minCriticalDamage = EditorGUILayout.IntField("Min critical damage:",itemToManage.minCriticalDamage);
//					// itemToManage.maxCriticalDamage = EditorGUILayout.IntField("Max critical damage:",itemToManage.maxCriticalDamage);
//					// GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minBaseLifeSteal = EditorGUILayout.FloatField("Min lifesteal:",itemToManage.minBaseLifeSteal);
//					// itemToManage.maxBaseLifeSteal = EditorGUILayout.FloatField("Max lifesteal:",itemToManage.maxBaseLifeSteal);
//					// GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minLifePercentage = EditorGUILayout.IntField("Min life percentage:",itemToManage.minLifePercentage);
//					// itemToManage.maxLifePercentage = EditorGUILayout.IntField("Max life percentage:",itemToManage.maxLifePercentage);
//					// GUILayout.EndHorizontal();
//					
//				}
//				//Armor 
//				else
//				 if(itemToManage.itemType!=EquipmentSlotType.reagent && itemToManage.itemType != EquipmentSlotType.consumable &&
//							itemToManage.itemType != EquipmentSlotType.socket && itemToManage.itemType != EquipmentSlotType.ring ) {
//					armorTypeToCreate = (ArmorTypeToCreate)EditorGUILayout.EnumPopup("Armor type:", armorTypeToCreate);
//					
//					// string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
//					
//					// for(int j = 0; j < itemTypes.Length; j++){
//					// 	if(itemTypes[j] == armorTypeToCreate.ToString()) {
//					// 		itemToManage.itemType = (EquipmentSlotType)System.Enum.Parse(typeof(EquipmentSlotType), itemTypes[j]);
//					// 		break;
//					// 	}
//					// }
//					
//					itemToManage.itemLevel = EditorGUILayout.IntField("Item level requirement:", itemToManage.itemLevel);
//					itemToManage.maxDurability = EditorGUILayout.FloatField("Max durability:", itemToManage.maxDurability);
//					
//					GUILayout.BeginHorizontal();
//					itemToManage.minArmor = EditorGUILayout.FloatField("Min armor:",itemToManage.minArmor);
//					itemToManage.maxArmor = EditorGUILayout.FloatField("Max armor:",itemToManage.maxArmor);
//					itemToManage.armor = EditorGUILayout.FloatField("Armor",Random.Range( itemToManage.minArmor,itemToManage.maxArmor));
//					GUILayout.EndHorizontal();
//					
//				
//					
//					GUILayout.BeginHorizontal();
//					itemToManage.minDexterity = EditorGUILayout.FloatField("Min dexterity:",itemToManage.minDexterity);
//					itemToManage.maxDexterity = EditorGUILayout.FloatField("Max dexterity:",itemToManage.maxDexterity);
//					itemToManage.dexterity = EditorGUILayout.FloatField("dex",Random.Range( itemToManage.minDexterity,itemToManage.maxDexterity));
//					GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minVitality = EditorGUILayout.IntField("Min vitality:",itemToManage.minVitality);
//					// itemToManage.maxVitality = EditorGUILayout.IntField("Max vitality:",itemToManage.maxVitality);
//					// GUILayout.EndHorizontal();
//					
//					GUILayout.BeginHorizontal();
//					itemToManage.minMagic = EditorGUILayout.FloatField("Min magic:",itemToManage.minMagic);
//					itemToManage.maxMagic = EditorGUILayout.FloatField("Max magic:",itemToManage.maxMagic);
//					itemToManage.magic = EditorGUILayout.FloatField("magic",Random.Range( itemToManage.minArmor,itemToManage.maxArmor));
//					GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minHolyResistance = EditorGUILayout.IntField("Min holy resistance:",itemToManage.minHolyResistance);
//					// itemToManage.minHolyResistance = EditorGUILayout.IntField("Max holy resistance:",itemToManage.minHolyResistance);
//					// GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minFrostResistance = EditorGUILayout.IntField("Min frost resistance:",itemToManage.minFrostResistance);
//					// itemToManage.maxFrostResistance = EditorGUILayout.IntField("Max frost resistance:",itemToManage.maxFrostResistance);
//					// GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minArcaneResistance = EditorGUILayout.IntField("Min arcane resistance:",itemToManage.minArcaneResistance);
//					// itemToManage.maxArcaneResistance = EditorGUILayout.IntField("Max arcane resistance:",itemToManage.maxArcaneResistance);
//					// GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minPoisonResistance = EditorGUILayout.IntField("Min poison resistance:",itemToManage.minPoisonResistance);
//					// itemToManage.maxPoisonResistance = EditorGUILayout.IntField("Max poison resistance:",itemToManage.maxPoisonResistance);
//					// GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minAllResistance = EditorGUILayout.IntField("Min all resistance:",itemToManage.minAllResistance);
//					// itemToManage.maxAllResistance = EditorGUILayout.IntField("Max all resistance:",itemToManage.maxAllResistance);
//					// GUILayout.EndHorizontal();
//					
//					// GUILayout.BeginHorizontal();
//					// itemToManage.minLifePercentage = EditorGUILayout.IntField("Min life percentage:",itemToManage.minLifePercentage);
//					// itemToManage.maxLifePercentage = EditorGUILayout.IntField("Max life percentage:",itemToManage.maxLifePercentage);
//					// GUILayout.EndHorizontal();
//				}
//				else if(itemToManage.itemType == EquipmentSlotType.ring){
//					ringType = (RingType)EditorGUILayout.EnumPopup("ring type",ringType);
//
//					// string[] itemTypes= System.Enum.GetNames(typeof(EquipmentSlotType));
//
//					// for(int j=0;j < itemTypes.Length;j++){
//					// 	if(itemTypes[i] == ringType.ToString()){
//					// 		itemToManage.itemType = (EquipmentSlotType)System.Enum.Parse(typeof(EquipmentSlotType),itemTypes[j]);
//					// 		break;
//					// 	}
//					// }
//
//					//
//					itemToManage.useEffectScriptName = EditorGUILayout.TextField("use Effect",itemToManage.useEffectScriptName);
//				}
//				else {
//					otherTypeToCreate = (OtherTypeToCreate)EditorGUILayout.EnumPopup("Item type:", otherTypeToCreate);
//					
//					string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
//					
//					for(int j = 0; j < itemTypes.Length; j++){
//						if(itemTypes[j] == otherTypeToCreate.ToString()) {
//							itemToManage.itemType = (EquipmentSlotType)System.Enum.Parse(typeof(EquipmentSlotType), itemTypes[j]);
//							break;
//						}
//					}
//					
//					if(itemToManage.itemType == EquipmentSlotType.consumable) {
//						
//						consumableTypeToCreate = (ConsumableTypeToCreate)EditorGUILayout.EnumPopup("Consumable type:", itemToManage.consumableType);
//						
//						itemToManage.useEffectScriptName = EditorGUILayout.TextField("Use effect script same:",itemToManage.useEffectScriptName);
//						
//						string[] consumableTypes = System.Enum.GetNames (typeof(ConsumableType));
//						
//						for(int j = 0; j < consumableTypes.Length; j++){
//							if(consumableTypes[j] == consumableTypeToCreate.ToString()) {
//								itemToManage.consumableType = (ConsumableType)System.Enum.Parse(typeof(ConsumableType), consumableTypes[j]);
//								break;
//							}
//						}
//
//						// itemToManage.useSound = (AudioClip)EditorGUILayout.ObjectField("Use sound:", itemToManage.useSound, typeof(AudioClip), false);
//						
//						itemToManage.stackable = EditorGUILayout.Toggle("Stackable:",itemToManage.stackable);
//						
//						if(itemToManage.stackable) {
//							itemToManage.maxStackSize = EditorGUILayout.IntField("Maximum stacksize:", itemToManage.maxStackSize);
//						}
//					}
//
//					if(itemToManage.itemType == EquipmentSlotType.reagent) {
//						itemToManage.stackable = EditorGUILayout.Toggle("Stackable:",itemToManage.stackable);
//						
//						if(itemToManage.stackable) {
//							itemToManage.maxStackSize = EditorGUILayout.IntField("Maximum stacksize:", itemToManage.maxStackSize);
//						}
//					}
//				}
//				
//				itemToManage.descriptionText = EditorGUILayout.TextField("Description text:",itemToManage.descriptionText);
//				
//			}
//		}
//		GUILayout.EndScrollView();
//	}
//
//	void CreateCraftingItem() {
//		craftItem.item = itemDatabase.FindItem(craftItemID);
//		
//		
//		List<string> itemNames = new List<string>();
//		
//		for(int i = 0; i < itemDatabase.items.Count; i++) {
//			itemNames.Add(itemDatabase.items[i].itemName);
//		}
//		
//		if(!string.IsNullOrEmpty(craftItem.item.itemID)) {
//			craftItemID = int.Parse(craftItem.item.itemID);
//		}
//		
//		craftItemID = EditorGUILayout.Popup("Result item: ", craftItemID, itemNames.ToArray());
//		
//		SerializedProperty prop1 = serObj.FindProperty("craftItem").FindPropertyRelative("materialIDs");
//		EditorGUILayout.PropertyField (prop1,new GUIContent("Required material IDs"),true);
//		SerializedProperty prop2 = serObj.FindProperty("craftItem").FindPropertyRelative("materialRequiredAmount");
//		prop2.arraySize = prop1.arraySize;
//		EditorGUILayout.PropertyField (prop2,new GUIContent("Required amount of materials:"),true);
//		craftItem.craftTime = EditorGUILayout.FloatField("Crafting duration:",craftItem.craftTime);
//		craftItem.craftCost = EditorGUILayout.IntField("Crafting cost:", craftItem.craftCost);
//		craftItem.baseType = (CraftingTabType)EditorGUILayout.EnumPopup("Base type:",craftItem.baseType);
//		GUILayout.Space(15);
//		if(GUILayout.Button("Create item")) {
//			if(craftItem.materialIDs.Count > 0) {
//				for(int i = 0; i < itemDatabase.craftItems.Count; i++) {
//					if(itemDatabase.craftItems[i].item.itemName == craftItem.item.itemName) {
//						EditorUtility.DisplayDialog("Item already exists.", "The item you're trying to add already exists!", "Ok");
//						return;
//					}
//				}
//				craftItem.ID = itemDatabase.craftItems.Count.ToString();
//				for(int i = 0; i < craftItem.materialIDs.Count; i++) {
//					craftItem.materials.Add(itemDatabase.FindItem(craftItem.materialIDs[i]));
//					
//				}
//				itemDatabase.AddCraftItem(craftItem);
//				craftItem = new CraftedItem();
//				UpdateCraftDatabase();
//			}
//			else {
//				EditorUtility.DisplayDialog("Materials error.", "You must add at least 1 material to the list!", "Ok");
//			}
//		}
//	}
//
//	void ManageCraftingItems() {
//			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
//			for(int i = 0; i < itemDatabase.craftItems.Count; i++) {
//				if(itemDatabase.craftItems[i] != crafItemToManage) {
//					GUILayout.BeginHorizontal();
//					if(GUILayout.Button(">","label",GUILayout.Width(10))) {
//						crafItemToManage = itemDatabase.craftItems[i];
//						
//						
//					}
//					if(GUILayout.Button(itemDatabase.craftItems[i].item.itemName)) {
//						crafItemToManage = itemDatabase.craftItems[i];
//						
//					}
//					GUI.color = Color.red;
//					if(GUILayout.Button("X",GUILayout.Width(25))) {
//						if(EditorUtility.DisplayDialog("Remove item", "Are you sure you want to remove the item?", "Remove", "Cancel")) {
//							itemDatabase.craftItems.Remove(itemDatabase.craftItems[i]);
//							UpdateCraftDatabase();
//						}
//					}
//					GUILayout.EndHorizontal();
//					GUI.color = Color.white;
//				}
//				else {
//					GUI.color = Color.green;
//					GUILayout.BeginHorizontal();
//					if(GUILayout.Button("v","label",GUILayout.Width(10))) {
//						crafItemToManage = null;
//						return;
//					}
//					if(GUILayout.Button(itemDatabase.craftItems[i].item.itemName)) {
//						crafItemToManage = null;
//						return;
//					}
//					GUI.color = Color.red;
//					if(GUILayout.Button("X",GUILayout.Width(25))) {
//						if(EditorUtility.DisplayDialog("Remove item", "Are you sure you want to remove the item?", "Remove", "Cancel")) {
//							itemDatabase.craftItems.Remove(itemDatabase.craftItems[i]);
//							UpdateCraftDatabase();
//						}
//					}
//					GUILayout.EndHorizontal();
//					GUI.color = Color.white;
//					
//					List<string> itemNames = new List<string>();
//					
//					for(int j = 0; j < itemDatabase.items.Count; j++) {
//						itemNames.Add(itemDatabase.items[j].itemName);
//					}
//					
//					craftItemID = int.Parse(crafItemToManage.item.itemID);
//					
//					craftItemID = EditorGUILayout.Popup("Result item: ", craftItemID, itemNames.ToArray());
//					
//					crafItemToManage.item = itemDatabase.FindItem(craftItemID);
//					SerializedProperty prop1 = serObj.FindProperty("crafItemToManage").FindPropertyRelative("materialIDs");
//					EditorGUILayout.PropertyField (prop1,new GUIContent("Required material IDs"),true);
//					SerializedProperty prop2 = serObj.FindProperty("crafItemToManage").FindPropertyRelative("materialRequiredAmount");
//					prop2.arraySize = prop1.arraySize;
//					EditorGUILayout.PropertyField (prop2,new GUIContent("Required amount of materials:"),true);
//					crafItemToManage.craftTime = EditorGUILayout.FloatField("Crafting duration:",crafItemToManage.craftTime);
//					crafItemToManage.craftCost = EditorGUILayout.IntField("Crafting cost:", crafItemToManage.craftCost);
//					crafItemToManage.baseType = (CraftingTabType)EditorGUILayout.EnumPopup("Base type:",crafItemToManage.baseType);
//					
//					SerializedProperty prop3 = serObj.FindProperty("crafItemToManage").FindPropertyRelative("materials");
//					prop3.arraySize = prop1.arraySize;
//					for(int j = 0; j < crafItemToManage.materialIDs.Count; j++) {
//						crafItemToManage.materials[j] = itemDatabase.FindItem(crafItemToManage.materialIDs[j]);
//					}
//				}
//			}
//			GUILayout.EndScrollView();
//
//	}
//
//	void UpdateDatabase() {
//		for(int i = 0; i < itemDatabase.items.Count; i++) {
//			itemDatabase.items[i].itemID = i.ToString();
//		}
//	}
//
//	void UpdateCraftDatabase() {
//		for(int i = 0; i < itemDatabase.craftItems.Count; i++) {
//			itemDatabase.craftItems[i].ID = i.ToString();
//		}
//	}
}
