using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;



//战斗场景元素管理包含以下元素
//1.背景以及交互功能
//2。玩家元素(owner,creatureInDeck,Castle,woker)
//3.敌人元素(enemyGroup, Camp,ExtraResource,)
public class AreaPosition:MonoBehaviour{

    public Players TopPlayer;
    public Players LowPlayers;
    public AreaPositions owner;

    //If PVP
    public Castle topCastle;
    public Castle lowCastle;


    
}