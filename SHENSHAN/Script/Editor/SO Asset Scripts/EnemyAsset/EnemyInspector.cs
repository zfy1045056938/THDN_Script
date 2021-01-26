using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyAsset)),CanEditMultipleObjects]
public class EnemyInspector : Editor
{
    public SerializedProperty
        EName_prop,
        Tags_prop,
        npcTpye_prop,
        Head_prop,
        Frame_prop,
        Health_prop,
        ratity_prop,
        conversation_prop,
        isLock_prop,
        isBoss_prop,
        Loc_prop,
        powerName_prop,
        exp_prop,
        gold_prop,

        hasQuest_prop,
        questlist_prop,
        cardList_prop,
        detail_prop,
        hasReward_prop,
        rewardid_prop,
        moneyReward_prop,
        expReward_prop,
        dustReward_prop,

        hasEntry_prop,
        entryDetail_prop,
        hasCardList_prop,
        
        isMerchant_prop,
        sellItems_prop;
        

         static ItemDatabase  itemDatabase;

        static MerchantController merchant;
       
      

    [SerializeField] public CardAsset[] cardList =new CardAsset[10];
    [SerializeField] public int[] itemID ;
    [SerializeField] public QuestLogWindow.QuestInfo[] questinfo;

    //Get Data From enemyAsset
    private void OnEnable()
    {
        
        EName_prop = serializedObject.FindProperty("EnemyName");
        Tags_prop = serializedObject.FindProperty("Tags");
        npcTpye_prop=serializedObject.FindProperty("npcType");
        isLock_prop = serializedObject.FindProperty("isLock");
        Head_prop = serializedObject.FindProperty("Head");
        Frame_prop = serializedObject.FindProperty("Frame");
        Health_prop = serializedObject.FindProperty("Health");
        ratity_prop = serializedObject.FindProperty("ratity");
        detail_prop = serializedObject.FindProperty("detail");
        conversation_prop = serializedObject.FindProperty("conver");
        Loc_prop = serializedObject.FindProperty("Loc");
        powerName_prop = serializedObject.FindProperty("powerName");
        exp_prop = serializedObject.FindProperty("exp");
        gold_prop = serializedObject.FindProperty("gold");

        isBoss_prop=serializedObject.FindProperty("isBoss");
        hasQuest_prop = serializedObject.FindProperty("hasQuest");
        questlist_prop = serializedObject.FindProperty("questList");
        hasEntry_prop = serializedObject.FindProperty("hasEntry");
        entryDetail_prop = serializedObject.FindProperty("entryDetail");
        moneyReward_prop = serializedObject.FindProperty("moneyReward");
        expReward_prop = serializedObject.FindProperty("expReward");
        dustReward_prop = serializedObject.FindProperty("dustReward");

        rewardid_prop = serializedObject.FindProperty("rewardId");
        hasReward_prop = serializedObject.FindProperty("hasReward");
 cardList_prop = serializedObject.FindProperty("cardList");
 hasCardList_prop=serializedObject.FindProperty("hasCard");

        isMerchant_prop =serializedObject.FindProperty("isMerchant");
        sellItems_prop=serializedObject.FindProperty("itemList");
       
    }
    
    private void OnInspectorUpdate() {
          itemDatabase = (ItemDatabase)Resources.Load("ItemDatabase",typeof(ItemDatabase))as ItemDatabase;
       
    }
    
    //Show UI
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
  
        EditorGUILayout.PropertyField(EName_prop);
        EditorGUILayout.PropertyField(Tags_prop);
        EditorGUILayout.PropertyField(npcTpye_prop);
        EditorGUILayout.PropertyField(isLock_prop);
        EditorGUILayout.PropertyField(Head_prop);
        EditorGUILayout.PropertyField(Frame_prop);
        EditorGUILayout.PropertyField(Health_prop);
        EditorGUILayout.PropertyField(ratity_prop);
        EditorGUILayout.PropertyField(conversation_prop);
        EditorGUILayout.PropertyField(Loc_prop);
        EditorGUILayout.PropertyField(detail_prop);
        EditorGUILayout.PropertyField(powerName_prop);
        EditorGUILayout.PropertyField(exp_prop);
        EditorGUILayout.PropertyField(gold_prop);

        EditorGUILayout.PropertyField(isBoss_prop);
        EditorGUILayout.PropertyField(hasQuest_prop);
        bool hasQuest = hasQuest_prop.boolValue;
        if (hasQuest)
        {
            EditorGUILayout.PropertyField(questlist_prop,true);
           
        } 
        bool hasEntry = hasEntry_prop.boolValue;
                     if (hasEntry)
                     {
                         EditorGUILayout.PropertyField(entryDetail_prop);
                     }

        EditorGUILayout.PropertyField(hasReward_prop);
            bool hasReward = hasReward_prop.boolValue;
            if (hasReward)
            {
                EditorGUILayout.PropertyField(moneyReward_prop);
                EditorGUILayout.PropertyField(dustReward_prop);
                EditorGUILayout.PropertyField(exp_prop);
                EditorGUILayout.PropertyField(rewardid_prop, true);
            }

            EditorGUILayout.PropertyField(hasCardList_prop);
            bool hasCard = hasCardList_prop.boolValue;
            if(hasCard){
            EditorGUILayout.PropertyField(cardList_prop, true);
            }

            EditorGUILayout.PropertyField(isMerchant_prop);
            bool isMerchant = isMerchant_prop.boolValue;
            if(isMerchant){
           
           EditorGUILayout.PropertyField(sellItems_prop,true);
           
            }
            
            //UI
            serializedObject.ApplyModifiedProperties();

    }
}
