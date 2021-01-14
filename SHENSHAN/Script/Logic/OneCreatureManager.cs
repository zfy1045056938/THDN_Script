using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using System;


/// <summary>
/// 控制场面随从行为,从资产库中获取资源
/// </summary>
public class OneCreatureManager:MonoBehaviour
{
    public CardAsset cardAsset;
    public OneCardManager previewManager;
    public GameObject soulViewObj;
    
    [Header("CreatureInfo")]
    public Text healthText;
    public Text defText;
    public Text atkText;
 

    [Header("Image Referenece")]
    public Image creatureGraphicImage;
    public Image creatureGlowImage;
    public Image defImage;    //If Armor is 0 then hide them
    public Sprite hurtArmor;
    public bool taunt = false;
    public Image TauntImage;
    private float glowPulse = 1f;
    private DamageElementsIcons types;
    
    //
    public Text FRT;
    public Text IRT;
    public Text PORT;
    public Text ELRT;

    public List<GameObject> buffList;
    public List<GameObject> viewBuffTabs;
    public Transform vbtPos;
    public Transform buffPos;
    /// <summary>
    /// Awake this instance.
    /// </summary>
    private void Awake()
    {
        if (cardAsset!=null)
        {
            ReadCreatureFromAsset();
        }
        //creatureGraphicImage.DOFade(0.4f, glowPulse).SetLoops(-1, LoopType.Yoyo);


    }

    void Start()
    {
        types = FindObjectOfType<DamageElementsIcons>();
    }

    
    /// <summary>
    /// Reads the creature from asset.
    /// </summary>
    public void ReadCreatureFromAsset()
    {
        //获取卡牌资产信息
        creatureGraphicImage.sprite = cardAsset.cardSprite;
        healthText.text = cardAsset.cardHealth.ToString();
        // FRT.text = cardAsset.fireResistance.ToString();
        // IRT.text = cardAsset.iceResistance.ToString();
        // PORT.text = cardAsset.poisonResistance.ToString();
        // ELRT.text = cardAsset.electronicResistance.ToString();
        
        
        if (cardAsset.cardDef >0)
        {
            defImage.gameObject.SetActive(true);
        }
        else
        {
            defImage.gameObject.SetActive(false);
        }
        defText.text = cardAsset.cardDef.ToString();
        atkText.text = cardAsset.cardAtk.ToString();
        
        //
     
        //
        if (previewManager!=null)
        {
            previewManager.cardAsset = cardAsset;
            previewManager.ReadCardFromAsset();
        }
        //
        // if (TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.soulView == true)
        // {
        //     soulViewObj.gameObject.SetActive(true);
        // }
        // else
        // {
        //     soulViewObj.gameObject.SetActive(false);
        // }

        taunt = cardAsset.taunt;
    }



    /// <summary>
    /// 是否可以攻击
    /// </summary>
    private bool canAtkNow = false;
    public bool CanAtkNow{
        get { return canAtkNow; }
        set{
            canAtkNow = value;
            //
            creatureGlowImage.enabled = value;
        }
    }


    /// <summary>
    /// 对目标造成伤害
    /// </summary>
    /// <param name="amount">Amount.</param>
    /// <param name="healthAfter">Health after.</param>
    public void TakeDamage(int amount,int healthAfter,int armorAfter){
        if (amount>0)
        {
            //创建伤害效果，对目标
            DamageEffect.CreateDamageEffect(transform.position, amount);
            healthText.text = healthAfter.ToString();
            defText.text= armorAfter.ToString();

        }
    }
    
    
    /// <summary>
    /// BUff Command UI LAYER notice buff with ICON,
    ///All effect inflence to player with Application
    /// </summary>

