using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using DG.Tweening;
using UnityEngine.UI;
/// <summary>
/// 进行拖动目标,对目标进行进行线性检测.
/// 1.开始拖动
/// 2.结束拖动
/// </summary>
public class DragSpellOnTarget : DragAction
{

    public TargetOptions targets = TargetOptions.None;
    private SpriteRenderer sr;
//    public Image sr;
//    public Image triangleSR;
    private LineRenderer lr;
    private WhereIsTheCardOfCreature whereIsThisCard;
    private VisualStates tempVisualState;
    private Transform triangle;
    private SpriteRenderer triangleSR;
    private GameObject target;
    private OneCardManager manager;
  
    public override bool OnCanDrag
    {
        get
        {
           return base.OnCanDrag && manager.CanbePlayNow;
//            return true;
        }
    }

    public void Awake()
    {
        lr = GetComponentInChildren<LineRenderer>();
        sr = GetComponent<SpriteRenderer>();
        lr.sortingLayerName = "AboveEverything";
        triangle = transform.Find("Triangle");
        triangleSR = triangle.GetComponent<SpriteRenderer>();
        
        //
        whereIsThisCard = GetComponentInParent<WhereIsTheCardOfCreature>();
        manager = GetComponentInParent<OneCardManager>();
    }

  
    

    public override void OnStartDrag()
    {
        
        tempVisualState = whereIsThisCard.visualState;
        whereIsThisCard.visualState = VisualStates.Dragging;
        sr.enabled = true;
        lr.enabled = true;
    }

    public override void OnEndDrag()
    {
        target = null;
        RaycastHit[] hit;

        hit = Physics.RaycastAll(origin: Camera.main.transform.position,
            direction: (-Camera.main.transform.position + this.transform.position).normalized,
            maxDistance: 300.0f);

        
        foreach (RaycastHit hits in hit)
        {
            if (hits.transform.tag.Contains("Player"))
            {
                target = hits.transform.gameObject;
            }else if (hits.transform.tag.Contains("Creatures"))
            {
                target = hits.transform.gameObject;
            }
        }

        bool targetVaild = false;

        if (target != null)
        {
            Players owner = null;
            if (tag.Contains("Low"))
            {
                owner = GlobalSetting.instance.lowPlayer;
            }
            else
                owner = GlobalSetting.instance.topPlayer;

            int targetID = target.GetComponentInParent<IDHolder>().uniqueID;
            //
            Debug.Log("Select spell target");
            switch (targets)
            {
                //全体角色
                case TargetOptions.AllCharacter:
                    owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().uniqueID, targetID);
                    targetVaild = true;
                    break;
                //全体随从
                case TargetOptions.AllCreature:
                    if (tag.Contains("Creatures"))
                    {
                        owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().uniqueID, targetID);
                        targetVaild = true;
                    }

                    break;
                //己方
                case TargetOptions.YoursCharacters:
                    if (target.tag.Contains("Creatures") || target.tag.Contains("Player"))
                    {
                        if (tag.Contains("Low") && target.tag.Contains("Low")
                            || (tag.Contains("Top") && target.tag.Contains("Low")))
                        {
                            owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().uniqueID, targetID);
                            targetVaild = true;
                        }
                        
                    }

                    break;
                //敌人随从
                case  TargetOptions.EmenyCreature:
                    if (target.tag.Contains("Creature"))
                    {
                        if (tag.Contains("Low")&& target.tag.Contains("Low")
                            || (tag.Contains("Top") && target.tag.Contains("Low")))
                        {
                            owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().uniqueID,targetID);
                            targetVaild = true;
                        }
                    }

                    break;
                case TargetOptions.Creature:
                    if (target.tag.Contains("Creatures") || target.tag.Contains("Player"))
                    {
                        if (tag.Contains("Low") && target.tag.Contains("Low")
                            || (tag.Contains("Top") && target.tag.Contains("Low")))
                        {
                            owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().uniqueID, targetID);
                            targetVaild = true;
                        }
                        
                    }
                    

                    break;
                
                case TargetOptions.Target:
                    if (target.tag.Contains("Creatures") || target.tag.Contains("Player"))
                    {
                       
                            owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().uniqueID, targetID);
                            targetVaild = true;
                        
                        
                    }
                    

                    break;
                default:
                    Debug.Log("No Spell Target");
                    break;

            }

        }
        else
        {
            Debug.LogWarning("Default Spell");
        }
        
        //
        if (!targetVaild)
        {
            whereIsThisCard.visualState = tempVisualState;
            whereIsThisCard.SetHandSortingOrder();
        }
        
        //
        transform.localPosition = new Vector3(0f,0f,0f);
        sr.enabled = false;
        lr.enabled = false;
        triangleSR.enabled = false;

    }

    public override void OnDraggnigInUpdate()
    {
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;

        float distanceToTarget = (direction * 2.3f).magnitude;
//        if (notNormalized.magnitude  > distanceToTarget)
//        {
//            lr.SetPositions(new Vector3[]{transform.parent.position,transform.position-direction*2.3f});
//            lr.enabled = true;
//            
//            //
//            triangleSR.enabled = true;
//            triangleSR.transform.position = transform.position - 1.5f * direction;
//            
//            //
//            float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
//            triangleSR.transform.rotation = Quaternion.Euler(0f,0f,rot_z-90);
//        }
//        else
//        {
//            //
//            lr.enabled = false;
//            triangleSR.enabled = false;
//        }
        if (notNormalized.magnitude > distanceToTarget)
        {
            lr.SetPositions(new Vector3[]
            {
                transform.parent.position, transform.position-direction*2.3f
            });
            //
            lr.enabled = true;
            
            //
            triangleSR.enabled = true;
            triangleSR.transform.position = transform.position - 1.5f * direction;
            //
            float rot_2 = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rot_2 - 90);
            
            

        }
        else
        {
            lr.enabled = false;
            triangleSR.enabled = false;
        }


    }

    public override void OnCancelDrag()
    {
        throw new System.NotImplementedException();
    }

    protected override bool DraggingSuccess()
    {
        return true;
    }
}