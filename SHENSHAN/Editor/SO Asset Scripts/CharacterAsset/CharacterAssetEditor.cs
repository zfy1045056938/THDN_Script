using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CharacterAsset)),CanEditMultipleObjects]
public class CharacterAssetEditor : Editor {


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
