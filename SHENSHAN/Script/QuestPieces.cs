using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers;
using PixelCrushers.DialogueSystem;

[System.Serializable]
public class QuestInfos
{
    public QuestInfos(string questName, string questDetail, int entryNumber, QuestState questState, string questLoc, QuestType questType, string entryDetail, int money, int qexp, int dust)
    {
        this.questName = questName;
        this.questDetail = questDetail;
        this.entryNumber = entryNumber;
        this.questState = questState;
        this.questLoc = questLoc;
        this.questType = questType;
        this.entryDetail = entryDetail;
        this.money = money;
        this.qexp = qexp;
        this.dust = dust;
    }

    public QuestInfos(string questName, string questDetail, int entryNumber, QuestState questState, string entryDetail, int money, int qexp, int dust)
    {
        this.questName = questName;
        this.questDetail = questDetail;
        this.entryNumber = entryNumber;
        this.questState = questState;
        this.entryDetail = entryDetail;
        this.money = money;
        this.qexp = qexp;
        this.dust = dust;
    }

    public QuestInfos(string questName, string questDetail, QuestState questState,int entryNumber,string entryDetail)
    {
        this.questName = questName;
        this.questDetail = questDetail;
        this.questState = questState;
        this.entryNumber = entryNumber;
        this.entryDetail = entryDetail;
    }

    public string questName;
    public string questDetail;
    public int  entryNumber;
    public QuestState questState;
    public string questLoc;
    public QuestType questType;
    public string entryDetail;
    public int money;
    public int qexp;
    public int dust;

}
public class QuestPieces : MonoBehaviour
{

    public UIPanel panel;
    public Text titleText;
    public Text detailText;
    public Text EntryText;
    public bool hasEntry;
    public Button accpetBtn;
    public Button cancelBtn;
    public Button ingBtn;
    public Image complete;
    public QuestState questState = QuestState.Unassigned;
    public QuestType questType = QuestType.Side;

    void Update()
    {
        accpetBtn.onClick.AddListener(() =>
        {

            foreach (QuestInfos q in BattleStartInfo.SelectEnemyDeck.enemyAsset.questList)
            {
                if (q.questName == this.titleText.text)
                {
                    QuestLog.StartQuest(titleText.text);
                    QuestLog.SetQuestEntry(titleText.text,q.entryNumber,q.entryDetail);
                    q.questState = QuestState.Active;
                    this.questType = QuestType.Side;
                    EntryText.text = QuestLog.GetQuestEntryCount(titleText.text).ToString() + "/3";

                    Debug.Log("Accept Quest Show Info");
                    Debug.Log("state" + q.questState.ToString());
                    Debug.Log("entry" + QuestLog.GetQuestEntryCount(titleText.text));

                    Debug.Log(QuestLog.GetQuestEntry(titleText.text, 1));

                    PlayerPrefs.SetString("QuestState" + "_" + titleText.text, q.questState.ToString());
                    if (PlayerPrefs.HasKey("QuestState" + "_" + titleText.text))
                    {
                        string queststates = PlayerPrefs.GetString("QuestState" + "_" + titleText.text);
                    }
                    Debug.Log(questState);
                    PlayerPrefs.SetInt("QuestEntry" + "_" + titleText.text,
                        QuestLog.GetQuestEntryCount(titleText.text));
                    PersistentDataManager.Apply();
                    accpetBtn.gameObject.SetActive(false);
                    ingBtn.gameObject.SetActive(true);
                }
            }
        });
        
        cancelBtn.onClick.AddListener(() =>
        {
            QuestLog.AbandonQuest(titleText.text);
            
            ingBtn.gameObject.SetActive(false);
            accpetBtn.gameObject.SetActive(true);
        });
        
        
    }
   
}
