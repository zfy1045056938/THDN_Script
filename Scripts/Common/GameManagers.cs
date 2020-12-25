using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
//using GDEGameEditor;

public enum Loctype :int
{
    None,
    Town,
    Map,
    Dungeon,
    Camp
}


///For Matches Config
///GM Manager the elements 
///1.Time Limit
///2.Matrix Config()
///
public class GameManagers : MonoBehaviour
{
    public NetworkManagerTHDN managerTHDN;

    public Board board;
    public static GameManagers instance;
    //Network Mpdule
    public Players[] playerList;

    public bool atDungeon = false;
    [Header("Properties")]
    public bool inBattle;
    public int gameTime;
    public GameManager gm;
    public bool CanMoveNextRoom = false;
    public bool isFinalRoom = false;
    public List<GamePiece> gp;
    //
    public int collectScore;
    public int requiredScore;
    public double LimitTime;
    public int currentRooms = -1;
    public bool isFinalRooms = false;
    public bool isFree = false; //Check demo

    //destory when leave dungeon
    [Header("Dungeon Collect ")]
    public int exploreRooms = -1;
    public int selectEvents = -1;
    public int partyNum = -1;
    public List<Entity> partyList;
    
    

     void Awake()
    {
        if(instance==null)instance=this;
        board = GameObject.FindObjectOfType<Board>().GetComponent<Board>();
    }
    void Start(){
//        managerTHDN.StartHost();
        //
        StartCoroutine("ExecuteGameLoop");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator ExecuteGameLoop()
    {

        yield return StartCoroutine("StartGameRoutine");
        //yield return StartCoroutine("PlayGameRoutine");
        //yield return StartCoroutine("WaitForRoutine", 0.5f);
        //yield return StartCoroutine("EndGameRoutine");
      
    }

  

    public IEnumerator StartGameRoutine()
         {
             if (board != null)
             {
                 board.SetupBoard();
             }
             yield return new WaitForSeconds(0.4f);
         }
    public IEnumerator PlayGameRoutine()
    {
        yield return new WaitForSeconds(0.4f);
    }
    public IEnumerator WaitForRoutine(float delay)
    {
        yield return new WaitForSeconds(0.4f);
    }
    public IEnumerator EndGameRoutine()
    {
        yield return new WaitForSeconds(0.4f);
    }

    /// <summary>
    /// for both health<=0
    /// </summary>
    public void SpecialMatches()
    {

    }


}
