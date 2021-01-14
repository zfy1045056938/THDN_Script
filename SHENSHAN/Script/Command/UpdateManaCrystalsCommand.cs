using UnityEngine;
using System.Collections;
using DG.Tweening;
public class UpdateManaCrystalsCommand : Command {

    private Players p;
    private int TotalMana;
    private int AvailableMana;

    public UpdateManaCrystalsCommand(Players p, int TotalMana, int AvailableMana)
    {
        this.p = p;
        this.TotalMana = TotalMana;
        this.AvailableMana = AvailableMana;
    }

    public override void StartCommandExecution()
    {
        p.playerArea.manaThisTurn.TotalCrystals = TotalMana;
        p.playerArea.manaThisTurn.AvailableCrystals = AvailableMana;

        Sequence s = DOTween.Sequence();
        s.PrependInterval(0.4f);
       
            Command.CommandExecutionComplete();
       
       
    }
}
