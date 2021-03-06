using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using DG.Tweening;
using PixelCrushers;
using CharacterInfo = UnityEngine.CharacterInfo;
using UnityEngine.SceneManagement;
using GameDataEditor;
using Cinemachine;
using TMPro;
using Michsky.LSS;
using Michsky.UI.Zone;
using Steamworks;
using TravelSystem;

///
public class TownManager : MonoBehaviour
{
    
    public UIPanel content;
   
    public MapLocation mapLocation;
    public MerchantController merchant;
    public NPCManager npcManager;
    [Header("Location Info")] 
    public TextMeshProUGUI locationText;
    
    public Image locationBG;
    public TextMeshProUGUI loactionDetail;
    public static TownManager instance;
    public List<EnemyAsset> locationEnemy;

    [Header("NPCInfo")] public GameObject NPCObj;
    public Transform npcPos;

    [Header("MapLocationCollections")]
    public Dictionary<string, MapLocation> mapDic = new Dictionary<string, MapLocation>();
    public List<MapLocation> maps;
    public NetworkManagerShenShan manager;
    public CharacterInfoPanel characterInfo;

    public DeckBuilderScreen DeckBuildScreen;
    public EnemyDeckSelection enemySelection;
    public StandardUIQuestLogWindow QuestPanel;
    public MainPanelManager topmenuPanel;
    public ConsoleManager consolePanel;
    public TopCharacterInfo topCharaInfo;
    public GameObject topInfo;
    public GameObject bottomUI;
    // public GameObject leaveBtn;

    public GameObject RewardPanel;
    public GameObject battleConfigPanel;
    public GameObject SettingPanel;
    public List<GameObject> npcTownObj;
    public List<GameObject> npcModels;
    public Transform npcModepos;
    // public GameObject leaveObj;
    public TooltipManager2 tooltip;
    [Header("NewBee")]
    public GameObject newBeePanel;
    public UnityEngine.UI.Toggle newBeeToggle;
    public bool hasNB = false;

    [Header("Dungeon Config")]
    public DungeonExplore explore;
    public bool atDungeon;
    public GameObject BG;
    public bool canStart=false;
    public bool PreSelectPack = false;
    public Text leaveTownText;
    public Button DeckViewBtn;

    [Header("DungeonStartNoticePanel")]
    public GameObject noticeUI;
    public GameObject MapObj;
    public TextMeshProUGUI currentLoc;
    public TextMeshProUGUI goalText;
    public TextMeshProUGUI dungeonLocText;
    public Button startBtn;
    public Transform targetPos;
    

   public bool isFree;
    private bool isShow;
    [HideInInspector]
    public bool isNew=true;

    
    private List<PlayerData> playerList;
    public bool fromBattle = true;
    [Header("Camera Config")]
    public CinemachineVirtualCamera gameCamera;
    private CinemachineVirtualCamera mainCamera;
    public GameObject  cameraGroup;
    public GameObject selectGroup;
    public GameObject mapCameraGroup;
    public CinemachineVirtualCamera worldMapCamera;


    public GameObject worldMap;

    public List<GameObject> areaList;

    [Header("BottomUI")] public Button CollectionBtn;
    public Button CurrCardBtn;

    [Header("GameConfig")]
     public Button dungeon_LeaveBtn;
    public Button common_QuitBtn;

    [Header("APPLICATION")]
    public int frame=60;
    [Header("World Enviorment")]
    public GameObject enviormentPrefab;
    private GameObject ModelLoad;
    private AssetBundle ab;

    public LoadingScreenManager loadingScreenManager;
    public MainPanelManager topPanelManager;

    public GameObject FtObj;
    public Transform Ftpos;

    [Header(" Popup")]
    public GameObject popupObj;
    public TextMeshProUGUI nameText;


    public static string currentLanguage = "ch";
    private void Awake()
    {
        instance = this;
        
   
        // characterInfo = GameObject.FindObjectOfType<CharacterInfoPanel>();
        // QuestPanel = GetComponent<StandardUIQuestLogWindow>();
        // RewardPanel = GetComponent<GameObject>()
        // SettingPanel = GetComponent<GameObject>();
        // merchant = GameObject.FindObjectOfType<MerchantController>();
        // DeckBuildScreen= GameObject.FindObjectOfType<DeckBuilderScreen>();
        // manager = GameObject.FindObjectOfType<NetworkManagerShenShan>();
        gameCamera.Priority=500;

//APPLICATIOn
     
    
    }
    

