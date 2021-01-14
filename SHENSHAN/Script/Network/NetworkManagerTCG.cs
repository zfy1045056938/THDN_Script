using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using PixelCrushers;
using Telepathy;
using UnityEngine.AI;
using UnityEditor;
using System;
using System.Security.Cryptography;
using Mirror.Examples.Pong;
using System.Text.RegularExpressions;
using Steamworks;

public enum NetworkState
{
    Offline,
    HandShake,
    Lobby,
    Online,
    World
}

[RequireComponent(typeof(Database))]
public partial class NetworkManagerTCG : NetworkManager
{

    public NetworkState state = NetworkState.Lobby;

    public NetworkManager networkManager;

    Dictionary<NetworkConnection, string> lobby  =  new Dictionary<NetworkConnection, string>();


    [Header("DB")] public int charaLimit = 3;
    public int charNameLimit = 16;
    public float saveInterval = 0.3f;
    public int accountMaxLength = 16;

    public string loginAccount = "admin";
    public string loginPwd = "admin";
   
    [Scene]
    public string[] subScene;
  

    private SaveLoadCharacter saveLoad;
   

    //ServerList
    [SerializeField]
    public class ServerList
    {
        public string sname;
        public string ip;
    }

    public List<ServerList> serverList = new List<ServerList>()
    {
      new ServerList{sname = "Local",ip = "LocalHost"}
    };

//    public CharactersAvailableMsg charavaliableMsg;
//
//    [Header("Select Save")] public Transform saveSlotPos;
//    public List<SaveSlots> saveSlots;
//    public int selection = -1;
    
    
    void Start()
    {
        saveLoad = gameObject.GetComponent<SaveLoadCharacter>();
        //Kepp Active
        DontDestroyOnLoad(this);
      
    }

    void Update()
    {
        if (ClientScene.localPlayer != null) state = NetworkState.Offline;
    }

    #region Server Module For TCG
    
    /// <summary>
    /// Server Start 
    /// </summary>
    public override void OnStartServer()
    {
        Debug.Log("DB CONNECT ");
     Database.instance.Connect();
     //
     Debug.Log("REGISTER HANDLER");
     //Create Create In Save Slot If Exists 

     try
     {
         NetworkServer.RegisterHandler<LoginMsg>(OnServerLogin);
           NetworkServer.RegisterHandler<CharacterCreateMsg>(OnServerCharacterCreate);
           NetworkServer.RegisterHandler<CharacterDeleteMsg>(OnServerCharacterDelete);
     }
     catch (Exception e)
     {
         Console.WriteLine(e);
         throw;
     }
  
    
     InvokeRepeating(nameof(SavePlayer),saveInterval,saveInterval);
 

     Utils.InvokeMany(typeof(NetworkManagerTCG), this, "OnStartServer_");
    }
    
    //IF CREATE PLAYER TO THE GAME THEN SAVE 
    void SavePlayer()
    {
      Database.instance.CharacterSaveMany(PlayerData.onlinePlayers.Values);
      Debug.Log("Save Success");
    }

//    public override void OnServerConnect(NetworkConnection connet)
//    {
//        string account = lobby[connet];
//        //
//        connet.Send(MakeCharacterAvaMsg(account));
//        //
//        Utils.InvokeMany(typeof(NetworkManagerTCG),this,"OnServerConnect_",connet);
//    }

    private void OnServerCharacterDelete(NetworkConnection con, CharacterDeleteMsg msg)
    {
//      if(lobby.ContainsKey(con)){
//          string account = lobby[con];
//          //
//          List<string> c = Database.instance.CharacterForAccount(account);
//
//          //
//          if(0 < msg.value && msg.value < c.Count){
//              Database.instance.CharacterDel(c[msg.value]);
//              //
//              Utils.InvokeMany(typeof(Database),this,"OnDBDelete_",msg);
//              //
//              con.Send(MakeCharacterAvaMsg(account));
//          }else{
//              ServerSendError(con,"INVALID INDEX",false);
//          }
//      }
//      else
//      {
//          Debug.Log("Something Wrong");
//          ServerSendError(con,"NOT IN LOBBY", true);
//
//      }
    }
    /// <summary>
    /// IF Network has eroor then occur MSG
    /// </summary>
    /// <param name="con"></param>
    /// <param name="error"></param>
    /// <param name="disconnect"></param>
    private void ServerSendError(NetworkConnection con, string error,bool disconnect)
    {
       con.Send(new ErrorMsg{text=error , causesDisconnect=disconnect});
    }

