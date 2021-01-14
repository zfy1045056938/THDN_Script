using System.Collections;
using System.Collections.Generic;
using Mirror.Examples.Additive;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyDeckSelection : MonoBehaviour
{
    		public GameObject screenContent;	//info
			public GameObject deckIcon;
        		public List<GameObject> deckIcons;
        	
				public Transform contentPos;
                public EnemyPanel enemySelectPanel;
    public static EnemyDeckSelection instance;

    [Header("BTN")] public Button StartBtn;
  
    public Button CancelBtn;

        	public  void Awake()
            {

	            if (instance == null) instance = this;

            }


            public void Update()
            {
	            if (PlayerData.localPlayer)
	            {
		            if (screenContent.activeSelf)
		            {
			            StartBtn.interactable=(BattleStartInfo.SelectEnemyDeck != null);
						
			            CancelBtn.onClick.AddListener(() =>
			            {
				            screenContent.SetActive(false);
			            });
		            }
	            }
            }

            /// <summary>
    /// Hides the screen.
    /// </summary>
    private void HideScreen()
    {
	    foreach (GameObject obj in deckIcons)
	    {
		    obj.GetComponent<DeckIcon>().DeSelect();
	    }

	    enemySelectPanel.selectedDeck = null;
        screenContent.gameObject.SetActive(false);
    }
            
            
            
    public void HideTheAsset(){
        enemySelectPanel.GetComponent<EnemyPanel>().portrait.gameObject.SetActive(false);
         enemySelectPanel.GetComponent<EnemyPanel>().portrait.enemyAsset=null;
    }


    /// <summary>
    /// Shows the screen.
    /// </summary>
	public void ShowScreen(){
        screenContent.gameObject.SetActive(true);
        foreach (GameObject obj in deckIcons)
        {
	        if (obj != null)
	        {
		        
			        obj.GetComponent<EnemyIcon>().InstantDeselect();
		        
	        }
        }
       
        enemySelectPanel.selectedDeck = null;

        }

    public void OpenConfigPanel()
    {
       
	    BattleConfigManager.instance.panel.Open();
	    
    }
  

}
