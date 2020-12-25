using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Mirror;
using Invector.vItemManager;
//items who can use or equip
//LEVELREQUIRED: p.level> i.level
//OTHERREQUIRED: p.sdi>i.level
//consumable items likes potions etc.
public abstract class UsableItem : vItem
{
    
    
    [Header("Usable")]
    //declare usable Item variable
    public int itemLevel;
    public int coldDown;


    [SerializeField] private string _cooldownCategory;

    public string CooldownCategory => string.IsNullOrWhiteSpace(_cooldownCategory) ? name : _cooldownCategory;

  


    public virtual bool CanUse(Players p, int inventoryIndex)
    {
        return p.level > itemLevel && p.GetItemCoolDown(CooldownCategory) == 0;
    }

   


    public virtual  void Use(Players p, int inventoryIndex)
    {
        if (coldDown > 0)
        {
            p.SetItemCoolDown(CooldownCategory, coldDown);
        }
    }

    public virtual void Onuse(Players p){}
}