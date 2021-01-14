using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.Zone;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ArtifactIcon : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public ArtifactType type;
    public int amount;
    public Image icon;

    //For Tooltip
    public TextMeshProUGUI titleText;
    
    
    public BlurManager manager;

    public void OnPointerEnter(PointerEventData eventData)
    {
       manager.BlurInAnim();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       manager.BlurOutAnim();
    }
}
