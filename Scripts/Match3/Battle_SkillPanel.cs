
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


/// <summary> 
/// load skill from skill bar who conifg by skill panel
/// player without battle can config customize skill but can't at bf()
/// when player battle with enemy,load skill from player templete und check skill states
/// the skills state needs check elements before the game start in order to start skill
/// by skillmotion (passive || init)
/// some skill will start before the game start
/// the skills state has cooldown, casttime, effecttime ,ragebouns
/// when player at matches und enough to cast by collection pools , highlight the skill pieces
/// und notice player can use at this time util the pieces gone
/// UI Module includes Skill bar und collectionPools for player collect gp at matches 

/// </summary>
public class Battle_SkillPanel:MonoBehaviour
{
    public CollectionsPool bcp;
    public Entity entity;
    [Header("UI")]
    public Transform skillPos;
    public GameObject skill;

    internal void Init(Players e)
    {
        throw new NotImplementedException();
    }
}