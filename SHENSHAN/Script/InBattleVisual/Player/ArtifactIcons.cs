using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArtifactI
{
    public ArtifactType type;
    public Sprite asprite;
}
public class ArtifactIcons : MonoBehaviour
{
    public ArtifactI[] art;
    public  Dictionary<ArtifactType,Sprite> artDi = new Dictionary<ArtifactType, Sprite>();
    public static ArtifactIcons instance;

    void Awake()
    {
        foreach (var a in art)
        {
            if (!artDi.ContainsKey(a.type))
            {
                artDi.Add(a.type,a.asprite);
            }
        }
    }
}
