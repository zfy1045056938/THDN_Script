using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class IdentifyControl : NetworkBehaviour
{
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
    }
}
