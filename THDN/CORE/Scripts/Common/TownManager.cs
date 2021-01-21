
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
//
using EmeraldAI;
using  Michsky.LSS;

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

   
    


    [Header("states")]
    //public GameManagers battleManager;  //setup board
    //public GameManager globalSetting;   //common 
    public SoundManager sound;  //sound
    public bool atDungeon = false;
    public bool haveEnemies = false;

    public bool isRndProp=false;


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
    public ShrineManager shrineManager;
    //HUD
    public GameObject hudOBj;
    
    public ItemDatabase itemDatabase;
    
    public GameObject currentScene;

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
        // itemDatabase = FindObjectOfType<ItemDatabase>().GetComponent<ItemDatabase>();
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
        // Debug.Log("Load Game Data");
        // LoadSkillData();
      
        // ItemDatabase.instance.
        //Load Map
        Debug.Log("Load Town");
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
     
        string uri = Application.dataPath + "/StreamingAssets/"+fn;

        Debug.Log(uri);
    
        //Load Init
        AssetBundleCreateRequest re = AssetBundle.LoadFromFileAsync(uri);

       
        
        assetBundle = re.assetBundle;
      
        yield return assetBundle;

     
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
 
    // Load scene object
    GameObject scene =assetBundle.LoadAsset<GameObject>(player.lastSceneName);
    sceneObj = scene;
    if(scene!=null){
      GameObject s =  Instantiate(scene);
        //
        // scene.GetComponent<GlobalSetting>().surface.BuildNavMesh();
        // scene.GetComponent<NavMeshSurface>().BuildNavMesh();
        NetworkServer.Spawn(s);
        sceneList.Add(s);
    }


    //
    Debug.Log("Check Player Pos");
    // Generate Player
    player.transform.position = sceneObj.GetComponent<GlobalSetting>().startPosition.position;
    // player.transform.SetParent(sceneObj.GetComponent<GlobalSetting>().startPosition);
    //
    if(isRndProp==true){
     GenerateProp();
    }

    //Generate data by datalist rnd
            InitEnemies();
        
    // //Check Items und validate
    // ItemDatabase.instance.Init();
    // THDNGameDatabase.instance.Init();
    InitPlayer();
    
   
   
    }

