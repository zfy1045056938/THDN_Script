using UnityEngine;
using System.Collections;
using DG.Tweening;
public class PlayACreatureCommand : Command
{
    private CardLogic cl;
    private int tablePos;
    private Players p;
    private int creatureID;

    public PlayACreatureCommand(CardLogic cl, Players p, int tablePos, int creatureID)
    {
        this.p = p;
        this.cl = cl;
        this.tablePos = tablePos;
        this.creatureID = creatureID;
    }

    public override void StartCommandExecution()
    {
        //Sound
        SoundManager.instance.PlaySound(GlobalSetting.instance.atkClip);
        // remove and destroy the card in hand 
        HandVisual PlayersHand = p.playerArea.handVisual;
        GameObject card = IDHolder.GetComponentWithID(cl.UniqueCardID);

        Sequence s = DOTween.Sequence();

//        s.Append(card.transform.DOMove(PlayersHand.playerPreviewSpot.position, 2.0f));
//        s.Insert(0f,card.transform.DORotate(Vector3.zero, 0.3f));
//        s.Append(card.transform.DOMove(PlayersHand.discardpoolPos.position, 2.0f));
//        s.Insert(0f,card.transform.DORotate(new Vector3(0f, -179f, 0f), 0.3f));
//        card.transform.parent = PlayersHand.discardpoolPos;
       
        PlayersHand.RemoveCard(card);
        GameObject.Destroy(card);
        //Add To discardPool
//        TurnManager.instance.WhoseTurn.discardPool.cardObj.Add(card);
        
        new DelayCommand(0.5f).AddToQueue();
        // enable Hover Previews Back
        HoverPreview.previewAllowed = true;
        // move this card to the spot 
        if (TurnManager.instance.WhoseTurn.table.creatureOnTable.Count <= 6)
        {
            p.playerArea.tableVisual.AddCreatureAtInIndex(cl.card, creatureID, tablePos);
        }
        else
        {
            Debug.Log("higher than 6 than can't play to table so may that one daamge to player");
            TurnManager.instance.WhoseTurn.otherPlayer.MaxHealth -=1;
            new DealDamageCommand(TurnManager.instance.WhoseTurn.otherPlayer.ID,1,TurnManager.instance.WhoseTurn.otherPlayer.MaxHealth,
            0,0,DamageElementalType.None).AddToQueue();
        }

    } 
}
