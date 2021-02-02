using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//crystals type
public enum CrystalsType{
    None,
    NormalCrystals,
    BloodCrystals,
    BusinessCrystals
}
/// <summary>
/// 回合水晶的管理,产出由工人效率决定
/// 1.正常情况,工人产出1crystal
/// 2.当使用java,then worker create  2 crystals 
/// 3. the crystals max => 30 ,when the crystals full worker state to rest ,if want to restart worker need the special card to call ,
/// like corecommand,or castle effect effect
/// 4. crystal is the core of the player that can avtive the player motion,
/// 5.crystal have 3 type (normal ,ghost crystal())
/// 
/// </summary>
/// 
[ExecuteInEditMode]
public class ManaPoolVisual : MonoBehaviour
{
    public static ManaPoolVisual instance ;
    
    
//    public int testFullCrystals ;
//    public int testTotalCrystalsThisTurns;

    public Text progressText;
    public Image[] crystals;

    //
    private  int totalCrystals;

    public int TotalCrystals
    {
        get { return totalCrystals; }
        set
        {

            if (value > crystals.Length)
                totalCrystals = crystals.Length;
            else if (value < 0)
                totalCrystals = 0;
            else
                totalCrystals = value;
            for (int i = 0; i < crystals.Length; i++)
            {
                if (i < totalCrystals)
                {

                    crystals[i].color = Color.gray;

                }
                else
                    crystals[i].color = Color.clear;


                progressText.text = string.Format("{0}/{1}", availableCrystals.ToString(), totalCrystals.ToString());
            }
        }
    }




    private int availableCrystals;

    public int AvailableCrystals
    {
        get { return availableCrystals; }
        set
        {
             if (value > totalCrystals)
                availableCrystals = totalCrystals;
            else if (value < 0)
                availableCrystals = 0;
            else
                availableCrystals = value;

            //SET TOTAL TO THE AVALIABLE 
            for (int i = 0; i < totalCrystals; i++)
            {
                if (i < availableCrystals)
                {

                    crystals[i].color = Color.white;
                }
                else
                {
                    crystals[i].color = Color.grey;
                }
            }
           
            progressText.text = string.Format("{0}/{1}", availableCrystals.ToString(),totalCrystals.ToString());
        }
    }

    //active component
   public void Awake(){
            instance =this;

    }

//
//    /// <summary>
//    /// Update this instance.
//    /// </summary>
//    private void Update()
//    {
//        if (Application.isEditor && ! Application.isPlaying)
//        {
//            totalCrystals = testTotalCrystalsThisTurns;
//            availableCrystals = testFullCrystals;
//        }
//    }


}
