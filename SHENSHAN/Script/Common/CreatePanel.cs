using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using Mirror;

public class CreatePanel:Singleton<CreatePanel>
{
    public GameObject createPanel;
    public Text playerName;
    // public PlayerData pData;
    public Image[] imageGroup;

    private NetworkManagerTCG manager;


    private void Awake()
    {
        // pData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
    }


    /// <summary>
    /// 创建角色
    /// </summary>
    public void ApplyBtn(){
        if (playerName !=null){
            CharacterCreateMsg msg = new CharacterCreateMsg{
                name=playerName.text,
                 
            };
            PlayerPrefs.SetString("PlayerName",msg.name);
            //Set Msg Handler
            NetworkClient.Send(msg);
           
           this.gameObject.SetActive(true);
            new ShowMessageCommand("Create Success",1.0f).AddToQueue();

        }else{
            new ShowMessageCommand("Name Not NULL",1.0f).AddToQueue();
           return;

        }

        // TownManager.instance.ShowTown(true,);
    }

            
    // PlayerPrefs.SetString("PlayerName", playerName.text);
    //     PlayerPrefs.SetInt("PlayerID", pData.PlayerID);
            
    //     //
    //     pData.PlayerLevel   += 1;
    //         pData.money +=1000;
    //             pData.PlayerGem +=0;
    //                 pData.SpItem +=0;
    //                     pData.dust +=0;
           
    //                     Debug.Log("Create Success name" + pData.name+"\t money" +pData.money);
    //     createPanel.gameObject.SetActive(false);
    //     }
    //     else
    //     {
    //         new ShowMessageCommand("U Need Enter Name",0.4f).AddToQueue();
    //         return;
    //     }
       

    
}