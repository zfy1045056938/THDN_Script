using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using GameDataEditor;
using System;

public class ItemEditorWindowNew : EditorWindow {

	Items item = new Items();
	public CraftedItem craftItem;
	public TypeOfCards typeofC;
	public Items gdeItems;

	ItemTypeToCreate itemTypeToCreate;

	ArmorTypeToCreate armorTypeToCreate;

	OtherTypeToCreate otherTypeToCreate;


	static CardCollection cardCollection;
	 CardAsset card = new CardAsset() ;
  public CardAsset  managerCard;

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
		LoadItems,
		ClearAllItems,

		CreateCard,
        LoadGdeCard,
		LoadAllCraftItems,
    }

	private SelectedAction selectedAction;

	// Add menu named "My Window" to the Window menu
	[MenuItem ("Window/NewItemsmanager")]
	static void Init () {
		itemDatabase = (ItemDatabase)Resources.Load("ItemDatabase", typeof(ItemDatabase)) as ItemDatabase;
		cardCollection = (CardCollection)Resources.Load("CardAndName", typeof(CardCollection)) as CardCollection;
		GDEDataManager.Init("gde_data");
		
		//
	
		EditorWindow.GetWindow(typeof(ItemEditorWindowNew));
	}

	void OnEnable() {
		serObj = new SerializedObject(this);

	}

	void OnInspectorUpdate() {
		itemDatabase = (ItemDatabase)Resources.Load("ItemDatabase", typeof(ItemDatabase)) as ItemDatabase;
		cardCollection = (CardCollection)Resources.Load("CardAndName", typeof(CardCollection)) as CardCollection;
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

		if(selectedAction == SelectedAction.LoadAllCraftItems) { 
			GUI.color = Color.green;
		}
		else {

			GUI.color = Color.white;
		}

	

		if(GUILayout.Button("Load crafted items")) {
			selectedAction = SelectedAction.LoadAllCraftItems;
		}
		if(GUILayout.Button("Manage crafted items")) {
			selectedAction = SelectedAction.manageCraftingItems;
		}else if (GUILayout.Button("Load All Items"))
		{
			selectedAction = SelectedAction.LoadItems;
		}else if(GUILayout.Button("Clear AllItem")){
			selectedAction = SelectedAction.ClearAllItems;
		}else if(GUILayout.Button("Create Card")){
			selectedAction=SelectedAction.CreateCard;
		}else if(GUILayout.Button("Load GDE Card")){
			selectedAction=SelectedAction.LoadGdeCard;
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
		else if(selectedAction == SelectedAction.createCraftingItem) {
			CreateCraftingItem();
		}
		else if(selectedAction == SelectedAction.manageCraftingItems) {
			ManageCraftingItems();
		
		}else if(selectedAction==SelectedAction.ClearAllItems){
			ClearAllItems();
		}else if(selectedAction==SelectedAction.CreateCard){
			CreateCard();
		}else if(selectedAction ==SelectedAction.LoadGdeCard){
			LoadGDECard();
		}else if(selectedAction == SelectedAction.LoadAllCraftItems) {
			LoadAllCraftItems();
		}

		serObj.ApplyModifiedProperties();
		
		if(GUI.changed) {
			//Apply the changes to the item database
			EditorUtility.SetDirty(itemDatabase);
			//Save the item database prefab
			PrefabUtility.SetPropertyModifications(PrefabUtility.GetPrefabObject(itemDatabase), PrefabUtility.GetPropertyModifications(itemDatabase));
		
			//
			//Apply the changes to the item database
			EditorUtility.SetDirty(cardCollection);
			//Save the item database prefab
			PrefabUtility.SetPropertyModifications(PrefabUtility.GetPrefabObject(cardCollection), PrefabUtility.GetPropertyModifications(cardCollection));
		}
	}
public void LoadAllCraftItems(){
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Load craft items")){
      List<GDECraftItemsData> gde= GDEDataManager.GetAllItems<GDECraftItemsData>();
	  //clear old 
	 
	  //
	for(int i=0;i<gde.Count;i++){
		//
		GDECraftItemsData g = new GDECraftItemsData(gde[i].Key);
		//
		CraftedItem ci = new CraftedItem(){
			item =itemDatabase.GetItemByName(g.cItems),	//convert?
			//
			materials = itemDatabase.ConvertListItems(g.cRequiredItems),
			materialIDs = (g.cItemAmount).ToIntList(),
			materialRequiredAmount =(g.cItemAmount).ToIntList(),
			cItemCost= g.cItemCost,
			cIcon = g.IconName,
			craftPerc=g.craftPerc,
			canSplit= g.CanSplit,	//split to dust 
			splitDust=g.SplitDust,
			hasLearn =g.IsInit,
			baseType = Utils.ConvertCraftType(g.CItemType),

		};

		//Add List
		itemDatabase.AddCraftItem(ci);

	}
		//
		if(itemDatabase.craftItems.Count>0){
			Debug.Log("Craft All CItems"+itemDatabase.craftItems.Count);
		}else{
			Debug.Log("Not Items");
		}

		
		//Update List
		UpdateCraftDatabase();
	  
}
		else if(GUILayout.Button("Clear CI")){
				 if(itemDatabase.craftItems.Count>0){
		  for(int i=0;i<itemDatabase.craftItems.Count;i++){
		  	itemDatabase.craftItems.Remove(itemDatabase.craftItems[i]);
		  }

		  Debug.Log(itemDatabase.craftItems.Count+"Now in list");
	  }
		}
GUILayout.EndHorizontal();
	//
	
	}


  
    
    private void LoadGDECard()
    {

//	    cardCollection.allCardsArray = new List<CardAsset>();
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Load Card")){
      List<GDECardAssetData> allCardData= GDEDataManager.GetAllItems<GDECardAssetData>();
	  Debug.Log("all card gde"+allCardData.Count);

	  for(int i=0;i<allCardData.Count;i++){
		  
		  GDECardAssetData ncard = new GDECardAssetData(allCardData[i].Key);
		  Debug.Log(ncard.CardName+"Load it");
		  CardAsset nItem  = new CardAsset();
		  nItem.name = ncard.CardName;
		  nItem.ratityOption=GetCardRarity(ncard.CardRatity);
		  nItem.tags=ncard.Tag;
		 
 		  nItem.cardDetail = ncard.CardDetail;
 		nItem.cardSprite = Sprite.Create(ncard.CardSprite,new Rect(0,0,ncard.CardSprite.width,ncard.CardSprite.height),Vector3.zero);
 		  nItem.manaCost=ncard.CardMana;
 		  nItem.OverrideLimitOfThisCardsInDeck=ncard.OverrideLimitOfThisCardInDeck;
 		  nItem.typeOfCards=GetTypeOfCard(ncard.CardType);
 		  nItem.characterAsset = GetCardCharacterAsset(nItem,ncard.Character);
		  
 		  //if Creature
 		  nItem.creatureType=GetCreatureType(ncard.CreatureType);
 		  nItem.cardAtk= ncard.CardMinAtk;
 		   nItem.cardDef=ncard.CardMinArmor;
 		    nItem.cardHealth=ncard.CardMinHealth;
 			//
 			//  nItem.STR= ncard.MinStr;
 			//   nItem.DEX=  ncard.MinDex;
 			//    nItem.INT= ncard.MinInt;
 			//    //
 			//     nItem.fireResistance= ncard.MinFireRes;
 			// 	 nItem.iceResistance= ncard.MinIceRes;
 			// 	  nItem.poisonResistance= ncard.MinPoisonRes;
 			// 	   nItem.electronicResistance= ncard.MinElecRes;
 				   //
 				   nItem.creatureScriptName = ncard.CreatureScriptName;
 				   nItem.specialCreatureAmount = ncard.CreatureAmount;
 				     //Spell
 					 nItem.target = GetSpellTarget(ncard.SpellTarget);
                     //
 					 nItem.spellScriptName = ncard.SpellScripName;
 					 nItem.SpecialSpellAmount=ncard.SpecialSpellAmount;
						
 //					 Others,Aura,
// 					 nItem.hasToken =ncard.HasToken;
 					 nItem.tokenCardAsset =ncard.TokenAsset;
					 
 					//  nItem.isEnemyCard =ncard.IsEnemyCard;
					 
 					 nItem.hasDET = ncard.HasDamageEffect;
 					 nItem.damageEType =GetElementalDamageType(ncard.DamageEffectType);
 					 nItem.spellBuffType =GetSpellType(ncard.BuffType);
                     nItem.disType = GetDiscover(ncard.DiscoverType);
                     nItem.artifactType = GetArtifact(ncard.ArtifactType);
					
 					 nItem.hasBuff = ncard.HasBuff;
 					 nItem.hasRound =ncard.HasRound;
                     nItem.RoundTime = ncard.Round;
                    //  nItem.cardFrom = ncard.CardFrom;
                     nItem.taunt = ncard.CardTaunt;

// 					//Generate Items
 					nItem.cardID = cardCollection.allCardsArray.Count;
 				    cardCollection.allCardsArray.Add(nItem);
				
                    
					
 					UpdateCardDB();

	  }
	//  UpdateCardDB();
	  }

	  if(GUILayout.Button("Clear Card")){
		for(int i=0;i<cardCollection.allCardsArray.Count;i++){
			cardCollection.allCardsArray.Remove(cardCollection.allCardsArray[i]);
		}
	  }
		GUILayout.EndHorizontal();
	 scrollPosition= GUILayout.BeginScrollView(scrollPosition);



	 if (cardCollection.allCardsArray!=null)
	 {
		 for (int i = 0; i < cardCollection.allCardsArray.Count; i++)
		 {
			 if (cardCollection.allCardsArray.Count > 0)
			 {
				 if (GUILayout.Button(cardCollection.allCardsArray[i].name))
				 {
					 managerCard = cardCollection.allCardsArray[i];

				 }
			 }
		 }
	 }

	 GUILayout.EndScrollView();
    }

    public DiscoverType GetDiscover(string dis)
    {
	    if (dis == "Rnd")
	    {
		    return DiscoverType.Rnd;
	    }else if (dis == "Oppenent")
	    {
		    return DiscoverType.Oppenent;
	    }else if (dis == "Rarity")
	    {
		    return DiscoverType.Rarity;
	    }

	    return DiscoverType.None;
    }

    public ArtifactType GetArtifact(string at)
    {
	    if (at == "Health")
	    {
		    return ArtifactType.Hp;
	    }else if (at == "Atk")
	    {
		    return ArtifactType.Atk;
	    }else if (at == "GiveCard")
	    {
		    return ArtifactType.GiveCard;
	    }else if(at=="Posion"){
			return ArtifactType.Posion;
		}else if(at=="Token"){
			return ArtifactType.Token;
		}else if(at=="Armor"){
			return ArtifactType.Armor;
		}

	    return ArtifactType.None;
    }

    private SpellBuffType GetSpellType(string spellType)
    {
       
	   if(spellType=="Health"){
		   return SpellBuffType.Health;
	   }else if(spellType =="Armor"){
		   return SpellBuffType.Armor;
	   }else if (spellType == "CharacterArmor")
	   {
		   return SpellBuffType.CharacterArmor;
	   }else if (spellType == "CHArmor")
	   {
		   return SpellBuffType.CharacterArmor;
	   }
	   else if(spellType=="FireArmor"){
		   return SpellBuffType.Atk;
	   }
	   
	   else if(spellType=="Atk"){
		   return SpellBuffType.Atk;
	   }else if(spellType=="STR"){
		   return SpellBuffType.STR;
	   }else if(spellType=="DEX"){
		   return SpellBuffType.DEX;
	   }else if(spellType=="INT"){
		   return SpellBuffType.INT;
	   }else if (spellType == "Taunt")
	   {
		   return SpellBuffType.Taunt;
	   }
	    else if (spellType == "HurtTaunt")
	   {
		   return SpellBuffType.HurtTaunt;
	   }
	    else if (spellType == "TableAmount")
	   {
		   return SpellBuffType.TableAmount;
	   }
	  
	   else if (spellType == "AtkDur")
	   {
		   return SpellBuffType.AtkDur;
	   }else if (spellType == "DoubleAtk")
	   {
		   return SpellBuffType.DoubleAtk;
	   }else if (spellType == "GroupAtk")
	   {
		   return SpellBuffType.GroupAtk;
	   }
	   else if (spellType == "FireRes")
	   {
		   return SpellBuffType.FireRes;
	   }
	   //Res
	   else if (spellType == "IceRes")
	   {
		   return SpellBuffType.IceRes;
	   }
	   else if (spellType == "PosRes")
	   {
		   return SpellBuffType.PosRes;
	   }
	   else if (spellType == "ElecRes")
	   {
		   return SpellBuffType.ElecRes;
	   }

	   
	   else if (spellType =="Machine"){
		   return SpellBuffType.Machine;
	   }else if (spellType =="Hyper"){
		   return SpellBuffType.Hyper;
	   }else if(spellType=="MachineAtk"){
		   return SpellBuffType.MachineAtk;
	   }else if(spellType =="MachineHeal"){
		   return SpellBuffType.MachineHeal;
		   }else if(spellType =="MachineArmor"){
		   return SpellBuffType.MachineArmor;
	   }else if(spellType=="MachineHyper"){
		   return SpellBuffType.MachineHyper;
	   }else if(spellType =="SoulView"){
		   return SpellBuffType.SoulView;
	   }else if(spellType=="Haste"){
		   return SpellBuffType.Haste;
	   }else if(spellType=="Clean"){
		   return SpellBuffType.Clean;
	   }else if(spellType=="Flash"){
		   return SpellBuffType.Flash;
	   }else if(spellType=="ExtraSpell"){
		   return SpellBuffType.ExtraSpell;
	   }

	   return SpellBuffType.None;
    }

    private DamageElementalType GetElementalDamageType(string damageEffectType)
    {
		if(damageEffectType=="Fire"){
			return DamageElementalType.Fire;
		}else if(damageEffectType=="Ice"){
			return DamageElementalType.Ice;
		}else if(damageEffectType=="Damage"){
			return DamageElementalType.Damage;
		}
		else if(damageEffectType=="Posion"){
			return DamageElementalType.Posion;
		}else if(damageEffectType=="Electronic"){
			return DamageElementalType.Electronic;
		}else if (damageEffectType == "Bloody")
		{
			return DamageElementalType.Bloody;
		}else if(damageEffectType=="GroupBloody"){
			return DamageElementalType.GroupBloody;
		}else if (damageEffectType == "Freeze")
		{
			return DamageElementalType.Freeze;
		}else if (damageEffectType == "CantAtk")
		{
			return DamageElementalType.CanAtk;
		}else if (damageEffectType == "MinusAtk")
		{
			return DamageElementalType.MinusAtk;
		}else if (damageEffectType == "Notice")
		{
			return DamageElementalType.Notice;
		}else if (damageEffectType == "HeroElec")
		{
			return DamageElementalType.HeroElec;
		}
		else if (damageEffectType == "TimeBomb")
		{
			return DamageElementalType.TimeBomb;
		}else if(damageEffectType=="HeroDamage"){
			return DamageElementalType.HeroDamage;
		}
		else if(damageEffectType=="HeroHealth"){
			return DamageElementalType.HeroHealth;
		}
		else if(damageEffectType=="HeroArmor"){
			return DamageElementalType.HeroArmor;
		}
		else if(damageEffectType=="ExtraSpell"){
			return DamageElementalType.ExtraSpell;
		}
		//Absorb
		else if(damageEffectType =="Absorb"){
			return DamageElementalType.Absorb;
		}else if(damageEffectType =="AbsorbArmor"){
			return DamageElementalType.AbsorbArmor;
		}else if(damageEffectType =="AbsorbBloody"){
			return DamageElementalType.AbsorbBloody;
		}else if(damageEffectType =="AbsorbFreeze"){
			return DamageElementalType.AbsorbFreeze;
		}else if(damageEffectType =="AbsorbHaste"){
			return DamageElementalType.AbsorbHaste;
		}else if(damageEffectType =="AbsorbPosion"){
			return DamageElementalType.AbsorbPosion;
		}else if(damageEffectType =="AbsorbTreasure"){
			return DamageElementalType.AbsorbTreasure;
		}
       return DamageElementalType.None;
    }

    private CharacterAsset GetCardCharacterAsset(CardAsset c,string characterAsset)
    {
        List<GDECharacterAssetData> gda = GDEDataManager.GetAllItems<GDECharacterAssetData>();
		for(int i=0;i<gda.Count;i++){
	     if(gda[i].ClassName==characterAsset){
			 GDECharacterAssetData cs =new GDECharacterAssetData(gda[i].Key);
			return new  CharacterAsset(GetPlayerJobs(cs.PlayersJob),cs.ClassName,cs.MaxHealth,
			cs.PowerName,GetCsAvaSprite(cs.AvatarImage),cs.Detail,GetCsBGSpritecs(cs.BGSprite),
			cs.AttackCard,cs.ArmorCard);
		 }
		}
		return null;
		
    }

    private Sprite GetCsBGSpritecs(Texture2D bGSprite)
    {
        return Sprite.Create(bGSprite,new Rect(0,0,bGSprite.width,bGSprite.height),Vector2.zero);
    }

    private Sprite GetCsAvaSprite(Texture2D avatarImage)
    {
		if(avatarImage!=null){
        return Sprite.Create(avatarImage,new Rect(0,0,avatarImage.width,avatarImage.height),Vector2.zero);
		}else{
			Debug.Log("Can't Get Sprite");
		}
		return null;
	}

    private PlayerJob GetPlayerJobs(string playersJob)
    {
        if(playersJob=="猎人"){
			return PlayerJob.Hunter;
		}else if(playersJob=="祈求者"){
			return PlayerJob.Magic;
		}else if(playersJob=="生存者"){
			return PlayerJob.Survicer;
		}
		return PlayerJob.None;
    }


    private TargetOptions GetSpellTarget(string spellTarget)
    {
        if(spellTarget=="AllCharacter"){
			return TargetOptions.AllCharacter;
		}else if(spellTarget=="Creature"){
			return TargetOptions.Creature;
		}else if(spellTarget=="EnemyCharacter"){
			return TargetOptions.EmenyCharacter;
		}else if(spellTarget=="EnemyCreature"){
			return TargetOptions.EmenyCreature;

		}else if(spellTarget=="YoursCharacters"){
			return TargetOptions.YoursCharacters;

		}else if(spellTarget=="AllCreature"){
			return TargetOptions.AllCharacter;
		}
		return TargetOptions.None;
    }

    private CardAsset GetTokenCard(string tokenCardAsset)
    {
      return cardCollection.GetCardAssetByName(tokenCardAsset);
    }

    private CardType GetCreatureType(string creatureType)
    {
		if(creatureType=="Animals"){
			return CardType.Animals;
		}else if(creatureType=="Human"){
			return CardType.Human;
		}else if(creatureType=="Qika"){
			return CardType.Qika;
		}else if (creatureType == "Ruiku")
		{
			return CardType.RuiKi;
		}else if(creatureType=="SeaWheel"){
			return CardType.SeaWheel;
		}else if(creatureType=="Kai")
			return CardType.Kai;
       return CardType.None;
    }

    private TypeOfCards GetTypeOfCard(string cardType)
    {
      if(cardType=="Creature"){
		  return TypeOfCards.Creature;
	  }else if(cardType=="Spell"){
		  return TypeOfCards.Spell;
	  }else if(cardType=="Token"){
		  return TypeOfCards.Token;
	  }
	  return TypeOfCards.Creature;
    }

    private CardRatityOption GetCardRarity(string data)
    {
        	
		 if(data == "Rare"){
			return CardRatityOption.RARE;
		}else if(data == "Epic"){
			return CardRatityOption.Epic;
		}else if(data == "Ancient"){
			return CardRatityOption.Ancient;
		}else if(data=="Legend"){
			return CardRatityOption.LEGEND;
		}
		
		return CardRatityOption.NORMAL;
    }

    void CreateCard(){

	
		typeofC = (TypeOfCards)EditorGUILayout.EnumPopup("CardType",typeofC);
		
		card.name =EditorGUILayout.TextField("Card Name",card.name);
		card.tags =EditorGUILayout.TextField("Card Tags",card.tags);
		card.manaCost =EditorGUILayout.IntField("Card Mana",card.manaCost);
			card.ratityOption =(CardRatityOption)EditorGUILayout.EnumPopup("Card Rarity",card.ratityOption);
			card.OverrideLimitOfThisCardsInDeck =EditorGUILayout.IntField("Card Limit",card.OverrideLimitOfThisCardsInDeck);
			card.cardSprite =(Sprite)EditorGUILayout.ObjectField("Card Icon",card.cardSprite,typeof(Sprite),false);
			card.cardDetail = EditorGUILayout.TextField("Card Detail",card.cardDetail);
			
		
// 		card.CharacterAsset=(CharacterAsset)EditorGUILayout.PropertyField("Character Asset",card.CharacterAsset,typeof(CharacterAsset),false);
// item.icon = (Sprite)EditorGUILayout.ObjectField("Icon name:", item.icon, typeof(Sprite), false);
		
		// card.CharacterAsset =EditorGUILayout.TextField("CharacterAsset",card.CharacterAsset);
		if(typeofC==TypeOfCards.Creature){
			card.cardHealth =EditorGUILayout.IntField("CreatureHealth",card.cardHealth);
			card.cardAtk = EditorGUILayout.IntField("Creature Atk",card.cardAtk);
			card.cardDef =EditorGUILayout.IntField("Creature Armor",card.cardDef);
				card.atkForOneTurn =EditorGUILayout.IntField("Atk For One Turn",card.atkForOneTurn);
			card.STR = EditorGUILayout.IntField("Card Str",card.STR);
			card.DEX = EditorGUILayout.IntField("Card Dex",card.DEX);
			card.INT = EditorGUILayout.IntField("Card Inte",card.INT);

			card.creatureType =(CardType)EditorGUILayout.EnumPopup("CreatureType",card.creatureType);

			//boolean
			card.hasDET=EditorGUILayout.Toggle("Has DET",card.hasDET);
			

			card.hasToken=EditorGUILayout.Toggle("Has Token",card.hasToken);

			card.hasBuff =EditorGUILayout.Toggle("HasBuff",card.hasBuff);
			
		
			//creatureScript
			card.creatureScriptName =EditorGUILayout.TextField("Creature Script Name",card.creatureScriptName);
			card.specialCreatureAmount= EditorGUILayout.IntField("Creature Special Amount",card.specialCreatureAmount);

			//others 
			card.isEnemyCard =EditorGUILayout.Toggle("IsEnemyCard",card.isEnemyCard);

			

		}else if(typeofC==TypeOfCards.Spell){
			card.spellScriptName =EditorGUILayout.TextField("Spell Name",card.spellScriptName);
			card.SpecialSpellAmount=EditorGUILayout.IntField("Spell Amount",card.SpecialSpellAmount);
			card.target =(TargetOptions)EditorGUILayout.EnumPopup("Card Target",card.target);
		}

		// Generate Card
		if(GUILayout.Button("Create Card")){
				card.cardID = cardCollection.allCardsArray.Count;
			cardCollection.AddCards(card);
			Debug.Log("Add Success"+card.name);
			UpdateCardDB();
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

		//Check items is unidentfied if true then show unknown items at merchant slot
		item.unidentified = EditorGUILayout.Toggle("Is unidentified",item.unidentified);
		
		item.itemRatity = (ItemRatity)EditorGUILayout.EnumPopup("Item quality:", item.itemRatity);
		item.equipmentSlotype =(EquipmentSlotType)EditorGUILayout.EnumPopup("slot Type",item.equipmentSlotype);
		item.icon = (Sprite)EditorGUILayout.ObjectField("Icon name:", item.icon, typeof(Sprite), false);
		
		// item.itemSound = (AudioClip)EditorGUILayout.ObjectField("Item sound:",item.itemSound, typeof(AudioClip),false);
		
		item.worldObject = (GameObject)EditorGUILayout.ObjectField("World object:", item.worldObject, typeof(GameObject), false);
		
		if(itemTypeToCreate == ItemTypeToCreate.weapon) {
			item.weaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon type:", item.weaponType);
			
			offhandOnly = EditorGUILayout.Toggle("Offhand", offhandOnly);
			if(offhandOnly)
				item.twoHanded = false;
			if(!offhandOnly)
				item.twoHanded = EditorGUILayout.Toggle("Two handed", item.twoHanded);
			item.itemLevel = EditorGUILayout.IntField("Item level requirement:", item.itemLevel);
			item.weaponDur = EditorGUILayout.IntField("Max durability:", item.weaponDur);
			
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
			//Resistance	
			GUILayout.BeginHorizontal();
			item.minFireRes = EditorGUILayout.IntField("Min Fire Res:",item.minFireRes);
			item.maxFireRes = EditorGUILayout.IntField("Max Fire Res:",item.maxFireRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minIceRes = EditorGUILayout.IntField("Min Ice Res:",item.minIceRes);
			item.maxIceRes = EditorGUILayout.IntField("Max Ice Res:",item.maxIceRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minPosionRes = EditorGUILayout.IntField("Min poison Res:",item.minPosionRes);
			item.maxPosionRes = EditorGUILayout.IntField("Max poison Res:",item.maxPosionRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minPhyRes = EditorGUILayout.IntField("Min Physics Res:",item.minPhyRes);
			item.maxPhyRes = EditorGUILayout.IntField("Max Physics Res:",item.maxPhyRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minElecRes = EditorGUILayout.IntField("Min Elec Res:",item.minElecRes);
			item.minElecRes = EditorGUILayout.IntField("Max Elec Res:",item.maxElecRes);
			GUILayout.EndHorizontal();
				
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
			
			//if armor has block chance then set block amount(20% str > target.str)
			if(armorTypeToCreate == ArmorTypeToCreate.offHand) {
				GUILayout.BeginHorizontal();
				item.minBlockChance = EditorGUILayout.IntField("Min block chance:",(int)item.minBlockChance);
				item.maxBlockChance = EditorGUILayout.IntField("Max block chance:",(int)item.maxBlockChance);

				item.minBlockAmount =EditorGUILayout.IntField("Min Block Amount",(int)item.minBlockAmount);
				item.maxBlockAmount =EditorGUILayout.IntField("Max Block Amount",(int)item.maxBlockAmount);
				
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
			//Resistance	
			GUILayout.BeginHorizontal();
			item.minFireRes = EditorGUILayout.IntField("Min Fire Res:",item.minFireRes);
			item.maxFireRes = EditorGUILayout.IntField("Max Fire Res:",item.maxFireRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minIceRes = EditorGUILayout.IntField("Min Ice Res:",item.minIceRes);
			item.maxIceRes = EditorGUILayout.IntField("Max Ice Res:",item.maxIceRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minPosionRes = EditorGUILayout.IntField("Min poison Res:",item.minPosionRes);
			item.maxPosionRes = EditorGUILayout.IntField("Max poison Res:",item.maxPosionRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minPhyRes = EditorGUILayout.IntField("Min Physics Res:",item.minPhyRes);
			item.maxPhyRes = EditorGUILayout.IntField("Max Physics Res:",item.maxPhyRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minElecRes = EditorGUILayout.IntField("Min Elec Res:",item.minElecRes);
			item.minElecRes = EditorGUILayout.IntField("Max Elec Res:",item.maxElecRes);
			GUILayout.EndHorizontal();
		
		}
		else if(itemTypeToCreate == ItemTypeToCreate.other) {
			//consume : stacksize || healamount || healtype ||CoolDown 
			//regrent : 
			//ring : stats || res || dur 
			//Gem : socket || socketType || 
			otherTypeToCreate = (OtherTypeToCreate)EditorGUILayout.EnumPopup("Item type:", otherTypeToCreate);
			
			string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
			
			for(int i = 0; i < itemTypes.Length; i++){
				if(itemTypes[i] == otherTypeToCreate.ToString()) {
					item.itemType = (EquipmentSlotType)System.Enum.Parse(typeof(EquipmentSlotType), itemTypes[i]);
					break;
				}
			}
			
			if(item.itemType == EquipmentSlotType.consumable) {
				
				consumableTypeToCreate = (ConsumableTypeToCreate)EditorGUILayout.EnumPopup("Consumable type:", consumableTypeToCreate);
				
				item.useEffectScriptName = EditorGUILayout.TextField("Use effect script same:",item.useEffectScriptName);
				
				string[] consumableTypes = System.Enum.GetNames (typeof(ConsumableType));
				
				for(int j = 0; j < consumableTypes.Length; j++){
					if(consumableTypes[j] == consumableTypeToCreate.ToString()) {
						item.consumableType = (ConsumableType)System.Enum.Parse(typeof(ConsumableType), consumableTypes[j]);
						break;
					}
				}

				// item.useSound = (AudioClip)EditorGUILayout.ObjectField("Use sound:", item.useSound,typeof(AudioClip), false);
				
				item.stackable = EditorGUILayout.Toggle("Stackable:",item.stackable);
				
				if(item.stackable) {
					item.maxStackSize = EditorGUILayout.IntField("Maximum stacksize:", item.maxStackSize);
				}
			}

			if(item.itemType == EquipmentSlotType.reagent) {
				item.stackable = EditorGUILayout.Toggle("Stackable:",item.stackable);
				
				if(item.stackable) {
					item.maxStackSize = EditorGUILayout.IntField("Maximum stacksize:", item.maxStackSize);
				}
			}


			if(item.itemType==EquipmentSlotType.ring){
				item.ringDur = EditorGUILayout.IntField("Ring Dur",item.ringDur);

					GUILayout.BeginHorizontal();
			item.minDamage = EditorGUILayout.IntField("Minimum  damage:", (int)item.minDamage);
			item.maxDamage = EditorGUILayout.IntField("Maximum  damage:", (int)item.maxDamage);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			item.minArmor = EditorGUILayout.IntField("Minimum  armor:", (int)item.minArmor);
			item.maxArmor = EditorGUILayout.IntField("Maximum  armor:", (int)item.maxArmor);
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
			//Resistance	
			GUILayout.BeginHorizontal();
			item.minFireRes = EditorGUILayout.IntField("Min Fire Res:",item.minFireRes);
			item.maxFireRes = EditorGUILayout.IntField("Max Fire Res:",item.maxFireRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minIceRes = EditorGUILayout.IntField("Min Ice Res:",item.minIceRes);
			item.maxIceRes = EditorGUILayout.IntField("Max Ice Res:",item.maxIceRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minPosionRes = EditorGUILayout.IntField("Min poison Res:",item.minPosionRes);
			item.maxPosionRes = EditorGUILayout.IntField("Max poison Res:",item.maxPosionRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minPhyRes = EditorGUILayout.IntField("Min Physics Res:",item.minPhyRes);
			item.maxPhyRes = EditorGUILayout.IntField("Max Physics Res:",item.maxPhyRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minElecRes = EditorGUILayout.IntField("Min Elec Res:",item.minElecRes);
			item.minElecRes = EditorGUILayout.IntField("Max Elec Res:",item.maxElecRes);
			GUILayout.EndHorizontal();
			if(item.itemRatity!=ItemRatity.Normal || item.itemRatity!=ItemRatity.Junk){
			GUILayout.BeginHorizontal();
			item.useEffectScriptName = EditorGUILayout.TextField("Effect Script Name",item.useEffectScriptName);
			item.minEffectAmount = EditorGUILayout.IntField("min use Effect Amount:",(int)item.minEffectAmount);
			item.maxEffectAmount = EditorGUILayout.IntField("max use Effect Amount:",(int)item.maxEffectAmount);
			item.cooldown = EditorGUILayout.IntField("Cool Down:",(int)item.cooldown);
			GUILayout.EndHorizontal();
			}
			
			
			}
			
			//Gem,can place items has slot 
			if(item.itemType==EquipmentSlotType.socket){
				item.socketType =(SocketType)EditorGUILayout.EnumPopup("SocketType",item.socketType);
			}

			if (item.itemType == EquipmentSlotType.gem)
			{
				item.gemLimit = EditorGUILayout.IntField("Gem Limit", item.gemLimit);
				
				//Common
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
			//Resistance	
			GUILayout.BeginHorizontal();
			item.minFireRes = EditorGUILayout.IntField("Min Fire Res:",item.minFireRes);
			item.maxFireRes = EditorGUILayout.IntField("Max Fire Res:",item.maxFireRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minIceRes = EditorGUILayout.IntField("Min Ice Res:",item.minIceRes);
			item.maxIceRes = EditorGUILayout.IntField("Max Ice Res:",item.maxIceRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minPosionRes = EditorGUILayout.IntField("Min poison Res:",item.minPosionRes);
			item.maxPosionRes = EditorGUILayout.IntField("Max poison Res:",item.maxPosionRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minPhyRes = EditorGUILayout.IntField("Min Physics Res:",item.minPhyRes);
			item.maxPhyRes = EditorGUILayout.IntField("Max Physics Res:",item.maxPhyRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minElecRes = EditorGUILayout.IntField("Min Elec Res:",item.minElecRes);
			item.minElecRes = EditorGUILayout.IntField("Max Elec Res:",item.maxElecRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			item.minArmor = EditorGUILayout.IntField("Min armor:",(int)item.minArmor);
			item.maxArmor = EditorGUILayout.IntField("Max armor:",(int)item.maxArmor);
			GUILayout.EndHorizontal();
			}
	
		}
		
		item.descriptionText = EditorGUILayout.TextField("Description text:",item.descriptionText);
		
		if(GUILayout.Button("Create item")) {
			item.itemID = itemDatabase.items.Count.ToString();
			itemDatabase.AddItem(item);
			item = new Items();
			UpdateDatabase();
		}
	}

	public void LoadAllItems()
	{
		
		
		List<GDEItemsData> allItems = GDEDataManager.GetAllItems<GDEItemsData>();

		for (int i = 0; i < allItems.Count; i++)
		{
			
				GDEItemsData geti = new GDEItemsData(allItems[i].Key);
					Debug.Log(geti.ItemName);
					gdeItems.itemID = geti.ItemID;
					gdeItems.itemName=geti.ItemName;
					gdeItems.width =geti.Width;
					gdeItems.height =geti.Height;
					gdeItems.itemRatity=GetRatity(geti.RatityType);
					Debug.Log("Pre Ge Sprite");
					if(geti.IconName!=null){
					Debug.Log(allItems[i].IconName+"has icon");
					Sprite s = Sprite.Create(geti.IconName, new Rect(0,0,geti.Width,
						geti.Height), Vector2.zero);
					  gdeItems.icon = s;  
					}else {
						//Set Default Icon Name
						Debug.Log("Null Icon Set Default Icon");
						Sprite s = Sprite.Create(geti.IconName,new Rect(0,0,geti.Width,geti.Height),Vector2.zero);
					}
					gdeItems.iconName =  geti.IconSprite;
					//
					gdeItems.descriptionText= geti.ItemDescription;
					gdeItems.itemLevel =geti.ItemLevel;
					// gdeItems.unidentified= geti.Unidentified;
					
					//type to different of the itemType(ItemType und EquipmentSlotType)
					gdeItems.itemType = GetGdeItemType(geti.ItemType);
					
					gdeItems.equipmentSlotype=GetGdeEquipmentSlotType(geti.EquipmentSlotType);
					gdeItems.consumableType =ConvertConable(geti.ConsumableType);
					gdeItems.stackable = geti.Stackable;
					gdeItems.maxStackSize= geti.MaxStackSize;
					gdeItems.stackSize =geti.StackSize;
					// gdeItems.weaponDur =geti.MaxDurablity;
					gdeItems.weaponType = GetGdeWeaponType(geti.WeaponType);
					// gdeItems.sellPrice =geti.SellPrice;
					// gdeItems.buyPrice =geti.BuyPrice;


					//Common 
					gdeItems.minDamage = geti.MinDamage;
					gdeItems.maxDamage= geti.MaxDamage;
					gdeItems.minArmor  =geti.MinArmor;
					gdeItems.maxArmor = geti.MaxArmor;
					//Stats
					// gdeItems.minStrength = geti.MinStrength;
					// gdeItems.maxStrength = geti.MaxStrength;
					// gdeItems.minDexterity = geti.MinDex;
					// gdeItems.maxDexterity =geti.MaxDex;
					// gdeItems.minMagic =geti.MinInte;
					// gdeItems.maxMagic = geti.MaxInte;
					//Res
					gdeItems.minFireRes = geti.MinFireRes;
					gdeItems.maxFireRes =geti.MaxFireRes;
					gdeItems.minIceRes = geti.MinIceRes;
					gdeItems.maxIceRes =geti.MaxIceRes;
					gdeItems.minPosionRes =geti.MinPosionRes;
					gdeItems.maxPosionRes=geti.MaxPoisonRes;
					gdeItems.minPhyRes=geti.MinPhyRes;
					gdeItems.maxPhyRes=geti.MaxPhyRes;
					gdeItems.minElecRes =geti.MinElecRes;
					gdeItems.maxElecRes = geti.MaxElecRes;

					//others
					gdeItems.useEffectScriptName=geti.UseEffectName;
					gdeItems.itemMana = geti.ItemMana;
					gdeItems.cooldown = geti.CoolDown;
					gdeItems.spellTarget = GetSpellTarget(geti.EffectTarget);
					//
					gdeItems.minEffectAmount =geti.MinEffectAmount;
					gdeItems.maxEffectAmount = geti.MaxEffectAmount;
					// gdeItems.gemLimit = geti.GemLimit;
					gdeItems.perc = geti.ItemPerc;
					// gdeItems.useBattle =geti.UseBattle;
					
					//value
					gdeItems.buyPrice = geti.ItemBuy;
					gdeItems.sellPrice = geti.ItemSell;

					//gem
					gdeItems.det =GetElementalDamageType(geti.DET);
					gdeItems.useEffectAmount=geti.UseEffectAmount;

					//series
					gdeItems.hasSet = geti.isSet;
				
					
					// gdeItems.serIDs =geti.SeriesID;

					gdeItems.setList=new List<string>(geti.SeriesID);

					
					gdeItems.setName = geti.SetName;
					gdeItems.counter = geti.Counter;
					
					// gdeItems.setNum = geti.setNum
					
					//Others
					gdeItems.covName = geti.CovName;
					

					


			

			itemDatabase.AddItem(gdeItems);
			//
			if(!itemDatabase.itemDic.ContainsKey(gdeItems.itemName)){
				itemDatabase.itemDic.Add(gdeItems.itemName,gdeItems);
				Debug.Log("add item "+gdeItems.itemName+"add dic");
			}
			gdeItems = new Items();
			
		}



//UpdateDatabase();
	}

    private EquipmentSlotType GetGdeItemType(string itemType)
    {
       if(itemType=="weapon"){
		return EquipmentSlotType.weapon;
	   }else if(itemType=="armor"){
		   return EquipmentSlotType.armor;
	   }else if(itemType=="ring"){
		   return EquipmentSlotType.ring;
	   }else if(itemType =="light"){
		   return EquipmentSlotType.light;
	   }else if(itemType=="magic"){
		   return EquipmentSlotType.magic;
	   }else if(itemType=="heavy"){
		   return EquipmentSlotType.heavy;
	   }else if(itemType=="consumable"){
		   return EquipmentSlotType.consumable;
	   }else if(itemType=="reagent"){
		   return EquipmentSlotType.reagent;
	   }else if(itemType=="offHand"){
		   return EquipmentSlotType.offHand;
	   }else if(itemType=="potion"){
		   return EquipmentSlotType.potion;
	   }
	   return EquipmentSlotType.None;
    }

    private WeaponType GetGdeWeaponType(string weaponType)
    {
    	   return WeaponType.Sword;
	   
    }

    public ConsumableType ConvertConable(string cn)
    {
	    if (cn == "Potion")
	    {
		    return ConsumableType.potion;
	    }else if (cn == "Scroll")
	    {
		    return ConsumableType.Scroll;
	    }

	    return ConsumableType.None;
    }

    private EquipmentSlotType GetGdeEquipmentSlotType(string itemType)
    {
         if(itemType=="weapon"){
		return EquipmentSlotType.weapon;
	   }else if(itemType=="armor"){
		   return EquipmentSlotType.armor;
	   }else if(itemType=="ring"){
		   return EquipmentSlotType.ring;
	   }else if(itemType =="light"){
		   return EquipmentSlotType.light;
	   }else if(itemType=="magic"){
		   return EquipmentSlotType.magic;
	   }else if(itemType=="heavy"){
		   return EquipmentSlotType.heavy;
	   }else if(itemType=="consumable"){
		   return EquipmentSlotType.consumable;
	   }else if(itemType=="reagent"){
		   return EquipmentSlotType.reagent;
	   }else if(itemType=="offHand"){
		   return EquipmentSlotType.offHand;
	   }else if(itemType=="potion"){
		   return EquipmentSlotType.potion;
	   }
	   return EquipmentSlotType.None;
    }

    void ManageItems() {


		if(GUILayout.Button("LoadItems")){
			LoadAllItems();
			Debug.Log("Load Done");
		}
		if(GUILayout.Button("LoadItemSpriteToList")){
			LoadSpriteToList();
			Debug.Log("Load Done");
		}
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
							else if(itemToManage.itemType !=EquipmentSlotType.ring &&
							itemToManage.itemType!=EquipmentSlotType.consumable &&
							itemToManage.itemType!=EquipmentSlotType.reagent && 
							itemToManage.itemType!=EquipmentSlotType.socket && 
							itemToManage.itemType!=EquipmentSlotType.gem
							) {
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

				//Press the button of itemName show itemDetail
				if(GUILayout.Button(itemDatabase.items[i].itemName)) {
					itemToManage = itemDatabase.items[i];
					string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
					
					for(int j = 0; j < itemTypes.Length; j++){
						if(itemTypes[j] == itemToManage.itemType.ToString()) {
							if(itemToManage.itemType == EquipmentSlotType.weapon) {
								break;
							}
							else if(itemToManage.itemType !=EquipmentSlotType.ring &&
							itemToManage.itemType!=EquipmentSlotType.consumable &&
							itemToManage.itemType!=EquipmentSlotType.reagent && 
							itemToManage.itemType!=EquipmentSlotType.socket &&
							itemToManage.itemType!=EquipmentSlotType.gem) {
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
				
				itemToManage.buyPrice = EditorGUILayout.IntField("Buy price:", itemToManage.buyPrice);
				
				itemToManage.sellPrice = EditorGUILayout.IntField("Sell price:", itemToManage.sellPrice);
				
				itemToManage.itemRatity = (ItemRatity)EditorGUILayout.EnumPopup("Item quality:", itemToManage.itemRatity);
				itemToManage.equipmentSlotype =(EquipmentSlotType)EditorGUILayout.EnumPopup("slot Type",itemToManage.equipmentSlotype);
				itemToManage.icon = (Sprite)EditorGUILayout.ObjectField("Icon name:", itemToManage.icon, typeof(Sprite), false);
				
				// itemToManage.itemSound = (AudioClip)EditorGUILayout.ObjectField("Item sound:",itemToManage.itemSound, typeof(AudioClip),false);
				
				itemToManage.worldObject = (GameObject)EditorGUILayout.ObjectField("World object:", itemToManage.worldObject, typeof(GameObject), false);
				
				itemToManage.unidentified  = EditorGUILayout.Toggle("IsUnidentified",itemToManage.unidentified);
				if(itemToManage.itemType == EquipmentSlotType.weapon) {
					itemToManage.weaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon type:", itemToManage.weaponType);
					
					offhandOnly = EditorGUILayout.Toggle("Offhand", offhandOnly);
			if(offhandOnly)
				itemToManage.twoHanded = false;
			if(!offhandOnly)
				itemToManage.twoHanded = EditorGUILayout.Toggle("Two handed", itemToManage.twoHanded);
			itemToManage.itemLevel = EditorGUILayout.IntField("Item level requirement:", itemToManage.itemLevel);
			item.weaponDur = EditorGUILayout.IntField("Max durability:", itemToManage.weaponDur);
			
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
			//Resistance	
			GUILayout.BeginHorizontal();
			itemToManage.minFireRes = EditorGUILayout.IntField("Min Fire Res:",itemToManage.minFireRes);
			itemToManage.maxFireRes = EditorGUILayout.IntField("Max Fire Res:",itemToManage.maxFireRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minIceRes = EditorGUILayout.IntField("Min Ice Res:",itemToManage.minIceRes);
			itemToManage.maxIceRes = EditorGUILayout.IntField("Max Ice Res:",itemToManage.maxIceRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minPosionRes = EditorGUILayout.IntField("Min poison Res:",itemToManage.minPosionRes);
			itemToManage.maxPosionRes = EditorGUILayout.IntField("Max poison Res:",itemToManage.maxPosionRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minPhyRes = EditorGUILayout.IntField("Min Physics Res:",itemToManage.minPhyRes);
			itemToManage.maxPhyRes = EditorGUILayout.IntField("Max Physics Res:",itemToManage.maxPhyRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minElecRes = EditorGUILayout.IntField("Min Elec Res:",itemToManage.minElecRes);
			itemToManage.minElecRes = EditorGUILayout.IntField("Max Elec Res:",itemToManage.maxElecRes);
			GUILayout.EndHorizontal();
				
					
				}
				else if(itemToManage.itemType !=EquipmentSlotType.ring &&
							itemToManage.itemType!=EquipmentSlotType.consumable &&
							itemToManage.itemType!=EquipmentSlotType.reagent && 
							itemToManage.itemType!=EquipmentSlotType.socket &&
							itemToManage.itemType != EquipmentSlotType.gem) {
					armorTypeToCreate = (ArmorTypeToCreate)EditorGUILayout.EnumPopup("Armor type:", armorTypeToCreate);
					
					string[] itemTypes = System.Enum.GetNames (typeof(EquipmentSlotType));
					
					for(int j = 0; j < itemTypes.Length; j++){
						if(itemTypes[j] == armorTypeToCreate.ToString()) {
							itemToManage.itemType = (EquipmentSlotType)System.Enum.Parse(typeof(EquipmentSlotType), itemTypes[j]);
							break;
						}
					}

					//
					GUILayout.BeginHorizontal();
			itemToManage.minArmor = EditorGUILayout.IntField("Minimum  armor:", (int)itemToManage.minArmor);
			itemToManage.maxArmor = EditorGUILayout.IntField("Maximum  armor:", (int)itemToManage.maxArmor);
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
			//Resistance	
			GUILayout.BeginHorizontal();
			itemToManage.minFireRes = EditorGUILayout.IntField("Min Fire Res:",itemToManage.minFireRes);
			itemToManage.maxFireRes = EditorGUILayout.IntField("Max Fire Res:",itemToManage.maxFireRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minIceRes = EditorGUILayout.IntField("Min Ice Res:",itemToManage.minIceRes);
			itemToManage.maxIceRes = EditorGUILayout.IntField("Max Ice Res:",itemToManage.maxIceRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minPosionRes = EditorGUILayout.IntField("Min poison Res:",itemToManage.minPosionRes);
			itemToManage.maxPosionRes = EditorGUILayout.IntField("Max poison Res:",itemToManage.maxPosionRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minPhyRes = EditorGUILayout.IntField("Min Physics Res:",itemToManage.minPhyRes);
			itemToManage.maxPhyRes = EditorGUILayout.IntField("Max Physics Res:",itemToManage.maxPhyRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minElecRes = EditorGUILayout.IntField("Min Elec Res:",itemToManage.minElecRes);
			itemToManage.minElecRes = EditorGUILayout.IntField("Max Elec Res:",itemToManage.maxElecRes);
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
					}

					if (itemToManage.itemType == EquipmentSlotType.gem)
					{
							item.gemLimit = EditorGUILayout.IntField("Gem Limit", itemToManage.gemLimit);
				
				//Common
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
			//Resistance	
			GUILayout.BeginHorizontal();
			itemToManage.minFireRes = EditorGUILayout.IntField("Min Fire Res:",itemToManage.minFireRes);
			itemToManage.maxFireRes = EditorGUILayout.IntField("Max Fire Res:",itemToManage.maxFireRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minIceRes = EditorGUILayout.IntField("Min Ice Res:",itemToManage.minIceRes);
			itemToManage.maxIceRes = EditorGUILayout.IntField("Max Ice Res:",itemToManage.maxIceRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minPosionRes = EditorGUILayout.IntField("Min poison Res:",itemToManage.minPosionRes);
			itemToManage.maxPosionRes = EditorGUILayout.IntField("Max poison Res:",itemToManage.maxPosionRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minPhyRes = EditorGUILayout.IntField("Min Physics Res:",itemToManage.minPhyRes);
			itemToManage.maxPhyRes = EditorGUILayout.IntField("Max Physics Res:",itemToManage.maxPhyRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minElecRes = EditorGUILayout.IntField("Min Elec Res:",itemToManage.minElecRes);
			itemToManage.minElecRes = EditorGUILayout.IntField("Max Elec Res:",itemToManage.maxElecRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minArmor = EditorGUILayout.IntField("Min armor:",(int)itemToManage.minArmor);
			itemToManage.maxArmor = EditorGUILayout.IntField("Max armor:",(int)itemToManage.maxArmor);
			GUILayout.EndHorizontal();
					}
					

					//Type is Ring und show to manager info
					if(itemToManage.itemType ==EquipmentSlotType.ring){
							GUILayout.BeginHorizontal();
			itemToManage.ringDur = EditorGUILayout.IntField("Ring  Dur:", (int)itemToManage.ringDur);

			GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			itemToManage.minDamage = EditorGUILayout.IntField("Minimum  damage:", (int)itemToManage.minDamage);
			itemToManage.maxDamage = EditorGUILayout.IntField("Maximum  damage:", (int)itemToManage.maxDamage);
			GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
			itemToManage.minArmor = EditorGUILayout.IntField("Minimum  armor:", (int)itemToManage.minArmor);
			itemToManage.maxArmor = EditorGUILayout.IntField("Maximum  armor:", (int)itemToManage.maxArmor);
			
			//Resistance	
			GUILayout.BeginHorizontal();
			itemToManage.minFireRes = EditorGUILayout.IntField("Min Fire Res:",itemToManage.minFireRes);
			itemToManage.maxFireRes = EditorGUILayout.IntField("Max Fire Res:",itemToManage.maxFireRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minIceRes = EditorGUILayout.IntField("Min Ice Res:",itemToManage.minIceRes);
			itemToManage.maxIceRes = EditorGUILayout.IntField("Max Ice Res:",itemToManage.maxIceRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minPosionRes = EditorGUILayout.IntField("Min poison Res:",itemToManage.minPosionRes);
			itemToManage.maxPosionRes = EditorGUILayout.IntField("Max poison Res:",itemToManage.maxPosionRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minPhyRes = EditorGUILayout.IntField("Min Physics Res:",itemToManage.minPhyRes);
			itemToManage.maxPhyRes = EditorGUILayout.IntField("Max Physics Res:",itemToManage.maxPhyRes);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			itemToManage.minElecRes = EditorGUILayout.IntField("Min Elec Res:",itemToManage.minElecRes);
			itemToManage.minElecRes = EditorGUILayout.IntField("Max Elec Res:",itemToManage.maxElecRes);
			GUILayout.EndHorizontal();


				if(itemToManage.itemRatity!=ItemRatity.Normal || itemToManage.itemRatity!=ItemRatity.Junk){
			GUILayout.BeginHorizontal();
			itemToManage.useEffectScriptName = EditorGUILayout.TextField("Effect Script Name",itemToManage.useEffectScriptName);
			itemToManage.minEffectAmount = EditorGUILayout.IntField("min use Effect Amount:",(int)itemToManage.minEffectAmount);
			itemToManage.maxEffectAmount = EditorGUILayout.IntField("max use Effect Amount:",(int)itemToManage.maxEffectAmount);
			itemToManage.cooldown = EditorGUILayout.IntField("Cool Down:",(int)item.cooldown);
			GUILayout.EndHorizontal();
			}


					}
		
						//Socket 
						if(itemToManage.itemType == EquipmentSlotType.socket){
							itemToManage.socketType =(SocketType)EditorGUILayout.EnumPopup("SocketType",itemToManage.socketType);
						}
				}
				
				itemToManage.descriptionText = EditorGUILayout.TextField("Description text:",itemToManage.descriptionText);
				
			}
		}
		GUILayout.EndScrollView();
	}

	void LoadSpriteToList(){
		itemDatabase.SpriteList = new List<Texture2D>();
		//
		Texture2D[] imageDB = Resources.LoadAll<Texture2D>("");
		if(imageDB.Length>0){
				for(int i=0;i<imageDB.Length;i++){
					itemDatabase.SpriteList.Add(imageDB[i]);
				}
		}
		Debug.Log(itemDatabase.SpriteList.Count+"has been add todb");
	}

	void CreateCraftingItem() {
		craftItem.item = itemDatabase.FindItem(craftItemID);
		
		
		List<string> itemNames = new List<string>();
		
		for(int i = 0; i < itemDatabase.items.Count; i++) {
			itemNames.Add(itemDatabase.items[i].itemName);
		}
		
		if(!string.IsNullOrEmpty(craftItem.item.itemID)) {
			craftItemID = int.Parse(craftItem.item.itemID);
		}
		
		craftItemID = EditorGUILayout.Popup("Result item: ", craftItemID, itemNames.ToArray());
		
		SerializedProperty prop1 = serObj.FindProperty("craftItem").FindPropertyRelative("materialIDs");
		EditorGUILayout.PropertyField (prop1,new GUIContent("Required material IDs"),true);
		SerializedProperty prop2 = serObj.FindProperty("craftItem").FindPropertyRelative("materialRequiredAmount");
		prop2.arraySize = prop1.arraySize;
		EditorGUILayout.PropertyField (prop2,new GUIContent("Required amount of materials:"),true);
		// craftItem.craftTime = EditorGUILayout.FloatField("Crafting duration:",craftItem.craftTime);
		// craftItem.craftCost = EditorGUILayout.IntField("Crafting cost:", craftItem.craftCost);
		craftItem.baseType = (CraftingTabType)EditorGUILayout.EnumPopup("Base type:",craftItem.baseType);
		GUILayout.Space(15);
		if(GUILayout.Button("Create item")) {
			if(craftItem.materialIDs.Count > 0) {
				for(int i = 0; i < itemDatabase.craftItems.Count; i++) {
					if(itemDatabase.craftItems[i].item.itemName == craftItem.item.itemName) {
						EditorUtility.DisplayDialog("Item already exists.", "The item you're trying to add already exists!", "Ok");
						return;
					}
				}
				craftItem.cItemID = itemDatabase.craftItems.Count.ToString();
				for(int i = 0; i < craftItem.materialIDs.Count; i++) {
					craftItem.materials.Add(itemDatabase.FindItem(craftItem.materialIDs[i]));
					
				}
				itemDatabase.AddCraftItem(craftItem);
				craftItem = new CraftedItem();
				UpdateCraftDatabase();
			}
			else {
				EditorUtility.DisplayDialog("Materials error.", "You must add at least 1 material to the list!", "Ok");
			}
		}
	}

	void ManageCraftingItems() {
			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			for(int i = 0; i < itemDatabase.craftItems.Count; i++) {
				if(itemDatabase.craftItems[i] != crafItemToManage) {
					GUILayout.BeginHorizontal();
					if(GUILayout.Button(">","label",GUILayout.Width(10))) {
						crafItemToManage = itemDatabase.craftItems[i];
						
						
					}
					if(GUILayout.Button(itemDatabase.craftItems[i].item.itemName)) {
						crafItemToManage = itemDatabase.craftItems[i];
						
					}
					GUI.color = Color.red;
					if(GUILayout.Button("X",GUILayout.Width(25))) {
						if(EditorUtility.DisplayDialog("Remove item", "Are you sure you want to remove the item?", "Remove", "Cancel")) {
							itemDatabase.craftItems.Remove(itemDatabase.craftItems[i]);
							UpdateCraftDatabase();
						}
					}
					GUILayout.EndHorizontal();
					GUI.color = Color.white;
				}
				else {
					GUI.color = Color.green;
					GUILayout.BeginHorizontal();
					if(GUILayout.Button("v","label",GUILayout.Width(10))) {
						crafItemToManage = null;
						return;
					}
					if(GUILayout.Button(itemDatabase.craftItems[i].item.itemName)) {
						crafItemToManage = null;
						return;
					}
					GUI.color = Color.red;
					if(GUILayout.Button("X",GUILayout.Width(25))) {
						if(EditorUtility.DisplayDialog("Remove item", "Are you sure you want to remove the item?", "Remove", "Cancel")) {
							itemDatabase.craftItems.Remove(itemDatabase.craftItems[i]);
							UpdateCraftDatabase();
						}
					}
					GUILayout.EndHorizontal();
					GUI.color = Color.white;
					
					List<string> itemNames = new List<string>();
					
					for(int j = 0; j < itemDatabase.items.Count; j++) {
						itemNames.Add(itemDatabase.items[j].itemName);
					}
					
					craftItemID = int.Parse(crafItemToManage.item.itemID);
					
					craftItemID = EditorGUILayout.Popup("Result item: ", craftItemID, itemNames.ToArray());
					
					crafItemToManage.item = itemDatabase.FindItem(craftItemID);
					SerializedProperty prop1 = serObj.FindProperty("crafItemToManage").FindPropertyRelative("materialIDs");
					EditorGUILayout.PropertyField (prop1,new GUIContent("Required material IDs"),true);
					SerializedProperty prop2 = serObj.FindProperty("crafItemToManage").FindPropertyRelative("materialRequiredAmount");
					prop2.arraySize = prop1.arraySize;
					EditorGUILayout.PropertyField (prop2,new GUIContent("Required amount of materials:"),true);
					// crafItemToManage.craftTime = EditorGUILayout.FloatField("Crafting duration:",crafItemToManage.craftTime);
					// crafItemToManage.craftCost = EditorGUILayout.IntField("Crafting cost:", crafItemToManage.craftCost);
					crafItemToManage.baseType = (CraftingTabType)EditorGUILayout.EnumPopup("Base type:",crafItemToManage.baseType);
					
					SerializedProperty prop3 = serObj.FindProperty("crafItemToManage").FindPropertyRelative("materials");
					prop3.arraySize = prop1.arraySize;
					for(int j = 0; j < crafItemToManage.materialIDs.Count; j++) {
						crafItemToManage.materials[j] = itemDatabase.FindItem(crafItemToManage.materialIDs[j]);
					}
				}
			}
			GUILayout.EndScrollView();

	}

	

	void UpdateCraftDatabase() {
		for(int i = 0; i < itemDatabase.craftItems.Count; i++) {
			itemDatabase.craftItems[i].cItemID = i.ToString();
		}
	}
	void UpdateDatabase() {
		Debug.Log(itemDatabase.items.Count+"In DB");
		for(int i = 0; i < itemDatabase.items.Count; i++) {
			itemDatabase.items[i].itemID = i.ToString();
		}
	}

	void UpdateCardDB(){
			for(int i = 0; i < cardCollection.allCardsArray.Count; i++) {
			cardCollection.allCardsArray[i].cardID = i;
		}
	}
	#region  Convert GDE String To Enum
	public ItemRatity GetRatity(string data){

		
		if(data=="Junk"){
			return ItemRatity.Junk;
		
		}else if(data == "Rare"){
			return ItemRatity.Rare;
		}else if(data == "Epic"){
			return ItemRatity.Epic;
		}else if(data == "Ancient"){
			return ItemRatity.Ancient;
		}
		return ItemRatity.Normal;
	}

	#endregion
}