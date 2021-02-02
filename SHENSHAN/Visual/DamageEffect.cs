using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;




/// <summary>
/// Damage effect.
/// </summary>
public class DamageEffect : MonoBehaviour
{

    public Sprite[] splashes;

    //
    public Image damageImage;

    public CanvasGroup cg;

    public static DamageEffects des =DamageEffects.None;



    public Text amountText;
   
    public static OneCardManager manager;
   
    private void Awake()
    {
        damageImage.sprite = splashes[Random.Range(0, splashes.Length)];
        manager = GetComponent<OneCardManager>();
    }

    //迭代器显示伤害效果
    public IEnumerator ShowDamageEffect(){
        cg.alpha = 1f;
     
        while (cg.alpha > 0)
        {
            cg.alpha -= 0.1f;
            yield return new WaitForSeconds(0.3f);
        }
        Destroy(this.gameObject);
    }


    private bool isAtk;
    private bool isHurt;
    public static void ShowBuffEffect(Vector3 pos, SpellBuffType type ,int amount){
        if(amount==0)return;

        SoundManager.instance.PlaySound(GlobalSetting.instance.buffClip);
        //
        GameObject effect = Instantiate(GlobalSetting.instance.BuffPrefab,pos,Quaternion.identity)as GameObject;
        DamageEffect  d= effect.GetComponent<DamageEffect>();
        if(amount<0){
            d.amountText.text="-"+(amount).ToString();
            d.damageImage.color=Color.green;
        }  else{
            d.amountText.text="+"+(amount).ToString();
        } 

        d.StartCoroutine(d.ShowDamageEffect());

    }
    //创建伤害效果,
    public static void CreateDamageEffect(Vector3 position,int amount){
        if (amount ==0)
        {
            return;
        }

        //创建对象
        
        GameObject newDamaggeObject = Instantiate(GlobalSetting.instance.DamagePrefab, position, Quaternion.identity) as GameObject;
        //
        DamageEffect de = newDamaggeObject.GetComponent<DamageEffect>();
        
        if (amount<0)
        {
            de.amountText.text = "+" + (-amount).ToString();
            de.damageImage.color = Color.green;
        }
        else
        {
            de.amountText.text = "-" + amount.ToString();
        }
        
        //Sound
        SoundManager.instance.PlaySound(GlobalSetting.instance.atkClip);

        de.StartCoroutine(de.ShowDamageEffect());
    }

    


}
