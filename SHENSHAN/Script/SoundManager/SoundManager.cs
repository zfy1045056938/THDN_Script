using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager :MonoBehaviour
{
//    public AudioSource sm;
//    public AudioClip[] mainBG;
    public AudioClip[] musicClips;
 
    public AudioClip[] UIClips;
    public AudioClip[] combatClips;
    //EquipSound
    public AudioClip levelClip;
    public AudioClip atkClip;
    
    public static SoundManager instance;

    [Range(0, 1)] public float musicVolume = 0.5f;
    [Range(0, 1)] public float fxVolume = 1.0f;

    public float lowPitch = 0.95f;
    public float highPitch = 1.05f;


    void Awake()
    {
   
        instance = this;
      
    }
    void Start()
    {
        DontDestroyOnLoad(this);
//        sm=GetComponent<AudioSource>();
//        Pla yMainBG(mainBG,Vector3.zero,musicVolume);
//     
       
    }

    void Update()
    {
//        if (sm != null)
//        {
//            sm.volume = musicVolume;
//        }
    }


    public AudioSource PlayClipAtPoint(AudioClip clip, Vector3 pos, float volume, bool randomPitch = true)
    {
       // StartCoroutine(SoundRoutline(clip, pos, volume, true));
     

        return null;
    }

    private IEnumerator SoundRoutline(AudioClip clip, Vector3 pos, float volume, bool randomPitch=true)
    {
        if (clip != null)
        {
            GameObject obj = new GameObject("SoundFX" + clip.name);
            Debug.Log(obj.name);
            obj.transform.position = pos;

            //
            AudioSource source = obj.AddComponent<AudioSource>();
            source.clip = clip;
            
//            
//            source.loop = true;
//           
//          
//            source.clip = clip;
//
//            musicVolume = volume;
            //
            if (randomPitch)
            {
                float rnd = Random.Range(lowPitch, highPitch);
                source.pitch = rnd;
            }

            //
            source.volume = volume;
            
            //
            source.Play();

            Destroy(obj, clip.length);

           yield return new WaitForSeconds(0.4f);
        }

        yield return null;
    }

    public AudioSource PlayClipAtPoints(AudioClip clip, Vector3 pos, float volume, bool randomPitch = true)
    {
//        if (clip != null)
//        {
//            sm.loop = true;
//            sm.clip = clip;
//            //
//            if (randomPitch)
//            {
//                float rnd = Random.Range(lowPitch, highPitch);
//                sm.pitch = rnd;
//            }
//            //
//            sm.volume = musicVolume;
//            //
//            sm.Play();
//            //
//           Destroy(sm);
//         
//            return sm;
//        }
//
//        return null;
        return null;
    }

    //
    public void PlayRndMusic()
    {
         PlayRandom(musicClips, Vector3.zero, musicVolume);
    }

//    public AudioSource PlayWinMusic()
//    {
//        return PlayRandom(winClips, Vector3.zero, musicVolume);
//    }
//
//    public AudioSource PlayLoseMusic()
//    {
//        return PlayRandom(loseClips, Vector3.zero, musicVolume);
//    }
//
//    public void PlayUIClip()
//    {
//        PlayRandom(UIClips, Vector3.zero, musicVolume);
//
//    }
//    
//    public AudioSource PlayeCombatSound(){
//        return PlayRandom(combatClips,Vector3.zero,musicVolume);
//    }
    
    //UI Sound
    public void PlaySound(AudioClip clip)
    {
       StartCoroutine(SoundRoutline(clip,Vector3.zero, 1.0f,false));
    }



public void PlayRandom(AudioClip[] clip, Vector3 pos, float volume = 1.0f)
    {
        if (clip != null)
        {
            if (clip.Length !=0)
            {
                int rnd = Random.Range(0, clip.Length );
                if (clip[rnd] != null)
                {
//                    AudioSource source = PlayClipAtPoints(clip[rnd], pos, volume);
                    StartCoroutine(SoundRoutline(clip[rnd], pos, volume, false));
                }
                
            }
        }

    }
    
public void PlayMainBG(AudioClip[] clip, Vector3 pos, float volume = 1.0f)
{
    if (clip != null)
    {
        if (clip.Length !=0)
        {
            int rnd = Random.Range(0, clip.Length );
            if (clip[rnd] != null)
            {
//                AudioSource source = PlayClipAtPoints(mainBG[rnd], pos, volume);

                StartCoroutine(SoundRoutline(clip[rnd],pos,volume,false));
               
            }
                
        }
    }
}
    

}
