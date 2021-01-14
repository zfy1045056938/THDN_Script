using UnityEngine;
using System.Collections;

public class DealDamageToTarget : SpellEffect 
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
    
        target.MaxHealth -= target.MaxHealth;
    }
}