using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public  class IDFactory
{
    public static int Count;
    public PlayerData players;
    public static int GetUniqueID()
    {
        Count++;
        //PlayerPrefs.GetString("PlayerName", player);
        return Count;
    }

    public static void ResetID()
    {
        Count = 0;
    }
}