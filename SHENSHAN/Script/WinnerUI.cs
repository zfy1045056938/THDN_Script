using System.Collections;
using System.Collections.Generic;
using PixelCrushers;
using UnityEngine;
using UnityEngine.UI;

public class WinnerUI : MonoBehaviour
{
    public UIPanel panel;
    public Text timeText;
    public int time=3;

    public bool canL = false;
    
    // Start is called before the first frame update
    void Start()
    {
       

       StartCoroutine(PreLeave());
        
    }
    
    

    IEnumerator PreLeave()
    {
        StartCoroutine(TimeRoutine());
        while (!canL)
        {
            yield return null;
        }
    
        if (BattleStartInfo.AtDungeon)
        {
            // SceneReloader.instance.ReturnToDungeon();
             SceneReloader.instance.ReturnTown("MainBattleScene");
        }
        else
        {
            SceneReloader.instance.ReturnTown("MainBattleScene");
            
        }

    }

    IEnumerator TimeRoutine()
    {
        while (time>0)
        {
            yield return new WaitForSeconds(1.0f);
            time--;
            if (time == 0)
            {
                canL = true;
            }
            
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        timeText.text = time.ToString();
    }
}
