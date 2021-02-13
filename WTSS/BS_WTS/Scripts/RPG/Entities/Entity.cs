using Mirror;
using Unity;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Telepathy;
using UnityEngine.UI;
using Invector.vItemManager;
using Invector.vCharacterController;
using System.Collections.Generic;

/// <summary>
    /// manager entity in server state
    /// sync world player data
    /// 
    /// </summary>
    [RequireComponent(typeof(NetworkProximityChecker))]
    [RequireComponent(typeof(NetworkIdentity))]
    [Serializable]
    
    public abstract  class Entity: NetworkBehaviour
        {
         
            public Outline outline;


            [Header("Base Info")] [SyncVar] public string eName;
            [SyncVar] public int eLevel;
            [SyncVar] public float eHeavy;
            [SyncVar]public GameObject eObject;
            public GameObject highlightObj;
            public vThirdPersonController thirdPersonController;
            public Camera characterCamera;
            
            
            
            #region  BaseInfo
            
            
            [SyncVar]private float _eHealth;

            public virtual float eHealth
            {
                get
                {
                    return _eHealth;
                }
                set
                {
                    eHealth = value;
                }
            }
            [SyncVar]private float _eArmor;

            public virtual float eArmor
            {
                get
                {
                    return _eArmor;
                }
                set
                {
                    _eArmor = value;
                }
            }
            [SyncVar]private float _eDamage;

            public virtual float eDamage
            {
                get
                {
                    return _eDamage;
                }
                set
                {
                    _eDamage = value;
                }
            }

            #endregion
            #region ServerModule

         
            public override void OnStartClient()
            {
                
            }

            public override void OnStartServer()
            {
                
            }

            

            private  void UpdateClient()
            {
                Util.InvokeMany(typeof(Entity),this,"UpdateClient_");
            }

            public  void UpdateServer()
            {
                //Dead Command
                if(eHealth <=0) OnDeath();
                
                Util.InvokeMany(typeof(Entity),this,"UpdateServer_");
            }

            /// <summary>
            /// Dead Module When health send msg to server
            /// </summary>
            [Server]
            public virtual void OnDeath()
            {
                
            }
            
            #endregion
            #region Commond Module
    
            

            #endregion

            #region Items Data

            public List<vItem> itemLootList;
             

            #endregion
        }
