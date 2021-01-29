using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class RingSkill : MonoBehaviour,IPointerClickHandler
{
 public GameObject content;
    public Items items;
    public int dur;

    public string effectName;
    public SpellEffect spellEffect;
   
    public GameObject iconPanel;
    public GameObject usedPanel;
    public Text ringDur;
    private int _mana;
    public int payMana {
        get { return _mana; }
        set { _mana = value; } }
    public Text manaText;
     public Image glow;
   
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
        if (items != null && items.useEffectScriptName != "")
        {

            #region old item struct 
            //ringDur.text =Mathf.FloorToInt(items.ringDur).ToString();

            //manaText.text = items.itemMana.ToString();

            ////active effect
            //if (items.useEffectScriptName != "" && items.useEffectScriptName != null)
            //{
            //    Debug.Log("Load items");
            //    spellEffect =
            //        System.Activator.CreateInstance(System.Type.GetType(items.useEffectScriptName)) as SpellEffect;
            //    spellEffect.owner = GlobalSetting.instance.lowPlayer;
            //    spellEffect.target = items.spellTarget;
            //}
            #endregion

            var card = CardCollection.instance.GetCardAssetByName(items.effectCardName);
            if (card != null)
            {
                CardLogic ca = new CardLogic(p, card);
                if (ca != null)
                {
                    //Add card to deck then shuffle it
                    p.deck.cards.Add(card);
                    p.deck.cards.Shuffle(); //shuffle the pack 
                }
            }
            else
            {
                iconPanel.gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (WasUsed == false  && payMana <= GlobalSetting.instance.lowPlayer.manaLeft)
            {
                // GlobalSetting.instance.lowPlayer.UseRing();
                //
                ringDur.text = dur.ToString();
                dur --;
                GlobalSetting.instance.lowPlayer.manaLeft-=  items.itemMana;

            }
        }
    }
}
