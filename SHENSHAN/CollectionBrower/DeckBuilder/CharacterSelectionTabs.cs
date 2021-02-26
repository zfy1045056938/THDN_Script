using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using GameDataEditor;
using Unity.Mathematics;


public class CharacterSelectionTabs:MonoBehaviour
{
//    public List<CharacterFiliterTabs> tabs = new List<CharacterFiliterTabs>();
public static CharacterSelectionTabs instance;
    public List<GameObject> tabs = new List<GameObject>();
    public List<GameObject> classTbas = new List<GameObject>();
    public GameObject classTabs;
    public GameObject neutralTabWhenCollectionBrowsing;
    public GameObject filterObj;
    public Transform tabsPos;
    public Transform clsPos;
    public AudioClip Click;
    public int currentIndex = -1;

    private float duration = 0.1f;

    void Start()
    {
        instance=this;
       CreateTabs();
    }

    public void CreateTabs()
    {
       //Clear Old Tabs
      ClearObj();
      
      
        List<GDECharacterAssetData> cd = GDEDataManager.GetAllItems<GDECharacterAssetData>();
        for (int i = 0; i < cd.Count; i++)
        {
            GameObject tabObj = Instantiate(filterObj,tabsPos.position,Quaternion.identity)as GameObject;
            tabObj.transform.parent = tabsPos.transform;
            tabObj.GetComponent<CharacterFiliterTabs>().characterAsset = GetCharacterAsset(cd[i]);

            if(TownManager.CheckLan()==true){
            tabObj.GetComponent<CharacterFiliterTabs>().nameText.text = cd[i].ClassName.ToString();
            }else{
                tabObj.GetComponent<CharacterFiliterTabs>().nameText.text = cd[i].EClassName.ToString();
            }
            tabObj.GetComponent<CharacterFiliterTabs>().Deselect();
            if (cd[i].ClassName == "中立")
            {
                neutralTabWhenCollectionBrowsing = tabObj;
                neutralTabWhenCollectionBrowsing.GetComponent<CharacterFiliterTabs>().characterAsset = GetCharacterAsset(cd[i]);
                neutralTabWhenCollectionBrowsing.GetComponent<CharacterFiliterTabs>().Select(true);
            }
            //
            tabs.Add(tabObj);
        }
    }

    
    public void ClearObj(){
        if(tabs==null)return;
        var pos =tabsPos;
        var newp = new List<GameObject>();
        for(int i=0;i<pos.childCount;i++){
            
            var c= tabsPos.transform.GetChild(i).transform.gameObject;
            newp.Add(c);
        }

        foreach(var oc in newp){
            if (oc != null)
            {
                Destroy(oc);
            }
        }
    }

    public void CreateClassTabs(string jobs)
    {
       

        List<GDECharacterAssetData> cd = GDEDataManager.GetAllItems<GDECharacterAssetData>();
        foreach(GDECharacterAssetData cs in cd){
           
            if (cs.ClassName == Convert(jobs))
            {
                Debug.Log("Crete Tabs for classes");
                GameObject obj = Instantiate(classTabs,tabsPos.position,Quaternion.identity)as GameObject;
                obj.transform.SetParent(tabsPos);
                obj.GetComponent<CharacterFiliterTabs>().characterAsset = GetCharacterAsset(cs);
                if(TownManager.CheckLan()==true){
                obj.GetComponent<CharacterFiliterTabs>().nameText.text = cs.ClassName.ToString();
                }else{
                     obj.GetComponent<CharacterFiliterTabs>().nameText.text = cs.EClassName.ToString();
                }
                obj.GetComponent<CharacterFiliterTabs>().Select(true);

                // classTbas.Add(obj);
                tabs.Add((obj));
            }else
             //Generate Neu Tabs To ClassesPos
                if(cs.ClassName=="中立"){
                    Debug.Log("Generate Neu card at build mode");
                    GameObject neu = GameObject.Instantiate(neutralTabWhenCollectionBrowsing,tabsPos.position,Quaternion.identity)as GameObject;
            neu.transform.SetParent(tabsPos);
            neu.GetComponent<CharacterFiliterTabs>().characterAsset =GetCharacterAsset(cs);
            if(TownManager.CheckLan()==true){
            neu.GetComponent<CharacterFiliterTabs>().nameText.text=cs.ClassName.ToString();
            }else{
                 neu.GetComponent<CharacterFiliterTabs>().nameText.text=cs.EClassName.ToString();
            }
            // classTbas.Add(neu);
            tabs.Add(neu);
            }
         
        }
    }

    public string Convert(string job)
    {
        if (job == "Survicer")
        {
            return "生存者";
        }
        else if (job == "Magic")
        {
            return "祈求者";
        }else if (job == "Hunter")
        {
            return "猎人";
        }

        return "中立";
    }

   

    public CharacterAsset GetCharacterAsset(GDECharacterAssetData d){
        GDECharacterAssetData data= new GDECharacterAssetData(d.Key);
        return new CharacterAsset(GetPlayerJobs(data.PlayersJob),
        data.ClassName,data.MaxHealth,data.PowerName,GetCsBGSpritecs(data.AvatarImage),data.Detail,GetCsAvaSprite(data.BGSprite),
        data.AttackCard,data.ArmorCard);
    }

