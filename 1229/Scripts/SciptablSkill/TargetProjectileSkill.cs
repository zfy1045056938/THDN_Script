﻿using System;
using Mirror;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 
/// </summary>
public class TargetProjectileSkill : DamageSkill
{
    [Header("Projectile")]
    public ProjectileSkillEffect projectile; // Arrows, Bullets, Fireballs, ...

    //bool HasRequiredWeaponAndAmmo(Entity caster)
    //{
    //    int weaponIndex = caster.GetEquippedWeaponIndex();
    //    if (weaponIndex != -1)
    //    {
    //        // no ammo required, or has that ammo equipped?
    //        WeaponItem itemData = (WeaponItem)caster.equipment[weaponIndex].item.data;
    //        return itemData.requiredAmmo == null ||
    //               caster.GetEquipmentIndexByName(itemData.requiredAmmo.name) != -1;
    //    }
    //    return false;
    //}

    //void ConsumeRequiredWeaponsAmmo(Entity caster)
    //{
    //    int weaponIndex = caster.GetEquippedWeaponIndex();
    //    if (weaponIndex != -1)
    //    {
    //        // no ammo required, or has that ammo equipped?
    //        WeaponItem itemData = (WeaponItem)caster.equipment[weaponIndex].item.data;
    //        if (itemData.requiredAmmo != null)
    //        {
    //            int ammoIndex = caster.GetEquipmentIndexByName(itemData.requiredAmmo.name);
    //            if (ammoIndex != 0)
    //            {
    //                // reduce it
    //                ItemSlot slot = caster.equipment[ammoIndex];
    //                --slot.amount;
    //                caster.equipment[ammoIndex] = slot;
    //            }
    //        }
    //    }
    //}

    public override bool CheckSelf(Entity caster, int skillLevel)
    {
        // check base and ammo
        return base.CheckSelf(caster, skillLevel) ;
    }

    public override bool CheckTarget(Entity caster)
    {
        // target exists, alive, not self, oktype?
        return caster.target != null ;
    }

    //public override bool CheckDistance(Entity caster, int skillLevel, out Vector3 destination)
    //{
    //    // target still around?
    //    if (caster.target != null)
    //    {
    //        destination = caster.target.collider.ClosestPoint(caster.transform.position);
    //        return Utils.ClosestDistance(caster.collider, caster.target.collider) <= castRange.Get(skillLevel);
    //    }
    //    destination = caster.transform.position;
    //    return false;
    //}

    public override void Apply(Entity caster, int skillLevel)
    {
        // consume ammo if needed
        //ConsumeRequiredWeaponsAmmo(caster);

        // spawn the skill effect. this can be used for anything ranging from
        // blood splatter to arrows to chain lightning.
        // -> we need to call an RPC anyway, it doesn't make much of a diff-
        //    erence if we use NetworkServer.Spawn for everything.
        // -> we try to spawn it at the weapon's projectile mount
        if (projectile != null)
        {
            GameObject go = Instantiate(projectile.gameObject, caster.transform.position,Quaternion.identity)as GameObject;
            ProjectileSkillEffect effect = go.GetComponent<ProjectileSkillEffect>();
            effect.target = caster.target;
            effect.caster = caster;
            effect.damage = damage.Get(skillLevel);
            effect.stunChance = stunChance.Get(skillLevel);
            effect.stunTime = stunTime.Get(skillLevel);
            NetworkServer.Spawn(go);
        }
        else Debug.LogWarning(name + ": missing projectile");
    }
}