using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

public class DragCreatureAttack : DragAction
{

    private SpriteRenderer sr;
    private LineRenderer lr;
    private WhereIsTheCardOfCreature whereIsTheCreature;
    public Transform triangle;
    public SpriteRenderer triangleSR;
    private  GameObject target;
    public
        OneCreatureManager manager;

    private CurvedLinePoint[] linePoints = new CurvedLinePoint[0];
    private Vector3[] linePositions = new Vector3[0];
    private Vector3[] linePositionsOld = new Vector3[0];


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponentInChildren<LineRenderer>();
        lr.sortingLayerName = "AboveEverything";
        triangle = transform.Find("Triangle");
        triangleSR = triangle.GetComponent<SpriteRenderer>();

        manager = GetComponentInParent<OneCreatureManager>();
        whereIsTheCreature = GetComponentInParent<WhereIsTheCardOfCreature>();

    }

    public override bool OnCanDrag
    {
        get
        {
            return base.OnCanDrag && manager.CanAtkNow;
          
        }
       
    }


    public override void OnCancelDrag()
    {
        throw new NotImplementedException();
    }

    public override void OnDraggnigInUpdate()
    {
        #region DraggingAtk_1
        //Vector3 notNormalized = transform.position - transform.parent.position;
        //Vector3 direction = notNormalized.normalized;
        //float distanceToTarget = (direction * 2.3f).magnitude;
        //if (notNormalized.magnitude > distanceToTarget)
        //{
        //    linePoints = lr.gameObject.GetComponentsInChildren<CurvedLinePoint>();

        //    Vector3 midPoint = Vector3.Lerp(transform.parent.position,
        //        transform.position - direction,0.5f);

        //    linePositions = new Vector3[linePoints.Length];
        //    //
        //    midPoint += new Vector3(0, 0, -lr.positionCount * 0.1f);

        //    if (midPoint.z>0)
        //    {
        //        midPoint.z = 0;
        //    }
        //    if (midPoint.z<-5f)
        //    {
        //        midPoint.z = -5f;
        //    }

        //    //
        //    if (lr.positionCount <2)
        //    {
        //        midPoint.z = 0;
        //    }


        //    linePoints[0].transform.position = transform.parent.position;
        //        linePoints[1].transform.position = midPoint;
        //        linePoints[2].transform.position = transform.parent.position - direction * 2f;


        //        linePositions[0] = linePoints[0].transform.position;
        //        linePositions[1] = linePoints[1].transform.position;
        //        linePositions[2] = linePoints[2].transform.position;




        //    if (linePositionsOld.Length != linePositions.Length)
        //    {

        //        linePositionsOld = new Vector3[linePositions.Length];
        //    }


        //    //Set method Values
        //    Vector3[] smoothedPoints = LineSmoother.SmoothLine(linePositions, 2);

        //    lr.positionCount = smoothedPoints.Length;
        //    lr.SetPositions(smoothedPoints);

        //    //
        //    lr.enabled = true;

        //    //
        //    triangleSR.enabled = true;
        //    triangleSR.transform.position = transform.position - 1.5f * direction;

        //    //
        //    float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;

        //    triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);


        //}
        //else
        //{
        //    lr.enabled = false;
        //    sr.enabled = false;
        //}

        #endregion

        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction * 2.3f).magnitude;
        
        if (notNormalized.magnitude> distanceToTarget)
        {
            //
            lr.SetPositions(new Vector3[] { transform.parent.position, transform.position - direction * 2.3f });
            lr.enabled = true;

            //
            triangleSR.enabled = true;
            triangleSR.transform.position = transform.position - 1.5f * direction;

            //
            float rot_2 = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rot_2 - 90);

        }
        else{
            lr.enabled = false;
            triangleSR.enabled = false;
        }
    }


    public override void OnEndDrag()
    {
        target = null;
       
        RaycastHit[] hit;
        hit = Physics.RaycastAll(origin: Camera.main.transform.position,
            direction: (-Camera.main.transform.position + this.transform.position).normalized,
            maxDistance:100.0f
            );


        foreach (RaycastHit h in hit)
        {
            if ((h.transform.tag == "TopPlayer" && this.tag == "LowCreatures") ||
                (h.transform.tag == "LowPlayer" && this.tag == "TopCreatures"))
            {
//                target = h.transform.gameObject;

                //no taunt at table
                target = h.transform.gameObject;

            }

            else if ((h.transform.tag == "TopCreatures" && this.tag == "LowCreatures") ||
                     (h.transform.tag == "LowCreatures" && this.tag == "TopCreatures"))
            {
                
                //no taunt at table
                target = h.transform.parent.gameObject;
            }
        }

        bool targetValid = false;

            if (target != null)
            {
                int targetID = target.GetComponentInParent<IDHolder>().uniqueID;

                if (targetID == GlobalSetting.instance.lowPlayer.playerID ||
                    targetID == GlobalSetting.instance.topPlayer.playerID)
                {

                    CreatureLogic.creatureCreatedThisGame[GetComponentInParent<IDHolder>().uniqueID].GoFace();
                    targetValid = true;
                }
                else if (CreatureLogic.creatureCreatedThisGame[targetID] != null)
                {
                    //
                   if(CreatureLogic.creatureCreatedThisGame[GetComponentInParent<IDHolder>().uniqueID].Sleep==false){
                    targetValid = true;
                    CreatureLogic.creatureCreatedThisGame[GetComponentInParent<IDHolder>().uniqueID]
                        .AtkWithCreatureWithID(targetID);
                   }else{

                   }

                }

                //TODO Castle 
            }
            else
            {
                transform.localPosition = Vector3.zero;
                sr.enabled = false;
                lr.enabled = false;
                triangleSR.enabled = false;
            }

            if (!targetValid)
            {
                if (tag.Contains("Low"))
                {
                    whereIsTheCreature.visualState = VisualStates.LowTable;
                }
                else
                {
                    whereIsTheCreature.visualState = VisualStates.TopTable;
                }

                whereIsTheCreature.SetTableSortingOrder();
            }

            //
            transform.localPosition = Vector3.zero;
            sr.enabled = false;
            lr.enabled = false;
            triangleSR.enabled = false;
        
    }

    public override void OnStartDrag()
    {
        whereIsTheCreature.visualState = VisualStates.Dragging;
        sr.enabled = true;
        lr.enabled = true;
    }

    protected override bool DraggingSuccess()
    {
       return true;
    }

    //check table creature has taunt
    public bool CheckTaunt()
    {
        foreach (var c in TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable)
        {
            if (c.Taunt == true)
            {
                return true;
            }
        }

        return false;
    }
}