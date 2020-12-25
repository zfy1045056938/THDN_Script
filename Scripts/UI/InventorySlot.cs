using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Text;
using Unity.Mathematics;
using Invector.vItemManager;


[Serializable]
public partial struct  InventorySlot
{
   public ItemReference item;
   public int amount;   //stacksize

   
   public InventorySlot(ItemReference item, int amount=1)
   {
      this.item = item;
      this.amount = amount;
   }

   public int DecreaseAmount(int reduce)
   {
      int limit = math.clamp(reduce, 0, amount);
      amount -= limit;
      return limit;
   }

   public int IncreaseAmount(int increase)
   {
      int limit = math.clamp(increase, 0, item.amount - amount);
      amount += limit;
      return limit;
   }

   public string Tooltip()
   {
      if (amount == 0) return "";
      //
      StringBuilder sb = new StringBuilder();
      sb.Replace("{AMOUNT}", amount.ToString());

      return sb.ToString();
   }
}

// public class SyncItemSlot:SyncList<InventorySlot>{}
