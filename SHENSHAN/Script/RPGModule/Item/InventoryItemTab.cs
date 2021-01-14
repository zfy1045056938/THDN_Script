using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public enum InventoryTabType{
    All,
    Equipment,
    Potion,
    Junk,
    Other,
    Quest,
}
/// <summary>
/// Inventory Tab Select
/// </summary>
public class MerchantInventoryItemTabTabs : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public List<ItemManager> items;

    public InventoryTabType tabType = InventoryTabType.All;

    private InventorySystem inventory;

    public Image glowImage;

    public void Awake()
    {
        glowImage = GetComponentInChildren<Image>();
    }
    // Use this for initialization
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventorySystem>();
    }

    public void OnPointerClick(PointerEventData data)
    {
        // inventory.ch(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (glowImage!=null)
        {
            glowImage.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (glowImage != null)
        {
            glowImage.gameObject.SetActive(true);
        }
    }
}