using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using System.Security.Cryptography;
using DG.Tweening;
using Language.Lua;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using Michsky.LSS;


//choose character
public  class SaveLoadCharacter : MonoBehaviour
{

  public List<SaveSlots> saveSlot;
  public UIPanel menuPanel;
  public int selection;
  public NetworkManagerShenShan manager;
  public GameObject BG;

  //
  public Button startBtn;
  public Button quitBtn;
  public Button createBtn;
public Button deleteBtn;
  public UIConfirmation uiConfirmation;  //Selected State
  public CreateCharacterUI createCharacter;
  public GameObject panel;
 
  public static SaveLoadCharacter instance;
  public GameObject saveSlotObj;
  public MenuManager menu;
  public Transform pos;
  public float time = 3.0f;
  private bool cl = false;
  public ShenShanLoading shenshanloading;

  public LoadingScreenManager ls;
  void Awake()
  {
      if (instance==null)
      {
          instance = this;
      }


  }

 


  void Update()
  {
  
    selection = manager.selection;
     if (manager.state == NetworkState.Lobby )
     { selection = manager.selection;
        
         //
         if (manager.charactersAvailableMsg != null)
         {
             CharactersAvailableMsg.CharacterPreview[] cs = manager.charactersAvailableMsg.characters;
             
             startBtn.interactable=(cs.Length>0);
//             startBtn.onClick.AddListener(() =>
//             {
//                 ClientScene.Ready(NetworkClient.connection);
//                 //
//                 NetworkClient.connection.Send(new CharacterSelectMsg {index = manager.selection});
//                 //
//                 manager.ClearPreviews();
//                 //
//                 panel.SetActive(false);
//
//                 TownManager.instance.ShowTown(true);
//             });
//             
//             

             createBtn.onClick.AddListener(() =>
             {
                 panel.SetActive(false);
                 createCharacter.Show();
             });

            deleteBtn.gameObject.SetActive(manager.selection!=-1);
            
            createBtn.gameObject.SetActive(cs.Length==0);

             
         }
         else
         {
             
         }
     }
  }

  public void DeletePlayer(){
 uiConfirmation.Show(" 是否删除该存档",()=>{
                    NetworkClient.Send(new CharacterDeleteMsg{index=manager.selection});
                });
  }


