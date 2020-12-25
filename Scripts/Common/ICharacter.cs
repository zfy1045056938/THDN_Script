﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter : IIdentifiable
{

    int Health { get; set; }
    void Die();
  

}
public interface IIdentifiable
{
    int ID { get; }
  
}
