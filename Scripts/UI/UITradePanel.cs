using System.Collections;
using System.Collections.Generic;
using PixelCrushers;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITradePanel : MonoBehaviour
{
    public static UITradePanel instance;
    public UIPanel panel;
    public TradeSlot slot;
    public Transform content;
    //public UIDragAndDropable drag;
    

    [HideInInspector] public int buyIndex = -1;
    [HideInInspector] public int sellIndex = -1;

    public UITradePanel()
    {
        if(instance==null)instance=this;
    }

    void Update()
    {
//        Players p = Players.localPlayer;
//
//       
//       
//        if (p != null && p.target != null && target is NPC npc &&
//            Util.CloserDistance(p.collider, p.target.collider) <= p.interactiveRange)
//        {
//            Util.BalancePrefab(slot.gameObject, npc.items.Length, content);
//            
//            for (int i = 0; i < npc.items.Length; ++i)
//            {
//                TradeSlot slots = content.GetComponent<TradeSlot>();
//                ScriptableItem items = npc.items[i];
//
//                //
//                int icopy = i;
//                slots.button.onClick.AddListener(() => { buyIndex = icopy; });
//                //
//                slots.image.color = Color.white;
//                slots.image.sprite = items.image;
//                //
//                slots.tooltip.enabled = true;
//                if (slots.tooltip.IsVisiable())
//                {
//                    slots.tooltip.text = new InventorySlot(new Item(items)).Tooltip();
//                }
//
//            }

            
        }
//        else
           //        {
           //            panel.Close();
           //        }



        //    public void ShowShopPanel(NPC npc)
        //    {
        //        panel.gameObject.SetActive(true);
        //        Players p = Players.localPlayer;

        //        if (p != null && npc!=null)
        //        {
                   
        //            Debug.Log("show panel");
        //            Util.BalancePrefab(slot.gameObject, npc.items.Length, content);

        //            for (int i = 0; i < npc.items.Length; ++i)
        //            {
        //                TradeSlot slots = content.GetComponentInChildren<TradeSlot>();
        //                ScriptableItem items = npc.items[i];

        //                if (slots != null)
        //                {
        //                    //
        //                    int icopy = i;

        //                    //
        //                    slots.image.color = Color.white;
        //                    slots.image.sprite = items.sprite;
        //                    //
        //                 //    slots.tooltip.enabled = true;
        //                 //    if (slots.tooltip.IsVisiable())
        //                 //    {
        //                 //        slots.tooltip.text = new InventorySlot(new Item(items)).Tooltip();
        //                 //    }
        //               }

        //        }
        //        else
        //        {
        //            panel.gameObject.SetActive(false);
        //        }
        //    }
        //    }

           

}
