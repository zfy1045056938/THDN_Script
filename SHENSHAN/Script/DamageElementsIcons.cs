using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class buffImages
{
    public Sprite bi;
    public DamageElementalType det;
}



public class DamageElementsIcons : MonoBehaviour
{
    public buffImages[] iconData;

     public Dictionary<DamageElementalType,Sprite> detDic=new Dictionary<DamageElementalType, Sprite>();

     public static DamageElementsIcons instance;


     void Awake()
     {
         foreach (buffImages d in iconData)
         {
             detDic.Add(d.det, d.bi);

         }
     }
}
