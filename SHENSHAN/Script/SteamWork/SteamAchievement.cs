using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using Steamworks;

 
public class SteamAchievement : MonoBehaviour
{
    [System.Serializable]
 public class Achievement_t{
     public Achievement aID;
    public string m_Name;
    public string m_Detail;
    public string m_Items;
    public bool m_Achieved;

        public Achievement_t(Achievement aID, string name, string detail, string items, bool achieved)
        {
            this.aID = aID;
            m_Name = name;
            m_Detail = detail;
            m_Items = items;
            m_Achieved = achieved;
        }
    }


  //
  public enum Achievement:int{
      ACH_WIN_ONEGAME,
      ACH_WIN_TENGAME,
      ACH_GOT_CHAP01,
      ACH_GOT_CHAP02,
      ACH_GOT_CHAP03,
      ACH_GOT_CHAP04,
      ACH_GOT_CHAP05,
      ACH_GOT_CHAP06,
      ACH_GOT_CHAP07,
      ACH_GOT_CHAP08,
      ACH_GOT_CHAP09,

  }

  public Achievement_t [] m_Achievements = new Achievement_t[]{
      new Achievement_t(Achievement.ACH_WIN_ONEGAME,"FirstGame","赢得一局胜利","null",false),
  };
  
  //
  private CGameID m_GameID;

  //
  private bool m_bRequestStats;
  private bool m_BStatsValid;

  //
  private bool m_bStoreStats;

  private float m_flGameFeetTraveld;
  private float m_ulTickCountGameStart;
  private double m_flGameDur;

  //Record Game Stats
  public int m_WinGame;
  public int m_Chap;

//
public Dictionary<string,Achievement_t> aDic = new Dictionary<string, Achievement_t>();

  protected Callback<UserStatsReceived_t> m_UserStatsReceived;
  protected Callback<UserStatsStored_t> m_UserStatsStored;
  protected Callback<UserAchievementStored_t> m_UserAStored;


void Start(){
    foreach(var a in m_Achievements){
        if(aDic.ContainsKey(a.m_Name)){
            aDic.Add(a.m_Name,a);
        }
    }
}
   private void OnEnable() {
       if(!SteamManager.Initialized){return;}

       //
       m_GameID = new CGameID(SteamUtils.GetAppID());

       m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
       m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStored);
       m_UserAStored  =Callback<UserAchievementStored_t>.Create(OnUAS);

       //
       m_bRequestStats=false;
       m_BStatsValid=false;
  }

  private void Update(){
      if(!SteamManager.Initialized){return;}

      if(!m_bRequestStats){
          if(!SteamManager.Initialized){
              m_bRequestStats=true;
              return;
          }
          //
          bool bSuccess = SteamUserStats.RequestCurrentStats();
          //
          m_bRequestStats=bSuccess;
      }
    
    //
    if(!m_BStatsValid)return;

    //Unlock Current Achievement
    foreach(Achievement_t a in m_Achievements){
        if(a.m_Achieved==true)return;

        switch(a.aID){
            case Achievement.ACH_WIN_ONEGAME:
                if(m_WinGame>=1){
                    UnlockAchievement(a);
                }
            break;
        }
    }

    //Store Stats In SDB
    if(m_bStoreStats){
        SteamUserStats.SetStat("NumWins",m_WinGame);
        SteamUserStats.SetStat("NumChap",m_Chap);

        //
        bool success =SteamUserStats.StoreStats();
        m_bStoreStats =success;
    }

  }

  public void OnUserStatsReceived(UserStatsReceived_t t){

  }
  public void OnUserStored(UserStatsStored_t t){

  }
  public void OnUAS(UserAchievementStored_t t){

  }

 

//
  public void UnlockAchievement(Achievement_t a){
      a.m_Achieved=true;
      //
      SteamUserStats.SetAchievement(a.aID.ToString());
      //
      m_bStoreStats=true;
  }

}
