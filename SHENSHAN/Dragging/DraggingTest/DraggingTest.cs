using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 拖动测试
/// </summary>
public class DraggingTest : MonoBehaviour {

    public bool usePointerDiscplacement = true;

    //
    public bool dragging = true;

    //default pointdisplacement
    public Vector3 pointerDisplacement = Vector3.zero;


    //
    public float zDisplacement;


    /// <summary>
    /// when press the mouse down
    /// </summary>
    private void OnMouseDown()
    {
        dragging = true;
        zDisplacement = -Camera.main.transform.position.z + transform.position.z;

        if (usePointerDiscplacement)
        {
            pointerDisplacement = -transform.position + MouseInWorldCoords();
        }
        else
            pointerDisplacement = Vector3.zero;
    }



    private void Update()
    {
        if (dragging)
        {
            Vector3 mousePos = MouseInWorldCoords();
            //
            transform.position = new Vector3(mousePos.x - pointerDisplacement.x,0f,0f);
        }
    }


    /// <summary>
    /// Ons the mouse up.reset the mouse press
    /// </summary>
    private void OnMouseUp()
    {
        if (dragging)
        {
            dragging = false;
        }
    }

    /// <summary>
    /// Mouses the in world coords.
    /// </summary>
    /// <returns>The in world coords.</returns>
    public Vector3 MouseInWorldCoords(){
        var screenMousePos = Input.mousePosition;
        screenMousePos.z = zDisplacement;

        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }
}
