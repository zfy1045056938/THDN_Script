using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDataEditor;


public class LoadCharacterByName :  MonoBehaviour {


    // private CharacterAsset[] allCharacterAssets;

    private Dictionary<string, CharacterAsset> allDic = new Dictionary<string, CharacterAsset>();

    public static LoadCharacterByName instance;
    //
    public  void Awake()
    {
        if (instance == null) instance = this;
        
   
    }

    void Start()
    {
        LoadCharacter();
    }

    
    //
    void LoadCharacter()
    {
        List<GDECharacterAssetData> allCharacter = GDEDataManager.GetAllItems<GDECharacterAssetData>();
                            for(int i=0; i<allCharacter.Count;i++ ){
                                CharacterAsset ca = new CharacterAsset(
              GetCharacterJob(allCharacter[i].PlayersJob),allCharacter[i].ClassName,
              allCharacter[i].MaxHealth,allCharacter[i].PowerName,GenerateSprite(allCharacter[i].AvatarImage),
              allCharacter[i].Detail,GenerateSprite(allCharacter[i].BGSprite),allCharacter[i].AttackCard,allCharacter[i].ArmorCard);
            if (!allDic.ContainsKey(ca.className))
            {
                Debug.Log(ca.className+"classes names");
                allDic.Add(ca.className, ca);
            }
        }
     
    }

    public Sprite GenerateSprite(Texture2D tex){
        return Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),Vector2.zero);
    }
public PlayerJob GetCharacterJob(string n){
    if(n=="祈求者"){
        return PlayerJob.Magic;
    }else if(n=="猎手"){
        return PlayerJob.Hunter;
    }else if(n=="生存者"){
        return PlayerJob.Survicer;
    }
    return PlayerJob.Survicer;
}

    public CharacterAsset GetCharacterByName(string name){
        if (allDic.ContainsKey(name))
        {
            return allDic[name];
        }else{
            return null;
        }
    }
}
