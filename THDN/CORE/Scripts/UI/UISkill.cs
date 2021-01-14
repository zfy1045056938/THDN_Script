using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;
using Invector.vItemManager;
using Invector.vCamera;
using Invector.vCharacterController;
using UnityEngine.EventSystems;
using System.Linq;




///ChangeLogs 1216
///use by masterwindow there have 2 skill panel for player  config
///1.BattleSkill
///default panel list und upgrade by skill every level up (1+*(Effect))
/// can use init skill needs  equip at the skilldisplay window for quick use
/// same as the android platform with quick equipdisplay und qs
/// 
/// 
///2.Personal Skill
/// ps for the player expect battle skill use for the scene und business ,
///  
///
/// the skill config by horizon placement with  transform array , needs check the partner to set
/// default character skill info that unlock the config when enough sp.
///
/// The whole skill will save at gameDB (list&dict) that player can got by dic or linq
/// </summary>
public class UISkill : MonoBehaviour
{
    private Players player;
    public Animator anim;

    //
    public Character currentCharacters; //default 0 , Init From Init
    public static UISkill instance;
   //when select show details
    public int selectSkillIndex = -1;   
    //
    public GameObject psPanel;
    public GameObject bsPanel;

    //
    [Header("Character Panel List")]
    public List<GameObject> characterList;
    public Transform characterPos;
    public List<GameObject> battleSkillList;
    public Transform bsPos;
    public List<GameObject> personalSkillList;
    public Transform psPos;

    //

    public bool canEdit = true; //when boss stage can't edit
    //non_null ,default is kuno  
    public int characterIndex = 0;
    public GameObject charactersPrefab;    //
    public int spoints = -1;
    public int ppoints = -1;


    [Header("SKill Area")]
    public bool fullArea = false;
    public List<GameObject> skillArea;  //same as equiparea

    public GameObject[] skillSlot;
    
    


    public GameObject panel;
    public GameObject slotPrefab;


    public bool canConfig = false;


   

    //Generic input
    GenericInput opencloseSkill = new GenericInput("P", "Start", "Start");



    public bool canLearn, canLevel, canEquip, canUnEquip;



    [Header("Select Detail")]
    public TextMeshProUGUI spText;
    public TextMeshProUGUI ppText;
    //
    public TextMeshProUGUI currentTeamNum;
    public TextMeshProUGUI totalTeamNum;    //default is 3 can upgrade 
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillLevel;
    public TextMeshProUGUI skillMaxLevel;
    public TextMeshProUGUI skillDetail;
    //data
    public TextMeshProUGUI skillTypeText;
    public TextMeshProUGUI sCastType;
    public TextMeshProUGUI sSkillCharacter;
    public TextMeshProUGUI sCastTime;
    public TextMeshProUGUI sCoolDown;
    public TextMeshProUGUI sAmount;
    public Image skillIcon;
    

    [Header("Sounds")]
    public AudioClip learnClip;
    public AudioClip levelClip;



    [Header("Config Panel")]
    public GameObject configPanel;

    public Button learnBtn;
    public Button levelBtn;
    public Button equipBtn;
    public Button unEquipBtn;


    [Header("Notice Color")]
    public Color learnColor;
    public Color upgradeColor;
    public Color equipColor;
    public Color maxLevelColor;

    public UnityEngine.Events.UnityEvent onEnable;
    public UnityEngine.Events.UnityEvent onDisable;
    public UnityEngine.Events.UnityEvent onSelectCharacter;
    public UnityEngine.Events.UnityEvent onSelectSkill;
    public UnityEngine.Events.UnityEvent onEquip;
    public UnityEngine.Events.UnityEvent onUnequip;
    public UnityEngine.Events.UnityEvent onLearn;

    //Call by Invoke
    public UnityEngine.Events.UnityEvent OnLevelSkill, OnSkillAdd, OnSkillEquip, OnSkillUnEquip;



    private void Start()
    {
        instance = this;
         player = Players.localPlayer;
        //Clear All data
        ResetData();
       
        // load by index ,default 0 is kuno 
        InitCurrentSkill();
        //
    }

