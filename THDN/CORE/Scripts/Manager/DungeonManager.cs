using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Linq;
using Invector.vItemManager;
using GameDataEditor;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using PixelCrushers.DialogueSystem;
using DungeonArchitect.Navigation;
using DungeonArchitect;
using Invector.Utils;

[System.Serializable]
public class DungeonManager : MonoBehaviour {
    
    public Players player;
    public GameObject currentScene;
    public ScriptableDungeon currentDungeon;
    [Header("Global Game Setting")]
    public bool isNight=false;
    public bool isCamp=false;   //check counter for player
    public int dayTime; //select by difficult und default  3min 
    public int nightTime;
    public int currentDay;
    public int totalDay; 
    public int campTime;

    // dungeon will check the CT to change the dungeon state
    public int currentTime ;
  
    public int totalCampTime=-1;

    [Header("Counter UI")]
    public GameObject CounterObj;
    public GameObject RewardObj;
    public TextMeshProUGUI currentDayText;
    public TextMeshProUGUI totalDayText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI dungeonStateText ;
    public TextMeshProUGUI cMoneyText;
    public TextMeshProUGUI cDustText;
    public TextMeshProUGUI cExpText;


  
    [Header("UI Color")]
    public Color safetyColor=Color.green;
    public Color warningColor =Color.red;

    [Header("Collect Data")]
    public static int dungeonMoney=0;
    public static int dungeonDust=0;
    public static float dungeonExp=0f;


    private void Start() {
        player = Players.localPlayer;

        

        StartCoroutine("DungeonTimeRoutine");
    }

    private void Update() {
       if (player){
            //

            //Bind UI
            currentDayText.text = currentDay.ToString();
            if(isNight){
            TimeText.text =dayTime.ToString();
        
            }else{
                TimeText.text = nightTime.ToString(); 
            }
            //
           

        }
    }   
    
    //
    void InitDungeon(){

    }

    public IEnumerator DungeonTimeRoutine(){

        yield return StartCounterRoutine();
        yield return null;
        yield return NextDayRoutine();

        yield return null;  
    }

    //Start Counter for first dungeon
    public IEnumerator StartCounterRoutine(){

        //
        Debug.Log("First Load Dungeon Reset time ");
        ResetDungeonTime();


        //
        if(isNight ==false && isCamp==false){
        StartCoroutine(CountDownRoutine(dayTime));
        }else if(isNight=true && isCamp==false){
            StartCoroutine(CountDownRoutine(nightTime));
        }else if(isCamp==true){
            StartCoroutine(CountDownRoutine(campTime));
        }
        
        yield return null;
    }

    public void ResetDungeonTime(){
        currentDay =1;
        totalDay =3; 
        dayTime = currentDungeon.dayTime;
        nightTime=currentDungeon.nightTime;
        campTime = currentDungeon.campTime;
      

    }

    public IEnumerator UpdateDungeonDayRoutine()
{   
    var scene=FindObjectOfType<GlobalSetting>();
    if(scene){
        currentDungeon =scene.currentDungeon;
        //Reset time for next day at same dungeon
        
    }

    yield return null;
}   
//the day state -> DAY->NIGHT->CAMP -> (next)DAY
IEnumerator CountDownRoutine( int Times)
    {
        while (Times > 1)
        {
            yield return new WaitForSeconds(1f);
            Times--;
            //pause
            //when time goes zero turn the state for next part 
            if(Times==0){
                //Change state by current 
                if(isNight==true){
                    //try night-> Camp
                    isCamp=true;
                    if(player.health>0){
                        //success this time got reward at this day 
                        player.CmdGotDungeonReward(dungeonMoney,dungeonDust,dungeonExp);
                    }
                   
                }else if(isNight ==false && isCamp==false){
                    //day -> night
                    if(player.health> 0 ){
                    isNight=true;
                    NightTime();
                    }
                }else if(isCamp==true){
                    //Camp -> Next day 
                    if(player.health>0){
                    if(currentDay< totalDay){
                    StartCoroutine(NextDayRoutine());
                    }else if(currentDay == totalDay){
                       StartCoroutine(FinalDayRoutine());
                    }
                    }else{
                        //UI Part
                        currentScene.GetComponent<GlobalSetting>().DeadConsole();
                    }
                }
            }

            TimeText.text = Times.ToString();
        }

        yield return null;

    }
// at night time , enemies will spawn round the player at the limit time ,
//player needs defeat the shenshan core avoid hurt by rndGenerate enemies
//nightmare different of the day enemies will arrgo und got bouns with them (Extra)
    void NightTime(){
        isNight=true;
        //got spawner und set enemies ability
        if(currentDungeon.rndEnemies==false){
            //got list enemies
        }else{
            //generate by spawner container
            var enemiesList = currentScene.GetComponent<GlobalSetting>().enemyList.ToList();

            if(enemiesList.Count>0){
                for(int i=0;i<enemiesList.Count;i++){
                    //enemies will increase by day * dungeonBouns * difficult 
                    //same with the reward bouns relation with money & dust & exp & skillexp
                    
                }
            }
        }
    }

    //
    public IEnumerator NextDayRoutine(){
        
        Debug.Log("Camp Time Check Player State");
        while(isNight==true &&  totalCampTime ==0){
            yield return null ;
        }
        Debug.Log("Next Day Module");
        if(currentDay< totalDay){
            currentDay++;
        }
        //
        if(currentDay==2){
            currentScene.GetComponent<GlobalSetting>().ChangeScene(currentDungeon.DDreiScene);
        }else if(currentDay==3){
             currentScene.GetComponent<GlobalSetting>().ChangeScene(currentDungeon.DZreiScene);
        }
        UpdateDungeonDayRoutine();

        yield return StartCounterRoutine();

    }

    public IEnumerator FinalDayRoutine(){

        yield return StartCounterRoutine();
    }


  
}