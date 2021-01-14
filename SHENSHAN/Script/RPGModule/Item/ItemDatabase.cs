using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameDataEditor;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 词缀类型
/// </summary>
public enum SABType
{
	STR,
	DEX,
	INT,
}

[System.Serializable]
public class SAB
{
	public SAB(int sabId, string sabName, SABType sabType, int minAmount, int maxAmount,float perc)
	{
		sabID = sabId;
		this.sabName = sabName;
		this.sabType = sabType;
		this.minAmount = minAmount;
		this.maxAmount = maxAmount;
		this.perc = perc;
	}

	public int sabID;
	public string sabName;
	public SABType sabType;
	public int minAmount;
	public int maxAmount;
	public float perc;
	public SAB(){}
}

public class ItemDatabase : MonoBehaviour
{
 	public List<Items> items;
	 public Dictionary<string,Items> itemDic= new Dictionary<string, Items>();
	 	public List<CraftedItem> craftItems;
	public List<Texture2D> SpriteList;
	public List<SAB> sabList;
	
	public static ItemDatabase instance;
	public float sabPerc = 0.4f;
	public float gemPerc = 0.1f;


	//Unknown Item Perc
	public const float rarePerc = 0.6f;
	public const float epicPerc = 0.3f;
	public const float lendPerc = 0.2f;
	public const float ancientPerc = 0.19f;

	//Schema 
	public const float raregSPerc = 0.5f;
	public const float epicSPerc = 0.4f;
	public const float lendSPerc = 0.1f;

	//Gem Slot Perc Limit is 3 
	public const float firstGemSlot = 0.4f;
	public const float secondGemSlot = 0.3f;
	public const float thirdGemSlot = 0.2f;



	void Start()
	{
		if (instance == null) instance = this;
	}
	//Add an item to the list of items
	public void AddItem(Items item) {
		items.Add(item);
	}

	//Find an item based on ID
	public Items FindItem(int id) {

		for(int i = 0; i < items.Count; i++) {
			if(int.Parse(items[i].itemID) == id) {
				return GenerateItem(items[i],true,false);
			}
		}
		return new Items();
	}

	public Items FindItemByName(string name){
			for(int i = 0; i < items.Count; i++) {
			if(items[i].itemName == name) {
				return GenerateItem(items[i],true,false);
			}
		}
		return new Items();
	}
	
	public Items FindItemLoad(int id) {
       Debug.Log("Load Item ");
		for(int i = 0; i < items.Count; i++) {
			if(int.Parse(items[i].itemID) == id) {
				return GenerateItem(items[i],true);
			}
		}
		return new Items();
	}

	public Items GotItem(Items its){
		for(int i=0;i<items.Count;i++){
			if(items[i]==its){
				return items[i];
			}
		}
		return null;
	}

	public Sprite GotItemSprite(Items its){
		for(int i=0;i<items.Count;i++){
			if(items[i]==its){
				return items[i].icon;
			}
		}
		return null;
	}
	public Sprite LoadSprite(Items item){
		Items nItem = InventorySystem.DeepCopy(item);
		
		return nItem.icon;
	}