    void Start(){
        
        AssetBundle.UnloadAllAssetBundles(true);
        Resources.UnloadUnusedAssets();

      PlayerPrefsX.GetBool("IsFirst");

      if (ConsoleManager.frombt == true)
      {
          consolePanel.panel.Open();
      }
      
     DialogueLua.SetActorField("Player",name,PlayerPrefs.GetString("PlayerName"));
    GDEDataManager.Init("gde_data");

    int getBoss = DialogueLua.GetVariable("DungeonBoss").asInt;
    Debug.Log("NOW HAS "+getBoss+"IN THE DB");
    DialogueLua.SetVariable("DungeonBoss",0);
    
//    LoadMap();

// ModelLoad =(GameObject)Resources.Load("CModel",typeof(GameObject));
   
        //Load Model From AB
         //Load Asset Bundle
     StartCoroutine(InitABMRoutine());
   

    }
    void Update()
    {
        if (ConsoleManager.frombt == true && atDungeon==false && ConsoleManager.GetReward==true)
        {
            Debug.Log("Town Reward");
            consolePanel.ReturnCal(PlayerData.localPlayer);
            ConsoleManager.GetReward=false;
        }
        startBtn.onClick.AddListener(()=>{
           canStart=true;
        });
        
        // leaveBtn.gameObject.SetActive(DeckStorge.instance.AllDecks.Count>0);
        
       //
       if(atDungeon==true){
          CollectionBtn.gameObject.SetActive(false);
          CurrCardBtn.gameObject.SetActive(true);
        }
       else
       {
           CollectionBtn.gameObject.SetActive(true);
           CurrCardBtn.gameObject.SetActive(false);
       }

       
    }
    
    
    public void LoadCardSprite()
    {
        List<GDECardAssetData> cs = GDEDataManager.GetAllItems<GDECardAssetData>();

        for (int i = 0; i < CardCollection.instance.allCardsArray.Count; i++)
        {
            GDECardAssetData c= new GDECardAssetData(cs[i].Key);
            if (c.CardName == CardCollection.instance.allCardsArray[i].name)
            {
                CardCollection.instance.allCardsArray[i].cardSprite =Utils.CreateSprite(c.CardSprite);
            }
        }
        
        List<GDEItemsData> id = GDEDataManager.GetAllItems<GDEItemsData>();

        for (int i = 0; i < ItemDatabase.instance.items.Count; i++)
        {
            GDEItemsData ca= new GDEItemsData(id[i].Key);
            //
            if (ca.ItemName == ItemDatabase.instance.items[i].itemName)
            {
                if (ca.IconName != null)
                {
                    ItemDatabase.instance.items[i].icon = Utils.CreateSprite(ca.IconName);
                }
            }
        }
        
        //
        if (atDungeon == true)
        {
            dungeon_LeaveBtn.gameObject.SetActive(true);
            common_QuitBtn.gameObject.SetActive(true);
        }
        else 
        {
            dungeon_LeaveBtn.gameObject.SetActive(false);
           
        }
        
    }

