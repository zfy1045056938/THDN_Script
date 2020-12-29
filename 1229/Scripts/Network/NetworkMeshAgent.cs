using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NetworkMeshAgent : NetworkBehaviourNonAlloc
{
   public NavMeshAgent agent;
   private Vector3 requiredVelocity;
   
   //
   private Vector3 lastUpdatePos;
   private Vector3 lastSerializedDestination;
   private Vector3 lastSerializedVelocity;

   private bool hadPath = false;
   
   //
   public void LookXY(Vector3 pos)
   {
      transform.LookAt(new Vector3(pos.x,transform.position.y,pos.z
      ));
   }

   bool HasPath()
   {
      return agent.hasPath || agent.pathPending;
   }

   /// <summary>
   /// 
   /// </summary>
   void Update()
   {
      if (isServer)
      {
         bool hasPaht = HasPath();
         //
         if(hasPaht && agent.destination!=lastSerializedDestination){SetDirtyBit(1);}
         else if(!hasPaht && agent.velocity !=lastSerializedVelocity)
         {
            SetDirtyBit(1);
         }else if (!hasPaht && Vector3.Distance(transform.position, lastUpdatePos) > agent.speed)
         {
            RpcWard(transform.position);
         }
         else if(hasPaht && !HasPath())
         {
            SetDirtyBit(1);
         }
      }else if (isClient)
      {
         if (requiredVelocity != Vector3.zero)
         {
            agent.ResetMovement();
            agent.velocity = requiredVelocity;
            LookXY(transform.position+requiredVelocity);
         }
      }
   }
   
   [ClientRpc]
   private void RpcWard(Vector3 transformPosition)
   {
      agent.Warp(transformPosition);
   }

   public override bool OnSerialize(NetworkWriter writer, bool initialState)
   {
      writer.WriteVector3(transform.position);
      writer.WriteSingle(agent.speed);
      bool hasPath = HasPath();
      writer.WriteBoolean(hasPath);
      if (hasPath)
      {
         writer.WriteVector3(agent.destination);
         writer.WriteSingle(agent.stoppingDistance);
         lastSerializedDestination = agent.destination;
      }
      else
      {
         writer.WriteVector3(agent.velocity);
         lastSerializedVelocity = agent.velocity;
      }

      return true;
   }


   public override void OnDeserialize(NetworkReader reader, bool initialState)
   {
      Vector3 pos = reader.ReadVector3();
      agent.speed = reader.ReadSingle();
      bool hasPath = reader.ReadBoolean();

      if (hasPath)
      {
         Vector3 destination = reader.ReadVector3();
         float stoppingDis = reader.ReadSingle();
         //
         if (agent.isOnNavMesh)
         {
            agent.stoppingDistance = stoppingDis;
            agent.destination = destination;
         }
         else
         {
            //
         }
         
      }
      else
      {
         Vector3 velocity = reader.ReadVector3();
         agent.ResetPath();
         requiredVelocity = velocity;
      }
      
      //
      if (Vector3.Distance(transform.position, pos) > agent.speed * 2 && agent.isOnNavMesh)
      {
         agent.Warp(pos);
      }
   }
}
