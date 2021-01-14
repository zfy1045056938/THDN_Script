using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ArrowRenderer:MonoBehaviour
{
    public float height = 0.5f;
    public float segmentLength = 0.5f;
    public float fadeDistance = 0.35f;
    public float speed = 1f;

    [SerializeField]
    GameObject arrowPrefab;
    [SerializeField]
    GameObject segmentPrefab;
    [SerializeField]
    Vector3 upwards = Vector3.up;

    [Space]
    [SerializeField]
    Vector3 start;
    [SerializeField]
    Vector3 ends;
    
    private Transform arrow;

    List<Transform> segment = new List<Transform>();
    
    public void SetPositions(Vector3 start,Vector3 ends)
    {
        this.start = start;
        this.ends = ends;
    }


    void UpdateSegment()
    {
        float distance = Vector3.Distance(start, ends);
        float radius = height / 2f + distance * distance / (8f * height);
        float diff = radius - height;
        float angle = 2f * Mathf.Acos(diff / radius);
        float length = angle * radius;
        float segmentAngle = segmentLength / radius * Mathf.Rad2Deg;

        //
        Vector3 center = new Vector3(0, -diff, distance / 2f);
        Vector3 left = Vector3.zero;
        Vector3 right = new Vector3(0, 0, distance);

        //
        int segmentsCount = (int)(length / segmentLength) + 1;

        CheckSegments(segmentsCount);

        float offset = Time.time * speed * segmentAngle;

        Vector3 firstSegentPos = Quaternion.Euler(Mathf.Repeat(offset, segmentAngle), 0f, 0f) * (left - center) + center;

        float fadeStartDistance = (Quaternion.Euler(segmentAngle / 2f, 0f, 0f) * (left - center) + center).z;

        for (int i = 0; i < segmentsCount; i++)
        {
            Vector3 pos = Quaternion.Euler(segmentAngle * i, 0f, 0f) * (firstSegentPos - center) + center;
            segment[i].localPosition = pos;
            segment[i].localRotation = Quaternion.FromToRotation(Vector3.up, pos - center);

            MeshRenderer rend = segment[i].GetComponent<MeshRenderer>();

            if (!rend)
            {
                continue;
            }

            if (!arrow)
            {
                arrow = Instantiate(arrowPrefab, transform).transform;
            }

            arrow.localPosition = right;
            arrow.localRotation = Quaternion.FromToRotation(Vector3.up, right - center);

            transform.position = start;
            transform.rotation = Quaternion.LookRotation(ends - start, upwards);


        }

    }

        void CheckSegments(int segmentsCounts)
        {
            while (segment.Count < segmentsCounts)
            {
                segment.Add(Instantiate(segmentPrefab, transform).transform);
                
            }

            for (int i = 0; i < segment.Count; i++)
            {
                GameObject segments = segment[i].gameObject;
                if (segments.activeSelf  != i < segmentsCounts)
                {
                    segments.SetActive(i < segmentsCounts);
                }
            }


          
        }



    
}