using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public class CollectState : MonoBehaviour
{
   //属性系数
   public static float _STR_NUM = 1.0f;
   public static float _DEX_NUM =1.2f;
   public static float _INT_NUM=1.3f;
   public static float _ATK_NUM=2.0f;
   
   public static float _MUT_NUM=2.0f;
   public static float _ARMOR_NUM=1.5f;
   public static float _MATCH4_NUM=3.0f;
   public static float _MATCH5_NUM=4.0f;

    

   public GPType match;
   public int num;
   public  TextMeshProUGUI text;
   public int damageCollect;
   public int armorCollect;
   public int spdCollect;

   private Players p;

  public void UpdateUI()
   {
      p= Players.localPlayer;
      text.text=num.ToString();
   }
  
   }

  
