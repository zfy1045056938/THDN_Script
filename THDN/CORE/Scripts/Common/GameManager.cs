using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Mirror;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.SequencerCommands;
using UnityEngine.UI;
using Cinemachine;

/// <summary>
/// For Thdn Match Module,
/// when player has battle request ,call GameManager::EnterBattle to load Battle Module
/// UIManager Validate exist the module
/// when has player and enemy ,load other component when it's finish start game
/// SoundManager &&Message&&stats
/// the quest system needs listen the handler util the battle over.
/// Turn manager listen turn when it 
/// </summary>
public class GameManager : Singleton<GameManager>
{
    public Board board;
   

    public bool IsGameover;
    public bool IsDealDamage=false;
    public NetworkManagerTHDN manager;
    
    public float timetest;
    public bool cando =false;
    public int TimesLeft;
    public bool isDead = false;
    public bool isReady=false;
    public bool canAtk;
    bool isReload;  //for next level or restart
    private bool readyReload = false;

    //Board entity
    private Players player;
    private Monster enemy;
    public Dictionary<string, Entity> pList = new Dictionary<string, Entity>();
    public int battleNum;

    public UIPanel Match3Panel;
    public Transform oriPos;
    public Button StartBtn;

    //Entity Panel load data to pa 
    public PlayerArea playerArea;
    public PlayerArea entityArea;

    //destory when leave dungeon
    [Header("Dungeon Collect ")]
    public int exploreRooms = -1;
    public int selectEvents = -1;
    public int partyNum = -1;
    public List<Entity> partyList;

    //when board load ,load both entity to bf with fsm state,when player matches
    // change state und start animation until target die
    // UpdateServer::FSM->state
    [Header("AI BF")]
    public CinemachineVirtualCamera battleCamera;

    void Start()
    {
       
        player = Players.localPlayer;

        board = FindObjectOfType<Board>().GetComponent<Board>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Players>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Monster>();

        //Add entiy to list at battlefield
        pList.Add(player.name,player);  //player
        pList.Add(player.target.name, enemy);   //enemy

        //Init the gameLogs
        GameDebug.Init(Application.dataPath,string.Format("BattleLogs"));
        //

//        if (player != null && player.target != null&& Match3Panel !=null)
//        {
//            OnGameStart();
//        }
        StartCoroutine("ExecuteGameLoop");
    }



void Update(){
        //Invoke Player Recovery 
        InvokeRepeating(player.Recover(), 1.0f, 1f);
        InvokeRepeating(enemy.Recover(), 1.0f, 1f);
        //



    }


    /// <summary>
    /// Start Game Core
    /// </summary>
    public void OnGameStart()
    {
        GameDebug.Log("MATCHES GAME INIT");
        StartCoroutine("ExecuteGameLoop");
//         ExecuteGameLoop();
    

    }
    
    int i=0;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator ExecuteGameLoop()
    {

        yield return StartCoroutine("LoadEntityAtBoard");
            yield return StartCoroutine("StartGameRoutine");
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine("PlayGameRoutine");
         
            //yield return StartCoroutine("DealDamageRoutine");
//            yield return StartCoroutine("NextTurnRoutine");
          
            yield return StartCoroutine("EndGameRoutine");
            
        

       
            
    }

    IEnumerator WaitForProcess(float delay=0f)
    {
        yield return new WaitForSeconds(delay);
    }
    
    IEnumerator SkillRoutine(){
        yield return null;
    }

    IEnumerator StartGameRoutine()
    {
        GameDebug.Log("MATCHES LOAD DONE START MATCHES");
        //Init
        if (UIManager.instance != null)

        {
            Debug.Log("================Match3 Module====================");
            if (UIManager.instance.message != null&& isReady!=true)
            {
                //Show MessagePanel

                Debug.Log("Show Message panel");
                UIManager.instance.message.ShowMessagePanel();


                //Init Part
                //Player
                if (UIManager.instance.player != null&& player!=null)
                {
                    UIManager.instance.player.Init();
                }

                if (UIManager.instance.enemy != null && player.target!=null)
                {
                    Entity e = null;
                    if (player.target is NPC npc)
                    {
                        player.target = npc;
                        e = npc;
                    }else if (player.target is Monster monster)
                    {
                        player.target = monster;
                        e = monster;
                    }
                    UIManager.instance.enemy.Init(e);
                }

                //Times
                if (UIManager.instance.turnManager != null)
                {
                    UIManager.instance.turnManager.InitTime();
                }

                //
                if (UIManager.instance.collectPool != null)
                {
                    UIManager.instance.collectPool.Init();
                }

              

            }
            else
            {

                Debug.LogWarning("Null");
            }
        }


        if (isReady == true)
        {

            if (board != null)
            {

                board.SetupBoard();
            }    
        }
        else
        {
            yield return null;
        }

        //Fade Off 
            //    yield return new WaitForSeconds(0.4f);
//           
            // Debug.Log("READY GAME");
            // //Setup Board
            // if (board != null && isReady==true)
            // {

            //     Debug.Log("Init Board");
            //     board.SetupBoard();
            // }
       
    }

 
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayGameRoutine()
    {
        
           
            if (isReady == true && UIManager.instance.turnManager != null)
            {

                UIManager.instance.turnManager.StartCountDown();

            }

            //while (!IsDealDamage)
            //{
            //    yield return null;
            //}

            yield return null;

    }
    
