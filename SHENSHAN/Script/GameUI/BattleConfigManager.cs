using System.Collections;
using System.Collections.Generic;
using System.Linq;

using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//add progressing bar
using TMPro;


public enum GameMode
{
    Classical,
    Squard,
    Wheel,
}

public enum GameDifficult
{
    Normal,
    Hard,
    Evil,
}


/// <summary>
/// 21-1-28 added difficult mode
/// </summary>
public class BattleConfigManager : MonoBehaviour
{
    public static BattleConfigManager instance;
    public UIPanel panel;

    [Header("Game Stats")] public GameMode gameMode = GameMode.Classical;
    public GameDifficult gameDifficult = GameDifficult.Normal;

    [Header("PlayerStats")] private PlayerData players;
    private PlayerPortraitVisual playerVisual;
    private Players playerInBattle;
    public List<EquipmentSlot> equipmentList;
    public List<EquipmentSlot> battlePackage;
    public InventorySystem inventory;
    public Text atkText;
    public Text atkDurText;
    public Text armorText;
    public Text armorDurText;
    public Text cardText;
    public Text cardNumberText;
    public Image completeImage;

    [Header("EnemyStats")] public Entity entity;
    private EnemyPortraitVisual enemyVisual;
    public Text detailText;
    public Image entityAvatar;
    public Text entityHealth;
    public Text entitySkillText;
    public TextMeshProUGUI atkAmount;
    public TextMeshProUGUI armorAmount;

    
    public GameObject[] stars;
    public List<QuestPieces> quests;
    public GameObject questPs;
    public Transform qp;

    public Button startBtn;
    
    private bool firstOpen = true;

    void Start()
    {

        
            if (instance = null)
            {
                instance = this;
            }

            players =FindObjectOfType<PlayerData>().GetComponent<PlayerData>();
        Debug.Log("Got Players"+players.name);
            
            inventory = GameObject.FindWithTag("Inventory").GetComponent<InventorySystem>();
            entity = GetComponent<Entity>();

          PersistentDataManager.Record();

          
        

    }
    
    void Update(){
        
        
        
        if(panel.isActiveAndEnabled)
        {
            LoadPlayerInfo();
        //
       
            
        for(int i=0;i<equipmentList.Count;i++){
            foreach (EquipmentSlot slot in inventory.equipmentSlots)
            {
                if (equipmentList[i].equipmentSlotType == slot.equipmentSlotType)
                {
                    if (slot.item.itemName != "")
                    {
                        equipmentList[i].itemIcon.gameObject.SetActive(true);
                        equipmentList[i].itemIcon.sprite = slot.itemIcon.sprite;
                        equipmentList[i].item = slot.item;

                        if (slot.item.equipmentSlotype == EquipmentSlotType.weapon)
                        {
                            BattleStartInfo.Weapon = slot.item;
                        }else if (slot.item.equipmentSlotype == EquipmentSlotType.armor)
                        {
                            BattleStartInfo.Armor = slot.item;
                        }else if (slot.item.equipmentSlotype == EquipmentSlotType.ring)
                        {
                            BattleStartInfo.Ring = slot.item;
                        }
                    }
                    else
                    {
                        equipmentList[i].itemIcon.gameObject.SetActive(false);
                        equipmentList[i].itemIcon.sprite = null;
                        equipmentList[i].item.itemName = "";
                        
                    }
                }
            }
        }
        }
        

        // if(Input.GetMouseButtonDown(0)){
        //     if(!EventSystem.current.IsPointerOverGameObject()){
        //             Debug.Log("Not Ray Cast");
        //     }
        // }
     
    }


    /// <summary>
    /// 
    /// </summary>
    public void LoadConfig()
    {
        panel.Open();
          //Load Player
          LoadPlayerInfo();
        
                    //Load Enemy
        
                    LoadEnemyInfo();
    }

    /// <summary>
    /// 
    /// </summary>
    void LoadPlayerInfo()
    {
        if (players != null&& BattleStartInfo.SelectDeck!=null)
        {
            BattleStartInfo.player = players;
            
            Debug.Log("Load"+BattleStartInfo.player.name);
            atkText.text = players.atk.ToString();
                atkDurText.text = players.atkCount.ToString();
                armorText.text = players.ArmorDef.ToString();
                armorDurText.text = players.ArmorDur.ToString();


            //
            atkAmount.text = BattleStartInfo.SelectDeck.atkNum.ToString();
            armorAmount.text = BattleStartInfo.SelectDeck.defNum.ToString();

            
        }
        else
        {
            Debug.Log("NULL PLAYER");
        }
        
       
    }

