using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BombType{
    None,
}
public class Bomb : GamePieces
{

    public BombType bombType= BombType.None;

}
