
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

///<summary>
///选择卡组界面包含以下信息
/// 1.characterslot
/// 2.castle info(castle+worker)
///	3.Mission Panel
///</summary>
public class DeckIcon : MonoBehaviour,IPointerClickHandler
{
    public Text DeckNameText;
    public GameObject packdidnotComplete;		//didn't finish pack
    public PlayerPortraitVisual portiait;  //
    public PlayerPortraitVisual infoPanel;
 
    private float initalScale=0.5f;					//default position
    private float targetScale = 1.1f;       //
    private bool selected = false;				//have select
    
    public DeckInfo DeckInformation ;  //pack info show right deck
	
    //Storge Enemy Data
  
    public static DeckIcon instance;

    void Awake()
    {
	    instance = this;
        portiait = GetComponent<PlayerPortraitVisual>();
     
        initalScale = transform.localScale.x;
        
        
    }

    public void ApplyLookForIcon(DeckInfo info)
    {
	  
        DeckInformation = info;

        //Show Pack who not complete
        packdidnotComplete.gameObject.SetActive(!info.IsComplete());

        portiait.portraitImage.sprite = info.characterAsset.avatarImage;
       portiait.portraitBackgroundImage.sprite=info.characterAsset.avatarBGImage;
        // portraitBackgroundImage.color = characterAsset.avatarBGTint;

       portiait.healthText.text =info.characterAsset.maxHealth.ToString();
       //
//        portiait.characterAsset =new CharacterAsset(
//	        
//	        );
//        portiait.ApplyLookFromAsset();
        DeckNameText.text = info.deckName;
        //
    }

 


			//撤销选择返回图标大小
			public void DeSelect(){
        transform.DOScale(initalScale,0.5f);
				selected=false;
			}

			//初始选择
			public void InstantDeSelect(){
             transform.localScale = Vector3.one;
				selected=false;
			}


            public void OnPointerClick(PointerEventData eventData)
            {
              
              if(eventData.button ==PointerEventData.InputButton.Left){
	            if (!selected)
	            {
                    Debug.Log("Select");
		            selected = true;
     
			            transform.DOScale(targetScale, 0.1f);   

           DeckSelectionScreen.instance.heroSelection.SelectDeck(this);
           //
           SoundManager.instance.PlayRndMusic();
		            DeckIcon[] allHero =FindObjectsOfType<DeckIcon>();
		            foreach (DeckIcon info in allHero)
		            {
			            if (info != this) info.DeSelect();  //
		            }

                }else{
                    
                    DeSelect();
                    DeckSelectionScreen.instance.heroSelection.SelectDeck(null);
                }
                }
            }

                
	          
            
}