using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndTurnButton : MonoBehaviour,IPointerClickHandler {
    public static bool interactable { get;  set; }

   

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button==PointerEventData.InputButton.Left)
        {
            TurnManager.instance.EndTurn();
        }
    }
}
