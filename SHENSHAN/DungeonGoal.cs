using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayfulSystems;
using PixelCrushers.DialogueSystem;
using PixelCrushers;
public class DungeonGoal : MonoBehaviour
{
    public UIPanel panel;
   public bool FinalBoss=false;
   public bool isFinish=false;
   public bool haveKey=false;
   public GameObject goalPanel;
   public Transform goalPos;
   public Text Current;
   public Text Goal;
   public ProgressBarPro currentSlider;



   void Update(){
    //    currentSlider.Value = DungeonManager.instance.dungeonCurrent;
   }

   
}
