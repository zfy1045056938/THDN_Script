using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(MonsterAsset)),CanEditMultipleObjects]
public class MonsterAssetEditor : Editor
{

    public void OnEnable()
    {

    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

    }

}
