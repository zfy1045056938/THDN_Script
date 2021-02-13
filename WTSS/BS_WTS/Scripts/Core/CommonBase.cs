using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using TMPro;
using PixelCrushers.DialogueSystem;
using PixelCrushers;
using Invector;
using Mirror;
using GameDataEditor;
using Dreamteck.Splines;


[System.Serializable]
public class CommonBase : MonoBehaviour
{
    public static CommonBase instance;
    private Players players;
    
    [Header("Common")]
    public GameObject bodyPart;
    public BoxCollider protectBox;
    public SplineComputer sp;
    public SplineFollower follower;

    [Header("UI Module")] public UIPanel panel;
    
    [Header("Stats")]
    public bool playerInArea = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        players = Players.localPlayer;
    }

    
    /// <summary>
    /// if the protection include player that increase resit for player
    /// The Abilities was increase by location und bouns info
    /// 
    /// </summary>
    public void DetectionPlayer()
    {
        playerInArea = true;
        //
        
    }
}
