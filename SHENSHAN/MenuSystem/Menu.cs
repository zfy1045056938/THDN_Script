using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Menu<T> : Menu where T : Menu<T>
{
  protected static T _instance;
  public static T instance{get{return _instance;}}

  protected virtual void Awake(){
      if(instance==null)_instance=(T)this;
  }

  protected virtual void OnDestory(){
      _instance=null;
  }

  public static void Open(){
      if(CommonMenuManager.instance!=null && instance!=null){
          CommonMenuManager.instance.OpenMenu(instance);
      }
  }
}



  [RequireComponent(typeof(Canvas))]
  public abstract class Menu:MonoBehaviour{
      public virtual void OnBackGround(){
          if(CommonMenuManager.instance!=null){
              CommonMenuManager.instance.CloseMenu();
          }
      }
  }



