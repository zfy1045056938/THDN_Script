using System.Collections;
using System.Collections.Generic;
using System.Linq;
// using DungeonArchitect.Navigation;
using PixelCrushers;
using UnityEngine;
using UnityEngine.UI;

public class DungeonConsoleUI : MonoBehaviour
{
//  private DungeonManager manager;
    public static DungeonConsoleUI instance;
    public UIPanel panel;
    public Text moneyText;
    public Text dustT;
    public Text expT;

    private int money;
    private int dust;
    private float exp;

    public GameObject itemObj;
    public Transform itemSlot;
    public bool isOpen = false;

    public Button closeBtn;

    // Start is called before the first frame update
    void Start()
    {
        // manager = FindObjectOfType<DungeonManager>();
        StartCoroutine(ShowConsole());
    }

    void Update()
    {
        closeBtn.onClick.AddListener(() =>
        {
        
            panel.Close();
        });
    }

    IEnumerator ShowConsole()
    {
        //
        // var target=FindObjectOfType<CharacterController2D>();
        // manager.dungeonCurrent++;
        // target.target=null;

        money = Mathf.RoundToInt(Random.Range(1, BattleStartInfo.SelectEnemyDeck.enemyAsset.moneyReward));
        ConsoleManager.MONEY += money;
        moneyText.text = money.ToString();
        yield return 0;
        dust = Mathf.RoundToInt(Random.Range(1, BattleStartInfo.SelectEnemyDeck.enemyAsset.dustReward));
        ConsoleManager.DUST += dust;
        dustT.text = money.ToString();
        yield return 0;
        exp = Random.Range(1, BattleStartInfo.SelectEnemyDeck.enemyAsset.exp);
        ConsoleManager.EXP += exp;
        expT.text = money.ToString();
        yield return 0;
        //
        if(Random.value<0.5)
        {
            var items = BattleStartInfo.SelectEnemyDeck.enemyAsset.rewardId.ToArray();
            if (items != null)
            {
                foreach (int itemID in items)
                {
                    Items item = ItemDatabase.instance.FindItem(itemID);
                    if (item != null)
                    {
                        InventorySystem.instance.AddItem(item);
                    }
                    Debug.Log("add To player inventory");
                    GameObject obj = Instantiate(itemObj,itemSlot.position,Quaternion.identity) as GameObject;

                    obj.transform.parent = itemSlot;
                    obj.GetComponent<ConsoleItems>().itemText.text = item.itemName;
                    obj.GetComponent<ConsoleItems>().numText.text = item.stackSize.ToString();
                }
            }
        }
        //
        yield return new WaitForSeconds(2.0f);
        closeBtn.gameObject.SetActive(true);

    }

   
}