#region  Enemy Module
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
                        var Ehealth = enemiesList[i].GetComponent<EmeraldAISystem>().CurrentHealth ;
                        // var Earmor =enemiesList[i].GetComponent<EmeraldAISystem>().Armor;
                        //Got Loot item by vitem-> vfrom

                        //network identity
                        var health = Random.Range(itemDatabase.cList[j].cHealth/2, itemDatabase.cList[j].cHealth );
                        var shield = Random.Range(itemDatabase.cList[j].cHealth/2, itemDatabase.cList[j].cArmor );
                        var damage = Random.Range(itemDatabase.cList[j].cHealth/2, itemDatabase.cList[j].cDamage);
                     
                        Debug.Log("Enemy->"+enemiesList[i].name+"Health"+health.ToString()+"shield"+shield.ToString()+"damage"+damage.ToString());
                        //invector ai
                        var iai = enemiesList[i].gameObject.GetComponent<vMeleeManager>();
                        if(iai){
                            iai.defaultDamage = new vDamage(Mathf.FloorToInt(damage));
                            
                        }
                        //emerald ai
                        var eAI = enemiesList[i].gameObject.GetComponent<EmeraldAISystem>();
                        if(eAI){
                            eAI.CurrentHealth = Mathf.FloorToInt(health);
                           
                        }
                        //load items by itemfrom
                        List<vItem> items = itemDatabase.itemList.Where(citem => citem.itemFrom == enemiesList[i].characterFrom).ToList();
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
                                    
                                    enemiesList[i].itemList.Add(items[ditem]);

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
 void InitEnemies(){
     var sObj = sceneObj.GetComponent<GlobalSetting>();

     if(sceneObj.GetComponent<GlobalSetting>().enemyList.Count>0){
            //
        
        for(int i=0;i<sObj.enemyList.Count;i++){
            //
            var cEnemy = itemDatabase.cList.Find(ci =>ci.cName == sObj.enemyList[i].name);
            //enemy data
            if(cEnemy!=null){
                //Add random value to target
                //Damage & health & armor
                var enemy = sObj.enemyList[i];
                enemy.healthMax =Mathf.FloorToInt( Random.Range(cEnemy.cHealth,cEnemy.cHealth*0.2f + cEnemy.cHealth));
                  enemy.damage =Mathf.FloorToInt( Random.Range(cEnemy.cDamage,cEnemy.cDamage*0.2f + cEnemy.cDamage));
                  enemy.armor =Mathf.FloorToInt( Random.Range(cEnemy.cArmor,cEnemy.cArmor*0.2f + cEnemy.cArmor));
                
            
            //item data
            var itemNumber = Random.Range(1,3);
            var itemCounter=Random.Range(1,3);
            var CMItemList = itemDatabase.itemList.FindAll(cm => cm.type == vItemType.CraftingMaterials).ToList();
            if(CMItemList.Count>0){
                while(itemNumber>0){
                    //detail Items
                    var currentItem = Random.Range(0,CMItemList.Count);
                    //item by index
                    // ItemReference ci =new ItemReference{
                    //     name = CMItemList[currentItem].name,
                    //     amount = itemCounter,
                    // };
                    //Add to monster list 

                    sObj.enemyList[i].itemList.Add(CMItemList[currentItem]);
                    //
                    itemNumber--;
                }
            }else{
                Debug.Log("Couldn't found items at db");
            }
            }else{
                Debug.Log("no target");
            }
        } 
     }
 }

 #endregion
    /*
    *
    * public List<DungeonChest> chest;
    public List<Entity> enemyList;
    public List<DungeonShrine> shrineList;
    */

    //generate by rnd und difficult(10%~30%)
   public void GenerateProp(){
     var cd = sceneObj.GetComponent<GlobalSetting>();
     var itemManager= FindObjectOfType<vItemManager>();
     if(cd && itemManager){
         //
        if(cd.enemyList.Count>0){
            for(int i=0;i<cd.enemyList.Count;i++){
                if(cd.enemyList[i]!=null){

                    //reload the item from dungeon.dataList
                      //check item counter
                    int SlotItem =Random.Range(1,2);
                    for(int j=0;j<SlotItem;j++){
                        //Check detail item 
                        int itemIndex= Random.Range(0,dungeonItemList.Count);
                        var detailItem = itemManager.GetItem(dungeonItemList[itemIndex].id);
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
            for(int j=0;j<cd.chestList.Count;j++){
                if(cd.chestList[j]!=null){
                    //got all items


                    //check item counter
                    int SlotItem =Random.Range(1,2);
                    for(int i=0;i<SlotItem;i++){
                        //Check detail item 
                        int itemIndex= Random.Range(0,dungeonItemList.Count);
                        vItem detailItem = itemManager.GetItem(dungeonItemList[itemIndex].id);
                        //gernerate item by id to vitemmanager
                        if(detailItem!=null){
                               //generate amount for itemreference
                        float itemAmount= Random.Range(1,3);
                        
                        //
                        ItemReference ir = new ItemReference{
                            id = detailItem.id,
                            name=detailItem.name,
                            amount = SlotItem,
                        };
                        cd.chestList[i].GetComponent<vItemCollection>().items.Add(ir);
                        
                       
                       
                     
                        }
                    }
                }
            }
            //
            for(int l=0;l<cd.shrineList.Count;l++){
                if(cd.Chest[l]!=null){
                    //got rnd event
                    int index = Random.Range(0,ItemDatabase.instance.deList.Count);
                    DungeonEvent cs = ItemDatabase.instance.GotDE(index);
                    if(cs!=null){
                        //set to cs.s
                        var de = cs;
                        var shrine = cd.shrineList[l].GetComponent<DungeonShrine>();
                        cd.shrineList[l].GetComponent<DungeonShrine>().de=cs;
                        shrine.amount = Random.Range(cs.Amount/2,cs.Amount);
                        shrine.effectTime = de.effectTime;
                        shrine.buffType=de.buffType;
                        //Bind UI
                        shrine.nameText.text= de.DeName.ToString();
                        shrine.typeText.text= de.buffType.ToString();
                        shrine.detailText.text= de.deDetail.ToString();
                        
                    }
                }
            }
        

     }else{
         Debug.Log("Couldn't found the dungeon");
         GameDebug.Log("ERROR::Couldn't found the dungeon");
         
     }

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
//Entity.Health -> TCM data 
//
    //     Debug.Log("Add Invector Component, Loaded Object stats");
    //    var ptpc=pObj.GetComponent<vThirdPersonController>();
       
    //    //currentHealth = HealthMax when level add maxHealth
    //    ptpc.MaxHealth = player.healthMax;

    //     ptpc.maxStamina = player.Stamina;
    //   ptpc.MaxMana=player.manaMax;
    // //    //
    //   ptpc.healthRecovery = player.healthRate;
    // //  .
    // //   ptpc.UpdateMotor();
    // Debug.Log(string.Format("LOAD PLAYER DONE PLAYER STATS IST===>hp->{0}\nstmina->{1}\n",ptpc.currentHealth.ToString(),ptpc.maxStamina.ToString()));

        //Damage Module 
        

      Debug.Log("Melee Combat");
   var pcombat=pObj.GetComponent<vMeleeManager>();
    pcombat.defaultDamage = new vDamage(Mathf.FloorToInt(player.damage)); 
    
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
       
       Debug.Log("Init GC");
         vGameController.playerPrefab = pObj;
        vGameController.spawnPoint = sceneObj.GetComponent<GlobalSetting>().startPosition;
        // pObj.transform.parent = sceneObj.GetComponent<GlobalSetting>().startPosition;
        // pObj.transform.parent = vGameController.spawnPoint; SM_Env_Basement_Ceiling_
        //
            Debug.Log("Add Invector Component, Loaded Object stats");
       var ptpc=pObj.GetComponent<vThirdPersonController>();
       
    //    currentHealth = HealthMax //when level add maxHealth
       ptpc.MaxHealth = player.healthMax;

        ptpc.maxStamina = player.Stamina;
      ptpc.MaxMana=player.manaMax;
    //    //
      ptpc.healthRecovery = player.healthRate;
    //  .
    //   ptpc.UpdateMotor();
    Debug.Log(string.Format("LOAD PLAYER DONE PLAYER STATS IST===>hp->{0}\nstmina->{1}\n",ptpc.currentHealth.ToString(),ptpc.maxStamina.ToString()));

        

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

  

    #endregion



    public void ChangeScene(string name){
      
        player.lastSceneName = name.ToString();
 PlayerPrefs.SetString("_lastsavename",player.lastSceneName);
        Debug.Log("Last Save Scene"+player.lastSceneName.ToString());
    //  GameObject d = sceneObj;

   

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
             player.transform.position =s.GetComponent<GlobalSetting>().startPosition.position;
            var p = FindObjectOfType<Players>();
            p.Warp(vGameController.spawnPoint.position);
            p.gameObject.transform.SetParent ( vGameController.spawnPoint);
             
    //    fadeCanvas.group.alpha=0;
            // vGameController.SpawnAtPoint(vGameController.spawnPoint);
        }else{
            Debug.Log("No scene");
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public void LoadSkillData()
    {
        
    }

}
