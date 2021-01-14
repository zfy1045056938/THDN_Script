using UnityEngine;
using System.Collections.Generic;
using Mirror;


[CreateAssetMenu(menuName="THDNITEM/SummonableItem")]
public abstract class SummonableItem : UsableItem
{
   
    public int price = 10;
    public int removeItemIfDied;


    public  bool CanUse(Players p, int inventoryIndex)
    {
        return base.CanUse(p, inventoryIndex);
           
    }

    public void Use(Players p, int inventoryIndex)
    {
        base.Use(p,inventoryIndex);
        p.nextRiskyActionTime = NetworkTime.time + 1;
    }

}