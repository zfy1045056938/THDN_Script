using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class CraftMaterials : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public TextMeshProUGUI nametext;
    
    public TextMeshProUGUI current;
   
    public Image icon;
    public Items mItem;
    private CraftManager crafting;

    public void Start() {
        crafting = GameObject.FindObjectOfType<CraftManager>().GetComponent<CraftManager>();
    }

    public void OnPointerEnter (PointerEventData data) {
        crafting.OnMaterialIconEnter(this);
    }
	
    public void OnPointerExit (PointerEventData data) {
        crafting.OnMaterialIconExit(this);
    }
}
