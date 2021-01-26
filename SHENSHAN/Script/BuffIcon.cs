using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Michsky.UI.Zone;
public class BuffIcon : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
  public DamageElementalType det;
  public SpellBuffType spType;
  public Image buffSprite;
  public Text roundTime;
  public Text amountText;
  public int round;
  public int amount;
  public TextMeshProUGUI tText;
  public TextMeshProUGUI dText;
  public BlurManager manager;
  public bool firstBuff=true;

    public void OnPointerEnter(PointerEventData eventData)
    {
       manager.BlurInAnim();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        manager.BlurOutAnim();
    }
}