    public IEnumerator InitABMRoutine(){
         string modelName = "gamebundle";
        
    // string uri= "file:///"+Application.dataPath+"/AssetBundles/"+modelName;
    // string uri=Application.dataPath+"/AssetBundles/StandaloneWindows/"+modelName;
    string uri=Application.dataPath+"/StreamingAssets/"+modelName;
    
    Debug.Log(uri);
    // UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.Get(uri);
    // //
    // yield return request.Send();
    // AssetBundle bundle  = request.
    // //Set Pos To World
    // GameObject g = bundle.LoadAsset<GameObject>(n);
    //  AssetBundleCreateRequest request= AssetBundle.LoadFromFileAsync(uri);
    // yield return  AssetBundle.GetAllLoadedAssetBundles();
    
    //Load Init
    AssetBundleCreateRequest re = AssetBundle.LoadFromFileAsync(uri);
    
    yield return re;
    ab = re.assetBundle;
    string PreInit = "";
    if(PlayerPrefs.HasKey("PreScene")){
        PreInit = PlayerPrefs.GetString("PreScene");
    }else{
        PreInit = "Chap01Qika";
        PlayerPrefs.SetString("PreScene",PreInit);
    }
    //
    GameObject g = ab.LoadAsset<GameObject>(PreInit);
    //
    if(g!=null){
        GameObject ng= Instantiate(g);
         ng.transform.position=Vector3.zero;
        enviormentPrefab = ng;
        mainCamera = enviormentPrefab.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        NetworkServer.Spawn(ng);
    }else{
        Debug.LogWarning("Can't Load From ab");
    }
    Debug.Log("Load Init Scene");

    //  yield return request;
    //  ab = request.assetBundle;
    Debug.Log("Model Load Done");

    gameCamera.LookAt=manager.selectionLocations[0];
    }
    
    
   void LoadTownData(string map)
   {
       Debug.Log("Load Map"+map.ToString());
int m=0;
       //Clear Old Obj
      foreach(var e in npcModels){
           if(e!=null){
               Destroy(e);
           }
       }

       //For Items Load
       LoadCardSprite();
       InventorySystem.instance.LoadInventory();
       InventorySystem.instance.LoadEquipment();
       
       
       MapLocation tLoc = null;
        foreach (var loc in maps)
        {
            if (loc.locationName == map)
            {
                tLoc = loc;
                mapLocation = tLoc;
                if (loc.townType == TownType.Main)
                {
                    PlayerPrefs.SetString("locName",map);
                }
                // Debug.Log("load map"+tLoc.ToString());
            }
        }

         //Load Scene
        if(enviormentPrefab!=null){
            Destroy(enviormentPrefab);
        }
        //Load Scene Obj
        GameObject sceneObj = ab.LoadAsset<GameObject>(mapLocation.locationScene);
        if(sceneObj!=null){
            GameObject nsObj = Instantiate(sceneObj)as GameObject;
            enviormentPrefab=nsObj;
            if(enviormentPrefab!=null){
                enviormentPrefab.GetComponentInChildren<CinemachineVirtualCamera>().Priority=400;
            }else{
                Debug.Log("Can't Got Camera");
            }
            nsObj.transform.position=Vector3.zero;
            NetworkServer.Spawn(nsObj);
        }else{
            Debug.Log("Can't Load Scene");
        }
    
       
        if (mapLocation != null)
        {
            //Load Location Data
            if(TownManager.CheckLan()==true){
            locationText.text = mapLocation.locationName;
            }else{
                locationText.text = mapLocation.elocationName;
            }
            locationBG.sprite = mapLocation.locationBG;
            // loactionDetail.text = mapLocation.locationDetail;
            leaveTownText.text = "离开"+mapLocation.locationName.ToString();
            locationEnemy = new List<EnemyAsset>();
            foreach (EnemyAsset ea in mapLocation.enemyList)
            {
                if (ea != null)
                {
                    locationEnemy.Add(ea);
                }
            }

       
            //Add NPC
            Debug.Log("=====>Add Town NPC Not Dungeon");
            if(mapLocation.isDungeon==false){
            npcTownObj = new List<GameObject>();
            for (int i = 0; i < locationEnemy.Count; i++)
            {
                if (locationEnemy[i] != null)
                {
                    if (NetworkClient.isConnected)
                    {
                        //Merchant und Others is NPC 
                        if(locationEnemy[i].npcType!=NpcType.Enemy){
                            //3d Add pos
                            //Dire
                        // GameObject enemyObj = Instantiate(NPCObj, npcPos.position, Quaternion.identity) as GameObject;
                        GameObject enemyModel =null;
                        //Load Model
                        Debug.Log("AB LOAD MODEL"+locationEnemy[i].EnemyName);
                        enemyModel =  LoadItemFromAB(locationEnemy[i].model,locationEnemy[i]);
                        

   m++;
       
                    Debug.Log("Got Enemy Module");
                        if(locationEnemy[i].npcType ==NpcType.Merchant||locationEnemy[i].npcType==NpcType.None ){
                            //Add Component         
                              enemyModel.GetComponent<NPCManager>().asset = locationEnemy[i];
                        enemyModel.GetComponent<NPCManager>().converName = locationEnemy[i].conver;
                        // enemyModel.GetComponent<MerchantController>().converName = locationEnemy[i].conver;
                        
 
                        // enemyObj.GetComponent<NPCManager>().model=Resources.Load("CModel/"+locationEnemy[i].model.name)as GameObject;
                        //                
                        }else if(locationEnemy[i].npcType == NpcType.Enemy|| locationEnemy[i].npcType == NpcType.Others ){
                             enemyModel.GetComponent<Monster>().enemyAsset = locationEnemy[i];
                        enemyModel.GetComponent<Monster>().converName = locationEnemy[i].conver;
                        enemyModel.GetComponent<Monster>().cardList = new List<CardAsset>(locationEnemy[i].cardList);
                        // enemyModel.GetComponent<Monster>().HP= locationEnemy[i].Health;
                        
                        
                        }
                        enemyModel.GetComponent<DialogueActor>().actor = locationEnemy[i].EnemyName;
                        enemyModel.GetComponent<DialogueActor>().portrait = locationEnemy[i].Head.texture;
                        //
  
                     
                           NetworkServer.Spawn(enemyModel);
                        Debug.Log("Try Got Model"+locationEnemy[i].model);
                        
                        
                        
                        
                        //if has pack set to enemy deck selection
                        if(locationEnemy[i].hasCard&&atDungeon==false){
                        GameObject enemyPack = Instantiate(enemySelection.deckIcon,enemySelection.contentPos.position,Quaternion.identity)as GameObject;
                        enemyPack.transform.parent = enemySelection.contentPos;
                        enemyPack.transform.localScale = new Vector3(1,1,1);
                        enemyPack.GetComponent<EnemyPortraitVisual>().enemyAsset = locationEnemy[i];
                        enemyPack.GetComponent<EnemyPortraitVisual>().EnemyHead.sprite = locationEnemy[i].Head;
                        //
                        if(TownManager.CheckLan()){
                        enemyPack.GetComponent<EnemyPortraitVisual>().EnemyName.text = locationEnemy[i].EnemyName;
                        }else{
                             enemyPack.GetComponent<EnemyPortraitVisual>().EnemyName.text = locationEnemy[i].eEnemyName;
                        }
                        //
                         //Sell Items if is merchant
                       
                        //
                        enemyPack.GetComponent<EnemyPortraitVisual>().enemyCardList = new List<CardAsset>(locationEnemy[i].cardList);
                        
                        enemySelection.deckIcons.Add(enemyPack);
                        }
                        //sell items
                        Debug.Log("Merchant NPC");
                        if (locationEnemy[i].npcType == NpcType.Merchant)
                        {
                            if (locationEnemy[i].itemShopIDs != null)
                            {
                                Debug.Log(locationEnemy[i].EnemyName+"check item shop"+locationEnemy[i].itemShopIDs.Count);
                                if (locationEnemy[i].itemShopIDs.Count > 0)
                                {
                                    for (int j = 0; j < locationEnemy[i].itemShopIDs.Count; j++)
                                    {
                                        Items item = ItemDatabase.instance.FindItemByName(locationEnemy[i].itemShopIDs[j]);
                                        Debug.Log("Try Get Item" + item.itemName + item.equipmentSlotype + "\n" +
                                                  item.itemType + "\n");

                                        enemyModel.GetComponent<MerchantController>().itemIDs
                                            .Add(locationEnemy[i].itemShopIDs[j]);
                                    }
                                }

                                enemyModel.GetComponent<MerchantController>().MerchantAddItemFromNPC();
                            }
                        }

                       
                    }
                    }

                }
                        
                }
            }else{
                Debug.Log("Dungeon NPC Generate At config Dungeon");
            }
            //Loc is Dungeon,Has Enemy and event needs explore,
            //the Bottom UI hide DeckBuilder 
            //
            if(mapLocation.isDungeon==true && atDungeon==true){
                
                Debug.Log("is Dungeon Show DungeonRoadSide");
                BattleStartInfo.AtDungeon=true;
               
                explore.panel.Open();
                    explore.mapLocation = mapLocation;
                    // leaveObj.gameObject.SetActive(false);


               if(mapLocation.dungeonType !=DungeonType.Init){
                    noticeUI.gameObject.SetActive(true);
               
                    //Set Notice UI Content
                    if(TownManager.CheckLan()==true){
                    dungeonLocText.text=mapLocation.locationName;
                    }else{
                        dungeonLocText.text=mapLocation.elocationName;
                    }
                    currentLoc.text=GetDungeonNames(mapLocation.dungeonType);
                    // goalText.text = mapLocation.locationDetail.ToString();
                    leaveTownText.text = "离开地下城";
                    //difficult
                    // if(BattleStartInfo.DungeonDifficult=="普通"){
                    //     DungeonExplore.instance.dungeonDifficult=DungeonDifficult.Normal;
                    // }else if(BattleStartInfo.DungeonDifficult=="困难"){
                    //     DungeonExplore.instance.dungeonDifficult=DungeonDifficult.Hard;
                    // }
                    //
               
                    

                    StartCoroutine(StartDungeon());
               }
                  else{
                      //
                      DungeonExplore.instance.ConfigDungeon();
                  }
            }
                   
           
          
        }
        else
        {
            Debug.LogError("NO LOC");
        }
        //
        DeckBuildScreen.ShowScreenForCollectionBrower();
        DeckBuildScreen.CloseCB();
            Debug.Log("Load Success"+locationText.text);
             
           
        
    }
    public string GetQuestDetail(){
         foreach (var q in QuestLog.GetAllQuests())
      {
          if (q != null)
          {
              return QuestLog.GetQuestDescription(q);
          }
          else
          {
              return  "找点事做";
          }
      }

      return "";
    }

