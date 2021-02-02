using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Table:MonoBehaviour
{
    public List<CreatureLogic> creatureOnTable = new List<CreatureLogic>();
    //
    public void PlaceCreatureAt(int index, CreatureLogic creature) { creatureOnTable.Insert(index, creature); }
}