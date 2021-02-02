using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.EventSystems;

public enum DungeonEventType
{
    None,
    
    HEAL,
    CHEAL,

    ATK,
    CATK,

    CARMOR,
    HARMOR,
    
    ESD,
    WDUR,
    ERS
}

[System.Serializable]
public class DungeonEventClass{
    public int deID;
    public string deName;
    public string deDetail;
    public float dePerc;
    public int deAmount;
    public float DEReward;
    public int DAAmount;
    // public int deMoney;
    // public int deDust;
    // public int deExp;
    public Sprite deIcon;
    public DungeonEventType DET = DungeonEventType.None; 
    public CardRatityOption rarity = CardRatityOption.NORMAL;

  
    public DungeonEventClass(){}

    public DungeonEventClass(int deID, string deName, string deDetail, float dePerc, int deAmount, float dEReward, int dAAmount, Sprite deIcon, DungeonEventType dET, CardRatityOption rarity)
    {
        this.deID = deID;
        this.deName = deName;
        this.deDetail = deDetail;
        this.dePerc = dePerc;
        this.deAmount = deAmount;
        DEReward = dEReward;
        DAAmount = dAAmount;
        this.deIcon = deIcon;
        DET = dET;
        this.rarity = rarity;
    }
}

[System.Serializable]
public class DungeonCard : MonoBehaviour,IPointerEnterHandler,IPointerClickHandler,IPointerExitHandler
{
public CardAsset card;
 public DungeonEventClass dec;
  public Text nText;
  public Text ddText;
  public Text drText;
  public Image glow;
  public Image select;
  public Image iconSprite;
  public int index;

  public bool hasSelect=false;
    public void OnPointerClick(PointerEventData eventData)
    {
        //select

if(eventData.button ==  PointerEventData.InputButton.Left){

 select.gameObject.SetActive(true);
        DungeonEvent.instance.SelectthisCard(this,index);
        Select();

}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       glow.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
         glow.gameObject.SetActive(false);
    }


    public void DeSelect(){
        hasSelect =false;

        transform.DOScale(new Vector3(0.8f,0.8f,1),0.5f);
    }

    public void Select(){
        hasSelect=true;

        DungeonCard[] cards = FindObjectsOfType<DungeonCard>();
        foreach(var c in cards){
            if(c!=this){
                c.DeSelect();
            }
        }
        transform.DOScale(new Vector3(1.0f,1.0f,1.0f),0.5f);
        //
        DungeonEvent.instance.selectCard = this;
    }
}
