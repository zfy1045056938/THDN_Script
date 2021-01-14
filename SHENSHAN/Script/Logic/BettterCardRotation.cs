 using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class BettterCardRotation : MonoBehaviour {


    public RectTransform cardFront;

    public RectTransform cardBack;

//    public Transform targetFacePoint;
//
//    public Collider col;
//
//    private bool showingBack = false;


    // 点击卡牌旋转目标
//    void Update()
//    {
////        RaycastHit[] hits;
////
////        //
////                            hits = Physics.RaycastAll(origin: Camera.main.transform.position,
////                                  direction: (-Camera.main.transform.position + targetFacePoint.position).normalized,
////                                  maxDistance: (-Camera.main.transform.position + targetFacePoint.position).magnitude);
////
////        //declare the through
////        bool passedThroughColliderOnCard = false;
////        foreach (RaycastHit r in hits)
////            
////        {
////
////
////            if (r.collider == col)
////            
////                passedThroughColliderOnCard = true;
////
////            
////        }
////
////        //
////        if (passedThroughColliderOnCard != showingBack)
////        {
////            showingBack = passedThroughColliderOnCard;
////            //反转显(back || front)
////            if (showingBack)
////            {
////                cardFront.gameObject.SetActive(false);
////                cardBack.gameObject.SetActive(true);
////            }else{
////                cardFront.gameObject.SetActive(true);
////                cardBack.gameObject.SetActive(false);
////            }
////        }
//    }
    
    //
    void Update(){
        if (IsCameraFront())
        {
            cardFront.gameObject.SetActive(true);
            cardBack.gameObject.SetActive(false);
        }
        else
        {
            cardFront.gameObject.SetActive(false);
            cardBack.gameObject.SetActive(true);
        }
    }

    bool IsCameraFront()
    {
        return Vector3.Dot(cardFront.transform.forward, Camera.main.transform.position - cardFront.transform.position) <
               0;
    }
} 