    /*
     *  public Button learnBtn;
    public Button levelBtn;
    public Button equipBtn;
    public Button unEquipBtn;
     */
    private void Update()
    {
        ////update state by index
        //if (panel.activeSelf && player != null&& selectSkillIndex!=-1)
        //{
        //    //update button by index
        //    Skill s = player.skills[selectSkillIndex];
        //    //learn btn
        //    if (s.data != null) {

        //        if(s.hasLearn ==true && s.level<s.maxLevel) {
        //            learnBtn.gameObject.SetActive(false);
                    
                    
        //        }else if(s.hasLearn==false) {
        //            learnBtn.gameObject.SetActive(true);
        //        }

        //        //levl
        //        levelBtn.interactable = spoints > 0 || ppoints > 0;


                    


        //    }


        //}

        spoints = player.skillPoints;
        ppoints = player.characterPoints;

        spText.text = spoints.ToString();
        ppText.text = ppoints.ToString();

        
        learnBtn.gameObject.SetActive(spoints > 0 || ppoints > 0 );
        levelBtn.interactable = spoints > 0 || ppoints > 0;


    }



    /// <summary>
    /// load skill by charactername und type because different for character und personal skill
    /// when boost the game needs load default skills by index without check ,when whole skill load done load by
    /// character db data to detail skill points for any skill who has learn.
    /// currentcharacter->skilcharacter==db.skill.name=> load skill&points => reload buffs
    /// </summary>
    public void InitCurrentSkill()
    {


        if (panel.activeSelf &  player != null)
        {
            //1.Load Character,when more than 1 character
            // //generate character  at slot can invoke the detail
            // for (int i = 0; i < characterList.Count; i++)
            // {
            //     //defaul is 0 before init in townmanager
            //     GameObject cObj = Instantiate(charactersPrefab, characterPos.position, Quaternion.identity) as GameObject;
            //     cObj.transform.SetParent(characterPos);
            //     //
            //     var c = characterList[i].GetComponent<Character>();
            //     cObj.GetComponent<PartyGuy>().currentCharacter = characterList[i].GetComponent<Character>();
            //     cObj.GetComponent<PartyGuy>().guySprite.sprite = c.data.icon;

            //     cObj.GetComponent<PartyGuy>().levelText.text = c.level.ToString();

            //     cObj.GetComponent<PartyGuy>().index = i;    //default load 0
            //     //
            //     cObj.GetComponent<Button>().onClick.AddListener(() =>
            //     {
            //         LoadCSkill(characterIndex);
            //     });


            //     //Network Listener
            //     NetworkServer.Spawn(cObj);
            // }
            // //2.Load SKill by character name
            //clear old obj
            for (int i = 0; i < battleSkillList.Count; i++)
            {
                if (battleSkillList[i] != null)
                {
                    Destroy(battleSkillList[i]);
                }
            }
            
                //query battleskill for current character
                var sdb = TownManager.instance.itemDatabase.skillsList;
                List<ScriptableSkill> csList = sdb.FindAll(s => s.skillType == SkillType.Battle);

                if(csList.Count>0){
                    for (int i = 0; i < csList.Count; i++)
                    {
                        //load current skill to pos
                        GameObject sObj = Instantiate(slotPrefab, bsPos.position, Quaternion.identity) as GameObject;
                        sObj.transform.SetParent(bsPos);
                        //Skill Prefab Data
                        var skillData = sObj.GetComponent<UISkillSlot>();
                        skillData.currentSkill = new Skill(csList[i]);
                        if (skillData.currentSkill.data != null)
                        {
                            //
                            skillData.image.sprite =Util.CreateSprite
                            (csList[i].spriteName);
                            skillData.skillType = csList[i].skillType;
                            skillData.levelText.text = skillData.currentSkill.level.ToString();
                            skillData.sIndex = i;
                            //exp=> value/maxvalue / (100 + n*10)*10% * l
                            skillData.sExpSlider.value = skillData.currentSkill.currentexp;

                            skillData.sExpSlider.maxValue = skillData.currentSkill.maxExp;


                        }
                        //

                        //callback
                        //when select change currentindex und detail info by index
                        //when currentslot skill can level und update button list by index
                        skillData.button.onClick.AddListener(() =>
                        {
                            UpdateSkill(csList[i]);
                        });
                        battleSkillList.Add(sObj);
                        NetworkServer.Spawn(sObj);


                    }
                }else{
                    Debug.Log("No Skill");
                }

                //personal skill
                //query battleskill for current character
                var pdb = TownManager.instance.itemDatabase.skillsList;
                List<ScriptableSkill> psList = sdb.FindAll(s => s.skillType == SkillType.Personal);

                if(psList.Count>0){
                    for (int i = 0; i < psList.Count; i++)
                    {
                        //load current skill to pos
                        GameObject sObj = Instantiate(slotPrefab, bsPos.position, Quaternion.identity) as GameObject;
                        sObj.transform.SetParent(psPos);
                        //Skill Prefab Data
                        var skillData = sObj.GetComponent<UISkillSlot>();
                        skillData.currentSkill = new Skill(psList[i]);
                        if (skillData.currentSkill.data != null)
                        {
                            //
                            skillData.image.sprite =Util.CreateSprite(psList[i].spriteName);
                            skillData.skillType = psList[i].skillType;
                            skillData.levelText.text = skillData.currentSkill.level.ToString();
                            skillData.sIndex = i;
                            //exp=> value/maxvalue / (100 + n*10)*10% * l
                            skillData.sExpSlider.value = skillData.currentSkill.currentexp;

                            skillData.sExpSlider.maxValue = skillData.currentSkill.maxExp;


                        }
                        //

                        //callback
                        //when select change currentindex und detail info by index
                        //when currentslot skill can level und update button list by index
                        skillData.button.onClick.AddListener(() =>
                        {
                            UpdateSkill(psList[i]);
                        });
                        personalSkillList.Add(sObj);
                        NetworkServer.Spawn(sObj);


                    }
                }else{
                    Debug.Log("No Skill");
                }

            //5. active buffs
            if(battleSkillList.Count>0)
            {
                //success load character skill ,reload buffs for player
                var passive = battleSkillList.FindAll(f => f.GetComponent<UISkillSlot>().currentSkill.data.sCastType == SkillCastType.Passive).ToList();
                if (passive.Count > 0)
                {
                    //reload buffs to player
                    for (int i = 0; i < passive.Count; i++)
                    {
                        if (passive[i].GetComponent<UISkillSlot>().currentSkill.hasLearn != false)
                        {
                            //Update data for Player
                            player.UpdateData(passive[i].GetComponent<UISkillSlot>().currentSkill);
                        }

                    }
                }
            }

            // refresh all
            //for (int i = 0; i < player.skills.Count; ++i)
            //{

            //    UISkillSlot slot = content.GetChild(i).GetComponent<UISkillSlot>();
            //    Skill skill = player.skills[i];

            //    bool isPassive = skill.data is PassiveSkill;

            //    // set state
            //    // slot.dragAndDropable.name = i.ToString();
            //    // slot.dragAndDropable.draggable = skill.level > 0 && !isPassive;

            //    // click event
            //    slot.button.interactable = skill.level > 0 &&
            //                               !isPassive &&
            //                               player.CheckSelf(skill); // checks mana, cooldown etc.

            //    //party guy? cand
            //    partyGuy.selectBtn.interactable = panel.activeSelf && TownManager.instance.atDungeon == false;

            //    //

            //    int icopy = i;

            //    slot.button.onClick.AddListener(() =>
            //    {
            //        // try use the skill or walk closer if needed
            //        // player.TryUseSkill(icopy);
            //        slot.currentSkill = skill;
            //        slot.sIndex = icopy; //when select show notice und detail infos
            //        currentIndex = icopy;
            //    });

            //    // image
            //    if (skill.level > 0)
            //    {
            //        slot.image.color = Color.white;
            //        slot.image.sprite = skill.image;
            //    }

            //    // description
            //    // slot.descriptionText.text = skill.ToolTip(showRequirements: skill.level == 0);

            //    // learn / upgrade
            //    if (skill.level < skill.maxLevel)
            //    {
            //        slot.upgradeButton.gameObject.SetActive(true);
            //        slot.upgradeButton.GetComponentInChildren<Text>().text = skill.level == 0 ? "Learn" : "Upgrade";
            //        slot.upgradeButton.onClick.AddListener(() => { player.CmdUpgradeSkill(icopy); });
            //    }
            //    else slot.upgradeButton.gameObject.SetActive(false);

            //    // cooldown overlay at dungeon skill ui
            //    //float cooldown = skill.CooldownRemaining();
            //    //slot.cooldownOverlay.SetActive(skill.level > 0 && cooldown > 0);
            //    //slot.cooldownText.text = cooldown.ToString("F0");
            //    //slot.cooldownCircle.fillAmount = skill.cooldown > 0 ? cooldown / skill.cooldown : 0;
            //}

            // skill experience
            //skillExperienceText.text = player.skillExp.ToString();
        }

        else
        {
            panel.SetActive(false);
        }
    }


