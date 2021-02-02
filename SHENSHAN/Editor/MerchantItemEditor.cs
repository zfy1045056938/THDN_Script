using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEditor;


public class MerchantItemEditor :EditorWindow 
{
    
    SerializedObject serObj;

    private static ItemDatabase itemDatabase;

    private static MerchantController merchant;

    [MenuItem(("Window/MerchantItemManager"))]
    static void Init()
    {
        if (!Selection.activeGameObject.GetComponent<MerchantController>())
        {
            EditorUtility.DisplayDialog("No Merchant","select a merchant","OK");
            return;
        }
        else
        {
           merchant= Selection.activeGameObject.GetComponent<MerchantController>();
        }
        
        itemDatabase = Resources.Load("ItemDatabase",typeof(ItemDatabase))as ItemDatabase;
        EditorWindow.GetWindow(typeof(MerchantItemEditor));
    }


    public void OnEnable()
    {
        serObj = new SerializedObject(merchant);
    }

    public void OnInspectorUpdate()
    {
        itemDatabase = (ItemDatabase)Resources.Load("ItemDatabase",typeof(ItemDatabase))as ItemDatabase;
        
    }

    private void OnGUI()
    {
      serObj.Update();

      List<string> itemName = new List<string>();

      for (int i = 0; i < itemDatabase.items.Count; i++)
      {
          itemName.Add(itemDatabase.items[i].itemName);
      }
      
      //
      SerializedProperty itemIDs = serObj.FindProperty("itemIDs");

      for (int i = 0; i < itemIDs.arraySize; i++)
      {
          EditorGUILayout.BeginHorizontal();
          itemIDs.GetArrayElementAtIndex(i).intValue = 
          EditorGUILayout.Popup(itemDatabase.FindItem(
                  itemIDs.GetArrayElementAtIndex(i).intValue).itemName,
              itemIDs.GetArrayElementAtIndex(i).intValue,
              itemName.ToArray()
          );

          GUI.color = Color.red;

        //   if (GUILayout.Button("X",GUILayout.Width(25)))
        //   {
        //       merchant.itemIDs.Remove(i);
        //   }
          //
          GUI.color = Color.white;
          EditorGUILayout.EndHorizontal();
      }
      
      
      //
      GUI.color= Color.green;
    //   if (GUILayout.Button("Add new Item"))
    //   {
    //       merchant.itemIDs.Insert(merchant.itemIDs.Count,0);
    //   }

      serObj.ApplyModifiedProperties();

      if (GUI.changed)
      {
          EditorUtility.SetDirty(itemDatabase);
      }

    }
}
