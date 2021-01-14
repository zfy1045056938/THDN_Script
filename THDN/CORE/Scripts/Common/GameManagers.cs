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
    private NetworkManagerTHDN managerTHDN;

    public Board board;
    public static GameManagers instance;
    //Network Mpdule
    public Players[] playerList;

    
    public List<GamePiece> gp;
    //
    public int collectScore;
    public int requiredScore;
    public double LimitTime;
   
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

   

}