    IEnumerator StartDungeon(){
        Debug.Log("Start Init");
        BG.gameObject.SetActive(true);
        //top pane hide pack
        foreach(var v in topPanelManager.buttons){
            if(v.name=="DungeonConfig" || v.name=="HidePanel"){
                v.gameObject.SetActive(true);
            }
            if(v.name=="Store"){
                v.gameObject.SetActive(false);
            }
        }

        //SetNoticeUI
        PreSelectPack = true;
        //SetCamera
        // mainCamera.Priority=400;
        explore.ConfigDungeon();
        yield return 0;


        while(!canStart){
            yield return null;
        }

        if(canStart==true&& BattleStartInfo.SelectDeck!=null){
            BG.gameObject.SetActive(false);
            noticeUI.gameObject.SetActive(false);
            // switch(DungeonExplore.instance.dungeonDifficult){
            //     case DungeonDifficult.Hard:
            //         //
            //         Debug.Log("Dungeon Mode Load");
            //         DungeonExplore.instance.LoadChaos();
            //         break;
            //     break;
            // }
        }
        //
        else
        {
            MessageManagers.instance.AddMessage("需要卡组",3.0f);
        }

         topPanelManager.PanelAnim(0);
        
        yield return 0;
    }


    //CORE SHOW TOWN DATA UND LOAD DATA FROM DATABASE
   public void ShowTown(bool isShow)
   {

      
       //try get variable
    //   DialogueLua.SetVariable("DungeonBoss",0);
      //show UI
    //   Sequence sequence= DOTween.Sequence();
     
       //Change Camera
      selectGroup.gameObject.SetActive(false);

    //    gameCamera.gameObject.SetActive(false);
    //    mainCamera.Priority=300;
      
       LoadMap();
    //    PlayerData.localPlayer.SavePanel.SetActive(false);
       //main -> town
       if( PlayerPrefs.HasKey("PlayerName"))
      { 
          content.Open();
          if (PlayerPrefs.HasKey("locName"))
          {
              string loc = PlayerPrefs.GetString("locName");
              Debug.Log("Loc Name Load"+loc.ToString());
              foreach (MapLocation map in maps)
              {
                  if (loc == map.locationName)
                  {
                      LoadTownData(loc);
                  }
              }
              //panel
            //   newBeePanel.gameObject.SetActive(true);
            //    if(hasNB==false){
            //     newBeePanel.SetActive(true);
            //     Debug.Log("HAS X KEY");
            // }
            // else{
            //     newBeePanel.gameObject.SetActive(true);
            //     Debug.Log("Show new bee");
            // }

          }
          else
          {
            //   First game
              GDEMapLocationData map = new GDEMapLocationData(GDEItemKeys.MapLocation_迷离意识);
              LoadTownData(map.MapName);
              if(map.MapType=="Main"){
              PlayerPrefs.SetString("locName",map.MapName);
              }
            //   show 
               newBeePanel.gameObject.SetActive(true);
               hasNB = PlayerPrefsX.GetBool("TownNBUI");
            // if(hasNB==false){
            //     newBeePanel.SetActive(false);
            //     Debug.Log("HAS X KEY");
            // }
            // else{
            //     newBeePanel.SetActive(true);
            //     Debug.Log("Show new bee");
            // }

          }
               
               
       for (int i = 0; i < transform.childCount; i++) 
       {
           transform.GetChild(i).gameObject.SetActive(isShow);
       }

    
      }
       else
       {
           Debug.Log("Not Lobby");
       }
       
       
   
   }

  

