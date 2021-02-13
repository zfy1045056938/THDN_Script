using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Database))]
[RequireComponent(typeof(NetworkAuthenticator))]
[RequireComponent(typeof(TelepathyTransport))]

public class NetworkmanagerWTS : NetworkBehaviour
{
    
    
    public override void OnStopClient()
    {
        base.OnStopClient();
    }

    public override void OnStartServer()
    {
       
        
        Util.InvokeMany(typeof(NetworkmanagerWTS),this,"OnStartServer_");
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
    }

    public override void OnStartClient()
    {
        Util.InvokeMany(typeof(NetworkmanagerWTS),this,"OnStartServer_");
    }

  
}
