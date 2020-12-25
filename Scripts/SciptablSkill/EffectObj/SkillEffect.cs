using UnityEngine;
using Mirror;

/// <summary>
/// Skilleffect obj show app when player cast skill target
/// </summary>
public abstract class SkillEffect : NetworkBehaviourNonAlloc
{
    // [SyncVar] NetworkIdentity: errors when null
    // [SyncVar] Entity: SyncVar only works for simple types
    // [SyncVar] GameObject is the only solution where we don't need a custom
    //           synchronization script (needs NetworkIdentity component!)
    // -> we still wrap it with a property for easier access, so we don't have
    //    to use target.GetComponent<Entity>() everywhere
    [SyncVar] GameObject _target;
    public Entity target
    {
        get { return _target != null ? _target.GetComponent<Entity>() : null; }
        set { _target = value != null ? value.gameObject : null; }
    }

    [SyncVar] GameObject _caster;
    public Entity caster
    {
        get { return _caster != null ? _caster.GetComponent<Entity>() : null; }
        set { _caster = value != null ? value.gameObject : null; }
    }
}