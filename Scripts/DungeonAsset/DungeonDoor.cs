using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PixelCrushers.DialogueSystem;
public enum DoorType{
    Normal,Lock,Special
}
//Check door type to interactive
public class DungeonDoor : MonoBehaviour
{
    private GameObject RollPanel;
    public Animator anim;
    public DoorType doorType;
    public bool isLock=false;
    private Dice dice;
    public float currentPerc;
    public float rollPerc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        anim.Play("Door Open");
    }

    public void RollDoor(){

    }
}
