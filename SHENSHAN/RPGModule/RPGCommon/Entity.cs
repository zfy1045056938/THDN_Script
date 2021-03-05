using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using Cinemachine;

//Manager Interactive Model Like NPC MONSTER and Player
//use mirror call cmd and sync system
public abstract class Entity : NetworkBehaviour
{

    [SyncVar]
    GameObject _target;
    public Entity target{
        get{return _target!=null ?  _target.GetComponent<Entity>():null;}
        set{_target=value!=null ? value.gameObject:null;}
    }

    [SyncVar]
    public int level =1;

    public int maxLevel=30;
    // [Header("Sync System")]
    // public SyncListItemSlot inventory =new SyncListItemSlot();
    // public SyncListEquipmentSlot equipment=new SyncListEquipmentSlot();

    [Header("Gold For both")]
    [SyncVar,SerializeField]public int _gold=0;
    public int gold{get{return _gold;}set{_gold=Mathf.Max(value,0);}}

    public QuestLogWindow.QuestInfo[] questList;
    public List<CardAsset> cardList;
    public string converName;
    public BoxCollider2D boxCollider;
    public Image avaSprite;
    public Image frameSprite;

    public CinemachineVirtualCamera faceCamera;
    public Outline outline;
    
    //For Merchant Panel
    public MerchantController merchantController;
    
void Awake(){
    Utils.InvokeMany(typeof(Entity),this,"_Awake");

}

public override void OnStartServer(){
    Utils.InvokeMany(typeof(Entity),this,"OnStartServer_");
}

protected abstract string UpdateServer();
protected abstract void UpdaetClient();




}
