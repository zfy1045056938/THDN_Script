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
using Invector.vCharacterController;
using Invector;

[System.Serializable]
public class DungeonManager : MonoBehaviour {
    
    public vThirdPersonController player;
    public vGameController gameController;
    public GameObject currentScene;
    public GameObject testObj;
    public ScriptableDungeon currentDungeon;
    [Header("Global Game Setting")]
    public bool isNight=false;
    public bool isCamp=false;   //check counter for player

    public bool StartCount=false;
    //validate the state
    public bool canChangeState=false;
    public bool canChangeDay=false;
    public bool isFinal=false;
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
        // player = Players.localPlayer;

        StartCount=false;
        //
        testObj.gameObject.SetActive(true);
        Time.timeScale=0;

        
    }

    private void Update() {
    //    if (player){
    //         //

    //         //Bind UI
    //         currentDayText.text = currentDay.ToString();
    //         if(isNight){
    //         TimeText.text =dayTime.ToString();
        
    //         }else{
    //             TimeText.text = nightTime.ToString(); 
    //         }
    //         //
           

    //     }

    currentDayText.text= currentDay.ToString();
    }   
    
  
    public IEnumerator DungeonTimeRoutine(){

        while(!StartCount){
            yield return null;
        }

        Debug.Log("Start Game");
        yield return StartCounterRoutine();
        // yield return null;
        // yield return NextDayRoutine();
        
        // yield return null;  
    }

    //Start Counter for first dungeon
    public IEnumerator StartCounterRoutine(){

        //
        Debug.Log("First Load Dungeon Reset time ");
        ResetDungeonTime();

        Debug.Log("Counter");

        //Start Counter Time
        // if(isNight ==false && isCamp==false){
        StartCoroutine(CountDownRoutine(currentTime));
        // }else if(isNight=true && isCamp==false){
        //     StartCoroutine(CountDownRoutine(currentTime));
        // }else if(isCamp==true){
        //     StartCoroutine(CountDownRoutine(currentTime));
        // }
        
        yield return null;
    }

    public void ResetDungeonTime(){
        isCamp=false;
        isNight=false;
        currentTime=0;
        currentDay =1;
        totalDay =3; 
        canChangeDay=false;
        canChangeState=false;
        //
        // dayTime = currentDungeon.dayTime;
        // nightTime=currentDungeon.nightTime;
        // campTime = currentDungeon.campTime;
        dayTime =10;
        nightTime = 10;
        campTime=10;
        //Reset currentTime
        currentTime = dayTime;
        //
      

    }

//Test
    public IEnumerator UpdateDungeonDayRoutine()
{   
    Debug.Log("Update Day");
    // var scene=FindObjectOfType<GlobalSetting>();
    // if(scene){
    //     currentDungeon =scene.currentDungeon;
    //     //Reset time for next day at same dungeon
        
    // }
    if(currentDay < totalDay){
        Debug.Log("Next day");
        StartCoroutine(CountDownRoutine(dayTime));
    }else{

    }

    yield return null;
}   
//the day state -> DAY->NIGHT->CAMP -> (next)DAY
IEnumerator CountDownRoutine( int Times)
    {
        canChangeState=false;
        while (Times > 1)
        {
            yield return new WaitForSeconds(1f);
            Times--;
            Debug.Log("ctimes ist"+Times);
            //pause
            //when time goes zero turn the state for next part 
            if(Times<=1){
                Debug.Log("Current Time is 0 turn dungeon state");
                canChangeState=true;
                //Change state by current 
                if(isFinal==false){
                if(isNight==true && isCamp==false ){
                    Debug.Log("Night->Camp current day ist \t"+currentDay);
                    //try night-> Camp
                    isCamp=true;
                    if(player.currentHealth>0){
                        canChangeState=false;
                        dungeonStateText.text ="CAMP";
                       StartCoroutine(CountDownRoutine(nightTime));
                    }
                   
                }else if(isNight ==false && isCamp==false){
                    Debug.Log("day-> night");
                    //day -> night
                    if(player.currentHealth> 0 ){
                    isNight=true;
                    // NightTime();
                    canChangeState=false;
                dungeonStateText.text ="NIGHT";
                //    StartCoroutine(UpdateStateRoutine(dayTime));
                    StartCoroutine(CountDownRoutine(nightTime));
                    }
                }else if(isCamp==true && isNight==true){
                    //Camp -> Next day 
                    Debug.Log("Camp time over try next day");
                    if(player.currentHealth>0 ){
                    if(currentDay< totalDay){
                        canChangeState=false;
                    StartCoroutine(NextDayRoutine());
                    }else if(currentDay == totalDay ){
                        canChangeState=false;
                        Debug.Log("Final day to the final scene");
                       StartCoroutine(FinalDayRoutine());
                    }
                    }else{
                        //UI Part
                        currentScene.GetComponent<GlobalSetting>().DeadConsole();
                    }
                }
                }else{
                    Debug.Log("Final Time last counter");
                    if(player.currentHealth>0 && FindObjectOfType<Players>().target.health <=0){
                            //Winner und drop chest
                            currentScene.GetComponent<GlobalSetting>().FinalConsole(player.currentHealth>0);
                    }else{
                        //lose returns console
                        currentScene.GetComponent<GlobalSetting>().FinalConsole(player.currentHealth>0);
                    }
                }
            }else{
                Debug.Log("Go on");
            }

            TimeText.text = Times.ToString();
        }
    yield return null;
      

    }

    IEnumerator UpdateStateRoutine(int time){
        

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
        
        
        Debug.Log("Next Day Module");
        if(currentDay< totalDay){

            currentDay++;
            currentDay =Mathf.Clamp(currentDay,1,totalDay);
            currentDayText.text =currentDay.ToString();
        }
        currentDayText.text ="Day";
        //reset
        canChangeDay=false;
        canChangeState=false;
        isNight=false;
        isCamp=false;
        currentTime =dayTime;
        //
        // if(currentDay==2){
        //     currentScene.GetComponent<GlobalSetting>().ChangeScene(currentDungeon.DDreiScene);
        // }else if(currentDay==3){
        //      currentScene.GetComponent<GlobalSetting>().ChangeScene(currentDungeon.DZreiScene);
        // }
        // UpdateDungeonDayRoutine();

        yield return CountDownRoutine(currentTime);

    }

    public IEnumerator FinalDayRoutine(){
        //TODO Change the final scene
        isNight=false;
        isCamp=false;
        isFinal=true;
        //
        currentScene.GetComponent<GlobalSetting>().ChangeScene(currentDungeon.DDreiScene);
        //
        currentTime=dayTime;
        //
        //
        yield return CountDownRoutine(currentTime);
    }


#region Test-canDelete Module

    public void Dungeon_StartGame(){
        testObj.gameObject.SetActive(false);
        StartCount=true;
        Time.timeScale =1;
        gameController.GeneratePlayer();
        //
        player= FindObjectOfType<vThirdPersonController>().GetComponent<vThirdPersonController>();
        //
        StartCoroutine("DungeonTimeRoutine");
    }

#endregion
}