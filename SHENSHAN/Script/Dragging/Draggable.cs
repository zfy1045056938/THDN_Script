using UnityEngine;
using System.Collections.Generic;

using DG.Tweening;
using UnityEngine.EventSystems;

   
//拖动控制对象
public class Draggable:MonoBehaviour{
    private StartDragBehaviour HowToStart =StartDragBehaviour.OnMouseDown;
    private EndDragBehaviour  HowEnd = EndDragBehaviour.OnMouseUp;

    //
    private bool dragging =false;

    private float zDisplacement;
    
    private Vector3 pointerDisplacement;

    private static Draggable _draggingThis;
    private static Draggable draggingThis{
        get{
            return _draggingThis;
        }
       
    }

    private DragAction dragAction;

    


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        dragAction=GetComponent<DragAction>();
    }

  
   

    void Update()
    {
        //当处于拖动状态时
        if(dragging){
            Vector3 mousePos = MouseInWorldCoords();
            //
            transform.position = new  Vector3(mousePos.x -pointerDisplacement.x,
                                                   mousePos.y-pointerDisplacement.y ,transform.position.z);
            //
            dragAction.OnDraggnigInUpdate();
        }    

        
    }

   

    /// <summary>
    /// Starts the dragging.
    /// </summary>
    public void StartDragging(){
        dragging =true;
        HoverPreview.previewAllowed =false;
        _draggingThis = this;
        dragAction.OnStartDrag();
        zDisplacement = -Camera.main.transform.position.z + transform.position.z;
        pointerDisplacement = -transform.position + MouseInWorldCoords();
    }


    /// <summary>
    /// Cancels the drag.
    /// </summary>
    public void CancelDrag(){
        if(dragging){
            dragging =false;
            HoverPreview.previewAllowed=true;
            _draggingThis =null;
            dragAction.OnCancelDrag();
        }
    }


    /// <summary>
    /// Mouses the in world coords.
    /// </summary>
    /// <returns>The in world coords.</returns>
    public Vector3 MouseInWorldCoords(){
        var screenMousePos = Input.mousePosition;
        screenMousePos.z = zDisplacement;
        return  Camera.main.ScreenToWorldPoint(screenMousePos);
    }

 

    /// <summary>
    /// Suitable for battle card (drag)
    /// </summary>
    private void OnMouseDown()
    {
        

        if (dragAction != null && dragAction.OnCanDrag && HowToStart == StartDragBehaviour.OnMouseDown)
        {

            StartDragging();
        }
        if (dragging && HowEnd == EndDragBehaviour.OnMouseDown)
        {
            dragging = false;
            HoverPreview.previewAllowed = true;
            _draggingThis = null;

            dragAction.OnEndDrag();
        }

    }

    

    private void OnMouseUp()
    {
        if (dragging && HowEnd == EndDragBehaviour.OnMouseUp)
        {
            dragging = false;
            HoverPreview.previewAllowed = true;
            _draggingThis = null;
            dragAction.OnEndDrag();
        }
    }

}