 using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// Hero power button.
/// </summary>
public class HeroPowerBtn:MonoBehaviour,IPointerClickHandler
{
    public AreaPositions owner;

    //
    public GameObject Front;
    public GameObject Back;
    public GameObject Glow;

    //
    private bool wasUsed = false;   // 是否使用
    private bool highlighted = false; //高领显示

    public bool WasUsed
    {
        get
        {
            return wasUsed;
        }

        set
        {
            wasUsed = value;
            //
            if (!wasUsed)
            {
                Front.SetActive(true);
                Back.SetActive(false);
            }else{
                Front.SetActive(false);
                Back.SetActive(true);
            }
        }
    }

    public bool Highlighted
    {
        get
        {
            return highlighted;
        }

        set
        {
            highlighted = value;
            Glow.SetActive(highlighted);
        }
    }


    /// <summary>
    /// Ons the pointer click.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button==PointerEventData.InputButton.Left)
        {
            if (!wasUsed && highlighted)
            {
                GlobalSetting.instance.playerInGame[owner].UseHeroPower();
                //
                wasUsed = !wasUsed;
            }
        }

    }
}