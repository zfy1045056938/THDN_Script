using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;
using TMPro;


/// <summary>
/// 玩家行为控制
/// 1.显示玩家信息
/// 2.对物体造成伤害(英雄技能)
/// 
/// 3.回合结算(final round )
/// 4.英雄类型(浪人,智者,游侠)
/// 浪人skil: hide,
/// magic:elemets remix (w,r,g,w,f)
/// hunter: (bear,eagles,wolves)
/// 
/// </summary>
public class PlayerPortraitVisual:MonoBehaviour
{
    public CharacterAsset characterAsset;
    public WeaponSkill weapon;
    public RingSkill ring;
    [Header("Component Reference")]
    public Text healthText;
    public Text jobText;
    public Text detailText;

    [Header("PlayerEquipmentData")]
    public Text atkText;
    public Text atkDurText;
    public Text defText;
    public Text defDurText;
    public Text ringDurText;

    //
    public TextMeshProUGUI StrText;
    public TextMeshProUGUI DexText;
    public TextMeshProUGUI InteText;
    public TextMeshProUGUI FlashPercText;
    public TextMeshProUGUI ExtraSDText;
   
    public GameObject coinPanel;
    public Text coinText;
    public Text nameText;
    
    private PlayerData playerData;

    [Header("HeroInfoInBattleGame")]
//    public Image heroPowerIconImage;
//    public Image heroPowerBackgroundImage;
    public Image portraitImage;
    public Image portraitBackgroundImage;

    public Sprite HurtDefSprite;
    public Image ArmorSprite;
    public GameObject WeaponImage;
    public GameObject ArmorImage;
    public GameObject RingImage;
    public GameObject STRObj;
    public GameObject DEXObj;
    public GameObject INTEObj;

    public List<GameObject> artifactList;
    public Transform artifactPos;

    public bool soulView = false;
            ///
         public  static MainAtkType atkType = MainAtkType.NONE;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    private void Awake()
    {
       
      playerData=FindObjectOfType<PlayerData>();
   

    }
    
    

    public void Start(){
        //  if (characterAsset != null && playerData !=null)
        // {
        //     ApplyLookFromAsset();
          
        // }
    }

    

//
    public void ApplyLookFromAsset(){

//        if (heroPowerIconImage!=null)
//        {
//            heroPowerIconImage.sprite = characterAsset.heroPowerBGImage;
//            heroPowerBackgroundImage.sprite = characterAsset.heroPowerBGImage;
//            heroPowerIconImage.color = characterAsset.heroPowerBGTint;
//        }

        if (BattleStartInfo.player != null)
        {
//            characterAsset = BattleStartInfo.SelectDeck.characterAsset;
            portraitImage.sprite = characterAsset.avatarImage;
            portraitBackgroundImage.sprite=characterAsset.avatarBGImage;
            // portraitBackgroundImage.color = characterAsset.avatarBGTint;
            
            healthText.text = characterAsset.maxHealth.ToString();
            Debug.Log("Got Sprite to AVA");
            
           
        }


        LocalizationJob();
        
        
    }
    void LocalizationJob(){
        switch(characterAsset.jobs){
            case PlayerJob.Hunter:
            jobText.text="猎人";
            break;
            case PlayerJob.Magic:
            jobText.text="研习者";
            break;
            case PlayerJob.Survicer:
            jobText.text="生存者";
            break;
        }
    }

    //Load Player Data From Main Menu 
    // public void ReadPlayerData(PlayerData  p){
        
    //     //
    //     // for(int i=0;i<equipmentSlots.Count;i++){
    //     //    p.equipSlot = new List<EquipmentSlot>();
    //     //    if(p.equipSlot[i].itemIcon!=null){
    //     //        equipmentSlots[i].transform.Find("ItemIcon").gameObject.SetActive(true);
    //     //        //Get Icon
    //     //        equipmentSlots[i].itemIcon = p.equipSlot[i].itemIcon;
    //     //        //
    //     //    }
    //     // }
    //     atkText.text = p.atk.ToString();
    //     atkDurText.text=p.atkCount.ToString();
       

    //     defText.text =p.ArmorDef.ToString();
    //     defDurText.text=p.ArmorDur.ToString();
    // }
    /// <summary>
    /// 对物体造成伤害,分为
    /// 1.single：单体伤害,指向单个物体进行攻击行为
    /// 2.group:
    /// 3.hide:
    /// </summary>
    /// <param name="amount">Amount.</param>
    /// <param name="healthAfter">Health after.</param>
    public void TakeDamage(int amount ,int healthAfter,int armorAfter){
     
      
        if (amount>0)
        {
            DamageEffect.CreateDamageEffect(transform.position,amount);
            //
            healthText.text = healthAfter.ToString();
            defText.text = armorAfter.ToString();
            Debug.Log(healthText.text+"NOW HEALTH");
            //armor
            
        }

//        if (TurnManager.instance.WhoseTurn.otherPlayer.CreatureDef < 0)
//        {
//            ArmorImage.gameObject.SetActive(false);
//        }
//        else
//        {
//            ArmorImage.gameObject.SetActive(true);
//        }
    }

