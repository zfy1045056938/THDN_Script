using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public enum AreaPos{Top,Low}    //地区位置
/// <summary>
/// Player area.
/// </summary>
public class PlayerArea:MonoBehaviour
{
    public static PlayerArea instance;
        public AreaPositions owner; //self position
        public bool ControlsOn =true;
        public PlayerDeckVisual pDeck;  //桌面
        public HandVisual handVisual;    //手部
        public PlayerPortraitVisual playerPortraitVisual;   //英雄
        public HeroPowerBtn heroPowerBtn;   //英雄技能
     public CastleManager castleManager;  //城堡
    //public WorkerManager workerManager;   //工人
    public TableVisual tableVisual;         
        public Transform portraitPosition;
        public Transform InitialPortraitPosition;
    public ManaPoolVisual manaThisTurn;
   public DiscardPool discardPool;
   public  BattleCharacterInfo bci;
  

    public bool AllowedToControllThisPlayer{get;set;}

    void Awake(){
        instance=this;

    }
        

}