	public string GetItemid(string id)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].itemID == id)
			{
				Items item = GenerateItem(items[i],true);
				return item.itemID;
			}
		}

		return "";

	}

	public string GetItemName(string id){
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].itemID == id)
			{
				Items item = GenerateItem(items[i],true);
				return item.itemName;
			}
		}

		return "";
	}
	//Add a crafting item to the list
	public void AddCraftItem(CraftedItem item) {
		craftItems.Add(item);
	}

	//Find a crafting item based on ID
	public CraftedItem FindCraftItem(int id) {
		for(int i = 0; i < craftItems.Count; i++) {
			if(int.Parse(craftItems[i].cItemID) == id) {
				return craftItems[i];
			}
		}
		return new CraftedItem();
	}

	public string GetItemSetName(string n)
	{
		var items = GDEDataManager.GetAllItems<GDEItemsData>().ToList();
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].ItemName == n)
			{
				return items[i].SetName;
			}
		}

		return "";

	}

	public Items GetItemByName(string n){
		Debug.Log(n.ToString());
		Items it = new Items();
		if(itemDic.ContainsKey(n)){
			it = itemDic[n];
		}else{
			Debug.Log("No Exist");
		}
		return it;
	}

	public Items GetItemById(string id){

		for (int i=0;i<items.Count;i++){
			if(items[i].itemID == id){
				return items[i];
			}
		}
		return null;
	}

	 public  List<Items> ConvertListItems(List<string> ds){
        List<Items> its = new List<Items>();

       
        if(ds.Count>0){
        for(int i=0;i<ds.Count;i++){
            Debug.Log("try got itemname"+ds[i]);
			Items item =new Items();
             item = GetItemByName(ds[i]);
            if(item!=null){
                its.Add(item);
            }else{
                Debug.Log("Item null");
            }
        }
        }

        return its;

    }
	public Items GenerateItem(Items item,bool isLoad=false,bool startIdentify=false) {
		//Create a copy of the item
		Debug.Log("Generate Items"+item);
		
		
		if (item.unidentified == true&&isLoad==false&&startIdentify==true)
		{
			
			//its the check unknown equipments can unlock slot
			//Gem Slot
//			if (Random.value < gemPerc)
//			{
//				item.hasSlot = true;
//				item.gemSlotNumber = 1;
//			
//			}
			
		#region unknown for equipment rarity
			//Ratity 
			float FV = Random.Range(0.0f,1.0f);
			//new Item
			int RareRV = Random.Range(20006, 20015);
			if (FV > (1 - epicPerc))
			{
				//Got the range of the epic item 
				int EpicRV = Random.Range(22001,22006);
				float SV = Random.Range(0.0f, 1.0f);
				if (SV > (1 - lendPerc))
				{
					//Got lend EquipIndex
					int LendV = Random.Range(23001, 23001);
					float TV = Random.Range(0.0f, 1.0f);
					if (TV > (1 - ancientPerc))
					{
						//Last P Got Ancient Equipment
						int ANV = Random.Range(24001, 24003);
						return GenerateNewItems(ANV);
					}
					else
					{
						return GenerateNewItems(LendV);
					}
				}
				else
				{
					return GenerateNewItems(EpicRV);
				}
			}
			else
			{
//				Items newItems = ItemDatabase.instance.FindItem(RareRV);
//				if (newItems != null)
//				{
//					newItems = InventorySystem.DeepCopy(newItems);
//
//					//Stats
//					item.strength = Random.Range(item.minStrength, item.maxStrength);
//					item.dexterity = Random.Range(item.minDexterity, item.maxDexterity);
//					item.magic = Random.Range(item.minMagic, item.maxMagic);
//
//					//REsistance
//					item.fireResistance = Random.Range(item.minFireRes, item.maxFireRes);
//					item.iceResistance = Random.Range(item.minIceRes, item.maxIceRes);
//					item.posionResistance = Random.Range(item.minPosionRes, item.maxPosionRes);
//					item.phyicsResistance = Random.Range(item.minPhyRes, item.maxPhyRes);
//					item.electronicResistance = Random.Range(item.minElecRes, item.maxElecRes);
//
//					//Armor
//					item.armor = Random.Range(item.minArmor, item.maxArmor);
//					//damage
//					item.damage = Random.Range(item.minDamage, item.maxDamage);
//
//
//					//potion
//					item.healAmount = Random.Range(item.minEffectAmount, item.maxEffectAmount);

//				}
//for detail
				return GenerateNewItems(RareRV);
			}
			#endregion
			
			#region Schema Perc
			
			#endregion
			//Gem Slot 
			#region Gem Slot Perc
			
			#endregion
			
		}
		else
		{
         
			Debug.Log("Try get detail items\t\t"+item.itemName);
			//got from merchant with detail not needs scroll,it load will fixed values

//			item = InventorySystem.DeepCopy(item);
//
////		if (InventorySystem.instance.identifying == true)
////		{
////			Debug.Log("Start identifing und got sab or gem slot");
////			float rnds = Random.Range(0.0f, 1.0f);
////			Debug.Log("ROOL POINTS IST" + rnds);
////			//Check perc
////			if (rnds > 0.4f)
////			{
////				Debug.Log("NORMAL SAB");
////				int rnd = Random.Range(0, sabList.Count);
////				for (int i = 0; i < sabList.Count; i++)
////				{
////					if (sabList[rnd] == sabList[i])
////					{
////						item.sabNames = sabList[rnd].sabName;
////						item.itemName = item.sabNames + item.itemName;
////					}
////
////				}
////			}
////		}
//
//			//Stats
//			item.strength = Random.Range(item.minStrength, item.maxStrength);
//			item.dexterity = Random.Range(item.minDexterity, item.maxDexterity);
//			item.magic = Random.Range(item.minMagic, item.maxMagic);
//
//			//REsistance
//			item.fireResistance = Random.Range(item.minFireRes, item.maxFireRes);
//			item.iceResistance = Random.Range(item.minIceRes, item.maxIceRes);
//			item.posionResistance = Random.Range(item.minPosionRes, item.maxPosionRes);
//			item.phyicsResistance = Random.Range(item.minPhyRes, item.maxPhyRes);
//			item.electronicResistance = Random.Range(item.minElecRes, item.maxElecRes);
//
//			//Armor
//			item.armor = Random.Range(item.minArmor, item.maxArmor);
//			//damage
//			item.damage = Random.Range(item.minDamage, item.maxDamage);
//
//
//			//potion
//			item.healAmount = Random.Range(item.minEffectAmount, item.maxEffectAmount);

			int count=0;
			count++;
if (PlayerPrefs.HasKey("ESTR" + item.itemName + count))
			{
				item.strength = PlayerPrefs.GetInt("ESTR" + item.itemName + count);
			}
			else
			{
				item.strength = Random.Range(item.minStrength, item.maxStrength);
				PlayerPrefs.SetInt("ESTR_" + item.itemName + count,(Mathf.FloorToInt(item.strength)));
			}
			//
			if (PlayerPrefs.HasKey("EDEX_" + item.itemName + count))
			{
				item.dexterity = PlayerPrefs.GetInt("EDEX_" + item.itemName + count);
			}
			else
			{
				item.dexterity = Random.Range(item.minDexterity, item.maxDexterity);
				PlayerPrefs.SetInt("EDEX_" + item.itemName + count,(Mathf.FloorToInt(item.dexterity)));
			}
			//
			if (PlayerPrefs.HasKey("EINTE_" + item.itemName + count))
			{
				item.strength = PlayerPrefs.GetInt("EINTE_" + item.itemName + count);
			}
			else
			{
				item.magic = Random.Range(item.minMagic, item.maxMagic);
				PlayerPrefs.SetInt("EINTE_" + item.itemName + count,(Mathf.FloorToInt(item.magic)));
			}
			
			
			//REsistance
			if (PlayerPrefs.HasKey("EFR_" + item.itemName + count))
			{
				item.fireResistance = PlayerPrefs.GetInt("EFR_" + item.itemName + count);
			}
			else
			{
				item.fireResistance = Random.Range(item.minFireRes, item.maxFireRes);
				PlayerPrefs.SetInt("EFR_" + item.itemName + count,(Mathf.FloorToInt(item.fireResistance)));
			}
			//
			if (PlayerPrefs.HasKey("EIR_" + item.itemName + count))
			{
				item.iceResistance = PlayerPrefs.GetInt("EIR_" + item.itemName + count);
			}
			else
			{
				item.iceResistance = Random.Range(item.minIceRes, item.maxIceRes);
				PlayerPrefs.SetInt("EIR_" + item.itemName + count,(Mathf.FloorToInt(item.iceResistance)));
			}
			//e posion r
			if (PlayerPrefs.HasKey("EPR_" + item.itemName + count))
			{
				item.posionResistance = PlayerPrefs.GetInt("EPR_" + item.itemName + count);
			}
			else
			{
				item.posionResistance = Random.Range(item.minPosionRes, item.maxPosionRes);
				PlayerPrefs.SetInt("EPR_" + item.itemName + count,(Mathf.FloorToInt(item.posionResistance)));
			}
			//e phy r
			if (PlayerPrefs.HasKey("EPHR_" + item.itemName + count))
			{
				item.phyicsResistance = PlayerPrefs.GetInt("EPHR_" + item.itemName + count);
			}
			else
			{
				item.phyicsResistance = Random.Range(item.minPhyRes, item.maxPhyRes);
				PlayerPrefs.SetInt("EPHR_" + item.itemName + count,(Mathf.FloorToInt(item.phyicsResistance)));
			}
			
			//elec re
			if (PlayerPrefs.HasKey("EER_" + item.itemName + count))
			{
				item.electronicResistance = PlayerPrefs.GetInt("EER_" + item.itemName + count);
			}
			else
			{
				item.electronicResistance = Random.Range(item.minElecRes, item.maxElecRes);
				PlayerPrefs.SetInt("EER_" + item.itemName + count,(Mathf.FloorToInt(item.electronicResistance)));
			}
			
			
			
			
			//Armor
			if (PlayerPrefs.HasKey("EDEF_" + item.itemName + count))
			{
				item.electronicResistance = PlayerPrefs.GetInt("EDEF_" + item.itemName + count);
			}
			else
			{
				item.armor = Random.Range(item.minArmor, item.maxArmor);
				PlayerPrefs.SetInt("EDEF_" + item.itemName + count,(Mathf.FloorToInt(item.armor)));
			}
			
			//damage
			if (PlayerPrefs.HasKey("EATK_" + item.itemName + count))
			{
				item.damage = PlayerPrefs.GetInt("EATK_" + item.itemName + count);
			}
			else
			{
				item.damage = Random.Range(item.minDamage, item.maxDamage);
				PlayerPrefs.SetInt("EATK_" + item.itemName + count,(Mathf.FloorToInt(item.damage)));
			}
			//healamount
			if (PlayerPrefs.HasKey("EHEALA_" + item.itemName + count))
			{
				item.healAmount = PlayerPrefs.GetInt("EHEALA_" + item.itemName + count);
			}
			else
			{
				item.healAmount = Random.Range(item.minEffectAmount, item.maxEffectAmount);
				PlayerPrefs.SetInt("EHEALA_" + item.itemName + count,(Mathf.FloorToInt(item.healAmount)));
			}


			return item;
		}

		return null;
	}


	public Items GenerateNewItems(int itemID)
	{
		
		int count = 0;
		
		Items item = ItemDatabase.instance.FindItem(itemID);
		Debug.Log("Rnd Loot Get item Detail"+item.itemName);
		if (item != null)
		{
			count++;
			item = InventorySystem.DeepCopy(item);

			//Stats
			if (PlayerPrefs.HasKey("ESTR" + item.itemName + count))
			{
				item.strength = PlayerPrefs.GetInt("ESTR" + item.itemName + count);
			}
			else
			{
				item.strength = Random.Range(item.minStrength, item.maxStrength);
				PlayerPrefs.SetInt("ESTR_" + item.itemName + count,(Mathf.FloorToInt(item.strength)));
			}
			//
			if (PlayerPrefs.HasKey("EDEX_" + item.itemName + count))
			{
				item.dexterity = PlayerPrefs.GetInt("EDEX_" + item.itemName + count);
			}
			else
			{
				item.dexterity = Random.Range(item.minDexterity, item.maxDexterity);
				PlayerPrefs.SetInt("EDEX_" + item.itemName + count,(Mathf.FloorToInt(item.dexterity)));
			}
			//
			if (PlayerPrefs.HasKey("EINTE_" + item.itemName + count))
			{
				item.strength = PlayerPrefs.GetInt("EINTE_" + item.itemName + count);
			}
			else
			{
				item.magic = Random.Range(item.minMagic, item.maxMagic);
				PlayerPrefs.SetInt("EINTE_" + item.itemName + count,(Mathf.FloorToInt(item.magic)));
			}
			
			
			//REsistance
			if (PlayerPrefs.HasKey("EFR_" + item.itemName + count))
			{
				item.fireResistance = PlayerPrefs.GetInt("EFR_" + item.itemName + count);
			}
			else
			{
				item.fireResistance = Random.Range(item.minFireRes, item.maxFireRes);
				PlayerPrefs.SetInt("EFR_" + item.itemName + count,(Mathf.FloorToInt(item.fireResistance)));
			}
			//
			if (PlayerPrefs.HasKey("EIR_" + item.itemName + count))
			{
				item.iceResistance = PlayerPrefs.GetInt("EIR_" + item.itemName + count);
			}
			else
			{
				item.iceResistance = Random.Range(item.minIceRes, item.maxIceRes);
				PlayerPrefs.SetInt("EIR_" + item.itemName + count,(Mathf.FloorToInt(item.iceResistance)));
			}
			//e posion r
			if (PlayerPrefs.HasKey("EPR_" + item.itemName + count))
			{
				item.posionResistance = PlayerPrefs.GetInt("EPR_" + item.itemName + count);
			}
			else
			{
				item.posionResistance = Random.Range(item.minPosionRes, item.maxPosionRes);
				PlayerPrefs.SetInt("EPR_" + item.itemName + count,(Mathf.FloorToInt(item.posionResistance)));
			}
			//e phy r
			if (PlayerPrefs.HasKey("EPHR_" + item.itemName + count))
			{
				item.phyicsResistance = PlayerPrefs.GetInt("EPHR_" + item.itemName + count);
			}
			else
			{
				item.phyicsResistance = Random.Range(item.minPhyRes, item.maxPhyRes);
				PlayerPrefs.SetInt("EPHR_" + item.itemName + count,(Mathf.FloorToInt(item.phyicsResistance)));
			}
			
			//elec re
			if (PlayerPrefs.HasKey("EER_" + item.itemName + count))
			{
				item.electronicResistance = PlayerPrefs.GetInt("EER_" + item.itemName + count);
			}
			else
			{
				item.electronicResistance = Random.Range(item.minElecRes, item.maxElecRes);
				PlayerPrefs.SetInt("EER_" + item.itemName + count,(Mathf.FloorToInt(item.electronicResistance)));
			}
			
			
			
			
			//Armor
			if (PlayerPrefs.HasKey("EDEF_" + item.itemName + count))
			{
				item.electronicResistance = PlayerPrefs.GetInt("EDEF_" + item.itemName + count);
			}
			else
			{
				item.armor = Random.Range(item.minArmor, item.maxArmor);
				PlayerPrefs.SetInt("EDEF_" + item.itemName + count,(Mathf.FloorToInt(item.armor)));
			}
			
			//damage
			if (PlayerPrefs.HasKey("EATK_" + item.itemName + count))
			{
				item.damage = PlayerPrefs.GetInt("EATK_" + item.itemName + count);
			}
			else
			{
				item.damage = Random.Range(item.minDamage, item.maxDamage);
				PlayerPrefs.SetInt("EATK_" + item.itemName + count,(Mathf.FloorToInt(item.damage)));
			}
			//healamount
			if (PlayerPrefs.HasKey("EHEALA_" + item.itemName + count))
			{
				item.healAmount = PlayerPrefs.GetInt("EHEALA_" + item.itemName + count);
			}
			else
			{
				item.healAmount = Random.Range(item.minEffectAmount, item.maxEffectAmount);
				PlayerPrefs.SetInt("EHEALA_" + item.itemName + count,(Mathf.FloorToInt(item.healAmount)));
			}
			
		}

		return item;
	}
	
	
}
