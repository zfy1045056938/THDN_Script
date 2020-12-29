using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Mirror;
using GameDataEditor;


    public partial struct Character
    {

    public int hash;

    public int cid;
    public string cName;
    public CRaces cRaces;
    public CType cType ;
    public string cModel;
    public float cHealth;
    public float cMana;
    public float cDamage;
    public float cArmor;
    //
    public float cCritPerc ;
    public float cFlashPerc;
    public float cBlockPerc;

    //
    public float cAShield;
    //PS
    public int kaLevel;
    public int LPLevel;
    public int sciLevel ;
    public int dungeoneeringLevel ;
    public int leaderLevel ;

    public int dice ;

    public int level;
    public int maxLevel;
    //enemy needs reward when defeat by player
    // public List<int> merchant_items; //merchant ,sometimes has business at rooms
    // public List<int> reward_items;   //enemy

    
    public string comboEffect;

   //
    public Character(ScriptableCharacter data)
    {
        hash = data.GetHashCode();
        cid = 0;
        cName = "";
        cRaces = CRaces.Qika;
        cType = CType.None;
        cModel = "";
        cHealth = 0f;
        cMana = 0f;
        cDamage = 0f;
        cArmor = 0f;
        cAShield = 0f;
        //
        cFlashPerc = 0f;
        cBlockPerc = 0f;
        cCritPerc = 0f;
        //
        level = 1;
        maxLevel = 30;
        kaLevel = 1;
        LPLevel = 1;
        sciLevel = 1;
        dungeoneeringLevel = 1;
        leaderLevel = 1;
        //d
        dice = 9;
        comboEffect = "";
       
        //
        // merchant_items =null;
        // reward_items =null;
           
    }

    public ScriptableCharacter data
    {
        get
        {
            if (!ScriptableCharacter.dict.ContainsKey(hash)) { return null; }
            return ScriptableCharacter.dict[hash];
        }
    }

}
public class SyncCharacter:SyncList<Character>{}

