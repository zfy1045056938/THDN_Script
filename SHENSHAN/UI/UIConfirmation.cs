using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// 确认窗口,当需要进行事件确认时出发
/// </summary>
public class UIConfirmation : MonoBehaviour
{
  public static UIConfirmation instance;
  public GameObject panel;
  public TextMeshProUGUI messageText;
  public Button confirmBtn;


  public UIConfirmation()
  {
    if (instance==null)
    {
      instance = this;
    }
  }

  public void Show(string msg, UnityAction call)
  {
    panel.gameObject.SetActive(true);
    messageText.text = msg;
    confirmBtn.onClick.AddListener(call);
   
    
  }
}