    //Update By OSX _1019 , Change All spelltype to player influence to table elements .
    public void AddBuffWithCreature(CreatureLogic cl, int amount,SpellBuffType types,int round)
    {
        cl.HasBuff = true;
        switch (types)
        {
            case SpellBuffType.None:
                break;
            case  SpellBuffType.Armor:
            Debug.Log("ARMOR EFFECT");
             SoundManager.instance.PlaySound(GlobalSetting.instance.armorClip);
                SoundManager.instance.PlaySound(GlobalSetting.instance.weaponClip);
                cl.CreatureDef += amount;
                defText.text = cl.CreatureDef.ToString();
                
                break;
            case SpellBuffType.CharacterArmor:
            Debug.Log("CharacterArmor Effect Active");
            SoundManager.instance.PlaySound(GlobalSetting.instance.armorClip);
                TurnManager.instance.WhoseTurn.CreatureDef += amount;
                TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.defText.text =
                    TurnManager.instance.WhoseTurn.CreatureDef.ToString();
                TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.ArmorImage.gameObject.SetActive(true);
                break;
            case SpellBuffType.FireArmor:
TurnManager.instance.WhoseTurn.CreatureDef += amount;
                TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.defText.text =
                    TurnManager.instance.WhoseTurn.CreatureDef.ToString();
                TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.ArmorImage.gameObject.SetActive(true);
                TurnManager.instance.WhoseTurn.FR += amount;
                break;
            
            case SpellBuffType.Health:
                cl.MaxHealth += amount;
                break;
            
            case SpellBuffType.Atk:
            Debug.Log("BUFF=====>ATK");
                cl.CreatureAtk += amount;
                atkText.text = cl.CreatureAtk.ToString();
                atkText.color = Color.green;
                Debug.Log("Atk ist"+cl.CreatureAtk);
                break;
            case SpellBuffType.DoubleAtk :
                cl.AttacksForThisTurn = 2;
                break;
            case SpellBuffType.GroupAtk:
                Debug.Log("GRoup atk");
                var cs = TurnManager.instance.WhoseTurn.table.creatureOnTable.ToArray();
                foreach (var c in cs)
                {
                    c.CreatureAtk += amount;
                }
                break;
            
            case SpellBuffType.AtkDur:
                SoundManager.instance.PlaySound(GlobalSetting.instance.weaponClip);
                TurnManager.instance.WhoseTurn.atkDur += amount;
                TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.atkDurText.text =TurnManager.instance.WhoseTurn.atkDur.ToString();
                break;
            case  SpellBuffType.DEX:
                Debug.Log("SpellBuff ----> Dex");
                TurnManager.instance.WhoseTurn.DEX += amount;
                TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.InteText.text =  TurnManager.instance.WhoseTurn.DEX.ToString();
                if ( TurnManager.instance.WhoseTurn.DEX % 3 == 0)
                {
                     TurnManager.instance.WhoseTurn.DEX += 1;
                }
                break;
            case SpellBuffType.INT:
                //cl change to player
            Debug.Log("SpellBuff ----> INTE");
                TurnManager.instance.WhoseTurn.INTE += amount;
                TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.InteText.text =  TurnManager.instance.WhoseTurn.INTE.ToString();
                if ( TurnManager.instance.WhoseTurn.INTE % 3 == 0)
                {
                     TurnManager.instance.WhoseTurn.INTE += 1;
                }
                break;
                  case SpellBuffType.ExtraSpell:
                  Debug.Log("Extra Spell");
               TurnManager.instance.WhoseTurn.ExtraSpellDamage +=amount;
               TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.ExtraSDText.text=TurnManager.instance.WhoseTurn.ExtraSpellDamage.ToString();
                break;
            case  SpellBuffType.STR:
               Debug.Log("SpellBuff ----> STR");
                TurnManager.instance.WhoseTurn.STR += amount;
                TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.StrText.text =  TurnManager.instance.WhoseTurn.STR.ToString();
                if ( TurnManager.instance.WhoseTurn.STR % 3 == 0)
                {
                     TurnManager.instance.WhoseTurn.STR += 1;
                }
                break;
//            case SpellBuffType.Bloody:
//                //Add Icon with 3 round
//                cl.MaxHealth -= 1;
//                DamageEffect.CreateDamageEffect(transform.position,amount);
//                //
//                CreateBuffIcon(SpellBuffType.Bloody, round);
//                cl.CreatureAtk += amount;
//                atkText.text = cl.CreatureAtk.ToString();
//                healthText.text = cl.MaxHealth.ToString();
//                healthText.color = Color.red;
//                atkText.color = Color.green;
//                break;
            case SpellBuffType.Taunt:
                SoundManager.instance.PlaySound(GlobalSetting.instance.armorClip);
                cl.Taunt=true;
                TauntImage.gameObject.SetActive(true);
                break;
            case SpellBuffType.HurtTaunt:
            //荆棘状态,受到伤害对攻击方造成荆棘伤害
            Debug.Log("HUrt Taunt ================>Active,amount ist"+amount);
               TurnManager.instance.WhoseTurn.CreatureDef += amount;
               TurnManager.instance.WhoseTurn.hasHurtDef=true;
               TurnManager.instance.WhoseTurn.hurtDef=1;
               TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.defText.text= TurnManager.instance.WhoseTurn.CreatureDef.ToString();
               TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.ArmorSprite.sprite =  TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.HurtDefSprite;
               
                break;
            // case SpellBuffType.CharacterArmor:
            //     cl.HasHurtTaunt=true;
            //     cl.CreatureDef += amount;
            //    defImage.sprite = hurtArmor;
            //     defImage.gameObject.SetActive(false);
            //     cl.hurtDef+=1;
            //     defText.text = cl.CreatureDef.ToString();
            // break;


            case SpellBuffType.TableAmount:
                Debug.Log("Table amount");
                int count = 0;
                var tamount = TurnManager.instance.WhoseTurn.table.creatureOnTable.Count;
                count = tamount;
                cl.CreatureAtk += count;
                break;
            //MACHINE
            // effect for self when end over , some can effect for player 
            ////////
            case SpellBuffType.Machine:
            Debug.Log("MACHINE STATE");

                cl.Machine=true;
            Debug.Log(cl.Machine+"load");
                break;
            
            case SpellBuffType.MachineArmor:
                SoundManager.instance.PlaySound(GlobalSetting.instance.machineClip);
                    cl.CreatureDef += amount;
                    defText.text = cl.CreatureDef.ToString();
                    defImage.gameObject.SetActive(true);
                
                break;
            case SpellBuffType.MachineAtk:
                SoundManager.instance.PlaySound(GlobalSetting.instance.machineClip);
                    cl.CreatureAtk += amount;
                    atkText.text = cl.CreatureAtk.ToString();
                
            break;
             case SpellBuffType.MachineHeal:
              SoundManager.instance.PlaySound(GlobalSetting.instance.machineClip);
               cl.MaxHealth+=amount;
            //    new DealDamageCommand().AddToQueue();
            break;
            case SpellBuffType.MachineHyper:
                 SoundManager.instance.PlaySound(GlobalSetting.instance.machineClip);
                    cl.CreatureAtk += amount;
                    cl.CreatureDef += amount;
                    cl.MaxHealth  += amount;

                    atkText.text = cl.CreatureAtk.ToString();
                    defText.text = cl.CreatureDef.ToString();
                    healthText.text = cl.MaxHealth.ToString();
                    
                
            break;

            case SpellBuffType.Haste:
                cl.AttacksForThisTurn=1;
            break;

            case SpellBuffType.Flash:
                Debug.Log("Flash Buff Active");
                //when active effect influce to every creature at your bf until round ends
                TurnManager.instance.WhoseTurn.hasFlash=true;
                TurnManager.instance.WhoseTurn.flashPerc=0.5f; 
                
            break;
            
        }

        atkText.text = cl.CreatureAtk.ToString();
        healthText.text = cl.MaxHealth.ToString();
        defText.text = cl.CreatureDef.ToString();
    }

