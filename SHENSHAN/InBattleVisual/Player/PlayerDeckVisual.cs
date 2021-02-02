using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 用于生成新卡进入手部(card->hand)
/// </summary>
public class PlayerDeckVisual:MonoBehaviour,IPointerEnterHandler,IPointerExitHandler

{

    public AreaPositions owner;  //player position
    public float highOfOneCard = 0.012f;
    public Text numberTxt;

    public void Start()
    {
       
        cardInDeck = GlobalSetting.instance.playerInGame[owner].deck.cards.Count;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter)
        {
            numberTxt.GetComponent<Text>().gameObject.SetActive(true);
            numberTxt.text = GlobalSetting.instance.playerInGame[owner].deck.cards.Count.ToString();
        }else{
            numberTxt.gameObject.SetActive(false);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging==false)
        {
            numberTxt.gameObject.SetActive(false);
        }
    }



    private int _cardInDeck = 0;
    public  int cardInDeck{
        get { return _cardInDeck; }
        set{
            _cardInDeck = value;
            transform.localPosition = new Vector3(0, 0, -highOfOneCard*value);
        }
    }

   
}
