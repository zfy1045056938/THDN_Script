using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using PixelCrushers.DialogueSystem;
//List Items
public class CraftButton : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler
{

	public CraftedItem item;
	private CraftManager crafting;
	public TextMeshProUGUI text;
	public Image itemSprite;
	public Button craftBtn;
	public List<int> craftingMaterialsCounters;

	public List<int> craftingAmountCounters;

	// Use this for initialization
	IEnumerator Start () {
		//set the button of the item to this button
		item.button = this;
		yield return new WaitForEndOfFrame();
		crafting =GameObject.FindObjectOfType<CraftManager>().GetComponent<CraftManager>();
		craftingMaterialsCounters = new List<int>();
		UpdateText();
	}

	//Update the text of the button equal to the amount it's possible to create and the item quality and the name of the item
	public void UpdateText() {
		// if(crafting==false) {
		// 	return;
		// }

		Debug.Log("Reduce Material Items");
		//Count how many items it's possible to create given the materials in the inventory
		craftingMaterialsCounters = new List<int>();
		for(int i = 0; i < item.materials.Count; i++) {
			int temp =0;
			for(int j = 0; j < crafting.inventory.items.Count; j++) {
				if(crafting.inventory.items[j].item.itemName == item.materials[i].itemName && j == crafting.inventory.items[j].itemStartNumber) {
					temp += crafting.inventory.items[j].item.stackSize;
					// Debug.Log(temp);
				}
			}
			craftingMaterialsCounters.Add(temp);
		}
		//Find the highest one
		int highestItemAvaliable = 0;
		for(int i = 0; i < craftingMaterialsCounters.Count; i++) {
			if(craftingMaterialsCounters[i] != 0 && craftingMaterialsCounters[i]/item.materialRequiredAmount[i] < highestItemAvaliable || (highestItemAvaliable == 0 && i == 0)) {
				highestItemAvaliable = Mathf.FloorToInt(craftingMaterialsCounters[i]/item.materialRequiredAmount[i]);
			}
		}

		
		// text.text = "";
		//If the highest amount is equal to zero just place "-" in front of the item name
		if(highestItemAvaliable == 0) {
			// text.text += "- " + item.item.itemName;
			crafting.acceptButton.interactable = false;
		}
		//Else if it's higher than zero place the highest number in front of the item name
		else {
			// text.text += "["+highestItemAvaliable.ToString()+"] "+ item.item.itemName;
			crafting.acceptButton.interactable = true;
			if(crafting.amountToCraft > highestItemAvaliable) {
				crafting.amountToCraftLabel.text = highestItemAvaliable.ToString();
				crafting.amountToCraft = highestItemAvaliable;
				crafting.craftCostLabel.text = (item.cItemCost * highestItemAvaliable).ToString();
			}
		}
		// text.color = crafting.inventory.FindColor(item.item);
		
	}

   

    //called when then player clicks on the item button
    public void OnPointerClick(PointerEventData date) {
		Debug.Log("Select Btn");
    	// crafting.materials = new List<CraftMaterials>();
		//clear all meterials items
    	
    	CraftManager.instance.ChangeCraftingItem(this);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    //    TooltipManager.instance.ShowTooltip(false,item.item,SlotType.Crafting,1,this.GetComponent<RectTransform>(),false);
	CraftManager.instance.ShowItem(item.item);
    }
}
