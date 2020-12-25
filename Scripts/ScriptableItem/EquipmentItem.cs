using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Mirror;


//equipment item in thdn for player who can equip or business with merchant
//equipment item in game has some type includes
//WEAPON || ARMOR || SPECIALITEM(IN DUNGEON CAN DESTORY WHEN LEAVE DUNGEON)
[CreateAssetMenu(menuName = "THDNITEM/EquipmentItem",order = 999)]
public class EquipmentItem : UsableItem
{
    
    [Header("Equipment")]
    //Equipment Stats
    public GameObject modelPrefab;
    public string category;

    //equipment extra
    public int healthBouns;
    public int damageBouns;
    public int armorBouns;

    public int speedBouns;
    //other vs
    [Range(0,1)]
    public float BlockPer;
    [Range(0,1)]
    public float CriPer;       
    
    public bool canBlock;
    public bool canLevelUp;    //BLACKSMITH


    
    public virtual  bool CanEquip(Players p, int inventoryIndex,int equipmentIndex)
    {
        string re = p.equipmentInfos[equipmentIndex].requiredCategory;
        return CanUse(p, inventoryIndex) && re != "" && category.StartsWith(re);
    }

    // public override bool CanUse(Players p, int inventoryIndex)
    // {
    //    return FindEquipableSlotFor(p,inventoryIndex)!=-1;
    // }

    int FindEquipableSlotFor(Players p, int index)
    {
        // for (int i = 0; i < p.equipment.Count; i++)
        // {
        //     if (CanEquip(p, index, i))
        //     {
        //         return i;
        //     }
        // }

        return -1;
    }


    // public override string Tooltip(int r,bool isRequirement=false)
    // {
    //    StringBuilder sb = new StringBuilder(tooltip);

    //    sb.Replace("{CATEGORY", category);
       
    //    sb.Replace("{BLOCK}", BlockPer.ToString());
    //    sb.Replace("CRIPER", CriPer.ToString());
    //    sb.Replace("CANBLOCK", canBlock?"YES":"NO");
    //    sb.Replace("CANLEVELUP",canLevelUp?"YES":"NO");

    //    return sb.ToString();
    // }
}