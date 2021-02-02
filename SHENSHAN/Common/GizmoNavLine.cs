using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GizmoNavLine : MonoBehaviour
{
  private void OnDrawGizmos()
  {
    NavMeshAgent agent = GetComponent<NavMeshAgent>();
    NavMeshPath mesh = agent.path;


    Color color = Color.white;

    switch (mesh.status)
    {
      case NavMeshPathStatus.PathComplete:color=Color.white;
        break;
      case NavMeshPathStatus.PathInvalid : color = Color.red;
        break;
      case NavMeshPathStatus.PathPartial: color = Color.yellow;
        break;
    }
    //
    for (int i = 0; i < mesh.corners.Length; ++i)
    {
      Debug.DrawLine(mesh.corners[i-1],mesh.corners[i],color);
    }
    
    //
    Debug.DrawLine(transform.position,transform.position+agent.velocity,Color.blue,0,false);

  }
}
