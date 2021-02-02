using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
    
public class WeaponSkill : MonoBehaviour,IPointerClickHandler
{
    public AreaPositions owner;
    public GameObject content;
    public GameObject iconPanel;
    public GameObject usedPanel;
    public Items weapon;

    private int _mana;
    public int payMana {
        get { return _mana; }
        set { _mana = value; } }
    public Text manaText;
    public Text atkText;
    public Text atkDurText;
     public Image glow;
     public string useEffectScriptName;
  
    public SpellEffect spellEffect;

 private bool wasUsed=false;

 public bool WasUsed
 {
     get { return wasUsed; }
     set
     {
         wasUsed = value;
         if (wasUsed==false)
         {
            usedPanel.gameObject.SetActive(false); 
         }
         else
         {
             usedPanel.gameObject.SetActive(true);
             
         }
     }
 }

 private bool hightLight;
 public bool Highlight
 {
     get { return hightLight; }
     set
     {
         hightLight = value;
         glow.gameObject.SetActive(hightLight);
     }
 }
 
 

 public void LoadItems(Players p)
    {
        if (weapon != null&&weapon.effectCardName !=null)
        {
              var card = CardCollection.instance.GetCardAssetByName(weapon.effectCardName);
            if (card!=null) {
                CardLogic ca = new CardLogic(p,card);
                if (ca != null)
                {
                    //Add card to deck then shuffle it
                    p.deck.cards.Add(card);
                    p.deck.cards.Shuffle(); //shuffle the pack 
                }

                    }
            else
            {
                Debug.Log("Can't got cardasset");
            }
        }
        else
        {
           content.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (WasUsed == false  && payMana <= GlobalSetting.instance.lowPlayer.manaLeft)
            {
                // GlobalSetting.instance.lowPlayer.UseWeapon();
                // //
                // atkDurText.text = GlobalSetting.instance.lowPlayer.atkDur.ToString();
                // GlobalSetting.instance.lowPlayer.manaLeft-=  weapon.itemMana;
            }
        }
    }

    
}
