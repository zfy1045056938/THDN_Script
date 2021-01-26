using UnityEngine;
using UnityEditor;


public  static class ScriptObjectUtility2ToCharacterAsset
{
    public static void CreateAsset<T>() where T :ScriptableObject{
        var asset = ScriptableObject.CreateInstance<T>();
        ProjectWindowUtil.CreateAsset(asset,"new" + typeof(T).Name+".asset");
    }
}