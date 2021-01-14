using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;


/// <summary>
/// 水晶筛选,水晶总共为30
/// 依次按照10+ 查询
/// 0-10 
/// 10-20
/// 21-30
/// 30+
/// </summary>
public class ManaFiliter:MonoBehaviour
{

 
    public Image[] info;

 
    public int currentIndex = -1;
    //根据职业分水晶类型
    //private CrystalsType crystalsType = CrystalsType.None;
    //private Jobs jobs = Jobs.None;
    //private CharacterAsset characterAsset;

    public Color32 hightlightColor = Color.white;
    public Color32 hideColor = Color.gray;
    public static ManaFiliter instance;
    public AudioClip click;
    /// <summary>
    /// Awake this instance.
    /// </summary>
    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        RemoveAllFiliter();
        currentIndex = -1;
        //CollectionBrower.instance.ManaCost = currentIndex;
    }
    /// <summary>
    /// Selects the mana filiter.
    /// </summary>
    public void SelectManaFiliter(int index)
    {
        RemoveAllFiliter();
        SoundManager.instance.PlayClipAtPoint(click, Vector3.zero, SoundManager.instance.musicVolume, false);
        if (index!=currentIndex)
        {
            currentIndex = index;
            info[index].color = hightlightColor;

        }else{
            currentIndex = -1;
        }
        CollectionBrower.instance.ManaCost = currentIndex;

    }


    /// <summary>
    /// Resets the mana.
    /// </summary>
    public void ResetMana(){

    }

    public void RemoveAllFiliter()
    {
        foreach (Image item in info)
        {
            item.color = hideColor;
        }
    }
}