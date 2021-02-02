using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellKill : SpellEffect
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {


    //    int rv = Random.Range(0,TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.Count);

     
       target.MaxHealth -= target.MaxHealth;
    }



   
}
