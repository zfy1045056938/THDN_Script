using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Monster : Entity
{
    public EnemyAsset enemyAsset;
    public NavMeshAgent agent;
    public Rigidbody2D rig2D;
    public float moveProp=0.1f;
    public float detationDistance = 0.2f;
    private Vector3 startPos;
    public bool isBoss=false;
   public Image killImage;
    private int _hp;
    public int HP{
        get{return _hp;
        }
        set{
           if(_hp<=0){
               Destroy(this);
                
           }
           _hp=value;
            
            }

    }
    [Header("UI Enemy Stats")] public SpriteRenderer avatarUI;
    public SpriteRenderer avaFrame;


    
    

    protected override string UpdateServer()
    {
        throw new System.NotImplementedException();
    }

    protected override void UpdaetClient()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Called when the mouse enters the GUIElement or Collider.
    // /// </summary>
    // void OnMouseEnter()
    // {
    //     outline.gameObject.SetActive(true);
    // }
    // /// <summary>
    // /// Called when the mouse is not any longer over the GUIElement or Collider.
    // /// </summary>
    // void OnMouseExit()
    // {
    //     outline.gameObject.SetActive(false);
    // }

     private void OnMouseDown() 
    {
     
     if(Utils.IsClickUI())return;

           if(converName!=null ){
               Debug.Log("Start Dialogue with"+this.name);
               SoundManager.instance.PlaySound(SoundManager.instance.atkClip);
               PlayerData.localPlayer.target=this;
               DialogueManager.StartConversation(converName);
       }else{
           Debug.Log("Select"+this.gameObject.name);
       }
    }

    /// <summary>
    /// Called when the mouse enters the GUIElement or Collider.
    /// </summary>
    void OnMouseEnter()
    {
        outline.enabled=true;
    }

    /// <summary>
    /// Called when the mouse is not any longer over the GUIElement or Collider.
    /// </summary>
    void OnMouseExit()
    {
        outline.enabled=false;
    }
}
