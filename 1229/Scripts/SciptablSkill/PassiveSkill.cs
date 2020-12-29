using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using System;

public abstract class PassiveSkill:BounsSkill{

	 [Header("Damage")]
    public LinearInt damage = new LinearInt{baseValue=1};
    public LinearFloat stunChance; // range [0,1]
    public LinearFloat stunTime; // in seconds



    
    
	
}