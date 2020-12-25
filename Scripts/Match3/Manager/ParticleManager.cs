
using UnityEngine;


    public class ParticleManager : MonoBehaviour
    {
        public GameObject clearFXPrefab;
        public GameObject breakFXPrefab;
        public GameObject doubleBreakPrefab;
        public GameObject bombFXPrefab;
        
        
        
        public void BombFXAt(int x,int y ,int z=0)
        {
            if (bombFXPrefab != null)
            {
                GameObject bombFx = Instantiate(bombFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                ParticlePlayer p = bombFx.GetComponent<ParticlePlayer>();
                if (p != null)
                {
                    p.Play();
                }
            }
        }

        public void BreakTileFXAt(int breakableValue, int x, int i1, int i2)
        {
            GameObject breakFX = null;
            ParticlePlayer pp = null;
            if (breakableValue > 1)
            {
                if (doubleBreakPrefab != null)
                {
                    breakFX = Instantiate(doubleBreakPrefab, new Vector3(x, i1, i2), Quaternion.identity) as GameObject;
                }
            }
            else
            {
                breakFX = Instantiate(breakFXPrefab, new Vector3(x, i1, i2), Quaternion.identity) as GameObject;
            }
            //
            if (breakFX != null)
            {
                pp = breakFX.GetComponent<ParticlePlayer>();
                pp.Play();
            }
        }

        public  void  ClearPieceFXAt(int x,int y,int z=0)
        {
            if (bombFXPrefab != null)
            {
                GameObject bombFx = Instantiate(clearFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                ParticlePlayer p = bombFx.GetComponent<ParticlePlayer>();
                if (p != null)
                {
                    p.Play();
                }
            }
        }
    }
