using System;
using UnityEngine;
using Mirror;
public class SelectableCharacter:MonoBehaviour{
    public int index=-1;
   

    void OnMouseDown()
    {
        ((NetworkManagerTHDN) NetworkManagerTHDN.singleton).selection = index;
        //
        GetComponent<Players>().SetIndicator(transform);
    }

    void Update()
    {
        if (((NetworkManagerTHDN)NetworkManagerTHDN.singleton).selection!= index)
        {
            Players p = GetComponent<Players>();
            Destroy(p.indicator);
        }
    }
}