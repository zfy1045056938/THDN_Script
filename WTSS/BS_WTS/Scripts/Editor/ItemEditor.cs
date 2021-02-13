using  UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemClass))]
public class ItemEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
    }
}