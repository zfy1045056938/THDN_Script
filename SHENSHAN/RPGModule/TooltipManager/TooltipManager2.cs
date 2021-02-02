using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System;

public class TooltipManager2 : MonoBehaviour
{
    public GameObject tooltip;
    public float MainHeigt;
    public Text tooltipText;
    public Text tooltipHeadText;
    public Image tooltipItemSprite;
    public Text tooltipContentText;
    public Text tooltipDesciptionText;
    public Text tooltipLevelText;
    public Text sellValueText;
    public Text buyText;
    private Color attributeColor;
    public float tooltipDelay=0.5f;
  
    [HideInInspector]
    public bool showTooltip;

    public IEnumerator ShowTooltip(bool right,Items items,SlotType type,int startNumber,RectTransform transform,bool merchant){
         showTooltip = true;
    Debug.Log("Show tooltip");
         if(right){
             tooltip.GetComponent<RectTransform>().pivot=new Vector2(1,1);
         }else{
                          tooltip.GetComponent<RectTransform>().pivot=new Vector2(0,1);

         }

         //
         yield return new WaitForSeconds(tooltipDelay);

         //showtooltip
         if(showTooltip){
             foreach(Transform t in transform){
                 t.gameObject.SetActive(true);
             }
             //generate item values in it
//             tooltipText.text=GenerateTooltip(items);
             //
             tooltip.gameObject.SetActive(true);
             tooltipHeadText.color=FindColor(items);
             tooltipHeadText.text=items.tooltipHeader.ToUpper();
             tooltipDesciptionText.text=items.descriptionText;
             //level Set Right side
             tooltipLevelText.text ="\t\t\t\t\t"+"level:"+items.itemLevel+"\n";
             //
             tooltipText.rectTransform.sizeDelta=new Vector2(tooltipLevelText.preferredWidth,tooltipLevelText.preferredHeight+4);
             tooltipDesciptionText.rectTransform.sizeDelta=new Vector2(tooltipDesciptionText.preferredWidth,tooltipDesciptionText.preferredHeight);
             //
             	tooltip.GetComponent<RectTransform>().sizeDelta = new Vector2(tooltipLevelText.rectTransform.sizeDelta.x + 50,
			 MainHeigt + tooltipText.rectTransform.sizeDelta.y +
			  tooltipDesciptionText.rectTransform.sizeDelta.y + tooltipLevelText.rectTransform.sizeDelta.y);

             //merchant page tooltip show right
             if(merchant){
                 sellValueText.text= items.buyPrice.ToString();
                 
             }else{
                 sellValueText.text=items.sellPrice.ToString();
             }

             //
             sellValueText.rectTransform.sizeDelta=new Vector2(sellValueText.preferredWidth,sellValueText.preferredHeight);
             RectTransform rect=tooltip.GetComponent<RectTransform>();

             //
             tooltip.transform.position = transform.position;

             //
             if(right){
                 tooltip.GetComponent<RectTransform>().pivot = new Vector2(1,1);
                 if(type == SlotType.Equipment){
                    // tooltip.transform.localPosition=new Vector3(items.width,items.height,0);
                 }else{
                     tooltip.transform.localPosition+=new Vector3(transform.sizeDelta.x *items.width,0);
                 }
             }

            //
            if(rect.localPosition.y < 0) {
				if(Mathf.Abs(rect.localPosition.y) + rect.sizeDelta.y > transform.root.GetComponent<CanvasScaler>().referenceResolution.y * 0.5f) {
					rect.localPosition -= new Vector3(0, transform.root.GetComponent<CanvasScaler>().referenceResolution.y * 0.5f - (Mathf.Abs(rect.localPosition.y) + rect.sizeDelta.y),0);
				}
			}
			else {
				if(Mathf.Abs(rect.sizeDelta.y - Mathf.Abs(rect.localPosition.y)) > transform.root.GetComponent<CanvasScaler>().referenceResolution.y * 0.5f) {
					rect.localPosition += new Vector3(0, Mathf.Abs(rect.sizeDelta.y - Mathf.Abs(rect.localPosition.y)) - transform.root.GetComponent<CanvasScaler>().referenceResolution.y * 0.5f,0);
				}
			}

             
         }else{
             HideTooltip();
         }

    }

