using UnityEngine;
using System.Collections;
using ChuMeng;
public class ItemData : MonoBehaviour
{
  
    public EquipConfigData equipData;
    //manager player data
    public CharAttribute info;

    public PropsConfigData propsData;

    public int hp
    {
        get
        {
            return propsData.level;
        }
    }

    private int _itemID;
    private string _itemName;

    public string ItemName
    {
        get
        {
            return equipData.name;
        }

    }

}