  public void StartGames()
  {

      if (manager.state == NetworkState.Lobby)
      {

          if (manager.charactersAvailableMsg != null)
          {

                      
ls.LoadSceneWait();

  try
           {
                 CharactersAvailableMsg.CharacterPreview[] cs = manager.charactersAvailableMsg.characters;

              ClientScene.Ready(NetworkClient.connection);
              //
              NetworkClient.connection.Send(new CharacterSelectMsg {index =0});
 
              manager.ClearPreviews();
           }
           catch (System.Exception)
           {
               
               throw;
           }


   Debug.Log("Load Town");
                  TownManager.instance.ShowTown(true);
                  PlayerData.LOCTYPE = LocType.Town;

                  LoadingScreen.instance.onFinishEvents.AddListener(()=>{
                      Debug.Log("End Button Event");
                       DialogueManager.StartConversation("回滚");
                  });
                  
                    menuPanel.gameObject.SetActive(false);
            //   StartCoroutine(StartGameRoutine());
            //    shenshanloading.Loading();
            //    loadingScreenManager.gameObject.SetActive(true);
           
            //   CharactersAvailableMsg.CharacterPreview[] cs = manager.charactersAvailableMsg.characters;

            //   ClientScene.Ready(NetworkClient.connection);
            //   //
            //   NetworkClient.connection.Send(new CharacterSelectMsg {index = manager.selection});

            //   manager.ClearPreviews();
            //   //
            // //   shenshanloading.LoadingForWaitRoutine();

            // //   FindObjectOfType<PlayerData>().GetComponent<SaveSlots>().panel.Close();

            //     //
            //     Debug.Log("Load Town");
            //       TownManager.instance.ShowTown(true);
            //       PlayerData.LOCTYPE = LocType.Town;
            //       DialogueManager.StartConversation("回滚");
                  
            //         menuPanel.gameObject.SetActive(false);

            // Debug.Log("Load Game");
            // shenshanloading.LoadingForWaitRoutine();
            //   TownManager.instance.ShowTown(true);
            //       PlayerData.LOCTYPE = LocType.Town;
            //       DialogueManager.StartConversation("回滚");
            
//              if (PlayerPrefs.HasKey("InMap"))
//              {
//                  bool check = PlayerPrefsX.GetBool("InMap");
//                  if (check)
//                  {
////                      BG.SetActive(true);
//                      shenshanloading.OpenPanel();
//                      shenshanloading.LeaveTown();
////                      StartCoroutine(LoadMapRoutine());
//
//                  }
//
//              }
//              else
//              {

            
//              }
//              }

            //   panel.SetActive(false);

            //       menu.panel.SetActive(false);
              

          }
      }
  }


public IEnumerator StartGameRoutine(){

     //    shenshanloading.Loading();


    // yield return ShowLoadingRoutine();
    yield return 0.4f;
     yield return LoadCharacterRoutine();
            
              //
            //   shenshanloading.LoadingForWaitRoutine();

            //   FindObjectOfType<PlayerData>().GetComponent<SaveSlots>().panel.Close();

                //
                Debug.Log("Load Town");
                  TownManager.instance.ShowTown(true);
                  PlayerData.LOCTYPE = LocType.Town;
                  DialogueManager.StartConversation("回滚");
                  
                    menuPanel.gameObject.SetActive(false);

    yield return new WaitForSeconds(1.0f);
}

IEnumerator ShowLoadingRoutine(){
    ls.LoadScene("MainBattleScene");
    yield return null;
}

IEnumerator LoadCharacterRoutine(){
    
           try
           {
                 CharactersAvailableMsg.CharacterPreview[] cs = manager.charactersAvailableMsg.characters;

              ClientScene.Ready(NetworkClient.connection);
              //
              NetworkClient.connection.Send(new CharacterSelectMsg {index = manager.selection});
 
              manager.ClearPreviews();
           }
           catch (System.Exception)
           {
               
               throw;
           }

           yield return null;
}
  IEnumerator LoadMapRoutine()
  {

//          BG.SetActive(true);
     
             
          
              while (time > 0f)
              {
                  
                  --time;
                  Debug.Log(time);

                  if (time == 1)
                  {
                      MenuManager.instance.worldMapPanel.Open();
                      PlayerData.LOCTYPE = LocType.Map;
                      TownManager.instance.gameCamera.Priority = 0;
                      TownManager.instance.worldMapCamera.Priority = 1;

                  }
                  
                  if(time==0.1){
                      cl = true;
                      BG.SetActive(false);
                  }
                  
                  
                 
                  Debug.Log("t");
              }

              yield return new WaitForSeconds(time*Time.deltaTime);


  }

public void SetValueToslot(PlayerData p,int index){
    GameObject obj = Instantiate(saveSlotObj,pos.transform.position,Quaternion.identity)as GameObject;
    
    if(p!=null&&obj!=null){
        //Set info to slot
        for(int i=0;i<saveSlot.Count;i++){
            if(saveSlot[i].name==""){
                //create instance and save
                saveSlot[i].playerName.text=p.name;
                saveSlot[i].dateTime.text=DateTime.UtcNow.ToString();
                saveSlot[i].gameObject.AddComponent<SelectableCharacter>().GetComponent<SelectableCharacter>().index=index;
            }
        }
    }
}
  public void Show()
  {
      Open();
  
  //Check character available
//      if (manager.charactersAvailableMsg != null && manager.state==NetworkState.Lobby)
//          {
//              CharactersAvailableMsg.CharacterPreview[] cs = manager.charactersAvailableMsg.characters;
//              //
//        
//                  for (int i = 0; i < saveSlot.Count; i++)
//                  {
//                      foreach(CharactersAvailableMsg.CharacterPreview n in cs){
//                          if(n.name!=null){
//                              SaveSlotInfo info = new SaveSlotInfo{
//                                  name= n.name,
//                                  dateTime=DateTime.Now,
//                              };
//                              saveSlot.Add(info);
//                          }
//                      }

                    //   byte[] extra = BitConverter.GetBytes(saveSlot[i]);
                    //   //
                    //   ClientScene.AddPlayer(NetworkClient.connection, extra);
                    //   //
                    //   PlayerData go = Instantiate(players, transform.position, Quaternion.identity);
                    //   go.name = PlayerPrefs.GetString("PlayerName");

                    //   //
                    //   if (go != null)
                    //   {
                    //       TownManager.instance.ShowTown(true);
                    //   }

                    //   else
                    //   {
                    //       Debug.Log("No Character Exist");
                    //       createCharacter.gameObject.SetActive(true);
                    //   }
//                  }
//        
//          }else{
//              Debug.Log("Check Netwoek");
//          }

  }

  public void Open(){
      panel.SetActive(true);
  }

  public void Close(){
      panel.SetActive(false);
  }

}