using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using GameDataEditor;

using System.Linq;

namespace PixelCrushers.DialogueSystem{
public class TooltipManager : MonoBehaviour {

   public GameObject tooltip;
   public GameObject itemTp;
   public GameObject normalTp;
   //Pos
   public GameObject equipDetail;//equipment
   public GameObject OthersDetail;//except equipment

   public GameObject weaponObj;
   public GameObject armorObj;
   public GameObject setObj;
   
	public float tooltipMainHeight;
	public Text tooltipText;
	public Text tooltipHeaderText;
	public Text tooltipSellValueText;
	public Text tooltipSellValueTextLabel;
	public Text tooltipDescriptionText;
	public Text tooltipRequiredLevelText;
	public Text tooltipRatityText;
	public Image tooltipImage;
	public Text armorText;
	[Header("Resistance")] public Text frText;
	public Text irText;
	public Text poRText;
	public Text ERText;
	
	public Text baseValueText;
	public Text baseDurText;

	public Text DescriptionText;
	public Text ItemScoreText;
	//public  Text StrengthText;
	//public Text DexText;
	//public Text MagicText;
	public Text specialText;
	public Image tooltipHeader;
	public Color attributeColor;
	public Text otherDesText;
	

	//Set
	public TextMeshProUGUI setText;
	//TODO
	public List<GameObject> setDetail;
	public List<GameObject> setEquipment;  
	public int setNum;
	public GameObject setTip;
	public Transform setListPos;
	public Transform setDetailPos;

	public GameObject setListObj;
	public GameObject setDetailObj;



	public int totalSetNum;
	public TextMeshProUGUI numText;
	public float tooltipDelay;
	public TextTable table;

	public GameObject ValuePanel;

	public static TooltipManager instance;
	
	[HideInInspector]
	public bool showTooltip;

