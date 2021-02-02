using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class DraftManager : MonoBehaviour
{
    public static DraftManager instance;
    public GameObject content;

    public UIPanel draftCardList;
    public TextMeshProUGUI currentRoundText;
    public Text totalRoundText;
    public TextMeshProUGUI noticeText;
    [Header("CardPrefab")]
    public GameObject creaturePrefab;
    public GameObject spellPrefab;
    public Transform cPos;
    public DeckBuilder deckBuilder;
    public int limitDraftCardList=8;

    public List<GameObject> selectCardList;
    public List<GameObject> draftObjs;
    public int totalRound=10;
    public int currentRound=0;
    public int equipRound =3;
    // public bool draftMode=false;
    public bool selectCardState=false;
    public bool equipmentState=false;
    public bool hasSelect =false;
    public bool fullDeck=false;
   
  public   int MaxepicCount=2;
        public int currEpic =0;
        public bool enoughEpic=false;

    void Start(){
        instance=this;

      

    }

            void Update(){


            //     if(draftMode==true){
            //          Debug.Log("Start Draft");
            // StartCoroutine(StartDraftRoutine());
            //     }
        
        currentRoundText.text=currentRound.ToString();
    
    }

    public void StartDraft(){
         
        //Clear All P V
        //
         if(DeckBuilderScreen.instance.draft==true){
             Debug.Log("Start Draft");
            StartCoroutine(StartDraftRoutine());
        }else{

        }
    }


    public void GenerateCard(int currentRound){
       
       
        
        Debug.Log("Generate Card");
          var classesCard = from e in CardCollection.instance.allCardsArray where e.characterAsset == deckBuilder.buildingForCharacter select e;   
        var creatureCards = from e in CardCollection.instance.allCardsArray where e.typeOfCards == TypeOfCards.Creature select e;   
        var spellCard = from e in CardCollection.instance.allCardsArray where e.typeOfCards == TypeOfCards.Spell select e;
        // var cCard = from r in CardCollection.instance.allCardsArray where r.characterAsset.className == deckBuilder.buildingForCharacter.className select r ;
        
        List<CardAsset>cards= new List<CardAsset>();
       
        if(deckBuilder.deckList.Count<=10){
            //Creature Card
          
                noticeText.text="选择一张随从牌";
            cards = creatureCards.ToList();
           

            if(enoughEpic==true){
                var note = from dc in creatureCards where dc.ratityOption != CardRatityOption.Epic  
                where dc.ratityOption != CardRatityOption.LEGEND 
                where dc.ratityOption != CardRatityOption.Ancient 
                select dc;
                cards = note.ToList();
            }
            Debug.Log("Got Card"+cards.Count);
           CardAsset c = new CardAsset();
           int rv = Random.Range(0,cards.Count);
           Debug.Log(rv +"is RND Num");
           for(int i=0;i<cards.Count;i++){
                if(rv == i){
                    
                  c = CardCollection.instance.GetCardAssetByName(cards[i].name);
                                     
                                                         
                    if(c.typeOfCards==TypeOfCards.Creature){
                    GameObject obj = Instantiate(creaturePrefab,cPos)as GameObject;
                    obj.transform.SetParent(cPos.transform);
                    obj.GetComponent<AddCardToDeck>().cardAsset =c;
                     obj.GetComponent<OneCardManager>().cardAsset=c;
                    
                    obj.GetComponent<OneCardManager>().ReadCardFromAsset();
                    
                    
                draftObjs.Add(obj);

                    }else if(c.typeOfCards==TypeOfCards.Spell){
                         GameObject obj = Instantiate(spellPrefab ,cPos)as GameObject;
                    obj.transform.SetParent(cPos.transform);
                    obj.GetComponent<AddCardToDeck>().cardAsset =c;
                     draftObjs.Add(obj);
                       obj.GetComponent<OneCardManager>().cardAsset=c;
                    obj.GetComponent<OneCardManager>().ReadCardFromAsset();
                    }else if(c.typeOfCards==TypeOfCards.Token){
                        //TO CREATURE
         GameObject obj = Instantiate(creaturePrefab,cPos)as GameObject;
                    obj.transform.SetParent(cPos.transform);
                    obj.GetComponent<AddCardToDeck>().cardAsset =c;
                     draftObjs.Add(obj);
                       obj.GetComponent<OneCardManager>().cardAsset=c;
                     obj.GetComponent<OneCardManager>().ReadCardFromAsset();
                    }
                    
                }
           }
          

        }else{
           fullDeck=true;

        }
      
        
    }

    //
    public IEnumerator StartDraftRoutine(){

        totalRoundText.gameObject.SetActive(false);
        
                selectCardState=false;
                fullDeck=false;
                hasSelect=false;
                enoughEpic=false;
                currentRound=0;
        //Clesr old card 
        foreach(var g in draftObjs){
            if(g!=null){
                Destroy(g);
            }
        }   
        //

        while(currentRound<10){
            selectCardState=true;
            ++currentRound;
            
           for(int i=1;i<limitDraftCardList;i++){
            GenerateCard(currentRound);
            }
        


            while(!hasSelect){
               yield return null;
                
            }

            
             //Clear Old Card
            RefreashPage(currentRound);
            

         

            if(fullDeck==true){
                
              
                selectCardState=false;
                fullDeck=false;
                hasSelect=false;
                enoughEpic=false;

                totalRoundText.gameObject.SetActive(true);
                totalRoundText.text="灵感结束";
            }
            
        }

        yield return new WaitForSeconds(0.4f);
    }

    public void RefreashPage(int current){{
        if(current<10){

               foreach(var g in draftObjs){
            if(g!=null){
                Destroy(g);
            }
        }


            GenerateCard(current);
            hasSelect=false;
            Canvas.ForceUpdateCanvases();
        }
    }

    }

   
}