    //accord to database find character if exists 
    private void OnServerCharacterCreate(NetworkConnection conn, CharacterCreateMsg message)
    {
        //loprint("OnServerCharacterCreate " + conn);

        // only while in lobby (aka after handshake and not ingame)
        if (lobby.ContainsKey(conn))
        {
            // allowed character name?
            if (IsAllowCharacterName(message.name))
            {
                // not existant yet?
                string account = lobby[conn];
                //Check Exists
                if (!Database.instance.CharacterExists(message.name))
                {
                    // not too may characters created yet?
                    if (Database.instance.CharacterForAccount(account).Count < charaLimit)
                    {

                    
                          //Generate new  Player In The Game
                          PlayerData p = gameObject.AddComponent<PlayerData>();
                          GameObject pobj =Instantiate(playerPrefab,transform.position,Quaternion.identity)as GameObject;
                          //accept msg name from the nametext
                          p.PlayerName = message.name;
                          p.account=account;
                          

                            //Set Player To Local
                          PlayerPrefs.SetString("PlayerName",p.PlayerName);
                          PlayerPrefs.SetInt("PlayerMoney",p.money);
                          PlayerPrefs.SetInt("PlayerDust",p.dust);
                          //
                          if(PlayerPrefs.HasKey("PlayerName")){
                              Debug.Log("Create Name In Ps is"+PlayerPrefs.GetString("PlayerName"));
                          }
                          
                         // addon system hooks
                            Utils.InvokeMany(typeof(NetworkManagerTCG), this, "OnServerCharacterCreate_", message, p);
                            // save the player
                            Database.instance.CharacterSave(p, false);
                            
                            // send available characters list again, causing
                            // the client to switch to the character
                            // selection scene again
                        //    conn.Send(Makeava(account));
                    } 
                    else
                    {
                        print("character limit reached: " + message.name);
                        ServerSendError(conn, "character limit reached", false);
                    }
                }
                else
                {
                    print("character name already exists: " + message.name);
                    ServerSendError(conn, "name already exists", false);
                }
            }
            else
            {
                print("character name not allowed: " + message.name);
                ServerSendError(conn, "character name not allowed", false);
            }
        }
        else
        {
            print("CharacterCreate: not in lobby");
            ServerSendError(conn, "CharacterCreate: not in lobby", true);
        }
    }
    
    /// <summary>
    /// Implements Client Call ClientScene.AddPlayer()::When Player Start Save und Enter To Game
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="msg"></param>
//     public override void OnServerAddPlayer(NetworkConnection conn, AddPlayerMessage msg)
//     {
//         if (lobby.ContainsKey(conn))
//              {
//                  //Receive Server Msg
//                  if (msg.value != null && msg.value.Length == sizeof(int))
//                  {
//                      try
//                      {
//                          int index = BitConverter.ToInt32(msg.value, 0);
//                          string account = lobby[conn];
//                          List<string> cs = Database.instance.CharacterForAccount(account);
//                          //
//                          if (0 <= index && index < cs.Count)
//                          {
//                              //Generate Player To The Client Scene 
//                              //Receive Account Msg TODO
// //                              Database.instance.LoadCharacter(account,saveSlots[index],false);
//                                               //
//                                               //
//                                              PlayerData pd = GetComponent<PlayerData>();
//                                              NetworkServer.AddPlayerForConnection(conn, playerPrefab);
//                                              //
//                                              Utils.InvokeMany(typeof(NetworkManagerTCG), this, "OnServerAddPlayer_", account);
//                                              //
//                                              lobby.Remove(conn);
//                                          }
//                                      }
//                                      catch (Exception e)
//                                      {
//                                          Console.WriteLine(e);
//                                          throw;
//                                      }
//                                      finally
//                                      {
//                                          ServerSendError(conn,"INVALID CHARACTERS",true);
//                                          Debug.Log("INVALID CHARACTERS"+msg.ToString());
//                                      }
                             
//                                  }
//                              }
//                     }
                    
                    
                    /// <summary>
                    /// Avaliable Character In The Database that client load them.
                    /// </summary>
                    /// <param name="account"></param>
                    /// <returns></returns>
//    private CharactersAvailableMsg MakeCharacterAvaMsg(string account)
//    {
////        List<PlayerData> player = Database.instance.CharacterForAccount(account)
////            .Select(c => Database.instance.LoadCharacter(c,GetPlayerClasses(), true))
////                .Select(go => go.GetComponent<PlayerData>()).ToList();
//        List<PlayerData> pd = new List<PlayerData>();
//        foreach (string cn in Database.instance.CharacterForAccount(account))
//        {
//            GameObject players = Database.instance.LoadCharacter(cn, pd, false);
//            pd.Add(players.GetComponent<PlayerData>());
//        }
//        //
//        CharactersAvailableMsg message = new CharactersAvailableMsg();
//        //Load c Fro character Server msg
//        message.Load(pd);
//        //
//        
//        //
//        pd.ForEach(p => Destroy(p.gameObject));
//        return message;
//    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="msg"></param>
   public void OnServerLogin(NetworkConnection conn, LoginMsg msg)
    {
        if (msg.version == Application.version)
        {
            if (Database.instance.TryLogin(msg.account, msg.password))
            {
                if (!AccountLoggedIn(msg.account))
                {
                    lobby[conn] = msg.account;
                    //Send Server To Check available Characters
//                    conn.Send(MakeCharacterAvaMsg(msg.account));
                    //
                    Utils.InvokeMany(typeof(NetworkManagerTCG),this,"OnServerLogin_",msg);
                    
                }
                else
                {
                    Debug.Log("INVALID LOGGIN"+msg.account);
                    ServerSendError(conn,"INVALID ACCOUNT",true);
                    
                }
            }
            else
            { 
                Debug.Log("INVALID LOGIN "+msg.account);
                ServerSendError(conn,"INVALID LOGIN",true);
            }
        }
        else
        {
            Debug.Log("INVALID ACCOUNT VERSION"+msg.account);
            ServerSendError(conn,"INVALID ACCOUNT",true);
        }
    }

