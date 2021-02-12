using Mirror;
using UnityEngine;
using TMPro;
using Dreamteck.Splines;
using Unity.Entities;
using Cinemachine;
using PixelCrushers.DialogueSystem;
using Unity.Collections;
using Unity.Burst;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
using GameDataEditor;
/*
*   Second Part
*  When Server log to game load town first
*  TM storge current scene und server state if at town
*  
*
*/
public class GlobalSetting : MonoBehaviour
{
    public SplineComputer sp;
    public SplineFollower mainTarget;

    public bool canStart = false;
    public GameObject UIpanel;
    
    //
    [HideInInspector] public GameObject player;

    [Header("Game Elements")] public GameObject sceneObj;
    public List<GameObject> EnemiesList;
    public GameObject[] chestList;
    public NavMeshSurface NavMeshSurface;

    void Awake()
    {
        GDEDataManager.Init("gde_data");
        
    }



    public void StopTheCar()
    {
        Debug.Log("Stop the target und active the conversaction");
        mainTarget.followSpeed = 0;
        
    }
}