    //Add creature extra bouns for player
    public void AddBouns(){

        Debug.Log("Add Bouns For DB");
      CreatureLogic cl = new CreatureLogic(TurnManager.instance.WhoseTurn,cardAsset);

      if(TurnManager.instance.WhoseTurn==GlobalSetting.instance.lowPlayer){
          //Bouns by select de
      if(DungeonExplore.DCATK>0){
          atkText.color=Color.green;
      }
      cl.CreatureAtk += DungeonExplore.DCATK;
      atkText.text= cl.CreatureAtk.ToString();
      //
      if(DungeonExplore.DCHeal>0){
          atkText.color=Color.green;
      }
      cl.MaxHealth += DungeonExplore.DCHeal;
      healthText.text= cl.MaxHealth.ToString();
      
      //
      if(DungeonExplore.DCARMOR>0){
          atkText.color=Color.green;
      }
      cl.CreatureDef += DungeonExplore.DCARMOR;
      defText.text= cl.CreatureDef.ToString();}
      else if(TurnManager.instance.WhoseTurn==GlobalSetting.instance.topPlayer)
      {
          //bouns by bouns
            cl.MaxHealth += DungeonExplore.DUNGEONENEMYBOUNS;
            cl.CreatureDef += DungeonExplore.DUNGEONENEMYBOUNS;
            defImage.gameObject.SetActive(true);
            healthText.text=cl.MaxHealth.ToString();
            defText.text = cl.CreatureDef.ToString();

            healthText.color = Color.green;
            defText.color=Color.green;
            
      }
      

    }
    /// <summary>
    /// BUff Command
    /// </summary>
    public void AddBuffWithCreature(CreatureLogic cl, int amount,DamageElementalType type)
    {
        switch (type)
        {
           case DamageElementalType.Posion:
               
               break;
            
        }
    }
    public void CreateVBT(DamageElementalType type,int r,int a,bool first){
        GameObject o = Instantiate(GlobalSetting.instance.viewEffectPrefab,vbtPos.position,Quaternion.identity)as GameObject;
        o.transform.SetParent(vbtPos);
        o.GetComponent<ViewEffectTabs>().typeText.text= type.ToString();
        switch(type){
            case DamageElementalType.Electronic:
        o.GetComponent<ViewEffectTabs>().detailText.text=string.Format("麻痹状态,{0}回合内失去{1}点攻击",r,a);
        break;
         case DamageElementalType.Posion:
        o.GetComponent<ViewEffectTabs>().detailText.text=string.Format("毒液伤害,总共造成{0}点伤害",GetNum(a));


        break;
         case DamageElementalType.Fire:
        o.GetComponent<ViewEffectTabs>().detailText.text=string.Format("燃烧伤害,{0}回合内每回合受到{1}点伤害",r,a);
        break;
         case DamageElementalType.Bloody:
        o.GetComponent<ViewEffectTabs>().detailText.text=string.Format("流血状态,{0}回合里每回合受到{1}点伤害",r,a);
        
        break;
        }
        //
        viewBuffTabs.Add(o);
    }

