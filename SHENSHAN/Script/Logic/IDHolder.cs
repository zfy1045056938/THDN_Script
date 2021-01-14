using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class IDHolder : MonoBehaviour
{
    public int uniqueID;
    public static List<IDHolder> allIDHolders = new List<IDHolder>();

    private void Awake()
    {
        allIDHolders.Add(this);
    }

    public static GameObject GetComponentWithID(int ID){
        foreach (IDHolder i in allIDHolders)
        {
            if(i==null)continue;
            
            if (i.uniqueID == ID)
           
                return i.gameObject;
        }

        return null;
    }

    public static void ClearIDHolderList(){
        allIDHolders.Clear();
    }

}
