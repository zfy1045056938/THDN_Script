 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Linq;
using  PixelCrushers.DialogueSystem;
using TMPro;

namespace PixelCrushers.DialogueSystem{
public class CharacterInfoPanel : MonoBehaviour
{
    
    public KeyCode hotKey ;
    public UIPanel panel;
   
    private PlayerData player;

    [Header(("Common"))]
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerHealText;
  
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI playerStrengthText;

    public TextMeshProUGUI playerDexText;
    public TextMeshProUGUI playerMagicText;
    public TextMeshProUGUI playerFrText;
    public TextMeshProUGUI playerIrText;
    public TextMeshProUGUI playerPrText;
    public TextMeshProUGUI playerERText;
    public TextMeshProUGUI SpecialEffectText;
    public TextMeshProUGUI playerDamageText;
    public TextMeshProUGUI playerDefText;
    public TextMeshProUGUI WeaponDurText;
    public TextMeshProUGUI flashText;
    public TextMeshProUGUI spdText;
    
    public TextMeshProUGUI[] setList;


    private CharacterStat[] stats;
    
    //
    private bool showCharacterPanel =false;

        
   private void Start() {
       player=FindObjectOfType<PlayerData>().GetComponent<PlayerData>();
   }
     
    void Update(){
        
        if(player!=null){
            if(Input.GetKeyDown(hotKey)){
                {
                   panel.gameObject.SetActive(true);
                }
            }
            
            //Load PlayerStats
            // playerNameText.text =player.name.ToString();
            // playerLevelText.text = player.playerHealth.ToString();

            playerHealText.text = player.playerHealth.ToString();
            // playerDexText.text= player.Dex.ToString();
            // playerMagicText.text =player.Magic.ToString();
            // playerStrengthText.text =player.Strength.ToString();


            playerIrText.text=player.IR.ToString();
            playerFrText.text=player.FR.ToString();
            playerPrText.text=player.PR.ToString();
            playerERText.text=player.ER.ToString();

            playerDamageText.text=player.atk.ToString();
            WeaponDurText.text=player.atkCount.ToString();
            //
            playerDefText.text=player.ArmorDef.ToString();
           
            // flashText.text=Mathf.FloorToInt(player.extraFlash * 100)+"%";
          
            spdText.text = player.extraSpellDamage.ToString();
          
    
                        
        }else{
            Debug.Log("Can't Got Player\n\n");
        }
    }


    public void ClearData()
    {
        PlayerData.localPlayer.atk = 0;
        PlayerData.localPlayer.atkCount = 0;
        PlayerData.localPlayer.ArmorDef = 0;
        //
        // PlayerData.localPlayer.Strength = 0;
        // PlayerData.localPlayer.Dex = 0;
        // PlayerData.localPlayer.Magic = 0;
        //
        PlayerData.localPlayer.FR = 0;
        PlayerData.localPlayer.IR = 0;
        PlayerData.localPlayer.PR = 0;
        PlayerData.localPlayer.ER = 0;
       
        //
        // PlayerData.localPlayer.extraFlash = 0;
        PlayerData.localPlayer.extraSpellDamage = 0;
        // PlayerData.localPlayer.ESDPerc = 0;
    }


       
    
    public void CloseOrOpen(bool b)
    {
        
       panel.Open();
    }
}
}