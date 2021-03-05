using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class CustomCursor
{
    public string cursorName;
    public Texture2D cursorImage;
}
public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;
    public List<CustomCursor> cursorList;
    private static List<CustomCursor> cursor;
    public   Image cimage;


 private void Start() {
      instance=this;
        cursor = cursorList;
        // cimage = GetComponent<Image>();
        UnityEngine.Cursor.visible = false;
        
        ChangeCursor("Common");
        
}
  

    public void Update()
    {
        cursor = cursorList;

       
        
            // cimage.rectTransform.position =
            //     new Vector3(
            //         Input.mousePosition.x + cimage.rectTransform.sizeDelta.x * cimage.rectTransform.lossyScale.x *0.4f ,
            //         Input.mousePosition.y - cimage.rectTransform.sizeDelta.x *
            //         cimage.rectTransform.lossyScale.y *0.4f  , 0f);
                

        // }
       
    }

    public  void HideCursor()
    {
        cimage.enabled = false;
    }

    public  void ShowCursor()
    {
        cimage.enabled = true;
    }

    public  void ChangeCursor(string cursorName)
    {
        for (int i = 0; i < cursor.Count; i++)
        {
            if (cursor[i].cursorName ==cursorName)
            {
                cimage.sprite = Utils.CreateSprite(cursor[i].cursorImage);
                 UnityEngine.Cursor.SetCursor(cimage.sprite.texture,new Vector2(1.0f,1.0f),CursorMode.ForceSoftware);
        
                
            }
            
        }
    }
    
}
