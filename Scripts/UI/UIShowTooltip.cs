using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIShowTooltip : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    
    public GameObject tooltipPrefab;

    [TextArea(0, 1)] public string text = "";

    private GameObject current;

    void CreateTooltip()
    {
        current=Instantiate(tooltipPrefab,transform.position,Quaternion.identity)as GameObject;
        //
        current.transform.SetParent(transform.root,true);
        current.transform.SetAsLastSibling();
        
        //
        current.GetComponentInChildren<Text>().text = text;

    }

    public void ShowTooltip(float delay)
    {
        Invoke(nameof(CreateTooltip),delay);
    }
    
    public bool IsVisiable()=>current!=null;


    void DestoryTooltip()
    {
        CancelInvoke(nameof(CreateTooltip));
        //
        Destroy(current);
    }
    // Update is called once per frame
    void Update()
    {
        if (current) current.GetComponentInChildren<Text>().text = text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip(0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       DestoryTooltip();
    }

    void OnDestory()
    {
        DestoryTooltip();
    }
}
