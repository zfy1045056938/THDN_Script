using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class CustomerCursor{
    public Sprite cursorImage;
    public string cursorName;
}


/// <summary>
/// Cursor manager.
/// </summary>
public class CursorManager : MonoBehaviour {


    public List<CustomerCursor> cursorList;
    private static List<CustomerCursor> cursors;
    private static Image image;

    //

	// Use this for initialization
	void Start () {
        cursors = cursorList;
        image = GetComponent<Image>();
        Cursor.visible = false;

       
    }
	
	// Update is called once per frame
	void Update () {
        //
        cursors = cursorList;
        //
        image = GetComponent<Image>();
        //
        if (image.enabled)
        {
            image.transform.position =
                     new Vector3(Input.mousePosition.x + image.rectTransform.sizeDelta.x * image.rectTransform.lossyScale.x * 0.5f,
                                                   Input.mousePosition.y + image.rectTransform.sizeDelta.y * image.rectTransform.lossyScale.y * 0.5f);

        }
    }

    public void ShowCursor(){
        image.enabled = true;
    }
    public void HideCursor(){
        image.enabled = false;
    }

    //
    public  void ChangeCursor(string cursorName){
        for (int i = 0; i < cursors.Count; i++)
        {
            if (cursors[i].cursorName==cursorName)
            {
                image.sprite = cursors[i].cursorImage;
            }
        }
    }
}