	void Awake(){
		if(instance==null)instance=this;
	}
	//Calculate and show the tooltip
	public IEnumerator ShowTooltip(bool right, Items item, SlotType type, int startNumber, RectTransform trans, bool merchant) {

		showTooltip = true;

		if(right) {
			tooltip.GetComponent<RectTransform>().pivot = new Vector2(1,1);
		}
		else {
			tooltip.GetComponent<RectTransform>().pivot = new Vector2(0,1);
		}

		yield return new WaitForSeconds(tooltipDelay);

		if (showTooltip)
		{
			foreach (Transform t in transform)
			{
				t.gameObject.SetActive(true);
			}
			//Extra
			// tooltipText.text = GenerateTooltip(item);

			//
			tooltip.gameObject.SetActive(true);
			tooltipHeader.color = FindColor(item);
			tooltipHeaderText.text = item.itemName.ToString();
			//

			if (!item.unidentified)
			{
				equipDetail.gameObject.SetActive(true);
				tooltipDescriptionText.text = item.descriptionText;


				if (tooltip && type != SlotType.Card)
				{
					ItemScoreText.text = item.score.ToString();

					tooltipRatityText.text = LoadRatityLoC(item);
					tooltipRequiredLevelText.text = "<size=10>需要等级: " + item.itemLevel.ToString() + "</size>";

					//
					// StrengthText.text = Mathf.RoundToInt(item.strength).ToString();
					// DexText.text = Mathf.RoundToInt(item.dexterity).ToString();
					// MagicText.text = Mathf.RoundToInt(item.magic).ToString();

					//
					frText.text = item.fireResistance.ToString();
					irText.text = item.iceResistance.ToString();
					poRText.text = item.posionResistance.ToString();
					ERText.text = item.electronicResistance.ToString();

//
					if (item.itemType == EquipmentSlotType.weapon)
					{
						weaponObj.gameObject.SetActive(true);
						armorObj.gameObject.SetActive(false);
						equipDetail.gameObject.SetActive(true);
						OthersDetail.gameObject.SetActive(false);
						baseValueText.text = Mathf.FloorToInt(item.damage).ToString();
						baseDurText.text = Mathf.FloorToInt(item.weaponDur).ToString();
					}
					else if (item.itemType == EquipmentSlotType.armor)
					{
						weaponObj.gameObject.SetActive(false);
						armorObj.gameObject.SetActive(true);
						equipDetail.gameObject.SetActive(true);
						OthersDetail.gameObject.SetActive(false);
						armorText.text = Mathf.FloorToInt(item.armor).ToString();
						baseDurText.text = item.armorDur.ToString();
					}
					else if (item.itemType == EquipmentSlotType.ring)
					{
						equipDetail.gameObject.SetActive(true);
						OthersDetail.gameObject.SetActive(false);
						baseValueText.text = item.armor.ToString();
						baseDurText.text = item.ringDur.ToString();
					}
					else if (item.itemType != EquipmentSlotType.weapon || item.itemType != EquipmentSlotType.armor &&
					         item.itemType != EquipmentSlotType.ring)
					{
						equipDetail.gameObject.SetActive(false);
						OthersDetail.gameObject.SetActive(true);
						otherDesText.text = item.descriptionText;
					}
				}
				else
				{
					//Show Pack note
					tooltipDescriptionText.text = item.descriptionText;

				}


//			tooltipRequiredLevelText.rectTransform.sizeDelta = new Vector2(tooltipRequiredLevelText.preferredWidth, tooltipRequiredLevelText.preferredHeight);

				// tooltipText.rectTransform.sizeDelta = new Vector2(tooltipText.preferredWidth-2, tooltipText.preferredHeight + 4);
				// tooltipDescriptionText.rectTransform.sizeDelta = new Vector2(tooltipRequiredLevelText.preferredWidth, tooltipDescriptionText.preferredHeight);

				// tooltip.GetComponent<RectTransform>().sizeDelta = new Vector2(tooltipRequiredLevelText.rectTransform.sizeDelta.x + 200, 
				// tooltipMainHeight + tooltipText.rectTransform.sizeDelta.y + tooltipDescriptionText.rectTransform.sizeDelta.y + tooltipRequiredLevelText.rectTransform.sizeDelta.y);

			}
			else
			{
				ItemScoreText.text = "???";
				//StrengthText.text = "???";
				//DexText.text = "???";
				//MagicText.text = "???";
				baseDurText.text = "???";
				baseValueText.text = "???";
				frText.text = "???";
				irText.text = "???";
				poRText.text = "???";
				ERText.text = "???";
				DescriptionText.text = "未鉴定物品";
				tooltipRatityText.text = "???";
//				tooltipHeaderText.color = FindColor(item);
				specialText.text = "???";
				OthersDetail.gameObject.SetActive(false);
			}


			// Debug.Log("Check Set Item");
			// //IF set show Set
			// if(item.hasSet==true){
			// 	setObj.gameObject.SetActive(true);
			// 	setText.text =  item.setName.ToString();
			// 	totalSetNum = InventorySystem.instance.setNum;
			// 	
			// 		//Clear Old obj
			// 		foreach(var o in setDetail ){
			// 			if(o!=null){
			// 				Destroy(o);
			// 			}
			// 		}
			// 			foreach(var o in setEquipment ){
			// 			if(o!=null){
			// 				Destroy(o);
			// 			}
			// 		}
			//
			//
			// 						//Show Set Item Name equals set Item Id as 
			// 						for(int i=0;i<item.setList.Count;i++){
			// 								setTip = Instantiate(setListObj) as GameObject;
			// 								setTip.transform.SetParent(setListPos);
			// 							
			// 								setTip.GetComponent<SetObj>().setName.text = ItemDatabase.instance.GetItemName(item.setList[i]);
			// 								setTip.GetComponent<SetObj>().setName.color = Color.grey;
			//
			// 								if (setTip.GetComponent<SetObj>().setName.text == item.itemName)
			// 								{
			// 									setTip.GetComponent<SetObj>().setName.color = Color.white;
			// 								}
			//
			// 								
			// 								
			// 								setEquipment.Add(setTip);
			// 						}
			//
			// 						//Got SetName
			// 						Debug.Log("Got set list detail");
			// 					List<GDESetListData> setList = GDEDataManager.GetAllItems<GDESetListData>();
			// 					
			// 						for(int i=0;i<setList.Count;i++){
			// 							if(item.setName== setList[i].SetListName ){
			// 								//has targe setlist
			// 								Debug.Log("Got listname");
			// 								//Got SetList Detail
			// 								
			// 									setDetailObj = Instantiate(setDetailObj) as GameObject;
			// 									setDetailObj.transform.localScale= new Vector3(1,1,1);
			// 									setDetailObj.transform.SetParent(setDetailPos);
			// 									setDetailObj.GetComponent<SetObj>().setDetail.text =
			// 										setList[i].SetEffect1.ToString();
			// 									setDetailObj.GetComponent<SetObj>().setDetail.color = Color.grey;
			// 									setDetail.Add(setDetailObj);
			//
			//
			//
			// 									setDetailObj = Instantiate(setDetailObj) as GameObject;
			// 									setDetailObj.transform.localScale= new Vector3(1,1,1);
			// 									setDetailObj.transform.SetParent(setDetailPos);
			// 									setDetailObj.GetComponent<SetObj>().setDetail.text =
			// 										setList[i].SetEffect2.ToString();
			// 									setDetailObj.GetComponent<SetObj>().setDetail.color = Color.gray;
			// 										
			// 									setDetail.Add(setDetailObj);
			//
			// 										if(totalSetNum>=2){
			// 						foreach(var v in setDetail){
			// 							if(v!=null && v.GetComponent<SetObj>().setDetail.text==setList[i].SetEffect1){
			// 								setDetailObj.GetComponent<SetObj>().setDetail.color = Color.white;
			// 									
			// 							}
			// 							
			// 							
			// 						}
			// 										}
			// 							else if(totalSetNum>=3){
			// 						foreach(var v in setDetail){
			// 							
			// 								setDetailObj.GetComponent<SetObj>().setDetail.color = Color.white;
			// 					
			// 						}
			// 					}
			//
			// 										for (int m = 0; m < setEquipment.Count; m++)
			// 										{
			// 											string setName =
			// 												ItemDatabase.instance.GetItemSetName(setEquipment[m]
			// 													.GetComponent<SetObj>().setName.text);
			// 											//
			// 											if (setName == InventorySystem.instance.setName)
			// 											{
			// 												setEquipment[m].GetComponent<SetObj>().setName.color =
			// 													Color.white;
			// 											}
			// 										}
			// 							
			// 							
			// 							
			// 							//show active set detail by setnum from inventory
			// 							// UpdateSetDetail( setNum);
			// 					
			// 					//Check Both
			// 					// var eq = InventorySystem.instance.equipmentSlots.ToList();
			// 					// for (int i=0;i<eq.Count;i++){
			// 					// 	if(eq[i].item.setName== InventorySystem.instance.setName){
			// 					// 		//
			// 					// 		foreach(var v in setEquipment){
			// 					// 			if(v.GetComponent<SetObj>().setName.text==eq[i].item.itemName){
			// 					// 				v.GetComponent<SetObj>().setName.color=Color.white;	
			// 					// 			}
			// 					// 		}
			// 					// 	}
			// 					// }
			// 					//
			// 					//
			// 				
			// 					
			// 							}
			// 						}
			// 				
			// 				
			// }

			if (merchant)
			{
				ValuePanel.gameObject.SetActive(true);
				tooltipSellValueText.text = item.buyPrice.ToString();
				tooltipSellValueTextLabel.text = "购买价格:";
			}
			else
			{
				ValuePanel.gameObject.SetActive(false);
			}
		

//			tooltipSellValueText.rectTransform.sizeDelta = new Vector2(tooltipSellValueText.preferredWidth, tooltipSellValueText.rectTransform.sizeDelta.y);

			RectTransform rect = tooltip.GetComponent<RectTransform>();
			
			tooltip.transform.position = trans.position;


			//Make sure the tooltip is at the correct side of the screen
			if(right) {
				tooltip.GetComponent<RectTransform>().pivot = new Vector2(1,1);
				if(type == SlotType.Equipment) {
					tooltip.transform.localPosition -= new Vector3(trans.sizeDelta.x*0.1f , -trans.sizeDelta.y*-0.3f );
				}else if (type == SlotType.Merchant)
				{
					tooltip.transform.localPosition -= new Vector3(-500f , 300f,0f );
				}
			}
			else {
				tooltip.GetComponent<RectTransform>().pivot = new Vector2(0,1);
				if(type == SlotType.Crafting) {
				}
				else {
					tooltip.transform.localPosition += new Vector3(trans.sizeDelta.x * item.width,0);

				}
			}

			//Make sure that the tooltip doesn't leave the screen
			if(rect.localPosition.y < 0) {
				if(Mathf.Abs(rect.localPosition.y) + rect.sizeDelta.y > transform.root.GetComponent<CanvasScaler>().referenceResolution.y * 0.20f) {
					rect.localPosition -= new Vector3(0, transform.root.GetComponent<CanvasScaler>().referenceResolution.y * 0.20f - (Mathf.Abs(rect.localPosition.y) + rect.sizeDelta.y),0);
				}
			}
			else {
				if(Mathf.Abs(rect.sizeDelta.y - Mathf.Abs(rect.localPosition.y)) > transform.root.GetComponent<CanvasScaler>().referenceResolution.y * 0.20f) {
					rect.localPosition += new Vector3(0, Mathf.Abs(rect.sizeDelta.y - Mathf.Abs(rect.localPosition.y)) - transform.root.GetComponent<CanvasScaler>().referenceResolution.y * 0.20f,0);
				}
			}
		}
		else {
			HideTooltip();
		}
			
	}

