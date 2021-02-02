using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class RatityColorData{
    public CardRatityOption options;
    public Sprite color;
}



public class RatityColors:MonoBehaviour{
    public RatityColorData[] data;

    public Dictionary<CardRatityOption, Sprite> colorsDictionary = new Dictionary<CardRatityOption, Sprite>();

    public static RatityColors instance;

    private void Awake()
    {
        if (instance != null && instance != null)
        
            Destroy(instance.gameObject);

        
        instance = this;
        DontDestroyOnLoad(this);

        foreach (RatityColorData d in data)
        {
            colorsDictionary.Add(d.options, d.color);

        }
    }
}