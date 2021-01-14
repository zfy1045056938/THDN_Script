using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// GetPlayerTothe Battle Scene include these module
/// 1.PlayerPacK(DeckName,CardAsset,CharacterAsset)
/// 2.EquipmentSlot(3)
/// 
/// </summary>
public class LoadCCFromC : MonoBehaviour
{

    public static LoadCCFromC instance;
    private Players p;
    private PlayerData players;
    public PlayerPortraitVisual visual;
    
    void Awake(){
        instance =this;
        p = GetComponent<Players>();
        players = GetComponent<PlayerData>();
       
        visual = FindObjectOfType<PlayerPortraitVisual>();
        Debug.Log("Load Data From Town");
        LoadCC();
    }
    
    


    public void LoadCC(){
        if (BattleStartInfo.player != null)
        {
            players = BattleStartInfo.player;
           
            if (BattleStartInfo.SelectDeck != null)
            {
                //
                if (BattleStartInfo.SelectDeck.characterAsset != null)
                {
                    p.charAsset = BattleStartInfo.SelectDeck.characterAsset;
                    p.MaxHealth = BattleStartInfo.SelectDeck.characterAsset.maxHealth + BattleStartInfo.player.playerHealth;  
                    // Debug.Log(p.charAsset.name+"Get asset");
                }
                else
                {
                    Debug.Log("INVALID CharacterAsset");
                }

                if (BattleStartInfo.SelectDeck.cardAssets != null)
                {
                    p.deck.cards = new List<CardAsset>(BattleStartInfo.SelectDeck.cardAssets);
                }
            }

            //Try Get Player Equipment
            if (BattleStartInfo.Weapon != null)
            {
                p.playerArea.playerPortraitVisual.weapon.weapon = BattleStartInfo.Weapon;
                p.atkDur = players.atkCount;
                p.CreatureAtk = players.atk;
            }

            
                p.CreatureDef = players.ArmorDef;
             if (BattleStartInfo.Ring != null)
            {
                p.playerArea.playerPortraitVisual.ring.items = BattleStartInfo.Ring;
            }

        }
        else
        {
            Debug.Log("Not Player");
        }
        
        
    }
    }

