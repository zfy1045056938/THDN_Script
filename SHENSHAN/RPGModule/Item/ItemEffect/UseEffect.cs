using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Potion Use Effect
public class UseEffect : MonoBehaviour
{
    public Items item;

    public PlayerData playerData;

    public MessageManager messageManager;

    public InventorySystem inventory;

    private void Awake()
    {
        messageManager = GameObject.FindObjectOfType<MessageManager>();
        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        inventory = GameObject.FindObjectOfType<InventorySystem>();
        
    }

    public virtual string Use()
    {
        return "";
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
