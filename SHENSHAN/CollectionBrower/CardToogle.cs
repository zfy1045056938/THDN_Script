using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
public class CardToogle:MonoBehaviour
{
    private Toggle t;

    private void Awake()
    {
        t = GetComponent<Toggle>();
    }
    public  void SetValue(bool v)
    {
        if (t!=null)
        {
            t.isOn = v;
        
        }
    }

    public void ValueChanged(){
        DeckBuilderScreen.instance.collectionBroswerScript.showingCardsPlayerDoesNotOwn = t.isOn;
    }
}