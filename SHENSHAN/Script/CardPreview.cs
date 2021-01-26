using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayfulSystems;
public class CardPreview : MonoBehaviour
{
  public GameObject content;
  public OneCardManager cardPreview;
  // public Sprite cardView;
  public float delay = 0f;
  public bool startCount = false;
  public static CardPreview instance;
 public  ProgressBarPro pro;
 public float timer = 1.0f;

 void Start()
 {
   
   StartCoroutine(DelayViewRoutine());
 }
 
 public IEnumerator DelayViewRoutine()
 {
   
    Debug.Log("Start Stay at spot");
     
     pro.SetValue(timer);

     StartCoroutine(CountRoutine());

     while (!startCount )
     {
       yield return null;
     }
     
     
     if (startCount == false)
     {
       Debug.Log("Count is 0");
       content.SetActive(false);
     }
   

 }

 public IEnumerator CountRoutine()
 {
   while (timer > 0)
   {
     
     yield return new WaitForSeconds(0.8f);
     pro.SetValue(timer-=0.2f);

     if (timer <= 0f)
     {
       Debug.Log("time=0");
       startCount = false;
       content.SetActive(false);
     }
   }
   
   
 }
 
 
  public void PreviewCard(OneCardManager card)
  {
    delay = 3.0f;
    content.SetActive(true);
    startCount = true;
    cardPreview.cardAsset = card.cardAsset;
    cardPreview.ReadCardFromAsset();
  }

  

  
}
