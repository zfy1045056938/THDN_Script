using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem{
public class testLOC : MonoBehaviour
{
    public TextTable textTable;
    void Start(){
        var table  = textTable.GetFieldTextForLanguage("test","CHN");
    }
}
}