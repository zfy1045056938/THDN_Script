using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
/// <summary>
/// 当处于玩家回合时,开放对场面的控制
/// </summary>
public class HoverPreview : MonoBehaviour {


    public GameObject turnThisOffWhenPreviewing;    
    public Vector3 targetPosition;       //目标位置
    public float targetScale;           //目标缩放距离
    public GameObject previewGameObject;
    public bool activateInAwake = false;

    public static HoverPreview currentlyViewing=null ;
    


    //悬浮跟随
    private static bool _previewAllowed =true;
    public static bool previewAllowed{
        get{
            return _previewAllowed;
        }
        set{
            _previewAllowed=value;
            if(!_previewAllowed){
                StopAllPreviews();
            }
        }
    }


    //关闭悬浮视图
    private bool _thisPreviewEnabled =false;
    public bool thisPreviewEnable {
        get{
            return _thisPreviewEnabled;
        }

        set{
            _thisPreviewEnabled=value;
            if(!_thisPreviewEnabled){
                StopThisPreview();
            }
        }
    }

    public bool overCollider{get;set;}

    void Awake()
    {
        thisPreviewEnable=activateInAwake;
    }

    /// <summary>
    /// For Battle Card
    /// </summary>
    void OnMouseEnter()
    {
        overCollider = true;
        if (previewAllowed && thisPreviewEnable)
        {
            PreviewThisObject();
        }
    }

    void OnMouseExit()
    {
        overCollider = false;

        if (!PreviewSomeCard())
        {
            StopAllPreviews();  //
        }
    }



    /// <summary>
    /// Previews the this object.
    /// </summary>
    public  void PreviewThisObject(){
        //1.reset
        StopAllPreviews();  
        //2.hoverpreview  set true
        currentlyViewing =this;
        //3.enable GameObjcet
        previewGameObject.gameObject.SetActive(true);
        //4.disable
        if(turnThisOffWhenPreviewing != null)
            turnThisOffWhenPreviewing.gameObject.SetActive(false);
        //5.
        previewGameObject.transform.localPosition =Vector3.zero;
        previewGameObject.transform.localScale =Vector3.one;

        //
        previewGameObject.transform.DOLocalMove(targetPosition,0.3f).SetEase(Ease.OutQuint);
        previewGameObject.transform.DOScale(targetScale,0.3f).SetEase(Ease.InQuint);

    }

    //停止悬浮显示
    public void StopThisPreview(){
        previewGameObject.SetActive(false);
        previewGameObject.transform.localScale =Vector3.zero;
        previewGameObject.transform.localPosition =Vector3.zero;
        if(turnThisOffWhenPreviewing !=null){
            turnThisOffWhenPreviewing.SetActive(true);
        }
    }

    /// <summary>
    /// Stops all previews.
    /// </summary>
    public static void StopAllPreviews(){
        if (currentlyViewing != null)
        {

            currentlyViewing.previewGameObject.SetActive(false);
            currentlyViewing.previewGameObject.transform.localScale = Vector3.one;
            currentlyViewing.previewGameObject.transform.localPosition = Vector3.zero;
           
            //
            if (currentlyViewing.turnThisOffWhenPreviewing != null)
            {
                currentlyViewing.turnThisOffWhenPreviewing.SetActive(true);

            }
        }
    }
     
    /// <summary>
    /// Previews some card.
    /// </summary>
    /// <returns><c>true</c>, if some card was previewed, <c>false</c> otherwise.</returns>
    public static bool PreviewSomeCard(){
        if(!previewAllowed )return false;
        //
        HoverPreview[] allHoverBlowUps =FindObjectsOfType<HoverPreview>();
        //
        foreach (HoverPreview h in allHoverBlowUps)
        {
            if(h.overCollider && h.thisPreviewEnable)
                return true;
        }
        return false;
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    overCollider = true;
    //    if (previewAllowed && thisPreviewEnable)
    //    {
    //        PreviewThisObject();
    //    }
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    overCollider = false;

    //    if (!PreviewSomeCard())
    //    {
    //        StopAllPreviews();  //
    //    }
    //}

   
}
