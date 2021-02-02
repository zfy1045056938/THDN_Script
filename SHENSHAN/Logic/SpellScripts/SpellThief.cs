using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mirror.Examples.Pong;
using UnityEngine;


/// <summary>
/// 窃取,随机3张对方牌库的并获取一张
/// </summary>
public class SpellThief : SpellEffect
{
    public override void ActiveEffect(int specialAmount = 0, ICharacter target = null)
    {
        
        Debug.Log("STEAL SPELL ACTIVE!!!!!!!!!!!!!!!!!!!!!");
       
        new DiscoverCommand(TurnManager.instance.WhoseTurn.ID,specialAmount,DiscoverType.Oppenent).AddToQueue();

    }
}
