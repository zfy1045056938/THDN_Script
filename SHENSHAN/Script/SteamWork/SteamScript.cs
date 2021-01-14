using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
public class SteamScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (SteamManager.Initialized)
        {
            string name = SteamFriends.GetPersonaName();
            Debug.Log("Your Name\t"+name);
        }
    }
	
	
}
