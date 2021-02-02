using System.Collections;
using System.Collections.Generic;
using PixelCrushers;
using UnityEngine;

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
    public CollectState[] statePool;
    public MatchValue match;

    private PlayerData p;
    public void Start()
    {
        instance = this;
        p=FindObjectOfType<PlayerData>();
    }

    void StatePool()
    {
        statePool = GetComponents<CollectState>();
    }

    public void Init()
    {
        foreach (CollectState state in statePool)
        {
            if (state != null)
            {
                state.num = 0;
            }

        }
    }

    /// <summary>
    /// Record it to the panel then show ui and cal damage for player
    /// </summary>
    public void CollectRecord(MatchValue value,int v)
    {

        // float CSTR=0f;
        // if (statePool != null)
        // {
        //     for (int i = 0; i < statePool.Length; i++)
        //     {
        //         //match 3 
        //         if (statePool[i].match == value )
        //         {
        //             switch (value)
        //             {
        //                 case MatchValue.STR:
        //                     statePool[i].match = value;
        //                     statePool[i].num += 1;
        //                     statePool[i].UpdateUI();
        //                     CSTR= CollectDmage(value);
        //                    PlayerStats.instance.CalDamage(CSTR);
        //                     break;
        //                 case MatchValue.DEX:
        //                     statePool[i].match = value;
        //                     statePool[i].num += 1;
        //                     statePool[i].UpdateUI();
        //                       CSTR= CollectDmage(value);
        //                    PlayerStats.instance.CalDamage(CSTR);
        //                     break;
        //                 case MatchValue.INT:
        //                     statePool[i].match = value;
        //                     statePool[i].num += 1;
        //                     statePool[i].UpdateUI();
        //                       CSTR= CollectDmage(value);
        //                    PlayerStats.instance.CalDamage(CSTR);
        //                     break;
        //                 case MatchValue.SWORD:
                                 
        //                                                 statePool[i].match = value;
        //                                                 statePool[i].num += v;
        //                                                 statePool[i].UpdateUI();
        //                                                   CSTR= CollectDmage(value);
        //                    PlayerStats.instance.CalDamage(CSTR);
        //                                                 break;
        //                 case MatchValue.ARMOR:
        //                     statePool[i].match = value;
        //                     statePool[i].num += v;
        //                     statePool[i].UpdateUI();
        //                       CSTR= CollectDmage(value);
        //                    PlayerStats.instance.CalDamage(CSTR);
        //                     break;
        //                 case MatchValue.MUT:
        //                     statePool[i].match = value;
        //                     statePool[i].num += v;
        //                     statePool[i].UpdateUI();
        //                       CSTR= CollectDmage(value);
        //                    PlayerStats.instance.CalDamage(CSTR);
        //                     break;
                        

        //             }


        //         }
        //     }

        // }
    }
     //when match3 collect piece and cal
   public float CollectDmage(MatchValue value){
      
      // float damageCommon=0f;
      // switch(value){
      //    case MatchValue.STR:
      //        damageCommon = 10+(p.level-1)*_STR_NUM;
      //      return damageCommon;
            
      //    break;
      //     case MatchValue.DEX:
      //        damageCommon = 10+(p.level-1)*_DEX_NUM;
      //      return damageCommon;
            
      //    break;
      //     case MatchValue.INT:
      //        damageCommon = 10+(p.level-1)*_INT_NUM;
      //      return damageCommon;
            
      //    break;
      //     case MatchValue.MUT:
      //        damageCommon = 10+(p.level-1)*_MUT_NUM;
      //      return damageCommon;
            
      //    break;
      //     case MatchValue.SWORD:
      //        damageCommon = 10+(p.level-1)*_ATK_NUM;
      //      return damageCommon;
            
      //    break;
      //     case MatchValue.ARMOR:
      //        damageCommon = 10+(p.level-1)*_ARMOR_NUM;
      //      return damageCommon;
            
      //    break;
        
      // }
 return 0; 
}
    
    
}

 