   public void EnterTown(string mapLoc,bool isDungeon)
   {
       //destory obj
       if(TravelSystem.TravelSystem.instance.currentPlayerObj!=null){
           Destroy(TravelSystem.TravelSystem.instance.currentPlayerObj);
       }
       //
       if(isDungeon==false){
       PlayerData.LOCTYPE = LocType.Town;
       if (PlayerPrefs.HasKey("InMap"))
       {
           PlayerPrefs.DeleteKey("InMap");
       }
       }else{
             PlayerData.LOCTYPE = LocType.Dungeon;
           atDungeon=true;
       }
       Debug.Log("Start Loading pro");
       loadingScreenManager.LoadSceneWait();
       //
       LoadTownData(mapLoc);
    //    mainCamera.enabled = true;
    //    mainCamera.Priority = 300;
       worldMapCamera.Priority = 0;
       worldMap.gameObject.SetActive(false);
       MapObj.gameObject.SetActive(false);
       mapCameraGroup.gameObject.SetActive(false);
       content.gameObject.SetActive(true);
   }
public static bool CheckLan(){
    if(currentLanguage=="zh"){
        return true;
    }else {
        return false;   //en0
    }
}
   //
public void JumpMap(){
    DungeonExplore.moneyPool = 0;
            DungeonExplore.dustPool = 0;
            DungeonExplore.expPool = 0;
 if (atDungeon == true)
        {
            //Reset All Data
            canStart = false;
            atDungeon = false;
            DungeonExplore.instance.ClearDungeon();
            DungeonExplore.instance.canLeave = false;
            DungeonExplore.instance.hasKillBoss = false;
            DungeonExplore.instance.hasCurrentDone = false;
            //
            
            //
            BattleStartInfo.AtDungeon = false;
            //
            DungeonExplore.instance.returnPanel.gameObject.SetActive(false);
            cameraGroup.gameObject.SetActive(true);
            //
            foreach(var v in topPanelManager.buttons){
            if(v.name=="DungeonConfig" || v.name=="HidePanel"){
                v.gameObject.SetActive(false);
            }
            if(v.name=="Store"){
                v.gameObject.SetActive(true);
            }
        }
            
        }

        //
        DeckStorge.instance.LoadDecksFromPlayerPrefs();
        if(worldMapCamera!=null && MapObj!=null)
        {
            //
           
          // LoadMap();
          Debug.Log("Load Map Data");
          TravelSystem.TravelSystem.instance.LoadMapData();
          TravelSystem.TravelSystem.instance.LoadPlayerTravel();

          
          //Clear Deck Icon
          foreach(GameObject ei in enemySelection.deckIcons){
              if(ei!=null){
                Destroy(ei);
           }
          }
          //Destory  envior
          if(enviormentPrefab!=null){
              Destroy(enviormentPrefab);
          }

          //destory fast travel list
        foreach(var fm in TravelSystem.TravelSystem.instance.tList){
            if(fm!=null){
                Destroy(fm);
            }
        }

          //enemymodel
          foreach(var e in npcModels){
              if(e!=null){Destroy(e);}
          }



            Debug.Log("Leave The town stay at world Map");

            PlayerPrefsX.SetBool("InMap", true);
            mapLocation = null;
            locationText.text = "";
            locationBG.sprite = null;
            locationEnemy = null;
            loactionDetail.text = "";
            content.gameObject.SetActive(false);

           
        //    mainCamera.gameObject.SetActive(false);
            // mainCamera.Priority=0;
        //    mapCameraGroup.gameObject.SetActive(true);
        mapCameraGroup.gameObject.SetActive(true);
           worldMapCamera.Priority=500;

           
            //destory obj
            for (int i = 0; i < npcTownObj.Count; i++)
            {
                Destroy(npcTownObj[i]);
            }
           
            Debug.Log("Opennmap");
            worldMap.gameObject.SetActive(true);
            worldMapCamera.enabled = true;
            // mainCamera.enabled = false;
        
       
               //Check Lock state
               foreach (var a in TravelSystem.TravelSystem.instance.areaCollection)
               {

                    if (PlayerPrefs.HasKey(a.location.elocationName + "_Lock"))
           {
               int g = PlayerPrefs.GetInt(a.location.elocationName + "_Lock");
               if (g == 0)
               {
                //    Debug.Log("has key "+newMap.locationName.ToString());
                a.Locked=false;
                
               
                   if (a.Locked == false)
                   {
                       a.gameObject.GetComponent<BoxCollider>().enabled=true;
                       a.gameObject.SetActive(true);
                   }
                   else
                   {
                        a.gameObject.GetComponent<BoxCollider>().enabled=false;
                        a.gameObject.SetActive(false);
                   }
                   
               }
           }
               }
            MapObj.gameObject.SetActive(true);
               
               
             //Obj
             Debug.Log("Load Map Loc"+TravelSystem.TravelSystem.instance.areaCollection.Count);
            int cl = 1;
             //
                 foreach(var c in TravelSystem.TravelSystem.instance.areaCollection){
                    if(PlayerPrefs.HasKey(c.location.elocationName+"_Lock")){
                        cl = PlayerPrefs.GetInt(c.location.elocationName+"_Lock");
                    }else{
                        cl=1;   //default is lock
                    }

                     if(cl==0){
                     GameObject tObj = Instantiate(FtObj,Ftpos.position,Quaternion.identity)as GameObject;
                 tObj.name = c.location.locationName.ToString();
                 tObj.transform.SetParent(Ftpos);
                 tObj.transform.localScale= new Vector3(1,1,1);
                 if(TownManager.CheckLan()==true){
                 tObj.GetComponent<FastTravel>().locText.text = c.location.locationName.ToString();
                 }else{
                      tObj.GetComponent<FastTravel>().locText.text = c.location.elocationName.ToString();
                 }
                  tObj.GetComponent<FastTravel>().currentArea = c;
                 tObj.GetComponent<FastTravel>().tBtn.onClick.AddListener(()=>{
                    //  if(TravelSystem.TravelSystem.instance.currentArea == tObj.GetComponent<FastTravel>().currentArea){
                    //      tObj.GetComponent<FastTravel>().tBtn.enabled=false;
                    //      return;
                    //  }
                     //Travel
                     TravelAreaWindow.instance.SetTravelAreaWindow(tObj.GetComponent<FastTravel>().currentArea);
                 });
                  TravelSystem.TravelSystem.instance.tList.Add(tObj);
                 NetworkServer.Spawn(tObj);
                    
                 }else{
                 }
                //  }
             }
             
             //
             TravelSystem.TravelSystem.instance.ShowAreaWindow(TravelSystem.TravelSystem.instance.currentArea);
             
//    TravelSystem.TravelSystem.instance.SetMarkerLocation();
 
  

               DialogueManager.SendUpdateTracker();

        }
}
    //show World Map
    public void WorldMap(){
       // TravelSystem.TravelSystem.instance.LoadPlayerTravel();
       loadingScreenManager.LoadSceneWait();
        // loadScreen.gameObject.SetActive(true); 
           JumpMap();
        //    TravelSystem.TravelSystem.instance.playerMarker.transform.position = Vector3.Lerp( TravelSystem.TravelSystem.instance.currentArea.gameObject.transform.position, TravelSystem.TravelSystem.instance.playerMarker.transform.position,0.4f*Time.deltaTime);
           

    }

