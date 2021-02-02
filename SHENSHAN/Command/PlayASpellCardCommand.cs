using UnityEngine;
using System.Collections;

public class 
    PlayASpellCardCommand: Command
{
    private CardLogic card;
    private Players p;
    //private ICharacter target;

    public PlayASpellCardCommand(Players p, CardLogic card)
    {
        this.card = card;
        this.p = p;
    }

    public override void StartCommandExecution()
    {
        Debug.Log(card.card.name+"\t\tActive EFFECT");
        // move this card to the spot
        p.playerArea.handVisual.PlayASpellFromHand(card.UniqueCardID);
        //
       
        // do all the visual stuff (for each spell separately????)
        p.playerArea.discardPool.cardpool.Add(card.card);
    }
}
