using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;


// 
//
public class DiscoverManager : MonoBehaviour
{
    public static DiscoverManager instance;
    public GameObject cardParent;
    public List<OneCardManager> managers;
    private bool showDiv = false;

    public bool ShowDiv
    {
        get { return showDiv; }
    }

    public UIPanel panel;

    public int cardIndex;
    public CardAsset[] allCardArray;
    public Dictionary<string, CardAsset> cardDic = new Dictionary<string, CardAsset>();

    public GameObject selectPrefab;

    public Button OpenBtn;
    public Button CloseBtn;
    public Text showText;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        instance = this;

        //
        // allCardArray=Resources.LoadAll<CardAsset>("");
        // allCardArray =Resources.LoadAll<CardAsset>("");
        // foreach(CardAsset  c in allCardArray){
        //     if(cardDic.ContainsKey(c.name)){
        //         cardDic.Add(c.name,c);

        //     }
        // }

    }

    /// <summary>
    /// second player for player will select rnd ,player default selectIndex is -1
    /// </summary>
    /// <param name="ca"></param>
    /// <param name="index"></param>
    /// <param name="type"></param>
    /// <param name="selectIndex"></param>
    public void ShowDiscover(List<CardAsset> ca, int index, DiscoverType type, int selectIndex=-1)
    {
        showText.text = "选择" + index + "张牌";
        OpenBtn.gameObject.SetActive(true);
        CloseBtn.gameObject.SetActive(false);
        panel.Open();
        ApplyLookToCards(ca, type);
        //Check the operation of the turn
        
        if(BattleStartInfo.SelectEnemyDeck!=null){
        //In Battle Scene 
        if (TurnManager.instance.WhoseTurn == GlobalSetting.instance.topPlayer)
        {
            panel.gameObject.SetActive(false);
            Sequence s = DOTween.Sequence();
            s.Append(cardParent.transform.DORotate(new Vector3(0f, -179f, 0f), 0.1f));
            s.AppendInterval(0.4f);
            ButtonHandler(1);
            
        }
        else
        {
            cardParent.SetActive(true);
            showDiv = true;
        }
        }else{
            
        }
        //AI select rnd second card
        if(selectIndex != -1)
        {
            Debug.Log("Second Player Select Card to hand");
            ButtonHandler(selectIndex);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="cs"></param>
    /// <param name="type"></param>
    public void ApplyLookToCards(List<CardAsset> cs = null, DiscoverType type = DiscoverType.None)
    {

        cs = new List<CardAsset>();
        var cards = from c in CardCollection.instance.allCardsArray where c.typeOfCards==TypeOfCards.Creature select c  ;
        
        
        switch (type)
        {
            case DiscoverType.Rnd:
                cs = CardCollection.instance.allCardsArray.ToList();
                SetDType(cards.ToList());

                break;
            case DiscoverType.Oppenent:
                cs = BattleStartInfo.SelectEnemyDeck.cards;
                SetDType(cards.ToList());
                break;
            case DiscoverType.HardMode:
                //Hard Mode set as a tmp card to the current card Collection und add to the 
                //tmp edit panel for player who can select card to pack und battle with enemy
                cs = CardCollection.instance.allCardsArray.ToList();
                SetDType(cards.ToList());
                break;
            case DiscoverType.SecondPlayer:
                cs = GlobalSetting.instance.secondList.ToList();
                SetDType(cs);
                break;
           
            default:
                break;
        }

    }

    public void SetDType(List<CardAsset> cs)
    {


//        for (int c = 0; c < cs.Count; c++)
//        {
            int rnd = Random.Range(0, cs.Count);
            OneCardManager om = new OneCardManager();
//            if (cs[rnd].name == cs[c].name)
//            {
                //Add To Selection manager
                //cs[c] is targetcard added to selection
                for (int i = 0; i < managers.Count; i++)
                {
                    //Create Card for empty slot
//                        om = new OneCardManager
//                        {
//                            cardAsset = cs[rnd],
//                        };

                        managers[i].cardAsset = cs[rnd];

                        managers[i].ReadCardFromAsset();
                        if (managers[i].gameObject != null)
                        {
                            if (managers[i].cardAsset.typeOfCards == TypeOfCards.Spell)
                            {
                                Debug.Log("Card is spell hide them =>\t" + managers[i].cardAsset.name);
                                managers[i].atkSprite.gameObject.SetActive(false);
                                managers[i].defSprite.gameObject.SetActive(false);
                                managers[i].healSprite.gameObject.SetActive(false);
                                // managers[i].typeText.gameObject.SetActive(false);
                            }

                            managers[i].gameObject.tag = "DiscoverCard";
                            ++cardIndex;
                            managers[i].gameObject.AddComponent<SelectionIndex>().GetComponent<SelectionIndex>().index =
                                cardIndex;
                            managers[i].name = cs[rnd].name;
                            rnd = Random.Range(0, cs.Count);
                        }
                }
//                }
            
//        }
    
    }

    public void HideDis()
    {
        showDiv = false;
        CloseBtn.gameObject.SetActive(false);
        panel.Close();
        OpenBtn.gameObject.SetActive(true);
    }

    //Choose card index then add to your hand,
    public void ButtonHandler(int indexOfCard)
    {
        if(BattleStartInfo.SelectEnemyDeck!=null){
        if(indexOfCard!=-1){
            selectPrefab = this.gameObject;
            selectPrefab.gameObject.tag="LowCard";      //Single Mode TODO
            Debug.Log(selectPrefab.name+"has clicked");

            if(managers[indexOfCard].cardAsset!=null){
                CardLogic cl = new CardLogic(managers[indexOfCard].cardAsset);
                //
                TurnManager.instance.WhoseTurn.GetACardNotFromDeck(managers[indexOfCard].cardAsset);
            }
            //Finish The spell return
            Reset();
        }else{
            Debug.Log("not index");
        }
         panel.Close();
        }else{
            //Reward stage Select card as tmp to tmp edit
            if(indexOfCard!=-1){
                //selectCard
                if(managers[indexOfCard].cardAsset!=null){
                    //Add to DungeonCE as temp destory when explore over
                    DungeonCardEdit.instance.AddCollectionAsTemp(managers[indexOfCard]);
                    panel.gameObject.SetActive(false);
                }
            }
        }

    }

    void Reset(){
        cardIndex=-1;
        selectPrefab=null;
        for (int i = 0; i < managers.Count; i++)
        {
            if (managers[i].cardAsset != null)
            {
                managers[i].cardAsset = null;
                managers[i].name = "Dic" + i;
            }
            Destroy(managers[i].GetComponent<SelectionIndex>());
        }
        
    }

    public void HidePanel()
    {
        panel.gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        panel.gameObject.SetActive(true);
    }
}
