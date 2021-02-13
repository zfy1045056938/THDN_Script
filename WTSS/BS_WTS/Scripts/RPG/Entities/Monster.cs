using System;
using Unity;
using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;
using TMPro;
using UnityEngine.EventSystems;
using Mirror;
using UnityEngine;
//AI


[Serializable]
public class Monster : Entity
    {
        
        
        public override float eHealth { get; set; }
        public override float eArmor { get; set; }
        public override float eDamage { get; set; }


       
        [SyncVar] private int _lootGold;
        public int lootGold
        {
            get
            {
                return _lootGold;
            }
            set
            {
                _lootGold = value;
            }
        }
        [SyncVar] private int _lootDust;
        public int lootDust
        {
            get
            {
                return _lootDust;
            }
            set
            {
                _lootDust = value;
            }
        }

        public GameObject lootObj;
        public 
        
        public override void OnStopServer()
        {
            base.OnStopServer();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
        }

        public override bool OnSerialize(NetworkWriter writer, bool initialState)
        {
            return base.OnSerialize(writer, initialState);
        }

        public override void OnDeserialize(NetworkReader reader, bool initialState)
        {
            base.OnDeserialize(reader, initialState);
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        public override void OnDeath()
        {
            base.OnDeath();
        }
    }
