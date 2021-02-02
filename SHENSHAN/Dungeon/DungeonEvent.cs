using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;
using UnityEngine.UI;
using GameDataEditor;
using DG.Tweening;
using TMPro;
using Mirror;
public class DungeonEvent : MonoBehaviour
{
    public static DungeonEvent instance;
   public UIPanel panel;
   public List<GameObject> dungeonObjs;
   public Transform dPos;
    public GameObject dungeonCard;
    public GameObject cardObj;
    //Load From GDE
    public List<DungeonEventClass> dungeonEvents;
    public Dictionary<string,DungeonEventClass> deDic = new Dictionary<string, DungeonEventClass>();
[Header("Dungeon Details")]
   public TextMeshProUGUI detailText;
   public TextMeshProUGUI rewardText;
   public TextMeshProUGUI nameText;
     public TextMeshProUGUI extraText;
     public TextMeshProUGUI enemyBouns;

   public int limitCard=3;

    [HideInInspector]
   public int index=-1;
   
   public DungeonCard selectCard;

[Header("Btn")]
   public Button selectBtn;
   public Button relootBtn;
   public Button cancelBtn; 
   
   
   #region  Dungeon extra point add to target

  
//    public static int DUNGEONEX
   
   
   #endregion


   void Start()
   {
       instance = this;
    LoadDEFromGDE();

    foreach(var v in dungeonEvents){
        if(!deDic.ContainsKey(v.deName)){
            deDic.Add(v.deName,v);
        }

        nameText.text="";
        rewardText.text="";
        enemyBouns.text="";
        detailText.text="";
    
    }

    // List<GDEDungeonEventData> gdd = GDEDataManager.GetAllItems<GDEDungeonEventData>();
    
    // //Create Sprite
    // for (int i = 0; i < dungeonEvents.Count; i++)
    // {
    //     GDEDungeonEventData ge = new GDEDungeonEventData(gdd[i].Key);
    //     if (ge.DEName == dungeonEvents[i].deName)
    //     {
    //         dungeonEvents[i].deIcon = Utils.CreateSprite(ge.DEIcon);
    //     }
    // }
   }



    void LoadDEFromGDE()
    {
        List<GDEDungeonEventData> dvd = GDEDataManager.GetAllItems<GDEDungeonEventData>();

        for (int i = 0; i < dvd.Count; i++)
        {
            DungeonEventClass dvc = new DungeonEventClass();
            dvc.deID = dvd[i].DungeonID;
            dvc.deName = dvd[i].DEName;
            dvc.deDetail = dvd[i].DEDetail;
            dvc.deAmount = dvd[i].DEAmount;
            dvc.dePerc = dvd[i].DEPerc;
            //dvc.DEReward = dvd[i].DEReward;
            //dvc.DAAmount = dvd[i].DAAMoun;
            dvc.deIcon = Utils.CreateSprite(dvd[i].DEIcon);
            dvc.rarity = Utils.GetCardRarity(dvd[i].DERarity);
            dvc.DET = GetDETFromGDE(dvd[i].DEType);

            dungeonEvents.Add(dvc);
            //    }
        }
    }



    /// <summary>
    /// 
    /// </summary>
   public void ShowDungeonEvent(int round,int num,int sIndex=-1){
       int counter =0;
       panel.gameObject.SetActive(true);
       if(dungeonEvents.Count!=0){
           for(int i=0;i<limitCard;i++){
              
                GameObject obj =Instantiate(dungeonCard,dPos.position,Quaternion.identity)as GameObject;
                obj.transform.SetParent(dPos);
               int rnd =Random.Range(0,dungeonEvents.Count);

               DungeonEventClass dev = dungeonEvents[rnd];
               if(dev!=null){
                   obj.GetComponent<DungeonCard>().dec =dev;
                   obj.GetComponent<DungeonCard>().nText.text=dev.deName.ToString();
                   obj.GetComponent<DungeonCard>().ddText.text=dev.deDetail.ToString();
                //    obj.GetComponent<DungeonCard>().drText.text=dev.deName.ToString();
                   obj.GetComponent<DungeonCard>().index = counter;
                      obj.GetComponent<DungeonCard>().iconSprite.sprite = dev.deIcon;
                  
                   
//                   obj.AddComponent<SelectionIndex>().GetComponent<SelectionIndex>().index = counter;
                   counter++;
                   
                dungeonObjs.Add(obj);

                    //Network Module
                    NetworkServer.Spawn(obj);
                    
               
               }else{
                   Debug.Log("No dungeon event");
               }
           }
       }
        //for ai select rnd index
        if (sIndex != -1)
        {
            selectCard = dungeonObjs[sIndex].GetComponent<DungeonCard>();
            ActiveCardEffect();
        }
    }

