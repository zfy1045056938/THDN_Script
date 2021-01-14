using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

public class DragSpellNoTarget : DragAction
{

    private WhereIsTheCardOfCreature whereIsCard;
    public OneCardManager manager;
    private int saveHandSlot;

    private void Awake()
    {
        whereIsCard = GetComponent<WhereIsTheCardOfCreature>();
        manager = GetComponent<OneCardManager>();
    }



    public override bool OnCanDrag
    {
        get
        {
            return base.OnCanDrag & manager.CanbePlayNow;
            // return true;
        }
    }

    public override void OnCancelDrag()
    {
        whereIsCard.slot = saveHandSlot;
        if (tag.Contains("Low"))
        {
            whereIsCard.visualState = VisualStates.LowHand;
        }
        else if (tag.Contains("Top"))
        {
            whereIsCard.visualState = VisualStates.TopHand;
        }
        //
        HandVisual playerHand = PlayerOwner.playerArea.handVisual;
        Vector3 oldCardPos = playerHand.slots.children[saveHandSlot].transform.localPosition;
        transform.DOLocalMove(oldCardPos, 1f);
    }

    public override void OnDraggnigInUpdate()
    {
       
    }

    public override void OnEndDrag()
    {
        if (DraggingSuccess())
        {
            PlayerOwner.PlayASpellFromHand(GetComponent<IDHolder>().uniqueID, -1);
//            PlayerOwner.playerArea.handVisual.PlayASpellFromHand(GetComponentInParent<IDHolder>().uniqueID);
        }
        else
        {
            whereIsCard.slot = saveHandSlot;
            if (tag.Contains("Low"))
            {
                whereIsCard.visualState = VisualStates.LowHand;
            }
            else 
            {
                whereIsCard.visualState = VisualStates.TopHand;
            }
            //
            HandVisual playerHand = PlayerOwner.playerArea.handVisual;
            Vector3 oldCardPos = playerHand.slots.children[saveHandSlot].transform.localPosition;
            transform.DOLocalMove(oldCardPos, 1f);
        }
    }

  
    public override void OnStartDrag()
    {
        saveHandSlot = whereIsCard.slot;
        
        
        whereIsCard.visualState = VisualStates.Dragging;

        whereIsCard.BringToFront();
      
    }

    protected override bool DraggingSuccess()
    {
        return TableVisual.CursorOverSomeTable;
    }
}