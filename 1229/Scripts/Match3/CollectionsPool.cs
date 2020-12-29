using System.Collections;
using System.Collections.Generic;
using PixelCrushers;
using UnityEngine;
using Mirror;
using TMPro;

/// <summary>
/// 
/// </summary>
public class CollectionsPool : MonoBehaviour
{ 
    //属性系数
   public static float _STR_NUM = 1.0f;
   public static float _DEX_NUM =1.2f;
   public static float _INT_NUM=1.3f;
   public static float _ATK_NUM=2.0f;
   
   public static float _MUT_NUM=2.0f;
   public static float _ARMOR_NUM=1.5f;
   public static float _MATCH4_NUM=3.0f;
   public static float _MATCH5_NUM=4.0f;

    public static CollectionsPool instance;
    public UIPanel panel;
    public GamePieces[] statePool;


    public Dictionary<int,GamePiece> csp = new Dictionary<int, GamePiece>();    //when cast skill check dic[p].amount

    //
    public Transform cpPos;
    public GameObject cgpObj;
    
    public GPType match;

    private Players p;
    public void Start()
    {
        instance = this;
        p=Players.localPlayer;
    }

   
    ///  <summary>
    ///     Load Cp pieces und reset as Obj
    /// </summary>
    public void Init()
    {
        if (statePool.Length > 0)
        {
            for(int i = 0; i < statePool.Length; i++)
            {
                //generate obj
                GameObject obj =EmeraldAIObjectPool.Spawn(cgpObj, cpPos.position, Quaternion.identity) as GameObject;
                obj.transform.SetParent(cpPos);
                obj.transform.localScale = Vector3.one;
                //bind data
                obj.GetComponent<GamePieces>().amount = 0;
                obj.GetComponent<GamePieces>().gp.gpType = GpPools.gpDic[i];
               
                //
                NetworkServer.Spawn(obj);   
            }
        }
    }

    /// <summary>
    /// Record it to the panel then show ui and cal damage for player
    /// VFX generate target gp set to target pool
    /// GPType 
    /// </summary>
    public void CollectRecord(GPType value,int v)
    {

        float CSTR=0f;
        if (statePool != null)
        {
            for (int i = 0; i < statePool.Length; i++)
            {
                //match 3 
                if (statePool[i].gp.gpType == value )
                {
                    switch (value)
                    {
                        // case GPType.STR:
                            
                        //     statePool[i].match = value;
                        //     statePool[i].num += 1;
                        //     statePool[i].UpdateUI();
                        //     CSTR= CollectDmage(value);
                        //    PlayerStats.instance.CalDamage(CSTR);
                        //     break;
                        // case GPType.DEX:
                        //     statePool[i].match = value;
                        //     statePool[i].num += 1;
                        //     statePool[i].UpdateUI();
                        //       CSTR= CollectDmage(value);
                        //    PlayerStats.instance.CalDamage(CSTR);
                        //     break;
                        // case GPType.INTE:
                        //     statePool[i].match = value;
                        //     statePool[i].num += 1;
                        //     statePool[i].UpdateUI();
                        //       CSTR= CollectDmage(value);
                        //    PlayerStats.instance.CalDamage(CSTR);
                        //     break;
                        // case GPType.ATk:
                                 
                        //                                 statePool[i].match = value;
                        //                                 statePool[i].num += v;
                        //                                 statePool[i].UpdateUI();
                        //                                   CSTR= CollectDmage(value);
                        //    PlayerStats.instance.CalDamage(CSTR);
                        //                                 break;
                        // case GPType.ARMOR:
                        //     statePool[i].match = value;
                        //     statePool[i].num += v;
                        //     statePool[i].UpdateUI();
                        //       CSTR= CollectDmage(value);
                        //    PlayerStats.instance.CalDamage(CSTR);
                        //     break;
                    // 

                    }


                }
            }

        }
    }
     //when match3 collect piece and cal
   public float CollectDmage(GPType value){
      
      float damageCommon=0f;
      switch(value){
         case GPType.STR:
             damageCommon = 10+(p.level-1)*_STR_NUM;
           return damageCommon;
            
         break;
          case GPType.DEX:
             damageCommon = 10+(p.level-1)*_DEX_NUM;
           return damageCommon;
            
         break;
          case GPType.INTE
          :
             damageCommon = 10+(p.level-1)*_INT_NUM;
           return damageCommon;
            
         break;
         
          case GPType.ATk:
             damageCommon = 10+(p.level-1)*_ATK_NUM;
           return damageCommon;
            
         break;
          case GPType.ARMOR:
             damageCommon = 10+(p.level-1)*_ARMOR_NUM;
           return damageCommon;
            
         break;
        
      }
 return 0f;
}
    
    
}

 

