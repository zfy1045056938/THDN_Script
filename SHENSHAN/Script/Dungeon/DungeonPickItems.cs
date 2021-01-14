using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DungeonPickItems : MonoBehaviour,IPointerClickHandler
{
    public Items item;


    public Text itemText;
    public Image itemSprite;
    public Text numText;
    public int itemNumber;
    
 

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Add Item To Inventory");
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PlayerData.localPlayer.CmdAddItems(item.itemID);
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        Debug.Log("Add Item To Inventory");
         PlayerData.localPlayer.CmdAddItems(item.itemID);
            Destroy(this.gameObject);
    }

  
}
