using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager3 : MonoBehaviour
{
   // Start is called before the first frame update
   public GameObject tooltip;
   
   //Common
   public Text headText;
   public Text toolText;
   public Text descText;

   public Text reLevelText;
   //GemSlot
   public Transform GemPos;
   
   //
   public float tooltipHeight;
   public float tooltipDelay;

   public Color attColor;
   [HideInInspector] public bool showTooltip;

   public IEnumerator ShowTooltip(bool right, Items item, SlotType type, int startNumber, RectTransform rects,
      bool merchant)
   {
      showTooltip = true;

      if (right)
      {
         tooltip.GetComponent<RectTransform>().sizeDelta=new Vector2(1,1);
      }
      else
      {
         
         tooltip.GetComponent<RectTransform>().sizeDelta=new Vector2(0,1);

      }
      //
      
      if (showTooltip)
      {
         foreach (Transform t in transform)
         {
            t.gameObject.SetActive(true);
         }
         //
         headText.text = item.itemName.ToString();
         toolText.text = GenerateTooltip(item);
         
//         headText.color = FindColor(item);
         
         
         descText.text = item.descriptionText.ToString();
         
         //
         reLevelText.text = "\t\t\t\t" + "需求等级" + item.itemLevel.ToString();
         reLevelText.rectTransform.sizeDelta=new Vector2(reLevelText.preferredWidth,reLevelText.preferredHeight);
         //
         
         toolText.rectTransform.sizeDelta=new Vector2(reLevelText.preferredWidth,toolText.preferredHeight);
         descText.rectTransform.sizeDelta=new Vector2(reLevelText.preferredWidth,descText.preferredHeight+4);
         //tooltip size
         tooltip.GetComponent<RectTransform>().sizeDelta=new Vector2(reLevelText.rectTransform.sizeDelta.x,
            tooltipHeight+toolText.rectTransform.sizeDelta.y + descText.rectTransform.sizeDelta.y);
//         tooltip.GetComponent<RectTransform>().sizeDelta=new Vector2(reLevelText.rectTransform.sizeDelta.x,
//            tooltipHeight+toolText.rectTransform.sizeDelta.y + descText.rectTransform.sizeDelta.y +
//                                                                                                           reLevelText.rectTransform.sizeDelta.y);
////         //merchant

         RectTransform rect = tooltip.GetComponent<RectTransform>();
         tooltip.transform.position =new Vector3(rect.transform.position.x,rect.transform.position.y,-20f);
         
         //
         if (right)
         {
            tooltip.GetComponent<RectTransform>().pivot = new Vector2(1,1);
            if (type == SlotType.Equipment)
            {
               Debug.Log("ToolTip From equipment slot");
               tooltip.transform.localPosition -= new Vector3(rects.sizeDelta.x , -rects.sizeDelta.y );
            }else if (type == SlotType.Merchant)
            {
               tooltip.transform.localPosition -= new Vector3(rects.sizeDelta.x,-rect.sizeDelta.y);
            }
         }
         else
         {
            tooltip.GetComponent<RectTransform>().pivot=new Vector2(0,1);
            if (type == SlotType.Crafting)
            {
               
            }
            else
            {
               tooltip.transform.localPosition += new Vector3(rects.sizeDelta.x,0);
            }
         }
         
         //
         if (rect.localPosition.y < 0)
                     {
                        if (Math.Abs(rect.localPosition.y) + rect.sizeDelta.y >
                            transform.root.GetComponent<CanvasScaler>().referenceResolution.y * 0.5)
                        {
                           rect.localPosition -= new Vector3(0f,transform.root.GetComponent<CanvasScaler>().referenceResolution.y*0.5f - Mathf.Abs(rect.localPosition.y) + rect.sizeDelta.y,0);
                           
                        }
                        
                     }
                     else
                     {
                       if( Mathf.Abs(rect.sizeDelta.y - Mathf.Abs(rect.localPosition.y))> transform.root.GetComponent<CanvasScaler>().referenceResolution.y * 0.5)
                        {
                           rect.localPosition += new Vector3(0,Mathf.Abs(rect.sizeDelta.y - Mathf.Abs(rect.localPosition.y)) -rect.root.GetComponent<CanvasScaler>().referenceResolution.y * 0.5f,0);
                        }
                     }
      }
      else
      {
         HideTooltip();
      }
      
      //
      yield return new WaitForSeconds(tooltipDelay);
   }

   public void HideTooltip()
   {
      showTooltip = false;
      foreach (Transform t in transform)
      {
         t.gameObject.SetActive(false);
      }
   }
   
   /// <summary>
   /// Except Values(3)
   /// </summary>
   /// <param name="item"></param>
   /// <returns></returns>
   public string GenerateTooltip(Items item)
   {
      int i = 0;
      string gtt = "";
      if (item.unidentified)
      {
         Color color = Color.red;
         gtt = "<color=#" + ColorToHex(color) + ">未鉴定物品</color>";
         if (item.itemType == EquipmentSlotType.weapon)
         {
            gtt += "<color=#" + ColorToHex(color) + ">" + item.itemRatity.ToString() + "";
            gtt += item.weaponType.ToString() + "</color>";

            gtt += item.weaponType.ToString() + "</color>";
            gtt += "<size=30>" +"伤害:"+ item.minDamage + "</size>";
            

         }
         //
         gtt += "<size=10><color=#" + ColorToHex(color) + ">未鉴定物品" + "</size>\n\n";
         gtt += "????\n";
         gtt += "???\n";
         
      }
      else
      {
         i++;
         Debug.Log("Generate Detail\t\t"+i);
         //Show Detail For 
         Color color = FindColor(item);
         item.tooltipHeader = item.itemName;
         
         if (item.itemType == EquipmentSlotType.weapon)
         {
            gtt += "武器类型:\t"+item.weaponType.ToString() +"\n\n";
            gtt += "伤害\t" + Mathf.FloorToInt(item.minDamage );
            gtt += "耐久度\t" + Mathf.FloorToInt(item.weaponDur)+"\n\n";
            
         }else if (item.itemType == EquipmentSlotType.armor)
         {
            
         }else if (item.itemType == EquipmentSlotType.consumable)
         {
            
         }
  
      }
      
      //
      if (item.itemRatity != ItemRatity.Normal && item.itemRatity != ItemRatity.Junk)
      {
        
         if (item.strength > 0)
         {
            gtt +="\t\t+"+ item.strength + "力量\n\n";
         }
         if (item.dexterity > 0)
         {
            gtt += "\t\t+" + item.dexterity + "敏捷\n\n";
         }
         if (item.magic > 0)
         {
               
         }
         if (item.fireResistance > 0)
         {
               
         }
         if (item.iceResistance > 0)
         {
               
         }
         if (item.posionResistance > 0)
         {
               
         }
         if (item.electronicResistance > 0)
         {
               
         }
         if (item.minBlockAmount > 0)
         {
               
         }
         
         gtt += gtt.Remove(gtt.Length - 1, 1);
        
      }

      
      return gtt;
   }

   public static string ColorToHex(Color32 color)
   {
      string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
      return hex;
   }

   Color FindColor(Items item)
   {
      InventorySystem inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventorySystem>();
      //
      if (item.itemRatity == ItemRatity.Junk)
      {
         return inventory.junkColor;
      }else if (item.itemRatity == ItemRatity.Normal)
      {
         return inventory.normalColor;
      }else if (item.itemRatity == ItemRatity.Rare)
      {
         return inventory.rareColor;
      }else if (item.itemRatity == ItemRatity.Epic)
      {
         return inventory.rareColor;
      }else if (item.itemRatity == ItemRatity.Legendary)
      {
         return inventory.legendaryColor;
      }else if (item.itemRatity == ItemRatity.Ancient)
      {
         return inventory.ancientColor;
      }

      return Color.clear;
   }
}
