
using Mirror;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using TMPro;
using DungeonArchitect;
using DungeonArchitect.Builders.GridFlow;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.AI;
using GameDataEditor;
using System.Linq;
//
using Invector;
using Invector.vCharacterController;
using Invector.vItemManager;
using Invector.vMelee;
using Invector.vCharacterController.vActions;
using Invector.vCamera;
using Invector.Utils;

public enum DifficultType
{
    Normal,
    HardCore,
    ShenShan,
}
/// <summary>
///  Townmanager for player 
/// </summary>
///
    [System.Serializable]
    public class TownManager : NetworkBehaviour {
        
    
    public static TownManager instance;
    [Header("Common")]
    public Players player;
    public GameObject mainCamera;
    public vGameController vGameController;
    public vThirdPersonCameraListData cameraListData;
    public vItemListData listData;
    public int party = -1;
    public List<Entity> partyList;
    public bool hasParty = false;
    public vFadeCanvas fadeCanvas;


    [Header("MapLoc")]
    public List<DungeonAsset> mapList;   //storge map list 
    public DungeonAsset dungeonaAsset;
    public GameObject dungeonObj;
    public List<vItem> dungeonItemList;

    [Header("DungeonSize")]
    //public int seed = -1;
    //public int dungeonSize = -1;
    //public DifficultType type = DifficultType.Normal;
    //public List<Entity> enemyList;
    //public DungeonConfig config;
    //public DungeonThemeEngine themeEngine;
    //public DungeonModel dungeonModel;
    //public SyncDungeon dungeonList=new SyncDungeon();
    //public Dungeon thdnDungeon;
    //public NavMeshSurface navMesh;
    


    [Header("states")]
    //public GameManagers battleManager;  //setup board
    //public GameManager globalSetting;   //common 
    public SoundManager sound;  //sound
    public bool atDungeon = false;
    public bool haveEnemies = false;

    


    [Header("Others")]
    
    public NavMeshSurface navmesh;  //build before load
   
    public NetworkManagerTHDN manager; ///network 
    private AssetBundle assetBundle;

    //public TravelSystem travel;
    [Header("LocDetail")]
    public GameObject sceneObj;
    public List<GameObject> sceneList;
    public string lastScenename;
    public NetworkManagerTHDN managerTHDN;

  

    [Header("UI Module")]

    
    public TextMeshPro loadingText;
    public vInventory inventory;
    public UISkill inventoryUI;
    public QuestLogWindow questLog;
    public UISetting setting;
    public vHUDController hUDController;
    public UIParty uIParty;

    //HUD
    public GameObject hudOBj;
    
    public ItemDatabase itemDatabase;
    public THDNGameDatabase gameDatabase;



    [vEditorToolbar("Common Obj")]
    public GameObject deadLoot;
   
    //for dungeon config
    public bool rndProp=false;
    public DifficultType difficult=DifficultType.Normal;
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        instance = this;
        //
        inventory = FindObjectOfType<vInventory>().GetComponent<vInventory>();
        managerTHDN = FindObjectOfType<NetworkManagerTHDN>().GetComponent<NetworkManagerTHDN>();
        //
        Debug.Log("Load Main Game");
        player = FindObjectOfType<Players>().GetComponent<Players>();
        //GameDebug
        // GameDebug.Init(Application.dataPath,"GameLogs");
      GDEDataManager.Init("gde_data");
        //Load Dungeon asset
        StartCoroutine(InitABMRoutine());
        //LoadItem
        // LoadItems();
        //LoadMap when change map load all objLoc
        //
        Debug.Log("Load Game Data");
        LoadSkillData();
      
        // ItemDatabase.instance.
        //Load Map
        LoadTown();
        // LoadParty();

        }

    #region ChangeMap
    // manager the location operation ,includes
    //1.leave town
    //when player leave town ,needs destory scene und obj ,camera need change map camera und
    // load playermark by current loc , at map loc, load all maploc who unlock or not . player can
    // select one of the locTile then enter the target dungeon
    //2.enter town
    //3.enterdungeon
    //TODO 127  leave load last scene name 
    public void LeaveTown()
    {
        //
        //if (thdnDungeon != null)
        //{
        //    thdnDungeon.DestroyDungeon();
        //    //
        //}
        
        ////destory old obj
        //foreach (var o in entityObj)
        //{
        //    if (o != null)
        //    {
        //        NetworkServer.Destroy(o);
        //    }
        //}
        //    if (sceneObj != null) { NetworkServer.Destroy(sceneObj); }
        //    //Change Camera depth
        //    if (townCamera != null)
        //    {
        //        townCamera.Priority = 0;
        //    }


        //load map tile
    //    Util.LoadMap();
        //Load map from gdeMapData und load all
        //Travel System load data
        //load player mark
        //playerMarker.transform.position = TravelSystem.travelSystem.playerMarker.trasform.position;
        //

    }

    

    
    //TODO _1117 load model from ab 
    // at thdn save (dungeon & model )
    public IEnumerator InitABMRoutine()
    {
       string fn="gb";
        // string uri= "file:///"+Application.dataPath+"/AssetBundles/"+modelName;
        // string uri=Application.dataPath+"/AssetBundles/StandaloneWindows/"+modelName;
        string uri = Application.dataPath + "/StreamingAssets/"+fn;

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

       
        
        assetBundle = re.assetBundle;
        Debug.Log(assetBundle.GetPath().ToString());
        yield return assetBundle;

    //     string PreInit = "";
    //     //TODO 127 
    //     if(PlayerPrefs.HasKey("_lastsavename")){
    //       string lastScene = PlayerPrefs.GetString("_lastsavename");
    //       lastScene = lastScenename;
    //   }
    //     GameObject g = ab.LoadAsset<GameObject>(PreInit);
    //     //
    //     if (g != null)
    //     {
    //         GameObject ng = Instantiate(g);
    //         ng.transform.position = Vector3.zero;
    //         sceneObj = ng;
    //         //mainCamera = enviormentPrefab.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
    //         NetworkServer.Spawn(ng);
    //     }
    //     else
    //     {
    //         Debug.LogWarning("Can't Load From ab");
    //     }
    //     Debug.Log("Load Init Scene");

    
     
    }

    /// <summary>
    /// TODO 127 LoadTown By AB or lastscene
    /// Init Player first loaded default dungeon then set scene to PS
    /// when loaded second,load the last dungeon which type ist town
    /// 
    /// </summary>
    /// <param name="f"></param>
    public void LoadTown()
    {
        Debug.Log(assetBundle.GetPath().ToString());
    //   Load scene
      if(PlayerPrefs.HasKey("_lastsavename")){
          string lastScene = PlayerPrefs.GetString("_lastsavename");
          Debug.Log(lastScene);
           lastScenename =lastScene;
      }else{
          lastScenename="SodtyxJails";
      }

    // Load scene object
    GameObject scene =assetBundle.LoadAsset<GameObject>(lastScenename);
    sceneObj = scene;
    if(scene!=null){
      GameObject s =  Instantiate(scene);
        //
        // scene.GetComponent<GlobalSetting>().surface.BuildNavMesh();
        // scene.GetComponent<NavMeshSurface>().BuildNavMesh();
        NetworkServer.Spawn(s);
        sceneList.Add(s);
    }
    // Generate Player
    player.transform.position = scene.GetComponent<GlobalSetting>().startPosition.position;
    //
    if(isRndProp==true){
    GenerateProp();
    }

        //TODO Update rnd enmey for dungeon by enemy folder
        if (haveEnemies == true)
        {
            GenerateEnemies();
        }
    // //Check Items und validate
    // ItemDatabase.instance.Init();
    // THDNGameDatabase.instance.Init();

    //Add Component
    Debug.Log("Init Player Module");
    InitPlayer();
    
    
    //Validate sync item
   
    

            
        
    }

    /// <summary>
    /// 
    /// </summary>
  void GenerateEnemies()
    {
        var enemiesList = sceneObj.GetComponent<GlobalSetting>().enemyList.ToList();

        if (enemiesList.Count > 0)
        {
            for(int i = 0; i < enemiesList.Count; i++)
            {
                //rnd value und items by dungeon
               
                //load enemy data from gdb preload
                for(int j = 0; j < itemDatabase.cList.Count; j++)
                    if(enemiesList[i].name == itemDatabase.cList[j].cName)
                    {
                        //got enemy und generate by rnd
                        Debug.Log("Got Enemy");
                        //Invector && AI

                        //Got Loot item by vitem-> vfrom

                        //network identity
                        var health = Random.Range(itemDatabase.cList[j].cHealth,itemDatabase.cList[j].cHealth + itemDatabase.cList[j].cHealth * 0.5);
                        var shield = Random.Range(itemDatabase.cList[j].cHealth, itemDatabase.cList[j].cHealth + itemDatabase.cList[j].cHealth * 0.5);
                        var damage = Random.Range(itemDatabase.cList[j].cHealth, itemDatabase.cList[j].cHealth + itemDatabase.cList[j].cHealth * 0.5);
                     

                        //invector ai

                        //emerald ai
                        //load items by itemfrom
                        List<vItem> items = itemDatabase.itemList.Where(citem => citem.itemFrom == enemiesList[i].cFrom).ToList();
                         if(items.Count > 0)
                        {
                            //rnd
                            int rndItemNum = Random.Range(1, 3);
                            for(int n = 0; n < rndItemNum; n++)
                            {
                                //check detail items
                                int ditem = Random.Range(0, items.Count);
                                int ditemCounter = Random.Range(1, 3);
                                //add to enemyAI Loot
                               while(ditemCounter>0)
                                {
                                    //Add to entity
                                    
                                    enemiesList[i].lootItemList.Add(ditem);

                                    //add to EAI

                                    //add to eItemCollection

                                    //
                                    ditemCounter--;
                                }
                            }
                        }  
                    }
            }
        }
    }

    /*
    *
    * public List<DungeonChest> chest;
    public List<Entity> enemyList;
    public List<DungeonShrine> shrineList;
    */

    //generate by rnd und difficult(10%~30%)
   public void GenerateProp(){
     var cd = sceneObj.GetComponent<GlobalSetting>();
     var itemManager= FindObjectofType<vItemManager>();
     if(cd && itemManager){
         //
        if(cd.enemyList.Count>0){
            for(int i=0;i<cd.enemyList.Count;i++){
                if(cd.enemyList[i]!=null){

                    //reload the item from dungeon.dataList
                      //check item counter
                    float SlotItem =Random.Range(1,2);
                    for(int i=0;i<SlotItem.Count;i++){
                        //Check detail item 
                        float itemIndex= Random.Range(0,dungeonItemList.Count);
                        var detailItem = itemManager.GetItems(dungeonItemList.id);
                        cd.enemyList[i].itemList.Add(detailItem);
                        //gernerate item by id to vitemmanager
                        // if(detailItem!=null){
                        //        //generate amount for itemreference
                        // float itemAmount= Random.Range(1,3);
                        
                        // cs.Chest[i].GetComponent<vItemCollection>().item 
                        // = new ItemRegerence{
                        //     id = detailItem.id,
                        //     name = detailItem.name,
                        //     amount = itemAmount,

                        // };
                       
                       
                     
                        // }
                    }
                }

                }
            }
            //
            for(int j=0;j<cs.Chest.Count;j++){
                if(cs.Chest[j]!=null){
                    //got all items


                    //check item counter
                    float SlotItem =Random.Range(1,2);
                    for(int i=0;i<SlotItem.Count;i++){
                        //Check detail item 
                        float itemIndex= Random.Range(0,dungeonItemList.Count);
                        var detailItem = itemManager.GetItems(dungeonItemList.id);
                        //gernerate item by id to vitemmanager
                        if(detailItem!=null){
                               //generate amount for itemreference
                        float itemAmount= Random.Range(1,3);
                        
                        cs.Chest[i].GetComponent<vItemCollection>().item 
                        = new ItemRegerence{
                            id = detailItem.id,
                            name = detailItem.name,
                            amount = itemAmount,

                        };
                       
                       
                     
                        }
                    }
                }
            }
            //
            for(int l=0;l<cs.shrineList.Count;l++){
                if(cs.Chest[l]!=null){
                    //got rnd event
                    int index = Random.Range(0,ItemDatabase.instance.dungeonEventList);
                    DungeonEvent cs = ItemDatabase.instnace.GotDE(index);
                    if(cs!=null){
                        //set to cs.s
                        var de = cs;
                        var shrine = cs.shrineList[i].GetComponent<DungeonShrine>();
                        cs.shrineList[i].GetComponent<DungeonShrine>().dungeonEvent=cs;
                        shrine.amount = Random.Range(cs.deAmount/2,cs.deAmount);
                        shrine.effectTime = de.deEffectTime;
                        shrine.buffType=de.bufType;
                        //Bind UI
                        shrine.nameText.text= de.dename.ToString();
                        shrine.typeText.text= de.buffType.ToString();
                        shrine.detailText.text= de.deDetail.ToString();
                        
                    }
                }
            }
        }

     }else{
         Debug.Log("Couldn't found the dungeon");
         GameDebug.Log("ERROR::Couldn't found the dungeon");
         
     }

   }
    
    
    /// <summary>
    /// every map have own dungeonAsset at map , map place all location with dungeonasset, when player
    /// enter the dungeon with unlock,enter this functions first ,needs check the dungeon type then load enity und scene
    /// if have events for player start cov first
    /// TODO
    /// </summary>
    public void EnterDungeon(int index, bool isDungeon,int num =3 , bool hasEvent=false)
    {
        //got dungeon index
        //DungeonAsset dungeon = dungeonList[index];
        //load dungeon theme und grid flow by dungeonasset::GDE
        
        
    }

    //Bind Data
    void InitPlayer(){
        //
       
        if(Players.localPlayer!=null){
        var pObj = FindObjectOfType<Players>().gameObject;
//      //Add Component


//             pObj.AddComponent<vGenericAction>().GetComponent<vGenericAction>();
//Check Curor

//  pcursor.tpInput.LockCursor(true);
        Debug.Log("Add Invector Component");
       var ptpc=pObj.GetComponent<vThirdPersonController>();
    //    ptpc.MaxHealth = player.healthMax;
    //    ptpc.currentHealth = player.healthMax;
    // //    //
    //   ptpc.healthRecovery = player.healthRate;
    //   //Movement speed
    //   ptpc.walkSpeed = player.WalkSpeed;
    //   ptpc.sprintStamina = player.SprintStamina;
    //   ptpc.currentStamina = player.Stamina;
    //   ptpc.maxStamina = player.Stamina;
    //   ptpc.staminaRecovery = player.staminaRate;
    //   //
      
    //   ptpc.rollSpeed = player.RollSpeed;
    // //   ptpc.UpdateMotor();
    // Debug.Log(string.Format("LOAD PLAYER DONE PLAYER STATS IST===>hp->{0}\nstmina->{1}\n",ptpc.currentHealth.ToString(),ptpc.maxStamina.ToString()));
     

      Debug.Log("Melee Combat");
   var pcombat=pObj.GetComponent<vMeleeManager>();
    pcombat.defaultDamage.damageValue =Mathf.FloorToInt(player.damage); 
// player.gameObject.AddComponent<vItemManager>();
//  var vmci= pObj.AddComponent<vMeleeCombatInput>().GetComponent<vMeleeCombatInput>();
//  StartCoroutine(vmci.CharacterInit());
// player.gameObject.AddComponent<vLockOn>();
//  var vmci= pObj.AddComponent<vMeleeCombatInput>().GetComponent<vMeleeCombatInput>();
//   var pcursor =ptpc.GetComponent<vGenericAction>();
//   pcursor.tpInput.LockCursor(pcursor.tpInput.showCursorOnStart);
  
//   pcursor.tpInput.ShowCursor(pcursor.tpInput.unlockCursorOnStart);
//  StartCoroutine(vmci.CharacterInit());
            //Item
           var pitem=pObj.GetComponent<vItemManager>();
            pitem.inventory = inventory;
            pitem.itemListData =listData;
            //inventory
             Debug.Log("Init Inventory");
            if (inventory!=null)
            {
               pitem.equipmentContainer = new GameObject("Equipment Container");
                pitem.equipmentContainer.transform.parent = transform;
                pitem.equipmentContainer.transform.localPosition = Vector3.zero;
                pitem.equipmentContainer.transform.localEulerAngles = Vector3.zero;

                // Initialize all Inventory Actions
                inventory.GetItemsHandler = pitem.GetItems;
                inventory.GetItemsAllHandler = pitem.GetAllItems;
                inventory.AddItemsHandler = pitem.AddItem;
                inventory.GetAllAmount =pitem. GetAllAmount;
                inventory.onEquipItem.AddListener(pitem.EquipItem);
                inventory.onUnequipItem.AddListener(pitem.UnequipItem);
                inventory.onDropItem.AddListener(pitem.DropItem);
                inventory.onDestroyItem.AddListener(pitem.DestroyItem);
                inventory.onUseItem.AddListener(pitem.UseItem);
                inventory.onOpenCloseInventory.AddListener(pitem.OnOpenCloseInventory);

                var melee = GetComponent<vMeleeCombatInput>();
                if (melee)
                    // Check the vMeleeCombatInput to see the conditions to lock the Inventory Input
                    inventory.IsLockedEvent = () => { return melee.lockInventory; };
            }else{
                Debug.Log("No Inventory");
            }

       
        //
        mainCamera.GetComponent<vThirdPersonCamera>().target=player.transform;
        mainCamera.GetComponent<vThirdPersonCamera>().Init();
       
         vGameController.playerPrefab = pObj;
        vGameController.spawnPoint = sceneObj.GetComponent<GlobalSetting>().startPosition;

        var tcinput =ptpc.GetComponent<vGenericAction>().tpInput;
        tcinput.cameraMain = Camera.main;
        //
        //
         var p_layer = LayerMask.NameToLayer("Player");
           pObj.layer = p_layer;
 
            foreach (Transform t in pObj.transform.GetComponentsInChildren<Transform>())
                t.gameObject.layer = p_layer;

            var s_layer = LayerMask.NameToLayer("StopMove");
            pObj.GetComponent<vThirdPersonMotor>().stopMoveLayer = LayerMask.GetMask(LayerMask.LayerToName(s_layer));
            Debug.Log("Load Player Done ");
            player.GetComponent<vItemManager>().inventory = inventory;
        
    }else{
        Debug.Log("no player prefab");
    
   
    }

    
    }

  


    /// <summary>
    /// 
    /// </summary>
    public void LeaveDungeon(bool isFinish) {
        if (isFinish)
        {
            //


        }
        else
        {
            //clear old dungeon und leave
        }
    }

    #endregion


    #region Dungeon Config
    /// <summary>
    /// BUILD DUNGEON MODULE RULES 
    /// </summary>
    /// <param name="targetDungeon"></param>
    /// <returns></returns>
    // public IEnumerator RebuildDungeonRoutine(Dungeon targetDungeon)
    // {
    //     //TODO
    //     int counter = 0;    //for check dungeon day 
    //     GameDebug.Init(Application.dataPath, "Dungeon::" + targetDungeon.ActiveModel.name.ToString());
    //     //Gamedebg.Log(string.Format("Build Counter:{0},",);
    //     AppendLoadingText("Destory old level und generate new  Level... ");
    //     targetDungeon.DestroyDungeon();
    //     NotifyDestroyed();
    //     yield return 0;
    //     AppendLoadingText("Start Build New Dungeon");
    //     //Check DungeonType
    //     targetDungeon.Build();
    //     //
    //     GlobalSetting.instance.seedID = targetDungeon.Config.Seed;
    //     GameDebug.Log("CURRENT DUNGEON SEED IST"+GlobalSetting.instance.seedID.ToString());
    //     GameDebug.Log("BUILD DUNGEON STRUCT DONE");
    //     yield return 0;
    //     NotifyBuild();
    //     yield return 0;
    //     AppendLoadingText("DONE!\n");
    //     AppendLoadingText("Building Navigation... ");
    //     //navmesh.BuildNavMesh();
    //     yield return 0;     // Wait for a frame to show our loading text
    //     //pre
    //     RebuildNavigation();
    //     AppendLoadingText("DONE!\n");
    //     //TODO entity module
    //     BuildPlayer(DungeonStartInfo.PLAYERS);
    //     AppendLoadingText("Spawning NPCs...");
    //     yield return 0;     // Wait for a frame to show our loading text

    //     //load player done, mini map trace player loc
    //     GlobalSetting.instance.miniMap.enabled = true;
    //     GlobalSetting.instance.playerObj = player.GetComponent<GridFlowMinimapTrackedObject>();
        
    //     GlobalSetting.instance.miniMap.gameObject.SetActive(true);
        
    //     yield return 0;
    //     //npcSpawner.RebuildNPCs();
    //     AppendLoadingText("DONE!\n");
        
    //     yield return 0;
    //     //show pre dungeon config before player r ready und loading all
    //     GlobalSetting.instance.preloadingUI.SetActive(true);
        
    //     yield return null;
    // }

    /// <summary>
    ///  dungoen spawn npc in  theme , so just trace player
    /// </summary>
    /// <param name="e"></param>.
    void BuildPlayer(Players  e)
    {
        // e = FindObjectOfType<Players>().GetComponent<Players>();
        // if (e != null)
        // {
        //     //Camera trace player obj
        //     GlobalSetting.instance.mainCamera.LookAt = player.transform;
        //     //Set character Module for dungeon UI
        //     GlobalSetting.instance.battle_Character.Init(e);
        //     //init skill bar und can't manager when at camp
        //     GlobalSetting.instance.battle_skill.Init(e);

        //     GlobalSetting.instance.InitTmpInventory(e);

        // }
    }

    //Load Data To UI
    public void LoadEntityToDungeonUI(Players entity){
        DungeonStartInfo.PLAYERS = entity;

    }

    
    /// <summary>
    /// At Dungeon when player explore this rooms done, unlock adj rooms who can
    /// explore next rooms , when select the doors enter next rooms
    /// 
    /// </summary>
    /// <param name="canNext"></param>
    /// <param name="isFinal"></param>
    /// <param name="dungeonDay"></param>
    public void NextRoom(bool canNext,bool isFinal , int dungeonDay)
    {
        //clear this rooms
        //load rooms pieces

        //Check rooms types 
    }

    private void AppendLoadingText(string v)
    {
        loadingText.text += v;
        GameDebug.Log(v);
    }
    /// <summary>
    /// For ai
    /// when player can 
    /// </summary>
    private void RebuildNavigation()
    {
        //navMesh.BuildNavMesh();
    }


    /// <summary>
    /// 
    /// </summary>
    private void NotifyBuild()
    {
        throw new NotImplementedException();
    }

    private void NotifyDestroyed()
    {
        
    }
    #endregion

    public void ChangeScene(string name){
      
        
    //  GameObject d = sceneObj;

    
    //  Destroy(d);
    //  NetworkServer.Destroy(d);
        //
        GameObject scene = assetBundle.LoadAsset<GameObject>(name);
        Debug.Log("Load Scene"+scene.name);
        //
        if(scene!=null){
         GameObject s=   Instantiate(scene,transform.position,Quaternion.identity);
            sceneObj=s;
            NetworkServer.Spawn(s);
        //   s.GetComponent<GlobalSetting>().dungeonNavAgent.
            vGameController.spawnPoint =s.GetComponent<GlobalSetting>().startPosition;
            Debug.Log(vGameController.spawnPoint.position);
            //  player.transform.position =s.GetComponent<GlobalSetting>().startPosition.position;
            var p = FindObjectOfType<Players>();
            p.Warp(vGameController.spawnPoint.position);
             
       fadeCanvas.group.alpha=0;
            // vGameController.SpawnAtPoint(vGameController.spawnPoint);
        }else{
            Debug.Log("No scene");
        }

        //
    
    }

    /// <summary>
    /// 
    /// </summary>
    public void LoadSkillData()
    {
        
    }

}
