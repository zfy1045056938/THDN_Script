using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_5_2 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
#endif
using PlayfulSystems.LoadingScreen;

[CustomEditor(typeof(LoadingScreenPro), true)]
[CanEditMultipleObjects]
public class LoadingScreenProDrawer : LoadingScreenProBaseDrawer {

    protected SerializedProperty audioListener;
    protected SerializedProperty sceneInfoHeader;
    protected SerializedProperty sceneInfoDescription;
    protected SerializedProperty sceneInfoImage;
    protected SerializedProperty tipHeader;
    protected SerializedProperty tipDescription;

    protected override void OnEnable() {
        base.OnEnable();

        if (loadingScreen.config != null && loadingScreen.config.loadAdditively) {
            EditorGUILayout.PropertyField(audioListener);

            if (DrawButton("Detect AudioListener"))
                TriggerAudioDetection();
        }

        audioListener = serializedObject.FindProperty("audioListener");
        sceneInfoHeader = serializedObject.FindProperty("sceneInfoHeader");
        sceneInfoDescription = serializedObject.FindProperty("sceneInfoDescription");
        sceneInfoImage = serializedObject.FindProperty("sceneInfoImage");
        tipHeader = serializedObject.FindProperty("tipHeader");
        tipDescription = serializedObject.FindProperty("tipDescription");
    }

    protected override SerializedProperty GetLastProperty() {
        return serializedObject.FindProperty("tipDescription");
    }

    protected override void DrawFields() {
        base.DrawFields();

        EditorGUILayout.PropertyField(sceneInfoHeader);
        EditorGUILayout.PropertyField(sceneInfoDescription);
        EditorGUILayout.PropertyField(sceneInfoImage);

        if (loadingScreen.config != null && !loadingScreen.config.showSceneInfos)
            EditorGUILayout.HelpBox("Scene Infos disabled in Loading Screen Pro config.", MessageType.Info);

        EditorGUILayout.PropertyField(tipHeader);
        EditorGUILayout.PropertyField(tipDescription);

        if (loadingScreen.config != null && !loadingScreen.config.showRandomTip)
            EditorGUILayout.HelpBox("Game Tips disabled in Loading Screen Pro config.", MessageType.Info);
    }

    void TriggerAudioDetection() {
        var proScreen = loadingScreen as LoadingScreenPro;

        if (proScreen == null)
            return;

        Undo.RecordObject(proScreen, "Detected AudioListener");
        proScreen.DetectAudioListener();
        EditorUtility.SetDirty(proScreen);
    }
}