    public void SetClassOnClassTab(CharacterAsset asset)
    {
        Debug.Log("Set Classes Obj");
      GameObject co =  GameObject.Instantiate(classTabs,tabsPos.position,Quaternion.identity)as GameObject;
      co.transform.SetParent( tabsPos);
        co.GetComponent<CharacterFiliterTabs>().characterAsset = asset;
         co.GetComponent<CharacterFiliterTabs>().nameText.text = LoadAssetName(asset);
         co.GetComponent<CharacterFiliterTabs>().Select(true);
         tabs.Add(co);
      
      
    }
 private Sprite GetCsBGSpritecs(Texture2D bGSprite)
    {
        return Sprite.Create(bGSprite,new Rect(0,0,bGSprite.width,bGSprite.height),Vector2.zero);
    }

private PlayerJob GetPlayerJobs(string playersJob)
    {
        if(playersJob=="猎人"){
			return PlayerJob.Hunter;
		}else if(playersJob=="祈求者"){
			return PlayerJob.Magic;
		}else if(playersJob=="生存者"){
			return PlayerJob.Survicer;
		}
		return PlayerJob.None;
    }
    public Sprite GetCsAvaSprite(Texture2D avatarImage)
    {
        return Sprite.Create(avatarImage,new Rect(0,0,avatarImage.width,avatarImage.height),Vector2.zero);
    }
    string LoadAssetName(CharacterAsset asset){
        if(TownManager.CheckLan()==true){
        switch(asset.jobs){
            case PlayerJob.Hunter:
                return "猎人";
                break;
            case PlayerJob.Magic:
            return "研习者";
            break;
            case PlayerJob.Survicer:
            return "生存者";
            break;
        }
        return "中立";
        }else{
             switch(asset.jobs){
            case PlayerJob.Hunter:
                return "猎人";
                break;
            case PlayerJob.Magic:
            return "研习者";
            break;
            case PlayerJob.Survicer:
            return "生存者";
            break;
        }
        return "中立";
        }
    }

    /// <summary>
    /// Selects the tab.
    /// </summary>
    /// <param name="characterFiliterTabs">Character filiter tabs.</param>
    /// <param name="v">If set to <c>true</c> v.</param>
//    public void SelectTab(CharacterFiliterTabs characterFiliterTabs, bool instant)
//    {
//        
//        
//        int newIndex = tabs.IndexOf(characterFiliterTabs);
//
//        //
//        if (newIndex == currentIndex)
//        {
//            return;
//        }
//
//        currentIndex = newIndex;
//        //
//        foreach (CharacterFiliterTabs t in tabs)
//        {
//
//            //
//            if (t != characterFiliterTabs)
//            {
//                t.Deselect(instant);
//            }
//
//        }
//
//        characterFiliterTabs.Select(instant);
//        SoundManager.instance.PlayClipAtPoint(Click, Vector3.zero, SoundManager.instance.musicVolume, false);
//        //Update Card todo
//        DeckBuilderScreen.instance.collectionBroswerScript.characterAsset = characterFiliterTabs.characterAsset;
//        Debug.Log(DeckBuilderScreen.instance.collectionBroswerScript.characterAsset);
//        DeckBuilderScreen.instance.collectionBroswerScript.includeAllCharacter = characterFiliterTabs.showAllCharacter;
//
//
//
//    }
    public void  SelectTab(GameObject characterFiliterTabs, bool instant)
    {

        int newIndex=tabs.IndexOf(characterFiliterTabs);
 //        if (DeckBuilder.instance.InDeckBuildingMode == false)
 //        {
 //
 //                foreach(var tab in tabs){
 //                    if(tab!=null && tab.GetComponent<CharacterFiliterTabs>().characterAsset==characterFiliterTabs.GetComponent<CharacterFiliterTabs>().characterAsset){
 // newIndex = tabs.IndexOf(characterFiliterTabs);
 //                    Debug.Log("Collection selection"+newIndex);
 //                    }
 //                }
 //            
 //             
 //        }
 //        else
 //        {
 //           foreach(var ct in classTbas){
 //           if(ct!=null && ct.GetComponent<CharacterFiliterTabs>().characterAsset==characterFiliterTabs.GetComponent<CharacterFiliterTabs>().characterAsset){
 //            newIndex = classTbas.IndexOf(characterFiliterTabs);
 //            Debug.Log("classes select"+newIndex);
 //           }
 //           }
 //        }
        //
        // foreach(var tab in tabs){
        //     if(tab!=null){
        //         newIndex = tabs.IndexOf(characterFiliterTabs);
        //         Debug.Log("Collection selection"+newIndex);
        //     }
        // }

    Debug.Log(newIndex+"select index");
        //
        if (newIndex == currentIndex)
        {
            return;
        }

        currentIndex = newIndex;
        //
//         foreach (GameObject t in tabs)
//         {
// if (t == null) return;
//             //
//             if (t != characterFiliterTabs)
//             {
//                 t.GetComponent<CharacterFiliterTabs>().Deselect(instant);
//             }

//         }
        
//         foreach (GameObject t in classTbas)
//         {
//             if (t == null) return;
//             if (t != characterFiliterTabs)
//             {
//                 t.GetComponent<CharacterFiliterTabs>().Deselect(instant);
//             }

//         }

        // characterFiliterTabs.GetComponent<CharacterFiliterTabs>().Select(instant);
        SoundManager.instance.PlayClipAtPoint(Click, Vector3.zero, SoundManager.instance.musicVolume, false);
        //Update Card todo
        DeckBuilderScreen.instance.collectionBroswerScript.characterAsset = characterFiliterTabs.GetComponent<CharacterFiliterTabs>().characterAsset.jobs;
        Debug.Log(DeckBuilderScreen.instance.collectionBroswerScript.characterAsset);
        DeckBuilderScreen.instance.collectionBroswerScript.includeAllCharacter = characterFiliterTabs.GetComponent<CharacterFiliterTabs>().showAllCharacter;



    }


}