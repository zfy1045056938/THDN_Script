using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellRelootDamage : SpellEffect
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        Debug.Log("RELOOT SCRIPT ACTIVE");
        int amount =TurnManager.instance.WhoseTurn.playerArea.handVisual.CardsInHand.Count;
        int rnd = Random.Range(0, amount);
//        int rnd = Random.Range(0,amount);
        while(amount > 0){
      new DiscardACardCommand(TurnManager.instance.WhoseTurn,rnd).AddToQueue();
      amount--;
        }
        TurnManager.instance.WhoseTurn.DrawACard();
    }

   
   

    public override void ActiveRoundEffect(int amount = 0, ICharacter target = null, int roundTime = 0, DamageElementalType type = DamageElementalType.None)
    {
        base.ActiveRoundEffect(amount, target, roundTime, type);
    }

    public override void CauseEventEffect()
    {
        base.CauseEventEffect();
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void RegisterEventEffect()
    {
        base.RegisterEventEffect();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public override void UnRegisterEventEffect()
    {
        base.UnRegisterEventEffect();
    }
}
