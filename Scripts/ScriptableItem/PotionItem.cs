using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using TMPro;

public class PotionItem:UsableItem
    {
       

    public override bool CanUse(Players p, int inventoryIndex)
    {
        return base.CanUse(p, inventoryIndex);
    }


    public override void Use(Players p, int inventoryIndex)
    {
        base.Use(p, inventoryIndex);
    }
}
