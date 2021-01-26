using UnityEngine;
using System.Collections;
using DG.Tweening;
public class DrawACardCommand : Command {

    private Players p;
    private CardLogic cl;
    private bool fast;
    private bool fromDeck;

    private Transform pos;

    private bool fromDis;

    private int amount;

    public DrawACardCommand(CardLogic cl, Players p, bool fast, bool fromDeck)
    {        
        this.cl = cl;
        this.p = p;
        this.fast = fast;
        this.fromDeck = fromDeck;
    }

    public DrawACardCommand( CardLogic cl,Players p, bool fast, bool fromDeck, Transform pos, bool fromDis, int amount)
    {
        this.cl = cl;
        this.p = p;
        
        this.fast = fast;
        this.fromDeck = fromDeck;
        this.pos = pos;
        this.fromDis = fromDis;
        this.amount = amount;
    }

    public override void StartCommandExecution()
    {
        //sound
        SoundManager.instance.PlaySound(GlobalSetting.instance.drawClip);
        if (p.hand.CardInHand.Count <= 6)
        {
            if (fromDeck)
            {
                Debug.Log("Draw Card" + cl.card.ToString() + " Module====================>Active");
                --p.playerArea.pDeck.cardInDeck;
                p.playerArea.handVisual.GivePlayerACard(cl.card, cl.UniqueCardID, fast, fromDeck);
            }
            else
            {
                Debug.Log("Discover A Card To Player" + cl.card.name);
                //Discover Mode
                p.playerArea.handVisual.GivePlayerACard(cl.card, cl.UniqueCardID, fast, fromDeck);
                //Other Way got a card
//                p.GetACardNotFromDeck(cl.card);
            }

        }
        else
        {
            Debug.Log("hand fully discard random card");
            int rnd = Random.Range(0, p.hand.CardInHand.Count);
            p.DiscardCardAtIndex(0);
            
        }

       
    }
}
