using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class CommonMenuManager : MonoBehaviour
{
   [SerializeField]
   private CommmonMenu mainMenuPrefab;
   [SerializeField]
    private Transform _menuParent;
   [SerializeField]
   private SettingMenu SettPrefab;
   [SerializeField]
   private PauseMenu pauseMenuPrefab;

   private Stack<Menu> _menuStack= new Stack<Menu>();

   private static CommonMenuManager _instance;
   public static CommonMenuManager instance {get{return _instance;}}

   //
   private void Awake(){
       _instance =this;
       InitMenu();
       DontDestroyOnLoad(gameObject);
   }

   private void OnDestory(){
       _instance=null;
   }

   private void InitMenu(){
       if(_menuParent == null){
           GameObject obj = new GameObject("Menus");
           _menuParent = obj.transform;
       }

       //
       DontDestroyOnLoad(_menuParent.gameObject);

       //
       BindingFlags mflags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
       FieldInfo[] fields = this.GetType().GetFields(mflags);

       //
       foreach(FieldInfo  f in fields){
           Menu mp = f.GetValue(this) as Menu;
           if(mp!=null){
               Menu minstance  = Instantiate(mp,_menuParent);

           }else{
               OpenMenu(mp);
           }
       }
   }

   public void OpenMenu(Menu instances){
       if(instance == null){
           return;
       }

       if(_menuStack.Count >0){
           foreach(Menu menu in _menuStack){
               menu.gameObject.SetActive(false);
           }
       }
       //
       instances.gameObject.SetActive(true);
       _menuStack.Push(instances);
   }

   public void CloseMenu(){
       if(_menuStack.Count==0){return;}

       Menu topMenu = _menuStack.Pop();
       topMenu.gameObject.SetActive(false);

       //
       if(_menuStack.Count>0){
           Menu nextMenu = _menuStack.Peek();
           nextMenu.gameObject.SetActive(true);
       }
   }
}