    public void UpdateTown()
    {
        foreach (var obj in npcTownObj)
        {
            obj.gameObject.SetActive(true);
        }
        Canvas.ForceUpdateCanvases();
    }


     public void ShowPopup(NPCManager nPCManager)
    {
       if(nPCManager==null)return;
       //
       popupObj.gameObject.SetActive(true);
       if(CheckLan()==true){
       nameText.text = npcManager.asset.EnemyName.ToString();
       }else{
           nameText.text = npcManager.asset.eEnemyName.ToString();
       }
       
    }
public GameObject LoadItemFromAB(string abName,EnemyAsset ea){
    if(ea.model!=""){
        return  GetObjRoutine(abName,ea);
    }
    return null;
}
public GameObject GetObjRoutine(string n,EnemyAsset ea){
    GameObject ng=null;
    if(ea.model!=null){
        Debug.Log("Got Model"+ea.model);
     ng= ab.LoadAsset<GameObject>(ea.model);
    }
    if(ng!=null)
    {   
    
     ng= Instantiate(ng)as GameObject;

        npcModels.Add(ng);
        ng.transform.SetParent(npcModepos);

        // mainCamera.LookAt=ng.transform;
    NetworkServer.Spawn(ng);

    return ng;
        }else{
    Debug.Log("ab can't load");
}
return null;
}
    public void LoadMap()
    {
        List<GDEMapLocationData> alllMap = GDEDataManager.GetAllItems<GDEMapLocationData>();
        for (int i = 0; i < alllMap.Count; i++)
        {
            // Debug.Log("try got map"+alllMap[i].MapName);
            MapLocation newMap = new MapLocation();
            newMap.locationID =alllMap[i].MapID.ToString();
            
            newMap.locationName = alllMap[i].MapName;
            newMap.elocationName =alllMap[i].EMapName;
            newMap.locationScene=alllMap[i].LocationScene;

            // Debug.Log("LOC NAMES IS"+newMap.locationName.ToString());
            newMap.townType = GetTownType(alllMap[i].MapType);
            newMap.dungeonType = GetDungeonType(alllMap[i].DungeonType);
            newMap.nextLocation = alllMap[i].NextLocation;

            newMap.locationDetail = alllMap[i].LocDetail;
            newMap.isDungeon = alllMap[i].IsDungeon;
            newMap.isLock = alllMap[i].IsLock;
            // newMap.sceneName = alllMap[i].LocationScene;
            newMap.itemList = alllMap[i].DItems;
            // newMap.cameraPos =alllMap[i].CameraPos;
            // newMap.DirePos=alllMap[i].DirePos;
            // newMap.camPos =new Vector3(alllMap[i].CamPos.x,alllMap[i].CamPos.y,alllMap[i].CamPos.z);
            // if(alllMap[i].CamObj!=null){
            // newMap.DireAsset =GetBossAsset(alllMap[i].CamObj);
            // }
            
            
           if (PlayerPrefs.HasKey(newMap.elocationName + "_Lock"))
           {
               int g = PlayerPrefs.GetInt(newMap.elocationName + "_Lock");
               if (g == 0)
               {
                   Debug.Log("has key "+newMap.elocationName.ToString());
                newMap.isLock=false;
                
               }
            
           }else{
               //first
               PlayerPrefs.SetInt(newMap.elocationName+"_Lock",1);
               newMap.isLock=true;
           }


            newMap.hasEvent = alllMap[i].HasEvent;
     
            //NPC
            newMap.enemyList = new List<EnemyAsset>(GetEnemyListFromGDE(alllMap[i].DungeonEnemyList));
            newMap.enemyPos = new List<Vector3>(alllMap[i].EnemyPos);
            newMap.NeedsKill = alllMap[i].DungeonNeedsKill;
            // newMap.locationBG = Utils.CreateSprite(alllMap[i].LocBG);
            //Loc Load Obj From AB
            //Boss
            newMap.hasBoss = alllMap[i].HasBoss; 
if(newMap.hasBoss==true){
            newMap.bossAsset = GetBossAsset(alllMap[i].DungeonBoss);
            
            
}
            maps.Add(newMap);

        }
        
        //dic
         for (int i = 0; i < maps.Count; i++)
         {
                if (!mapDic.ContainsKey(maps[i].locationName))
                {
                    mapDic.Add(maps[i].locationName,maps[i]);
                }
         }
    }