    /// <summary>
    /// Damage module
    /// skill>items>normaldamage>deadwish
    /// </summary>
    /// <returns></returns>
    IEnumerator DealDamageRoutine()
    {

        if (IsDealDamage == true)
        {
            float v=0f;
            board.m_playerInputEnabled=false;
            Debug.Log("==============Battle Module===============");
            if (UIManager.instance.player.CalDamage(v) > 0 || UIManager.instance.enemy.CalDamage() > 0)
            {

                //Check Spped who atk first
                if (player.speed >= player.target.speed)
                {
                    Debug.Log("Deal damage to target,player -> enemy");
                    //
                    float playerAmount = UIManager.instance.player.CalDamage(v);
                    float enemyAmount = UIManager.instance.enemy.CalDamage();

                    //player atk 
                    // StartCoroutine(DamageEnemy(player.target, playerAmount));
                    DamageEnemys(player.target, playerAmount);
                    Debug.Log("now enemy health and player health are" + player.target.health + "\t" + player.health);

                    if (player.health <= 0 || player.target.health <= 0)
                    {
                        IsDealDamage = false;
                    }
                    else
                    {

                        Debug.Log("enemy -> player");
                        player.target.DealDamageToTarget(player, enemyAmount);

                    }

                    //Update UI
                    UIManager.instance.enemy.UpdateStats();
                    UIManager.instance.player.UpdateStats();

                    //

                }
                else
                {
                    //Enemy first  
                    float enemyAmount = UIManager.instance.enemy.CalDamage();
                    StartCoroutine(DamagePlay(player, enemyAmount));

                }

                cando = true;

            }
            
        }
        else
        {
            yield return null;
        }


    }

    IEnumerator NextTurnRoutine()
    {
        Debug.Log("NExt turn");

        if (cando == true)
        {
            Debug.Log("==============================next turn" + i);
            // UIManager.instance.turnManager.NextRound();
            UIManager.instance.turnManager.ResetTime();
            board.ClearBoard();

            StartCoroutine(board.RefillRoutine());
            yield return new WaitForSeconds(1.0f);
            Debug.Log("NetworkTime for refill" + NetworkTime.time);
            cando = false;
            IsDealDamage = false;

            UIManager.instance.collectPool.Init();
            Debug.Log("Next turn return");

            StartCoroutine(PlayGameRoutine());

            while (player.health > 0)
            {
                yield return null;
            }

            Debug.Log("Dead");
        }
        else
        {
            
            yield return null;
        }


    }

    IEnumerator EndGameRoutine()
    {

        GameDebug.Log("MATCHES ENDS CHEC WINNER");
        
        //
        if (player.health>player.target.health)
        {
            UIManager.instance.WinnerPanel();
            Time.timeScale = 0;
        }
        else
        {
            UIManager.instance.LosePanel();
             Time.timeScale = 0;
        }
        yield return null;
    }
    
    
    public void UpdateMoves()
    {
        
    }
 
    public void BeginGame(){
         UIManager.instance.message.panel.gameObject.SetActive(false);
         Debug.Log("start game");
        isReady=true;
        OnGameStart();
    }
    public void Reload(){

    }

  public  void StartGame()
    {
        if (isReady == true && player != null && player.target != null)
        {
            OnGameStart();
        }
    }

  IEnumerator DamageEnemy(Entity entity, int amount)
  {
      if (entity.health <= 0)
      {
          yield return null;
      }
      player.DealDamageToTarget(entity,amount);
  }

  void DamageEnemys(Entity entity,float a){
      player.DealDamageToTarget(entity,a);
  }

  IEnumerator DamagePlay(Entity entity, float amount)
  {
      if (player.health <= 0)
      {
          yield return null;
          
      }
      player.target.DealDamageToTarget(player,amount);
  }

    #region Reset Board
    //clear battlelogs und cp data 
    public void ClearBoard() {


    }
    


    #endregion
}