    public int GetNum(int a){
        int  count= 0;
        for(int i=a;i<0;i-- ){
            count += i ; 
        }
        

        return count;
    }
    
    public void CreateBuffIcon(DamageElementalType type, int round,int amount)
    {
        GameObject obj  = Instantiate(GlobalSetting.instance.buffIcon,buffPos.position,Quaternion.identity)as GameObject;
        obj.transform.parent = buffPos;
        obj.transform.localScale=new Vector3(1,1,1);

  obj.GetComponent<BuffIcon>().round = round;
        obj.GetComponent<BuffIcon>().roundTime.text = round.ToString();
          obj.GetComponent<BuffIcon>().amountText.text = amount.ToString();
        obj.GetComponent<BuffIcon>().det = type;
         obj.GetComponent<BuffIcon>().amount = amount;

        if(round>0&&amount>0){
            obj.GetComponent<BuffIcon>().roundTime.text=round.ToString();
            obj.GetComponent<BuffIcon>().amountText.text = amount.ToString();
        }else if(amount>0&&round<=0){
             obj.GetComponent<BuffIcon>().roundTime.text=amount.ToString();
        }
    
         
         obj.GetComponent<BuffIcon>().tText.text = type.ToString();
         
         obj.GetComponent<BuffIcon>().dText.text =string.Format("{0}回合内造成{1}伤害",round.ToString(),amount.ToString());
         
        obj.GetComponent<BuffIcon>().buffSprite.sprite = types.detDic[type];
        
        buffList.Add(obj);
    }
    
