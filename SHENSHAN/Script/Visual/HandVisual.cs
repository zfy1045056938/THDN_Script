using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public enum HandCardOperType{
    Keep,
    Throw,
}
//游戏中对玩家手部卡牌的管理包含以下功能
//1.发牌到玩家手中.初始为5张,每回合发一张,当超过最大持牌数(8)时，在下一张牌发出时,可进行对当前牌或者发牌丢弃
//2.手部牌进入牌池,或者发动卡牌触发效果,对卡牌类型进行判断
//3.当手部具有同名牌时,可触发特殊效果(针对特定职业卡)
public class HandVisual:MonoBehaviour{
    public AreaPositions  areaPosition;

    public bool TakeCardsOpenly =true;

    public SameDistanceChildren slots;

    private HandCardOperType  handCardOperType; 
    private int _limitOfCardOfHand = 8;
    public int LmitOfCardOfHand{
        get { return _limitOfCardOfHand; }

    }
    [Header("Transform Reference")]
    public Transform drawPrevewSpot;
    public Transform deckTransform;
    public Transform otherCardDrawSourceTransform;
    public Transform playerPreviewSpot;
    public Transform discardpoolPos;
    //
    public AudioClip giveCardSound;
    //
    public List<GameObject> CardsInHand = new List<GameObject>();
    public GameObject preview;
    public static HandVisual instance;


   
    

    /// <summary>
    /// Adds the card.
    /// </summary>
    /// <param name="card">Card.</param>
    public void AddCard(GameObject card){
        //TODO 123
        if (CardsInHand.Count <= 9)
        {
            CardsInHand.Insert(0, card);
            //
            //card.transform.SetParent(slots.transform);
            //card.transform.SetParent(slots.GetComponent<Transform>().parent);
            //
            card.transform.SetParent(slots.transform);
            //card.transform.localScale = slots.transform.localScale; 
            card.transform.localScale = new Vector3(2, 2, 3);
            //
            PlaceCardsOnNewslots();
            //Update Hand Slots
            UpdatePlacementOfslots();
        }
        else
        {
            TurnManager.instance.WhoseTurn.DiscardCardAtIndex(0);
        }
    }

    /// <summary>
    /// Removes the card.
    /// </summary>
    /// <param name="card">Card.</param>
    public void RemoveCard(GameObject card){

        CardsInHand.Remove(card);
        //
        new DelayCommand(0.5f).AddToQueue();
        
            PlaceCardsOnNewslots();
            UpdatePlacementOfslots();
    }

    /// <summary>
    /// Gets the index of the card at.
    /// </summary>
    /// <returns>The card at index.</returns>
    /// <param name="index">Index.</param>
    public GameObject GetCardAtIndex(int index){
        return CardsInHand[index];
    }  


    //moving card to pllayerHand and Check the CardLimit is Full
    public void UpdatePlacementOfslots(){
        float posX;
        if(CardsInHand.Count>0){
            posX = (slots.children[0].transform.localPosition.x - 
                    slots.children[CardsInHand.Count-1].transform.localPosition.x)/2f;
            
        }else {
            posX = 0f;
        }

        //
        slots.gameObject.transform.DOLocalMoveX(posX,0.3f);
    }

    //将卡牌防止手部栏位
    public void PlaceCardsOnNewslots(){
            foreach(GameObject g in CardsInHand){
            g.transform.DOLocalMoveX(slots.children[CardsInHand.IndexOf(g)].localPosition.x,0.3f);
            //Set Ording
            WhereIsTheCardOfCreature w =g.GetComponent<WhereIsTheCardOfCreature>();
                w.slot = CardsInHand.IndexOf(g);
                w.SetHandSortingOrder();
            }
    }

    //玩家出牌后在桌面上显示随从卡牌,如有特殊效果则在出牌后触发效果
    public GameObject CreateCardAtPosition(CardAsset c,Vector3 posistion,Vector3 eularAngles){
            GameObject card =null;

        //当卡牌类型为随从时则创建随从
        if (c.cardHealth > 0|| c.creatureType==CardType.Machine)
        {
            card = Instantiate(GlobalSetting.instance.CreatureCardPrefab, posistion, Quaternion.Euler(eularAngles)) as GameObject;
        }
        else
        {
           
            //群体卡无箭头直接触发,none is common 
            if (c.target==TargetOptions.AllCharacter ||c.target==TargetOptions.AllCreature || c.target == TargetOptions.EmenyCreature || c.target == TargetOptions.Castle|| c.target==TargetOptions.None)
            {
                card = Instantiate(GlobalSetting.instance.NoTargetSpellCardPrefab, posistion, Quaternion.Euler(eularAngles)) as GameObject;
            }
            //指向性法术
            else if(c.target == TargetOptions.Creature || c.target == TargetOptions.EmenyCharacter || c.target == TargetOptions.YoursCharacters 
                    )
            {
                card = Instantiate(GlobalSetting.instance.TargetSpellCardPrefab, posistion, Quaternion.Euler(eularAngles)) as GameObject;

                DragSpellOnTarget dragSpell = card.GetComponentInChildren<DragSpellOnTarget>();
                dragSpell.targets = c.target;
            }
        }

        OneCardManager oneCard = card.GetComponent<OneCardManager>();
            oneCard.cardAsset=c;
            Debug.Log(oneCard.cardAsset.name.ToString());
            oneCard.ReadCardFromAsset();    //read asset from mutiDocument
        
        
        return card;
  
    }

