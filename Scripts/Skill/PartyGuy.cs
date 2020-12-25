using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.UI;


/// <summary>
/// skill list show party guy who save by dungeon, player at dungeon
/// can manager guy with(stats & skills & eqipment ), with the limit of slot
/// player at camp can upgrade personal skill und increase the party num(leader)
/// player select tap show the skill list  
/// </summary>
[System.Serializable]
public  class PartyGuy:MonoBehaviour
{
    
    public static PartyGuy instance;
    public GameObject content;
    //rtv data
    public Character currentCharacter;
   
    public int index;  //select
    [Header("UI")]
    public Image guySprite;
    public Image glow;
    public TextMeshProUGUI levelText;
    public Slider expSlider;


    public int partyNum;   //limit by player leader

    public Button selectBtn;
    private void Start()
    {
        instance = this;

    }

    

    
}
