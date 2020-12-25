using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vItemManager;
using Invector;
using InnerDriveStudios.DiceCreator;
public enum ChestType{
    Normal,
    Lockpick,
    SItem,
}
public class DungeonChest : MonoBehaviour
{

    public GameObject panel;
    public vItemCollection itemCollection;
    public ChestType chestType = ChestType.Normal;
    public DieCollection dieCollection;
    public bool hasOpen=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)&&panel.activeSelf){
                dieCollection.Roll();
            
        }
    }

    public void StartCheck(){
        panel.gameObject.SetActive(true);
    }
}
