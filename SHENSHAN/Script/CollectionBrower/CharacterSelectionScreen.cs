using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDataEditor;
using PixelCrushers;
using UnityEngine.UI;
using GameDataEditor;
using Mirror;
using TMPro;
public class CharacterSelectionScreen :MonoBehaviour
{
    public UIPanel  ScreenContent;
    public GameObject scObjs;
    public static CharacterSelectionScreen instance;
    public HeroInfoPanel heroPanel;
    public Transform porPos;
    // public PortraitMenu[] portraitMenu;
    public List<GameObject> portraitMenu;
    public Button selectBtn;
    public List<CharacterAsset> allCa;
    public Dictionary<string,CharacterAsset> characterDic =new Dictionary<string, CharacterAsset>();


    public GameObject prePanel;
    
    public GameObject preObj;
     public GameObject cardNameRibbon;
     public Transform preListPos;
     public Transform rebibbonPos;
    public List<GameObject> preList;
    public bool isShow;
    private void Awake()
    {
        instance = this;
        SetCharacterToDic();
        LoadCharacterSelect();
//       
    }
    void Update(){
        selectBtn.gameObject.SetActive(heroPanel.playerPortrait.characterAsset!=null);
    }

    public void SetCharacterToDic()
    {
	    List<GDECharacterAssetData> allc = GDEDataManager.GetAllItems<GDECharacterAssetData>();

	    for (int i = 0; i < allc.Count; i++)
	    {
		    CharacterAsset c = GetCharacterAsset(allc[i]);
		    allCa.Add(c);
	    }
	    
	    //
	    foreach (CharacterAsset c in allCa)
	    {
		    if (!characterDic.ContainsKey(c.className))
		    {
			    characterDic.Add(c.className,c);
		    }
	    }
	    
	    
    }

    public CharacterAsset GetCharacterByName(string cn)
    {
	    if (characterDic.ContainsKey(cn))
	    {
		    return characterDic[cn];
	    }

	    return null;
    }
  

    void LoadCharacterSelect(){
       
        //
        ClearObj();
      
        //
            List<GDECharacterAssetData> ad = GDEDataManager.GetAllItems<GDECharacterAssetData>();
            for(int i=0 ;i<ad.Count;i++){
                if(ad[i].ClassName!="中立"){
                    GameObject scObj = Instantiate(scObjs,porPos.position,Quaternion.identity) as GameObject;
                    scObj.transform.parent = porPos.transform;
                    scObj.GetComponent<PortraitMenu>().asset =GetCharacterAsset(ad[i]);
                    scObj.GetComponent<PlayerPortraitVisual>().portraitImage.sprite =Utils.CreateSprite(ad[i].AvatarImage);
                     scObj.GetComponent<PlayerPortraitVisual>().portraitBackgroundImage.sprite =Utils.CreateSprite(ad[i].BGSprite);
                      scObj.GetComponent<PlayerPortraitVisual>().healthText.text =ad[i].MaxHealth.ToString();
                    //   scObj.GetComponent<PlayerPortraitVisual>().detailText.text=ad[i].Detail.ToString();
                    //   scObj.GetComponent<PlayerPortraitVisual>()..text =ad[i].MaxHealth.ToString();
                   
                    portraitMenu.Add(scObj);
                }
            }
    }

      public CharacterAsset GetCharacterAsset(GDECharacterAssetData d){
        GDECharacterAssetData data= new GDECharacterAssetData(d.Key);
        return new CharacterAsset(Utils.GetPlayerJob(data.PlayersJob),data.ClassName,data.MaxHealth,data.PowerName,Utils.CreateSprite(data.AvatarImage),
        data.Detail,Utils.CreateSprite(data.BGSprite),data.AttackCard,data.ArmorCard);
    }

    public void ClearObj(){
        
			if (portraitMenu == null) {
				return;
			}

			var npcs = new List<GameObject>();
			var parentTransform = porPos.transform;
			for(int i = 0; i < parentTransform.childCount; i++) {
				var npc = porPos.transform.GetChild(i).gameObject;
				npcs.Add(npc);
			}

			foreach (var npc in npcs) {
				if (Application.isPlaying) {
					Destroy(npc);
				} else {
					DestroyImmediate(npc);
				}
			}
		
    }



    public void ShowPrePanel(string character){
        if(isShow==true){
       prePanel.SetActive(true);
        //
        List<GDEInitPackData> initPack = GDEDataManager.GetAllItems<GDEInitPackData>();
        for(int i=0;i<initPack.Count;i++){
            if(initPack[i].PackCharacter == TownManager.instance.GetCardCharacterAsset(character) ){
                //
                GameObject obj = Instantiate(preObj,preListPos.position,Quaternion.identity)as GameObject;
                obj.transform.SetParent(preListPos);
                obj.GetComponent<InitPackObj>().name = initPack[i].PackName;
                obj.GetComponent<InitPackObj>().cardList = new List<CardAsset>(TownManager.instance.ConvertCard(initPack[i].PackList));
                obj.GetComponent<InitPackObj>().objBtn.onClick.AddListener(()=>{
                        //Set To Pack List
                    var cardLists = TownManager.instance.ConvertCard(initPack[i].PackList);
                    if(cardLists.Count>0){
                        for(int j=0;j<cardLists.Count;j++){
                            //add card 
                            GameObject ob=Instantiate(cardNameRibbon,rebibbonPos.position,Quaternion.identity)as GameObject;
                            ob.transform.SetParent(rebibbonPos);
                            rebibbonPos.GetComponent<CardNameRibbon>().nameText.text = cardLists[i].name.ToString();
                            rebibbonPos.GetComponent<CardNameRibbon>().asset = cardLists[i];
                             
                            rebibbonPos.GetComponent<CardNameRibbon>().Quantity=1;

                            NetworkServer.Spawn(ob);
                            

                        }
                    }

                });
                
                
                NetworkServer.Spawn(obj);

                

            }
        }
        
        
        }

    }


  public void HideThePre(){
      isShow=false;
      PlayerPrefs.SetInt("PreCardPanel",0);
  }
  

    /// <summary>
    /// Shows the screen.
    /// </summary>
    public void ShowScreen(){
         LoadCharacterSelect();
        ScreenContent.gameObject.SetActive(true);
       
        foreach (GameObject p in portraitMenu)
        {
            if(p!=null){
            p.GetComponent<PortraitMenu>().Deselect();
            }
        }

       
        //CharacterSelectionScreen.instance.ScreenContent.gameObject.SetActive(true);
        //DeckSelectionScreen.instance.heroSelection.SelectCharacter(null);
        heroPanel.SelectCharacter(null);
    }

    public void ShowScreenWithDraft(){
       
        Debug.Log("SET DRAFT WITH TRUE");
        DeckBuilder.instance.draft=true;
          LoadCharacterSelect();
        ScreenContent.gameObject.SetActive(true);
       
        foreach (GameObject p in portraitMenu)
        {
            if(p!=null){
            p.GetComponent<PortraitMenu>().Deselect();
            }
        }
        heroPanel.SelectCharacter(null);

    }


    public void HideScreen(){
        ScreenContent.gameObject.SetActive(false);
    }

    public void DraftScreenOpen(){
        DeckBuilder.instance.InDeckBuildingMode=true;
      DeckBuilderScreen.instance.draft=true;
          LoadCharacterSelect();
        ScreenContent.gameObject.SetActive(true);
       
        foreach (GameObject p in portraitMenu)
        {
            if(p!=null){
            p.GetComponent<PortraitMenu>().Deselect();
            }
    }
    }
}