using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavmeshPathGizmo : MonoBehaviour
{
   private void OnDrawGizmos()
   {
       NavMeshAgent agent = GetComponent<NavMeshAgent>();

       NavMeshPath path = agent.path;
       
       Color color = Color.white;
       switch (path.status)
       {
           case NavMeshPathStatus.PathComplete:color=Color.white;
               break;
           case NavMeshPathStatus.PathInvalid:color=Color.red;
               break;
           case NavMeshPathStatus.PathPartial: color = Color.green;
               break;
       }
       //
       for (int i = 1; i < path.corners.Length; ++i)
       {
           Debug.DrawLine(path.corners[i-1],path.corners[i],color);
           
       }
       Debug.DrawLine(transform.position,transform.position+agent.velocity,Color.blue,0,false);
   }
}
