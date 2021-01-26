using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dungeonize))]
public class DungeonizeEditor : Editor
{
   public override void OnInspectorGUI()
   {
      DrawDefaultInspector();
      //
      Dungeonize d = (Dungeonize) target;
      //
      if (GUILayout.Button("Generate"))
      {
         d.ClearOldDungeon();
         d.Generate();
      }
   }
}
