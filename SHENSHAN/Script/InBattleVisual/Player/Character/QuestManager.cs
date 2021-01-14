using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// 任务管理
/// </summary>
public class QuestManager:MonoBehaviour
{
    private int questID;
    private string questName;
    private int requiredLevel;
    private string questDescription;
    private string fromNPC;


    /// <summary>
    /// Initializes a new instance of the <see cref="T:QuestManager"/> class.
    /// </summary>
    /// <param name="questID">Quest identifier.</param>
    /// <param name="questName">Quest name.</param>
    /// <param name="requiredLevel">Required level.</param>
    /// <param name="questDescription">Quest description.</param>
    /// <param name="fromNPC">From npc.</param>
    public QuestManager(int questID, string questName, int requiredLevel, string questDescription, string fromNPC)
    {
        QuestID = questID;
        QuestName = questName;
        RequiredLevel = requiredLevel;
        QuestDescription = questDescription;
        FromNPC = fromNPC;
    }

    public int QuestID
    {
        get
        {
            return questID;
        }

        set
        {
            questID = value;
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

    public int RequiredLevel
    {
        get
        {
            return requiredLevel;
        }

        set
        {
            requiredLevel = value;
        }
    }

    public string QuestDescription
    {
        get
        {
            return questDescription;
        }

        set
        {
            questDescription = value;
        }
    }

    public string FromNPC
    {
        get
        {
            return fromNPC;
        }

        set
        {
            fromNPC = value;
        }
    }
}