    /// <summary>
    /// 与buff不同的是获得伤害性持续效果,在每回合开始受到伤害,buff效果为基础属性增益,damageeffect为伤害性属性增益,
    /// </summary>
    /// <param name="cl"></param>
    /// <param name="amount"></param>
    /// <param name="roundTime"></param>
    /// <param name="healAfter"></param>
    /// <param name="type"></param>
    public void ElementalBuff(CreatureLogic cl,int amount, int roundTime, DamageElementalType type)
    {
        bool hasEffect=false;

        //Check has Elemental
        for(int i=0;i<buffList.Count;i++){
            if(buffList[i].GetComponent<BuffIcon>().det==type){
                //has effect just add round time
                Debug.Log("Has Effect just add round");
                UpdateRound(type,roundTime,amount);
                hasEffect=true;
            }
        }
        

        if(hasEffect==false){
         cl.HasAura = true;
        //if has extra spell  b.v+esp
        int esp = 0;        
            switch (type)
            {
                case DamageElementalType.Posion:
                  SoundManager.instance.PlaySound(GlobalSetting.instance.posionClip);
                    //OnTurnStart amoun-1>0
                    Debug.Log("Posion Effect Cause");
                    if(TurnManager.instance.WhoseTurn.hasESD==true){
                        amount += TurnManager.instance.WhoseTurn.ExtraSpellDamage;
                    }
                    if(cl.HasPosion==false){
                    cl.HasPosion = true;
                    cl.PosPoint = amount;
                    healthText.text = cl.MaxHealth.ToString();
                    //Buff Icon
//                    GameObject obj  = Instantiate(GlobalSetting.instance.buffIcon,buffPos.position,Quaternion.identity)as GameObject;
//                    obj.transform.parent = buffPos;
//                    obj.transform.localScale=new Vector3(1,1,1);
//                    obj.GetComponent<BuffIcon>().roundTime.text = roundTime.ToString();
//                    obj.GetComponent<BuffIcon>().round = roundTime;
//                    obj.GetComponent<BuffIcon>().det = type;
//                    obj.GetComponent<BuffIcon>().buffSprite = types.detDic[type];
                    //save to list when turn start check the time and damage target
//                    buffList.Add(obj);
                    CreateBuffIcon(type,roundTime,amount);
                    CreateVBT(type,roundTime,amount,true);
                    //
                    GlobalSetting.instance.SETLogs(string.Format("毒液伤害{0}回合内造成{1}点伤害",roundTime,amount));
                    }else{
                        UpdateRound(type,roundTime,amount);
                    }
                    break;
                case DamageElementalType.Bloody:
                    Debug.Log("DE Active ===> Bloody" );
                    SoundManager.instance.PlaySound(GlobalSetting.instance.bloodyClip);
                    //每回合失去1点血,攻击增加1
                    GetDamageByDET(cl,type,amount);
                    healthText.text = cl.MaxHealth.ToString();
                    atkText.text=cl.CreatureAtk.ToString();
                    healthText.color=Color.red;
                    CreateBuffIcon(type,roundTime,amount);
                    //Glogs
                   GlobalSetting.instance.SETLogs(string.Format("3回合内每回合造成{0}点伤害",amount));
                    break;
                case DamageElementalType.Electronic:
                  SoundManager.instance.PlaySound(GlobalSetting.instance.elecClip);
                  
                     if(TurnManager.instance.WhoseTurn.hasESD==true){
                        amount += TurnManager.instance.WhoseTurn.ExtraSpellDamage;
                    }
                  
                     GlobalSetting.instance.SETLogs(string.Format("麻痹状态,伤害降至为0,持续两回合"));
                    if(cl.HasElec==false){

                    cl.HasElec = true;
                    int eb = 0;
                   cl.CreatureAtk =eb;
                   atkText.text = cl.CreatureAtk.ToString();
                    CreateBuffIcon(type,roundTime,amount);
                    }else{
                        //Add Round
                        UpdateRound(type,roundTime);
                    }
                    
                    break;
                case DamageElementalType.Fire:
                    SoundManager.instance.PlaySound(GlobalSetting.instance.burningClip);
                     if(TurnManager.instance.WhoseTurn.hasESD==true){
                        amount += TurnManager.instance.WhoseTurn.ExtraSpellDamage;
                    }
                    //different of state ,持续性伤害
                    if (cl.HasFire==false)
                    {
                        cl.HasFire = true;
                        cl.MaxHealth -= amount;

                        CreateBuffIcon(type, roundTime, amount);
                        GlobalSetting.instance.SETLogs(string.Format("燃烧效果触发,每回合造成{0}点伤害,持续3回合",amount));
                    }
                    else
                    {
                        UpdateRound(type, roundTime,amount);
                    }

                    break;
                    //TODO
                    // case DamageElementalType.Burning:
                    // SoundManager.instance.PlaySound(GlobalSetting.instance.burningClip);
                    //  if(TurnManager.instance.WhoseTurn.hasESD==true){
                    //     amount += TurnManager.instance.WhoseTurn.ExtraSpellDamage;
                    // }
                    //different of state ,持续性伤害
                    // if (cl.HasFire==false)
                    // {
                    //     cl.HasFire = true;
                    //     cl.MaxHealth -= amount;

                    //     CreateBuffIcon(type, roundTime, amount);
                    // }
                    // else
                    // {
                    //     UpdateRound(type, roundTime,amount);
                    // }

                    // break;
                case DamageElementalType.Damage:
                    Debug.Log("Spell Damage to target");
                    int extraBouns =0;

                    if(TurnManager.instance.WhoseTurn.hasESD){
                        Debug.Log("Has EB und EB ist"+extraBouns.ToString());
                        extraBouns = TurnManager.instance.WhoseTurn.ExtraSpellDamage;
                        cl.MaxHealth -= (amount+extraBouns);

                    }else{
                        cl.MaxHealth -= amount;
                    }
                     healthText.text=  cl.MaxHealth.ToString();
                    break;
                case DamageElementalType.Freeze:
                Debug.Log("FREEZE ACTIVE EFFECT");
                  SoundManager.instance.PlaySound(GlobalSetting.instance.freezeClip);
                    //冻结,无法行动,后期失去效果
                    if(UnityEngine.Random.Range(0.0f,1.0f)>(amount/2)){
                    cl.IsFreeze = true;
                    GetDamageByDET(cl,type,amount);
                    CreateBuffIcon(type,roundTime,amount);
                    GlobalSetting.instance.SETLogs(string.Format("冰冻状态,1回合内无法行动"));
                    }else{
                        Debug.Log("No active ICE");
                    }
                      break;
                case DamageElementalType.Rage:
                    //暴怒,伤害+,血量降至1
                    cl.Rage = true;
                    CreateBuffIcon(type,roundTime,amount);
                    cl.MaxHealth -= cl.MaxHealth + 1;
                    cl.CreatureAtk += 3;
                    //
                     GlobalSetting.instance.SETLogs(string.Format("暴怒效果,血量降至为1,伤害提升3"));
                    break;
                case DamageElementalType.CanAtk:
                    //击晕 无法行动k
                    cl.AttacksForThisTurn = 0;
                    CreateBuffIcon(type,roundTime,amount);
                    break;
                case DamageElementalType.MinusAtk:
                    Debug.Log("Minus Atk active");
                    cl.CreatureAtk -= amount;
                    atkText.text =cl.CreatureAtk.ToString();
                    atkText.color= Color.red;
                   
                    break;


                    case DamageElementalType.ExtraSpell:
                    //Check has ExtraSpell
                    TurnManager.instance.WhoseTurn.hasESD=true;
                    //has esd then add relative 
                    TurnManager.instance.WhoseTurn.ExtraSpellDamage+=amount;

                    Debug.Log("Add ESP"+TurnManager.instance.WhoseTurn.ExtraSpellDamage);
                   
                    break;
                case DamageElementalType.DeadView:
                    
                    break;
                
                //ABSORB
                case DamageElementalType.Absorb:
                Debug.Log("ABSORB EFFECT ACTIVE und got amount ist"+amount);
                    SoundManager.instance.PlaySound(GlobalSetting.instance.AbsorbClip);
                    TurnManager.instance.WhoseTurn.MaxHealth += amount;
                    UpdateCharacterStats(SpellBuffType.Health,amount);
                    break;
                case DamageElementalType.AbsorbArmor:
                      Debug.Log("AbsorbArmor EFFECT ACTIVE und got amount ist"+amount);
                    SoundManager.instance.PlaySound(GlobalSetting.instance.AbsorbClip);
                    TurnManager.instance.WhoseTurn.MaxHealth += amount;
                    TurnManager.instance.WhoseTurn.CreatureDef += amount;
                     UpdateCharacterStats(SpellBuffType.Health,amount);
                     UpdateCharacterStats(SpellBuffType.Armor,amount);
                    break;
                   
                case DamageElementalType.AbsorbBurning:

                     Debug.Log("AbsorbBurning EFFECT ACTIVE");
                    SoundManager.instance.PlaySound(GlobalSetting.instance.AbsorbClip);
                    TurnManager.instance.WhoseTurn.MaxHealth += amount;
                    
                     UpdateCharacterStats(SpellBuffType.Health,amount);
                    ElementalBuff(cl,amount,roundTime,DamageElementalType.Fire);
                    break;
                case DamageElementalType.AbsorbHaste:
                    
                      Debug.Log("AbsorbHaste EFFECT ACTIVE");
                    SoundManager.instance.PlaySound(GlobalSetting.instance.AbsorbClip);
                    TurnManager.instance.WhoseTurn.MaxHealth += amount;
                    break;
                case DamageElementalType.AbsorbBloody:

                      Debug.Log("AbsorbBloody EFFECT ACTIVE");
                    SoundManager.instance.PlaySound(GlobalSetting.instance.AbsorbClip);
                    TurnManager.instance.WhoseTurn.MaxHealth += amount;
                     UpdateCharacterStats(SpellBuffType.Health,amount);
                     ElementalBuff(cl,amount,roundTime,DamageElementalType.Bloody);
                    break;
                case DamageElementalType.AbsorbFreeze:

                     Debug.Log("AbsorbFreeze EFFECT ACTIVE");
                    SoundManager.instance.PlaySound(GlobalSetting.instance.AbsorbClip);
                    TurnManager.instance.WhoseTurn.MaxHealth += amount;
                     UpdateCharacterStats(SpellBuffType.Health,amount);
                     ElementalBuff(cl,amount,roundTime,DamageElementalType.Freeze);

                    break;
                case DamageElementalType.AbsorbTreasure:
  Debug.Log("AbsorbTreasure EFFECT ACTIVE");
                    SoundManager.instance.PlaySound(GlobalSetting.instance.AbsorbClip);
                    TurnManager.instance.WhoseTurn.coinNumber += amount;
                    TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.coinText.text = TurnManager.instance.WhoseTurn.coinNumber.ToString();
                    break;
                case DamageElementalType.AbsorbPosion:

                     Debug.Log("ABSORB EFFECT ACTIVE");
                    SoundManager.instance.PlaySound(GlobalSetting.instance.AbsorbClip);
                    TurnManager.instance.WhoseTurn.MaxHealth += amount;
                     UpdateCharacterStats(SpellBuffType.Health,amount);
                     ElementalBuff(cl,amount,roundTime,DamageElementalType.Posion);
                    break;

                
            }
            
        }else{
            hasEffect=false;
        }
            atkText.text = cl.CreatureAtk.ToString();
            healthText.text = cl.MaxHealth.ToString();
            defText.text = cl.CreatureDef.ToString();
    }



