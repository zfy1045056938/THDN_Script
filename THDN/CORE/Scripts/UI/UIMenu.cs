using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using GameDataEditor;
using System.Reflection;
using System;
public class UIMenu : MonoBehaviour
{
    public NetworkManagerTHDN manager;

    public Button startBtn;
    public Button createBtn;
    public Button exitBtn;

    public Animator createPanel;
//    public CinemachineVirtualCamera camera;
    void Start()
    {
//        camera = GetComponent<CinemachineVirtualCamera>();
        manager.StartHost();
        //
        LoadGameItems();
    }

     private void Update() {
            startBtn.gameObject.SetActive(manager.selection!=-1);

            //
            createBtn.onClick.AddListener(()=>{

                createPanel.Play("Panel In");
            });

            //

            exitBtn.onClick.AddListener(()=>{
                Application.Quit();
            });
    }

    void LoadGameItems(){
        List<GDEItemsData> gitem = GDEDataManager.GetAllItems<GDEItemsData>();
        //
        for(int i=0;i<gitem.Count;i++){
            GDEItemsData gDE= new GDEItemsData(gitem[i].Key);
           

           
        }
    }

  
}
