using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//确定随从所指向的位置,以及随从处于的状态
//
public enum VisualStates{
    //手部
    LowHand,
    TopHand,
    Dragging,   //抓取状态
    //
    Checking,
    //
    //牌桌
    LowTable,
    TopTable,
    Transition,
    Discard,
}

/// <summary>
/// Where is the card of creature.
/// </summary>
public class WhereIsTheCardOfCreature : MonoBehaviour {
        private HoverPreview HoverPreview;

        private Canvas canvas ;
        
    private int TopStringSort=500;

        //格子
        private int _slot =-1;
        public int slot{get{return _slot;}set{
            _slot=value;

            //if (value != -1)
            //{
            //    canvas.sortingOrder = HandSortingOrder(slot);
            //}
        }
    }

        //是否拖动状态
        private VisualStates state ;
        public VisualStates visualState{
            get{return state; }
            set{
            state = value;
                 switch (state)
                 {
                case VisualStates.LowHand:
                    HoverPreview.thisPreviewEnable =true;
                        break;
                    case VisualStates.TopHand:
                        HoverPreview.thisPreviewEnable = false;
                        break;
                    case VisualStates.LowTable:
                    HoverPreview.thisPreviewEnable = true;
                    break;
                case VisualStates.TopTable:
                    HoverPreview.thisPreviewEnable = true;
                    break;

                    case VisualStates.Dragging:
                        HoverPreview.thisPreviewEnable = false;
                        break;
                case VisualStates.Transition:
                    HoverPreview.thisPreviewEnable = false;
                    break;
                     
                 }
            }
        }


        void Awake()
        {
            HoverPreview = GetComponent<HoverPreview>();
            //
            if(HoverPreview == null)
            HoverPreview =GetComponentInChildren<HoverPreview>();
                //
        canvas = GetComponentInChildren<Canvas>();

        }

        public void BringToFront(){
            canvas.sortingOrder = TopStringSort;
            canvas.sortingLayerName ="AboveEverything";

        }

         //设置手部优先级序列
        public void SetHandSortingOrder(){
            if(slot != -1)
                canvas.sortingOrder =HandSortingOrder(slot);

        canvas.sortingLayerName ="Cards";   //bing layerName called cards
        }

        public void SetTableSortingOrder(){
            canvas.sortingOrder=0;
            canvas.sortingLayerName ="Creatures";
        }

        public int HandSortingOrder(int placeInHand){
            return (-(placeInHand+1)*9);
        }

}