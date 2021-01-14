using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using TMPro;

[System.Serializable]
    public class WeaponItem:EquipmentItem
    {
       

    public override bool CanUse(Players p, int inventoryIndex)
    {
        return base.CanUse(p, inventoryIndex);
    }

    public override void Onuse(Players p)
    {
        base.Onuse(p);
    }

    // public override string Tooltip(int r, bool isRequirement = false)
    // {
    //     return base.Tooltip(r, isRequirement);
    // }

    public override void Use(Players p, int inventoryIndex)
    {
        base.Use(p, inventoryIndex);
    }
}