    //TODO
    public void UpdateCharacterStats(SpellBuffType type,int amount){
        switch(type){
            case SpellBuffType.Health:
            Debug.Log("Update Stat For Player == > Health :: num ist "+amount);
            TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.healthText.text = TurnManager.instance.WhoseTurn.MaxHealth.ToString();
            DamageEffect.ShowBuffEffect(transform.position,type,amount);
            break;

            case SpellBuffType.CharacterArmor:
                 Debug.Log("Update Stat For Player == > CA :: num ist "+amount);
            TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.defText.text = TurnManager.instance.WhoseTurn.CreatureDef.ToString();
            DamageEffect.ShowBuffEffect(transform.position,type,amount);
            break;

            case SpellBuffType.STR:
             Debug.Log("Update Stat For Player == > STR :: num ist "+amount);
            TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.StrText.text = TurnManager.instance.WhoseTurn.STR.ToString();
            DamageEffect.ShowBuffEffect(transform.position,type,amount);

            break;

             case SpellBuffType.DEX:
              Debug.Log("Update Stat For Player == > DEX :: num ist "+amount);
            TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.DexText.text = TurnManager.instance.WhoseTurn.DEX.ToString();
            DamageEffect.ShowBuffEffect(transform.position,type,amount);

            break;
             case SpellBuffType.INT:
              Debug.Log("Update Stat For Player == > INTE :: num ist "+amount);
            TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.InteText.text = TurnManager.instance.WhoseTurn.INTE.ToString();
            DamageEffect.ShowBuffEffect(transform.position,type,amount);

            break;

            case SpellBuffType.Atk:
             Debug.Log("Update Stat For Player == > ATK :: num ist "+amount);
            TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.atkText.text = TurnManager.instance.WhoseTurn.CreatureAtk.ToString();
            DamageEffect.ShowBuffEffect(transform.position,type,amount);

            break;

            case SpellBuffType.AtkDur:
             Debug.Log("Update Stat For Player == > AD :: num ist "+amount);
             SoundManager.instance.PlaySound(GlobalSetting.instance.weaponClip);
             TurnManager.instance.WhoseTurn.atkDur+=amount;
            TurnManager.instance.WhoseTurn.playerArea.playerPortraitVisual.atkDurText.text = TurnManager.instance.WhoseTurn.atkDur.ToString();
            DamageEffect.ShowBuffEffect(transform.position,type,amount);

            break;

        }
    }

