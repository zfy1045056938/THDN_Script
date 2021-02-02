using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
/*
*
*
*/
public  class  Singleton<T> :MonoBehaviour where T  : MonoBehaviour{
     static T m_instance;

    public static T instance{
        get{
            m_instance = GameObject.FindObjectOfType<T>();
            //
            if (m_instance==null)
            {
                Debug.Log("Invaild Instance");
                
            }else{
                GameObject singleton = new GameObject(typeof(T).Name);
                m_instance = singleton.AddComponent<T>();
            }
            return m_instance;
        }
    }
	



	public virtual void Awake(){
        if(m_instance != null){
			//
            m_instance =this as T;
            transform.parent = null;
			//
            DontDestroyOnLoad(this.gameObject);

			}else {
				Destroy(this.gameObject);
			}
	}
}