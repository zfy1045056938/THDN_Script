using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制卡牌管理页面
/// </summary>
public class CardCollectionMenuManager : MonoBehaviour {

    public static CardCollectionMenuManager instance;
    public CharacterPanel playerInfoManager;
    public CastleManager castleManager;
    public CraftCardManager craftCardManager;


    private void Awake()
    {
        instance = this;
    }


    
}
