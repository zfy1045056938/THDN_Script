using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Mirror;
public  class ScriptableBuff:ScriptableObjectNonAlloc


    {

    public virtual bool CheckSelf(Entity entity, int level, bool hasBuff) {
        return true;
    }
    public virtual bool CheckTarget(Entity entity, int level, bool hasBuff) {
        return true;

    }


}

