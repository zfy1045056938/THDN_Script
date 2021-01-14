using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;

/// <summary>
/// 搜索框进行关键词查询,
/// 1.根据随从卡牌关键字
/// 2.根据随从类型进行筛选
/// 3.根据稀有程度
/// 4.根据英雄类型
/// </summary>
public class KeyWordInputField:Singleton<KeyWordInputField>
{

    public InputField inputField;
   

    public  void Awake()
    {
        inputField = gameObject.GetComponent<InputField>();    

    }


    public void Clear()
    {

        this.inputField.text = "";
        DeckBuilderScreen.instance.collectionBroswerScript.Keyword = inputField.text;

    }

    public void Submit(){
        DeckBuilderScreen.instance.collectionBroswerScript.Keyword = inputField.text;
    }
}