using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public partial class LoginMsg : MessageBase
{
    public string account;
    public string password;
    public string version;
}

public partial class CharacterDeleteMsg : MessageBase{
    public int index;
}

public partial class LoginSuccessMsg :MessageBase{
    
}

public partial class CharacterCreateMsg : MessageBase
{
    public string name;
public int classIndex;

}



public partial class CharacterSelectMsg : MessageBase
{
    public int index;
}
// server to client ////////////////////////////////////////////////////////////
// we need an error msg packet because we can't use TargetRpc with the Network-
// Manager, since it's not a MonoBehaviour.
public partial class ErrorMsg : MessageBase
{
    public string text;
    public bool causesDisconnect;
}

public partial class CharactersAvailableMsg : MessageBase
{

    public partial struct CharacterPreview{
        public string name;
        public string className;
    }

    public CharacterPreview[] characters;

    
    public void Load(List<PlayerData> p)
    {
     characters=new CharacterPreview[p.Count];
     for (int i = 0; i < characters.Length; ++i)
     {
         PlayerData ps = p[i];
         characters[i] = new CharacterPreview
         {
             name = ps.name,
             className=ps.className,
         };

     }
     
     //
     Utils.InvokeMany(typeof(CharactersAvailableMsg),this,"Load_",p);
    }
}

public partial class SaveSystemMsg : MessageBase
{
    public int index;
}

public partial class ChangeSceneMsg : MessageBase
{
    public string sceneName;
}