    // public void LoadCSkill(int index)
    // {
    //     if (index == -1) return;
    //     //clear old battle Obj
    //     UpdateSkill(index);
    //     selectSkillIndex = index;   //

        
    //     Canvas.ForceUpdateCanvases();

    // }

    /// <summary>
    /// 1.show details
    /// 2.update glow when first level touch
    /// 3.if level==0->|1 
    /// </summary>
    /// <param name="index"></param>
    public void UpdateSkill(ScriptableSkill s)
    {
        ResetData();
        //load detail skill data by player syncdata
        if (player != null)
        {
            //load skill
            // Skill s = player.skills[index];
            if (s != null)
            {
                //1.show detail
                skillName.text = s.name.ToString();
                skillIcon.sprite= s.sSprite;
                skillLevel.text = s.level.ToString();
                skillMaxLevel.text = s.maxLevel.ToString();
                skillDetail.text =s.sDetail.ToString();
                skillTypeText.text = s.skillType.ToString();
                sCastType.text = s.sCastType.ToString();
                sCoolDown.text = s.cooldown.ToString();
            
                sSkillCharacter.text = s.SkillCharacter.ToString();
                sAmount.text = s.sAmount.ToString();
                // skillIcon.sprite = s.data.sSprite;
            }
        }
        


        //
        onSelectSkill.Invoke(); 
    }

