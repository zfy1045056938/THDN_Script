using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public abstract class DragAction : MonoBehaviour
{

    public abstract void OnStartDrag();
    public abstract void OnEndDrag();
    public abstract void OnDraggnigInUpdate();
    public abstract void OnCancelDrag();


    public virtual bool OnCanDrag
    {
        get{
            return GlobalSetting.instance.CanControlPlayer(PlayerOwner);
        //    return true;
        }

    }



   

    //Check Player Position
    public virtual Players PlayerOwner{
        get{ 
            if (tag.Contains("Low"))
        {
            return GlobalSetting.instance.lowPlayer;
        }
        else if (tag.Contains("Top"))
        {
            return GlobalSetting.instance.topPlayer;
        }
            return null;
    }
    }
    protected abstract bool DraggingSuccess();

   
}