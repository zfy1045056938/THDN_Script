using System.Collections.Generic;
using System.Collections;
using UnityEngine;


//控制战斗场面城堡要素,包含以下功能
//1.数据控制
//2.城堡效果生成
//3.资产激活
public class CastleVisual:MonoBehaviour{

    public CastlePrefab castlePrefab = null;
    public CastleManager castleManager;

    public void UseSkill(){}
    

    public bool GenerateMana(){
        return true;
    }

}