    private bool AccountLoggedIn(string msgAccount)
    {
        return lobby.ContainsValue(msgAccount) || PlayerData.onlinePlayers.Values.Any(p => p.account == msgAccount);
    }

    private bool IsAllowCharacterName(string characterName)
    {
        return characterName.Length <= charNameLimit &&
               Regex.IsMatch(characterName, @"^[a-zA-Z0-9_]+$");
    }


    // public override void OnClientSceneChanged(NetworkConnection conn){
    //     if(ClientScene.localPlayer==null){
    //         OnClientChangeScene("MainBattleScene");
    //         ClientScene.AddPlayer();

    //     }   
    //     }

    public  void OnStopServer(object SavePlayers)
    {
        print("OnStopServer");
        CancelInvoke(nameof(SavePlayers));


        // addon system hooks
        Utils.InvokeMany(typeof(NetworkManagerTCG), this, "OnStopServer_");
    }

  

    //
    public bool IsConnecting()=> NetworkClient.active && !ClientScene.ready;
    
    #endregion

    #region Client Module For TCG Offline

    /// <summary>
    /// 
    /// </summary>
    /// <param name="conn"></param>
    public override void OnClientConnect(NetworkConnection conn)
    {
        
//        NetworkClient.RegisterHandler<CharactersAvailableMsg>(OnClientCharacterCreate);
        NetworkClient.RegisterHandler<ErrorMsg>(OnClientError);
        
        //LoginMS=>For Server version
        LoginMsg msg = new LoginMsg{account = loginAccount,password = loginPwd,version=Application.version};
        try
        {
        conn.Send(msg);
          Debug.Log("Login Msg Sent"+msg.account+"Version is"+msg.version);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        state = NetworkState.HandShake;

        Utils.InvokeMany(typeof(NetworkManagerTCG),this,"CLIENTCONNECT_",conn);
        
        //base.OnClientConnect(conn)
        ClientScene.Ready(conn);

    }
  
  
  
    private void OnClientError(NetworkConnection conn, ErrorMsg msg)
    {
      Debug.Log("ONCLIENTERROR_"+msg.ToString());
      
      //
      if (msg.causesDisconnect)
      {
          conn.Disconnect();
          
              //
              if (NetworkServer.active)
              {
                  StopHost();
              }
      }
    }


//    private void OnClientCharacterCreate(NetworkConnection arg1, CharactersAvailableMsg msg)
//    {
//
//
//        charavaliableMsg = msg;
//        Debug.Log("Client Character Create");
//        //
//        state = NetworkState.Lobby;
//       
//      
//       for (int i = 0; i < msg.characters.Length; ++i)
//       {
//           CharactersAvailableMsg.CharacterPreview character = charavaliableMsg.characters[i];
//
//           // find the prefab for that class
//            PlayerData prefab = GetComponent<PlayerData>();
//           if (prefab != null)
//               LoadPreview(prefab.gameObject, saveSlots[i].transform, i, character);
//           else
//               Debug.LogWarning("Character Selection: no prefab found for class " + character.name);
//       }
//      
//    
////        
//
//        // addon system hooks
//        Utils.InvokeMany(typeof(NetworkManagerTCG), this, "OnClientCharactersAvailable_", charavaliableMsg);
//    }
//
    #endregion
//    
//   void LoadPreview(GameObject prefab, Transform location, int selectionIndex, CharactersAvailableMsg.CharacterPreview character)
//   {
//       GameObject preview = Instantiate(prefab, saveSlots[0].transform);
//       //
//       PlayerData pd = GetComponent<PlayerData>();
//       pd.PlayerName = character.name;
//   }

//   public void ClearPreviews()
//   {
//       selection = -1;
//    
//   }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="conn"></param>
    /// <param name="msg"></param>
//    public void OnClientCharactersAvailableAtSaveSlot(NetworkConnection conn, CharactersAvailableMsg msg)
//    {
//        charavaliableMsg = msg;
//        Debug.Log("msg Length"+charavaliableMsg.characters.Length);
//        //
//        state = NetworkState.Lobby;
//        //
////        ClearPreviews();
//        //
//        for (int i = 0; i < charavaliableMsg.characters.Length; i++)
//        {
//            CharactersAvailableMsg.CharacterPreview c = charavaliableMsg.characters[i];
//            //    
//        }
//    }



}
