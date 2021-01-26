 using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using System;
using System.Linq;
using GameDataEditor;

//保存卡组进序列化
//1.
[System.Serializable]
public class DeckInfo{
    
    public string deckName;
    public CharacterAsset characterAsset;
    public List<CardAsset> cardAssets;
    public bool isComplete=false;
    public static DeckInfo selectedDeck;   //Tmp
    public int atkNum;
    public int defNum;
    //
        public DeckInfo(string deckName,CharacterAsset characterAsset,List<CardAsset> cs,int atkNum,int defNum){
           
         this.deckName=deckName;
         this.characterAsset =DeckStorge.instance.LoadCharacterFromGDE(characterAsset.className);
        this. atkNum =  atkNum;
        this.defNum = defNum;

//             CharacterSelectionScreen.instance.GetCharacterByName(characterAsset.className);
//              new CharacterAsset(
//              characterAsset.jobs,characterAsset.className,characterAsset.maxHealth,characterAsset.heroPowerName,characterAsset.avatarImage,characterAsset.description,characterAsset.avatarBGImage);
       cardAssets = new List<CardAsset>(cs);
        }

   

        public int NumberOfthisCardInDeck(CardAsset asset){
            int count  =0;
            foreach(CardAsset ca in cardAssets){
                if(ca==asset){
                    count++;
                }
                
            }
            return count;
        }

    /// <summary>
    /// Ises the complete.
    /// </summary>
    /// <returns><c>true</c>, if complete was ised, <c>false</c> otherwise.</returns>
    public bool IsComplete()
    {
        return (DeckBuilderScreen.instance.buildScript.amountOfCardsInDeck == cardAssets.Count);
     }
}

/// <summary>
/// Deck storge.
/// </summary>
public class DeckStorge : MonoBehaviour
{

    public static DeckStorge instance;
    public List<DeckInfo> AllDecks;
    
    //
    private bool alreadyLoadedDecks = false;
    List<CardAsset> cards = new List<CardAsset>();
    private CharacterAsset characterAsset;
    public int limitofDeckSlot=20;

   
    public void Awake()
    {
        AllDecks = new List<DeckInfo>();
        instance = this;

    }


 
   

    /// <summary>
    /// Loads the decks from player prefs.
    /// </summary>
    public void LoadDecksFromPlayerPrefs()
    {
        List<DeckInfo> deckFound = new List<DeckInfo>();
        //
        for (int i = 0; i <= 9; i++)
        {
            string deckListKey = "Deck" + i.ToString();


            string deckNameKey = "Deckname" + i.ToString();

            string characterKey = "Hero" + i.ToString();
           

            int atkNum = PlayerPrefs.GetInt("atkNum_"+i.ToString());
            
            int defNum = PlayerPrefs.GetInt("defNum_"+i.ToString());
            string[] DeckAsCardNames = PlayerPrefsX.GetStringArray(deckListKey);
            
            //
            if (DeckAsCardNames.Length > 0 && PlayerPrefs.HasKey(characterKey) && PlayerPrefs.HasKey(deckNameKey))
            {
                string characterName = PlayerPrefs.GetString(characterKey);
                string deckName = PlayerPrefs.GetString(deckNameKey);

                
                //Add card to list
                List<CardAsset> card = new List<CardAsset>();
                foreach (string names in DeckAsCardNames)
                {
                    card.Add(CardCollection.instance.GetCardAssetByName(names));
                }
                //
//                deckFound.Add(new DeckInfo(deckName,LoadCharacterByName.instance.GetCharacterByName(characterName),card));
                deckFound.Add(new DeckInfo(deckName,LoadCharacterFromGDE(characterName),card,atkNum,defNum));

            }
        }
        
        AllDecks = deckFound;
        // Debug.Log("Now Deck exists "+AllDecks.Count+"In collection");
      
      
    }

    public CharacterAsset LoadCharacterFromGDE(string cn)
    {
        List<GDECharacterAssetData> gad = GDEDataManager.GetAllItems<GDECharacterAssetData>();

        for(int i = 0; i<gad.Count;i++)
        {
            if (cn == gad[i].ClassName)
            {
                return new CharacterAsset(Utils.GetPlayerJob(gad[i].PlayersJob),gad[i].ClassName,
                gad[i].MaxHealth,gad[i].PowerName,Utils.CreateSprite(gad[i].AvatarImage),gad[i].Detail,
                Utils.CreateSprite(gad[i].BGSprite),gad[i].AttackCard,gad[i].ArmorCard);
            }
        }

        return null;
    }
    /// <summary>
    /// Save Deck To PlayerPrefs 
    /// </summary>
    public void SaveDecksIntoPlayerPrefs(){
        for (int i = 0; i < 9; i++)
        {
            string characterKey = "Hero" + i.ToString();
            string deckNameKey = "Deckname" + i.ToString();
            //
            if (PlayerPrefs.HasKey(characterKey))
            {
                PlayerPrefs.DeleteKey(characterKey);
            }
            if (PlayerPrefs.HasKey(deckNameKey))
            {
                PlayerPrefs.DeleteKey(deckNameKey);
            }
        }
        for (int j = 0; j < AllDecks.Count; j++)
            {
                string decklist = "Deck" + j.ToString();
                string characterKey = "Hero" + j.ToString();
                string deckNameKey = "Deckname" + j.ToString();
                int atkNum=0;
                int defNum=0;
                //
                List<string> cLIst = new List<string>();
                foreach (CardAsset c in AllDecks[j].cardAssets)
                {
                    cLIst.Add(c.name);
                }

                string[] carray = cLIst.ToArray();
            //Set Playerprefs 
            PlayerPrefsX.SetStringArray(decklist, carray);
            PlayerPrefs.SetString(deckNameKey, AllDecks[j].deckName);
            PlayerPrefs.SetString(characterKey, AllDecks[j].characterAsset.className);
            PlayerPrefs.SetInt("atknum_"+j.ToString(),atkNum);
            PlayerPrefs.SetInt("defnum_"+j.ToString(),defNum);
            
            }
    }
//
    private void OnApplicationQuit()
    {
     
        PlayerPrefs.Save();
        
        SaveDecksIntoPlayerPrefs();
    }
}


