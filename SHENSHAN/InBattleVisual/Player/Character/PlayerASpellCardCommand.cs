using UnityEngine;
using System.Collections.Generic;
using System.Collections;


/// <summary>
/// 
/// </summary>
public class PlayerASpellCardCommand:Command
{
    private Players players;
    private CardLogic playerCard;

    public PlayerASpellCardCommand(Players players, CardLogic playerCard)
    {
        this.players = players;
        this.playerCard = playerCard;
    }

    public override void StartCommandExecution()
    {
        Command.CommandExecutionComplete();
        //
        players.playerArea.handVisual.PlayASpellFromHand(playerCard.UniqueCardID);
    }
}