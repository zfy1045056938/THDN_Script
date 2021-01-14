using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Deck:MonoBehaviour
{


    public List<CardAsset> cards = new List<CardAsset>();


    void Awake()
    {
        cards.Shuffle();
    }

}