using System.ComponentModel;
namespace Invector.vItemManager {
     public enum vItemType {
       [Description("Consumable")] Consumable=0,
       [Description("Melee")] MeleeWeapon=1,
       [Description("Shooter")] ShooterWeapon=2,
       [Description("Ammo")] Ammo=3,
       [Description("Archery")] Archery=4,
       [Description("Builder")] Builder=5,
       [Description("CraftingMaterials")] CraftingMaterials=6,
       [Description("Defense")] Defense=7,
       [Description("Junk")]Junk=8,
       [Description("Hats")]Hats=9,
       [Description("Armor")] Armor=10,
       [Description("Set")] Set=11,
       
     }
     public enum vItemAttributes {
       [Description("")] Health=0,
       [Description("")] Stamina=1,
       [Description("<i>Damage</i> : <color=red>(VALUE)</color>")] Damage=2,
       [Description("")] StaminaCost=3,
       [Description("")] DefenseRate=4,
       [Description("")] DefenseRange=5,
       [Description("(VALUE)")] AmmoCount=6,
       [Description("")] MaxHealth=7,
       [Description("")] MaxStamina=8,
       [Description("(VALUE)")] SecundaryAmmoCount=9,
       [Description("")] SecundaryDamage=10
     }
}