    /// <summary>
    /// 玩家死亡后爆炸效效果 
    /// </summary>
    public void Explose(){
        Instantiate(GlobalSetting.instance.ExplosionPrefab, transform.position, Quaternion.identity) ;
        //
        
        


//           SceneReloader.instance.ReturnFromDungeon("MainMenu");
            // s.PrependInterval(5.0f);
            new DelayCommand(6.0f).AddToQueue();
             SceneReloader.instance.ReturnTown("MainBattleScene");
        BattleStartInfo.IsWinner = false;
        DungeonExplore.instance.canLeave = true;
        DungeonExplore.instance.LeaveDungeon();
        Debug.Log("Show");
       


    }

    /// <summary>
    /// 
    /// </summary>
    public void LoadStatsFromAsset()
    {
        Debug.Log("Load Player stats");
       UpdateV();

    }

    public void AddArtifact(ArtifactType type, int amount)
    {

        switch (type)
        {
           case  ArtifactType.Atk:
               CreateArtifact(type, amount);
//               TurnManager.instance.WhoseTurn.MaxHealth += amount;
//               
               break;
           case ArtifactType.GiveCard:
               CreateArtifact(type,amount);
//               TurnManager.instance.WhoseTurn.DrawACard(false);
               break;
            case ArtifactType.Posion:
                CreateArtifact(type,amount);
                break;
            case ArtifactType.Hp:
                CreateArtifact(type,amount);
                break;
                case ArtifactType.Armor:
                CreateArtifact(type,amount);
                break;
                case ArtifactType.Token:
                CreateArtifact(type,amount);
                break;
                    
             default:
                 break;
        }
        
        
    }

    public void CreateArtifact(ArtifactType type, int amount)
    {
        GameObject obj = Instantiate(GlobalSetting.instance.artifactIcon,artifactPos.position,Quaternion.identity)as GameObject;
        obj.transform.SetParent(artifactPos);
        obj.GetComponent<ArtifactIcon>().type = type;
         obj.GetComponent<ArtifactIcon>().titleText.text = type.ToString();
         
        obj.GetComponent<ArtifactIcon>().icon.sprite = FindObjectOfType<ArtifactIcons>().GetComponent<ArtifactIcons>().artDi[type];
        obj.transform.localScale=Vector3.one;
        
        obj.GetComponent<ArtifactIcon>().amount = amount;
        
        artifactList.Add(obj);
    }
    
    /// <summary>
    /// Update stats by artifact type with amount 
    /// </summary>
    /// <param name="t"></param>
    /// <param name="amount"></param>
    public void UpdateStates(ArtifactType t, int amount)
    {
        switch (t)
        {
            case ArtifactType.Hp:
                Debug.Log("Artifact Add HP");
                TurnManager.instance.WhoseTurn.MaxHealth += amount;
                break;
            case ArtifactType.Atk:
                Debug.Log("Artifact Add Atk");
                TurnManager.instance.WhoseTurn.CreatureAtk += amount;
                break;
            case ArtifactType.GiveCard:
                TurnManager.instance.WhoseTurn.DrawACard(false);
                break;
            case ArtifactType.Token: 


                break;
            case ArtifactType.Armor:
                TurnManager.instance.WhoseTurn.CreatureDef += amount;
                break;
            case ArtifactType.Mp:
            break;
            case ArtifactType.Posion:
                //Random Add Buff to rnd  opp creature
                Debug.Log("POSION ARTIFACR EFFECT===========>Active");
                CreatureLogic [] cl = TurnManager.instance.WhoseTurn.otherPlayer.table.creatureOnTable.ToArray();
                
                int rnd =Random.Range(0,cl.Length);

                new DealBuffCommand(cl[rnd].ID,amount,DamageElementalType.Posion,0);
            
            break;
                
        }
    }

