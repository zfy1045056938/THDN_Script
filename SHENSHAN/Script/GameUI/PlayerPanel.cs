using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class PlayerPanel : MonoBehaviour
{
    public GameObject ScreenContent;

    private int id;
    public string nameText;
    public int levelText;
    public Image playerImage;
    public int atkText;
    public int defText;

    public Button ShowPanel;


    [Header("Prefab")]
    public GameObject QuestPanel;
    public GameObject DetailPanel;
    public GameObject CharacterDataPanel;


    [Header("Quest Info")]
    public Text questName;
    public Text questDeatail;
    public Button acceptQuest;
    public Button abandonQuest;
    public Image rewardItmem;
    public float remainTime = Time.time;

    //private PlayerControllers controller;
    private Players players;
    private QuestManager quests;

    private bool isShow =false;
    public List<QuestManager> list_quest;
    public List<Players> list_players;
    

    private void Awake()
    {
        players = GameObject.FindGameObjectWithTag("Players").GetComponent<Players>();
        quests = GameObject.FindGameObjectWithTag("Quest").GetComponent<QuestManager>();

    }

   

    /// <summary>
    /// Start this instance.
    /// </summary>
    public void Start()
    {
        OpenOrCloseScreen();
        ShowQuestList();
        if(players==null){
            //players.ID = id;
            nameText=players.name;
           
           
            //controller.GetPlayer(id, name, levelText);
        }
    }

    /// <summary>
    /// Shows the quest list.
    /// </summary>
    private void ShowQuestList()
    {
        if (QuestPanel == null && ScreenContent==null && !isShow)
        {
            return;
        }

        //
        for (int i = 0; i < list_quest.Count; i++)
        {
            QuestManager index = list_quest[i];
            if (list_quest==null)
            {
                return;
            }
            questName.text = list_quest[0].name;
            questDeatail.text = list_quest[0].QuestDescription;
            //
            if (RemoveThisQuest())
            {
                list_quest.Remove(index);
            }else{
                Debug.Log("Haven't found Object");
            }
        }
    }

    /// <summary>
    /// Removes the this quest.
    /// </summary>
    /// <returns><c>true</c>, if this quest was removed, <c>false</c> otherwise.</returns>
    private bool RemoveThisQuest()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Opens the or close screen.
    /// </summary>
    public bool OpenOrCloseScreen()
    {
        if (ScreenContent != null)
        {

            ScreenContent.gameObject.SetActive(true);
            QuestPanel.gameObject.SetActive(true);
            CharacterDataPanel.gameObject.SetActive(true);
            DetailPanel.gameObject.SetActive(true);
            return isShow;
        }
        else
        {

            Debug.Log("Invaild Object");

            return isShow;
        }



    }


}
