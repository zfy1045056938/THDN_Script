using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class QuestModel 
{
    private Guid questId;
    private string questName;
    private string questDetail;

    public QuestModel(){}

    public QuestModel(Guid questId, string questName, string questDetail)
    {
        this.questId = questId;
        this.questName = questName;
        this.questDetail = questDetail;
    }

    public Guid QuestId
    {
        get
        {
            return questId;
        }

        set
        {
            questId = value;
        }
    }

    public string QuestName
    {
        get
        {
            return questName;
        }

        set
        {
            questName = value;
        }
    }

    public string QuestDetail
    {
        get
        {
            return questDetail;
        }

        set
        {
            questDetail = value;
        }
    }
}
