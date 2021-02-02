using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public enum Jobs{
    None,
  Traveller,
    Hunter,
    Magic,
   
}
/// <summary>
/// Character Create Editor
/// </summary>
public class CustomerInspector : ScriptableObject {


    public Jobs jobs;

   
    
    public int health;

    public Sprite heroAvatarImage;
    public Sprite heroPowerImage;
    public Sprite avatarBGImage;
    public Sprite heroPowerBGImage;
    public Color32 heroClassTint;
    public Color32 heroFrameTint;


    public CastleType castle;

     



    private void OnEnable()
    {

    }
}
