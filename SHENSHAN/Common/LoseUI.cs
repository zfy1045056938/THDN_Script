using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;
using UnityEngine.UI;
public class LoseUI : MonoBehaviour
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
    
        if (BattleStartInfo.AtDungeon=true)
        {
            // SceneReloader.instance.ReturnToDungeon();
              BattleStartInfo.IsWinner = false;
        DungeonExplore.instance.canLeave = true;
             SceneReloader.instance.ReturnTown("MainBattleScene");
        }
        else
        {
                  SceneReloader.instance.ReturnTown("MainBattleScene");
        BattleStartInfo.IsWinner = false;
        DungeonExplore.instance.canLeave = true;
        DungeonExplore.instance.LeaveDungeon();
            
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
