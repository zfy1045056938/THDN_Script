using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Linq;


public class CreateCharacterUI : MonoBehaviour
{

    public Text nameText;
    public Dropdown classText;
    public NetworkManagerShenShan manager;
    
    //BTN CALL
    public Button createBtn;
    public Button cancelBtn;

    public GameObject panel;

    public static CreateCharacterUI instance;
    public SaveLoadCharacter savePanel;

    void Awake(){
        if(instance==null)instance=this;
    }

    private void Start()
    {
        
        savePanel.GetComponent<SaveSlots>();
    }


    // Update is called once per frame
    void Update()
    {

            classText.options = manager.playerClasses.Select(p=>new Dropdown.OptionData(p.name)).ToList();

        //
    
        //         Show();

        //         createBtn.interactable = manager.IsAllowedCharacterName(nameText.text);
        //         createBtn.onClick.AddListener(() =>
        //         {
        //             CharacterCreateMsg cmsg  = new CharacterCreateMsg
        //             {
        //                 name =nameText.text,
        //                 classIndex = classText.value,
        //             };
        //             //
        //             NetworkClient.Send(cmsg);
                  
        //             //
                    
        //             Hide();
        //             savePanel.Open();
                    
        //         });
                
                //
        //         createBtn.onClick.AddListener(() =>
        //         {
        //             panel.SetActive(false);
                   
        //         });
        //     }
        //     else
        //     {
        //         Hide();
        //     }
        // }
        // else
        // {
        //     Hide();
        // }
    }
    

    public void CreatePLayer(){
 if (panel.activeSelf)
        {
            if (manager.state == NetworkState.Lobby)
            {

                createBtn.interactable = manager.IsAllowedCharacterName(nameText.text);
               
                    CharacterCreateMsg cmsg  = new CharacterCreateMsg
                    {
                        name =nameText.text,
                        classIndex = classText.value,
                    };
                    //
                    NetworkClient.Send(cmsg);
                  
                    Hide();
                    savePanel.Open();
                
              
    }
        }
    }

    
    public void Show(){panel.SetActive(true);}
    public void Hide(){panel.SetActive(false);}


}
