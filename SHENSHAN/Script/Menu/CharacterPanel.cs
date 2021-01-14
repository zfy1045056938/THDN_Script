using UnityEngine;
using System.Collections;

using UnityEngine.UI;
/*
 * 
 *  角色信息面板,包含以下内容
 *  1.角色数据
 *  2.任务面板
 *  3.
 * 
 */
public class CharacterPanel : MonoBehaviour
{

    public Players p_info;
    //
    public GameObject screenContent;
    public GameObject characterInfoPrefab;
    public GameObject questPanelPrefab;

    [Header("GeneralInfo")]
    private string _playerName;
    private int _level;

    public string PlayerName
    {
        get
        {
            return _playerName;
        }

        set
        {
            _playerName = p_info.name;
        }
    }

    public int Level
    {
        get
        {
            return _level;
        }

        set
        {
            _level = value;
        }
    }
}
