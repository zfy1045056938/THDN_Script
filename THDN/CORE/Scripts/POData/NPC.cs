
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using Unity.Mathematics;
using System.Collections.Generic;

[RequireComponent(typeof(NetworkMeshAgent))]
public class NPC : Entity
{
    public ScriptableItem[] items;
    public Transform teleportPro;
    public bool byPassNpcDialoguePanel = false;
    
    public DSRewardAuthorization rewardAuthorization=new DSRewardAuthorization();

    public bool offersSummableRec;
    [Server]
    public override string UpdateServer()
    {
        return state;
    }

  
    [Client]
    protected override void UpdateClient()
    {
        health = healthMax;
        
        Util.InvokeMany(GetType(),this,"UpdateClient_");
        
    }

    public void StartConversationStart(Transform p)
    {
        if (!isServer) return;
        RegisterRewardAu(p.GetComponent<Players>());
    }

    public void OnConversationEnd(Transform p)
    {
        if (!isServer) return;
        UnRegisterRewardAu(p.GetComponent<Players>());
    }

    public void UnRegisterRewardAu(Players p)
    {
        if (p == null || !rewardAuthorization.enforceAuthorization) return;
        p.dsAddPlayerGold = preValidAddPlayerGold;
    }

    protected DSValideInt preValidAddPlayerGold;
    public void RegisterRewardAu(Players p)
    {
        if (p == null || !rewardAuthorization.enforceAuthorization) return;
        preValidAddPlayerGold = p.dsAddPlayerGold;
    }

    public override string UpdateClient_IDLE()
    {
        throw new System.NotImplementedException();
    }


    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override string ToString()
    {
        return base.ToString();
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

    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
        return base.OnSerialize(writer, initialState);
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        base.OnDeserialize(reader, initialState);
    }

   

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
    }

    public override void OnStopAuthority()
    {
        base.OnStopAuthority();
    }

  

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override bool IsWorthUpdate()
    {
        return base.IsWorthUpdate();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    
    protected override void UpdateOverlays()
    {
        base.UpdateOverlays();
    }

    public override void DealDamageToTarget(Entity entity, float amount, float fTime = 0, float lTime = 0, DamageType type = DamageType.Normal, ElementDamageType det = ElementDamageType.None)
    {
        base.DealDamageToTarget(entity, amount, fTime, lTime, type, det);
    }

    public override void OnAggro(Entity e)
    {
        base.OnAggro(e);
    }





    public override void OnDeath()
    {
        base.OnDeath();
    }

    public override void Warp(Vector3 pos)
    {
        throw new System.NotImplementedException();
    }

    public override int damage
    {
        get { return base.damage; }
    }

    public override int armor
    {
        get { return base.armor; }
    }

    public override int healthMax
    {
        get { return base.healthMax; }
    }

  

    public override int aShield => base.aShield;


    public override int lockpick => base.lockpick;

    public override int science => base.science;

    public override int leader => base.leader;


    public override float heavy => base.heavy;

    public override int dungeoneering => base.dungeoneering;

    public override float manaMax { get => base.manaMax; set => base.manaMax = value; }

    public override float crit => base.crit;

    public override float speed => base.speed;

    public override float blockChance => base.blockChance;

   
}

public class DSRewardAuthorization
{
    public bool enforceAuthorization = false;
    public ScriptableItem[] items;
    public int goldMax = 9999;
}