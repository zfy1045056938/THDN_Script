using System;
using UnityEngine;
using Mirror;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using TMPro;
using DungeonArchitect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;


/// <summary>
/// 
/// </summary>
public class Battle_CharacterInfo:Singleton<Battle_CharacterInfo>
{
    
    public Entity entity;
    [Header("STATS")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public Slider healthSlider;
    public Slider manaSlider;
    public Slider rageSlider;
    //
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI flashText;
    public TextMeshProUGUI critText;
    public TextMeshProUGUI spdText;
    public TextMeshProUGUI rageText;


    //Others 
    public TextMeshProUGUI diceText;


    private void Start()
    {
        healthSlider.value = entity.healthMax;
        manaSlider.value = entity.manaMax;
        //rageSlider.value = entity.rage;
    }

    private void Update()
    {


        if (entity != null) {
              healthText.text= entity.healthMax.ToString();
     manaText.text =entity.manaMax.ToString();
     damageText.text =entity.damage.ToString();
    armorText.text = entity.armor.ToString();
    flashText.text=entity.blockChance.ToString()+"%";
     critText.text=entity.crit.ToString()+"%";
   spdText.text= entity.speed.ToString();
  //rageText.text =entity.rage.ToString();

} else
        {
            
        }
    }


    public void ResetInfo()
    {
        healthText.text = "0";
        manaText.text = "0";
        damageText.text = "0";
        armorText.text = "0";
        flashText.text = "0";
        critText.text = "0";
        spdText.text = "0";
        rageText.text = "0";
    }


    /// <summary>
    /// Init Player Stats to UI when first load dungeon
    /// 
    /// </summary>
    /// <param name="entity"></param>
    public void Init(Players entity)
    {

        if (entity != null)
        {
            healthText.text = entity.healthMax.ToString();
            manaText.text = entity.manaMax.ToString();
            damageText.text = entity.damage.ToString();
            armorText.text = entity.armor.ToString();
            flashText.text = entity.blockChance.ToString() + "%";
            critText.text = entity.crit.ToString() + "%";
            spdText.text = entity.speed.ToString();
            //rageText.text = entity.rage.ToString();
        }
        }
}