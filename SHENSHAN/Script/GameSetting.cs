using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;



public class GameSetting : MonoBehaviour
{
    public static GameSetting instance;
    [SerializeField] public Slider FxSlider;

    [SerializeField] public Dropdown ResolutionDrop;

    public Resolution[] res;
    
    public AudioSource sound;

    
  [SerializeField]
    public float fxVolume;


    void Start()
    {
        instance = this;
        fxVolume = SoundManager.instance.musicVolume;

    }

   

    
    /// <summary>
    /// 
    /// </summary>
    public void SaveSetting()
    {
       FxSlider.value = fxVolume;
        
        PlayerPrefs.SetFloat("Fxvolume",FxSlider.value);
        PlayerPrefs.SetString("Res",ResolutionDrop.options[0].text);
        instance.gameObject.SetActive(false);
        
        
    }

    public void OnFxChange()
    {
        sound.volume = FxSlider.value;
    }

   

    public void LoadSetting()
    {
        PlayerPrefs.GetFloat("Fxvloume");
        PlayerPrefs.GetFloat("Res");
    }
    

}
