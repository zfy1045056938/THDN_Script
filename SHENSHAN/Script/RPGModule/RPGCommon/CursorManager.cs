using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class CustomCursor
{
    public string cursorName;
    public Sprite cursorImage;
}
public class CursorManager : MonoBehaviour
{
//     public List<CustomCursor> cursorList;
//     private static List<CustomCursor> cursor;
    private static  Image image;

    public void Start()
    {
        // cursor = cursorList;
        image = GetComponent<Image>();
        UnityEngine.Cursor.visible = false;
        
        
    }

    public void Update()
    {
        // cursor = cursorList;

        image = GetComponent<Image>();

        if (image.enabled)
        {
            image.rectTransform.position =
                new Vector3(
                    Input.mousePosition.x + image.rectTransform.sizeDelta.x * image.rectTransform.lossyScale.x *0.4f ,
                    Input.mousePosition.y - image.rectTransform.sizeDelta.x *
                    image.rectTransform.lossyScale.y *0.4f  , 0f);
                

        // }
        // UnityEngine.Cursor.SetCursor(image,new Vector2(1000.0f,1000.0f),CursorMode.ForceSoftware);
        
    }

    // public static void HideCursor()
    // {
    //     image.enabled = false;
    // }

    // public static void ShowCursor()
    // {
    //     image.enabled = true;
    // }

    // public static void ChangeCursor(string cursorName)
    // {
    //     for (int i = 0; i < cursor.Count; i++)
    //     {
    //         if (cursor[i].cursorName ==cursorName)
    //         {
    //             image.sprite = cursor[i].cursorImage;
    //         }
            
    //     }
    // }
    }
}
