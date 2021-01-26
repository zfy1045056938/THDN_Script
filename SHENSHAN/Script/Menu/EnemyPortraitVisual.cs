using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using EmeraldAI;
using TMPro;

[System.Serializable]
public class EnemyRewardInfo
{
    public string itemID;
    public float pros = 0.25f;
    public ItemDatabase database = new ItemDatabase();
    public EnemyRewardInfo(string itemID, float porp)
    {
        this.itemID = database.GetItemid(itemID);
        this.pros = porp;

    }
}
public class EnemyPortraitVisual : MonoBehaviour
{
    public EnemyAsset enemyAsset;
    [Header("UI")] public Text healthText;
    public Image EnemyHead;
    public Image EnemyPower;
    public Image EnemyDefImage;
    public Text EnemyDef;
    public Text EnemyName;

    public Text health;

    public Text damage;

    public Text armor;

    public Text str;
    public Text dex;
    public Text inte;
    public Text flashPerc;
    public Text esd;
   

    public Text fr;
    
    public Text ir;
    public Text pr;
    public Text er;
    [Header("Enemy CardList")] public  List<CardAsset> enemyCardList = new List<CardAsset>();
    public static EnemyPortraitVisual instance;
    

    void Awake()
    {
        if (instance == null) instance = this;
       
       
    }

    void Start()
    {
      
        if (enemyAsset != null)
        {
            ReadFromEnemyAsset();
        }
        
       


    }


    //Battle Scene Load Asset
    public void ReadFromEnemyAsset()
    {
        
        healthText.text =enemyAsset.Health.ToString();
        EnemyName.text = enemyAsset.EnemyName;
        EnemyHead.sprite = enemyAsset.Head;
    
        
        
    }

public void SetInfo(EnemyAsset e){
     if(e!=null){
           EnemyName.text =e.EnemyName.ToString();
    
            //dex.text= e.dex.ToString();
            //inte.text =e.inte.ToString();
            //str.text =e.str.ToString();


            ir.text=e.ir.ToString();
            fr.text=e.fr.ToString();
            pr.text=e.pr.ToString();
            er.text=e.er.ToString();

            // damage.text=e.damage.ToString();
            // armor.text=e.def.ToString();
            // healthText.text = e.Health.ToString();


            flashPerc.text=Mathf.FloorToInt(e.flashPerc * 100)+"%";
           esd.text=Mathf.FloorToInt(e.extraSpellDamage*100)+"%";
        }
}
   
    public void TakeDamage(int amount, int healthAfter)
    {
        if (amount > 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, amount);
            healthText.text = healthAfter.ToString();
        }
    }

    public void Explode()
    {
        Instantiate(GlobalSetting.instance.ExplosionPrefab, transform.position, Quaternion.identity);
        Sequence s = DOTween.Sequence();
        s.PrependInterval(2f);
        s.OnComplete(() => GlobalSetting.instance.GameOverPanel.SetActive(true));
    }
}
