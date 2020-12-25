using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;


//TODO
public class UICharacterInfo : MonoBehaviour
{
    public GameObject content;
    public Players entity;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI armorText;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI staminaText;

    public TextMeshProUGUI blockPercText;
    public TextMeshProUGUI critPercText;
    public TextMeshProUGUI flashPercText;

    public TextMeshProUGUI shieldText;
    public TextMeshProUGUI kaText;
    public TextMeshProUGUI lpText;
    public TextMeshProUGUI sciText;
    public TextMeshProUGUI deText;
    public TextMeshProUGUI ldText;



    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        Players p = Players.localPlayer;

        //
        if(p!=null && content.activeSelf)
        {


            //(0,*max)
            healthText.text = p.healthMax.ToString();
            manaText.text = p.manaMax.ToString();
            staminaText.text = p.Stamina.ToString();
            //
           damageText.text = p.damage.ToString();
            armorText.text = p.armor.ToString();
            //
            blockPercText.text = p.blockPerc.ToString();
            critPercText.text = p.critPerc.ToString();
            flashPercText.text = p.flashPerc.ToString();
            shieldText.text = p.aShield.ToString();
            //
            kaText.text = p.kissass.ToString();
            lpText.text = p.lockpick.ToString();
            sciText.text = p.science.ToString();
            deText.text = p.dungeoneering.ToString();
            ldText.text = p.leader.ToString();
          


        }
    }
}
