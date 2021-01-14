using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter : IIdentifiable
{

    int MaxHealth { get; set; }
    int CreatureDef { get; set; }
    int CreatureAtk { get; set; }
    void Die();
  

}
public interface IIdentifiable
{
    int ID { get; }
  
}
