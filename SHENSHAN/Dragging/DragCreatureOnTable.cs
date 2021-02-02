using UnityEngine;
using System.Collections;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine.EventSystems;
/// <summary>
/// 将手牌拖入战场需要判断该卡牌是否具有效果和稀有度，
/// 1.随从：当玩家满足条件时拖入战场,包含以下功能：进入战场特效（由随从稀有度以及类型决定)
/// 2.当拖入传奇随从时，Affix激活特效并置入战场
/// 3.当随从具有以下效果时激活提前激活效
///         3.1冲锋：当置入战场时highlight显示轮廓并激活行动阶段,
///         3.2AOE: when creature required AOE effect check other player deck creature number ,then 
///      accord creature effect hurt them ,check the another effect;
///         3.3
/// </summary>
public class DragCreatureOnTable : DragAction,IPointerEnterHandler,IPointerExitHandler
{

    public int saveHandSlot;
    private WhereIsTheCardOfCreature whereIsCard;
    private IDHolder idScript;
    private VisualStates tmpState;
    private OneCardManager manager;
    private TableVisual playerOwner;
    private bool GetCard = false;

    private float maxScale=4f;
    private float initScale=2f;

    
    private void Awake()
    {
        whereIsCard = GetComponent<WhereIsTheCardOfCreature>();
        manager = GetComponent<OneCardManager>();
    }


    public override void OnCancelDrag()
    {
        //when table is full or otherplace
        //PlayerOwner.HighlightPlayableCards(false);
        whereIsCard.SetHandSortingOrder();
        whereIsCard.visualState = tmpState;
        HandVisual playerHand = PlayerOwner.playerArea.handVisual;
        Vector3 olaCardPos = playerHand.slots.children[saveHandSlot].transform.localPosition;
        transform.DOLocalMove(olaCardPos, 0.3f);
    }

    public override void OnDraggnigInUpdate()
    {
    }

    public override bool OnCanDrag {
        get
        {
            return base.OnCanDrag &&
                   manager.CanbePlayNow;

//            return true;

        }
    }
    public override void OnEndDrag()
    {
        //if (!enabled) return;
        if (DraggingSuccess())
        {
          
            int tablePos = PlayerOwner.playerArea.tableVisual.TablePosForNewCreature(Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)).x);
            //
            Debug.Log(tablePos.ToString()+"Pos");
            //
            PlayerOwner.PlayACreatureFromHand(GetComponent<IDHolder>().uniqueID,tablePos);
          
        }
        else
        {
//            //enabled = false;
//            //
//            whereIsCard.SetHandSortingOrder();
//            whereIsCard.visualState = tmpState;
//            HandVisual playerHand = PlayerOwner.playerArea.handVisual;
//            Vector3 oldCardPos = playerHand.slots.children[saveHandSlot].transform.localPosition;
//            transform.DOLocalMove(oldCardPos, 1f);
    OnCancelDrag();
        }
        TableVisual.instance.noticeImg.gameObject.SetActive(false);
    }

    public override void OnStartDrag()
    {
        saveHandSlot = whereIsCard.slot;
        tmpState = whereIsCard.visualState;
        whereIsCard.visualState = VisualStates.Dragging;
        whereIsCard.BringToFront();
        //Show Notice IMG
       
     
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override bool DraggingSuccess()
    {

        if (PlayerOwner.table.creatureOnTable.Count < 6)
        {
//        bool tableNotFull = (PlayerOwner.table.creatureOnTable.Count <6);
            return TableVisual.CursorOverSomeTable;
        }
        else
        {
            return  false;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       transform.DOScale(initScale,0.4f);
        transform.DOLocalMoveY(0f,0.4f,false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(maxScale,0.4f);
        transform.DOLocalMoveY(102f,0.4f,false);
    }

   

    
}
