using UnityEngine;
using System.Collections;
using DG.Tweening;
public class StartATurnCommand : Command {

    private Players p;
    private CardLogic cl;
    public StartATurnCommand(Players p)
    {
        this.p = p;
    }

    public override void StartCommandExecution()
    {


        Debug.Log("It " + p.ToString() + "Turn");
        TurnManager.instance.WhoseTurn = p;
    
//        if (p == GlobalSetting.instance.topPlayer && TurnManager.instance.WhoseTurn == GlobalSetting.instance.lowPlayer)
//        {
//             GlobalSetting.instance.endTurnText.text = p.ToString();
//             GlobalSetting.instance.EndBtn.interactable = false;
//        }else if (p == GlobalSetting.instance.lowPlayer &&
//                  TurnManager.instance.WhoseTurn == GlobalSetting.instance.topPlayer)
//        {
//              GlobalSetting.instance.endTurnText.text = p.ToString();
//                         GlobalSetting.instance.EndBtn.interactable = false;
//        }
//GlobalSetting.instance.EnableEndTurnButtonOnStart(TurnManager.instance.WhoseTurn);
        
        CommandExecutionComplete();
      
       

    }
}
