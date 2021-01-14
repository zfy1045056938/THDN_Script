using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DEIcon
{
    public DEIcon(DungeonEventType type, Sprite sprite)
    {
        this.type = type;
        this.sprite = sprite;
    }

    public DungeonEventType type;
    public Sprite sprite;
}
public class DungeonEventIcon : MonoBehaviour
{

    public DEIcon[] icons;

    public Dictionary<DungeonEventType, Sprite> dvtDic= new Dictionary<DungeonEventType, Sprite>();

    public static DungeonEventIcon instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        foreach (var ic in icons)
        {
           
                dvtDic.Add(ic.type,ic.sprite);
            
        }
        
    }

   
}
