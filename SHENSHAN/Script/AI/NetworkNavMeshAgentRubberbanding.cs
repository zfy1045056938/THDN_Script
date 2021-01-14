using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NetworkNavMeshAgentRubberbanding : NetworkBehaviour
{
   public NavMeshAgent agent;
   public Entity entity;

   private Vector3 lastServerPos;
   private Vector3 lastSendPos;
   private double lastSentTime;

   private const float epsilon = 0.1f;

   bool IsValidDestination(Vector3 pos)
   {
      return true;
   }
   [Command]
   void CmdMoved(Vector3 pos)
   {
      if (IsValidDestination(pos))
      {
         agent.stoppingDistance = 0;
         agent.destination = pos;

         SetDirtyBit(1);
      }
      else
      {
         SetDirtyBit(1);
      }
   }

   void Update()
   {
      if (isServer)
      {
         if (Vector3.Distance(transform.position, lastServerPos) > agent.speed)
         {
            SetDirtyBit(1);
         }

         lastServerPos = transform.position;
      }
      
      //
      if (isLocalPlayer)
      {
         if (NetworkTime.time >= lastSentTime + syncInterval)
         {
            if (isServer)
            {
               SetDirtyBit(1);
            }
            else
            {
               
               CmdMoved(transform.position);
            }

            lastSentTime = NetworkTime.time;
            lastSendPos = transform.position;
         }
      }
      
      
   }
   
   //
   [Server]
   public void ResetMovement()
   {
      TargetResetMovement(transform.position);
      //
      SetDirtyBit(1);
   }
   
   //
   [TargetRpc]
   void TargetResetMovement(Vector3 resetPos)
   {
      agent.ResetMovement();
      agent.Warp(resetPos);
   }
 
   
   //
   public override bool OnSerialize(NetworkWriter writer, bool init)
   {
      writer.WriteVector3(transform.position);
      //
      writer.WriteSingle(agent.speed);

      return true;
   }


   public override void OnDeserialize(NetworkReader reader, bool init)
   {
      Vector3 pos = reader.ReadVector3();
      float spd = reader.ReadSingle();
      
      //
      if (agent.isOnNavMesh)
      {
         if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
         {
            if (!isLocalPlayer)
            {
               agent.stoppingDistance = 0;
               agent.speed = spd;
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
            
         }
      }
      else
      {
         
      }
   }
}
