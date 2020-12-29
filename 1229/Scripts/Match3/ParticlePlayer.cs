using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{

    public ParticleSystem[] player;

    public float lifeTime = 1f;

    public bool dimm = true;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponents<ParticleSystem>();
        
        if(dimm){Destroy(gameObject,lifeTime);}
    }

    public void Play()
    {
        foreach (ParticleSystem p in player)
        {
            p.Stop();
            p.Play();
            
        }
        Destroy(gameObject,lifeTime);
    }

   
}
