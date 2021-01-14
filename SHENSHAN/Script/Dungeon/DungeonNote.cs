using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonNote : MonoBehaviour
{
  public static DungeonNote instance;
  public GameObject content;
  public Text titleText;
  public Text detailText;


  void Start()
  {
    instance = this;
  }

  public IEnumerator ShowRoutline()
  {
    
    
    
    yield return new WaitForSeconds(0.4f);
  }
}
