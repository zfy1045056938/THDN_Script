using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vItemManager;
using Invector;
using Invector.Utils;
using InnerDriveStudios.DiceCreator;
using UnityEngine.UI;
public enum ChestType{
    Normal,
    Lockpick,
    SItem,
}

public enum UnlockType{
    Normal,
    Anti,
    Key,
    Skill,
    Items,
}


//3type for chest 
//1.normal chest when interactive then open
//2.item ,needs items before that open
//3. skill&item ,hyper item inculdes when open ,the items must higher than normal 
//und low perc epic 
public class DungeonChest : MonoBehaviour
{

    public GameObject panel;
    private Players players;
    
    public UnlockType unlockType = UnlockType.Normal;
    private vItemManager itemManager;
    public vContainsItemTrigger needsItem;

    public vItemCollection itemCollection;
    public ChestType chestType = ChestType.Normal;
    public DieCollection dieCollection;
    [vEditorToolbar("Item Check")]
    public int itemNum;
    public int skillLevel;
    public Text currentText;
    public Text needText;
    public Image reIcon;
    
    //
    [vEditorToolbar("Level Check")]
    public Text openPercText;
    public Text currentLevelText;
    public Text needLevelText;
    public float openPerc = 0f;

    public bool hasOpen=false;
    public bool hasItem=false;
    public bool hasSkill=false;


    public Color enoughColor =Color.green;
    public Color notenoughColor =Color.red;

    // Start is called before the first frame update
    void Start()
    {
        players = Players.localPlayer;
        itemManager = FindObjectOfType<vItemManager>();
    }

    // Update is called once per frame
    void Update()
    {
     
     if(hasItem==true){
         ShowTips();
     }

    }

    public void StartCheck(){
        panel.gameObject.SetActive(true);
    }

    public void ShowTips(){
        //got item by id
        vItem gotItem = itemManager.GetItem(needsItem.itemID);
        if(gotItem!=null){
            //show current und needs
            currentText.text = gotItem.amount.ToString();
            needText.text = itemNum.ToString();
            reIcon.sprite =Util.CreateSprite(gotItem.iconName);
            //level
            if(hasSkill==true){
            currentLevelText.text =  players.lockpick.ToString();
            needLevelText.text = skillLevel.ToString();
            }
        }
    }


    //check skill before
    public bool ReachSkill(Player p , int required,  ){
        return p.lockpick >= required; 
    }

    

    
}