    public void HideTooltip()
    {
       showTooltip=false;
       foreach(Transform t in transform){
           t.gameObject.SetActive(false);
       }
    }

    //Generate tooltip in the 
    private string GenerateTooltip(Items items)
    {
        string generatedTooltipText="";

        //item not unidentified show ??? 
        if(items.unidentified){
            Color color =Color.red;
            generatedTooltipText = "<color=#"+ColorToHex(color)+">Unidentified</color>";

            //
            if(items.itemType == EquipmentSlotType.weapon){
                generatedTooltipText += "<color=#"+ColorToHex(color)+ ">" + items.itemRatity.ToString() + " ";
				generatedTooltipText += items.weaponType.ToString()+"</color>\n\n";

				generatedTooltipText += items.weaponType.ToString()+"</color>\n\n";
				generatedTooltipText += items.minDamage.ToString() + "-" + items.maxDamage.ToString() + " <color=grey>damage</color>\n";
			 }
             //
             generatedTooltipText += "<size=20><color=#"+ColorToHex(color)+"can't equip</size></color>\n\n";
            
            generatedTooltipText+= "??????\n";
            generatedTooltipText+= "??????\n";
            generatedTooltipText+= "??????\n";
            generatedTooltipText+= "??????";
            
        }else{
            //not unidentied show item info
            Color color =FindColor(items);
            items.tooltipHeader = "<color=#"+ColorToHex(color)+">"+items.itemName+"</color>";

            //
            if(items.itemType ==EquipmentSlotType.weapon){
                generatedTooltipText += "<size=15>Damage"+Mathf.FloorToInt(items.damage)+"</size>\n\n";
                generatedTooltipText += "<size=15>Dur"+Mathf.FloorToInt(items.weaponDur)+"</size>\n\n";
                //


            }else if(items.itemType ==EquipmentSlotType.armor){
                  generatedTooltipText += "<size=15>Armor"+Mathf.FloorToInt(items.armor)+"</size>\n\n";
                generatedTooltipText += "<size=15>Dur"+Mathf.FloorToInt(items.armorDur)+"</size>\n\n";
                
            }
            else  if(items.itemType ==EquipmentSlotType.ring){
                  generatedTooltipText += "<size=15>RingArmor"+Mathf.FloorToInt(items.armor)+"</size>\n\n";
                generatedTooltipText += "<size=15>RingDur"+Mathf.FloorToInt(items.armorDur)+"</size>\n\n";
                
                
            }else if(items.itemType==EquipmentSlotType.consumable){
                //potion
            }else{
                //other just show description
                generatedTooltipText+="<size=20>"+items.descriptionText+"</size>";
            }
            
            //Rare Item Show Extra value example resistances .etc
            //except normal und junk ratity items
            if(items.itemRatity != ItemRatity.Junk || items.itemRatity != ItemRatity.Normal){
                //color for text
                generatedTooltipText += "<color=#"+ColorToHex(attributeColor)+">\n\n";
                if(items.fireResistance>0){
                    generatedTooltipText += "+"+items.fireResistance.ToString()+"fire resistance\n";
                }
                generatedTooltipText =generatedTooltipText.Remove(generatedTooltipText.Length-1,1);
                generatedTooltipText += "</color>";

            }
        }
        

        return generatedTooltipText;
    }

    private Color FindColor(Items items)
    {
       	InventorySystem inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventorySystem>();
		if(items.itemRatity == ItemRatity.Junk) {
			return inventory.junkColor;
		}
		else if(items.itemRatity == ItemRatity.Legendary) {
			return inventory.legendaryColor;
		}
		else if(items.itemRatity == ItemRatity.Epic) {
			return inventory.magicColor;
		}
		else if(items.itemRatity == ItemRatity.Normal) {
			return inventory.normalColor;
		}
		else if(items.itemRatity == ItemRatity.Rare) {
			return inventory.rareColor;
		}else{
            return inventory.junkColor;
        }
		
		return Color.clear;
    }

    private string ColorToHex(Color32 color)
    {
       	string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
    }
}