   public DungeonEventClass GetIDByDE(string dName){
       if (deDic.ContainsKey(dName))
       {
               return deDic[dName];
       }
       return null;
   }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="card"></param>
    /// <param name="sindex"></param>
   public void SelectthisCard(DungeonCard  card,int sindex){
       if(card==null && sindex==0){
           selectCard=null;
           index=0;
       }else{
           selectCard =card;
        index = sindex;
        nameText.text=selectCard.dec.deName.ToString();
        detailText.text =selectCard.dec.deDetail.ToString();
        rewardText.text =selectCard.dec.DEReward*100+"%";
        enemyBouns.text =selectCard.dec.DAAmount.ToString();
       }
        
   }

   public void ClosePanel(){
       //clear obj
       foreach(var v in dungeonObjs){
           if(v!=null){
               Destroy(v);
           }
       }
       //
         detailText.text="";
    rewardText.text="";
    nameText.text="";
      extraText.text="";
       //
       panel.gameObject.SetActive(false);
   }

   public void ActiveCardEffect(bool isTop =false)
   {
       if (selectCard != null)
       {
            if (isTop) { 
           DungeonEventClass getDEC = deDic[selectCard.nText.text];

           if (getDEC != null)
           {
               //Add to RP 
               GameObject detObj = Instantiate(DungeonExplore.instance.dungeonEffectIcon,
                   DungeonExplore.instance.dungeonEffectPos.position, Quaternion.identity) as GameObject;
               detObj.transform.SetParent(DungeonExplore.instance.dungeonEffectPos);
               
               detObj.GetComponent<DungeonIcon>().dvc = getDEC;
            
               detObj.GetComponent<DungeonIcon>().det = getDEC.DET;
               detObj.GetComponent<DungeonIcon>().IconSprite.sprite = getDEC.deIcon;
               detObj.GetComponent<DungeonIcon>().amount = getDEC.deAmount;


               DungeonExplore.instance.dungeonIcons.Add(detObj);

                //
                //Add Bouns to DungeonExplore
              //
              DungeonExplore.instance.AddBouns(getDEC);

                
               //Clear the card
               panel.gameObject.SetActive(false);


                //TEST CODE
                // DiscoverManager.instance.ShowDiscover(CardCollection.instance.allCardsArray,1,DiscoverType.HardMode);

               foreach (var v in dungeonObjs)
               {
                   if (v != null)
                   {
                       Destroy(v);
                   }
               }

           }

            }
            else
            {
                Debug.Log("AI Active ShenShan Module");
                DungeonEventClass getDEC = deDic[selectCard.nText.text];
                //
                DungeonExplore.instance.AddBouns(getDEC);


            }
        }
       else
       {
           Debug.Log("UnSelect Card");
       }
       
       

   }

   public DungeonEventType GetDETFromGDE(string n)
   {
       if (n == "HDEF")
       {
           return DungeonEventType.HARMOR;
       
       }else if (n == "CDEF")
       {
           return DungeonEventType.CARMOR;
       }
       //
       else if (n == "HATK")
       {
           return DungeonEventType.ATK;
       }else if (n == "CATK")
       {
           return DungeonEventType.CATK;
       }
       //
       else if (n == "HHEAL")
       {
           return DungeonEventType.HEAL;
       }else if (n == "CHEAL")
       {
           return DungeonEventType.CHEAL;
       }
       //
       else if (n == "ESD")
       {
           return DungeonEventType.ESD;
       }else if (n == "WDUR")
       {
           return DungeonEventType.WDUR;
       }else if (n == "ERESOURCES")
       {
           return DungeonEventType.ERS;
       }
       

       return DungeonEventType.None;
   }


  

   public void ClearEvent()
   {
       panel.gameObject.SetActive(false);

       foreach (var v in dungeonObjs)
       {
           if (v != null)
           {
               Destroy(v);
           }
       }
   }

}
