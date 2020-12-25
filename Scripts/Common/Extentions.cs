using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine.AI;


//extra classes  for method
    public static class Extentions
    {

        public static int ToInt(this string value, int errVal = 0)
        {
            Int32.TryParse(value, out errVal);
            return errVal;
        }
        public static List<U> FindDup<T,U>(this List<T>t, Func<T,U>keySelector)
        {
            return t.GroupBy(keySelector).Where(group => group.Count() > 1)
                .Select(group => group.Key).ToList();

        }

        public static int GetStableHashCode(this string code)
        {
            unchecked
            {
                int hash = 23;
                foreach (char t in code)
                
                    hash = hash * 31 + t;
                    return hash;
                
            }

           

        }

        //
        public static Vector3 NearestValidDestination(this NavMeshAgent agent,Vector3 destinaion){
            
           

            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(destinaion, path))
            {
                return path.corners[path.corners.Length - 1];
            }

           
            
            //
            if(NavMesh.SamplePosition(destinaion,out NavMeshHit hit,agent.speed*2,NavMesh.AllAreas))
                if (agent.CalculatePath(hit.position, path))
                    return path.corners[path.corners.Length - 1];

            return agent.transform.position;
        }

    
    }
