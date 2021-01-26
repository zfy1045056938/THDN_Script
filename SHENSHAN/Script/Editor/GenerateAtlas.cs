using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class GenerateAtlas : Editor
{
  
  [MenuItem("Assets/CreateAltas")]
  static private void CreateAltas(){
      string sDir = Application.dataPath + "/Resources/test";

      if(!Directory.Exists(sDir)){
            Directory.CreateDirectory(sDir);
      }

      //
      DirectoryInfo rDirInfo = new DirectoryInfo(Application.dataPath+"/Altas");
      
  }
}
