using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;



[RequireComponent(typeof(NavMeshAgent))]
public class NetworkNavAgentBanding : NetworkBehaviourNonAlloc
{
        public Entity entity;
        public NavMeshAgent agent;
        
        //last pos 
        private Vector3 lastServerPos;
        private Vector3 lastSentPos;
        private double lastSendTime;
        
        //
        private const float epsilon = 0.1f;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        bool IsValidDestination(Vector3 pos)
        {
                return entity.health > 0 &&
                       (entity.state == "IDLE" && entity.state == "Moving");
        }

        [Command]
        public void CmdMove(Vector3 pos)
        {
                if (IsValidDestination(pos))
                {
                        agent.stoppingDistance = 0;
                        agent.destination = pos;
                        
                        //
                        SetDirtyBit(1);
                }
                else
                {
                        SetDirtyBit(1);
                }
        }

        /// <summary>
        /// 
        /// </summary>
        void Update()
        {
                if (isServer)
                {
                        if (Vector3.Distance(transform.position, lastServerPos) > agent.speed)
                        {
                                SetDirtyBit(1);
                               
                        }
                }    
                //
                lastServerPos = transform.position;
                
                //
                if (isLocalPlayer)
                {
                        if (NetworkTime.time >= lastSendTime + syncInterval)
                        {
                                if(isServer)
                                        SetDirtyBit(1);
                                else
                                {
                                        
                                        CmdMove(transform.position);
                                }

                                lastSendTime = NetworkTime.time;
                                lastSentPos = transform.position;
                        }    
                }
        }

        [Server]
        public void ResetMovement()
        {
                TargetResetMovement(transform.position);
                SetDirtyBit(1);
        }

        [TargetRpc]
        public void TargetResetMovement(Vector3 resetPos)
        {
                agent.ResetMovement();
                agent.Warp(resetPos);
        }
        [ClientRpc]
        public void RpcWarp(Vector3 pos){
                agent.Warp(pos);
        }
        public override bool OnSerialize(NetworkWriter writer, bool initialState)
        {
              writer.WriteVector3(transform.position);
              //
              writer.WriteSingle(agent.speed);
              return true;
        }

        public override void OnDeserialize(NetworkReader reader, bool init)
        {
                Vector3 pos = reader.ReadVector3();
                float speed = reader.ReadSingle();
                
                //
                if (agent.isOnNavMesh)
                {
                        if(NavMesh.SamplePosition(pos,out NavMeshHit hit,0.1f,NavMesh.AllAreas))
                        {
                                if (!isLocalPlayer)
                                {
                                        agent.stoppingDistance = 0;
                                        agent.speed = speed;
                                        agent.destination = pos;
                                }
                                
                                //
                                if (Vector3.Distance(transform.position, pos) > agent.speed * 2 && agent.isOnNavMesh)
                                {
                                        agent.Warp(pos);
                                }
                        }
                        else
                        {
                                Debug.Log("b");     
                        }
                }
                else
                {
                        Debug.Log("b");
                }
        }
}
