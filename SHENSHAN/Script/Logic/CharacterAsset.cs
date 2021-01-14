using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

//角色资源管理
//包含头像图片资源
//基本属性
[System.Serializable]
public class CharacterAsset
{
    public PlayerJob jobs;
    public string className;
    public int maxHealth = 30;
    public string heroPowerName;
    public Sprite avatarImage;
    public string description;
    public Sprite avatarBGImage;

    public CharacterAsset(PlayerJob jobs, string className, int maxHealth, string heroPowerName, Sprite avatarImage, string description, Sprite avatarBGImage)
    {
        this.jobs = jobs;
        this.className = className;
        this.maxHealth = maxHealth;
        this.heroPowerName = heroPowerName;
        this.avatarImage = avatarImage;
        this.description = description;
        this.avatarBGImage = avatarBGImage;
    }
    public CharacterAsset(){}


    // public int strength;    //力量
    // public int wisdom;  //智慧
    // public int dexterity;//敏捷
    // public int constitution;    //体格

    // public int damage;
    // public int defence; //防御力

    // public int fireResistance;
    // public int iceResistance;
    // public int poisonResistence;
    // public int electronicReststance;
    // public int trapResistance;
    // public int leaderPower;



    // public Sprite heroPowerBGImage;
    // public Color32 avatarBGTint;
    // public Color32 heroPowerBGTint;
    // public Color32 classCardTint;
    // public Color32 classRibbonsTint;


}