    void LoadEnemyInfo()
    {
        entityAvatar.sprite = BattleStartInfo.SelectEnemyDeck.enemyAsset.Head;
        detailText.text = BattleStartInfo.SelectEnemyDeck.enemyAsset.detail.ToString();
        entityHealth.text = BattleStartInfo.SelectEnemyDeck.enemyAsset.Health.ToString();

        string entryN = DialogueLua.GetVariable("enemiesKilled").asString;
        Debug.Log(entryN);
//
//        QuestInfos[] infos = BattleStartInfo.SelectEnemyDeck.enemyAsset.questList.ToArray();
//        quests = new List<QuestPieces>();

//        if (firstOpen)
//        {
//            //Load Quest from enemy 
//            for (int j = 0; j < BattleStartInfo.SelectEnemyDeck.enemyAsset.questList.Length; j++)
//            {
//
//                        QuestPieces ps = Instantiate(questPs, transform.position, Quaternion.identity)
//                            .GetComponent<QuestPieces>();
//                        ps.transform.parent = qp.transform;
//                        quests.Add(ps);
//                        ps.titleText.text = BattleStartInfo.SelectEnemyDeck.enemyAsset.questList[j].questName;
//                        ps.detailText.text = BattleStartInfo.SelectEnemyDeck.enemyAsset.questList[j].questDetail;
//                        //Get State from ps if has else create
//                        if (PlayerPrefs.HasKey("QuestState" + "_" +
//                                               BattleStartInfo.SelectEnemyDeck.enemyAsset.questList[j].questName))
//                        {
//                            Debug.Log("has state");
//                            string lState = PlayerPrefs.GetString(
//                                "QuestState" + "_" + BattleStartInfo.SelectEnemyDeck.enemyAsset.questList[j]
//                                    .questName);
//                            if (lState == "Active")
//                            {
//                                 
//                                BattleStartInfo.SelectEnemyDeck.enemyAsset.questList[j].questState =
//                                    QuestState.Active;
//                            }
//                            else if (lState == "Success")
//                            {
//                                BattleStartInfo.SelectEnemyDeck.enemyAsset.questList[j].questState =
//                                    QuestState.Success;
//                            }
//                        }
//                        else
//                        {
//                            ps.questState = BattleStartInfo.SelectEnemyDeck.enemyAsset.questList[j].questState;
//                        }
//                        
//                        ps.questState = BattleStartInfo.SelectEnemyDeck.enemyAsset.questList[j].questState;
//                        
//                        if (ps.questState == QuestState.Unassigned)
//                        {
//                            //Unassign ,show button and add quest to the dialogue database
//                            ps.accpetBtn.gameObject.SetActive(true);
//                            ps.cancelBtn.gameObject.SetActive(true);
//
//
//                            //Add To db
//                            QuestLog.AddQuest(ps.titleText.text, ps.detailText.text, ps.questState);
//                            if (BattleStartInfo.SelectEnemyDeck.enemyAsset.questList[j].entryNumber > 0)
//                            {
//
//                                //has entry then add
//                                QuestLog.AddQuestEntry(ps.titleText.text,
//                                    BattleStartInfo.SelectEnemyDeck.enemyAsset.questList[j].entryDetail);
//                                ps.EntryText.gameObject.SetActive(true);
//                                ps.EntryText.text = QuestLog.GetQuestEntryCount(ps.titleText.text) + "/" +
//                                                    BattleStartInfo.SelectEnemyDeck.enemyAsset.questList[j].entryNumber;
//
//
//                            }
//                            else
//                            {
//                                ps.EntryText.gameObject.SetActive(false);
//                            }
//                        }
//                        else if (ps.questState == QuestState.Active)
//                        {
//                            ps.accpetBtn.gameObject.SetActive(false);
//                            ps.ingBtn.gameObject.SetActive(true);
//                            ps.EntryText.gameObject.SetActive(true);
//                            ps.EntryText.text = entryN + "/" +
//                                                BattleStartInfo.SelectEnemyDeck.enemyAsset.questList[j].entryNumber;
//
//
//                        }
//                        else if (ps.questState == QuestState.Success)
//                        {
//                            ps.accpetBtn.gameObject.SetActive(false);
//                            ps.cancelBtn.gameObject.SetActive(false);
//                            ps.complete.gameObject.SetActive(true);
//                            ps.EntryText.gameObject.SetActive(false);
//                            //
//
//                        }
//                        else
//                        {
//
//                        }
//
//                        //Check number is higher than required 
//                        if (QuestLog.GetQuestEntryCount(ps.titleText.text) >
//                            BattleStartInfo.SelectEnemyDeck.enemyAsset.questList[j].entryNumber)
//                        {
//                            QuestLog.SetQuestState(ps.titleText.text, QuestState.Success);
//                            BattleStartInfo.SelectEnemyDeck.enemyAsset.questList[j].questState = QuestState.Success;
//                            PlayerPrefs.SetString("QuestState" + "_" +
//                                                  BattleStartInfo.SelectEnemyDeck.enemyAsset.questList[j].questName,
//                                "Success");
//                            //Get Reward
//                            players.money += BattleStartInfo.SelectEnemyDeck.enemyAsset.moneyReward;
//                            players.dust += BattleStartInfo.SelectEnemyDeck.enemyAsset.dustReward;
//                            players.experience += BattleStartInfo.SelectEnemyDeck.enemyAsset.expReward;
//
//
//                        }
//
//            }
//
//            firstOpen = false;
//        }
//        else
//        {
//           
//        }


    }


//        QuestLog.AddQuest("test","test",QuestState.Unassigned);
//
//        quests = new List<QuestPieces>();
//      for (int i = 0; i < quests.Count; i++)
//      {
//          if (quests[i].titleText.text == "")
//          {
//              
//              quests[i].titleText.text = QuestLog.GetQuestTitle("test");
//              quests[i].detailText.text = QuestLog.GetQuestDescription("test",QuestState.Unassigned);
//          }
//      }

    public void ClosePanel()
    {
        panel.Close();
    }



}
