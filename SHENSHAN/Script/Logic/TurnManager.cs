using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using PixelCrushers.DialogueSystem;


// this class will take care of switching turns and counting down time until the turn expires
public class TurnManager : MonoBehaviour {

    // PUBLIC FIELDS
    public CardAsset CoinCard;

    // for Singleton Pattern
    public static TurnManager instance;


    // a static array that will store both players, should always have 2 players
    public static Players[] Players ;

    // PRIVATE FIELDS
    // reference to a timer to measure 
    private RopeTimer timer;

   
    
    // PROPERTIES
    private Players whoseTurn;
    public Players WhoseTurn
    {
        get
        {
            return whoseTurn;
        }

        set
        {
            whoseTurn = value;
            timer.StartTimer();
           
            //
            DialogueManager.SendUpdateTracker();
            // GlobalSetting.instance.EnableEndTurnButtonOnStart(whoseTurn);

            TurnMaker tm = whoseTurn.GetComponent<TurnMaker>();
            // player`s method OnTurnStart() will be called in tm.OnTurnStart();
            tm.OnTurnStart();
            if (tm is PlayerTurnMaker)
            {
               GlobalSetting.instance.EndBtn.enabled=true;
                whoseTurn.HighlightPlayableCards();
            }
            // remove highlights for opponent.
            whoseTurn.otherPlayer.HighlightPlayableCards(true);
                
        }
    }


    // METHODS
//    void Awake()
//    {
//        Players = GameObject.FindObjectsOfType<Players>();
//        instance = this;
//        timer = GetComponent<RopeTimer>();
//    }



    void Start()
    {
        GameDebug.Init(Application.dataPath,"BattleLog");
        CardCollection.instance.LoadCard();
        Players = GameObject.FindObjectsOfType<Players>();
        instance = this;
        timer = GetComponent<RopeTimer>();
        Debug.Log("Turn Start");

        if (GlobalSetting.instance.lowPlayer != null)
        {
            Debug.Log("LowPlayer has Data");
        }
        //foreach (Players p in Players)
        //{
        //    Debug.Log(p.name);
            
        //}

      foreach (string q in QuestLog.GetAllQuests())
              {
                  if (q != null)
                  {
                      Debug.Log(q);
                  }
                  else
                  {
                      Debug.Log("No quest track now ");
                  }
              }


        OnGameStart();

       

    }