    public void BUpdateV()
    {
        atkText.text = TurnManager.instance.WhoseTurn.CreatureAtk.ToString();
        defText.text = TurnManager.instance.WhoseTurn.CreatureDef.ToString();
        // atkDurText.text = TurnManager.instance.WhoseTurn.atkDur.ToString();

        // if (TurnManager.instance.WhoseTurn.atkDur > 0)
        // {
            
        //     WeaponImage.gameObject.SetActive(true);
        // }
        // else
        // {
        //     WeaponImage.gameObject.SetActive(false);
        // }

        if (TurnManager.instance.WhoseTurn.CreatureDef > 0)
        {
            ArmorImage.gameObject.SetActive(true);
        }
        else
        {
            ArmorImage.gameObject.SetActive(false);
        }

        if (TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.weapon.useEffectScriptName == "")
        {
            TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.weapon.useEffectScriptName =
                "SpellWeaponDamage";
            
               weapon.payMana=  TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.weapon.payMana ;
                TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.weapon.manaText.text = TurnManager
                    .instance.WhoseTurn.playerArea.playerPortraitVisual.weapon.payMana.ToString();
            
           //Load Spell for player can atk target
        }else if (TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.weapon.useEffectScriptName != "" &&
                TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.weapon.useEffectScriptName != null)
            {
                TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.weapon.spellEffect =
                    System.Activator.CreateInstance(
                        System.Type.GetType(TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.weapon.useEffectScriptName)) as SpellEffect;
                TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.weapon.spellEffect.owner = GlobalSetting.instance.lowPlayer;
                TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.weapon.spellEffect.target = weapon.weapon.spellTarget;
            }
    }

    public void UpdateV()
    {
        if (BattleStartInfo.player != null)
        {
            //Load weapon
            //作为法术牌的武器牌对玩家伤害的计算为自身属性加成(3)+武器基本伤害+词缀+特殊效果
            // if (BattleStartInfo.Weapon != null)
            // {
            //     weapon.payMana = weapon.weapon.itemMana;
            //     if (weapon.weapon.useEffectScriptName != "")
            //     {
            //         weapon.useEffectScriptName = weapon.weapon.useEffectScriptName.ToString();
            //     }

            //     atkText.text = BattleStartInfo.player.atk.ToString();
            //     atkDurText.text = weapon.weapon.weaponDur.ToString();
            //     if ( playerData.atkCount > 0)
            //     {
            //         WeaponImage.gameObject.SetActive(true);
            //     }
            //     else
            //     {
            //         WeaponImage.gameObject.SetActive(false);
            //     }

              
            // }
            // else
            // {
            //    weapon.payMana=1; // default for wepaon by spell or else..
            //     WeaponImage.gameObject.SetActive(false);
            // }


            //load armor
            defText.text = playerData.ArmorDef.ToString();
            // if (playerData.ArmorDef > 0)
            // {
            //     ArmorImage.gameObject.SetActive(true);
            // }
            // else
            // {
            //     ArmorImage.gameObject.SetActive(false);
            // }

            //
            // if(BattleStartInfo.player.Strength>0){
            // STRObj.gameObject.SetActive(true);
            // StrText.text= BattleStartInfo.player.Strength.ToString();
            // }else{
            //     STRObj.gameObject.SetActive(false);
            // }
            // if(BattleStartInfo.player.Strength>0){
            // DEXObj.gameObject.SetActive(true);
            // DexText.text = BattleStartInfo.player.Dex.ToString();
            // FlashPercText.text=BattleStartInfo.player.extraFlash*100+"%";
            // }else{
            //     DEXObj.gameObject.SetActive(false);
            // }

            // if(BattleStartInfo.player.Magic>0){
            // INTEObj.gameObject.SetActive(true);
            // InteText.text=BattleStartInfo.player.Magic.ToString();
            // ExtraSDText.text = BattleStartInfo.player.extraSpellDamage.ToString();
            // // ExtraSDPercText.text=BattleStartInfo.player.ESDPerc*100+"%";
            // }else{
            //     INTEObj.gameObject.SetActive(false);
            // }
            // if(TurnManager.instance.WhoseTurn.hurtDef>0){
            //     ArmorSprite.sprite=HurtDefSprite;
            // }
        }
    }

    public void UpdateArtifact(ArtifactType type, int amount)
    {
        Debug.Log("Update Artifact Amount");
        new DelayCommand(0.4f).AddToQueue();
        switch (type)
        {
            case ArtifactType.GiveCard:
                while (amount > 0)
                {

                  TurnManager.instance.WhoseTurn.DrawACard(false);
                }

                break;
            case ArtifactType.Hp:
                TurnManager.instance.WhoseTurn.MaxHealth += amount;
                break;
            case ArtifactType.Atk:
                break;
            default:
                break;
        }
    }

    public void LoadNames(){
        nameText.text = PlayerData.localPlayer.name.ToString();
    }
}