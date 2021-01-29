using UnityEngine;
using System.Collections;
using System.Linq;
using GameDataEditor;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Mirror;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ShenShanModule : MonoBehaviour
{

    private Players whoseTurn;
    private ItemDatabase itemDatabase;
    public GameObject SSObj;
    public GameObject scPrefab;
    public Transform scPos;
    public List<GameObject> cardList;
    public  TextMeshProUGUI selectnameText;
    public TextMeshProUGUI detailText;

    public int index = -1;
    public int currentRound = -1;
    


    // Use this for initialization
    void Start()
    {
        itemDatabase = FindObjectOfType<ItemDatabase>().GetComponent<ItemDatabase>();

    }

    // Update is called once per frame
    void Update()
    {
        whoseTurn = TurnManager.instance.WhoseTurn;
    }


    /// <summary>
    /// when player mana>= 7 then active shenshan module
    /// shen shan module active effect by dungeon event , it's will also effect for oppenent
    /// 
    /// </summary>
   public void Init(int round,int cardNum=3)
    {
        currentRound = round;
        //

        Debug.Log("============ShenShan Module Active");
        if (TurnManager.instance.WhoseTurn == GlobalSetting.instance.topPlayer)
        {
            //AI select
            int selectIndex = Random.Range(0, 2);
            DungeonEvent.instance.ShowDungeonEvent(round, cardNum,selectIndex);
        }else
        {
            //player select
            DungeonEvent.instance.ShowDungeonEvent(round, cardNum,-1);
        }
    }

    ///
    public void ShenShanModules(bool isSecond){
        var secondList= CardCollection.instance.allCardsArray.FindAll(ct=>ct.tags=="second").ToList();
        if(secondList.Count>0){
            //
            for(int i=0;i<secondList.Count;i++){
                GameObject obj = Instantiate(SSObj,scPos.position,Quaternion.identity)as GameObject;
                obj.transform.SetParent(scPos);
                //
                var dc =obj.GetComponent<DungeonCard>();
                obj.GetComponent<DungeonCard>().card = secondList[i];
                dc.nText.text = secondList[i].card.name.ToString();
                dc.ddText.text =secondList[i].card.cardDef.ToString();
                //TODO
                

                //

                cardList.Add(obj);

                //Network
                NetworkServer.Spawn(obj);
            }
        }
    }
}
