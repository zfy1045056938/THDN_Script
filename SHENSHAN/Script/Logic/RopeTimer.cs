using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Rope timer.
/// </summary>
public class RopeTimer : MonoBehaviour,IEventSystemHandler
{
    public GameObject RopeGameObject;
    public Slider RopeSlider;
    public float TimeFOrOneTurn = 30.0f;
    public float RopeBurnTime;
    public Text timeText;
   
    //
    private float timerTillZero;
    private bool counting = false;
    private bool ropeIsBurning;

    [SerializeField]
    public UnityEvent timeExpired = new UnityEvent();


    /// <summary>
    /// Starts the timer.
    /// </summary>
    public void StartTimer(){
        timerTillZero = TimeFOrOneTurn;
        counting = true;
        ropeIsBurning = false;
        if (RopeGameObject != null)
        {
            RopeGameObject.SetActive(false);
        }
    }

    public void StopTimer(){

        counting = false;
    }

    private void Awake()
    {
        
        if (RopeGameObject!=null)
        {
            RopeSlider.minValue = 0;
            RopeSlider.maxValue = RopeBurnTime;
            RopeGameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (counting)
        {
            timerTillZero -= Time.deltaTime;
            if (timeText != null)
            {
                timeText.text = ToString();
            }

            if (RopeGameObject != null)
            {
                if (timerTillZero <= RopeBurnTime && !ropeIsBurning)
                {
                    ropeIsBurning = true;
                    RopeGameObject.SetActive(true);
                }

                //
                if (ropeIsBurning)
                {
                    RopeSlider.value = timerTillZero;
                }
            }

            //

            if (timerTillZero <= 0)
            {
                counting = false;
                timeExpired.Invoke();
            }
        }
    }


    public override string ToString()
    {
        int inSeconds = Mathf.RoundToInt(timerTillZero);
        //
        string justSeconds = (inSeconds % 60).ToString();
        //
        if (justSeconds.Length==1)
        {
            justSeconds = "0" + justSeconds;
        }
//        string justMinutes = (inSeconds / 60).ToString();
//        //
//        if (justMinutes.Length ==1)
//        {
//            justMinutes = "0" + justMinutes;
//        }
        return string.Format("{0}",justSeconds);
    }

}