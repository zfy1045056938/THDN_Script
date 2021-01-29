using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;


/// <summary>
/// 随从攻击指令需要判断以下条件
/// 1.是否为当前回合
/// 2.是否可以进行攻击指令(effeect,states(if freeze then can't move jkkbvhjj))
/// 3.判断攻击行为（单体还是群体还是特殊攻击效果)
/// 4.当场面有仇恨随从,优先攻击攻击
/// 5.controller
/// </summary>
public  class CreatureAtkVisual :MonoBehaviour
{
    private OneCreatureManager manager;
    private WhereIsTheCardOfCreature whereIsTheCreature;
    //public CardEffects effect = CardEffects.None; 
    private void Awake()
    {
        manager = GetComponent<OneCreatureManager>();
        //
        whereIsTheCreature = GetComponent<WhereIsTheCardOfCreature>();
    }


    /// <summary>
    /// Attacks the target.
    /// </summary>
    public void AttackTargetToObject(int targetUniqueID,
                                    int damageTakenByTarget,
                                    int damageTakenByAttack,
                                    int attackHealthAfter,
                                    int targetHealthAfter,
                                    int attackArmorAfter,
                                    int targetArmorAfter,
                                     CardEffects cardEffects=CardEffects.None){
 
       
        
       
        manager.CanAtkNow = false;
        //Get Target ID
        GameObject target = IDHolder.GetComponentWithID(targetUniqueID);
        
        whereIsTheCreature.BringToFront();
        //VisualStates tmpState = VisualStates.Transition;
        VisualStates tmpState = tag.Contains("Low") ? VisualStates.LowTable : VisualStates.TopTable;
        whereIsTheCreature.visualState = VisualStates.Transition;
        
        //
//        SoundManager.instance.PlayeCombatSound();
        //
        transform.DOMove(target.transform.position, 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InCubic).OnComplete(() =>
        {
            //Get particle System
//           GameObject fbx = GameObject.Instantiate(GlobalSetting.instance.fbxObj,transform.position,Quaternion.identity) as GameObject;
//          fbx.GetComponent<ParticleSystem>().Play();
//          
           

           //Damege popup
            if (damageTakenByTarget>0)
            {
                DamageEffect.CreateDamageEffect(target.transform.position, damageTakenByTarget);

            }
            if (damageTakenByAttack > 0)
           
            {
                DamageEffect.CreateDamageEffect(transform.position, damageTakenByTarget);
            }
            
        //    if(targetArmorAfter <=0 || attackArmorAfter <=0)
        //    {
        //        GetComponent<OneCreatureManager>().defText.gameObject.SetActive(false);
        //        GetComponent<OneCreatureManager>().defImage.gameObject.SetActive(false);
        //    }
           
          
            //9
            if (targetUniqueID == GlobalSetting.instance.lowPlayer.playerID || targetUniqueID == GlobalSetting.instance.topPlayer.playerID)
                
            {
                Debug.Log("Create Damage Effect");
                DamageEffect.CreateDamageEffect(target.transform.position,damageTakenByAttack);
                target.GetComponent<PlayerPortraitVisual>().healthText.text = targetHealthAfter.ToString();
                target.GetComponent<PlayerPortraitVisual>().defText.text = targetArmorAfter.ToString();

              
            }
            else
            {
                target.GetComponent<OneCreatureManager>().healthText.text = targetHealthAfter.ToString();
                DamageEffect.CreateDamageEffect(target.transform.position,damageTakenByAttack); 
                target.GetComponent<OneCreatureManager>().defText.text = targetArmorAfter.ToString();
            }

            whereIsTheCreature.SetTableSortingOrder();
            whereIsTheCreature.visualState = tmpState;
            
            manager.defText.text = attackArmorAfter.ToString();
            manager.healthText.text = attackHealthAfter.ToString();
            
            
            if(targetArmorAfter <=0 || attackArmorAfter <=0)
            {
                GetComponent<OneCreatureManager>().defImage.gameObject.SetActive(false);
                manager.defImage.gameObject.SetActive(false);
            }

//           fbx.GetComponent<ParticleSystem>().Stop();
//           Destroy(fbx);
//      
            //opp
           
            Sequence s = DOTween.Sequence();
            s.AppendInterval(0.4f);
            s.OnComplete(() =>
            {
                Command.CommandExecutionComplete();
            });
        });

    }

    

   
}