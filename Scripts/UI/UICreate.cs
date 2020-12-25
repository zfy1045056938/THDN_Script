using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Mirror;
using System.Linq;
using UnityEngine.AI;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

//using Michsky.UI.Zone;


/// <summary>
///  use DICE to customize stats
///  
/// </summary>
public class UICreate:MonoBehaviour{
    public GameObject panel;
    public NetworkManagerTHDN managerTHDN;

    //
    public Button createBtn;
    public Button cancelBtn;
    public TMP_InputField tmpImp;
    //
    public TextMeshProUGUI nameText;
    // public Dropdown classText;
    public TMP_Dropdown selector;
    //TODO 127 DICE to roll rnd number 
    public int totalPoints = 3;
    public int depoints = 0;
    public int sciencepoints = 0;
    public int leaderpoints = 0;
    public int kissasspoints = 0;
    public int lockpoints=0;
    
    //
    public TextMeshProUGUI deText;
    public TextMeshProUGUI leaderText;
    public TextMeshProUGUI lpText;
    public TextMeshProUGUI kissassText;
    public TextMeshProUGUI scienceText;

    public TextMeshProUGUI totalText;
    //
    public Slider kaSlider;
    public Slider scSlider;
    public Slider deSlider;
    public Slider leaderSlider;
    public Slider lpSlider;

    //
    public Button kaBtn;
    public Button deBtn;
    public Button lpBtn;
    public Button scBtn;
    public Button ldBtn;


    void Start()
    {

        managerTHDN.GetComponent<NetworkManagerTHDN>();
       

    }
    void Update()
    {

       
         //Add Classes to dropdown list
        selector.options= managerTHDN.playerClasses.Select(
            p=>new TMP_Dropdown.OptionData(p.name)
        ).ToList();

        // selector.elements = managerTHDN.playerClasses.Select(panel=>panel.name).ToList();
        //
        
        //
        cancelBtn.onClick.AddListener(()=>{
            panel.SetActive(false);
        });


        deSlider.value = depoints;
        kaSlider.value=kissasspoints;
        leaderSlider.value=leaderpoints;
        scSlider.value=sciencepoints;
        lpSlider.value=lockpoints;
        totalText.text= totalPoints.ToString();

        deText.text=depoints.ToString();
        kissassText.text=kissasspoints.ToString();
        leaderText.text=leaderpoints.ToString();
        lpText.text=lockpoints.ToString();
        scienceText.text=sciencepoints.ToString();

        // kaBtn.onClick.AddListener(()=>{
        //     AddkaPoint();
        // });
        // scBtn.onClick.AddListener(()=>{
        //     AddscPoint();
        // });
        // lpBtn.onClick.AddListener(()=>{
        //     AddlpPoint();

        // });
        // deBtn.onClick.AddListener(()=>{
        //     AdddePoint();
        // });
        // ldBtn.onClick.AddListener(()=>{
        //     AddleadPoint();
        // });
        

        if(totalPoints<1){

        }


        }
    // }

    public void CreateC(){

        
            CharacterCreateMsg message=new CharacterCreateMsg{
                names =nameText.text,
                className=selector.value,
                ka=kissasspoints,
                lp=lockpoints,
                leader=leaderpoints,
                de=depoints,

                 
            };
            NetworkClient.Send(message);
            Hide();
        
    }

    public void Open(){
        panel.SetActive(true);
    }
    public void Hide(){
        if(panel.activeSelf){
            panel.SetActive(false);
        }
    }

    
    public void AddscPoint(){
        if(totalPoints>0){
        sciencepoints+=1;
        totalPoints-=1;
        }else{
            
        }
    }

    public void AddlpPoint(){
        if(totalPoints>0){
        lockpoints+=1;
        totalPoints-=1;
        }
    }
      public void AddkaPoint(){
        if(totalPoints>0){
        kissasspoints+=1;
        totalPoints-=1;
        }
    }
      public void AdddePoint(){
        if(totalPoints>0){
            totalPoints-=1;
        kissasspoints+=1;;
        }
    }
      public void AddleadPoint(){
        if(totalPoints>0){
        leaderpoints+=1;
        totalPoints-=1;
        }
    }
    
}