    public EnemyAsset GetBossAsset(GDEEnemyAssetData boss)
    {
        // Debug.Log("Try Got Boss"+boss.NpcName);
        boss = new GDEEnemyAssetData(boss.Key);
        
        EnemyAsset e =new EnemyAsset();
        e.EnemyName=boss.NpcName;
        e.eEnemyName = boss.ENpcname;
        // Debug.Log(e.EnemyName+"Got boss");
        e.Tags = boss.Tags;
        e.npcType = Utils.ConvertNpcType(boss.NpcType);
        e.Head=Utils.CreateSprite(boss.Head);
        e.Frame=Utils.CreateSprite(boss.Frame);
        e.Health=boss.Health;
        e.ratity = Utils.ConvertRarity(boss.Rarity);
               
        e.conver= boss.Conver;
        // e.Loc=boss.MapLocation;
        e.powerName=boss.PowerName;
        e.exp=boss.Exp;
        e.gold=boss.Money;
        e.isLock=boss.IsLock;
        e.detail=boss.Detail;
        e.hasCard=boss.HssCard;
        e.model=boss.ModelName;
        // e.pos=boss.Pos;
                
        if (e.hasCard == true)
        {
            e.cardList = new List<CardAsset>(ConvertCard(boss.CardList));
        }

      if(boss.ItemSellID.Count>0){
            e.itemShopIDs = new List<string>(ConvertItems(boss.ItemSellID));
        }

        e.isBoss=boss.IsBoss;

        return e;

    }
    //
    public List<EnemyAsset> GetEnemyListFromGDE(List<GDEEnemyAssetData> enemy){

        List<EnemyAsset> eas = new List<EnemyAsset>();



        for (int i = 0; i < enemy.Count; i++)
        {
            EnemyAsset e = new EnemyAsset();
            e.EnemyName = enemy[i].NpcName;
            e.eEnemyName=enemy[i].ENpcname;
            // Debug.Log(e.EnemyName + "Got enemy ");
            e.Tags = enemy[i].Tags;
            e.npcType = Utils.ConvertNpcType(enemy[i].NpcType);
            e.Head = Utils.CreateSprite(enemy[i].Head);
            e.Frame = Utils.CreateSprite(enemy[i].Frame);
            e.Health = enemy[i].Health;
            e.ratity = Utils.ConvertRarity(enemy[i].Rarity);

            e.conver = enemy[i].Conver;
            // e.Loc = enemy[i].MapLocation;
            e.powerName = enemy[i].PowerName;
            e.exp = enemy[i].Exp;
            e.gold = enemy[i].Money;
            e.isLock = enemy[i].IsLock;
            e.detail = enemy[i].Detail;
            e.hasCard = enemy[i].HssCard;
            //
            e.model =enemy[i].ModelName;
            //
           

            if (e.hasCard == true)
            {
                e.cardList = new List<CardAsset>(ConvertCard(enemy[i].CardList));
            }

            if (enemy[i].ItemSellID.Count > 0)
            {
                // e.itemShopIDs = new List<string>(ConvertItems(enemy[i].ItemSellID));
                e.itemShopIDs = new List<string>(enemy[i].ItemSellID);
            }
        

        e.isBoss=enemy[i].IsBoss;
           
           
            eas.Add(e);
        }


        
        
        return new List<EnemyAsset>(eas);

    }
    public static List<string> ConvertItems(List<string> it){
        List<string> itemList = new List<string>();
        List<GDEItemsData> aitems = GDEDataManager.GetAllItems<GDEItemsData>();

        for(int i=0;i<it.Count;i++)
            for(int j =0 ;j<aitems.Count;j++){
            if(it[i]==aitems[j].ItemID){
                // it[i] =new GDEItemsData(it[i].Key);
                Debug.Log(it[i]+"Find Items");
                 Items its = ItemDatabase.instance.FindItem(int.Parse(it[i]));
                    itemList.Add(its.itemID);
            }
            }
        

        return itemList;
    }

