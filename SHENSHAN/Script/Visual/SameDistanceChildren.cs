using UnityEngine;
using System.Collections;


//控制牌序
public class SameDistanceChildren:MonoBehaviour
{
    public Transform[] children;

    private void Awake()
    {
        Vector3 firstElementPos = children[0].position;

        Vector3 lastElementPos = children[children.Length - 1].transform.position;

        

        //
        float XDist = (lastElementPos.x - firstElementPos.x) / (float)(children.Length - 1);
        float YDist = (lastElementPos.y - firstElementPos.y) / (float)(children.Length -1);
        float ZDist = (lastElementPos.z - firstElementPos.z) / (float)(children.Length - 1);

        Vector3 Dist = new Vector3(XDist, YDist, ZDist);

        for(int i =1;i<children.Length;i++){
            children[i].transform.position= children[i-1].transform.position + Dist;
        }
    }
}