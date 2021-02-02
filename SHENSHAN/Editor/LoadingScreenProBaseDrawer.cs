using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_5_2 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
#endif
using PlayfulSystems.LoadingScreen;

[CustomEditor(typeof(LoadingScreenProBase), true)]
[CanEditMultipleObjects]
public class LoadingScreenProBaseDrawer : Editor {

    protected LoadingScreenProBase loadingScreen;
    protected SerializedProperty script;
    protected SerializedProperty config;
    protected SerializedProperty behaviorAfterLoad;
    protected SerializedProperty timeToAutoContinue;
    protected SerializedProperty waitAfterCompletion;

    protected virtual void OnEnable() {
        script = serializedObject.FindProperty("m_Script");
        config = serializedObject.FindProperty("config");
        behaviorAfterLoad = serializedObject.FindProperty("behaviorAfterLoad");
        timeToAutoContinue = serializedObject.FindProperty("timeToAutoContinue");
    }

    public override void OnInspectorGUI() {
        loadingScreen = target as LoadingScreenProBase;

        if (loadingScreen == null)
            return;

        serializedObject.Update();
        DrawFields();
        DrawRemaining();
        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void DrawFields() {
#if UNITY_5_2 || UNITY_5_3_OR_NEWER
        DrawHelpBox();
#endif

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(script);
        EditorGUILayout.PropertyField(config);

        EditorGUILayout.PropertyField(behaviorAfterLoad);
        GUI.enabled = (loadingScreen.behaviorAfterLoad == LoadingScreenProBase.BehaviorAfterLoad.ContinueAutomatically);
        EditorGUILayout.PropertyField(timeToAutoContinue);
        GUI.enabled = true; 
    }

    protected virtual SerializedProperty GetLastProperty() {
        return serializedObject.FindProperty("timeToAutoContinue");
    }

    protected void DrawRemaining() {
        var property = GetLastProperty();

       while (property.NextVisible(false)) {
            EditorGUILayout.PropertyField(property, true);
        }
    }

    protected bool DrawButton(string label) {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel(new GUIContent(" "));

            if (GUILayout.Button(label))
                return true;
        }
        EditorGUILayout.EndHorizontal();
        return false;
    }

#if UNITY_5_2 || UNITY_5_3_OR_NEWER
    void DrawHelpBox() {
        if (IsCurrentSceneLoadingScene() == false) 
            EditorGUILayout.HelpBox("This scene is not set to the LoadingScreenConfig script!\nIt is currently set to: \"" + loadingScreen.GetLoadingSceneName() +
                "\".\nIf you want this scene to be the default loading scene, then edit the script where it says !!!EDIT HERE!!!", MessageType.Info);

        if (HasLegalConfig() == false) {
            EditorGUILayout.HelpBox("No Loading Screen Config set! This is essential to run the loading screen.", MessageType.Warning);

            if (GUILayout.Button("Find Loading Screen Config"))
                SetLegalConfig();
        }

        if (NumScenesNotInBuildSettings() > 0) { 
            EditorGUILayout.HelpBox("This Scene is not added to the BuildSettings (or is disabled)!", MessageType.Warning);

            if (GUILayout.Button("Add Open Scenes to Build Settings"))
                AddCurrentScenesToBuildSettings();
        }
    }

    bool HasLegalConfig() {
        return loadingScreen.config != null;
    }

    void SetLegalConfig() {
        //AssetDatabase.GUIDToAssetPath to get assetpaths and e.g AssetDatabase.LoadAssetAtPath to load an asset.
        LoadingScreenConfig config;

        string configAssetName = "LoadingScreenProConfig";
        var assets = AssetDatabase.FindAssets(configAssetName);

        if (assets == null || assets.Length == 0 || assets[0] == null) { 
            Debug.LogWarning("No Asset with name "+ configAssetName + " found!");
            return;
        }

        var path = AssetDatabase.GUIDToAssetPath(assets[0]);
        config = AssetDatabase.LoadAssetAtPath<LoadingScreenConfig>(path);

        if (config == null) {
            Debug.LogWarning("Couldn't load Asset of Type " + typeof(LoadingScreenConfig).ToString() + " at "+path+"!");
            return;
        }

        Undo.RecordObject(loadingScreen, "Detected Config File");
        loadingScreen.config = config;
        EditorUtility.SetDirty(loadingScreen);
    }


    bool IsCurrentSceneLoadingScene() {
        var name = loadingScreen.GetLoadingSceneName();
        Scene scene;

        for (int i = 0; i < EditorSceneManager.sceneCount; i++) {
            scene = EditorSceneManager.GetSceneAt(i);
            if (scene.name == name)
                return true;
        }

        return false;
    }

    int NumScenesNotInBuildSettings() {
        int i = 0;
        Scene scene;
        EditorBuildSettingsScene buildScene;

        for (int s = 0; s < EditorSceneManager.sceneCount; s++) {
            scene = EditorSceneManager.GetSceneAt(s);
            buildScene = GetSceneBuildSettings(scene);

            if (buildScene == null || buildScene.enabled == false)
                i++;
        }

        return i;
    }

    EditorBuildSettingsScene GetSceneBuildSettings(Scene scene) {
        var buildScenes = EditorBuildSettings.scenes;

        for (int b = 0; b < buildScenes.Length; b++)
            if (buildScenes[b].path == scene.path)
                return buildScenes[b];

        return null;
    }
    
    void AddCurrentScenesToBuildSettings() {
        var newScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        Scene scene;
        EditorBuildSettingsScene buildScene;

        for (int i = 0; i < EditorSceneManager.sceneCount; i++) {
            scene = EditorSceneManager.GetSceneAt(i);
            buildScene = GetSceneBuildSettings(scene);

            if (buildScene == null) { 
                newScenes.Add(new EditorBuildSettingsScene(scene.path, true));
            }
            else if (buildScene.enabled == false) {
                for (int s = 0; s < newScenes.Count; s++) { 
                    if (newScenes[s].path == scene.path) {
                        if (newScenes[s].enabled == false)
                            newScenes[s].enabled = true;

                        break;
                    }
                }
            }
        }


        EditorBuildSettings.scenes = newScenes.ToArray();
    }
#endif


    void TriggerViewDetection() {
        if (loadingScreen == null)
            return;

        Undo.RecordObject(loadingScreen, "Detected Views in Active Objects");
        //loadingScreenPro.DetectViewObjects();
        EditorUtility.SetDirty(loadingScreen);
    }


}