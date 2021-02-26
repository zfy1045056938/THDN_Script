using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using PixelCrushers.DialogueSystem;
using PixelCrushers.Wrappers;
using DG.Tweening;
using PixelCrushers.DialogueSystem.UnityGUI;
using UnityEditor;
using Mirror;
using Steamworks;
using UnityEngine.Rendering;
using Cinemachine;
using TMPro;


public enum ResoultionType
{
    window,
    FullScreen,
    
}
/// <summary>
/// 1.响应式UI
/// 2.获取玩家(STEAM)信息
/// 3.
/// </summary>
public class MenuManager : MonoBehaviour
{
    public GameObject panel;
    [Header("BaseInfo")]
    public PlayerData players;

    public GameObject content;
    public Camera playerCamera;
   
    
    
    [Header("SoundManager")]
    public SoundManager soundManager;

    [Header("System")]
    public UnityEvent onOpen = new UnityEvent();
    public UnityEvent onClose = new UnityEvent();
    private Rect windowRect = new Rect(0, 0, 500, 500);
    private ScaledRect scaledRect = ScaledRect.FromOrigin(ScaledRectAlignment.MiddleLeft,
        ScaledValue.FromPixelValue(300),
        ScaledValue.FromPixelValue(320));
   

    [Header("DataBind")]
    public Dictionary<int, PlayerData> player = new Dictionary<int, PlayerData>();

    public NetworkManagerShenShan networkManager;
    //private method
    private Transform initalPos;
    private List<PlayerData> playerDatas;
    private bool isOpening = false;

    [Header("InfoPanel")]
    public Text playerNameTxt;
    public CreatePanel newHeroPanel;
    public UIConfirmation confirmation;

    [Header("bool")]
     private bool showCreatepanel;
    public static MenuManager instance;

    public GameObject MainMenuUI;
    public GameObject MainGameUI;
    public GameObject menuPanel;
    public GameObject settingPanel;
    public GameObject selectCharacterPanel;
    public UIPanel worldMapPanel;
    public Transform menuPos;
    public Transform returnPos;
    public GameObject savePanel;
    public Transform selectPos;

    public GameObject[] startPos;
public GameObject createUI;
    public NetworkManagerShenShan manager;
   
    //
   

    
    
    [Header("Btn")]
    
     public Button LoadBtn;
    public Button deleBtn;
    public Button playBtn;
    public Button QuitBtn;

    [Header("Loc UI")]
    public UILocalizationManager gtt;
    public TextMeshProUGUI playText;
    public TextMeshProUGUI loadText;
    public TextMeshProUGUI exitText;

    public TextMeshProUGUI versionText;
    public static string GameLanaguage;


    public  void Awake()
    {
     
        instance = this;
        Debug.Log("Check Language");
        DialogueManager.SetLanguage("en");
        
        Debug.Log(Application.systemLanguage+"\thas check");
      
        //Start Host();
        StartCoroutine(HostRoutine());

        //
       
        //Set Screen
        Screen.SetResolution(Screen.currentResolution.width,Screen.currentResolution.height,false);
           Application.targetFrameRate =60;

           //Check isfree
           if(TownManager.instance.isFree==true){
               versionText.text=Application.version+" BY (LYNCHHEAD) 友情呈现版";

           }else{
                 versionText.text=Application.version+" BY (LYNCHHEAD)";
           }

           UpdateTextForLanguage();
    }
    
    
    
    public void SelectPanel(){
        selectCharacterPanel.transform.DOMoveY(100.0f,1.0f);
        Debug.Log("Show Panel");
    }

        void UpdateTextForLanguage(){
            Debug.Log("Update Lanaguage"+PlayerPrefs.GetString("Language"));
            string cl = "";
           if(PlayerPrefs.HasKey("Language")){
               cl = PlayerPrefs.GetString("Language");
               DialogueManager.SetLanguage(cl);
           }else{
               //Default
               DialogueManager.SetLanguage("zh");
               cl = "zh";
           }

           TownManager.currentLanguage = cl;

           Debug.Log("current lanaguage ist "+TownManager.currentLanguage);

        }
    public IEnumerator HostRoutine(){

        //Show Panel
        // SelectPanel();
        //Start Host
        networkManager.StartHost();

        //



        yield return new WaitForSeconds(0.4f);
    }

    /// <summary>
    /// 创建人物
    /// </summary>
    private void ShowGeneratePanel()
    {
        newHeroPanel.gameObject.SetActive(true);
    }
  


public void ChangeCamera(){
//  blendListCamera.
}
   
  public void SetLanguage(string cl){

      DialogueManager.SetLanguage(cl);
  }
    
    
    public void DeletePlayer(){
        if(PlayerPrefs.HasKey("PlayerName")){
            PlayerPrefs.DeleteAll();
            Debug.Log("ClearDone");
            Application.Quit();
        }
    }

    public void SetMenuStates(bool open)
    {
        isOpening = open;
        if (open)
            windowRect = scaledRect.GetPixelRect();
            Time.timeScale = open ? 0 : 1;
                    
        if (open)
        {
            onOpen.Invoke();
        }else{
            onClose.Invoke();
        }
    }

  

    public void ShowMenuPanel()
    {

          
            menuPanel.gameObject.SetActive(true);
            menuPanel.gameObject.transform.DOMove(menuPos.transform.position, 0.4f);
       


    }

    public void ShowSettingPanel()
    {
        menuPanel.gameObject.SetActive(false);
        settingPanel.gameObject.SetActive(true);
        
    }

    public void HideMenuPanel()
    {
        menuPanel.gameObject.SetActive(false);
        menuPanel.gameObject.transform.DOMove(returnPos.transform.position, 0.4f);
    }
    /// <summary>
    /// 
    /// </summary>
       public  void StartGame(){    
            
                
            Debug.Log("Server Load Over");
        
            if(NetworkClient.isConnected  && PlayerPrefs.HasKey("PlayerName")){
               
             if(players!=null){
                TownManager.instance.ShowTown(true);
             }
             
            }else{
                Debug.Log("No Character Exist");
              createUI.gameObject.SetActive(true);
            }
           
            
    }



    public void ClearPS()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Delete Clear");
    }
 
   
}
