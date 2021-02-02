using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;
public class SelectableCharacter:MonoBehaviour
{
    public int index=-1;

   
    void Update()
    {
        if(((NetworkManagerShenShan)NetworkManager.singleton).selection !=index);
        
    }


    public void OnMouseDown()
    {
         
        Debug.Log("uClick the player");
        ((NetworkManagerShenShan)NetworkManagerShenShan.singleton).selection = index;
        //
    }
}