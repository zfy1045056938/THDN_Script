using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class CreatePanel:Singleton<CreatePanel>
{
    public GameObject createPanel;
    public Text playerName;
    public Players pData;


    private void Awake()
    {
        pData = GameObject.FindGameObjectWithTag("Player").GetComponent<Players>();
    }


    public void ApplyBtn(){
        PlayerPrefs.SetString("PlayerName", playerName.text);
        // PlayerPrefs.SetInt("PlayerID", pData.);
        createPanel.gameObject.SetActive(false);

    }
}