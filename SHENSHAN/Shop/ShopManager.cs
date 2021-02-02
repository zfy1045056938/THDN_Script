using UnityEngine;
using System.Collections.Generic;
using ChuMeng;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;



public enum ShopItemType{
    None,
    Packs,
    Skin,

    Special,
}
/// <summary>
/// Shop manager INCLUDE THESE CONTENT
/// 1.ITEMS PAY BY COINS DIFFERENT OF MERCHANT (GOLD)
/// 2. PACK(SERIES)
/// 3. PACK MANAGER ,PACK(5)&&INCLUDE 5 CARDS/1PACK(3NORMAL2NONORMAL)
/// </summary>
public class ShopManager : Singleton<ShopManager>
{
    public GameObject ScreenContent;
    public GameObject PackPrefab;
    private int PackInfo;

    public PlayerData player;

    public Vector3 PacksParent =new Vector3();

    public Transform InitialPackSpot;

    //
    public float PosXRange = 4f;
    public float PosYRange = 8f;

    public float RotationRange = 10f;

    private ShopItemType shopItemType = ShopItemType.Packs;   // 区分商品种类
    public int PackPrice;
    //打开包区域
    public PackOpeningArea packOpeningArea;

    //
    public int startAmountOfMoney = 1000;
    public int startAmountOfDust = 1000;
    public int metalNumber = 0;

    public List<ChapterConfigData> data;
  

    public int PacksCreated { get; set; }

    public float packPlacementOffset = -0.01f;


    //
    public int existingPackCount = 0;
    public bool isActive = false;
    // private GDEPlayerInfoData pd;

  
    public override void Awake()
    {
     
        player = GameObject.FindObjectOfType<PlayerData>();

        HideScreen();
        if (PlayerPrefs.HasKey("UnOpenedPack"))
        {
            StartCoroutine(GivePack(PlayerPrefs.GetInt("UnOpenedPacks"), true));
        }

        

    }

    public void Update()
    {
        
    }

    #region resources 
    private int money;
    public int Money
    {
        get
        {
            return money;
        }
        set
        {
           
            money = value;
            // moneyText.text = pd.PlayerMoney.ToString();
        }
    }
    private int dust;
    public int Dust
    {
        get
        {
            return dust;
        }
        set
        {
            dust = value;
            // DustText.text =pd.PlayerDust.ToString();
        }
    }
    private int metal;
    public int Metal
    {
        get { return metal; }
        set
        {
            metal = value;
            //metalText.text = player..ToString();
        }
    }

    #endregion
    public void BuyPack()
    {

        if (shopItemType == ShopItemType.Packs)
        {
            if (Money >= PackPrice)
            {
                Debug.Log("Buy Pack");
                StartCoroutine(GivePack(1));
            }

        }

    }


    /// <summary>
    /// Gives the pack.
    /// </summary>
    /// <returns>The pack.</returns>
    /// <param name="NumberOfPacks">Number of packs.</param>
    /// <param name="instant">If set to <c>true</c> instant.</param>
    public IEnumerator GivePack(int NumberOfPacks, bool instant = false)
    {
        for (int i = 0; i <= NumberOfPacks; i++)
        {
            GameObject g = Instantiate(PackPrefab, PacksParent,Quaternion.identity) as GameObject;
            Debug.Log("Generate Packs"+g.name);
            Vector3 localPositionForNewPack = new Vector3(Random.Range(-PosXRange, PosXRange),
            Random.Range(-PosYRange, PosYRange), PacksCreated * packPlacementOffset);
            //
            g.transform.localEulerAngles =
            new Vector3(0f, 0f, Random.Range(-RotationRange, RotationRange));
            PacksCreated++;

            //
            g.GetComponentInChildren<Canvas>().sortingOrder = PacksCreated;
            //
            if (instant)
            {
                g.transform.localPosition = localPositionForNewPack;
            }
            else
            {
                g.transform.position = InitialPackSpot.position;
                g.transform.DOLocalMove(localPositionForNewPack, 0.5f);
                yield return new WaitForSeconds(0.5f);
            }

        }
        yield break;

    }


    /// <summary>
    /// 将用户数据保存进持久层
    /// </summary>
    public void SaveDustAndMoneyToPlayerPrefs()
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.SetInt("Dust", dust);
        PlayerPrefs.SetInt("Metal", metal);
    }

    /// <summary>
    /// Shows the screen.
    /// </summary>
    public void ShowScreen()
    {
        if (ScreenContent == null)
        {
            return;
        }
        ScreenContent.SetActive(true);
     
    }
    /// <summary>
    /// Hides the screen.
    /// </summary>
    public void HideScreen()
    {
        ScreenContent.SetActive(false);
        this.gameObject.SetActive(true);
    }

   
       

    
}
