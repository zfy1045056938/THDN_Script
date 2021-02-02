using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


/// <summary>
/// 对工人资产的管理，包含以下功能
/// 1.对工人属性的管理
/// 2.获取工人资产
/// 3.工人技能 
/// 4.工人生产效率计算
/// </summary>
public class WorkerManager:MonoBehaviour
{

    public static WorkerManager instance;
    public List<WorkerManager> workers = new List<WorkerManager>();
    [HideInInspector]
    public GameObject workerPrefab;
    [SerializeField]
    protected int workerHp;
    [SerializeField]
    protected int workerAtk;
    [SerializeField]
    protected int workerGenerateCrystals = 1;
    [SerializeField]
    protected int workerSpeed = 1;
    public List<int> workerList = new List<int>();
    public int goldMountain = 1;
    private WorkerType workerType = WorkerType.None;    //工人类型
    private Players playerInGame ;

    [Header("worker image")]
    public Image workerImage;
    public Image workerBG;
    public Image goldMountainsImage;
    public Image workerTools;


    [Header("worker list")]
    public Dictionary<string, WorkerManager> workDic = new Dictionary<string, WorkerManager>();
    public ManaPoolVisual manaPoolVisual;
    public List<int> mountainList = new List<int>();
   // public List<Worker> workerList = new List<Worker>();


    private Vector3 initalPosition = Vector3.one;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GenerateCrystals();
        }
    }

    /// <summary>
    /// 获取金山数目,工人生产水晶
    /// 1.数目=(worker*exists(buff))*gold
    /// 2.the first round,workertype generate number= (workerType)*workergenerate
    /// TODO
    /// </summary>
    public void GenerateCrystals(){
        bool isGenerate = false;

        for (int i = 0; i < mountainList.Count; i++)
        {
            for (int j = 0; j < workers.Count; j++)
            {
                if (workers.Count ==0 || mountainList.Count==0)
                {
                    isGenerate = false;
                    workerGenerateCrystals+=1;
                    //playerInGame.Crystals = workerGenerateCrystals;
                    //


                }
                //worker genreate crystals
                if (workerType  ==WorkerType.AtkWorker&&!isGenerate)
                {
                    workers[j].workerGenerateCrystals +=2;
                    //if player mana > 2 then use worker skill to go face
                    if (playerInGame.manaLeft >=2 )
                    {
                        GoFace(true);

                    }
                }
                else if (workerType ==WorkerType.DefWorker && !isGenerate)
                {
                    

                }
                else if (workerType ==WorkerType.Monk && !isGenerate)
                {
                    
                }
            }
        }
    }
    
    public void GoFace(bool isAtk){}


}

