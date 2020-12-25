using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// when skil 
/// </summary>
public class UISkillSlot:MonoBehaviour,IPointerEnterHandler,IPointerClickHandler
    {
        // public UIShowTooltip tooltip;
        //runtime data
        public Skill currentSkill;
        private UISkill skillpanel;
    public SkillType skillType = SkillType.None;    
        //public UIDragAndDropable dragAndDropable;
        public Image image;
        public Button button;
        // public GameObject cooldownOverlay;
    public float skilExp;
    public Slider sExpSlider; 
        // public TextMeshProUGUI cooldownText;
        // public TextMeshProUGUI skillName;
        // public TextMeshProUGUI detailText;
        public TextMeshProUGUI levelText;
        // public Image cooldownCircle;
    public int sIndex = -1;      // check player has select if select info und Notice UI show it (skillIndex=slot.index)
       

    public UnityEngine.Events.UnityEvent OnLearn,OnUpgraden,OnEquip,OnUnEquip;

    //spl
    public Dictionary<GamePiece, int> gpList;   //core module 

        public GameObject noticeTabs;


/// <summary>
/// Start is called on the frame when a script is enabled just before
/// any of the Update methods is called the first time.
/// </summary>
void Start()
{
    skillpanel = FindObjectOfType<UISkill>().GetComponent<UISkill>();
}
    public void OnPointerClick(PointerEventData eventData)
    {
        //double click add point to current skill
        if (eventData.clickCount > 1 &&eventData.button==PointerEventData.InputButton.Left)
        {
            Debug.Log("u click twice");
            //relative learn or  upgrade the skill 
            if (currentSkill.level > 0)
            {
                //upgrade
                skillpanel.UpgradeSkill(currentSkill.hash);
            }
            else
            {
                //learn skill
                skillpanel.LearnSkill(currentSkill.hash);
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       skillpanel.UpdateSkill(currentSkill.data);
    }


}

