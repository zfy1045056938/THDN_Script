using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArea : MonoBehaviour
{
       public AreaPositions owner; //self position
        public bool ControlsOn =true;
        public PlayerDeckVisual pDeck;  //桌面
                public HandVisual handVisual;    //手部
        public EnemyPortraitVisual playerPortraitVisual;   //英雄
        public HeroPowerBtn heroPowerBtn;   //英雄技能
     public CastleManager castleManager;  //城堡
    //public WorkerManager workerManager;   //工人
    public TableVisual tableVisual;         
        public Transform portraitPosition;
        public Transform InitialPortraitPosition;
    public ManaPoolVisual manaThisTurn;
    public BattleEnemyInfo info;
}
