using System.Collections;
using System.Collections.Generic;
using PixelCrushers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// discard pool
/// storge 
/// </summary>
public class DiscardPool : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
   public UIPanel panel;
   public AreaPositions owner;
   public Image HasCardSprite;
   public Image NocardSprite;
   public Text cardCount;
   public GameObject[] gameObj;
   public List<CardAsset> cardpool;
   public List<GameObject> cardObj;

   void Update()
   {

      if (panel.isActiveAndEnabled)
      {
         if (cardpool.Count > 0)
         {
            HasCardSprite.gameObject.SetActive(true);
            NocardSprite.gameObject.SetActive(false);
         }
         else
         {
            NocardSprite.gameObject.SetActive(true);
            HasCardSprite.gameObject.SetActive(false);
         }
      }
   }
//   public Dictionary<int,CardAsset> cardList =new Dictionary<int, CardAsset>();
   public void OnPointerEnter(PointerEventData eventData)
   {
      cardCount.text = GlobalSetting.instance.playerInGame[owner].discardPool.cardpool.Count.ToString();
      cardCount.gameObject.SetActive(true);
   }

   public void OnPointerExit(PointerEventData eventData)
   {
      cardCount.gameObject.SetActive(false);
   }
}
