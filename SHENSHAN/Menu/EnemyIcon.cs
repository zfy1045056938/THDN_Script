using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine.EventSystems;

[System.Serializable]
public class EnemyInfo{
    public EnemyInfo(List<CardAsset> cards, EnemyAsset enemyAsset)
    {
        this.cards = cards;
        this.enemyAsset = enemyAsset;
    }

    public List<CardAsset> cards;
    public EnemyAsset enemyAsset;
    public List<QuestInfos> questList;
    public EnemyPortraitVisual visual;
    public static EnemyInfo selectEnemy;
    public EnemyInfo (List<CardAsset>cardList,EnemyAsset asset,EnemyPortraitVisual v){
        //Fixed Config by editor
//        this.cards=new List<CardAsset>(EnemyPortraitVisual.instance.enemyCardList);
        this.cards = new List<CardAsset>(cardList);
        this.enemyAsset =asset;
        this.visual =v;
    }

    public EnemyInfo(List<QuestInfos> questInfo)
    {
        this.questList = new List<QuestInfos>(questInfo);
    }
    public EnemyInfo(){}
}



//Storge data , when click the icon show the enemy target
public class EnemyIcon : MonoBehaviour,IPointerClickHandler
{
        public EnemyInfo EnemyInfoDetail { get; set; }
    public EnemyPortraitVisual portrait;
    //
    private float InitialScale;
    private float TargetScale = 1.3f;
    private bool selected = false;
    public static EnemyIcon instance;
 
    public List<EnemyInfo> infos { get; set; }
    void Awake()
    {
        instance = this;
        portrait = GetComponent<EnemyPortraitVisual>();
        InitialScale = transform.localScale.x;
       
    }


    void Start()
    {
        SetNpc();
    }

    void SetNpc()
    {
        infos = new List<EnemyInfo>();
        //
        EnemyInfo info = new EnemyInfo(new List<CardAsset>(portrait.enemyAsset.cardList), portrait.enemyAsset,portrait);
        EnemyInfoDetail = info;
        infos.Add(EnemyInfoDetail);
        Debug.Log("Add NPC Success");
        foreach (EnemyInfo e in infos )
        {
            Debug.Log("ENEMY NAME =>\t\t"+e.enemyAsset.EnemyName);
        }
    }
    //Bind Data to Selected Enemy Icon
    public void ApplyLookToIcon(EnemyInfo info)
    {
        info = new EnemyInfo(portrait.enemyAsset.cardList,portrait.enemyAsset,portrait);
        EnemyInfoDetail = info;
        infos.Add(EnemyInfoDetail);

        portrait.enemyAsset = info.enemyAsset;   //READ ENEMY ASSET
        portrait.enemyCardList = info.cards;    //READ CARD LIST
        portrait.ReadFromEnemyAsset();
       
    }

//    void OnMouseDown()
//    {
//        // show the animation
//        if (!selected)
//        {
//            selected = true;
//            // zoom in on the deck only if it is complete
//          
//            //Get This Enemy und show the detail infopanel
//            EnemyPanel.instance.SelectDeck(this);
//            // deselect all the other Portrait Menu buttons 
//            DeckIcon[] allPortraitButtons = GameObject.FindObjectsOfType<DeckIcon>();
//            foreach (DeckIcon m in allPortraitButtons)
//                if (m != this)
//                    m.DeSelect();
//        }
//        else
//        {
//            Deselect();
//            EnemyDeckSelection.instance.enemySelectPanel.SelectDeck(null);
//        }
//    }

    public void Deselect()
    {
        transform.DOScale(InitialScale, 0.5f);
        selected = false;
    }

    public void InstantDeselect()
    {
        transform.localScale = new Vector3(InitialScale, InitialScale, InitialScale);
        selected = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // show the animation
        if (!selected)
        {
            selected = true;
            // zoom in on the deck only if it is complete
          
            //Get This Enemy und show the detail infopanel
            EnemyDeckSelection.instance.enemySelectPanel.SelectDeck(this);
            //sound
            SoundManager.instance.PlayRndMusic();
            // deselect all the other Portrait Menu buttons 
            DeckIcon[] allPortraitButtons = GameObject.FindObjectsOfType<DeckIcon>();
            foreach (DeckIcon m in allPortraitButtons)
                if (m != this)
                    m.DeSelect();
        }
        else
        {
            Deselect();
            EnemyDeckSelection.instance.enemySelectPanel.SelectDeck(null);
        }
    }

   
}
