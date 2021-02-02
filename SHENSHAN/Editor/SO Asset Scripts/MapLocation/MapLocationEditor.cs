using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapLocation)),CanEditMultipleObjects]
public class MapLocationEditor : Editor
{
    public SerializedProperty
        Names_prop,
        Detail_prop,
        Npc_Prop,
        IsLock_prop,
        townType_prop,
        bg_Prop,
        priceflow_prop,
        hasCrash_prop,
        isDungeon_prop,
        minLevel_prop,
        maxLevel_prop,
        hasEvent_prop,
        events_prop,
        dunTheme_prop,
        enemyList_prop,
        hasBoss_prop,
        bossAsset_prop,
        bossList_prop,
        itemList_prop,
        dungeonType_prop,
        maxEnemyKill_prop;

        


    private void OnEnable()
    {
        Names_prop = serializedObject.FindProperty("locationName");
        Detail_prop = serializedObject.FindProperty("locationDetail");
        Npc_Prop = serializedObject.FindProperty("npcList");
        townType_prop = serializedObject.FindProperty("townType");
        IsLock_prop = serializedObject.FindProperty("isLock");
        enemyList_prop=serializedObject.FindProperty("enemyList");
        bg_Prop = serializedObject.FindProperty("locationBG");
        priceflow_prop = serializedObject.FindProperty("itemPriceflow");
        hasCrash_prop = serializedObject.FindProperty("hasCrash");
        isDungeon_prop= serializedObject.FindProperty("isDungeon");
        minLevel_prop= serializedObject.FindProperty("minLevel");
        maxLevel_prop= serializedObject.FindProperty("maxLevel");
        hasEvent_prop= serializedObject.FindProperty("hasEvent");
        events_prop= serializedObject.FindProperty("events");
        dunTheme_prop= serializedObject.FindProperty("dungeonTheme");
        hasBoss_prop= serializedObject.FindProperty("hasBoss");
        bossAsset_prop= serializedObject.FindProperty("bossAsset");
        itemList_prop= serializedObject.FindProperty("itemList");
        dungeonType_prop= serializedObject.FindProperty("dungeonType");
        maxEnemyKill_prop =serializedObject.FindProperty("NeedsKill");
        bossList_prop=serializedObject.FindProperty("bossList");

    }

    public override void OnInspectorGUI()
    {
       serializedObject.Update();

       EditorGUILayout.PropertyField(Names_prop);
       EditorGUILayout.PropertyField(Detail_prop);
       EditorGUILayout.PropertyField(Npc_Prop,true);
       EditorGUILayout.PropertyField(IsLock_prop);
       EditorGUILayout.PropertyField(townType_prop);
       EditorGUILayout.PropertyField(bg_Prop);
       EditorGUILayout.PropertyField(priceflow_prop);
       EditorGUILayout.PropertyField(hasCrash_prop);


       EditorGUILayout.PropertyField(isDungeon_prop);
       bool isDungeon = isDungeon_prop.boolValue;
       if (isDungeon)
       {
           EditorGUILayout.PropertyField(minLevel_prop);
           EditorGUILayout.PropertyField(maxLevel_prop);
           EditorGUILayout.PropertyField(enemyList_prop,true);
           EditorGUILayout.PropertyField(hasEvent_prop);
           bool hasEvent = hasEvent_prop.boolValue;
           if (hasEvent)
           {
               EditorGUILayout.PropertyField(events_prop, true);
           }
           EditorGUILayout.PropertyField(dunTheme_prop);
           
           EditorGUILayout.PropertyField(hasBoss_prop);
           bool hasBoss = hasBoss_prop.boolValue;
           if (hasBoss)
           {
               EditorGUILayout.PropertyField(bossAsset_prop);
               EditorGUILayout.PropertyField(bossList_prop,true);
           }

           EditorGUILayout.PropertyField(itemList_prop, true);
           EditorGUILayout.PropertyField(dungeonType_prop);
           DungeonType t = (DungeonType) dungeonType_prop.enumValueIndex;
           switch (t)
           {
               case  DungeonType.None:
                   
                   break;
           }
           EditorGUILayout.PropertyField(maxEnemyKill_prop);

       }
       serializedObject.ApplyModifiedProperties();
    }
}
