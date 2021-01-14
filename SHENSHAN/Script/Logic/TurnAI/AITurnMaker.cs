using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using PixelCrushers.DialogueSystem;
using  DG.Tweening;
using Language.Lua;


//AI CONTROL 
//
public class AITurnMaker : TurnMaker
{

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        Debug.Log("Emeny Turn\t\t"+GlobalSetting.instance.topPlayer.name);
      
        new ShowMessageCommand("对方的回合", 2.0f).AddToQueue();
        //发送卡牌
        p.DrawACard(); 
        

      
      //PUT TO QUEUE COMMAND
           Sequence s= DOTween.Sequence();
               s.OnComplete(()=>{  
                  StartCoroutine( MakeAITurn()); 
                   new DelayCommand(0.4f).AddToQueue();
                   Command.CommandExecutionComplete();
                  
               });
       
    }
    //迭代器
    public IEnumerator MakeAITurn()
    {
        bool stratrgyAtkFirst = false;
        
        if (Random.Range(0, 2) == 0) stratrgyAtkFirst = true;
        //
        while (MakeOneAIMove(stratrgyAtkFirst))
        {
            yield return null;
        }
           //延迟调用
                InsertDelay(1f);
                //结束回合
                TurnManager.instance.EndTurn();
    
        //
        yield return new WaitForSeconds(0.4f);
    }
    //AI 动作
    bool MakeOneAIMove(bool atkFirst)
    {
        //first chechk weapon stage
        if (p.CreatureAtk > 0 && p.atkDur > 0 && p.manaLeft >= 2 && p.useWeapon ==false )
        {
            PlayerWeapon();
        }
        
        
        if (Command.CardDrawPending()) return true;
        //当为首次攻击,有几种选择
        //1.根据费用使用卡牌,如果场面无怪使用随从，根据环境使用卡牌
        //2.如果出牌后还有剩余卡牌则使用英雄技能
        //3.处于当前回合时如果场面包含随从,如果有嘲讽随从优先级为1，如果对手包含攻击力高的随从优先攻击(2),否则攻击英雄
        else if (atkFirst)
        {


            return AttackWithCreature() || PlayACardFromHand();

        }
        else
            return PlayACardFromHand() || AttackWithCreature();


    }

    /// <summary>
    /// Uses the spell card.
    /// </summary>
    /// <returns><c>true</c>, if spell card was used, <c>false</c> otherwise.</returns>
    private bool UseSpellCard()
    {
        return true;
    }

    /// <summary>
    /// Atks the with creature who hate.
    /// </summary>
    /// <returns><c>true</c>, if with creature who hate was atked, <c>false</c> otherwise.</returns>
    public bool AtkWithCreatureWhoHate()
    {
        return false;
    }

    //打出手中的卡牌,获取玩家手中的卡牌,根据费用进行操作，包含以下可能
    //1.场面无怪时，打出随从牌，如有剩余费用使用英雄技能(召唤随从或者使用增益性法术)
    //2.当场面有怪时，根据随从属性作出选择,如果自身属性大于地方属性则攻击，否则攻击英雄,如果场面卡牌为背面(TODO)，则攻击英雄
    //3.当场面有多个怪时,优先使用效果卡牌，如果存在群体性效果则优先使用该牌
    //4.如果自身血量小于敌方总攻击时,采取特殊手段
    //5.城堡:当随从卡牌类型为军队或者攻城卡牌， 则优先攻击城堡或者工人
    public bool PlayACardFromHand()
    {
        foreach (CardLogic c in p.hand.CardInHand)
        {
            if (c.CanBePlayer)
            {
               if (c.card.typeOfCards == TypeOfCards.Spell )
                {
                    // code to play a spell from hand
                    // TODO: depending on the targeting options, select a random target.
                    if (c.card.target == TargetOptions.None || c.card.target==TargetOptions.AllCharacter || c.card.target==TargetOptions.AllCreature)
                    {
                        Debug.Log(" AIPlay Spell");
                        p.PlayASpellFromHand(c, null);
                        InsertDelay(0.3f);
                        //Debug.Log("Card: " + c.ca.name + " can be played");
                        return true;
                    }                        
                }
                else
                {
                    while (p.table.creatureOnTable.Count < 5)
                    {
                        Debug.Log("AI Play Creaete");
                        // it is a creature card
                        p.PlayACreatureFromHand(c, 0);
                       InsertDelay(0.3f);
                        return true;
                    }
                       
                }
            }
        }
        return false;
    }


                //使用英雄技能,
                bool UseHeroPower()
                {
                    if (p.manaLeft >= 2 && !p.usedHeroPowerThisTurn)
                    {
                        p.UseHeroPower();
                           p.manaLeft -= 2; 
                           InsertDelay(1.5f);
                           //
                           return true;
                        
                    }
                    return false;
                }

                public bool PlayerWeapon()
                {
                    if (p.manaLeft > 0 && p.useWeapon == false)
                    {
                        p.UseWeapon();
                        return true;
                    }

                    return false;
                }
    //攻击随从
  bool AttackWithCreature(){
        //获取桌面上的随从
        foreach (CreatureLogic cl in p.table.creatureOnTable)
        {
            if (cl.AttacksForThisTurn > 0)
            {
                //当场面存在怪时
                if(p.otherPlayer.table.creatureOnTable.Count>0){
                    int index =Random.Range(0,p.otherPlayer.table.creatureOnTable.Count);
                    //
                    int logic = p.otherPlayer.table.creatureOnTable[index].ID;
                    //根据左面索引
//                    logic.AtkWithCreatureWithID(index);
                   cl.AtkWithCreatureWithID(logic);
                  
                    return true;
                }else{ 
                    //当场面无随从时,攻击英雄
                    cl.GoFace();   
                    InsertDelay(1.5f);
                    return true;
                }
                
            }
        }
        return false;
    }

    //延迟时间
    void InsertDelay(float idy){
        new DelayCommand(idy).AddToQueue();
    }

    
}