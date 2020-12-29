using System.Collections;
using System.Collections.Generic;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

/// <summary>
/// manager game time when time =0 go next turn until one health <=0
/// </summary>
public class MatchTurnManager : MonoBehaviour
{
    public static MatchTurnManager instance;
    public UIPanel panel;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI RoundText;
    public float Times=60;
    
    public int maxTime = 5;
    public Time time;
    public bool pause;
    public int gameTime;
    private IEnumerator flashRoutine;
    public void InitTime()
    {
      Times =60;

        // Round=0;
        //
        timeText.text = Times.ToString();
        
    }
    public void StartCountDown(){
       StartCoroutine(CountDownRoutine());
    
    }

    IEnumerator CountDownRoutine()
    {


        while (Times > 1)
        {

          
            yield return new WaitForSeconds(1f);
            Times--;
            //pause


            timeText.text = Times.ToString();
        }

        GameManager.instance.IsDealDamage = true;


    }

    void UpdateTime(){
        if(pause==true){return;}

        //
        timeText.text= Times.ToString();
       
    }
    // }
    // public void NextRound(){
    //           if (Times == 0)
    //     {
    //         Round++;
    //     UpdateTime(); 
            
    //     }
    // }

    public void ResetTime(){
        Times=10;
        
    }
}