  public string GetCardCharacterAsset(string characterAsset)
    {
        List<GDECharacterAssetData> gda = GDEDataManager.GetAllItems<GDECharacterAssetData>();
		for(int i=0;i<gda.Count;i++){
	     if(gda[i].ClassName==characterAsset){
			 GDECharacterAssetData cs =new GDECharacterAssetData(gda[i].Key);
			return cs.ClassName;
		 }
		}
		return null;
		
    }

    public  List<CardAsset> ConvertCard(List<GDECardAssetData> c){
        List<CardAsset> cs = new List<CardAsset>();
        for( int i=0;i<c.Count; i++){
            // Debug.Log("LoadCard"+c[i].CardName);
            CardAsset ca = CardCollection.instance.GetCardAssetByName(c[i].CardName);

            cs.Add(ca);
           
        }

        return cs;
    }

    public TownType GetTownType(string t){
        if(t=="Main"){
            return TownType.Main;
        }else 
        return TownType.Side;
    }

    public DungeonType GetDungeonType(string d){
        if(d=="Kill"){
            return DungeonType.Kill;
        }else if(d=="Escape"){
            return DungeonType.Escape;
        }else if(d=="Collect"){
            return DungeonType.Collect;
        }else if (d == "Boss")
        {
            return DungeonType.Boss;
        }else if(d=="Init"){return DungeonType.Init;}

        return DungeonType.None;
    }

    public string GetDungeonNames(DungeonType type)
    {
        if (type == DungeonType.Kill)
        {
            return "击杀";
            
        }else if (type == DungeonType.Collect)
        {
            return "收集";
        }else if (type == DungeonType.Boss)
        {
            return "首领";
        }else if (type == DungeonType.Escape)
        {
            return "逃离";
        }

        return "";
    }
    public MapLocation GetLoctionByNames(string loc)
    {
        if (mapDic.ContainsKey(loc))
        {
            return mapDic[loc];
        }
        else
            return null;
    }

    public void NewBeeClose(){
        if(newBeeToggle.isOn){
            //Save
           PlayerPrefs.SetInt("NBP0",1);
           
        }
         newBeePanel.gameObject.SetActive(false);
    }

    public void PreSelect()
    {
        PreSelectPack = false;
    }



    public void ApplicationQuit(){
       Resources.UnloadAsset(ab);
       ab.Unload(true);
       if(ab.Contains("npcmodel")){
           Debug.Log("has model");
       }else{
           Debug.Log("All clear ab");
       }
       Debug.Log("Unlock AB");
    }
}

