using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


[CustomEditor(typeof(CardAsset)), CanEditMultipleObjects]
public class CardAssetCustomInspector : Editor
{

    public SerializedProperty


        //creature
        atk_prop,
        def_prop,
        health_prop,
        creatureScriptName_prop,
        specialCreatureAmount_prop,
        creatureType_prop,
        atkForOneTurn_prop,
        charge_prop,
        isEnemyCard_prop,
        //normal
        characterAsset_prop,
        cardImage_prop,
        tag_prop,
        ratityOption_prop,
        typeofCard_prop,
        manacost_prop,
        hasToken_prop,
        tokenCard_prop,
        isToken_prop,
        overriderLimitOfCard_prop,
        description_prop,
        taunt_prop,
        hasRound_prop,
        RoundEvent_prop,
   

        //spell
        target_prop,
        specialAmount_prop,
        spellScriptName_prop,
        
        //buff
        creatureBuffType_prop,
        hasBuff_prop,
        elementalDamageType_prop,
        hasDET_prop,
        hasdetailCreatureType_prop,
        deatilCreatureType_prop,
        
        //resistance
        fireResistance_prop,
        iceResistance_prop,
        poisonResistance_prop,
        electronicResistance_prop,

        //stats
        STR_prop,
        DEX_prop,
        INT_prop,
        SPD_prop,
        RES_prop;
        


        /// <summary>
    /// Ons the enable.
    /// </summary>
    private void OnEnable()
    {
//        cardID_prop = serializedObject.FindProperty("CardID");
        description_prop = serializedObject.FindProperty("cardDetail");
        characterAsset_prop = serializedObject.FindProperty("characterAsset");
        cardImage_prop = serializedObject.FindProperty("cardSprite");
        tag_prop = serializedObject.FindProperty("tags");
        ratityOption_prop = serializedObject.FindProperty("ratityOption");
        manacost_prop = serializedObject.FindProperty("manaCost");
        overriderLimitOfCard_prop = serializedObject.FindProperty("OverrideLimitOfThisCardsInDeck");
        typeofCard_prop = serializedObject.FindProperty("typeOfCards");
        
      
        //Creature
        atk_prop = serializedObject.FindProperty("cardAtk");
        def_prop = serializedObject.FindProperty("cardDef");
        health_prop = serializedObject.FindProperty("cardHealth");
        hasToken_prop = serializedObject.FindProperty("hasToken");
        tokenCard_prop = serializedObject.FindProperty("tokenCardAsset");
        isToken_prop = serializedObject.FindProperty("isTokenCard");
        creatureScriptName_prop = serializedObject.FindProperty("creatureScriptName");
        taunt_prop = serializedObject.FindProperty("taunt");
        specialCreatureAmount_prop = serializedObject.FindProperty("specialCreatureAmount");
        creatureType_prop = serializedObject.FindProperty("creatureType");
        atkForOneTurn_prop = serializedObject.FindProperty("atkForOneTurn");
        charge_prop=serializedObject.FindProperty("charge");
        //resistance
        fireResistance_prop = serializedObject.FindProperty("fireResistance");
        iceResistance_prop = serializedObject.FindProperty("iceResistance");
        poisonResistance_prop = serializedObject.FindProperty("poisonResistance");
        electronicResistance_prop = serializedObject.FindProperty("electronicResistance");
        //Spell
        target_prop = serializedObject.FindProperty("target");
        specialAmount_prop = serializedObject.FindProperty("SpecialSpellAmount");
        spellScriptName_prop = serializedObject.FindProperty("spellScriptName");
        hasRound_prop = serializedObject.FindProperty("hasRound");
        RoundEvent_prop = serializedObject.FindProperty("RoundTime");
        
        //stats
        STR_prop = serializedObject.FindProperty("STR");
        DEX_prop = serializedObject.FindProperty("DEX");
        INT_prop = serializedObject.FindProperty("INT");
        SPD_prop = serializedObject.FindProperty("SPD");
        RES_prop = serializedObject.FindProperty("RES");
        
        //extra
        elementalDamageType_prop = serializedObject.FindProperty("damageEType");
        hasDET_prop = serializedObject.FindProperty("hasDET");
        hasBuff_prop = serializedObject.FindProperty("hasBuff");
        creatureBuffType_prop = serializedObject.FindProperty("spellBuffType");
        hasdetailCreatureType_prop = serializedObject.FindProperty("hasDetailCreatureType");
        deatilCreatureType_prop = serializedObject.FindProperty("detailCreatureType");
    

         //others
         isEnemyCard_prop=serializedObject.FindProperty("isEnemyCard");




    }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();




//        EditorGUILayout.PropertyField(cardID_prop);
            EditorGUILayout.PropertyField(characterAsset_prop);
            EditorGUILayout.PropertyField(cardImage_prop);
            EditorGUILayout.PropertyField(tag_prop);
            EditorGUILayout.PropertyField(ratityOption_prop);
            EditorGUILayout.PropertyField(description_prop);
            EditorGUILayout.PropertyField(manacost_prop);
            EditorGUILayout.PropertyField(isToken_prop);
            EditorGUILayout.PropertyField(overriderLimitOfCard_prop);
            EditorGUILayout.PropertyField(typeofCard_prop);
            EditorGUILayout.PropertyField(hasBuff_prop);
            bool hasBuff = hasBuff_prop.boolValue;
            if (hasBuff)
            {
                EditorGUILayout.PropertyField(creatureBuffType_prop);
            }

