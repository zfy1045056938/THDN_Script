using UnityEngine;
using System.Collections;


[System.Serializable]
public abstract class AbstractUIRoot:MonoBehaviour, IUserInterfaceRoot
{
  
    public abstract void Show();
    public abstract void Hide();

   
}