    //发给玩家卡牌，在以下情况发生效果
    //1.在回合结束后,在确认玩家或者cpu结束回合时则发牌员进行发牌操作
    //2.卡牌效果出发,在战吼或者亡语触发时调用.
    //3.角色特殊效果触发,开始发牌
    //4.当玩家到达最大持牌数时如果继续进行发牌行为则发生爆牌行为，如果所持有卡牌数为空则友军伤害收到加成效果,
    //5.工人所产生的特殊效果，在玩家指定工人的劳动行为时,在进行探险操作时有几率触发发牌操作.
    //6.野外资源可能产生效果
    public void GivePlayerACard(CardAsset c,int uniqueID,bool fast =false,bool fromDeck=true,bool isLimitOfHandCard=false,Transform pos=null,bool fromdis=false){
        GameObject card;
       
        SoundManager.instance.PlayClipAtPoint(giveCardSound, Vector3.zero, SoundManager.instance.musicVolume, false);
        //Particle Effect
        GlobalSetting.instance.drawCardEffect.Play();
        if(fromDeck)    //判断是否来自牌桌还是其他来源
        {
          
            card =CreateCardAtPosition(c,deckTransform.position,new Vector3(0f,-179f,0f));
            card.transform.tag=areaPosition.ToString()+"Card";
//            card.transform.root.tag=areaPosition.ToString()+"Card";
           
        }else{
            //Discover or other spell give card
            //Preivew Play Card 
            card=CreateCardAtPosition(c,otherCardDrawSourceTransform.position,new Vector3(0f,-179f,0f));
            card.transform.SetParent(slots.transform);
            card.transform.localScale=Vector3.one;
//            card.transform.root.tag=areaPosition.ToString()+"Card";
            card.transform.tag=areaPosition.ToString()+"Card";
//            card.transform.root.tag=areaPosition.ToString()+"Card";
        }

        // card.transform.localScale=slots.transform.localScale;
        foreach(Transform t in card.GetComponent<Transform>().root){
            t.root.tag = areaPosition.ToString()+"Card";
            t.tag = areaPosition.ToString() + "Card";
        }

        //
        card.transform.localScale = Vector3.one;
        //scale
        //card.transform.localScale = slots.transform.position;
        //骰子效果触发,
        //发牌员
        AddCard(card);

        //Set Card Layer To the top
        WhereIsTheCardOfCreature w = card.GetComponent<WhereIsTheCardOfCreature>();
        w.BringToFront();
        w.slot =0;
        w.visualState= VisualStates.Transition;
       
        ////
        //Core Module
        ////
        IDHolder ids= card.AddComponent<IDHolder>();
        ids.uniqueID=uniqueID;
       

        Sequence s =DOTween.Sequence();
        //将牌发入手中
        if (!fast)
        {
            //
            s.Append(card.transform.DOLocalMove(drawPrevewSpot.position,
            GlobalSetting.instance.CardTransitionTime));
            
            //
            if (TakeCardsOpenly)
                // s.Insert(0f, card.transform.DORotate(Vector3.zero, GlobalSetting.instance.CardTransitionTime));
                 s.Insert(0f, card.transform.DORotate(Vector3.zero, GlobalSetting.instance.CardTransitionTime));
            else
                s.Insert(0f, card.transform.DORotate(new Vector3(0f, 179f, 0f), GlobalSetting.instance.CardTransitionTime));
            
            s.AppendInterval(GlobalSetting.instance.CardPreviewTime);
            //Set
            s.Append(card.transform.DOLocalMove(slots.children[0].transform.localPosition,
                GlobalSetting.instance.CardTransitionTime));
           
        }
        else
        {
//            s.Insert(0f,card.transform.DOLocalMove(drawPrevewSpot.position,
//                GlobalSetting.instance.CardTransitionTime));
            s.Append(card.transform.DOLocalMove(slots.children[0].transform.localPosition,
            GlobalSetting.instance.CardTransitionTimeSet));
            if (TakeCardsOpenly)
            {
                s.Insert(0f, card.transform.DOLocalRotate(Vector3.zero, GlobalSetting.instance.CardTransitionTimeSet));
            }else{
                s.Insert(0f,card.transform.DOLocalRotate(new Vector3(0f,-179f,0f),GlobalSetting.instance.CardTransitionTimeFast));
            }

        }
        
            s.PrependInterval(0.4f);
            s.OnComplete(() => { ChangeLastCardStatusToInHand(c, w); });
    }
    //
    void ChangeLastCardStatusToInHand(CardAsset card,WhereIsTheCardOfCreature w){
        if(areaPosition ==AreaPositions.Low){
            w.visualState =VisualStates.LowHand;
            }else{
            w.visualState=VisualStates.TopHand;
            }

        w.SetHandSortingOrder();
        
        Command.CommandExecutionComplete();
          

    }


