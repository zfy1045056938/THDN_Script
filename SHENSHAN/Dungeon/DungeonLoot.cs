using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class DungeonLoot : Entity
{


    public List<Items> lootItems;
    
    private DungeonExplore explore;
    public Animation anim;
    public DungeonLoot()
    {
    }

    public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
    {
        base.DeserializeSyncVars(reader, initialState);
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool InvokeCommand(int cmdHash, NetworkReader reader)
    {
        return base.InvokeCommand(cmdHash, reader);
    }

    public override bool InvokeRPC(int rpcHash, NetworkReader reader)
    {
        return base.InvokeRPC(rpcHash, reader);
    }

    public override bool InvokeSyncEvent(int eventHash, NetworkReader reader)
    {
        return base.InvokeSyncEvent(eventHash, reader);
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        base.OnDeserialize(reader, initialState);
    }

    public override void OnNetworkDestroy()
    {
        base.OnNetworkDestroy();
    }

    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
        return base.OnSerialize(writer, initialState);
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public override void OnStopAuthority()
    {
        base.OnStopAuthority();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
    }

    public override bool SerializeSyncVars(NetworkWriter writer, bool initialState)
    {
        return base.SerializeSyncVars(writer, initialState);
    }

    public override string ToString()
    {
        return base.ToString();
    }

    protected override void UpdaetClient()
    {
        throw new System.NotImplementedException();
    }

    protected override string UpdateServer()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        explore = GetComponent<DungeonExplore>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


     private void OnMouseDown() {
         //Start Animation show Item
        
         DungeonExplore.instance.ShowLoot(lootItems);
    anim.Play();

    }
}
