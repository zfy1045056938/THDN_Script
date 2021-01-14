using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatuureRoundDamage : CreatureEffect
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CreatuureRoundDamage(Players owner, CreatureLogic creature, int specialAmount,int round,SpellBuffType sbt,DamageElementalType det): base(owner, creature, specialAmount,round,sbt,det)
    {
    }
}
