using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.Mathematics;
using TMPro;
using System.Reflection;
using System.Linq;



public class GpPools:MonoBehaviour
    {
    public GamePiece[] gp;
    public static Dictionary<int, GPType> gpDic = new Dictionary<int, GPType>();


    private void Start()
    {
        for(int i = 0; i < gp.Length;i++)
        {
            if (!gpDic.ContainsValue(gp[i].gpType))
            {
                gpDic.Add(i, gp[i].gpType);
            }
        }
    }
}
