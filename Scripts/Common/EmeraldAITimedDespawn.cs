using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class EmeraldAITimedDespawn:MonoBehaviour
    {
        public float SecondsToDespawn = 3;
        float Timer;

        void Update()
        {
            Timer += Time.deltaTime;
            if (Timer >= SecondsToDespawn)
            {
                EmeraldAIObjectPool.Despawn(gameObject);
            }
        }

        void OnDisable()
        {
            Timer = 0;
        }
    }

