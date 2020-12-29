using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

/// <summary>
/// use for dungeon & matches board
/// 
/// </summary>
  public class Dungeon_SkillBar:MonoBehaviour
    {


    public static Dungeon_SkillBar instance;

   
    public GameObject panel;
    public UISkillSlot slotPrefab;
    //
    public PartyGuy partyGuy;
    public SkillbarEntry[] skillBar;
    public CollectionsPool collectionPools;
    public Battle_RagePanel ragePanel;
    
    public Transform content;

   


    public int currentIndex = -1;

 
    void Update()
    {
        Players player = Players.localPlayer;
        if (player)
        {
            
            // only update the panel if it's active
            if (panel.activeSelf )
            {
                // instantiate/destroy enough slots
                // (we only care about non status skills)
                UIUtils.BalancePrefabs(slotPrefab.gameObject, player.skills.Count, content);

                // refresh all
                for (int i = 0; i < player.skills.Count; ++i)
                {
                    UISkillSlot slot = content.GetChild(i).GetComponent<UISkillSlot>();
                    Skill skill = player.skills[i];

                    bool isPassive = skill.data is PassiveSkill;

               

                    // click event
                    slot.button.interactable = skill.level > 0 &&
                                               !isPassive &&
                                               player.CheckSelf(skill); // checks mana, cooldown etc.

                    //party guy? cand
                    partyGuy.selectBtn.interactable = panel.activeSelf && TownManager.instance.atDungeon == false;

                    //

                    int icopy = i;

                    //


                    slot.button.onClick.AddListener(() => {
                        // try use the skill or walk closer if needed
                        // player.TryUseSkill(icopy);
                        slot.currentSkill = skill;
                        slot.sIndex = icopy; //when select show notice und detail infos
                        currentIndex = icopy;
                    });

                    // image
                    // if (skill.level > 0)
                    // {
                    //     slot.image.color = Color.white;
                    //     slot.image.sprite = skill;
                    // }

                    // description
                    // slot.descriptionText.text = skill.ToolTip(showRequirements: skill.level == 0);



                    // cooldown overlay at dungeon skill ui
                    float cooldown = skill.CooldownRemaining();
                    // slot.cooldownOverlay.SetActive(skill.level > 0 && cooldown > 0);
                    // slot.cooldownText.text = cooldown.ToString("F0");
                    // slot.cooldownCircle.fillAmount = skill.cooldown > 0 ? cooldown / skill.cooldown : 0;
                }

              
            }
        }
        else panel.SetActive(false);
    }
}

