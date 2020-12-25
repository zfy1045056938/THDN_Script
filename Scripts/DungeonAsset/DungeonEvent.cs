using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DEType
{
    None=0,
    Heal=1,
    Buff=2,   
    Damage=3,

}
[System.Serializable]
public class DungeonEvent : MonoBehaviour
{
    private int deID;
    private string deName;
    private float amount;
    private DEType _dType;
    public string deDetail;
    //
   

    public DungeonEvent() { }

    public DungeonEvent(int deID, string deName, float amount, DEType dType,string detail)
    {
        this.deID = deID;
        this.deName = deName;
        this.amount = amount;
        this.dType = dType;
        this.deDetail=detail;
    }

    public int DeID { get => deID; set => deID = value; }
    public string DeName { get => deName; set => deName = value; }
    public float Amount { get => amount; set => amount = value; }
    public DEType dType { get => _dType; set => _dType = value; }
}
