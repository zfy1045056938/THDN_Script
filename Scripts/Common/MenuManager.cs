using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using PixelCrushers.DialogueSystem;
using PixelCrushers.Wrappers;
using DG.Tweening;
using PixelCrushers.DialogueSystem.UnityGUI;
using UnityEditor;

//Menu Controller
public class MenuManager : AbstractUIRoot
{

    [Header("BaseInfo")]
    public Players players;
    public Text moneyText;
    public Text dustText;
    public GUISkin guiSkin;

    [Header("Menu")]
    public GameObject PlayPrefab;
    public GameObject SavePrefab;
    public GameObject LoadPrefab;
    public GameObject CollectionPrefab;
    public GameObject ShopPrefab;
    public GameObject SavePanelPrefab;
    //Quest Panel
    public GameObject SimpleQuestLogWinow;
    public GameObject DeatailQuestLogWindow;
    public CreatePanel createPanel;


    [Header("SoundManager")]
    public SoundManager soundManager;

    [Header("System")]
    public UnityEvent onOpen = new UnityEvent();
    public UnityEvent onClose = new UnityEvent();
    private Rect windowRect = new Rect(0, 0, 500, 500);
    private ScaledRect scaledRect = ScaledRect.FromOrigin(ScaledRectAlignment.MiddleLeft, ScaledValue.FromPixelValue(300), ScaledValue.FromPixelValue(320));
   

    [Header("DataBind")]
    public Dictionary<int, Players> player = new Dictionary<int, Players>();
    //private method
    private Transform initalPos;
    private List<Players> playerDatas;
    private bool isOpening = false;

    [Header("InfoPanel")]
    public Text playerNameTxt;
    public CreatePanel newHeroPanel;

    [Header("bool")]
     private bool showCreatepanel;

    public static MenuManager instance;
    

    public  void Awake()
    {
        players = GameObject.FindGameObjectWithTag("Player").GetComponent<Players>();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        PlayerPrefs.SetString("PlayerName", "joe");
        instance = this;
    }

    public void Start()
    {

        if (PlayerPrefs.HasKey("PlayerName"))
        {
            showCreatepanel = false;
            newHeroPanel.gameObject.SetActive(false);
            Debug.Log(PlayerPrefs.GetString("PlayerName"));
            PlayerPrefs.GetString("PlayerName", playerNameTxt.text);

        }else{
            showCreatepanel = true;
            newHeroPanel.gameObject.SetActive(true);
           
        }


    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)&& !DialogueManager.isConversationActive )
        {
            Debug.Log("Open The Tiny Menu");
            SetMenuStates(!isOpening);
        }
    }

    private void OnGUI()
    {
        if (isOpening )
        {
            if (guiSkin!=null)
            {
                windowRect = GUI.Window(0, windowRect, WindowFunction, "Menu");
            }
        }
    }

    public void WindowFunction(int windowID){
        if (GUI.Button(new Rect(10, 60, windowRect.width - 20, 48), "Quest List"))
        {
            ShowQuestList();
        }
        else if (GUI.Button(new Rect(10, 110, windowRect.width - 20, 48), "Save Game"))
        {
            ShowSavePanel();
                                                                   
        }else if(GUI.Button(new Rect(10,150,windowRect.width-20,48),"Load Game")){
            LoadGame();

        }else if (GUI.Button(new Rect(10,160,windowRect.width-20,48),"Setting"))
        {
            ShowSetting();
        }else if (GUI.Button(new Rect(10,210,windowRect.width-20,48),"Quit"))
        {
            LevelManager.Destroy(this.gameObject);
        }
    }

    private void LoadGame()
    {
        //SaveSystem.LoadGame();
    }

    private void ShowSetting()
    {
        throw new NotImplementedException();
    }

    private void ShowSavePanel()
    {
        Debug.Log("Save Operation");
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            SaveSystem.SaveToSlot(1);
            Debug.Log("Save Success");
        }
    }

    private void ShowQuestList()
    {
        Debug.Log("Show Panel List U Have{0}To Do",null);

        //if ((SimpleQuestLogWinow!=null && DeatailQuestLogWindow!=null && ! ))
        //{
        //    DialogueManager.ShowAlert("Quest Log");
        //}
    }

    private void SetMenuStates(bool open)
    {
        isOpening = open;
        if (open)
        {
            windowRect = scaledRect.GetPixelRect();
            Time.timeScale = open ? 0 : 1;
                    }
        if (open)
        {
            onOpen.Invoke();
        }else{
            onClose.Invoke();
        }
    }

    /// <summary>
    /// Hide this instance.
    /// </summary>
    public override void Hide()
    {
        this.gameObject.SetActive(false);

    }

    /// <summary>
    /// Show this instance.
    /// </summary>
    public override void Show()
    {
        if (instance!=null && playerDatas!=null)
        {
            //soundManager.PlayingSound();
            this.gameObject.SetActive(false);
            instance.gameObject.SetActive(true);
        }
    }



}
