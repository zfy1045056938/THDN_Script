using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using GameDataEditor;
using SimpleJSON;



/// <summary>
/// 卡牌管理页面,用于查看卡牌拥有情况,用于对卡牌功能的操作与管理,包含一下页面
/// 1.基本的信息查看页面
/// 2.按类型查看卡牌信息
/// 3.查看单个卡牌的信息
/// 4.卡牌鉴赏
/// 5.制作页面与强化功能
/// 6.构建卡组,
/// 7.卡组显示
/// 8.城堡管理
/// 9.对角色信息的管理
/// </summary>
public class CardCollection:MonoBehaviour
{

    private DeckBuilder deckBuilder;
    //public CraftManager craftManager;
    private OnecardInformation oneCardInformation;
    

    public int DefaultNumberOfBasicCards = 4;
    //
    public Dictionary<string, CardAsset> AllCardOfCard = new Dictionary<string, CardAsset>();
    //
    public Dictionary<CardAsset, int> QuantityOfEachCards = new Dictionary<CardAsset, int>();
    
    public Dictionary<CardRatityOption,Color> ratity = new Dictionary<CardRatityOption, Color>();
    
    
    //
    // public CardAsset[] allCardsArray;
    public List<CardAsset> allCardsArray ;

    
    private CardAsset assets;
    private CharacterAsset characterAsset;
    private CharacterAsset[] jobAsset;
    private CardAsset cardAsset;
    private ManaFiliter manaList;

    public static CardCollection instance;

    public static float ROLLNORMALPERC=0.9f;
    public static float ROLLRARECARDPERC = 0.7f;
    public static float ROLLEPICCARDPERC=0.3f;
    public static float ROLLLENDGENDPERC=0.1f;

    private void Awake()
    {
        instance = this; 
      
      
        Debug.Log(allCardsArray.Count+"in array");
        // allCardsArray = Resources.LoadAll<CardAsset>("");  //Laod all resource
        for(int i=0;i<allCardsArray.Count;i++){
                        
            if (!AllCardOfCard.ContainsKey(allCardsArray[i].name))
            {
                
                AllCardOfCard.Add(allCardsArray[i].name, allCardsArray[i]);  //添加进dictionary
            }
        }
        
      
                        
        LoadQuantityofCardsPlayerPrefs();
    
    }

    public void LoadCardToDic(){
         for(int i=0;i<allCardsArray.Count;i++){
                        
            if (!AllCardOfCard.ContainsKey(allCardsArray[i].name))
            {
                
                AllCardOfCard.Add(allCardsArray[i].name, allCardsArray[i]);  //添加进dictionary
            }
        }
    }

    public void DraftAddQuality(CardAsset asset){
        if(QuantityOfEachCards.ContainsKey(asset)&&QuantityOfEachCards[asset]<2)
        {
            if (QuantityOfEachCards.ContainsKey(asset))
            {
                QuantityOfEachCards[asset]++;
            }
            else
            {
                Debug.Log("no exist card");
            }
        }else{
            Debug.Log("FULL CARD at dic 0");
        }

    }

    /// <summary>
    /// Loads the quantityof cards player prefs.
    /// </summary>
    public void LoadQuantityofCardsPlayerPrefs(){
        foreach(CardAsset ca in allCardsArray){
            if(ca.ratityOption==CardRatityOption.NORMAL){
                //TODO
                QuantityOfEachCards.Add(ca,DefaultNumberOfBasicCards);
            }else if(PlayerPrefs.HasKey(ca.name+"_NumberOf_")){
                QuantityOfEachCards.Add(ca,PlayerPrefs.GetInt(ca.name+"_NumberOf_"));
            }else
            QuantityOfEachCards.Add(ca,0);
        }
    }



    //写入用户持久层
    public void SaveQuantityOfCardsIntoPlayerPrefs(){
        foreach(CardAsset ca in allCardsArray){
                if(ca.ratityOption==CardRatityOption.NORMAL){
                    PlayerPrefs.SetInt(ca.name+"_NumberOf_",DefaultNumberOfBasicCards);
                   
                   
                }else{
                    PlayerPrefs.SetInt(ca.name+"_NumberOf_",QuantityOfEachCards[ca]);
                }
                
        }
    }

