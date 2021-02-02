
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class TurnMaker : MonoBehaviour
{
    protected Players p;

    void Awake()
    {
        p = GetComponent<Players>();
    }

    public virtual void OnTurnStart()
    {
        // add one mana crystal to the pool;
       GlobalSetting.instance.EnableEndTurnButtonOnStart(p);
        p.OnTurnStart();
    }
}