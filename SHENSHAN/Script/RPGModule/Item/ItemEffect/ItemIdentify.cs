using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIdentify : UseEffect
{
    public override string Use()
    {
       
        inventory.startIdentifying = true;

        return "Identify";
    }
}
