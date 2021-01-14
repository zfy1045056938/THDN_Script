using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;


public class TopCharacterInfo : MonoBehaviour
{
   private PlayerData playerData;

   [Header("UI")] 
   public TextMeshProUGUI levelText;
   public TextMeshProUGUI moneyText;
   public TextMeshProUGUI dustText;
   public TextMeshProUGUI specText;
   public TextMeshProUGUI gemText;
  public TextMeshProUGUI nameText;
   public static TopCharacterInfo instance;

 

   void Start()
   {
      if (instance == null) instance = this;
      playerData = FindObjectOfType<PlayerData>().GetComponent<PlayerData>();
   }

   void Update()
   {
        nameText.text = playerData.name;
         levelText.text =  playerData.PlayerLevel.ToString();
         moneyText.text = Mathf.Clamp(playerData.money,0,playerData.money).ToString(); 
         dustText.text = Mathf.Clamp(playerData.dust,0,playerData.dust).ToString();
         gemText.text = Mathf.Clamp(playerData.playerGem,0,playerData.playerGem).ToString();
         specText.text = Mathf.Clamp(playerData.special,0,playerData.special).ToString();
      
   }
}