    public void UpdateRound(DamageElementalType det, int round,int amount=0)
    {
        for (int i = 0; i < buffList.Count; i++)
        {
            if (buffList[i].GetComponent<BuffIcon>().det == det)
            {
                buffList[i].GetComponent<BuffIcon>().round += round;
                
                buffList[i].GetComponent<BuffIcon>().amount += amount;
                buffList[i].GetComponent<BuffIcon>().roundTime.text= buffList[i].GetComponent<BuffIcon>().round.ToString() ;
            }
        }
    }

    public void GetDamageByDET(CreatureLogic cl,DamageElementalType types,int amount)
    {
        switch (types)
        {
            case DamageElementalType.Bloody:
                cl.MaxHealth -= amount;
                cl.CreatureAtk += 1;
                healthText.text = cl.MaxHealth.ToString();
                healthText.color=Color.red;
                break;
            case DamageElementalType.Electronic:   
                cl.HasElec = true;
                cl.CreatureAtk -= amount;
                atkText.color=Color.yellow;    
                break;
            case DamageElementalType.Fire:             
                cl.MaxHealth-=amount;
                break;
            case DamageElementalType.Freeze:
                cl.IsFreeze = true;  
               
                break;
            case DamageElementalType.Rage:
                cl.Rage = true;
                cl.MaxHealth -= cl.MaxHealth + 1;
                cl.CreatureAtk += 3;
                
                break;
          
                
        }
    }

