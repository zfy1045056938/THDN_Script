using System.Collections;
using System.Collections.Generic;
using Mirror;
using PixelCrushers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
// using Michsky.LSS;
using System;

public class UIStartPanel : MonoBehaviour
{
    public NetworkManagerTHDN manager;
    // public LoadingScreenManager lsmanager;
    public GameObject panel;
    public Button StartBtn;
    public Button createBtn;
    public Button QuitBtn;
    public Button continuteBtn;
    public int index;

     private void Start() {
         manager.StartHost();
         //
        
    }

    private void CheckCharacter()
    {
        if(manager.state==NetworkState.Lobby && manager.charactersAvailableMsg!=null){
        CharacterAvailableMsg.CharacterPreview [] cs = manager.charactersAvailableMsg.characters;
        if(cs.Length>0){
            continuteBtn.interactable=true;
            index =0;
        }
        }else{
            Debug.Log("No Connect");
        }

            }

    void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject()){
            return;
        }
//        if (panel.activeSelf)
//        {
//            if (manager.state == NetworkState.Lobby && manager.charactersAvailableMsg != null)
//            {
//                CharacterAvailableMsg.CharacterPreview[] cs = manager.charactersAvailableMsg.characters;
//                
//                StartBtn.gameObject.SetActive(manager.selection!=-1);
//                
//                
//                StartBtn.onClick.AddListener(() =>
//                {
//                    ClientScene.Ready(NetworkClient.connection);
//                    //
//                    NetworkClient.connection.Send(new CharacterSelectMsg {index = manager.selection});
//                    //
//                    manager.ClearPreviews();
//                    //
//                    panel.SetActive(false);
//                    
//                });
//            }
//
//        }
// StartBtn.gameObject.SetActive(manager.selection!=-1);
// continuteBtn.gameObject.SetActive(manager.selection!=-1);
    }

    public void StartGame()
    {
        if (manager.state == NetworkState.Lobby && manager.charactersAvailableMsg != null)
        {
            //
            CharacterAvailableMsg.CharacterPreview[] cs = manager.charactersAvailableMsg.characters;
            //
            StartBtn.gameObject.SetActive(manager.selection!=-1);

            ClientScene.Ready(NetworkClient.connection);
            //
            NetworkClient.connection.Send(new CharacterSelectMsg {index = 0});
            //
            manager.ClearPreviews();
            //
            panel.SetActive(false);
            //
            Debug.Log("Load Scene");
            
            // lsmanager.LoadScene("MainGame");
            Camera.main.gameObject.SetActive(false);
            //
        }
    }

}
