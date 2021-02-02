using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;




//打开牌包
public class ScriptToOpenPack:MonoBehaviour{
    public Image glowImage;
    public Image GlowColor;

    public bool allowedOpen =false;


    public Collider collider;
    void Awake()
    {
        collider =GetComponent<BoxCollider>();
      
    }

    //opening pack then all card show the BackBtn Show
   public void AllPackOpen(){
        allowedOpen=true;
        ShopManager.instance.packOpeningArea.AllowedToDragAPack=false;
        ShopManager.instance.packOpeningArea.BackButton.interactable=false;
        if(PackOpeningArea.instance.SlotsForCards.Length>0){
        if(CursorOverCard()){
                glowImage.DOColor(Color.white, 0.05f);
        }
        }else{
            Debug.Log("NO PACK IN THE PACKOPENINGAREA");
        }

    }

 

    public bool CursorOverCard(){


        RaycastHit [] hits;

        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
        bool PassThroughTableCollider=false;
        //
        foreach(RaycastHit h in hits){
            if(h.collider == collider){
                PassThroughTableCollider=true;
            }

        }        
        return  PassThroughTableCollider;
    }

    private void OnMouseEnter() {
        if(allowedOpen){
            GlowColor.DOColor(Color.clear, 0.5f);
        }
        
    }

    ///when press the pack  then opening pack show pack include
    //1.5 pack (4 random 1 rare)
    //2.1 pack over if have other then reply when exist card= 0
    
    void OnMouseDown()
    {
        // 进入序列 
        if(allowedOpen){
            allowedOpen=false;
            Sequence s = DOTween.Sequence();    
            
            s.Append(transform.DOLocalMoveZ(-2f,0.5f));
            s.Append(transform.DOShakeRotation(1f,3,3,3,true));

            
        }
        
    }


}