using System.Collections;
using System.Collections.Generic;
using PixelCrushers;
using UnityEngine;
using UnityEngine.UI;
using PlayfulSystems.ProgressBar;
using PlayfulSystems;
public class UIExp : MonoBehaviour
{
    public static UIExp instance;
    public UIPanel panel;
    public ProgressBarPro slider;
    public Text stausText;

    void Start()
    {
        instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        PlayerData p = PlayerData.localPlayer;
        if (p != null)
        {
            panel.Open();
            slider.Value =p.exppercent();
            stausText.text="Lv."+p.PlayerLevel+"("+(p.exppercent()*100).ToString("F2")+"%)";
        }
        else
        {
            panel.Close();
        }
    }
}