    /// <summary>
    /// CORE MANAGER THE MODULE 
    /// </summary>
    public void OnGameStart()
    {
        //Clear Old Logs
        foreach(var l in GlobalSetting.instance.tpList){
            if(l!=null){Destroy(l);}
        }
          GlobalSetting.instance.SETLogs("------The Doors Log System-------");
          GlobalSetting.instance.SETLogs("------lynchhead-------");
          GlobalSetting.instance.SETLogs("------"+System.DateTime.Now+"-------");
   
        CardLogic.CardsCreatedThisInGame.Clear();
        CreatureLogic.creatureCreatedThisGame.Clear();

        foreach (Players p in Players)
        {
            p.manaThisTurn = 0;
            p.manaLeft = 0;
            p.discardPool.panel.Open();


            if (p == GlobalSetting.instance.lowPlayer)
            {
                //Load player
                //chanlog 123
                //if have weapon set to common damage card but different to armor card
                // weapon equals atk card
                //armor not equals def card und it's  will increase by card effect(Armor)
                p.LoadCharacterInfoFromAsset();
                p.AddDungeonBouns();
                p.LoadStatsFromdata();
                //p.playerArea.playerPortraitVisual.weapon.LoadItems();
                //p.playerArea.playerPortraitVisual.ring.LoadItems();
                
                GlobalSetting.instance.lowPlayer.playerArea.playerPortraitVisual.LoadStatsFromAsset();
                p.LoadBattleInfo(BattleStartInfo.player);
                p.playerArea.playerPortraitVisual.LoadNames();
                //
                Debug.Log("Load Player Done");
            }
            else if(p==GlobalSetting.instance.topPlayer){
             p.LoadEnemyAssetFromVisual(p);
             Debug.Log("Load Enemy Done");
               p.LoadBattleInfoEnemy(BattleStartInfo.SelectEnemyDeck.enemyAsset);
                //   p.playerArea.playerPortraitVisual.weapon.LoadItems();
                //p.playerArea.playerPortraitVisual.ring.LoadItems();
             
            }
            
   
            p.TransmitInfoAboutPlayerToVisual();
           //
           if(p==GlobalSetting.instance.lowPlayer)
           {
                p.playerArea.pDeck.cardInDeck = p.deck.cards.Count;
           }
           else if(p==GlobalSetting.instance.topPlayer)
           {
               p.enemyArea.pDeck.cardInDeck = p.deck.cards.Count;
           }
           
           //
           
            // move both portraits to the center
            p.playerArea.portraitPosition.transform.position = p.playerArea.InitialPortraitPosition.position;
        }
        
        //Enemy Module Load Data To The Target object TODO

        Sequence s = DOTween.Sequence();
//        //Prepard Stage:: Player Show Name und Move To target place
//        s.Append(Players[0].playerArea.portraitPosition.transform.DOLocalMove(Players[0].playerArea.portraitPosition.position, 1f).SetEase(Ease.InQuad));
//        s.Insert(0f, Players[1].playerArea.portraitPosition.transform.DOLocalMove(Players[1].playerArea.portraitPosition.position, 1f).SetEase(Ease.InQuad));
//        //
//        s.Append(Players[0].playerArea.InitialPortraitPosition.transform.DOMove(Players[0].playerArea.portraitPosition.position, 1f).SetEase(Ease.InQuad));
//        s.Insert(0f, Players[1].playerArea.InitialPortraitPosition.transform.DOMove(Players[1].playerArea.portraitPosition.position,1f).SetEase(Ease.InQuad));
//        s.PrependInterval(1.5f);

        // StartCoroutine(GiveCardS());
       s.OnComplete(() =>
       {
           // determine who starts the game.
           int rnd = Random.Range(0, 2); // 2 is exclusive boundary
           Debug.Log("rnd is "+rnd);
           // Debug.Log(Players.Playerss.Length);
           Players whoGoesFirst = Players[rnd];
           Debug.Log(whoGoesFirst);
           Players whoGoesSecond = whoGoesFirst.otherPlayer;
           Debug.Log(whoGoesSecond);

           // draw 4 cards for first player and 5 for second player
           //changelog 123  ->lastest card module
           // player deck totally have 20 card(10 customize + 10 common(5+5))
           // if equipment have init effect , as a card for equipment effect  set to hand in the first round
           // according to equipment effect it will check effect every the turn step(draw,play,attack,end,next)
           // the items card can add to discard pool until player call the dp
           // different to oppenent , extra 
           int initDraw = 6;    // init draw 6 (3+3+n!)n<=3 -> 9
           for (int i = 0; i < initDraw; i++)
           {
               // second player draws a card
               whoGoesSecond.DrawACard(true);
               // first player draws a card
               whoGoesFirst.DrawACard(true);
               //
//               whoGoesSecond.GetACardNotFromDeck((CoinCard));
           }
           

           // add one more card to second player`s hand
           whoGoesSecond.DrawACard(true);
            
           //
//           GlobalSetting.instance.lowPlayer.GetACardNotFromDeck(CoinCard);
//           
         

           new StartATurnCommand(whoGoesFirst).AddToQueue();
       });
       //

    }

    public IEnumerator GiveCardS()
    {
        Debug.Log(Players.Length);
        foreach(Players s in Players){
            Debug.Log(s.name+"\n\n");
        }
        int rnd = Random.Range(0,2);  // 2 is exclusive boundary
        Debug.Log("rnd"+rnd);
        Players whoGoesFirst = Players[rnd];
        Debug.Log(whoGoesFirst);
        Debug.Log(rnd+"\n\n\n\n");
        Players whoGoesSecond =whoGoesFirst.otherPlayer;
        Debug.Log(whoGoesSecond);

        int initDraw = 4;
        for (int i = 0; i < initDraw; i++)
        {
            // second player draws a card
            whoGoesSecond.DrawACard(true);
            // first player draws a card
            whoGoesFirst.DrawACard(true);
        }
        // add one more card to second player`s hand
        whoGoesSecond.DrawACard(true);
        new StartATurnCommand(whoGoesFirst).AddToQueue();
        
        yield return new WaitForSeconds(0.4f);
    }
   
   

    // FOR TEST PURPOSES ONLY
    public void EndTurnTest()
    {
        timer.StopTimer();
        timer.StartTimer();
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void EndTurn()
    {
        //discover spell end
    //    DiscoverManager dis =FindObjectOfType<DiscoverManager>();
    //    for (int i=0;i<dis.panel.transform.childCount;i++){
    //         dis.panel.transform.GetChild(i).gameObject.SetActive(false);
    //    }


        Draggable[] AllDraggableObjects = GameObject.FindObjectsOfType<Draggable>();
        foreach (Draggable d in AllDraggableObjects)
            d.CancelDrag();
        // stop timer
        timer.StopTimer();
        // send all commands in the end of current player`s turn
        whoseTurn.OnTurnEnd();
        //
       

        new StartATurnCommand(whoseTurn.otherPlayer).AddToQueue();


      
    }
   

    public void StopTheTimer()
    {
        timer.StopTimer();
    }

}

