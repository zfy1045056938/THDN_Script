using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Mirror;
using GameDataEditor;




/// <summary>
/// Character obj when player select to party(<limit), with indie und place at dungeon with random rooms
/// hows get heros at dungeon ?  with dungeon events or dungeon goals(quest bind to dialogue as string)
/// </summary>

[CreateAssetMenu(menuName = "THDNITEM/Characters ", order = 999)]
public partial class ScriptableCharacter:ScriptableObjectNonAlloc
    {
    public int hash;

    //Common
    public int cid;
    public string cName;
    public CRaces cRaces = CRaces.Qika;
    public CType cType = CType.None;
    public string cModel;
    //
    public float cHealth;
    public float cMana;
    public float cStamina;


    public float cDamage;
    public float cArmor;
    //
    public float cCritPerc;
    public float cFlashPerc;
    public float cBlockPerc;

    //
    public float cAShield;
    //PS
    public int level = 1;
    public int maxLevel = 30;
    public int kaLevel;
    public int LPLevel;
    public int sciLevel;
    public int dungeoneeringLevel;
    public int leaderLevel;
    public Sprite icon;
    public int dice;
    //enemy needs reward when defeat by player
    public List<string> merchant_items; //merchant ,sometimes has business at rooms
    public List<string> reward_items;   //enemy


    public string covName; //dialogue
    public string cAvatar;

    public string comboEffect;  //show at battlefield

    //
    public float cSprintStamina;
    public float cWalkSpeed;
    public float cRollStamina;
    public float cRollSpeed;
    public float cRollDistance;
    public float cDefStamina;
    public float cJumpStamina;
    public float cLadderStamina;
    public float cStaminaRecover;

    /// <summary>
    /// 
    /// </summary>
    private static Dictionary<int, ScriptableCharacter> cache;
    public static Dictionary<int, ScriptableCharacter> dict
    {
        get
        {
            if (cache == null)
            {
                //
                ScriptableCharacter[] items = Resources.LoadAll<ScriptableCharacter>("Characters/");
                //
                if (items != null)
                {
                    //create instance for item
                    List<string> dup = items.ToList().FindDup(item => item.name);

                    //to dic
                    if (dup.Count == 0)
                    {
                        cache = items.ToDictionary(item => item.name.GetStableHashCode(), item => item);
                    }

                }
            }
            return cache;
        }
    }




}

