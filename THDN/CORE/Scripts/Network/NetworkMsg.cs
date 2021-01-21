using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using Invector.vItemManager;
public partial class LoginMsg:MessageBase{
    public string account;
    public string pwd;
    public string version;
}

public partial class CharacterSelectMsg:MessageBase{
    public int index;
}

/// <summary>
/// 
/// </summary>
public partial class CharacterCreateMsg:MessageBase{
    public string names;
    public int className;
    //
    // public int de;
    // public int sc;
    // public int ka;
    // public int lp;
    // public int leader;
    
    //gamedata.avatarLib
    // public string iconName;
    //select model names
    //public string modelName;

}

public partial class CharacterDeleteMsg:MessageBase{
    public int index;
}

public partial class Error:MessageBase{
    public string text;
    public bool causeDisconnect;
}

public partial class CharacterAvailableMsg:MessageBase{
    public partial struct CharacterPreview{
        public string name;
        public string className;

    }
    public CharacterPreview[] characters;


public void Load(List<Players> players){
    characters = new CharacterPreview[players.Count];
    for(int i=0;i<players.Count;++i){
        Players p= players[i];
        characters[i] = new CharacterPreview{
            name=p.name,
            className = p.className,
            // equipment = p.equipment.ToArray()
        };

    }
    Util.InvokeMany(typeof(CharacterAvailableMsg),this,"Load_",players);
}

}

public partial class LoginSuccessMsg:MessageBase{

}

public partial class ErrorMsg:MessageBase{
    public string msg;
    internal bool causesDisconnect;
}




