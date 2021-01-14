using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;


public class PlayCreatureCommand:Command
{
    private CardLogic playerCard;
    private Players players;
    private int tablePos;
    private int uniqueCreatureId;

    public PlayCreatureCommand(CardLogic playerCard, Players players, int tablePos, int uniqueCreatureId)
    {
        this.playerCard = playerCard;
        this.players = players;
        this.tablePos = tablePos;
        this.uniqueCreatureId = uniqueCreatureId;
    }

    public override void StartCommandExecution()
    {
        HandVisual playerHand = players.playerArea.handVisual;
        GameObject card = IDHolder.GetComponentWithID(playerCard.UniqueCardID);
        playerHand.RemoveCard(card);
//       GameObject.Destroy(card);
        //
//        card.transform.DOLocalMove(players.discardPool.transform.position, 3.0f);
        GameObject.Destroy(card);
        //
        HoverPreview.previewAllowed = true;
        //Move Index
        players.playerArea.tableVisual.AddCreatureAtInIndex(playerCard.card, uniqueCreatureId, tablePos);
    }
}