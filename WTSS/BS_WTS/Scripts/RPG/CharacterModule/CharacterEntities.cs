using System;
using Unity.Entities;
using GameDataEditor;
using Unity.Collections;
using Mirror;


/// <summary>
/// manager entities  Character Data
/// not object
/// </summary>
///
[Serializable]
public struct CharacterEntites : IComponentData
{
    public int cid;
    public string cName;
    [NonSerialized]
    public EType cType;
    [NonSerialized]
    public string cModel;
    //data
    public float cHealth;
    public float cDamage;
    public float cHeavy;
    public float cArmor;
    
  
    public void Serialize(ref SerializeContext context, ref NetworkWriter writer)
    {
       
    }

    public void Deserialize(ref SerializeContext context, ref NetworkReader reader)
    {
      
    }

}