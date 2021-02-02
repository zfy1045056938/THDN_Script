using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;



/// <summary>
/// Merchant tabs.
/// </summary>
public class MerchantTabs:MonoBehaviour,IPointerClickHandler,IPointerEnterHandler
{
    public List<Items> items;

    public TabType tabType ;

    private Merchant merchant;

    
    // Use this for initialization
    void Start()
    {
        merchant = GameObject.FindGameObjectWithTag("Merchant").GetComponent<Merchant>();
    }

    public void OnPointerClick(PointerEventData data)
    {
        merchant.ChangeTab(this);
    }
 public IEnumerator CusorChange(string cursorName)
    {
        //CursorManager.ChangeCursor(cursorName);
        yield return new WaitForSeconds(0.4f);
    }
    
    public string cursorName ="HoverObject";
    public void OnPointerEnter(PointerEventData data)
    {
        Debug.Log("U Enter The tabType");
        
        StartCoroutine(CusorChange(cursorName));
        
        
       
    }

  
   
}