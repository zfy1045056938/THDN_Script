using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        Rect rect = GetComponent<RectTransform>().rect;
        //
        Vector2 min = transform.TransformPoint(rect.min);
        Vector2 max = transform.TransformPoint(rect.max);
        Vector2 sizeWorld = max - min;
        //
        float x = Mathf.Clamp(min.x, 0, max.x);
        float y = Mathf.Clamp(min.y, 0, max.y);
        //
        Vector2 offset = (Vector2) transform.position - min;
        transform.position=new Vector2(x,y)+offset;
    }
}