	public void UpdateSetDetail(int num){
		var setList  = GDEDataManager.GetAllItems<GDESetListData>().ToList();

		// if(num>=2){
		// 	for(int i=0;i<setList.Count;i++){
		// 		if(setList[i].SetListName==InventorySystem.instance.setName){
		// 			//Has same name
		// 		// 	for(int j=0;j<setDetail.Count;j++){
		// 		// 		var sList = setList[i].SetListEffect.ToList();
		// 		// 		for(int n=0;n<sList.Count;n++){
		// 		// 			for(int m =0 ;m<setEquipment.Count;m++)
		// 		// 				if(sList[n] == setEquipment[m].GetComponent<SetObj>().setDetail.text){
		// 		// 					setEquipment[m].GetComponent<SetObj>().setDetail.color=Color.white;
		// 		// 				}
							
		// 		// 		}
		// 		// }
		// 	}
		// }
	// }
	}
string LoadRatityLoC(Items i){
	switch(i.itemRatity){
		case ItemRatity.Normal:
			return "普通";
			break;
		case ItemRatity.Rare:
			return"稀有";
			break;
			case ItemRatity.Epic:
				return"史诗";
			break;
			case ItemRatity.Ancient:
				return"古代";
			break;
			case ItemRatity.Junk:
				return"垃圾";
			break;
		
	}

	return "";
}
	//Hide the tooltip
	public void HideTooltip() {
		showTooltip = false;
		foreach(Transform trans in transform) {
			trans.gameObject.SetActive(false);
		}
	}

/// <summary>
/// Extra(MIDDLE FRAME)
///	1.weapon => PROPERTIES , usescriptname(),socket
/// 2,armor => PROPERTIES , USN(), BLOCK ,SOCKET
///3.AMULET(SLOT3)=>PROPERTIES,SPECIAL EFFECT(REARE)
//
/// </summary>
/// <param name="item"></param>
/// <returns></returns>
	public string GenerateTooltip(Items item) {
		string generatedTooltipText = "";
		//
		if(item.unidentified) {
			Color color = Color.red;
			generatedTooltipText = "<color=#" + ColorToHex(color) + ">Unidentified </color>";
			
			if(item.itemType == EquipmentSlotType.weapon)
			{
				generatedTooltipText += "<color=#"+ColorToHex(color)+ ">" + item.itemRatity.ToString() + " ";
				generatedTooltipText += item.weaponType.ToString()+"</color>\n\n";
				

			}
			
			generatedTooltipText += "<size=10><color=#"+ColorToHex(color)+ ">This item cannot be equipped until it's identified.</color></size>\n\n";
			
			generatedTooltipText += "????\n";
			generatedTooltipText += "????\n";
			generatedTooltipText += "????\n";
			generatedTooltipText += "????";
			OthersDetail.gameObject.SetActive(false);
		}
		 else {
			//Has item Show name
			 Color color = FindColor(item);
			 item.tooltipHeader = "<size=20><color=#" + ColorToHex(color) + ">" + item.itemName + "</color></size>";
			
			 //weapon show damage 
			 if(item.itemType == EquipmentSlotType.weapon) {
				 baseValueText.text = "<size=10>Damage" + Mathf.FloorToInt(item.damage) + "</size>\n";
				  baseDurText.text = "<size=20>weaponDur" + item.weaponDur.ToString() + "</size>";
			
			 }
			 //armor
			 else if (item.itemType == EquipmentSlotType.armor)
			 {
				 //Armor
				 baseValueText.text = "<size=20>Armor" +Mathf.FloorToInt( item.armor) + "</size>";
				 baseDurText.text = "<size=20>ArmorDur" + item.armorDur.ToString() + "</size>";
			 }
			 //Ring Slot => EXTRA()
			 else if(item.itemType == EquipmentSlotType.ring){
				 	baseValueText.text= "<size=10>RingArmor"+item.armor.ToString()+"</size>";
					  baseDurText.text = "<size=20>RingDur" + item.ringDur.ToString() + "</size>";
			 }
			 else if(item.itemType == EquipmentSlotType.consumable) {
			 	generatedTooltipText += item.consumableType.ToString()+"\n\n";
			 	if(item.consumableType == ConsumableType.potion) {
			 		generatedTooltipText += "Heals you instantly for: <color=green>" + item.healAmount.ToString() + "</color> life.\n\n";
					
			 		generatedTooltipText += "Cooldown: " + item.cooldown.ToString();
			 	}
			 	else if(item.consumableType == ConsumableType.Scroll) {
			 		generatedTooltipText += 	"<color=green>Right to use.</color>\n\n" +
			 			"Use on unidentified items to reveal their hidden powers.";
			 	}
			 }
			 else {
			 	generatedTooltipText += "<color=#"+ColorToHex(FindColor(item))+ ">" + item.itemRatity.ToString() + " ";
			 	generatedTooltipText += item.itemType.ToString()+"</color>";
			 }
		 }


	

			generatedTooltipText += "<color=#"+ColorToHex(attributeColor)+">\n";

			
			 if (item.fireResistance>0)
			{
				generatedTooltipText += "+" + item.fireResistance.ToString() + "fireResistance\n";
			}else if (item.iceResistance>0)
			{
				generatedTooltipText += "+" + item.iceResistance.ToString() + "iceResistance\n";
			}else if (item.posionResistance >0)
			{
				generatedTooltipText += "+" + item.posionResistance.ToString() + "posionResistance\n";
			}else if (item.phyicsResistance>0)
			{
				generatedTooltipText += "+" + item.phyicsResistance.ToString() + "phyicsResistance\n";
			}
			generatedTooltipText +="</color>";
			//Remove the last "\n"
			generatedTooltipText = generatedTooltipText.Remove(generatedTooltipText.Length - 1,1);

			generatedTooltipText += "</color>";
		
			generatedTooltipText += "<color=#" + ColorToHex(attributeColor) + ">\n\n";
			if (item.useEffectScriptName!= "")
			{
				generatedTooltipText += "Special Effect:\t\t" + item.useEffectScriptName;
			}

			
		return generatedTooltipText;
	}

	//Find the color based on the quality of the item
	Color FindColor(Items item) {
		InventorySystem inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventorySystem>();
		if(item.itemRatity == ItemRatity.Junk) {
			return inventory.junkColor;
		}
		else if(item.itemRatity == ItemRatity.Legendary) {
			return inventory.legendaryColor;
		}
		else if(item.itemRatity == ItemRatity.Normal) {
			return inventory.normalColor;
		}
		else if(item.itemRatity == ItemRatity.Rare) {
			return inventory.rareColor;
		}
		else if(item.itemRatity == ItemRatity.Junk) {
			return inventory.setColor;
		}
		return Color.red;
	}

	//Convert the color into hex decimal
	public static string ColorToHex(Color32 color)
	{
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
	}
}
}