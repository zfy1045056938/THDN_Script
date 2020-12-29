using System.Collections;
using System.Collections.Generic;
using PixelCrushers;
using UnityEngine;
using UnityEngine.UI;
public class UIShortCut : MonoBehaviour
{
   public static UIShortCut instance;
   public UIPanel panel;
   public Button inventoryBtn;
   public UIPanel inventory;
   public Button equipmentBtn;
   public UIPanel equipment;
   public UIPanel expBar;

   void Start()
   {
      instance = this;
   }

}
