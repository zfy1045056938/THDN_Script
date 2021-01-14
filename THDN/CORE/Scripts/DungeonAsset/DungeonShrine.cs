using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Invector.vItemManager;
using Invector;
using UnityEngine.Events;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.UnityGUI;
public enum ShrineType{
    Normal,
    Exchange,
    Cash,
    
}

//

//when select the shrine add buff for player with counter time
// when time out then destory same to Buff DB
public class DungeonShrine:MonoBehaviour{

    public GameObject panel;
    public GameObject sidePanel;
    private Players player;
    public ParticleSystem ps;
    public bool hasActive=false;
public bool hasRequired=false;
    [Header("UI Module")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI detailText;
    
    public  int eventCount =3;
    public float rewardBouns=0;

    [Header("Shrine Data")]
    public BuffType  buffType= BuffType.None;
    public float amount;
    public float effectTime;
    public float needMoney;
    
    public DungeonEvent de;
    
    

  void Start(){
      player = Players.localPlayer;
      
  }

  void Update(){
      if(hasRequired==true){
        sidePanel.SetActive(true);
      }
  }

  public bool EnoughMoney(float c){
      return c>=  needMoney;
  }

    


  
}