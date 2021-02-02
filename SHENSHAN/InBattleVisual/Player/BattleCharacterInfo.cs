using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class BattleCharacterInfo : MonoBehaviour
{
 public static BattleCharacterInfo instance;
 public Players p;
 
 public Text playerHealthText;

 
  public Text  nameText;
   public Text playerStrengthText;
  public Text playerDexText;
    public Text playerMagicText;
    public Text playerFrText;
    public Text playerIrText;
    public Text playerPrText;
    public Text playerERText;
    
    public Text playerDamageText;
    public Text playerDefText;
    public Text flashText;
    public Text spdText;
    public Text spdPercText;
    public Text hurtTauntText;

    public GameObject DetailObj;
    public GameObject EZObj;
    public Transform InitPos;
    public Transform viewPos;
    public Transform equipPos;
    public GameObject EquipmentObj;
    public Image itemImage;
    
    public List<GameObject> equipmentLists;
    

void Start(){
    instance=this;
 
}

void Update(){
    //  playerDexText.text= p.DEX.ToString();
    //         playerMagicText.text =p.INTE.ToString();
    //         playerStrengthText.text =p.STR.ToString();


            playerIrText.text=p.IR.ToString();
            playerFrText.text=p.FR.ToString();
            playerPrText.text=p.PR.ToString();
            playerERText.text=p.ER.ToString();

            playerDamageText.text=p.CreatureAtk.ToString();
            playerDefText.text=p.CreatureDef.ToString();
            playerHealthText.text = p.MaxHealth.ToString();

            //extra
            // flashText.text=Mathf.FloorToInt(p.flashPerc * 100)+"%";
            spdText.text = p.ExtraSpellDamage.ToString();
            // spdPercText.text=Mathf.FloorToInt(p.sdPerc *100) +"%";
            // hurtTauntText.text=p.hurtDef.ToString();
}



    public void SetInfo(PlayerData player){
          if(player!=null){
           nameText.text =BattleStartInfo.SelectEnemyDeck.enemyAsset.EnemyName.ToString();
    
            // playerDexText.text= player.Dex.ToString();
            // playerMagicText.text =player.Magic.ToString();
            // playerStrengthText.text =player.Strength.ToString();


            playerIrText.text=player.IR.ToString();
            playerFrText.text=player.FR.ToString();
            playerPrText.text=player.PR.ToString();
            playerERText.text=player.ER.ToString();

            playerDamageText.text=player.atk.ToString();
            playerDefText.text=player.ArmorDef.ToString();
            playerHealthText.text = player.playerHealth.ToString();


            // flashText.text=Mathf.FloorToInt(player.extraFlash * 100)+"%";
            spdText.text = player.extraSpellDamage.ToString();
            // spdPercText.text=Mathf.FloorToInt(player.extra *100) +"%";
            //  hurtTauntText.text=p.hurtDef.ToString();
       
        }
    }

    public void ShowDetail(){
       
        DetailObj.transform.DOLocalMoveY(-13f,1.0f);
        GameObject obj =null;
        if(BattleStartInfo.Weapon!=null){
            obj = Instantiate(EquipmentObj,equipPos.position,Quaternion.identity)as GameObject;
            obj.transform.SetParent(equipPos);
            obj.GetComponent<BattleEquipmentView>().items = BattleStartInfo.Weapon;
            obj.GetComponent<BattleEquipmentView>().NText.text = BattleStartInfo.Weapon.itemName.ToString();
            
            obj.GetComponent<BattleEquipmentView>().detailText.text = BattleStartInfo.Weapon.descriptionText.ToString();

            obj.GetComponent<BattleEquipmentView>().itemImage.sprite = BattleStartInfo.Weapon.icon;
            equipmentLists.Add(obj);
        }else if(BattleStartInfo.Armor!=null){
                 obj = Instantiate(EquipmentObj,equipPos.position,Quaternion.identity)as GameObject;
            obj.transform.SetParent(equipPos);
            obj.GetComponent<BattleEquipmentView>().items = BattleStartInfo.Weapon;
            obj.GetComponent<BattleEquipmentView>().NText.text = BattleStartInfo.Weapon.itemName.ToString();
            
            obj.GetComponent<BattleEquipmentView>().detailText.text = BattleStartInfo.Weapon.descriptionText.ToString();

            obj.GetComponent<BattleEquipmentView>().itemImage.sprite = BattleStartInfo.Weapon.icon;
            equipmentLists.Add(obj);
        }else if(BattleStartInfo.Ring!=null){
                 obj = Instantiate(EquipmentObj,equipPos.position,Quaternion.identity)as GameObject;
            obj.transform.SetParent(equipPos);
            obj.GetComponent<BattleEquipmentView>().items = BattleStartInfo.Weapon;
            obj.GetComponent<BattleEquipmentView>().NText.text = BattleStartInfo.Weapon.itemName.ToString();
            
            obj.GetComponent<BattleEquipmentView>().detailText.text = BattleStartInfo.Weapon.descriptionText.ToString();

            obj.GetComponent<BattleEquipmentView>().itemImage.sprite = BattleStartInfo.Weapon.icon;
            equipmentLists.Add(obj);
        }
    }

    public void HideDetail(){
       
        DetailObj.transform.DOLocalMoveY(25f,1.0f);
    }
}