    /// <summary>
    /// Plaies AS pell from hand.
    /// </summary>
    /// <param name="handID">Hand identifier.</param>
    public void PlayASpellFromHand(int handID){
        GameObject card =IDHolder.GetComponentWithID(handID);
//        TurnManager.instance.WhoseTurn.discardPool.cardObj.Add(card);
        PlayASpellFromHand(card);


    }
    
    /// <summary>
    /// freeze creature at round num(),when 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="amount"></param>
    /// <param name="freeze"></param>
    public void FreezeCreature(int targetID, int amount, bool freeze)
    {

        CreatureLogic cl = new CreatureLogic
        {
            UniqueCreatureId = targetID,
        };
        
        if (cl != null)
        {
            cl.IsFreeze = freeze;
            cl.AttacksForThisTurn -= amount;
            
        }
    }


    /// <summary>
    /// Plaies AS pell from hand.
    /// </summary>
    /// <param name="cardVisual">Card visual.</param>
    void PlayASpellFromHand(GameObject cardVisual){
        
        
//        Command.CommandExecutionComplete();
        //
        cardVisual.GetComponent<WhereIsTheCardOfCreature>().visualState =VisualStates.Transition;
        //
        RemoveCard(cardVisual);
        //
//        cardVisual.transform.SetParent(playerPreviewSpot.transform);
//        
//        cardVisual.transform.localScale=new Vector3(0.1f,0.1f,0.1f);
//        cardVisual.transform.SetAsFirstSibling();
//        cardVisual.gameObject.layer =0;
//        Sequence s = DOTween.Sequence();
//        
////        s.Append(cardVisual.transform.DOMove(playerPreviewSpot.position, 1.0f).SetEase(Ease.Linear))
////            
////            .Insert(1f,cardVisual.transform.DORotate(new Vector3(0f, -179f, 0f), 3.0f))
////            .Join(cardVisual.transform.DOMove(discardpoolPos.position, 3f).SetEase(Ease.Linear));
////           
//       
//            s.Append(cardVisual.transform.DOMove(playerPreviewSpot.position, 1.0f));
//            s.Insert(0f,cardVisual.transform.DORotate(Vector3.zero, 1.0f));
//            s.Insert(1f, cardVisual.transform.DOScale(new Vector3(0.9f, 0.9f, 0.5f), 0.3f));
//            s.Join(cardVisual.transform.DOMove(discardpoolPos.position, 1f).SetEase(Ease.Linear));
       //Set preview to the spot und Generate Card for preview
       GameObject obj = Instantiate(preview,drawPrevewSpot.position,Quaternion.identity)as GameObject;
       obj.transform.SetParent(drawPrevewSpot);
       obj.transform.localScale=new Vector3(0.1f,0.1f,1.0f);
       obj.GetComponent<CardPreview>().PreviewCard(cardVisual.GetComponent<OneCardManager>());
        
        cardVisual.transform.parent = discardpoolPos;
       

//       s.Append( cardVisual.transform.DOMove(playerPreviewSpot.position, 5.0f));
//        
//            s.Insert(1f, cardVisual.transform.DORotate( new Vector3(0f,-179f,0f), 2.0f));
//         
//        
//            cardVisual.transform.parent = discardpoolPos;
//            s.Append(cardVisual.transform.DOLocalMove(discardpoolPos.position,2.0f));
//            

//        s.AppendInterval(1.0f);
//        //
//        s.OnComplete(() =>
//        {
            Command.CommandExecutionComplete();
            Destroy(cardVisual);
//        });
    }
    /// <summary>
    /// discard effect target move to discard pool
    /// </summary>
    /// <param name="index"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void DiscardCardAtIndex(int index)
    {
       Debug.Log("Discard card is "+index);
       GameObject obj = CardsInHand[index];
       //
       RemoveCard(obj);

       Sequence s = DOTween.Sequence();
       s.Append(obj.transform.DOMove(GlobalSetting.instance.playerInGame[areaPosition].playerArea.discardPool.transform.position,1f));
       s.Insert(0f, obj.transform.DORotate(new Vector3(0, -179, 0), 1f));
       //
       s.OnComplete(() =>
       {
          Destroy(obj);
           Command.CommandExecutionComplete();
       });
       //
       UpdatePlacementOfslots();
       PlaceCardsOnNewslots();
    }
}