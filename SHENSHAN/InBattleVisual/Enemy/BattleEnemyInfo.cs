using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleEnemyInfo : MonoBehaviour
{
    public static BattleEnemyInfo instance;
  public EnemyAsset p;
 
 public Text playerHealthText;

 
  public Text  nameText;
  //  public Text playerStrengthText;
  // public Text playerDexText;
  //   public Text playerMagicText;
    public Text playerFrText;
    public Text playerIrText;
    public Text playerPrText;
    public Text playerERText;
    
    public Text playerDamageText;
    public Text playerDefText;
    // public Text flashText;
    public Text spdText;
    public Text spdPercText;
    public Text hurtTauntText;

    public GameObject DetailObj;
    public GameObject EZObj;
    public Transform InitPos;
    public Transform viewPos;
    public Transform equipPos;
    public GameObject EquipmentObj;
    
    public List<GameObject> equipmentLists;
    

void Start(){
    instance=this;
 
}

void Update(){
    nameText.text = BattleStartInfo.SelectEnemyDeck.enemyAsset.EnemyName.ToString();
     // playerDexText.text= p.dex.ToString();
     //        playerMagicText.text =p.inte.ToString();
     //        playerStrengthText.text =p.str.ToString();


            playerIrText.text=p.ir.ToString();
            playerFrText.text=p.fr.ToString();
            playerPrText.text=p.pr.ToString();
            playerERText.text=p.er.ToString();

           


            // flashText.text=Mathf.FloorToInt(p.flashPerc * 100)+"%";
            spdText.text = p.extraSpellDamage.ToString();
          
}



    public void SetInfo(EnemyAsset player){
          if(player!=null){
           nameText.text =BattleStartInfo.SelectEnemyDeck.enemyAsset.EnemyName.ToString();
    
            // playerDexText.text= player.dex.ToString();
            // playerMagicText.text =player.inte.ToString();
            // playerStrengthText.text =player.str.ToString();


            playerIrText.text=player.ir.ToString();
            playerFrText.text=player.fr.ToString();
            playerPrText.text=player.pr.ToString();
            playerERText.text=player.er.ToString();

            playerDamageText.text=player.damage.ToString();
            playerDefText.text=player.def.ToString();
            playerHealthText.text = player.Health.ToString();


            // flashText.text=Mathf.FloorToInt(player.flashPerc * 100)+"%";
            spdText.text = player.extraSpellDamage.ToString();
          
        }
    }
}
