using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class CraftTab : MonoBehaviour,IPointerClickHandler
{
    // Start is called before the first frame update
    public CraftingTabType tabType;

    private CraftManager crafting;

    public Image image;

    public TextMeshProUGUI nameText;
   
    public List<CraftedItem> items;

    public void Start() {
        crafting = GameObject.FindGameObjectWithTag("CraftSystem").GetComponent<CraftManager>();
    }

    public void OnPointerClick(PointerEventData data) {
        crafting.ChangeCraftingTab(this);

    }
}