   public void LoadCard()
    {
        for (int i = 0; i < allCardsArray.Count; i++)
        {

            if (!AllCardOfCard.ContainsKey(allCardsArray[i].name))
            {

                AllCardOfCard.Add(allCardsArray[i].name, allCardsArray[i]); //添加进dictionary
            }
        }
    }

    public void AddCards(CardAsset card){
    allCardsArray.Add(card);
}
    

    public CardAsset GetCardAssetByName(string name){
        if(AllCardOfCard.ContainsKey(name)){
            return AllCardOfCard[name];
        }else return null;
    }

    private void OnApplicationQuit()
    {
        SaveQuantityOfCardsIntoPlayerPrefs();   
    }

    //Linq To Query Card Type || Info 
    //条件筛选包括
    //1.是否拥有卡牌
    //2.稀有度
    //3.键盘输入
    //4.水晶数
    // public List<CardAsset> GetCard(bool showingcardPlayerIsHave =false,
    //                                bool includeAllRatities =true,
    //                                bool includeAllCharacterCard=true,
    //                                int ManaCost=-1,
    //                                CardRatityOption ratity = CardRatityOption.NORMAL,
    //                                CharacterAsset characterAsset = null,
    //                                string keyword="",
    //                                bool includeTokenCard=false
    // ){

    //     var cards = from card in allCardsArray select card;
    
    //     if (!showingcardPlayerIsHave)
    //     {
    //         cards = cards.Where(card =>QuantityOfEachCards[card]>0);
    //     }
    //     if (!includeTokenCard)
    //     {
    //         cards = cards.Where(card => card.isTokenCard == false);
    //     }
    //     //Show Card for the characterasstet
    //     // if (!includeAllCharacterCard)
    //     // {
    //     //     cards = cards.Where(card => card.characterAsset == characterAsset);
    //     // }

    //     if (keyword!=null && keyword!=" ")
    //     {
    //         cards = cards.Where(card => (card.name.ToLower().Contains(keyword.ToLower()) ||
    //                                    (card.tags.ToLower().Contains(keyword.ToLower()) && !keyword.ToLower().Contains(""))));
    //     }

    //     if (!includeAllRatities)
    //     {
    //         cards = cards.Where(card => card.ratityOption == ratity);
    //     }
        
        
        
    //     if (ManaCost==7)
    //     {
    //         cards = cards.Where(card => card.manaCost >= 7);
    //     }
    //     else if (ManaCost!=-1)
    //     {
    //         cards = cards.Where(card => card.manaCost == ManaCost);
    //     }


    //     var returnList = cards.ToList<CardAsset>();
    //     // returnList.Sort((x,y)=>{
    //     //     if(x.manaCost > y.manaCost){
    //     //         return 1;
    //     //     }else if(x.manaCost<y.manaCost){
    //     //         return -1;
    //     //     }else{
    //     //         return x.name.CompareTo(y.name);
    //     //     }
    //     // });
    //     // returnList.Sort();
        

