﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class NetworkBehaviourNonAlloc : NetworkBehaviour
{
    private string cachedName;

    public new string name
    {
        get
        {
            if (string.IsNullOrWhiteSpace(cachedName))
            {
                cachedName = base.name;
            }

            return cachedName;
        }
        set { cachedName = base.name = value; }
    }
}