            EditorGUILayout.PropertyField(hasDET_prop);
            bool hasDET = hasDET_prop.boolValue;
            if (hasDET)
            {
                EditorGUILayout.PropertyField(elementalDamageType_prop);
            }


            bool hasDCreature = hasdetailCreatureType_prop.boolValue;
            if (hasDCreature)
            {
                EditorGUILayout.PropertyField(deatilCreatureType_prop);
            }

            EditorGUILayout.PropertyField(hasRound_prop);
            bool hasRound = hasRound_prop.boolValue;
            if (hasRound)
            {
                EditorGUILayout.PropertyField(RoundEvent_prop);
            }
            EditorGUILayout.PropertyField(isEnemyCard_prop);

        //根据枚举分类
        TypeOfCards tc = (TypeOfCards)typeofCard_prop.enumValueIndex;
        switch (tc)
        {
            case TypeOfCards.Creature:
                EditorGUILayout.PropertyField(atk_prop);
                EditorGUILayout.PropertyField(def_prop);
                EditorGUILayout.PropertyField(health_prop);
                EditorGUILayout.PropertyField(creatureType_prop);
                EditorGUILayout.PropertyField(charge_prop);
                EditorGUILayout.PropertyField(atkForOneTurn_prop);
                EditorGUILayout.PropertyField(taunt_prop);
                EditorGUILayout.PropertyField(creatureScriptName_prop);
                EditorGUILayout.PropertyField(specialCreatureAmount_prop);
                EditorGUILayout.PropertyField(fireResistance_prop);
                EditorGUILayout.PropertyField(iceResistance_prop);
                EditorGUILayout.PropertyField(poisonResistance_prop);
                EditorGUILayout.PropertyField(electronicResistance_prop);
                EditorGUILayout.PropertyField(STR_prop);
                EditorGUILayout.PropertyField(DEX_prop);
                EditorGUILayout.PropertyField(INT_prop);
                EditorGUILayout.PropertyField(SPD_prop);
                EditorGUILayout.PropertyField(RES_prop);
                
                EditorGUILayout.PropertyField(hasToken_prop);
                bool isToken = hasToken_prop.boolValue;
                if (isToken)
                {
                    EditorGUILayout.PropertyField(tokenCard_prop);
                }



                break;
            case TypeOfCards.Spell:
                EditorGUILayout.PropertyField(target_prop);
                EditorGUILayout.PropertyField(specialAmount_prop);
                EditorGUILayout.PropertyField(spellScriptName_prop);
                break;
        }

    


        serializedObject.ApplyModifiedProperties();
    }





}
