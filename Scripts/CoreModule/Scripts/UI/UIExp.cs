using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers;
public class UIExp : MonoBehaviour
{
   public Slider expBar;
   public UIPanel panel;
   public  Text stasText;


void Update(){
    Players p = Players.localPlayer;

    if(p!=null){
        panel.gameObject.SetActive(true);
        //expBar.value= p.ExpPercExpPrecentent();
        //   stasText.text="Lv."+p.level+"("+(p.ExpPercExpPrecentent()*100).ToString()+"%)";
    }else{
        panel.gameObject.SetActive(false);
    }
}
}
