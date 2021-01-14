using UnityEngine;
using System.Collections;

public class GameOverCommand : Command{

    private Players looser;
        public PlayerData player;

    public GameOverCommand(Players looser,PlayerData p)
    {
        this.looser = looser;
        this.player=p;
    }

    public override void StartCommandExecution()
    { 
        
        looser.playerArea.playerPortraitVisual.Explose();

        Command.CommandExecutionComplete();
    
        
    }
}