    public void UpdateBuff(CreatureLogic cl)
    {
        Debug.Log("ROUND UPDATE BUFF");
        for (int i = 0; i < buffList.Count; i++)
        {
           
            if (buffList[i].GetComponent<BuffIcon>().round > 0)
            {
               
//                GetDamageByDET(cl,buffList[i].GetComponent<BuffIcon>().det,buffList[i].GetComponent<BuffIcon>().amount);
                RoundUpdateBuff(cl, buffList[i].GetComponent<BuffIcon>().det,
                    buffList[i].GetComponent<BuffIcon>().amount);
                if (buffList[i].GetComponent<BuffIcon>().round <= 0)
                {
                     ResetEffect(cl,buffList[i].GetComponent<BuffIcon>().det);
                    Destroy(buffList[i]);
                    
                }
                
                buffList[i].GetComponent<BuffIcon>().round--;
                buffList[i].GetComponent<BuffIcon>().roundTime.text = buffList[i].GetComponent<BuffIcon>().round.ToString();
            }
            else
            {
                Destroy(buffList[i]);
            }
           
        }
    }
    public void ResetEffect(CreatureLogic cl,DamageElementalType det){
        switch(det){
            case DamageElementalType.Electronic:
                Debug.Log("Reset Elec ");
                cl.HasElec=false;
                atkText.text = cl.CreatureAtk.ToString();
            break;
            
            case DamageElementalType.Fire:
                Debug.Log("Reset Elec ");
               cl.HasFire=false;
            break;
            case DamageElementalType.Posion:
                Debug.Log("Reset Posion ");
                cl.HasPosion=false;
               
            break;
            case DamageElementalType.Freeze:
                Debug.Log("Reset Freeze ");
                cl.IsFreeze=false;
                
            break;
           
        }
    }
    //Update state for creatre who got DE round effect , needs Counter remain round for effects until the 
    //effect ends 
    // Almost DE for creature both rounds effect 
    private void RoundUpdateBuff(CreatureLogic cl, DamageElementalType det, int amount)
    {
        switch (det)
        {
            case DamageElementalType.Bloody:
                cl.MaxHealth -= 1;
                cl.CreatureAtk += 1;

                atkText.text = cl.CreatureAtk.ToString();
                break;
            case DamageElementalType.Freeze:
                cl.AttacksForThisTurn=0;
                break;
            case DamageElementalType.Fire:
                cl.MaxHealth-=1;
                break;
            case DamageElementalType.Ice:
                break;
            case DamageElementalType.Rage:
                break;
            case DamageElementalType.Electronic:
                Debug.Log("Elec Update effect");
              
                break;
            case DamageElementalType.Posion:
                Debug.Log("Posion DE Cause Active");
                
                cl.MaxHealth -=  (--amount);
                healthText.text=cl.MaxHealth.ToString();
                break;
            case DamageElementalType.IgnoreDamage:
                break;

        }
        atkText.text = cl.CreatureAtk.ToString();
        healthText.text = cl.MaxHealth.ToString();
        defText.text = cl.CreatureDef.ToString();
    }
}