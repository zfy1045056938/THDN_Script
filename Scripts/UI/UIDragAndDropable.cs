using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDragAndDropable : MonoBehaviour,IBeginDragHandler,IEndDragHandler,IDragHandler,IDropHandler
{
    public PointerEventData.InputButton button = PointerEventData.InputButton.Left;

    public GameObject draggPrefab;

    public static GameObject currentDragged;
    
    //to cursor
    public bool draggable = true;
    //top the world
    public bool dropable = true;

    [HideInInspector] public bool draggedToSlot = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (draggable && eventData.button == button)
        {
            currentDragged = Instantiate(draggPrefab, transform.position, Quaternion.identity);
            currentDragged.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
            currentDragged.transform.SetParent(transform.root,true);
            currentDragged.transform.SetAsLastSibling();
            
            //
            GetComponent<Button>().interactable = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       Destroy(currentDragged);
       //
       if (draggable && eventData.button == button)
       {
           if (!draggedToSlot && eventData.pointerEnter == null)
           {
               Players.localPlayer.SendMessage("OnDragAndClear_"+tag,name.ToInt(),SendMessageOptions.DontRequireReceiver);
           }
           
           //
           draggedToSlot = false;
           //
           GetComponent<Button>().interactable = true;
       }
    }
    

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == button && !draggable)
        {
            currentDragged.transform.position = eventData.position;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (draggable && eventData.button == button)
        {
            UIDragAndDropable drop = eventData.pointerDrag.GetComponent<UIDragAndDropable>();
            //
            if (drop != null && drop.draggable)
            {
                //
                drop.draggedToSlot = this;
                if (drop != this)
                {
                    int from = drop.name.ToInt();
                    int to = name.ToInt();
                    Players.localPlayer.SendMessage("OnDragAndDrop_"+drop.tag+"_"+tag,new int[]{from,to},SendMessageOptions.DontRequireReceiver);
                }
            }

        }
    }
}
