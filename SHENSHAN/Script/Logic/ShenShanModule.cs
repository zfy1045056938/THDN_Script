using UnityEngine;
using System.Collections;
using System.Linq;
using GameDataEditor;
using System.Collections.Generic;
using UnityEngine.EventSystems;


[System.Serializable]
public class ShenShanModule : MonoBehaviour
{

    private Players whoseTurn;
    private ItemDatabase itemDatabase;
    public GameObject SSObj;
    public GameObject scPrefab;
    public Transform scPos;
    public List<GameObject> cardList;
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
    /// 
    /// </summary>
   public void Init(int round,int cardNum=3)
    {
        currentRound = round;
        //
        while (cardList.Count>=3)
        {
            GameObject obj = Instantiate(scPrefab, scPos.position, Quaternion.identity) as GameObject;

            obj.transform.SetParent(scPos);
            //
            obj.GetComponent<DungeonCard>().dec =
        }
        
    }
}
