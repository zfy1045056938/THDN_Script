using Mirror;
using UnityEngine;
using SQLite;
using GameDataEditor;


#region Database

class Entity
{
    [PrimaryKey] public string character { get; set; }
    [Indexed] public int Eid { get; set; }
    public string EName { get; set; }
    public EType EntityType { get; set; }
    public string EModel { get; set; }
    public float EHealth { get; set; }
    public float EDamage { get; set; }
    public float EHeavy { get; set; }
    public float EArmor { get; set; }
    
}

class MapScene
{
    [PrimaryKey] public string character
    { get; set; }
    [Indexed] public int MID { get; set; }
    public string MType { get; set; }
    public string MScene { get; set; }
    public int MTimes { get; set; }
}

class Items
{
    [PrimaryKey] public string character { get; set; } 
    [Indexed] public int ItemID { get; set; }
    public string ItemName { get; set; }
   
    public string ItemObj { get; set; }
    public int ItemWidth { get; set; }
    public int ItemHeight { get; set; }

    public int ItemStacksize { get; set; }
    
    
}

class ESkill
{
    [PrimaryKey] public string charcter { get; set; }
    [Indexed] public int  SID { get; set; }
    public string SName { get; set; }
    public int SLevel { get; set; }

    public float SExp { get; set; } // exp>100 slevel++
    public float SAmount { get; set; }
    
}

class Buffs
{
    [PrimaryKey] public string character { get; set; }
    [Indexed] public int BID { get; set; }
    public string BuffName { get; set; }
    public float BAmount { get; set; }
    public int BTimes { get; set; }
}
#endregion


///
public class Database : MonoBehaviour {

    
}