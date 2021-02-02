using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.Experimental.UIElements.Button;


//快速选项栏
public class MainGameUI : MonoBehaviour
{

    public GameObject StatsPanel;

    public GameObject InventoryPanel;

    public GameObject QuestPanel;

    public GameObject CollectionPanel;

    public static MainGameUI instance;
    
  

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
           StatsPanel.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryPanel.gameObject.SetActive(true);
        }else if (Input.GetKeyDown(KeyCode.L))
        {
            QuestPanel.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            CollectionPanel.gameObject.SetActive(true);
        }
    }
}
