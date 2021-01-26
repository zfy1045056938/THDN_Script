using UnityEngine;
using UnityEditor;
public static class ScriptObjectUtility2
{
    public static void CreateAsset<T>() where T: ScriptableObject
    {
        
        var asset = ScriptableObject.CreateInstance<T>();
        ProjectWindowUtil.CreateAsset(asset, "Resources/New" + typeof(T).Name + ".asset");
        Debug.Log("Create Success");
    }
}