using UnityEngine;
using UnityEngine.UI;
using TmPro;
using Invector.ItemManager;
using Invector;
using UnityEngine.Events;
using PixelCrusher.Dialogue;

public enum ShrineType{
    Normal,
    Exchange,
    Cash,
    
}

//
public enum BuffType{
    None,
    
}
//when select the shrine add buff for player with counter time
// when time out then destory same to Buff DB
public class DungeonShrine:MonoBehaviour{

    public UIPanel panel;
    private Players player;
    public ParticleSystem ps;
    public bool hasActive=false;

    [Header("UI Module")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI detailText;
    

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

  }

  public bool EnoughMoney(float c){
      return c>=  needMoney;
  }

    
  public void ActiveEffect(){
      
  }

  
}