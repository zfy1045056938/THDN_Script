using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
/// <summary>
/// Deck selection.
/// </summary>
//public class DeckSelection 
//{
//    public GameObject selectionContent;
//    public DeckIcon[] deckIcons;
//    public HeroIcon heroIcon;
   

//}



public class DeckSelectionScreen :MonoBehaviour{

        		public GameObject screenContent;	//info
        		// public DeckIcon[] deckIcons;
				public List<GameObject> deckIcons;
        		public HeroInfoPanel heroSelection;
				public GameObject playerDeck;
				public Transform deckPos;
				
                private PlayerData player;
                private NPCManager npc;
    public static DeckSelectionScreen instance;
    private bool inTowns;
    
  

    [Header("BTN")] public Button StartBtn;
    public Button EditBtn;
    public Button CancelBtn;

        	public  void Awake(){
		
        instance = this;
        player = FindObjectOfType<PlayerData>();
        npc = FindObjectOfType<NPCManager>();
		
            }

            public void Update()
            {

	            StartBtn.interactable = (BattleStartInfo.SelectDeck != null);
	            
	            
	            StartBtn.onClick.AddListener(() =>
	            {
		            screenContent.SetActive(false);

		            if (TownManager.instance.PreSelectPack==false)
		            {
			            
			            EnemyDeckSelection.instance.ShowScreen();
			           
		            }
	            });

            }

        

            /// <summary>
    /// Hides the screen.
    /// </summary>
    private void HideScreen()
    {
        screenContent.gameObject.SetActive(false);
    }


    //显示桌面
  public  void ShowDecks()
    {
	  DeckStorge.instance.LoadDecksFromPlayerPrefs();
	    //当桌面无创建人物,则跳转到collections
	    if (DeckStorge.instance.AllDecks.Count == 0)
	    {
		    HideScreen();
		    CharacterSelectionScreen.instance.ShowScreen();
		    return;
	    }
	    else
	    {
		    //显示图标
		    foreach (GameObject icon in deckIcons)
		    {
			    icon.gameObject.SetActive(false);
			    icon.GetComponent<DeckIcon>().InstantDeSelect();
		    }

		    for (int j = 0; j < DeckStorge.instance.AllDecks.Count; j++)
		    {
			    GameObject obj = Instantiate(playerDeck, deckPos.position,
				    Quaternion.identity) as GameObject;
			    obj.transform.SetParent(deckPos);
			    obj.GetComponent<DeckIcon>().ApplyLookForIcon(DeckStorge.instance.AllDecks[j]);
			    obj.GetComponent<DeckIcon>().DeckNameText.text = DeckStorge.instance.AllDecks[j].deckName.ToString();
			    obj.gameObject.SetActive(true);

			    NetworkServer.Spawn(obj);

			    deckIcons.Add(obj);

		    }
	    }

    }


    /// <summary>
    /// Shows the screen.
    /// </summary>
	public void ShowScreen(bool intown)
    {
	    Debug.Log(intown);
	    
	    inTowns = intown;

	    if (TownManager.instance.atDungeon == false)
	    {
		    screenContent.gameObject.SetActive(true);

		    ShowDecks();
		    heroSelection.OnOpen();
	    }
	    else
	    {
		    Debug.Log("Dungeon Only use One Pack to explore can get new tmp Card From dungeon");
		    EnemyDeckSelection.instance.ShowScreen();
	    }

    }

   
        
        }


