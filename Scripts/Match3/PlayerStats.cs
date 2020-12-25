using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using PixelCrushers;
using TMPro;
using UnityEngine.UI;


/// <summary>
/// Show Player Stats und equip info
/// skill info (3)
/// 
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;
    public UIPanel panel;
    private Players player;
    public TextMeshProUGUI HpText;
    public TextMeshProUGUI HpLeftText;
    public TextMeshProUGUI MpText;
    public TextMeshProUGUI MpLeftText;
    public TextMeshProUGUI AtkText;
    public TextMeshProUGUI spdText;
    public TextMeshProUGUI DefText;

    public TextMeshProUGUI TotalText;

    public Slider HpSlider;

    public Slider MpSlider;

    public bool isDead = false;
    
    // Start is called before the first frame update
    void Start()
    {
        player = Players.localPlayer;
        instance=this;
    }

    // Update is called once per frame
    void Update()
    {
        if (player!=null)
        {
            HpText.text = player.healthMax.ToString();
            HpSlider.value = player.HealthPercent();
            HpLeftText.text = player.health.ToString();
            MpSlider.value = player.MPPercent();
            MpText.text = player.manaMax.ToString();
            MpLeftText.text = player.mana.ToString();
            spdText.text=player.speed.ToString();
            AtkText.text = player.damage.ToString();
            DefText.text = player.armor.ToString();
            TotalText.text = player.damage.ToString();

        }
        
        
    }

    
    
    public float CalDamage(float v)
    {
        //base + match3collect + equipmentBouns +skill Effect+item Effect
        float total =player.damage+Mathf.RoundToInt(v);
        return total;
    }

    public void Init()
    {
        if (player != null)
        {
            isDead = false;
            HpText.text = player.healthMax.ToString();
            HpLeftText.text = player.health.ToString();
            Debug.Log("Hp"+player.health);
            
            MpText.text = player.manaMax.ToString();
            MpLeftText.text = player.mana.ToString();
            
            AtkText.text = player.damage.ToString();
            DefText.text = player.armor.ToString();
            TotalText.text="0";

            //Load SKill


            //Load Equipment


            //Check EE

            //
        }
    }

   public void UpdateStats()
   {
       HpLeftText.text = player.health.ToString();
       
   }
}
