using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonArchitect;

public class NpcSpawner : DungeonEventListener
{
    public GameObject parentObj;
    public GameObject [] template;
    public Vector3 npcOffset = Vector3.zero;
    public float spawnProp = 0.25f;

    public override void OnPostDungeonLayoutBuild(Dungeon dungeon, DungeonModel model){
        RebuildNpc();
    }

    public void RebuildNpc(){
        // DestroyOldNPC();
        // if(template.Length==0)return;
        // var wayPoint =
    }
}
