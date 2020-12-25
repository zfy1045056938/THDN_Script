using System;
using UnityEngine;
using Mirror;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using TMPro;
using DungeonArchitect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Battle_EquipmentPanel:Singleton<Battle_EquipmentPanel>
{
    public Entity entity;

    public List<EquipmentItem> equipments;

    public bool hasEquipment = false;

    public int equipmentNum = -1;

    public EquipmentItem[] battleItems;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }


}