    //     return returnList;
    // }
     public List<CardAsset> GetCard(bool showingcardPlayerIsHave =false,
                                   bool includeAllRatities = false,
                                   bool includeAllCharacterCard=false,
                                   int ManaCost=-1,
                                   CardRatityOption ratity = CardRatityOption.NORMAL,
                                   PlayerJob playerJob =PlayerJob.None,
                                   string keyword="",
                                   bool includeTokenCard=false,
                                   bool isEnemyCard = false
    ){

        var cards = from card in allCardsArray select card;
        //Token not show up
        cards =cards.Where(card=>card.typeOfCards!=TypeOfCards.Token);
        
        if (showingcardPlayerIsHave==false)
        {
            cards = cards.Where(card =>QuantityOfEachCards[card]>0);
        }
        // if (!includeTokenCard)
        // {
        //     cards = cards.Where(card => card.isTokenCard == false);
        // }
        //Show Card for the characterasstet
//        if (!includeAllCharacterCard)
//        {
//            cards = cards.Where(card => card.characterAsset == characterAsset);
//        }
        //show target card with tab 
        if (playerJob != PlayerJob.None)
        {
            cards = cards.Where(card => card.characterAsset.jobs == playerJob);
        }
        else
        {
          
            cards = cards.Where(card => card.characterAsset.jobs == PlayerJob.None);
        }

        

        if (keyword!=null && keyword!=" ")
        {
            cards = cards.Where(card => (card.name.ToLower().Contains(keyword.ToLower()) ||
                                       (card.tags.ToLower().Contains(keyword.ToLower()) && !keyword.ToLower().Contains(""))));
        }

        if (!includeAllRatities)
        {
            cards = cards.Where(card => card.ratityOption == ratity);
        }

        if(ratity != CardRatityOption.NORMAL){
            cards=cards.Where(card=>card.ratityOption==ratity);

        }

        
        
        
        
        if (ManaCost==7)
        {
            cards = cards.Where(card => card.manaCost >= 7);
        }
        else if (ManaCost!=-1)
        {
            cards = cards.Where(card => card.manaCost == ManaCost);
        }

        cards=cards.Where(card=>card.typeOfCards == TypeOfCards.Creature);

        var returnList = cards.ToList<CardAsset>();
        // returnList.Sort((x,y)=>{
        //     if(x.manaCost > y.manaCost){
        //         return 1;
        //     }else if(x.manaCost<y.manaCost){
        //         return -1;
        //     }else{
        //         return x.name.CompareTo(y.name);
        //     }
        // });
        // returnList.Sort();
        returnList.Sort();
        

        return returnList;
    }

    /// <summary>
    /// Gets the card with ratity.
    /// </summary>
    /// <returns>The card with ratity.</returns>
    /// <param name="ratity">Ratity.</param>
//     public List<CardAsset> GetCardWithRatity(CardRatityOption ratity){
// //        var cards = from CardInfo in allCardsArray
// //                          where CardInfo.ratityOption == CardRatityOption.NORMAL
// //                          select CardInfo;
// //        var returnList = cards.ToList<CardAsset>();
//         return GetCard(true, true, false, -1, ratity,null,"",false);
//     }

  public List<CardAsset> GetCardWithRatity(CardRatityOption ratity){
//        var cards = from CardInfo in allCardsArray
//                          where CardInfo.ratityOption == CardRatityOption.NORMAL
//                          select CardInfo;
//        var returnList = cards.ToList<CardAsset>();
        return GetCard(true, true, false, -1, ratity,characterAsset.jobs,"",false,true);
    }
    
    
    
    //Ratity
    // public List<CardAsset> QueryByRatity(CardRatityOption ratity)
    // {
    //     return GetCard(true, false, false, -1, ratity, null, "", false);
    // }
    ///// <summary>
    ///// Gets the cards with job.
    ///// </summary>
    ///// <returns>The cards with job.</returns>
    ///// <param name="characterAsset">Character asset.</param>
    //public List<CardAsset>GetCardsWithJob(CharacterAsset characterAsset){
    //    var job = from CharacterAsset in jobAsset
    //              where characterAsset.jobs == PlayerJob.Magic
    //              select characterAsset;
    //    var returnToList = jobAsset.ToList<CharacterAsset>();

    //    return GetCard(false,false,false,-1,CardRatityOption.NORMAL,characterAsset," ",false);
    //}

    public List<CardAsset> GetCardForClassess(CharacterAsset asset){
        var cards = from card in allCardsArray
                    where card.characterAsset==asset
                    select card;
        var returnList =cards.ToList<CardAsset>();
        return returnList;
    }

    /// <summary>
    /// 根据卡牌职业遍历
    /// </summary>
    /// <param name="asset"></param>
    /// <returns></returns>
    public List<CardAsset> GetCardsOfCharacter(CharacterAsset asset){

//        var cards = from card in allCardsArray
//                    where card.characterAsset = asset
//                    select card;
//        var returList = cards.ToList<CardAsset>();
//        returList.Sort();


        // return GetCard(true, true, false, -1,CardRatityOption.NORMAL,asset,"",false);
        return GetCard(true, true, false, -1,CardRatityOption.NORMAL,asset.jobs,"",false,true);
    }

}