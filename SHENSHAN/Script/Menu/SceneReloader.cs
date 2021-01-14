using System;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using PlayfulSystems.LoadingScreen;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Mirror;
using Mono.Data.Sqlite;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.UnityGUI;
using Steamworks;
using Michsky.LSS;

///<summary>
///
///载入场景，根据场景名字进行跳转
///1.IDFactoryGetID
///2.HolderGetList()
///3.LoadCommand
///4.SceneManagerLoadSceneByName
///</summary>
public class SceneReloader : LoadingScreenProBase
{
	public static SceneReloader instance;
	public string reloadName;
	private PlayerData p;
	private EnemyPortraitVisual enemy;
	public ConsoleManager consoleManager;
	
	private NetworkManagerShenShan manager;
	public LevelManager level;
	public ShenShanLoading shenshanLoading;
	
public LoadingScreenManager loadingScreenManager;
	void Awake()
	{
instance=this;
		p = FindObjectOfType<PlayerData>();
		manager= FindObjectOfType<NetworkManagerShenShan>();
		
	}

    private void Start()
    {
       
        p = GetComponent<PlayerData>();
        enemy = GetComponent<EnemyPortraitVisual>();
		consoleManager=GetComponent<ConsoleManager>();
		level = GetComponent<LevelManager>();
	
    }
    
    
    /// <summary>
    /// 
    /// </summary>
	public void ReloadScene(){
IDFactory.ResetID();
IDHolder.ClearIDHolderList();
Command.CommandQueue.Clear();
Command.CommandExecutionComplete();
if (SceneManager.GetActiveScene().IsValid())
{
	SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Additive);
	SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
}
else
{
	SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
}
	}
	


	public void SaveGame(){
		 var saveSystem = FindObjectOfType<SaveSystem>();
            if (saveSystem != null)
            {
                SaveSystem.SaveToSlot(1);
            }
            else
            {
                string saveData = PersistentDataManager.GetSaveData();
                PlayerPrefs.SetString("SavedGame", saveData);
                Debug.Log("Save Game Data: " + saveData);
            }
            DialogueManager.ShowAlert("Game saved." );
	}

   

public	void OnApplicationQuit(){
	
	if(SteamAPI.IsSteamRunning()){
		
		SteamAPI.Shutdown();
	}
	PersistentDataManager.Record();
		
		NetworkServer.Shutdown();
		ClientScene.ClearSpawners();
	
		//remove db 
		DialogueLua.SetVariable("DungeonBoss",0);
		DialogueLua.SetVariable("DungeonMonster",0);
	
	Application.Quit();

	}
 


/// <summary>
/// 
/// </summary>
public void ReturnTown(string sname)
{
	NetworkManager.networkSceneName = sname;
	
//	NetworkManager.singleton.ServerChangeScene(NetworkManager.networkSceneName);
//	SceneManager.UnloadSceneAsync("MainBattleScene");
Debug.Log("Clear Battle Data");
	IDFactory.ResetID();
	IDHolder.ClearIDHolderList();
	Command.CommandQueue.Clear();
	Command.CommandExecutionComplete();
	//
	if (PlayerData.LOCTYPE == LocType.Town)
	{ 
		ConsoleManager.sceneType = SceneType.Town;
		ConsoleManager.frombt = true;
//	NetworkIdentity iden = GameObject.FindWithTag("Player").GetComponent<NetworkIdentity>();
//	NetworkServer.SendToClientOfPlayer(iden,new SceneMessage{sceneName = NetworkManager.networkSceneName,sceneOperation = SceneOperation.UnloadAdditive});
//	SceneManager.UnloadSceneAsync(NetworkManager.networkSceneName);
		StartCoroutine(ReturnRoutine(NetworkManager.networkSceneName));
		ConsoleManager.instance.ShowTown();
	}else if (PlayerData.LOCTYPE == LocType.Map)
	{
		ConsoleManager.sceneType = SceneType.Map;
		
		StartCoroutine(ReturnRoutine(NetworkManager.networkSceneName));
		BattleStartInfo.IsWinner = false;
	}else if(PlayerData.LOCTYPE==LocType.Dungeon){
		ConsoleManager.sceneType = SceneType.Dungeon;
		ConsoleManager.frombt = true;
////	NetworkIdentity iden = GameObject.FindWithTag("Player").GetComponent<NetworkIdentity>();
////	NetworkServer.SendToClientOfPlayer(iden,new SceneMessage{sceneName = NetworkManager.networkSceneName,sceneOperation = SceneOperation.UnloadAdditive});
////	SceneManager.UnloadSceneAsync(NetworkManager.networkSceneName);
		StartCoroutine(ReturnRoutine(NetworkManager.networkSceneName));
		ConsoleManager.instance.ShowTown();

    
	}
}

public void ReturnFromDungeon(string sname)
{
	NetworkManager.networkSceneName = sname;
	StartCoroutine(ReturnRoutine(NetworkManager.networkSceneName));
	BattleStartInfo.sceneName = "";
	ConsoleManager.instance.ShowTown();
}

public void Skipbattle(){
	GlobalSetting.instance.topPlayer.MaxHealth-=100;
}
IEnumerator ReturnRoutine(string sname)
{
	NetworkManager.networkSceneName = sname;
 yield return SceneManager.UnloadSceneAsync(NetworkManager.networkSceneName);
 
//	 NetworkManager.singleton.ClientChangeScene(NetworkManager.networkSceneName,
//		SceneOperation.UnloadAdditive);
}

public void MoveDungeon(string sname)
{
	TownManager.instance.gameCamera.enabled = false;
	
	TownManager.instance.battleConfigPanel.SetActive(false);
	TownManager.instance.content.Open();
	//
	NetworkManager.networkSceneName = sname;
	SceneManager.LoadSceneAsync(NetworkManager.networkSceneName, LoadSceneMode.Additive);
}
public void ChangeScene(string sname)
{
	
//	TownManager.instance.gameCamera.enabled = false;
//	
TownManager.instance.enviormentPrefab.gameObject.SetActive(false);
	TownManager.instance.battleConfigPanel.SetActive(false);
	TownManager.instance.cameraGroup.gameObject.SetActive(false);
	TownManager.instance.content.gameObject.SetActive(false);
	//hide obj
	foreach(var e in TownManager.instance.npcModels){
		if(e!=null){
			e.gameObject.SetActive(false);
		}
	}
//	TownManager.instance.content.Close();
	
	// shenshanLoading.OpenPanel();
	
	// if (shenshanLoading.isActiveAndEnabled)
	// {
	// 	shenshanLoading.GetComponent<ShenShanLoading>().StartLoading(sname);
	// }

	loadingScreenManager.LoadScene(sname);
	
//	break;
//	case SceneType.Dungeon:
//		//Dungeon Config Send To ConsoleManager
//		NetworkManager.networkSceneName = sname;
//		SceneManager.LoadScene(NetworkManager.networkSceneName, LoadSceneMode.Additive);
//		break;
}

public void DungeonChangeScene(string sname)
{
	shenshanLoading.OpenPanel();
	
		shenshanLoading.GetComponent<ShenShanLoading>().StartLoading(sname);
}

public void ReturnToDungeon()
{
	DungeonExplore.instance.canLeave = true;
	BattleStartInfo.IsWinner = false;
	
		StartCoroutine(ReturnRoutine("MainBattleScene"));
		// DungeonUIManager.instance.ReturnBattlefield();
		TownManager.instance.content.gameObject.SetActive(true);
	
}


}
   
