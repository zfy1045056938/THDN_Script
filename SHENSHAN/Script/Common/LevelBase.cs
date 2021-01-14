using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ExponentialLong{
    public float bv;
    public float mult;
    public float Get(int level) => (mult*Mathf.Pow(bv,(level-1)));
}
