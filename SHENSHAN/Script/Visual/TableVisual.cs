using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using System;
using PixelCrushers.DialogueSystem;

//桌面行为管理包含以下
//1.管理桌面随从位置(以索引方式存储随从):（当场面存在同名随从，则叠加属性(磁力属性))
//2.对牌桌上的随从进行控制权限(当处于当前回合)

public class TableVisual : MonoBehaviour
{
    public AreaPositions areaPosition;

    public SpriteRenderer noticeImg;
    public List<GameObject> creatureOnTable = new List<GameObject>();
  public SameDistanceChildren slots;
    //
    private bool cursorOverThisTable = false;

    private BoxCollider col;
    
    public AudioClip placeSound;

    public static TableVisual instance;
    public Transform dpPos;
    /// <summary>
    /// Gets a value indicating whether this <see cref="T:TableVisual"/> cursor over some table.
    /// </summary>
    /// <value><c>true</c> if cursor over some table; otherwise, <c>false</c>.</value>
    public static bool CursorOverSomeTable
    {
        get
        {
            Debug.Log("U Hover The Table");
            //Find GameObject of BothTable
            TableVisual[] bothTables =FindObjectsOfType<TableVisual>();
            return (bothTables[0].CursorOverThisTable || bothTables[1].CursorOverThisTable);
        }
    }
    private void Awake()
    {
        if(instance==null)instance=this;
        col = GetComponentInChildren<BoxCollider>();
        
    }

    public bool CursorOverThisTable
    {
        get { return cursorOverThisTable; }  
    }
  

    void Update()
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 1000f);
        bool passedThroughTableToCollider = false;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == col)
            {
                passedThroughTableToCollider = true;
            }


        }
        cursorOverThisTable = passedThroughTableToCollider;
    }


    //将桌面上的随从通过索引添加进
    //1.creature GameObject
    //2.OneCreatureManager
    //3.WhereCardOfCreature
    //4.IDHolder=>uniqueID
    //5.CheckSlotIsNULL
    //6.PlaceNewSlotToDeck 
    //7.CommandExecute

    public void AddCreatureAtInIndex(CardAsset ca, int uniqueID, int index)
    {
        
        //generate creature instance
//     
        GameObject creatures = Instantiate(GlobalSetting.instance.CreaturePrefab, slots.children[index].transform.position,Quaternion.identity)as GameObject;


      
            //sound
            SoundManager.instance.PlayClipAtPoint(placeSound, Vector3.zero, SoundManager.instance.musicVolume, false);

            //
            OneCreatureManager oneCreature = creatures.GetComponent<OneCreatureManager>();
            oneCreature.cardAsset = ca;
            oneCreature.ReadCreatureFromAsset();
            Debug.Log("Add DB For c==>"+oneCreature.cardAsset.name.ToString());
        //    oneCreature.AddBouns();///

            creatures.transform.SetParent(slots.transform);
            creatures.transform.localScale = Vector3.one;

//            foreach (Transform t in creatures.GetComponentInChildren<Transform>())
//
//                t.tag = areaPosition.ToString() + "Creatures";

            foreach (Transform ts in creatures.GetComponentsInChildren<Transform>())
            {
                ts.tag = areaPosition.ToString() + "Creatures";
            }
           
            
            //Add Creature To dic
            creatureOnTable.Insert(index,creatures);
            
            //
            WhereIsTheCardOfCreature w = creatures.GetComponent<WhereIsTheCardOfCreature>();
            w.slot = index;
            if (areaPosition == AreaPositions.Low)
            {
                w.visualState = VisualStates.LowTable;
            }
            else if (areaPosition == AreaPositions.Top)
            {
                w.visualState = VisualStates.TopTable;
            }

            IDHolder ids = creatures.gameObject.AddComponent<IDHolder>();
            ids.uniqueID = uniqueID;
            
            //
          oneCreature.CanAtkNow=false;
          //Add component with (Only SIngle Mode)
          if (creatures.tag == "TopCreatures")
          {
              
                      creatures.gameObject.AddComponent<IncrementOnDestroy>().variable =
                          "enemiesKilled";
                      creatures.gameObject.GetComponentInChildren<IncrementOnDestroy>().max =
                          3;
                     
                      creatures.gameObject.GetComponentInChildren<IncrementOnDestroy>().increment =
                          1;

                      creatures.gameObject.GetComponentInChildren<IncrementOnDestroy>().onIncrement.AddListener(() =>
                      {
                          creatures.GetComponentInChildren<DialogueSystemTrigger>().OnUse();
                      });
                  
          }

          ShiftSlotsGameObjectAccordingToNumberOfCreature();
           
           //place creature
            PlaceCreatureOnNewSlot();
            //Show preview pos
            


            Sequence s = DOTween.Sequence();
            s.PrependInterval(0.4f);
            s.OnComplete(() => { Command.CommandExecutionComplete(); });
        
    }



//    //Check the table of CreatureCount if mouse point the table >0 then return count else Count +1 
    public int TablePosForNewCreature(float mouseX)
    {
        if (creatureOnTable.Count == 0 || mouseX >slots.children[0].transform.position.x)
            return 0;
        else if (mouseX < slots.children[creatureOnTable.Count-1].transform.position.x)
            return creatureOnTable.Count;
        //
        for (int i = 0; i < creatureOnTable.Count; i++)
        {
            if (mouseX < slots.children[i].transform.position.x &&
                mouseX > slots.children[i+1].transform.position.x)
                return i + 1;
        }
          return 0;
  
    }

    /// <summary>
    /// Removes the creature with identifier.
    /// </summary>
    /// <param name="idRemove">Identifier remove.</param>
    public void RemoveCreatureWithID(int idRemove)
    {

        GameObject cToReMove = IDHolder.GetComponentWithID(idRemove);

//        Sequence sq = DOTween.Sequence();
//        cToReMove.transform.parent = dpPos;
//        sq.Append(cToReMove.transform.DOMove(dpPos.position, 1.0f));
//       sq.Insert(0f, cToReMove.transform.DORotate(Vector3.zero, 0.4f));
        
        creatureOnTable.Remove(cToReMove);
        Destroy(cToReMove);
        
        //
        ShiftSlotsGameObjectAccordingToNumberOfCreature();
        PlaceCreatureOnNewSlot();
        Sequence s = DOTween.Sequence();
        s.OnComplete(() =>
        {
            Command.CommandExecutionComplete();
            
        });
        
    }

   

    /// <summary>
    /// Shifts the slots game object according to number of creature.
    /// </summary>
    public void ShiftSlotsGameObjectAccordingToNumberOfCreature()
    {
        float posX;
        if (creatureOnTable.Count > 0)
        {
            posX = (slots.children[0].transform.localPosition.x - slots.children[creatureOnTable.Count-1].transform.localPosition.x) / 2f;

        }
        else
        
            posX = 0f;
        
        //creature animation when place new creature between 2 creatures
        slots.gameObject.transform.DOLocalMoveX(posX, 1.0f);
    }

    //place creature into thw slot(if NULL)
    public void PlaceCreatureOnNewSlot()
    {
        foreach (GameObject g in creatureOnTable)
        {
            //todo
           
                g.transform.DOLocalMoveX(slots.children[creatureOnTable.IndexOf(g)].transform.localPosition.x, 0.3f);
            
           

        }
    }
}
