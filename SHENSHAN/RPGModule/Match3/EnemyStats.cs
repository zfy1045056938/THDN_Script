using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers;
using TMPro;
public class EnemyStats : MonoBehaviour
{
   public UIPanel panel;
   public Slider HP;
   
  public TextMeshProUGUI healthText;
  public TextMeshProUGUI healthLeftText;
  
   public TextMeshProUGUI DamageText;
   public TextMeshProUGUI armorText;
   public TextMeshProUGUI spdText;
   public TextMeshProUGUI enemyNameText;
   public TextMeshProUGUI enemyLevelText;
   public TextMeshProUGUI enemyRatityText;
   public TextMeshProUGUI TotalText;
   private Players players;
   void Start()
   {

       players = FindObjectOfType<Players>();
      
      
     
      
   }
//    void Update(){
//        DamageText.text=players.target.damage.ToString();
//        armorText.text=players.target.armor.ToString();
//        spdText.text=players.target.speed.ToString();

//        enemyNameText.text=players.target.name.ToString();
//        enemyLevelText.text=players.target.level.ToString();

//        healthText.text=players.target.healthMax.ToString();
//        healthLeftText.text=players.target.health.ToString();
       
//        HP.value = players.target.Healthper();
      
//        TotalText.text=CalDamage().ToString();
//    }
//     public void Init(Entity e)
//     {
//         DamageText.text=players.target.damage.ToString();
//        armorText.text=players.target.armor.ToString();
//        spdText.text=players.target.speed.ToString();

//        enemyNameText.text=players.target.name.ToString();
//        enemyLevelText.text=players.target.level.ToString();

//        healthText.text=players.target.healthMax.ToString();
//        healthLeftText.text=players.target.health.ToString();
       
//        HP.value = players.target.Healthper();
      
//        TotalText.text=CalDamage().ToString();
//     }

//     public int CalDamage(){
//         return players.target.damage;
//     }

//     public void UpdateStats()
//     {
//         healthLeftText.text = players.target.health.ToString();
//     }
}
