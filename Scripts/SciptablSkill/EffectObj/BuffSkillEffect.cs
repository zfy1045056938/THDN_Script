using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Collections.Generic;
using System;



public class BuffSkillEffect:SkillEffect
    {
    float lastRemainingTime = Mathf.Infinity;
    [SyncVar, HideInInspector] public string buffName;

    void Update()
    {
        // only while target still exists, buff still active and hasn't been
        // recasted
        if (target != null)
        {
            int index = target.GetBuffIndexByName(buffName);
            if (index != -1)
            {
                Buffs buff = target.buffs[index];
                if (lastRemainingTime >= buff.BuffTimeRemaining())
                {
                    transform.position = target.collider.bounds.center;
                    lastRemainingTime = buff.BuffTimeRemaining();
                    return;
                }
            }
        }

        // if we got here then something wasn't good, let's destroy self
        if (isServer) NetworkServer.Destroy(gameObject);
    }
}

