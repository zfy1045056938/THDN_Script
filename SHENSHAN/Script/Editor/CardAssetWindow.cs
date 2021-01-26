using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameDataEditor;

public enum SelectionType{
    AllItem,
    CreateItem,
    LoadCardFromGde,


}
public class CardAssetWindow : EditorWindow
{
    public CardAsset cardAsset;
    static CardCollection collection;
  public SerializedObject serObj;
    public SelectionType selectionType;

   public GDEItemManager gde_manager;
//    public GDECardAssetData gde_Card;

   public CardAsset viewCard;

 private Vector2 scrollPos;
    [MenuItem("Window/CardManager")]
     static void Init(){

         GDEDataManager.Init("gde_data");
        collection = Resources.Load("CardAndNames",typeof(CardCollection))as CardCollection;
        EditorWindow.GetWindow(typeof(CardAssetWindow));
    }
      private void OnEnable() {

      serObj = new SerializedObject(this);    
    }

     private void OnInspectorUpdate() {
         collection=(CardCollection)Resources.Load("CardAndNames",typeof(CardCollection))as CardCollection;
    }

   private void OnGUI() {
        serObj.Update();

        GUILayout.Space(10);
        GUI.color = Color.green;
        GUILayout.BeginHorizontal();
        if(selectionType == SelectionType.AllItem){
            GUI.color =Color.green;
        }else{
            GUI.color=Color.white;
        }
        if(GUILayout.Button("View All Card")){
            selectionType = SelectionType.AllItem;
        }
        //
         if(selectionType == SelectionType.CreateItem){
            GUI.color =Color.green;
        }else{
            GUI.color=Color.white;
        }
        if(GUILayout.Button("Generate Card")){
            selectionType = SelectionType.CreateItem;
        }
        //
        if(selectionType == SelectionType.AllItem){
            GUI.color =Color.green;
        }else{
            GUI.color=Color.white;
        }
        if(GUILayout.Button("View All Card")){
            selectionType = SelectionType.LoadCardFromGde;
        }
        GUILayout.EndHorizontal();

        GUI.color=Color.white;

        GUILayout.Space(25);
        if(selectionType == SelectionType.AllItem){
            ViewAllItems();
        }else if(selectionType == SelectionType.CreateItem){
            CreateItems();
        }else if(selectionType==SelectionType.LoadCardFromGde){
            LoadCardFromGde();
        }


        serObj.ApplyModifiedProperties();
        if(GUI.changed){
            EditorUtility.SetDirty(collection);
            //
            PrefabUtility.SetPropertyModifications(PrefabUtility.GetPrefabObject(collection),PrefabUtility.GetPropertyModifications(collection));
        }
  }


  void ViewAllItems(){
   scrollPos = GUILayout.BeginScrollView(scrollPos);
for(int i=0;i<collection.allCardsArray.Count;i++){
   if(collection.allCardsArray[i]==viewCard){
       GUILayout.BeginHorizontal();
       if(GUILayout.Button(">","label",GUILayout.Width(10))){
           viewCard = collection.allCardsArray[i];
           //Show Card
           if(GUILayout.Button(collection.allCardsArray[i].name)){
               viewCard =collection.allCardsArray[i];
           }
           //
           GUILayout.EndHorizontal();
           GUI.color=Color.white;
           
       }
   }else{
       GUI.color=Color.green;
       GUILayout.BeginHorizontal();
        if(GUILayout.Button("^","label",GUILayout.Width(10))){
            viewCard=null;
            return;
        }
         if(GUILayout.Button(collection.allCardsArray[i].name)){
            viewCard=null;
            return;
        }
       GUILayout.EndHorizontal();
       GUI.color=Color.white;
       viewCard.name = EditorGUILayout.TextField("Card Name",viewCard.name.ToString());
   }
}
   GUILayout.EndScrollView();
  }

  void CreateItems(){

  }

  void LoadCardFromGde(){
      
  }


}