    #region Config

    //got skill data by rtskill.hash 
    //when selct the btn needs use command in players

    public void LearnSkill(int index)
    {
        player.CmdLearnSkill(index);
        onLearn.Invoke();


    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    public void UpgradeSkill(int index)
    {
        player.CmdUpgradeSkill(index);
        OnLevelSkill.Invoke();

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    public void EquipSkill(int index)
    {

        player.CmdEquipSKill(index);
        onEquip.Invoke();

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    public void UnEquipSkill(int index)
    {


        player.CmdUnEquipSkill(index);
        onUnequip.Invoke();

    }

    #endregion

    /*
     *  [Header("Select Detail")]
    public TextMeshProUGUI spText;
    public TextMeshProUGUI ppText;
    //
    public TextMeshProUGUI currentTeamNum;
    public TextMeshProUGUI totalTeamNum;    //default is 3 can upgrade 
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillLevel;
    public TextMeshProUGUI skillMaxLevel;
    public TextMeshProUGUI skillDetail;
    //data
    public TextMeshProUGUI skillTypeText;
    public TextMeshProUGUI sCastType;
    public TextMeshProUGUI sSkillCharacter;
    public TextMeshProUGUI sCastTime;
    public TextMeshProUGUI sCoolDown;
    public TextMeshProUGUI sAmount;
    public Image skillIcon;
     */
    public void ResetData()
    {
        skillName.text = "";
        skillLevel.text = "";
        //skillMaxLevel.text = "";
        skillDetail.text = "";
        skillTypeText.text = "";
        sCastType.text = "";
        sCoolDown.text = "";
        sSkillCharacter.text = "";
        sAmount.text = "";
        skillIcon.sprite = null;

    }
}




